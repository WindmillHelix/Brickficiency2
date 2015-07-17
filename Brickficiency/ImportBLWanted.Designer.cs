namespace Brickficiency
{
    partial class ImportBLWanted
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
            this.unLabel = new System.Windows.Forms.Label();
            this.unBox = new System.Windows.Forms.TextBox();
            this.pwBox = new System.Windows.Forms.TextBox();
            this.pwLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.wantedListBox = new System.Windows.Forms.ListBox();
            this.wlLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.importWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // unLabel
            // 
            this.unLabel.AutoSize = true;
            this.unLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unLabel.Location = new System.Drawing.Point(12, 14);
            this.unLabel.Name = "unLabel";
            this.unLabel.Size = new System.Drawing.Size(151, 20);
            this.unLabel.TabIndex = 0;
            this.unLabel.Text = "BrickLink Username";
            // 
            // unBox
            // 
            this.unBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unBox.Location = new System.Drawing.Point(182, 12);
            this.unBox.Name = "unBox";
            this.unBox.Size = new System.Drawing.Size(140, 24);
            this.unBox.TabIndex = 1;
            this.unBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box_KeyPress);
            // 
            // pwBox
            // 
            this.pwBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwBox.Location = new System.Drawing.Point(182, 44);
            this.pwBox.Name = "pwBox";
            this.pwBox.PasswordChar = '*';
            this.pwBox.Size = new System.Drawing.Size(140, 24);
            this.pwBox.TabIndex = 2;
            this.pwBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box_KeyPress);
            // 
            // pwLabel
            // 
            this.pwLabel.AutoSize = true;
            this.pwLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwLabel.Location = new System.Drawing.Point(12, 46);
            this.pwLabel.Name = "pwLabel";
            this.pwLabel.Size = new System.Drawing.Size(146, 20);
            this.pwLabel.TabIndex = 0;
            this.pwLabel.Text = "BrickLink Password";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(12, 77);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 20);
            this.statusLabel.TabIndex = 4;
            // 
            // wantedListBox
            // 
            this.wantedListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wantedListBox.FormattingEnabled = true;
            this.wantedListBox.ItemHeight = 18;
            this.wantedListBox.Location = new System.Drawing.Point(182, 120);
            this.wantedListBox.Name = "wantedListBox";
            this.wantedListBox.Size = new System.Drawing.Size(140, 112);
            this.wantedListBox.TabIndex = 3;
            this.wantedListBox.Visible = false;
            this.wantedListBox.DoubleClick += new System.EventHandler(this.wantedListBox_DoubleClick);
            this.wantedListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box_KeyPress);
            // 
            // wlLabel
            // 
            this.wlLabel.AutoSize = true;
            this.wlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wlLabel.Location = new System.Drawing.Point(12, 120);
            this.wlLabel.Name = "wlLabel";
            this.wlLabel.Size = new System.Drawing.Size(143, 20);
            this.wlLabel.TabIndex = 0;
            this.wlLabel.Text = "Select Wanted List";
            this.wlLabel.Visible = false;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(247, 274);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 26);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            this.cancelButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Button_KeyPress);
            // 
            // nextButton
            // 
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.Location = new System.Drawing.Point(166, 274);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 26);
            this.nextButton.TabIndex = 5;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            this.nextButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Button_KeyPress);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.bsx";
            this.saveFileDialog1.Filter = "BrickStore Files (*.bsx)|*.bsx|All files (*.*)|*.*";
            this.saveFileDialog1.Title = "Save BSX File";
            // 
            // importWorker
            // 
            this.importWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.importWorker_DoWork);
            // 
            // ImportBLWanted
            // 
            this.AcceptButton = this.nextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 312);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.wlLabel);
            this.Controls.Add(this.wantedListBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.pwBox);
            this.Controls.Add(this.pwLabel);
            this.Controls.Add(this.unBox);
            this.Controls.Add(this.unLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportBLWanted";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import BrickLink Wanted List";
            this.Shown += new System.EventHandler(this.ImportBLWanted_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label unLabel;
        private System.Windows.Forms.TextBox unBox;
        private System.Windows.Forms.TextBox pwBox;
        private System.Windows.Forms.Label pwLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ListBox wantedListBox;
        private System.Windows.Forms.Label wlLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.ComponentModel.BackgroundWorker importWorker;
    }
}