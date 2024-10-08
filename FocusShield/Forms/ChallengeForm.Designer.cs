namespace FocusShield.Forms
{
    partial class ChallengeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblChallenge = new Label();
            txtAnswer = new TextBox();
            btnSubmit = new Button();
            SuspendLayout();
            // 
            // lblChallenge
            // 
            lblChallenge.AutoSize = true;
            lblChallenge.Location = new Point(83, 71);
            lblChallenge.Name = "lblChallenge";
            lblChallenge.Size = new Size(73, 20);
            lblChallenge.TabIndex = 0;
            lblChallenge.Text = "challenge";
            // 
            // txtAnswer
            // 
            txtAnswer.Location = new Point(83, 107);
            txtAnswer.Name = "txtAnswer";
            txtAnswer.PlaceholderText = "Enter your answer";
            txtAnswer.Size = new Size(181, 27);
            txtAnswer.TabIndex = 1;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(93, 149);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(181, 29);
            btnSubmit.TabIndex = 2;
            btnSubmit.Text = "Submit answer";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // ChallengeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSubmit);
            Controls.Add(txtAnswer);
            Controls.Add(lblChallenge);
            Name = "ChallengeForm";
            Text = "ChallengeForm";
            Load += ChallengeForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblChallenge;
        private TextBox txtAnswer;
        private Button btnSubmit;
    }
}