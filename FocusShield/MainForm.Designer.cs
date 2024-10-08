namespace FocusShield
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            dgvBlockList = new DataGridView();
            colItemType = new DataGridViewTextBoxColumn();
            colItemName = new DataGridViewTextBoxColumn();
            colTimeLimit = new DataGridViewTextBoxColumn();
            colUsageTime = new DataGridViewTextBoxColumn();
            txtItemName = new TextBox();
            nudTimeLimit = new NumericUpDown();
            btnAddItem = new Button();
            notifyIcon = new NotifyIcon(components);
            contextMenuTray = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            label1 = new Label();
            btnRemoveItem = new Button();
            timerAppMonitor = new System.Windows.Forms.Timer(components);
            timerMonitor = new System.Windows.Forms.Timer(components);
            cmbApplications = new ComboBox();
            btnRefreshApplications = new Button();
            ToggleThemeCheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dgvBlockList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudTimeLimit).BeginInit();
            contextMenuTray.SuspendLayout();
            SuspendLayout();
            // 
            // dgvBlockList
            // 
            dgvBlockList.AccessibleDescription = "to list applications and websites.";
            dgvBlockList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBlockList.Columns.AddRange(new DataGridViewColumn[] { colItemType, colItemName, colTimeLimit, colUsageTime });
            dgvBlockList.Location = new Point(12, 12);
            dgvBlockList.Name = "dgvBlockList";
            dgvBlockList.RowHeadersWidth = 51;
            dgvBlockList.Size = new Size(776, 132);
            dgvBlockList.TabIndex = 0;
            // 
            // colItemType
            // 
            colItemType.HeaderText = "Type";
            colItemType.MinimumWidth = 6;
            colItemType.Name = "colItemType";
            colItemType.Width = 125;
            // 
            // colItemName
            // 
            colItemName.HeaderText = "Name";
            colItemName.MinimumWidth = 6;
            colItemName.Name = "colItemName";
            colItemName.Width = 125;
            // 
            // colTimeLimit
            // 
            colTimeLimit.HeaderText = "Time Limit (mins)";
            colTimeLimit.MinimumWidth = 6;
            colTimeLimit.Name = "colTimeLimit";
            colTimeLimit.Width = 125;
            // 
            // colUsageTime
            // 
            colUsageTime.HeaderText = "Running Time (mins)";
            colUsageTime.MinimumWidth = 6;
            colUsageTime.Name = "colUsageTime";
            colUsageTime.Width = 125;
            // 
            // txtItemName
            // 
            txtItemName.AccessibleDescription = "to add new applications/websites.";
            txtItemName.Location = new Point(12, 150);
            txtItemName.Name = "txtItemName";
            txtItemName.PlaceholderText = "Enter Application or Website Name";
            txtItemName.Size = new Size(300, 27);
            txtItemName.TabIndex = 1;
            // 
            // nudTimeLimit
            // 
            nudTimeLimit.AccessibleDescription = "to specify time limits for each item.";
            nudTimeLimit.Location = new Point(12, 443);
            nudTimeLimit.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            nudTimeLimit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudTimeLimit.Name = "nudTimeLimit";
            nudTimeLimit.Size = new Size(620, 27);
            nudTimeLimit.TabIndex = 2;
            nudTimeLimit.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // btnAddItem
            // 
            btnAddItem.AccessibleDescription = "for adding/removing items.";
            btnAddItem.Location = new Point(12, 476);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(300, 29);
            btnAddItem.TabIndex = 3;
            btnAddItem.Text = "Add to Block List";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuTray;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "FocusShield is running";
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            // 
            // contextMenuTray
            // 
            contextMenuTray.DropShadowEnabled = false;
            contextMenuTray.ImageScalingSize = new Size(20, 20);
            contextMenuTray.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3 });
            contextMenuTray.Name = "contextMenuTray";
            contextMenuTray.Size = new Size(115, 76);
            contextMenuTray.Text = "Open";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(114, 24);
            toolStripMenuItem1.Text = "Open";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(114, 24);
            toolStripMenuItem2.Text = "Exit";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(114, 24);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 420);
            label1.Name = "label1";
            label1.Size = new Size(438, 20);
            label1.TabIndex = 4;
            label1.Text = "Minimum time 1 minute & Maximum time 1440 minutes (24 hours)";
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.Location = new Point(332, 476);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(300, 29);
            btnRemoveItem.TabIndex = 5;
            btnRemoveItem.Text = "Remove Selected";
            btnRemoveItem.UseVisualStyleBackColor = true;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // timerAppMonitor
            // 
            timerAppMonitor.Interval = 60000;
            timerAppMonitor.Tick += timerAppMonitor_Tick;
            // 
            // timerMonitor
            // 
            timerMonitor.Interval = 30000;
            timerMonitor.Tick += timerMonitor_Tick;
            // 
            // cmbApplications
            // 
            cmbApplications.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbApplications.FormattingEnabled = true;
            cmbApplications.Location = new Point(318, 149);
            cmbApplications.Name = "cmbApplications";
            cmbApplications.Size = new Size(470, 28);
            cmbApplications.TabIndex = 6;
            // 
            // btnRefreshApplications
            // 
            btnRefreshApplications.Location = new Point(318, 183);
            btnRefreshApplications.Name = "btnRefreshApplications";
            btnRefreshApplications.Size = new Size(470, 29);
            btnRefreshApplications.TabIndex = 7;
            btnRefreshApplications.Text = "Refresh Applications";
            btnRefreshApplications.UseVisualStyleBackColor = true;
            btnRefreshApplications.Click += btnRefreshApplications_Click;
            // 
            // ToggleThemeCheckBox
            // 
            ToggleThemeCheckBox.AutoSize = true;
            ToggleThemeCheckBox.Location = new Point(649, 444);
            ToggleThemeCheckBox.Name = "ToggleThemeCheckBox";
            ToggleThemeCheckBox.Size = new Size(111, 24);
            ToggleThemeCheckBox.TabIndex = 8;
            ToggleThemeCheckBox.Text = "Dark Theme";
            ToggleThemeCheckBox.UseVisualStyleBackColor = true;
            ToggleThemeCheckBox.CheckStateChanged += ToggleThemeCheckBox_CheckStateChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 517);
            Controls.Add(ToggleThemeCheckBox);
            Controls.Add(btnRefreshApplications);
            Controls.Add(cmbApplications);
            Controls.Add(btnRemoveItem);
            Controls.Add(label1);
            Controls.Add(btnAddItem);
            Controls.Add(nudTimeLimit);
            Controls.Add(txtItemName);
            Controls.Add(dgvBlockList);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Focus Shield - Manage Your Focus";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBlockList).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudTimeLimit).EndInit();
            contextMenuTray.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvBlockList;
        private TextBox txtItemName;
        private NumericUpDown nudTimeLimit;
        private Button btnAddItem;
        private NotifyIcon notifyIcon;
        private Label label1;
        private Button btnRemoveItem;
        private ContextMenuStrip contextMenuTray;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Timer timerAppMonitor;
        private System.Windows.Forms.Timer timerMonitor;
        private ComboBox cmbApplications;
        private Button btnRefreshApplications;
        private DataGridViewTextBoxColumn colItemType;
        private DataGridViewTextBoxColumn colItemName;
        private DataGridViewTextBoxColumn colTimeLimit;
        private DataGridViewTextBoxColumn colUsageTime;
        private CheckBox ToggleThemeCheckBox;
    }
}