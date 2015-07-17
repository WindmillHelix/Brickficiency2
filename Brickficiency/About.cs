using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brickficiency
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo link = new System.Diagnostics.ProcessStartInfo("http://www.buildingoutloud.com/go/brickficiency");
            System.Diagnostics.Process.Start(link);
        }

        private void About_Shown(object sender, EventArgs e)
        {
            versionLabel.Text = MainWindow.version;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo link = new System.Diagnostics.ProcessStartInfo("http://www.famfamfam.com/lab/icons/silk/");
            System.Diagnostics.Process.Start(link);
        }

        private void rebricklinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo link = new System.Diagnostics.ProcessStartInfo("http://www.rebrickable.com");
            System.Diagnostics.Process.Start(link);
        }
    }
}
