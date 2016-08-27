namespace Brickficiency
{
    partial class About
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.CreditsPanel = new System.Windows.Forms.GroupBox();
            this.CreditsTable = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.RedditLinkLabel = new System.Windows.Forms.LinkLabel();
            this.CreditsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(153, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Brickficiency 2";
            // 
            // versionLabel
            // 
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(275, 14);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(114, 25);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "ver";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CreditsPanel
            // 
            this.CreditsPanel.Controls.Add(this.CreditsTable);
            this.CreditsPanel.Location = new System.Drawing.Point(15, 76);
            this.CreditsPanel.Name = "CreditsPanel";
            this.CreditsPanel.Size = new System.Drawing.Size(374, 119);
            this.CreditsPanel.TabIndex = 7;
            this.CreditsPanel.TabStop = false;
            this.CreditsPanel.Text = "Credits";
            // 
            // CreditsTable
            // 
            this.CreditsTable.AutoScroll = true;
            this.CreditsTable.AutoSize = true;
            this.CreditsTable.ColumnCount = 2;
            this.CreditsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.98913F));
            this.CreditsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.01087F));
            this.CreditsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreditsTable.Location = new System.Drawing.Point(3, 16);
            this.CreditsTable.Name = "CreditsTable";
            this.CreditsTable.RowCount = 2;
            this.CreditsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.CreditsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.CreditsTable.Size = new System.Drawing.Size(368, 100);
            this.CreditsTable.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Visit on Reddit: ";
            // 
            // RedditLinkLabel
            // 
            this.RedditLinkLabel.AutoSize = true;
            this.RedditLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RedditLinkLabel.Location = new System.Drawing.Point(114, 45);
            this.RedditLinkLabel.Name = "RedditLinkLabel";
            this.RedditLinkLabel.Size = new System.Drawing.Size(98, 17);
            this.RedditLinkLabel.TabIndex = 9;
            this.RedditLinkLabel.TabStop = true;
            this.RedditLinkLabel.Text = "/r/brickficiency";
            this.RedditLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RedditLinkLabel_LinkClicked);
            // 
            // About
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(398, 237);
            this.Controls.Add(this.RedditLinkLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CreditsPanel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Shown += new System.EventHandler(this.About_Shown);
            this.CreditsPanel.ResumeLayout(false);
            this.CreditsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.GroupBox CreditsPanel;
        private System.Windows.Forms.TableLayoutPanel CreditsTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel RedditLinkLabel;
    }
}