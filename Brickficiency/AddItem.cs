using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Brickficiency.Classes;
using WindmillHelix.Brickficiency2.Common;

namespace Brickficiency
{
    public partial class AddItem : Form
    {
        Color enabledcol = new Color();
        Color disabledcol = new Color();
        HoverZoom hoverZoomWindow = new HoverZoom();
        public string filter
        {
            get{
                return filterBox.Text;
            }
        }

        public AddItem()
        {
            InitializeComponent();
        }

        private void AddItem_Load(object sender, EventArgs e)
        {
            if (typePick.Items.Count == 0)
            {
                Screen screen = Screen.FromPoint(Cursor.Position);
                this.Location = screen.Bounds.Location;
                this.Left = screen.WorkingArea.Right - this.Width;
                this.Top = screen.WorkingArea.Height / 2 - this.Height / 2;

                foreach (string type in MainWindow.db_typenames.Values)
                {
                    if (type != "Unsorted Lot")
                    {
                        typePick.Items.Add(type);
                    }
                }

                typePick.SelectedItem = "Part";
                ChangeType();
            }

            foreach (DBColour dbcolour in MainWindow.db_colours.Values)
            {
                Color thiscol;
                if (dbcolour.id == "0")
                {
                    thiscol = (Color)System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }
                else
                {
                    thiscol = (Color)System.Drawing.ColorTranslator.FromHtml("#" + dbcolour.rgb);
                }
                DataGridViewRow dgvrow = new DataGridViewRow();
                DataGridViewTextBoxCell dgvcell1 = new DataGridViewTextBoxCell();
                dgvcell1.Style.ForeColor = thiscol;
                dgvcell1.Style.BackColor = thiscol;
                DataGridViewTextBoxCell dgvcell2 = new DataGridViewTextBoxCell();
                dgvcell2.Value = dbcolour.name;
                dgvrow.Cells.Add(dgvcell1);
                dgvrow.Cells.Add(dgvcell2);
                colourGrid.Rows.Add(dgvrow);
            }
            colourGrid.Rows[0].Cells[1].Selected = true;
            colourGrid.FirstDisplayedScrollingRowIndex = 0;

            enabledcol = colourGrid.ForeColor;
            disabledcol = System.Drawing.SystemColors.ControlDark;
        }

        private void AddItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ChangeType()
        {
            foreach (string t in MainWindow.db_typenames.Keys)
            {
                if ((string)typePick.SelectedItem == MainWindow.db_typenames[t])
                {
                    if (((string)typePick.SelectedItem == "Part") || ((string)typePick.SelectedItem == "Gear"))
                    {
                        colourGrid.Enabled = true;
                        colourGrid.ForeColor = enabledcol;
                    }
                    else
                    {
                        colourGrid.Enabled = false;
                        colourGrid.ForeColor = disabledcol;
                    }
                    catList.Items.Clear();

                    List<string> items = new List<string>();
                    items.Add("(All Items)");

                    foreach (DBBLItem item in MainWindow.db_blitems.Values.Where(i => i.type == t))
                    {
                        if (!items.Contains(MainWindow.db_categories[item.catid].name))
                        {
                            items.Add(MainWindow.db_categories[item.catid].name);
                        }
                    }

                    catList.Items.AddRange(items.ToArray());
                }
            }
        }

        private void typePick_SelectedValueChanged(object sender, EventArgs e)
        {
            ChangeType();
        }

