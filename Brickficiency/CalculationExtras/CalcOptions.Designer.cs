namespace Brickficiency
{
    partial class CalcOptions
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
            this.countryListBox = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.allCheck = new System.Windows.Forms.CheckBox();
            this.naCheck = new System.Windows.Forms.CheckBox();
            this.eurCheck = new System.Windows.Forms.CheckBox();
            this.asiaCheck = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.continueCheck = new System.Windows.Forms.CheckBox();
            this.sortCheck = new System.Windows.Forms.CheckBox();
            this.calculateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.matchesBox = new System.Windows.Forms.NumericUpDown();
            this.loginCheck = new System.Windows.Forms.CheckBox();
            this.unBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.blacklistButton = new System.Windows.Forms.Button();
            this.minComboBox = new System.Windows.Forms.NumericUpDown();
            this.maxComboBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.approxLabel = new System.Windows.Forms.Label();
            this.approxNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.matchesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxComboBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.approxNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // countryListBox
            // 
            this.countryListBox.CheckOnClick = true;
            this.countryListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.Location = new System.Drawing.Point(8, 236);
            this.countryListBox.Margin = new System.Windows.Forms.Padding(4);
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.Size = new System.Drawing.Size(221, 184);
            this.countryListBox.TabIndex = 5;
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.countryListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(103, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "or";
            // 
            // allCheck
            // 
            this.allCheck.AutoSize = true;
            this.allCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.allCheck.Location = new System.Drawing.Point(8, 34);
            this.allCheck.Margin = new System.Windows.Forms.Padding(4);
            this.allCheck.Name = "allCheck";
            this.allCheck.Size = new System.Drawing.Size(204, 29);
            this.allCheck.TabIndex = 1;
            this.allCheck.Text = "Query All Countries";
            this.allCheck.UseVisualStyleBackColor = true;
            this.allCheck.CheckedChanged += new System.EventHandler(this.allCheck_CheckedChanged);
            // 
            // naCheck
            // 
            this.naCheck.AutoSize = true;
            this.naCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.naCheck.Location = new System.Drawing.Point(8, 101);
            this.naCheck.Margin = new System.Windows.Forms.Padding(4);
            this.naCheck.Name = "naCheck";
            this.naCheck.Size = new System.Drawing.Size(158, 29);
            this.naCheck.TabIndex = 2;
            this.naCheck.Text = "North America";
            this.naCheck.UseVisualStyleBackColor = true;
            this.naCheck.CheckedChanged += new System.EventHandler(this.naCheck_CheckedChanged);
            // 
            // eurCheck
            // 
            this.eurCheck.AutoSize = true;
            this.eurCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eurCheck.Location = new System.Drawing.Point(8, 138);
            this.eurCheck.Margin = new System.Windows.Forms.Padding(4);
            this.eurCheck.Name = "eurCheck";
            this.eurCheck.Size = new System.Drawing.Size(97, 29);
            this.eurCheck.TabIndex = 3;
            this.eurCheck.Text = "Europe";
            this.eurCheck.UseVisualStyleBackColor = true;
            this.eurCheck.CheckedChanged += new System.EventHandler(this.eurCheck_CheckedChanged);
            // 
            // asiaCheck
            // 
            this.asiaCheck.AutoSize = true;
            this.asiaCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asiaCheck.Location = new System.Drawing.Point(8, 175);
            this.asiaCheck.Margin = new System.Windows.Forms.Padding(4);
            this.asiaCheck.Name = "asiaCheck";
            this.asiaCheck.Size = new System.Drawing.Size(73, 29);
            this.asiaCheck.TabIndex = 4;
            this.asiaCheck.Text = "Asia";
            this.asiaCheck.UseVisualStyleBackColor = true;
            this.asiaCheck.CheckedChanged += new System.EventHandler(this.asiaCheck_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(369, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(230, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Matches per combination";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(321, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(276, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Minimum combination to query";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(316, 86);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(282, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "Maximum combination to query";
            // 
            // continueCheck
            // 
            this.continueCheck.AutoSize = true;
            this.continueCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.continueCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continueCheck.Location = new System.Drawing.Point(279, 122);
            this.continueCheck.Margin = new System.Windows.Forms.Padding(4);
            this.continueCheck.Name = "continueCheck";
            this.continueCheck.Size = new System.Drawing.Size(381, 54);
            this.continueCheck.TabIndex = 9;
            this.continueCheck.Text = "Continue looking for larger combinations\r\nif a smaller one has been found";
            this.continueCheck.UseVisualStyleBackColor = true;
            // 
            // sortCheck
            // 
            this.sortCheck.AutoSize = true;
            this.sortCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sortCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortCheck.Location = new System.Drawing.Point(359, 183);
            this.sortCheck.Margin = new System.Windows.Forms.Padding(4);
            this.sortCheck.Name = "sortCheck";
            this.sortCheck.Size = new System.Drawing.Size(306, 29);
            this.sortCheck.TabIndex = 10;
            this.sortCheck.Text = "Sort report by colour then name";
            this.sortCheck.UseVisualStyleBackColor = true;
            // 
            // calculateButton
            // 
            this.calculateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculateButton.Location = new System.Drawing.Point(435, 411);
            this.calculateButton.Margin = new System.Windows.Forms.Padding(4);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(125, 34);
            this.calculateButton.TabIndex = 20;
            this.calculateButton.Text = "Calculate";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(568, 411);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(125, 34);
            this.cancelButton.TabIndex = 21;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // matchesBox
            // 
            this.matchesBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matchesBox.Location = new System.Drawing.Point(627, 15);
            this.matchesBox.Margin = new System.Windows.Forms.Padding(4);
            this.matchesBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.matchesBox.Name = "matchesBox";
            this.matchesBox.Size = new System.Drawing.Size(67, 26);
            this.matchesBox.TabIndex = 6;
            this.matchesBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.matchesBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.matchesBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheelFix_matches);
            // 
            // loginCheck
            // 
            this.loginCheck.AutoSize = true;
            this.loginCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.loginCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginCheck.Location = new System.Drawing.Point(321, 220);
            this.loginCheck.Margin = new System.Windows.Forms.Padding(4);
            this.loginCheck.Name = "loginCheck";
            this.loginCheck.Size = new System.Drawing.Size(342, 29);
            this.loginCheck.TabIndex = 15;
            this.loginCheck.Text = "Log in to retrieve Price Guide pages";
            this.loginCheck.UseVisualStyleBackColor = true;
            this.loginCheck.CheckedChanged += new System.EventHandler(this.loginCheck_CheckedChanged);
            // 
            // unBox
            // 
            this.unBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unBox.Location = new System.Drawing.Point(449, 257);
            this.unBox.Margin = new System.Windows.Forms.Padding(4);
            this.unBox.Name = "unBox";
            this.unBox.Size = new System.Drawing.Size(243, 26);
            this.unBox.TabIndex = 16;
            this.unBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(331, 257);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 25);
            this.label7.TabIndex = 17;
            this.label7.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(97, 199);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 29);
            this.label3.TabIndex = 20;
            this.label3.Text = "or";
            // 
            // blacklistButton
            // 
            this.blacklistButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blacklistButton.Location = new System.Drawing.Point(301, 411);
            this.blacklistButton.Margin = new System.Windows.Forms.Padding(4);
            this.blacklistButton.Name = "blacklistButton";
            this.blacklistButton.Size = new System.Drawing.Size(125, 34);
            this.blacklistButton.TabIndex = 19;
            this.blacklistButton.Text = "Blacklist";
            this.blacklistButton.UseVisualStyleBackColor = true;
            this.blacklistButton.Click += new System.EventHandler(this.blacklistButton_Click);
            // 
            // minComboBox
            // 
            this.minComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minComboBox.Location = new System.Drawing.Point(627, 50);
            this.minComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.minComboBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.minComboBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minComboBox.Name = "minComboBox";
            this.minComboBox.Size = new System.Drawing.Size(67, 26);
            this.minComboBox.TabIndex = 7;
            this.minComboBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.minComboBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minComboBox.ValueChanged += new System.EventHandler(this.minComboBox_ValueChanged);
            this.minComboBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheelFix_minCombo);
            // 
            // maxComboBox
            // 
            this.maxComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxComboBox.Location = new System.Drawing.Point(627, 86);
            this.maxComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.maxComboBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.maxComboBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxComboBox.Name = "maxComboBox";
            this.maxComboBox.Size = new System.Drawing.Size(67, 26);
            this.maxComboBox.TabIndex = 8;
            this.maxComboBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.maxComboBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxComboBox.ValueChanged += new System.EventHandler(this.maxComboBox_ValueChanged);
            this.maxComboBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheelFix_maxCombo);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(332, 289);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(218, 34);
            this.label8.TabIndex = 22;
            this.label8.Text = "May be required if you see errors\ndownloading price guide pages.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.allCheck);
            this.groupBox1.Controls.Add(this.countryListBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.naCheck);
            this.groupBox1.Controls.Add(this.eurCheck);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.asiaCheck);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(237, 433);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Countries to Query";
            // 
            // approxLabel
            // 
            this.approxLabel.AutoSize = true;
            this.approxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.approxLabel.Location = new System.Drawing.Point(274, 347);
            this.approxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.approxLabel.Name = "approxLabel";
            this.approxLabel.Size = new System.Drawing.Size(235, 25);
            this.approxLabel.TabIndex = 23;
            this.approxLabel.Text = "Seconds per solution size";
            // 
            // approxNumericUpDown
            // 
            this.approxNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.approxNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.approxNumericUpDown.Location = new System.Drawing.Point(530, 346);
            this.approxNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.approxNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.approxNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.approxNumericUpDown.Name = "approxNumericUpDown";
            this.approxNumericUpDown.Size = new System.Drawing.Size(67, 26);
            this.approxNumericUpDown.TabIndex = 24;
            this.approxNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.approxNumericUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // CalcOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 460);
            this.Controls.Add(this.approxNumericUpDown);
            this.Controls.Add(this.approxLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.maxComboBox);
            this.Controls.Add(this.minComboBox);
            this.Controls.Add(this.blacklistButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.unBox);
            this.Controls.Add(this.loginCheck);
            this.Controls.Add(this.matchesBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.calculateButton);
            this.Controls.Add(this.sortCheck);
            this.Controls.Add(this.continueCheck);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalcOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculation Options";
            this.Shown += new System.EventHandler(this.CalcOptions_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CalcOptions_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.matchesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxComboBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.approxNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox countryListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox allCheck;
        private System.Windows.Forms.CheckBox naCheck;
        private System.Windows.Forms.CheckBox eurCheck;
        private System.Windows.Forms.CheckBox asiaCheck;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox continueCheck;
        private System.Windows.Forms.CheckBox sortCheck;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown matchesBox;
        private System.Windows.Forms.CheckBox loginCheck;
        private System.Windows.Forms.TextBox unBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button blacklistButton;
        private System.Windows.Forms.NumericUpDown minComboBox;
        private System.Windows.Forms.NumericUpDown maxComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label approxLabel;
        private System.Windows.Forms.NumericUpDown approxNumericUpDown;
    }
}