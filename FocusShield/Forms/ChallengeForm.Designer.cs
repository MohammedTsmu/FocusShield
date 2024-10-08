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
            label1 = new Label();
            SuspendLayout();
            // 
            // lblChallenge
            // 
            lblChallenge.AutoSize = true;
            lblChallenge.Location = new Point(53, 90);
            lblChallenge.Name = "lblChallenge";
            lblChallenge.Size = new Size(268, 20);
            lblChallenge.TabIndex = 0;
            lblChallenge.Text = "SECURITY CHALLENGE | نظام منع الغش";
            // 
            // txtAnswer
            // 
            txtAnswer.Location = new Point(53, 113);
            txtAnswer.Name = "txtAnswer";
            txtAnswer.PlaceholderText = "Enter your answer";
            txtAnswer.Size = new Size(268, 27);
            txtAnswer.TabIndex = 1;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(53, 146);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(268, 29);
            btnSubmit.TabIndex = 2;
            btnSubmit.Text = "Submit answer";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 35);
            label1.Name = "label1";
            label1.Size = new Size(268, 20);
            label1.TabIndex = 3;
            label1.Text = "SECURITY CHALLENGE | نظام منع الغش";
            // 
            // ChallengeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(374, 224);
            Controls.Add(label1);
            Controls.Add(btnSubmit);
            Controls.Add(txtAnswer);
            Controls.Add(lblChallenge);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ChallengeForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChallengeForm";
            Load += ChallengeForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblChallenge;
        private TextBox txtAnswer;
        private Button btnSubmit;
        private Label label1;
    }
}