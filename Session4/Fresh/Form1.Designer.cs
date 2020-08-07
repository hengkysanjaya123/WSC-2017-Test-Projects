namespace Fresh
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewResultsSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDetailedResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(106)))), ((int)(((byte)(166)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Size = new System.Drawing.Size(839, 511);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewResultsSummaryToolStripMenuItem,
            this.viewDetailedResultsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(839, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewResultsSummaryToolStripMenuItem
            // 
            this.viewResultsSummaryToolStripMenuItem.Name = "viewResultsSummaryToolStripMenuItem";
            this.viewResultsSummaryToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.viewResultsSummaryToolStripMenuItem.Text = "&View Results Summary";
            this.viewResultsSummaryToolStripMenuItem.Click += new System.EventHandler(this.viewResultsSummaryToolStripMenuItem_Click);
            // 
            // viewDetailedResultsToolStripMenuItem
            // 
            this.viewDetailedResultsToolStripMenuItem.Name = "viewDetailedResultsToolStripMenuItem";
            this.viewDetailedResultsToolStripMenuItem.Size = new System.Drawing.Size(130, 20);
            this.viewDetailedResultsToolStripMenuItem.Text = "&View Detailed Results";
            this.viewDetailedResultsToolStripMenuItem.Click += new System.EventHandler(this.viewDetailedResultsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(839, 487);
            this.panel2.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 511);
            this.Name = "Form1";
            this.Text = "Flight Satisfaction Survey Reports";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewResultsSummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewDetailedResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
    }
}