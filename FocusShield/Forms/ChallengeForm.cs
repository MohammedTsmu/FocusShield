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
    public partial class ChallengeForm : Form
    {
        public ChallengeForm()
        {
            InitializeComponent();


        }

        private string GenerateChallenge()
        {
            Random rand = new Random();
            int number1 = rand.Next(1, 20);
            int number2 = rand.Next(1, 20);
            lblChallenge.Text = $"Solve: {number1} + {number2}";
            return (number1 + number2).ToString();
        }

        private string correctAnswer;
        private void ChallengeForm_Load(object sender, EventArgs e)
        {
            correctAnswer = GenerateChallenge();
        }



        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtAnswer.Text == correctAnswer)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect answer. Please try again.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAnswer.Clear();
                correctAnswer = GenerateChallenge(); // Create a new challenge
            }
        }
        
    }
}