        private void ChangeItems()
        {
            string selid = "";
            if (itemList.SelectedItems.Count > 0)
            {
                selid = itemList.SelectedItems[0].SubItems[0].Text;
            }

            foreach (string t in MainWindow.db_typenames.Keys)
            {
                if ((string)typePick.SelectedItem == MainWindow.db_typenames[t])
                {
                    if ((string)catList.SelectedItem == "(All Items)")
                    {
                        itemList.Clear();
                        ColumnHeader ch = new ColumnHeader();
                        itemList.Columns.Add("id", "Item ID");
                        itemList.Columns.Add("name", "Description");

                        List<ListViewItem> lvis = new List<ListViewItem>();

                        foreach (DBBLItem item in MainWindow.db_blitems.Values.Where(i => i.type == t &&
                            (filter == "" || i.name.ToLower().Contains(filter.ToLower()) || i.number.ToLower().Contains(filter.ToLower()))))
                        {
                            ListViewItem listitems = new ListViewItem();
                            listitems.SubItems[0].Text = item.number;
                            listitems.SubItems.Add(item.name);
                            lvis.Add(listitems);
                        }

                        itemList.Items.AddRange(lvis.ToArray());

                        if (selid != "")
                        {
                            foreach (ListViewItem lvi in itemList.Items)
                            {
                                if (lvi.SubItems[0].Text == selid)
                                {
                                    lvi.Selected = true;
                                    break;
                                }
                            }
                            if ((itemList.Items.Count > 0) && (itemList.SelectedItems.Count == 0))
                            {
                                itemList.Items[0].Selected = true;
                            }
                        }
                        else
                        {
                            if (itemList.Items.Count > 0)
                            {
                                itemList.Items[0].Selected = true;
                            }
                        }

                        itemList.Columns["id"].Width = -1;
                        itemList.Columns["name"].Width = -2;
                    }
                    else
                    {
                        foreach (string cat in MainWindow.db_categories.Keys)
                        {
                            if ((string)catList.SelectedItem == MainWindow.db_categories[cat].name)
                            {
                                itemList.Clear();
                                itemList.Columns.Add("id", "Item ID");
                                itemList.Columns.Add("name", "Description");

                                List<ListViewItem> lvis = new List<ListViewItem>();

                                foreach (DBBLItem item in MainWindow.db_blitems.Values.Where(i => i.catid == cat && i.type == t &&
                                    (filter == "" || i.name.ToLower().Contains(filter.ToLower()) || i.number.ToLower().Contains(filter.ToLower()))))
                                {
                                    ListViewItem listitems = new ListViewItem();
                                    listitems.SubItems[0].Text = item.number;
                                    listitems.SubItems.Add(item.name);
                                    lvis.Add(listitems);
                                }

                                itemList.Items.AddRange(lvis.ToArray());

                                if (selid != "")
                                {
                                    foreach (ListViewItem lvi in itemList.Items)
                                    {
                                        if (lvi.SubItems[0].Text == selid)
                                        {
                                            lvi.Selected = true;
                                            break;
                                        }
                                    }
                                    if ((itemList.Items.Count > 0) && (itemList.SelectedItems.Count == 0))
                                    {
                                        itemList.Items[0].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (itemList.Items.Count > 0)
                                    {
                                        itemList.Items[0].Selected = true;
                                    }
                                }

                                itemList.Columns["id"].Width = -1;
                                itemList.Columns["name"].Width = -2;
                            }
                        }
                    }
                    itemList.ListViewItemSorter = new ListViewItemComparer(1);
                }
            }
        }

        private void catList_SelectedValueChanged(object sender, EventArgs e)
        {
            ChangeItems();
        }

        private void colourGrid_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.ColumnIndex == 0)
            {
                e.Cell.Selected = false;
                colourGrid.Rows[e.Cell.RowIndex].Cells[1].Selected = true;
            }
        }

        private void filterBox_TextChanged(object sender, EventArgs e)
        {
            ChangeItems();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedItems.Count == 0)
            {
                return;
            }

