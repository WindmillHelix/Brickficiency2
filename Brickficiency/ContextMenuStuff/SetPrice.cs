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
    public partial class SetPrice : Form
    {
        public decimal num
        {
            get { return (decimal)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public SetPrice()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void MouseWheelFix(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
            handledArgs.Handled = true;
            if (e.Delta > 0)
            {
                if (numericUpDown1.Value != numericUpDown1.Maximum)
                {
                    numericUpDown1.Value = numericUpDown1.Value + numericUpDown1.Increment;
                }
            }
            else
            {
                if (numericUpDown1.Value != numericUpDown1.Minimum)
                {
                    numericUpDown1.Value = numericUpDown1.Value - numericUpDown1.Increment;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SetPrice_Shown(object sender, EventArgs e)
        {
            numericUpDown1.Focus();
            numericUpDown1.Select(0, numericUpDown1.ToString().Length);
        }
    }


}
