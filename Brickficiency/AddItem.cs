using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

using Brickficiency.Classes;
using Brickficiency.Helpers;

using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Services;
using WindmillHelix.Brickficiency2.Services.Data;

namespace Brickficiency
{
    public partial class AddItem : Form
    {
        private readonly IItemService _itemService;
        private readonly Action<string> _debouncedFilterTextChanged;
        private readonly IItemTypeService _itemTypeService;
        private readonly IImageService _imageService;

        private readonly HoverZoom _hoverZoomWindow = new HoverZoom();
        private readonly object _hoverLock = new object();
        private string _hoverKey = null;

        private Color _enableColor = new Color();
        private Color _disabledColor = new Color();

        public AddItem(
            IItemService itemService, 
            IItemTypeService itemTypeService,
            IImageService imageService)
        {
            _imageService = imageService;
            _itemTypeService = itemTypeService;
            _itemService = itemService;

            _debouncedFilterTextChanged = Actions.Debounce<string>(x => this.InvokeAction(ChangeItems));
            InitializeComponent();
        }

        private ItemType SelectedItemType
        {
            get
            {
                return ItemTypeComboBox.SelectedItem as ItemType;
            }
        }

        private string SearchText
        {
            get
            {
                return filterBox.Text;
            }
        }

        private int? SelectedCategoryId
        {
            get
            {
                string categoryName = (string)catList.SelectedItem;
                string categoryIdString;
                if (categoryName == "(All Items)")
                {
                    categoryIdString = null;
                }
                else
                {
                    categoryIdString = MainWindow.db_categories.Values.Single(x => x.name == categoryName).id;
                }

                int? categoryId = ParseUtil.TryParseInt(categoryIdString);
                return categoryId;
            }
        }

        private void AddItem_Load(object sender, EventArgs e)
        {
            var itemTypes =
                _itemTypeService.GetItemTypes()
                    .Where(x => x.ItemTypeCode != ItemTypeCodes.UnsortedLot)
                    .OrderBy(x => x.Name)
                    .ToList();

            var partItemType = itemTypes.Single(x => x.ItemTypeCode == ItemTypeCodes.Part);

            ItemTypeComboBox.DataSource = itemTypes;
            ItemTypeComboBox.DisplayMember = nameof(ItemType.Name);
            ItemTypeComboBox.ValueMember = nameof(ItemType.ItemTypeCode);

            Screen screen = Screen.FromPoint(Cursor.Position);
            this.Location = screen.Bounds.Location;
            this.Left = screen.WorkingArea.Right - this.Width;
            this.Top = screen.WorkingArea.Height / 2 - this.Height / 2;

            ItemTypeComboBox.SelectedItem = partItemType;
            ItemTypeComboBox.SelectedValueChanged += typePick_SelectedValueChanged;

            ChangeType();

            foreach (DBColour dbcolour in MainWindow.db_colours.Values)
            {
                Color thiscol;
                if (dbcolour.id == "0")
                {
                    thiscol = ColorTranslator.FromHtml("#FFFFFF");
                }
                else
                {
                    thiscol = ColorTranslator.FromHtml("#" + dbcolour.rgb);
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

            _enableColor = colourGrid.ForeColor;
            _disabledColor = System.Drawing.SystemColors.ControlDark;
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
            if (SelectedItemType?.ItemTypeCode == ItemTypeCodes.Part
                || SelectedItemType?.ItemTypeCode == ItemTypeCodes.Gear)
            {
                colourGrid.Enabled = true;
                colourGrid.ForeColor = _enableColor;
            }
            else
            {
                colourGrid.Enabled = false;
                colourGrid.ForeColor = _disabledColor;
            }

            ResetCategoryList();
        }

        private void ResetCategoryList()
        {
            catList.Items.Clear();

            List<string> items = new List<string>();
            items.Add("(All Items)");

            var itemTypeCode = SelectedItemType?.ItemTypeCode;
            foreach (var categoryIdString in
                MainWindow.db_blitems.Values.Where(i => i.type == itemTypeCode)
                    .Select(x => x.catid)
                    .Distinct())
            {
                items.Add(MainWindow.db_categories[categoryIdString].name);
            }

            catList.Items.AddRange(items.ToArray());
        }

        private void typePick_SelectedValueChanged(object sender, EventArgs e)
        {
            ChangeType();
        }

        private void ChangeItems()
        {
            string selid = string.Empty;
            if (itemList.SelectedItems.Count > 0)
            {
                selid = itemList.SelectedItems[0].SubItems[0].Text;
            }

            var itemTypeCode = SelectedItemType?.ItemTypeCode;
            var categoryId = SelectedCategoryId;

            itemList.Clear();
            itemList.Columns.Add("id", "Item ID");
            itemList.Columns.Add("name", "Description");

            List<ListViewItem> lvis = new List<ListViewItem>();

            var filtered = _itemService.SearchItems(itemTypeCode, categoryId, SearchText);

            foreach (var item in filtered)
            {
                ListViewItem listItem = new ListViewItem();
                listItem.Tag = item;

                listItem.SubItems[0].Text = item.ItemId;
                listItem.SubItems.Add(item.Name);
                lvis.Add(listItem);
            }

            itemList.Items.AddRange(lvis.ToArray());

            if (selid != string.Empty)
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

            itemList.ListViewItemSorter = new AddEditItemListViewItemComparer(1);
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
            _debouncedFilterTextChanged(SearchText);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedItems.Count == 0)
            {
                return;
            }

            var itemTypeCode = SelectedItemType?.ItemTypeCode;
            string number = itemList.SelectedItems[0].SubItems[0].Text;
            string id = itemTypeCode + "-" + number;
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

        private void itemList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ListView listView = (ListView)sender;

            // Check if e.Item is selected and the ListView has a focus.
            if (!listView.Focused && e.Item.Selected)
            {
                Rectangle rowBounds = e.Bounds;
                int leftMargin = e.Item.GetBounds(ItemBoundsPortion.Label).Left;
                Rectangle bounds = new Rectangle(
                                       leftMargin,
                                       rowBounds.Top,
                                       rowBounds.Width - leftMargin,
                                       rowBounds.Height);
                e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds);
            }
            else e.DrawDefault = true;
        }

