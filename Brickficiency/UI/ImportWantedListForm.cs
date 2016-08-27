using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Brickficiency.Classes;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Services;

namespace Brickficiency.UI
{
    public partial class ImportWantedListForm : Form
    {
        private readonly IWantedListService _wantedListService;

        public ImportWantedListForm(IWantedListService wantedListService)
        {
            _wantedListService = wantedListService;
            InitializeComponent();

            WantedListsListBox.CheckOnClick = true;
            WantedListsListBox.SelectedValueChanged += WantedListsListBox_SelectedValueChanged;
        }

        private void WantedListsListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            OkButton.Enabled = WantedListsListBox.CheckedItems.Count > 0;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var worker = new BackgroundWorker();
            worker.DoWork += LoadWantedLists;
            worker.RunWorkerAsync();
        }

        private void LoadWantedLists(object sender, DoWorkEventArgs e)
        {
            var lists = _wantedListService.GetWantedLists().OrderBy(x => x.Name).ToList();

            this.InvokeAction(() =>
            {
                // order on these statements is important
                WantedListsListBox.DataSource = lists;
                WantedListsListBox.DisplayMember = nameof(WantedList.Name);
                WantedListsListBox.ValueMember = nameof(WantedList.WantedListId);

                WantedListsListBox.Enabled = true;
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            WantedListsListBox.DataSource = null;
            base.OnClosing(e);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "Importing...";
            ProgressBar.Value = 0;
            ProgressBar.Visible = true;
            Enabled = false;

            var worker = new BackgroundWorker();
            worker.DoWork += ImportWantedListItems;
            worker.RunWorkerAsync();
        }

        private void ImportWantedListItems(object sender, DoWorkEventArgs e)
        {
            try
            {
                var checkedItems = WantedListsListBox.CheckedItems.OfType<WantedList>().ToList();
                int toComplete = checkedItems.Count + 1;
                int completed = 0;

                var itemsToAdd = new List<WantedListItem>();

                foreach (var wantedList in checkedItems)
                {
                    var items = _wantedListService.GetWantedListItems(wantedList.WantedListId);
                    itemsToAdd.AddRange(items.Where(x => x.Quantity > 0));
                    completed++;

                    this.InvokeAction(() =>
                    {
                        ProgressBar.Value = ProgressBar.Maximum * completed / toComplete;
                    });
                }

                var converted = itemsToAdd.Select(x => new Item
                {
                    id = string.Format("{0}-{1}", x.ItemTypeCode, x.ItemId),
                    extid = string.Format("{0}-{1}-{2}", x.ItemTypeCode, x.ColorId, x.ItemId),
                    type = x.ItemTypeCode,
                    number = x.ItemId,
                    colour = x.ColorId.ToString()
                }).ToList();

                MainWindow.dgv[MainWindow.currenttab].InvokeAction(() =>
                    {
                        foreach (var item in itemsToAdd)
                        {
                            MainWindow.dgv_AddItem(
                                string.Format("{0}-{1}", item.ItemTypeCode, item.ItemId),
                                item.ColorId.ToString(),
                                item.Quantity,
                                item.Condition);
                        }
                    });

                this.InvokeAction(() =>
                {
                    this.Close();
                });
            }
            catch(Exception thrown)
            {
                throw;
            }
            finally
            {
                this.InvokeAction(() =>
                {
                    ProgressBar.Visible = false;
                    StatusLabel.Text = string.Empty;

                    Enabled = true;
                });
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
