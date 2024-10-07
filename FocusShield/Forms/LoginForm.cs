using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FocusShield.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredPassword = txtPassword.Text;

            // Temporary hardcoded password; later can be encrypted/stored securely
            string correctPassword = "FocusShield2024";

            if (enteredPassword == correctPassword)
            {
                // Password correct, allow access to settings
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect Password. Please try again.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
            }
        }

    }
}
