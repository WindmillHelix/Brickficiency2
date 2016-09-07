using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brickficiency
{
    public partial class CalcOptions : Form
    {
        #region vars and setup
        int checkbox_workaround = 0;
        Blacklist blacklistWindow = new Blacklist();

        public CalcOptions()
        {
            InitializeComponent();
        }

        private void CalcOptions_Shown(object sender, EventArgs e)
        {
            foreach (string country in MainWindow.db_countries.Keys)
            {
                countryListBox.Items.Add(country);
            }

            if (MainWindow.settings.countries.Count() != 0)
            {
                foreach (string country in MainWindow.settings.countries)
                {
                    if ((country == "All") || (country == "North America") || (country == "Europe") || (country == "Asia"))
                        continue;
                    countryListBox.SetItemCheckState(countryListBox.Items.IndexOf(country), CheckState.Checked);
                }
            }

            if (MainWindow.settings.countries.Contains("North America"))
            {
                naCheck.Checked = true;
            }
            if (MainWindow.settings.countries.Contains("Europe"))
            {
                eurCheck.Checked = true;
            }
            if (MainWindow.settings.countries.Contains("Asia"))
            {
                asiaCheck.Checked = true;
            }

            if (MainWindow.settings.countries.Contains("All"))
            {
                allCheck.Checked = true;
            }

            matchesBox.Value = System.Convert.ToInt32(MainWindow.settings.nummatches);

            minComboBox.Value = MainWindow.settings.minstores;
            maxComboBox.Value = MainWindow.settings.maxstores;

            continueCheck.Checked = MainWindow.settings.cont;
            sortCheck.Checked = MainWindow.settings.sortcolour;

            if (MainWindow.settings.login == true)
            {
                loginCheck.Checked = true;
            }
        }
        #endregion

        public void ShowApproxOptions(Boolean showThem)
        {
            approxLabel.Visible = showThem;
            approxNumericUpDown.Visible = showThem;
        }

        #region Checkbox stuff
        private void loginCheck_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void allCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (allCheck.Checked == true)
            {
                countryListBox.Enabled = false;
                naCheck.Enabled = false;
                eurCheck.Enabled = false;
                asiaCheck.Enabled = false;
            }
            else
            {
                countryListBox.Enabled = true;
                naCheck.Enabled = true;
                eurCheck.Enabled = true;
                asiaCheck.Enabled = true;
            }
        }

        private void naCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_workaround == 0)
            {
                if (naCheck.Checked == true)
                {
                    eurCheck.Checked = false;
                    asiaCheck.Checked = false;
                    foreach (int item in countryListBox.CheckedIndices)
                    {
                        countryListBox.SetItemCheckState(item, CheckState.Unchecked);
                    }
                    naCountries();
                }
                else
                {
                    foreach (int item in countryListBox.CheckedIndices)
                    {
                        countryListBox.SetItemCheckState(item, CheckState.Unchecked);
                    }
                }
            }
        }

        private void eurCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_workaround == 0)
            {
                if (eurCheck.Checked == true)
                {
                    naCheck.Checked = false;
                    asiaCheck.Checked = false;
                    foreach (int item in countryListBox.CheckedIndices)
                    {
                        countryListBox.SetItemCheckState(item, CheckState.Unchecked);
                    }
                    eurCountries();
                }
                else
                {
                    foreach (int item in countryListBox.CheckedIndices)
                    {
                        countryListBox.SetItemCheckState(item, CheckState.Unchecked);
                    }
                }
            }
        }

        private void asiaCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_workaround == 0)
            {
                if (asiaCheck.Checked == true)
                {
                    eurCheck.Checked = false;
                    naCheck.Checked = false;
                    foreach (int item in countryListBox.CheckedIndices)
                    {
                        countryListBox.SetItemCheckState(item, CheckState.Unchecked);
                    }
                    asiaCountries();
                }
                else
                {
                    foreach (int item in countryListBox.CheckedIndices)
                    {
                        countryListBox.SetItemCheckState(item, CheckState.Unchecked);
                    }
                }
            }
        }

        private void countryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkbox_workaround = 1;
            eurCheck.Checked = false;
            naCheck.Checked = false;
            asiaCheck.Checked = false;
            checkbox_workaround = 0;
        }

        private void naCountries()
        {
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Canada"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("USA"), CheckState.Checked);
        }

        private void eurCountries()
        {
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Austria"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Belarus"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Belgium"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Bosnia and Herzegovina"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Bulgaria"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Croatia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Czech Republic"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Denmark"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Estonia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Finland"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("France"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Germany"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Greece"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Hungary"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Ireland"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Italy"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Latvia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Lithuania"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Luxembourg"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Monaco"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Netherlands"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Norway"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Poland"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Portugal"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Romania"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Russia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("San Marino"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Serbia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Slovakia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Slovenia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Spain"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Sweden"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Switzerland"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Turkey"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Ukraine"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("United Kingdom"), CheckState.Checked);
        }

        private void asiaCountries()
        {
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("China"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("India"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Indonesia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Japan"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Macau"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Malaysia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Pakistan"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Philippines"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Russia"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Singapore"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("South Korea"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Syria"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Taiwan"), CheckState.Checked);
            countryListBox.SetItemCheckState(countryListBox.Items.IndexOf("Thailand"), CheckState.Checked);
        }
        #endregion

        #region Button Stuff
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            MainWindow.settings.countries.Clear();
            if (allCheck.Checked == true)
            {
                MainWindow.settings.countries.Add("All");
            }
            else if (naCheck.Checked == true)
            {
                MainWindow.settings.countries.Add("North America");
                foreach (string country in countryListBox.CheckedItems)
                    MainWindow.settings.countries.Add(country);
            }
            else if (eurCheck.Checked == true)
            {
                MainWindow.settings.countries.Add("Europe");
                foreach (string country in countryListBox.CheckedItems)
                    MainWindow.settings.countries.Add(country);
            }
            else if (asiaCheck.Checked == true)
            {
                MainWindow.settings.countries.Add("Asia");
                foreach (string country in countryListBox.CheckedItems)
                    MainWindow.settings.countries.Add(country);
            }
            else
            {
                foreach (string country in countryListBox.CheckedItems)
                    MainWindow.settings.countries.Add(country);
            }

            MainWindow.settings.nummatches = (int)matchesBox.Value;

            MainWindow.settings.minstores = (int)minComboBox.Value;
            MainWindow.settings.maxstores = (int)maxComboBox.Value;
            MainWindow.settings.approxtime = (int)approxNumericUpDown.Value;


            MainWindow.settings.cont = continueCheck.Checked;
            MainWindow.settings.sortcolour = sortCheck.Checked;

            if (loginCheck.Checked == true)
            {
                MainWindow.settings.login = true;
            }
            else
                MainWindow.settings.login = false;

            DialogResult = DialogResult.OK;
        }
        #endregion

        #region minmax stuff
        private void minComboBox_ValueChanged(object sender, EventArgs e)
        {
            if (minComboBox.Value > maxComboBox.Value)
                maxComboBox.Value = minComboBox.Value;
        }

        private void maxComboBox_ValueChanged(object sender, EventArgs e)
        {
            if (minComboBox.Value > maxComboBox.Value)
                minComboBox.Value = maxComboBox.Value;
        }
        #endregion

        #region Keypress stuff
        private void CalcOptions_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
        #endregion

        private void blacklistButton_Click(object sender, EventArgs e)
        {
            DialogResult result = blacklistWindow.ShowDialog();
        }

        #region Mouse Wheel Fix
        private void MouseWheelFix_minCombo(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
            handledArgs.Handled = true;
            if (e.Delta > 0)
            {
                if (minComboBox.Value != minComboBox.Maximum)
                {
                    minComboBox.Value++;
                }
            }
            else
            {
                if (minComboBox.Value != minComboBox.Minimum)
                {
                    minComboBox.Value--;
                }
            }
        }
        private void MouseWheelFix_maxCombo(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
            handledArgs.Handled = true;
            if (e.Delta > 0)
            {
                if (maxComboBox.Value != maxComboBox.Maximum)
                {
                    maxComboBox.Value++;
                }
            }
            else
            {
                if (maxComboBox.Value != maxComboBox.Minimum)
                {
                    maxComboBox.Value--;
                }
            }
        }
        private void MouseWheelFix_matches(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
            handledArgs.Handled = true;
            if (e.Delta > 0)
            {
                if (matchesBox.Value != matchesBox.Maximum)
                {
                    matchesBox.Value++;
                }
            }
            else
            {
                if (matchesBox.Value != matchesBox.Minimum)
                {
                    matchesBox.Value--;
                }
            }
        }
        #endregion

    }
}