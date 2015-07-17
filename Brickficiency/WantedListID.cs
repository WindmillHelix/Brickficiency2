using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Brickficiency
{
    public partial class WantedListID : Form
    {
        public string wlid
        {
            get
            {
                return textBox1.Text;
            }
        }

        public WantedListID()
        {
            InitializeComponent();
        }

        private void WantedListID_Shown(object sender, EventArgs e)
        {
            this.Left = System.Windows.Forms.Cursor.Position.X + 10;
            this.Top = System.Windows.Forms.Cursor.Position.Y + 10;

            textBox1.Text = "";
            textBox1.Focus();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
