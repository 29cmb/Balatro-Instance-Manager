namespace Balatro_Instance_Manager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            instances = new System.Windows.Forms.TableLayoutPanel();
            balatroPath = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            launch = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            balatroSteamPath = new System.Windows.Forms.TextBox();
            lovelyIgnore = new System.Windows.Forms.CheckBox();
            clear = new System.Windows.Forms.Button();
            deselected = new System.Windows.Forms.CheckBox();
            multiInstance = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // instances
            // 
            instances.ColumnCount = 6;
            instances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            instances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            instances.Location = new System.Drawing.Point(12, 242);
            instances.Name = "instances";
            instances.RowCount = 2;
            instances.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            instances.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            instances.Size = new System.Drawing.Size(776, 176);
            instances.TabIndex = 0;
            // 
            // balatroPath
            // 
            balatroPath.Location = new System.Drawing.Point(12, 31);
            balatroPath.Name = "balatroPath";
            balatroPath.Size = new System.Drawing.Size(776, 23);
            balatroPath.TabIndex = 1;
            balatroPath.KeyDown += balatroPath_KeyDown;
            balatroPath.Leave += balatroPath_Unfocused;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(98, 15);
            label1.TabIndex = 2;
            label1.Text = "Balatro Data Path";
            // 
            // launch
            // 
            launch.BackColor = System.Drawing.Color.FromArgb(((int)((byte)128)), ((int)((byte)255)), ((int)((byte)128)));
            launch.Font = new System.Drawing.Font("Inter", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            launch.Location = new System.Drawing.Point(12, 136);
            launch.Name = "launch";
            launch.Size = new System.Drawing.Size(776, 47);
            launch.TabIndex = 3;
            launch.Text = "Launch";
            launch.UseVisualStyleBackColor = false;
            launch.Click += launch_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 57);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 15);
            label2.TabIndex = 5;
            label2.Text = "Balatro Steam Path";
            // 
            // balatroSteamPath
            // 
            balatroSteamPath.Location = new System.Drawing.Point(12, 75);
            balatroSteamPath.Name = "balatroSteamPath";
            balatroSteamPath.Size = new System.Drawing.Size(776, 23);
            balatroSteamPath.TabIndex = 4;
            balatroSteamPath.KeyDown += balatroSteamPath_KeyDown;
            balatroSteamPath.Leave += balatroSteamPath_Unfocused;
            // 
            // lovelyIgnore
            // 
            lovelyIgnore.AutoSize = true;
            lovelyIgnore.Location = new System.Drawing.Point(15, 108);
            lovelyIgnore.Name = "lovelyIgnore";
            lovelyIgnore.Size = new System.Drawing.Size(140, 19);
            lovelyIgnore.TabIndex = 6;
            lovelyIgnore.Text = "Remove .lovelyignore";
            lovelyIgnore.UseVisualStyleBackColor = true;
            // 
            // clear
            // 
            clear.BackColor = System.Drawing.Color.Red;
            clear.Font = new System.Drawing.Font("Inter", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            clear.ForeColor = System.Drawing.Color.Black;
            clear.Location = new System.Drawing.Point(12, 189);
            clear.Name = "clear";
            clear.Size = new System.Drawing.Size(776, 47);
            clear.TabIndex = 7;
            clear.Text = "Clear Mods";
            clear.UseVisualStyleBackColor = false;
            clear.Click += clear_Click;
            // 
            // deselected
            // 
            deselected.AutoSize = true;
            deselected.Checked = true;
            deselected.CheckState = System.Windows.Forms.CheckState.Checked;
            deselected.Location = new System.Drawing.Point(161, 108);
            deselected.Name = "deselected";
            deselected.Size = new System.Drawing.Size(222, 19);
            deselected.TabIndex = 8;
            deselected.Text = "Delete mods from deselected profiles";
            deselected.UseVisualStyleBackColor = true;
            // 
            // multiInstance
            // 
            multiInstance.AutoSize = true;
            multiInstance.Location = new System.Drawing.Point(389, 108);
            multiInstance.Name = "multiInstance";
            multiInstance.Size = new System.Drawing.Size(136, 19);
            multiInstance.TabIndex = 9;
            multiInstance.Text = "Allow multi-instance";
            multiInstance.UseVisualStyleBackColor = true;
            multiInstance.CheckedChanged += multiInstance_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 447);
            Controls.Add(multiInstance);
            Controls.Add(deselected);
            Controls.Add(clear);
            Controls.Add(lovelyIgnore);
            Controls.Add(label2);
            Controls.Add(balatroSteamPath);
            Controls.Add(launch);
            Controls.Add(label1);
            Controls.Add(balatroPath);
            Controls.Add(instances);
            Text = "Balatro Instance Manager";
            Load += Form1_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel instances;
        private TextBox balatroPath;
        private Label label1;
        private Button launch;
        private Label label2;
        private TextBox balatroSteamPath;
        private CheckBox lovelyIgnore;
        private Button clear;
        private CheckBox deselected;
        private System.Windows.Forms.CheckBox multiInstance;
    }
}
