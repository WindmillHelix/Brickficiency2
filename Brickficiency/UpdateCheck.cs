using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WindmillHelix.Brickficiency2.Services.Data;

namespace Brickficiency
{
    public partial class UpdateCheck : Form
    {
        private readonly IDataUpdateService _dataUpdateService;

        public UpdateCheck(IDataUpdateService dataUpdateService)
        {
            _dataUpdateService = dataUpdateService;
            InitializeComponent();
        }

        private void UpdateCheck_Shown(object sender, EventArgs e)
        {
            var lastDataUpdate = _dataUpdateService.GetLastFullUpdate();
            if (lastDataUpdate.HasValue)
            {
                int days = Convert.ToInt32((DateTime.Now - lastDataUpdate.Value).TotalDays);
                label1.Text = "The catalog is " + days + " days old. Update?";
            }
            else
            {
                label1.Text = "The catalog has never been updated. Update?";
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
