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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            instances = new TableLayoutPanel();
            balatroPath = new TextBox();
            label1 = new Label();
            launch = new Button();
            SuspendLayout();
            // 
            // instances
            // 
            instances.ColumnCount = 6;
            instances.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            instances.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            instances.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            instances.Location = new Point(12, 113);
            instances.Name = "instances";
            instances.RowCount = 2;
            instances.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            instances.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            instances.Size = new Size(776, 176);
            instances.TabIndex = 0;
            // 
            // balatroPath
            // 
            balatroPath.Location = new Point(12, 31);
            balatroPath.Name = "balatroPath";
            balatroPath.Size = new Size(776, 23);
            balatroPath.TabIndex = 1;
            balatroPath.KeyDown += balatroPath_KeyDown;
            balatroPath.Leave += balatroPath_Unfocused;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 13);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 2;
            label1.Text = "Balatro Path";
            // 
            // launch
            // 
            launch.BackColor = Color.FromArgb(128, 255, 128);
            launch.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            launch.Location = new Point(12, 60);
            launch.Name = "launch";
            launch.Size = new Size(776, 47);
            launch.TabIndex = 3;
            launch.Text = "Launch";
            launch.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 314);
            Controls.Add(launch);
            Controls.Add(label1);
            Controls.Add(balatroPath);
            Controls.Add(instances);
            Name = "Form1";
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
    }
}
