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
using Brickficiency.Providers;

using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Services;
using WindmillHelix.Brickficiency2.Services.Data;

namespace Brickficiency
{
    public partial class AddItem : Form
    {
        private readonly Action<string> _debouncedFilterTextChanged;

        private readonly IItemService _itemService;
        private readonly IItemTypeService _itemTypeService;
        private readonly IImageService _imageService;
        private readonly IColorService _colorService;
        private readonly ICategoryService _categoryService;

        private readonly List<Category> _categories = new List<Category>();
        private readonly List<ItemColor> _colors = new List<ItemColor>();

        private readonly HoverZoom _hoverZoomWindow = new HoverZoom();
        private readonly object _hoverLock = new object();
        private string _hoverKey = null;

        private Color _enabledColor = new Color();
        private Color _disabledColor = new Color();
        private IItemWorkbook _itemWorkbook;

        public AddItem(
            IItemService itemService, 
            IItemTypeService itemTypeService,
            IImageService imageService,
            IColorService colorService,
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _colorService = colorService;
            _imageService = imageService;
            _itemTypeService = itemTypeService;
            _itemService = itemService;

            _debouncedFilterTextChanged = Actions.Debounce<string>(x => this.InvokeAction(ChangeItems));
            InitializeComponent();
        }

        public void Show(IItemWorkbook itemWorkbook)
        {
            _itemWorkbook = itemWorkbook;
            Show();
        }

        #region Facades
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

        private Category SelectedCategory
        {
            get
            {
                var result = catList.SelectedValue as Category;
                return result;
            }
        }

        private ItemColor SelectedColor
        {
            get
            {
                if (!ColorGrid.Enabled)
                {
                    return _colors.Single(x => x.ColorId == 0);
                }

                var colorName = ColorGrid.SelectedCells[0].Value.ToString();
                var result = string.IsNullOrWhiteSpace(colorName)
                                 ? _colors.Single(x => x.ColorId == 0)
                                 : _colors.Single(x => x.Name == colorName);

                return result;
            }
        }
        #endregion

        private void AddItem_Load(object sender, EventArgs e)
        {
            _colors.Clear();
            _colors.AddRange(_colorService.GetColors());

            _categories.Clear();
            _categories.AddRange(_categoryService.GetCategories());

            InitializeItemTypeComboBox();
            InitializeColorGrid();
            OnChangeItemType();

            Screen screen = Screen.FromPoint(Cursor.Position);
            this.Location = screen.Bounds.Location;
            this.Left = screen.WorkingArea.Right - this.Width;
            this.Top = screen.WorkingArea.Height / 2 - this.Height / 2;

            ItemTypeComboBox.SelectedValueChanged += HandleItemTypeComboBoxSelectedValueChanged;

            _enabledColor = ColorGrid.ForeColor;
            _disabledColor = SystemColors.ControlDark;
        }

        private void InitializeItemTypeComboBox()
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

            ItemTypeComboBox.SelectedItem = partItemType;
        }

        private void InitializeColorGrid()
        {
            foreach (var color in _colors)
            {
                Color thisColor;
                if (color.ColorId == 0)
                {
                    thisColor = ColorTranslator.FromHtml("#FFFFFF");
                }
                else
                {
                    thisColor = ColorTranslator.FromHtml("#" + color.Rgb);
                }

                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell swatchCell = new DataGridViewTextBoxCell();
                swatchCell.Style.ForeColor = thisColor;
                swatchCell.Style.BackColor = thisColor;

                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                nameCell.Value = color.Name;

                row.Cells.Add(swatchCell);
                row.Cells.Add(nameCell);

                ColorGrid.Rows.Add(row);
            }

            ColorGrid.Rows[0].Cells[1].Selected = true;
            ColorGrid.FirstDisplayedScrollingRowIndex = 0;
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

        private void OnChangeItemType()
        {
            if (SelectedItemType?.ItemTypeCode == ItemTypeCodes.Part
                || SelectedItemType?.ItemTypeCode == ItemTypeCodes.Gear)
            {
                ColorGrid.Enabled = true;
                ColorGrid.ForeColor = _enabledColor;
            }
            else
            {
                ColorGrid.Enabled = false;
                ColorGrid.ForeColor = _disabledColor;
            }

            ResetCategoryList();
        }

        private void ResetCategoryList()
        {
            var itemTypeCode = SelectedItemType?.ItemTypeCode;
            var categoryIds = _itemService.GetCategoryIdsForItemType(itemTypeCode);

            var query = from id in categoryIds
                        join c in _categories on id equals c.CategoryId
                        orderby c.Name
                        select new KeyValuePair<Category, string>(c, c.Name);

            var categories = query.ToList();
            categories.Insert(0, new KeyValuePair<Category, string>(null, "(All Items)"));

            catList.DataSource = categories;
            catList.ValueMember = nameof(KeyValuePair<Category, string>.Key);
            catList.DisplayMember = nameof(KeyValuePair<Category, string>.Value);
        }

        private void HandleItemTypeComboBoxSelectedValueChanged(object sender, EventArgs e)
        {
            OnChangeItemType();
        }

        private void ChangeItems()
        {
            string selid = string.Empty;
            if (itemList.SelectedItems.Count > 0)
            {
                selid = itemList.SelectedItems[0].SubItems[0].Text;
            }

            var itemTypeCode = SelectedItemType?.ItemTypeCode;
            var categoryId = SelectedCategory?.CategoryId;

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
                ColorGrid.Rows[e.Cell.RowIndex].Cells[1].Selected = true;
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
            string itemId = itemList.SelectedItems[0].SubItems[0].Text;

            _itemWorkbook.AddItem(itemTypeCode, itemId, SelectedColor.ColorId);
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
            ColorGrid.Focus();
        }
    }
}