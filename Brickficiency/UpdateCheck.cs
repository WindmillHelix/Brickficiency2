using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Brickficiency
{
    public partial class UpdateCheck : Form
    {
        public UpdateCheck()
        {
            InitializeComponent();
        }

        private void UpdateCheck_Shown(object sender, EventArgs e)
        {
            int days = Convert.ToInt32((DateTime.Now - File.GetLastWriteTime(MainWindow.databasezipfilename)).TotalDays);
            label1.Text = "The database is " + days + " days old. Update?";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
