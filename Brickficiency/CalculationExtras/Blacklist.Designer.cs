namespace Brickficiency
{
    partial class Blacklist
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
            this.label1 = new System.Windows.Forms.Label();
            this.blackListBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.MaximumSize = new System.Drawing.Size(270, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 80);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter the ID numbers (one per line) of any stores you do not want results " +
    "from. The ID\'s are listed on the report.";
            // 
            // blackListBox
            // 
            this.blackListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blackListBox.Location = new System.Drawing.Point(12, 93);
            this.blackListBox.Multiline = true;
            this.blackListBox.Name = "blackListBox";
            this.blackListBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.blackListBox.Size = new System.Drawing.Size(260, 181);
            this.blackListBox.TabIndex = 1;
            this.blackListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Blacklist_KeyPress);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(178, 281);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(94, 28);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButon
            // 
            this.okButon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButon.Location = new System.Drawing.Point(78, 281);
            this.okButon.Name = "okButon";
            this.okButon.Size = new System.Drawing.Size(94, 28);
            this.okButon.TabIndex = 2;
            this.okButon.Text = "Ok";
            this.okButon.UseVisualStyleBackColor = true;
            this.okButon.Click += new System.EventHandler(this.okButon_Click);
            // 
            // Blacklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 321);
            this.Controls.Add(this.okButon);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.blackListBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Blacklist";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklist";
            this.Shown += new System.EventHandler(this.Blacklist_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Blacklist_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox blackListBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButon;
    }
}