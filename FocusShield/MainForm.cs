using FocusShield.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ////// Get the input from TextBox and NumericUpDown
            //string itemName = txtItemName.Text.Trim();
            //int timeLimit = (int)nudTimeLimit.Value;

            ////// Validate input
            //if (string.IsNullOrEmpty(itemName))
            //{
            //    MessageBox.Show("Please enter an application or website name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            ////// Add the new row to DataGridView
            //dgvBlockList.Rows.Add("App/Website", itemName, timeLimit);

            ////// Clear the TextBox for new input
            //txtItemName.Clear();

            // Get the input from TextBox and NumericUpDown
            string itemName = txtItemName.Text.Trim();
            int timeLimit = (int)nudTimeLimit.Value;

            // Validate input
            if (string.IsNullOrEmpty(itemName))
            {
                MessageBox.Show("Please enter an application or website name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Determine if the input is a website (by checking for a '.' character)
            if (itemName.Contains("."))
            {
                // Block the website
                BlockWebsite(itemName);
                dgvBlockList.Rows.Add("Website", itemName, timeLimit);
            }
            else
            {
                // Add application to the block list
                dgvBlockList.Rows.Add("App", itemName, timeLimit);
            }

            // Clear the TextBox for new input
            txtItemName.Clear();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            //// Check if any row is selected
            //if (dgvBlockList.SelectedRows.Count > 0)
            //{
            //    // Remove the selected row
            //    foreach (DataGridViewRow row in dgvBlockList.SelectedRows)
            //    {
            //        dgvBlockList.Rows.Remove(row);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please select a row to remove.", "Remove Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

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
                    dgvBlockList.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to remove.", "Remove Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //MainForm_FormClosing hides the form and displays the NotifyIcon.
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            // Get the list of running processes
            var runningProcesses = Process.GetProcesses();

            foreach (DataGridViewRow row in dgvBlockList.Rows)
            {
                string itemName = row.Cells["colItemName"].Value.ToString();
                int allowedTime = Convert.ToInt32(row.Cells["colTimeLimit"].Value);

                // Check if the process is running
                foreach (var process in runningProcesses)
                {
                    if (process.ProcessName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Update or initialize the app's usage time in the dictionary
                        if (appTimeUsage.ContainsKey(itemName))
                        {
                            appTimeUsage[itemName]++;
                        }
                        else
                        {
                            appTimeUsage[itemName] = 1;
                        }

                        // Check if the time limit is exceeded
                        if (appTimeUsage[itemName] > allowedTime)
                        {
                            // Kill the process if time limit exceeded
                            try
                            {
                                process.Kill();
                                MessageBox.Show($"{itemName} has been blocked after exceeding the time limit.", "FocusShield", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                appTimeUsage[itemName] = 0; // Reset usage after blocking
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Unable to terminate {itemName}. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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

                // Check if the website is already blocked
                if (lines.Any(line => line.Contains(website)))
                {
                    MessageBox.Show($"{website} is already blocked.", "Website Block", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Append the redirect rule to the hosts file
                using (StreamWriter sw = File.AppendText(hostsPath))
                {
                    sw.WriteLine($"{redirectIP} {website}");
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

            try
            {
                // Read the current contents of the hosts file
                string[] lines = File.ReadAllLines(hostsPath);

                // Filter out the line that contains the website
                var updatedLines = lines.Where(line => !line.Contains(website)).ToArray();

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

    }
}
