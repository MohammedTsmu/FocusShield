using FocusShield.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FocusShield
{
    public partial class MainForm : Form
    {
        private Dictionary<string, int> appTimeUsage = new Dictionary<string, int>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string itemName;
            int timeLimit = (int)nudTimeLimit.Value;

            // Determine if a website or application is being blocked
            if (!string.IsNullOrEmpty(txtItemName.Text.Trim())) // For websites
            {
                itemName = txtItemName.Text.Trim();

                if (itemName.Contains("."))
                {
                    // Block the website
                    BlockWebsite(itemName);
                    dgvBlockList.Rows.Add("Website", itemName, timeLimit);
                }
                else
                {
                    MessageBox.Show("Invalid website address.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (cmbApplications.SelectedIndex >= 0) // For applications
            {
                itemName = cmbApplications.SelectedItem.ToString();
                dgvBlockList.Rows.Add("App", itemName, timeLimit);
            }
            else
            {
                MessageBox.Show("Please enter a website or select an application.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Clear the TextBox and ComboBox for new input
            txtItemName.Clear();
            cmbApplications.SelectedIndex = -1;
        }


        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (!AuthenticateUser())
            {
                MessageBox.Show("Unauthorized access. Changes are not allowed.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChallengeForm challengeForm = new ChallengeForm();
            if (challengeForm.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Challenge not completed. Changes cannot be made.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceed with removing the item if authentication is successful
            if (dgvBlockList.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvBlockList.SelectedRows)
                {
                    if (row.Cells["colItemName"].Value != null)
                    {
                        string itemName = row.Cells["colItemName"].Value.ToString();

                        // Check if the item is a website and unblock it from hosts file
                        if (row.Cells["colItemType"].Value.ToString() == "Website")
                        {
                            UnblockWebsite(itemName);
                        }

                        dgvBlockList.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to remove.", "Remove Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void LoadBlockedWebsites()
        {
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
            string redirectIP = "127.0.0.1";

            try
            {
                // Read the current contents of the hosts file
                string[] lines = File.ReadAllLines(hostsPath);

                foreach (string line in lines)
                {
                    // Check if the line contains a blocked website entry added by FocusShield
                    if (line.StartsWith(redirectIP) && line.Contains("# FocusShield"))
                    {
                        // Extract the website name from the line
                        string website = line.Substring(redirectIP.Length).Split('#')[0].Trim();

                        // Add the blocked website to the DataGridView if it is not already present
                        dgvBlockList.Rows.Add("Website", website, 0); // Assuming a default time of 0 since it's a website
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load blocked websites. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRunningProcesses()
        {
            cmbApplications.Items.Clear(); // Clear existing items

            try
            {
                var runningProcesses = Process.GetProcesses();

                foreach (var process in runningProcesses)
                {
                    if (!string.IsNullOrEmpty(process.ProcessName) && !cmbApplications.Items.Contains(process.ProcessName))
                    {
                        cmbApplications.Items.Add(process.ProcessName);
                    }
                }

                // Sort processes alphabetically
                cmbApplications.Sorted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load running processes. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadInstalledApplications()
        {
            cmbApplications.Items.Clear(); // Clear existing items

            // Fetch installed applications from the registry
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string uninstallKey64 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            try
            {
                // Add installed applications from both registry paths (32-bit and 64-bit)
                using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstallKey))
                {
                    if (key != null)
                    {
                        foreach (string subKeyName in key.GetSubKeyNames())
                        {
                            using (Microsoft.Win32.RegistryKey subKey = key.OpenSubKey(subKeyName))
                            {
                                string appName = subKey?.GetValue("DisplayName") as string;
                                if (!string.IsNullOrEmpty(appName))
                                {
                                    cmbApplications.Items.Add(appName);
                                }
                            }
                        }
                    }
                }

                using (Microsoft.Win32.RegistryKey key64 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstallKey64))
                {
                    if (key64 != null)
                    {
                        foreach (string subKeyName in key64.GetSubKeyNames())
                        {
                            using (Microsoft.Win32.RegistryKey subKey = key64.OpenSubKey(subKeyName))
                            {
                                string appName = subKey?.GetValue("DisplayName") as string;
                                if (!string.IsNullOrEmpty(appName) && !cmbApplications.Items.Contains(appName))
                                {
                                    cmbApplications.Items.Add(appName);
                                }
                            }
                        }
                    }
                }

                // Sort applications alphabetically
                cmbApplications.Sorted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load installed applications. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //MainForm_FormClosing hides the form and displays the NotifyIcon.
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FadeOutForm();

            // Save blocked applications to settings before closing
            SaveBlockedApplicationsToSettings();

            // Prevent the form from closing, minimize to system tray instead
            e.Cancel = true;
            this.Hide();
            notifyIcon.Visible = true;

            // Optional: Show a balloon tip to notify user
            notifyIcon.BalloonTipTitle = "FocusShield";
            notifyIcon.BalloonTipText = "The application is still running in the background.";
            notifyIcon.ShowBalloonTip(3000);
        }



        //notifyIcon_DoubleClick restores the form if the user double-clicks on the NotifyIcon.
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Restore the application from the system tray
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void timerAppMonitor_Tick(object sender, EventArgs e)
        {
            timerAppMonitor.Stop(); // Stop timer to prevent overlap

            try
            {
                // Get the list of running processes
                var runningProcesses = Process.GetProcesses();

                foreach (DataGridViewRow row in dgvBlockList.Rows)
                {
                    // Ensure the row's cell value is not null before proceeding
                    if (row.Cells["colItemName"].Value == null || row.Cells["colTimeLimit"].Value == null)
                    {
                        continue; // Skip this iteration if there is any null value
                    }

                    string itemName = row.Cells["colItemName"].Value.ToString();
                    int allowedTime;

                    // Safely parse the time limit value
                    if (!int.TryParse(row.Cells["colTimeLimit"].Value.ToString(), out allowedTime))
                    {
                        continue; // Skip this iteration if parsing fails
                    }

                    // Check if it is an application
                    if (row.Cells["colItemType"].Value.ToString() == "App")
                    {
                        foreach (var process in runningProcesses)
                        {
                            try
                            {
                                // Match the process name
                                if (process.ProcessName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                                {
                                    // If the application is permanently blocked, kill the process immediately
                                    if (appTimeUsage.ContainsKey(itemName) && appTimeUsage[itemName] == int.MaxValue)
                                    {
                                        process.Kill();
                                        MessageBox.Show($"{itemName} is permanently blocked and has been terminated.", "FocusShield", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        continue; // Skip further checks for permanently blocked apps
                                    }

                                    // Update or initialize the app's usage time in the dictionary
                                    if (appTimeUsage.ContainsKey(itemName))
                                    {
                                        appTimeUsage[itemName]++;
                                    }
                                    else
                                    {
                                        appTimeUsage[itemName] = 1;
                                    }

                                    // Update the DataGridView UI to reflect the updated time
                                    row.Cells["colUsageTime"].Value = appTimeUsage[itemName];

                                    // Check if the time limit is exceeded
                                    if (appTimeUsage[itemName] >= allowedTime)
                                    {
                                        // Kill the process if time limit exceeded
                                        process.Kill();
                                        MessageBox.Show($"{itemName} has been blocked after exceeding the time limit.", "FocusShield", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        // Mark the application as permanently blocked
                                        appTimeUsage[itemName] = int.MaxValue;
                                        row.Cells["colUsageTime"].Value = "Blocked";

                                        // Save the blocked state
                                        SaveBlockedApplicationsToSettings();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions thrown during process interaction
                                MessageBox.Show($"Unable to interact with {itemName}. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while monitoring processes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                timerAppMonitor.Start(); // Restart timer after processing
            }
        }





        private void MainForm_Load(object sender, EventArgs e)
        {
            FadeInForm();
            SetupGlobalExceptionHandling();


            // Load blocked applications into the DataGridView
            LoadBlockedApplicationsFromSettings();

            // Load blocked websites into the DataGridView
            LoadBlockedWebsites();

            // Start the monitoring timer
            timerAppMonitor.Start();
        }



        //Websites Blocking
        private void BlockWebsite(string website)
        {
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
            string redirectIP = "127.0.0.1";

            try
            {
                // Read the current contents of the hosts file
                string[] lines = File.ReadAllLines(hostsPath);

                // Check if the website is already blocked by FocusShield
                if (lines.Any(line => line.Contains(website) && line.Contains("# FocusShield")))
                {
                    MessageBox.Show($"{website} is already blocked by FocusShield.", "Website Block", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Append the redirect rule to the hosts file with a FocusShield identifier
                using (StreamWriter sw = File.AppendText(hostsPath))
                {
                    sw.WriteLine($"{redirectIP} {website} # FocusShield");
                }

                MessageBox.Show($"{website} has been blocked successfully.", "Website Block", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to block {website}. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UnblockWebsite(string website)
        {
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
            string redirectIP = "127.0.0.1";

            try
            {
                // Read the current contents of the hosts file
                string[] lines = File.ReadAllLines(hostsPath);

                // Define the exact pattern we are looking for in the hosts file
                string targetLine = $"{redirectIP} {website} # FocusShield";

                // Filter out the line that exactly matches the blocked website entry added by FocusShield
                var updatedLines = lines.Where(line => !line.Trim().Equals(targetLine, StringComparison.OrdinalIgnoreCase)).ToArray();

                // Write the updated content back to the hosts file
                File.WriteAllLines(hostsPath, updatedLines);

                MessageBox.Show($"{website} has been unblocked successfully.", "Website Unblock", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to unblock {website}. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private bool AuthenticateUser()
        {
            LoginForm loginForm = new LoginForm();
            return loginForm.ShowDialog() == DialogResult.OK;
        }

        //Restart the app if closed
        private void timerMonitor_Tick(object sender, EventArgs e)
        {
            // Get all processes with the name "FocusShield"
            var processes = Process.GetProcessesByName("FocusShield");

            // If more than one instance is running, do nothing
            if (processes.Length > 1)
            {
                return;
            }

            // If only one instance is running, continue monitoring
            if (processes.Length == 0)
            {
                // Restart FocusShield
                Process.Start(Application.ExecutablePath);
            }
        }

        private void btnRefreshApplications_Click(object sender, EventArgs e)
        {
            //LoadInstalledApplications(); // Load the list of installed applications
            LoadRunningProcesses(); // Load the list of running processes

        }


        private void SaveBlockedApplicationsToSettings()
        {
            try
            {
                System.Collections.Specialized.StringCollection blockedItems = new System.Collections.Specialized.StringCollection();

                foreach (DataGridViewRow row in dgvBlockList.Rows)
                {
                    if (row.Cells["colItemName"].Value != null && row.Cells["colItemType"].Value != null && row.Cells["colTimeLimit"].Value != null && row.Cells["colUsageTime"].Value != null)
                    {
                        string itemType = row.Cells["colItemType"].Value.ToString();
                        string itemName = row.Cells["colItemName"].Value.ToString();
                        string timeLimit = row.Cells["colTimeLimit"].Value.ToString();
                        string usageTime = row.Cells["colUsageTime"].Value.ToString() == "Blocked" ? "Blocked" : appTimeUsage.ContainsKey(itemName) ? appTimeUsage[itemName].ToString() : "0";

                        // Format: ItemType,ItemName,TimeLimit,UsageTime
                        blockedItems.Add($"{itemType},{itemName},{timeLimit},{usageTime}");
                    }
                }

                // Save to application settings
                Properties.Settings.Default.BlockedItems = blockedItems;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save blocked applications to settings. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void LoadBlockedApplicationsFromSettings()
        {
            try
            {
                System.Collections.Specialized.StringCollection blockedItems = Properties.Settings.Default.BlockedItems;

                if (blockedItems != null)
                {
                    foreach (string blockedItem in blockedItems)
                    {
                        string[] parts = blockedItem.Split(',');

                        if (parts.Length == 4)
                        {
                            string itemType = parts[0];
                            string itemName = parts[1];
                            int timeLimit;
                            string usageTime = parts[3];

                            if (int.TryParse(parts[2], out timeLimit))
                            {
                                dgvBlockList.Rows.Add(itemType, itemName, timeLimit, usageTime);

                                if (usageTime == "Blocked")
                                {
                                    appTimeUsage[itemName] = int.MaxValue; // Permanently blocked apps
                                }
                                else if (int.TryParse(usageTime, out int parsedUsageTime))
                                {
                                    appTimeUsage[itemName] = parsedUsageTime;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load blocked applications from settings. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles
        //Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles
        //Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles//Styles

        //1. Improve the DataGridView Layout
        private void CustomizeDataGridView()
        {
            // Set column headers with better titles
            dgvBlockList.Columns["colItemType"].HeaderText = "Type";
            dgvBlockList.Columns["colItemName"].HeaderText = "Application/Website";
            dgvBlockList.Columns["colTimeLimit"].HeaderText = "Time Limit (mins)";
            dgvBlockList.Columns["colUsageTime"].HeaderText = "Usage Time";

            // Set column widths
            dgvBlockList.Columns["colItemType"].Width = 100;
            dgvBlockList.Columns["colItemName"].Width = 200;
            dgvBlockList.Columns["colTimeLimit"].Width = 150;
            dgvBlockList.Columns["colUsageTime"].Width = 150;

            // Auto resize rows based on content
            dgvBlockList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Set row height
            dgvBlockList.RowTemplate.Height = 40;

            // Set a clean, modern style
            dgvBlockList.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvBlockList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBlockList.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvBlockList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBlockList.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


        //Add Icons to Buttons
        //private void SetButtonIcons()
        //{
        //    // Load icons from the embedded resources
        //    btnAddItem.Image = Properties.Resources.Add_PNG_32px;  // Example: Add_PNG_32px is the name of the image in resources
        //    btnRemoveItem.Image = Properties.Resources.Delete_PNG_32px;
        //    btnRefreshApplications.Image = Properties.Resources.Refresh_PNG_32px;

        //    // Set icon alignment and size for Add button
        //    btnAddItem.ImageAlign = ContentAlignment.MiddleLeft;
        //    btnAddItem.TextAlign = ContentAlignment.MiddleRight;
        //    btnAddItem.Padding = new Padding(10);
        //    btnAddItem.TextImageRelation = TextImageRelation.ImageBeforeText; // Display image before text

        //    // Set icon alignment and size for Remove button
        //    btnRemoveItem.ImageAlign = ContentAlignment.MiddleLeft;
        //    btnRemoveItem.TextAlign = ContentAlignment.MiddleRight;
        //    btnRemoveItem.Padding = new Padding(10);
        //    btnRemoveItem.TextImageRelation = TextImageRelation.ImageBeforeText;

        //    // Set icon alignment and size for Refresh button
        //    btnRefreshApplications.ImageAlign = ContentAlignment.MiddleLeft;
        //    btnRefreshApplications.TextAlign = ContentAlignment.MiddleRight;
        //    btnRefreshApplications.Padding = new Padding(10);
        //    btnRefreshApplications.TextImageRelation = TextImageRelation.ImageBeforeText;
        //}





        //3. Add ToolTips for Better User Experience
        //ToolTips will give the user hints about what each button or input does.
        private void SetToolTips()
        {

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAddItem, "Add a new application or website to the block list.");
            toolTip.SetToolTip(btnRemoveItem, "Remove the selected application or website from the block list.");
            toolTip.SetToolTip(btnRefreshApplications, "Refresh the list of running applications.");
            toolTip.SetToolTip(cmbApplications, "Select an application to block.");
            toolTip.SetToolTip(txtItemName, "Enter the website URL to block.");
            toolTip.SetToolTip(nudTimeLimit, "Set the time limit (in minutes) for the selected application or website.");
        }
       



        //4. Dark/Light Theme Support
        private void ToggleTheme(bool isDarkTheme)
        {
            if (isDarkTheme)
            {
                this.BackColor = Color.FromArgb(45, 45, 48);
                dgvBlockList.BackgroundColor = Color.FromArgb(28, 28, 28);
                dgvBlockList.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
                dgvBlockList.DefaultCellStyle.ForeColor = Color.White;

                foreach (Control control in this.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = Color.FromArgb(63, 63, 70);
                        btn.ForeColor = Color.White;
                    }
                }
            }
            else
            {
                this.BackColor = Color.White;
                dgvBlockList.BackgroundColor = Color.White;
                dgvBlockList.DefaultCellStyle.BackColor = Color.White;
                dgvBlockList.DefaultCellStyle.ForeColor = Color.Black;

                foreach (Control control in this.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = SystemColors.Control;
                        btn.ForeColor = Color.Black;
                    }
                }
            }
        }


        private void ToggleThemeCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            ToggleTheme(true);
        }


        //5. Handle Unhandled Exceptions Gracefully
        //Call SetupGlobalExceptionHandling() in your MainForm_Load method.

        private void SetupGlobalExceptionHandling()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(GlobalExceptionHandler);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
        }

        private void GlobalExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("An unexpected error occurred: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show("An unhandled error occurred: " + ex.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        //6. User-friendly Notifications
        private void ShowBalloonNotification(string title, string text)
        {
            //Call this function whenever a website/ app is blocked or unblocked.
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = text;
            notifyIcon.ShowBalloonTip(3000);
        }


        //7. Smooth UI Animations
        //Call FadeInForm() when the form loads, and FadeOutForm() when closing or minimizing to the system tray.
        private async void FadeInForm()
        {
            for (double opacity = 0.0; opacity <= 1.0; opacity += 0.05)
            {
                this.Opacity = opacity;
                await Task.Delay(10); // Adjust for smoother/faster animation
            }
        }

        private async void FadeOutForm()
        {
            for (double opacity = 1.0; opacity >= 0.0; opacity -= 0.05)
            {
                this.Opacity = opacity;
                await Task.Delay(10);
            }
            this.Hide();
        }

        






        //Final Implementation
        //Once you implement these suggestions, your app will have a polished look and feel, providing a better user experience.Here’s what you need to do in summary:

        //Customize DataGridView layout for a cleaner look.
        //Add icons to buttons to enhance the interface.
        //Add tooltips to make the app more user-friendly.
        //Implement dark/light theme switching for better usability.
        //Handle unhandled exceptions globally.
        //Add smooth notifications for blocking/unblocking actions.
        //Add simple animations for form transitions.


    }
}