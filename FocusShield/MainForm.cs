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
            // Get the input from TextBox and NumericUpDown
            string itemName = txtItemName.Text.Trim();
            int timeLimit = (int)nudTimeLimit.Value;

            // Validate input
            if (string.IsNullOrEmpty(itemName))
            {
                MessageBox.Show("Please enter an application or website name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add the new row to DataGridView
            dgvBlockList.Rows.Add("App/Website", itemName, timeLimit);

            // Clear the TextBox for new input
            txtItemName.Clear();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            // Check if any row is selected
            if (dgvBlockList.SelectedRows.Count > 0)
            {
                // Remove the selected row
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

    }
}