        private void itemList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            const int TEXT_OFFSET = 1;
            // I don't know why the text is located at 1px to the right. Maybe it's only for me.

            ListView listView = (ListView)sender;

            // Check if e.Item is selected and the ListView has a focus.
            if (!listView.Focused && e.Item.Selected)
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;

                Rectangle bounds = new Rectangle(
                                       rowBounds.Left + leftMargin,
                                       rowBounds.Top,
                                       e.ColumnIndex == 0
                                           ? labelBounds.Width
                                           : (rowBounds.Width - leftMargin - TEXT_OFFSET),
                                       rowBounds.Height);

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

                var format = align | TextFormatFlags.SingleLine | TextFormatFlags.GlyphOverhangPadding
                             | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis;

                TextRenderer.DrawText(
                    e.Graphics,
                    e.SubItem.Text,
                    listView.Font,
                    bounds,
                    SystemColors.HighlightText,
                    format);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void itemList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;

        }

        private void SetHoverKey(string value)
        {
            // todo: introduce locking on this
            _hoverKey = value;
        }

        private Task OnImageAcquired(Task<Image> imageTask, string hoverKey)
        {
            if (_hoverKey != hoverKey)
            {
                // they must have moused over something else before we got the image
                return Task.FromResult(true);
            }

            lock (_hoverLock)
            {
                if (_hoverKey != hoverKey)
                {
                    // they must have moused over something else before we got the image
                    return Task.FromResult(true);
                }

                if (imageTask.IsFaulted || imageTask.Result == null)
                {
                    _hoverZoomWindow.ShowNotFound();
                    return Task.FromResult(true);
                }

                _hoverZoomWindow.ShowImage(imageTask.Result);
                return Task.FromResult(true);
            }
        }

        private void itemList_MouseMove(object sender, MouseEventArgs e)
        {
            itemList.Focus();

            ListViewItem item = itemList.GetItemAt(e.X, e.Y);
            if (item == null)
            {
                SetHoverKey(null);
                return;
            }

            var itemDetails = item.Tag as ItemDetails;
            if (itemDetails == null)
            {
                return;
            }

            _hoverZoomWindow.Location = new Point(Cursor.Position.X + 10, Cursor.Position.Y + 20);
            if (!_hoverZoomWindow.Visible)
            {
                _hoverZoomWindow.Show();
                this.Focus();
            }

            this.TopMost = false;

            var hoverKey = string.Format("{0}_{1}", itemDetails.ItemTypeCode, itemDetails.ItemId);
            if (hoverKey == _hoverKey)
            {
                return;
            }

            SetHoverKey(hoverKey);

            _hoverZoomWindow.ShowLoading();

            _imageService.GetLargeImageAsync(itemDetails.ItemTypeCode, itemDetails.ItemId)
                .ContinueWith(t => OnImageAcquired(t, hoverKey));
        }

        private void itemList_MouseLeave(object sender, EventArgs e)
        {
            SetHoverKey(null);
            _hoverZoomWindow.Hide();
            this.TopMost = true;
        }

        private void itemList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            itemList.ListViewItemSorter = new AddEditItemListViewItemComparer(e.Column);
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
}