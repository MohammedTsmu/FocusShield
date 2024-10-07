using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FocusShield
{
    public partial class MainForm : Form
    {
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

    }
}
