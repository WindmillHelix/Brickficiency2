using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Brickficiency.ContextMenuStuff
{
    public partial class SetComments : Form
    {
        public string text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public SetComments()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SetComments_VisibleChanged(object sender, EventArgs e)
        {
            textBox1.Select();
        }
    }


}
