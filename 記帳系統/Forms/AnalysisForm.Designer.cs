namespace 記帳系統.Forms
{
    partial class AnalysisForm
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
            this.navBar1 = new 記帳系統.Components.NavBar();
            this.SuspendLayout();
            // 
            // navBar1
            // 
            this.navBar1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navBar1.Location = new System.Drawing.Point(143, 320);
            this.navBar1.Name = "navBar1";
            this.navBar1.Size = new System.Drawing.Size(539, 118);
            this.navBar1.TabIndex = 0;
            // 
            // AnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.navBar1);
            this.Name = "AnalysisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "AnalysisForm";
            this.Text = "圖表分析";
            this.Load += new System.EventHandler(this.AnalysisForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Components.NavBar navBar1;
    }
}