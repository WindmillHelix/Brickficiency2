using Brickficiency.Classes;
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
    public partial class ColourPicker : Form
    {
        private string colour;
        public string num
        {
            get { return colour; }
            set
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    foreach (DBColour dbcolour in MainWindow.db_colours.Values)
                    {
                        if (dbcolour.id != "0")
                        {
                            Color thiscol = (Color)System.Drawing.ColorTranslator.FromHtml("#" + dbcolour.rgb);
                            DataGridViewRow dgvrow = new DataGridViewRow();
                            DataGridViewTextBoxCell dgvcell1 = new DataGridViewTextBoxCell();
                            dgvcell1.Style.ForeColor = thiscol;
                            dgvcell1.Style.BackColor = thiscol;
                            DataGridViewTextBoxCell dgvcell2 = new DataGridViewTextBoxCell();
                            dgvcell2.Value = dbcolour.name;
                            dgvrow.Cells.Add(dgvcell1);
                            dgvrow.Cells.Add(dgvcell2);
                            dataGridView1.Rows.Add(dgvrow);
                        }
                    }
                    dataGridView1.Rows[0].Cells[1].Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString() == MainWindow.db_colours[value].name)
                    {
                        row.Cells[1].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        public ColourPicker()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            setColour();
        }

        private void setColour()
        {
            foreach (DBColour dbcolour in MainWindow.db_colours.Values)
            {
                if (dbcolour.name == dataGridView1.SelectedCells[0].Value.ToString())
                {
                    colour = dbcolour.id;
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void ColourPicker_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                setColour();
            }
        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.ColumnIndex == 0)
            {
                e.Cell.Selected = false;
                dataGridView1.Rows[e.Cell.RowIndex].Cells[1].Selected = true;
            }
        }

        private void ColourPicker_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                setColour();
            }
        }
    }
}
