using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Services.Ldd;

namespace Brickficiency.UI
{
    public partial class ImportLddForm : Form
    {
        private readonly ILddFileService _lddFileService;
        private readonly ILddMapperService _lddMapperService;
        private readonly List<Tuple<MappedPart, int>> _mappedParts = new List<Tuple<MappedPart, int>>();

        public ImportLddForm(ILddFileService lddFileService, ILddMapperService lddMapperService)
        {
            _lddFileService = lddFileService;
            _lddMapperService = lddMapperService;

            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var dialogResult = OpenLddFileDialog.ShowDialog();
            if(dialogResult != DialogResult.OK)
            {
                this.Close();
                return;
            }

            var fileInfo = new FileInfo(OpenLddFileDialog.FileName);
            FileNameTextBox.Text = fileInfo.Name;

            var worker = new BackgroundWorker();
            worker.DoWork += DoWork;

            CancelButton.Enabled = true;
            worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            SetProgress(5);

            SetStatusText("Extracting parts list...");
            var items = _lddFileService.ExtractPartsList(OpenLddFileDialog.FileName);
            SetProgress(15);

            SetStatusText("Aggregating parts...");
            var aggregated = items
                .GroupBy(x => x)
                .Select(g => new { Part = g.Key, Count = g.Count() })
                .ToList();
            SetProgress(20);

            SetStatusText("Mapping parts...");
            int processedCount = 0;
            var mappedParts = new List<Tuple<MappedPart, int>>();
            int errorCount = 0;
            foreach (var lddPart in aggregated)
            {
                var mapResult = _lddMapperService.MapItem(lddPart.Part);
                if(mapResult.WasSuccessful)
                {
                    mappedParts.Add(new Tuple<MappedPart, int>(mapResult.Mapped, lddPart.Count));
                }
                else
                {
                    errorCount++;
                    var message = string.Format(
                        "{0} - DesignId: [{1}]; LDD Color: [{2}], Decoration: [{3}], Quantity: [{4}]",
                        mapResult.Message,
                        lddPart.Part.DesignId,
                        lddPart.Part.Materials,
                        lddPart.Part.Decoration,
                        lddPart.Count);
                    AddMessage(message);
                }

                processedCount++;
                SetProgress(20 + (50 * processedCount / aggregated.Count));
            }

            var resultMessage = string.Format(
                "Successfully mapped: {0} unique parts, {1} total parts",
                mappedParts.Count,
                mappedParts.Sum(x => x.Item2));
            AddMessage(resultMessage);

            _mappedParts.Clear();
            _mappedParts.AddRange(mappedParts);

            if (errorCount == 0)
            {
                SetStatusText("Parsing complete.");
            }
            else
            {
                SetStatusText(string.Format("Parsing complete, {0} errors.", errorCount));
            }

            this.InvokeAction(() => ProgressBar.Visible = false);

            SetProgress(100);
            this.InvokeAction(() => OKButton.Enabled = true);
        }

        private void AddMessage(string value)
        {
            this.InvokeAction(() =>
            {
                MessagesTextBox.AppendText(value + "\r\n");
            });
        }

        private void SetStatusText(string value)
        {
            this.InvokeAction(() => StatusLabel.Text = value);
        }

        private void SetProgress(int value)
        {
            this.InvokeAction(() => ProgressBar.Value = value);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            MainWindow.dgv[MainWindow.currenttab].InvokeAction(() =>
            {
                foreach (var item in _mappedParts)
                {
                    MainWindow.dgv_AddItem(
                        string.Format("{0}-{1}", ItemTypeCodes.Part, item.Item1.ItemId),
                        item.Item1.ColorId.ToString(),
                        item.Item2,
                        ItemCondition.Used);
                }
            });
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
