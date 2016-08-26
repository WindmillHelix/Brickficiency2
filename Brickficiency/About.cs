using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brickficiency
{
    public partial class About : Form
    {
        private class Credit
        {
            public Credit(string role, string name, string url)
            {
                Role = role;
                Name = name;
                Url = url;
            }

            public string Role { get; set; }

            public string Name { get; set; }

            public string Url { get; set; }
        }

        public About()
        {
            InitializeComponent();

            var credits = GetCredits();
            CreditsTable.RowCount = credits.Count;

            for (int i = 0; i < credits.Count; i++)
            {
                if (i > 0)
                {
                    CreditsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                }

                var credit = credits[i];

                var label = new Label() { Text = credit.Role };
                var link = new LinkLabel() { Text = credit.Name, Tag = credit.Url };
                link.Click += Link_Click;

                CreditsTable.Controls.Add(label, 0, i);
                CreditsTable.Controls.Add(link, 1, i);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void Link_Click(object sender, EventArgs e)
        {
            var linkLabel = (LinkLabel)sender;
            OpenLink(linkLabel.Tag.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://www.buildingoutloud.com/go/brickficiency");
        }

        private void About_Shown(object sender, EventArgs e)
        {
            versionLabel.Text = MainWindow.version;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://www.famfamfam.com/lab/icons/silk/");
        }

        private void rebricklinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://www.rebrickable.com");
        }

        private List<Credit> GetCredits()
        {
            var result = new List<Credit>();

            result.Add(new Credit("Original Application", "BuildingOutLoud", "http://www.buildingoutloud.com/go/brickficiency"));
            result.Add(new Credit("Icons", "Mark James", "http://www.famfamfam.com/lab/icons/silk/"));
            result.Add(new Credit("LDD Import Data", "Rebrickable", "http://www.rebrickable.com"));

            return result;
        }

        private void OpenLink(string url)
        {
            var start = new ProcessStartInfo(url);
            Process.Start(start);
        }

        private void RedditLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://www.reddit.com/r/Brickficiency");
        }
    }
}