            foreach (string t in MainWindow.db_typenames.Keys)
            {
                if ((string)typePick.SelectedItem == MainWindow.db_typenames[t])
                {
                    string type = t;
                    string number = itemList.SelectedItems[0].SubItems[0].Text;
                    string id = t + "-" + number;
                    string colour = "0";
                    if (colourGrid.Enabled == true)
                    {
                        foreach (DBColour dbcolour in MainWindow.db_colours.Values)
                        {
                            if (dbcolour.name == colourGrid.SelectedCells[0].Value.ToString())
                            {
                                colour = dbcolour.id;
                            }
                        }
                    }

                    if (MainWindow.db_blitems.ContainsKey(id))
                    {
                        MainWindow.dgv_AddItem(id, colour);
                    }
                    else
                    {
                        MessageBox.Show("Oops? " + id);
                    }
                }
            }
        }

        private void itemList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ListView listView = (ListView)sender;

            // Check if e.Item is selected and the ListView has a focus.
            if (!listView.Focused && e.Item.Selected)
            {
                Rectangle rowBounds = e.Bounds;
                int leftMargin = e.Item.GetBounds(ItemBoundsPortion.Label).Left;
                Rectangle bounds = new Rectangle(leftMargin, rowBounds.Top, rowBounds.Width - leftMargin, rowBounds.Height);
                e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds);
            }
            else
                e.DrawDefault = true;
        }

        private void itemList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            const int TEXT_OFFSET = 1;    // I don't know why the text is located at 1px to the right. Maybe it's only for me.

            ListView listView = (ListView)sender;

            // Check if e.Item is selected and the ListView has a focus.
            if (!listView.Focused && e.Item.Selected)
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);
                TextFormatFlags align;
                switch (listView.Columns[e.ColumnIndex].TextAlign)
                {
                    case HorizontalAlignment.Right:
                        align = TextFormatFlags.Right;
                        break;
                    case HorizontalAlignment.Center:
                        align = TextFormatFlags.HorizontalCenter;
                        break;
                    default:
                        align = TextFormatFlags.Left;
                        break;
                }
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listView.Font, bounds, SystemColors.HighlightText,
                    align | TextFormatFlags.SingleLine | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
            }
            else
                e.DrawDefault = true;
        }

        private void itemList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;

        }

        private void itemList_MouseMove(object sender, MouseEventArgs e)
        {
            itemList.Focus();

            ListViewItem item = itemList.GetItemAt(e.X, e.Y);
            if (item == null)
            {
                return;
            }

            ListViewItem.ListViewSubItem subitem = item.GetSubItemAt(e.X, e.Y);
            if (subitem == null)
            {
                return;
            }

            foreach (string t in MainWindow.db_typenames.Keys)
            {
                if ((string)typePick.SelectedItem == MainWindow.db_typenames[t])
                {
                    string filename = MainWindow.GenerateImageFilename(t + "-" + item.SubItems[0].Text);

                    if (!File.Exists(filename))
                    {
                        hoverZoomWindow.BackgroundImage = null;
                        hoverZoomWindow.Width = 100;
                        hoverZoomWindow.Height = 50;
                        hoverZoomWindow.ShowLabel();

                        lock (MainWindow.imgTimerLock)
                        {
                            MainWindow.imageDLList.Insert(0, new ImageDL()
                            {
                                extid = "",
                                file = filename,
                                url = MainWindow.GenerateImageURL(t + "-" + item.SubItems[0].Text),
                                type = "l"
                            });
                        }
                    }
                    else
                    {
                        Bitmap bmp = (Bitmap)Image.FromFile(filename);
                        hoverZoomWindow.BackgroundImage = bmp;
                        hoverZoomWindow.Width = bmp.Width;
                        hoverZoomWindow.Height = bmp.Height;
                        hoverZoomWindow.HideLabel();
                    }
                }
            }

            if (hoverZoomWindow.Visible == false)
            {
                hoverZoomWindow.Show();
            }
            hoverZoomWindow.Location = new System.Drawing.Point(Cursor.Position.X + 10, Cursor.Position.Y + 20);
            this.TopMost = false;
            this.Focus();
        }

        private void itemList_MouseLeave(object sender, EventArgs e)
        {
            hoverZoomWindow.Hide();
            this.TopMost = true;
        }

        private void itemList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            itemList.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        private void catList_MouseEnter(object sender, EventArgs e)
        {
            catList.Focus();
        }

        private void colourGrid_MouseEnter(object sender, EventArgs e)
        {
            colourGrid.Focus();
        }
    }

    class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }
}
