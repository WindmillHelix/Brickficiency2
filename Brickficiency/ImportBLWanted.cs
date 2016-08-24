using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using Brickficiency.Classes;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace Brickficiency
{
    public partial class ImportBLWanted : Form
    {
        // don't really want this referenced here directly, but it will take a bit to decouple this code
        private readonly IBricklinkLoginApi _bricklinkLoginApi;

        #region Define vars
        CookieContainer cookies = new CookieContainer();
        List<string> wantedpage = new List<string>();
        List<string> wanteditemsraw = new List<string>();
        List<Item> wanteditems = new List<Item>();
        List<string> wantedlists = new List<string>();
        string selectedlist = "All";
        public delegate void AdviseParentEventHandler(string text);
        public event AdviseParentEventHandler AdviseParent;
        int wantedImportStep = 0;

        public ImportBLWanted(IBricklinkLoginApi bricklinkLoginApi)
        {
            // don't really want this referenced here directly, but it will take a bit to decouple this code
            _bricklinkLoginApi = bricklinkLoginApi;

            InitializeComponent();
        }
        #endregion

        #region cancel button is clicked, close window
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region next button is clicked
        private void nextButton_Click(object sender, EventArgs e)
        {
            if (wantedImportStep == 0)
            {
                if (unBox.Text == "")
                {
                    unBox.Select();
                    ImportStatus("Please enter a username and password");
                }
                else if (pwBox.Text == "")
                {
                    pwBox.Select();
                    ImportStatus("Please enter a password");
                }
                else
                {
                    if (!importWorker.IsBusy)
                    {
                        ImportStatus("");
                        AddStatus("##Clear##");
                        //GetAndParseWanted();
                        importWorker.RunWorkerAsync();
                    }
                }
            }
            else
            {
                if (MainWindow.settings.username != unBox.Text)
                {
                    MainWindow.settings.username = unBox.Text;
                    MainWindow.WriteSettings();
                }
                ImportWanted();
                DialogResult = DialogResult.OK;
            }
        }
        #endregion

        #region Import Wanted List into the Main Window
        private void ImportWanted()
        {
            selectedlist = "Wanted List: " + wantedListBox.SelectedItem.ToString();
            List<Item> tmpwanted = new List<Item>();
            int lotcount = 0;
            int itemcount = 0;

            foreach (Item item in wanteditems)
            {
                if ((item.remarks == selectedlist) || (selectedlist == "Wanted List: All"))
                {
                    if (MainWindow.db_blitems.ContainsKey(item.id))
                    {
                        tmpwanted.Add(item);
                    }
                    else
                    {
                        AddStatus("Could not import item with ID: " + item.id + Environment.NewLine);
                    }
                }
            }
            foreach (Item item in tmpwanted)
            {
                item.status = "I";
                item.imageurl = "https://www.bricklink.com/getPic.asp?itemType=" + item.type + (item.colour == "0" ? "" : "&colorID=" + item.colour) + "&itemNo=" + item.number;
                item.categoryid = MainWindow.db_blitems[item.id].catid;
                item.type = MainWindow.db_blitems[item.id].type;
                try
                {
                    string test = MainWindow.db_typenames[item.type];

                }
                catch (Exception)
                {
                    MessageBox.Show("-" + item.type + "-");
                } 
                itemcount = itemcount + item.qty;
                lotcount++;
            }

            DataTable thisdt = MainWindow.item2dt(tmpwanted);

            //foreach (DataRow dr in thisdt.Rows)
            //{
            //    AddStatus(dr.Field<string>("extid") + Environment.NewLine);
            //}

            if (MainWindow.dt.Count > 0)
            {
                MainWindow.dt[MainWindow.currenttab].Dispose();
                MainWindow.dt.RemoveAt(MainWindow.currenttab);
            }
            MainWindow.dt.Add(thisdt);
            AddStatus(itemcount + " items in " + lotcount + " lots imported." + Environment.NewLine);
        }
        #endregion

        #region Get and Parse Wanted
        private void GetAndParseWanted()
        {
            wantedpage.Clear();
            wanteditemsraw.Clear();
            wanteditems.Clear();
            StreamWriter swr = new StreamWriter(MainWindow.debugimportfilename);

            ImportStatus("Logging in...");
            AddStatus("Logging In..." + Environment.NewLine);
            swr.Write("Logging in..." + Environment.NewLine);

            string rawpage = GetWantedPage();

            if (rawpage == null)
            {
                ImportStatus("Invalid Username or Password.", Color.Red);
                AddStatus("Invalid Username or Password." + Environment.NewLine);
                swr.Write("Invalid Username or Password." + Environment.NewLine);
                swr.Close();
                return;
            }
            else if (rawpage == "##PageFail##")
            {
                ImportStatus("Failed to retrieve Wanted Page.", Color.Red);
                swr.Write("Failed to retrieve Wanted Page. (Connection error)", Color.Red);
                swr.Close();
                return;
            }
            else
            {
                swr.Write(rawpage + Environment.NewLine + Environment.NewLine);

                wantedpage = rawpage.Split(new char[] { '\n', '\r' }).ToList();
                foreach (string line in wantedpage)
                {
                    swr.Write(line + Environment.NewLine);
                    Match linecheck = Regex.Match(line, @"\<A ID=\'imgLink", RegexOptions.IgnoreCase);
                    if (linecheck.Success)
                    {
                        swr.Write("At least one wanted item found" + Environment.NewLine + Environment.NewLine);
                        wanteditemsraw = line.Split(new string[] { "<A ID=\'imgLink" }, StringSplitOptions.None).ToList();
                        break;
                    }
                    else
                    {
                        swr.Write("No items found on this line" + Environment.NewLine + Environment.NewLine);
                    }
                }

                wanteditemsraw.RemoveAt(0);

                foreach (string itemraw in wanteditemsraw)
                {
                    string number = "";
                    string name = "";
                    string type = "";
                    string colour = "0";
                    string cond = "U";
                    string qty = "0";
                    string price = "0";
                    string list = "";

                    swr.Write(itemraw + Environment.NewLine);

                    Match imagematch = Regex.Match(itemraw, @"catalogItemPic.asp\?" + "(.)=(.*?)\'", RegexOptions.IgnoreCase);
                    if (imagematch.Success)
                    {
                        type = imagematch.Groups[1].Value;
                        number = imagematch.Groups[2].Value;;
                        swr.Write("Type       : " + type + Environment.NewLine +
                                "ID         : " + number + Environment.NewLine);
                    }
                    else
                    {
                        swr.Write("ERROR in ID and Type." + Environment.NewLine + Environment.NewLine);
                        AddStatus("Skipping 1 item in import" + Environment.NewLine);
                        continue;
                    }

                    Match numbernamematch = Regex.Match(itemraw, "Name: (.*?)\"", RegexOptions.IgnoreCase);
                    if (numbernamematch.Success)
                    {
                        name = numbernamematch.Groups[1].Value;
                        swr.Write("Name       : " + name + Environment.NewLine);
                    }
                    else
                    {
                        swr.Write("ERROR in Name." + Environment.NewLine + Environment.NewLine);
                        AddStatus("Skipping 1 item in import" + Environment.NewLine);
                        continue;
                    }

                    string nametmp = name.Replace("(", "");
                    nametmp = nametmp.Replace(")", "");
                    string itemtmp = itemraw.Replace("(", "");
                    itemtmp = itemtmp.Replace(")", "");

                    foreach (DBColour dbcolor in MainWindow.db_colours.Values)
                    {
                        Match colourmatch = Regex.Match(itemtmp, ">" + dbcolor.name + " " + @name, RegexOptions.IgnoreCase);
                        if (colourmatch.Success)
                        {
                            string colourname = dbcolor.name;
                            colour = dbcolor.id;
                            swr.Write("Colour Name: " + colourname + Environment.NewLine + Environment.NewLine);
                            swr.Write("Colour     : " + colour + " (" + MainWindow.db_colours[colour] + ")" + Environment.NewLine);
                        }
                    }

                    if (colour == "0")
                    {
                        swr.Write("No Colour found." + Environment.NewLine + Environment.NewLine);
                    }

                    Match condmatch = Regex.Match(itemraw, "<OPTION VALUE=\"(.)\" SELECTED>", RegexOptions.IgnoreCase);
                    if (condmatch.Success)
                    {
                        cond = condmatch.Groups[1].Value;
                        if (cond == "X")
                        {
                            cond = "U";
                        }
                        swr.Write("Condition  : " + cond + Environment.NewLine);
                    }
                    else
                    {
                        swr.Write("ERROR in condition regex." + Environment.NewLine + Environment.NewLine);
                        AddStatus("Skipping 1 item in import" + Environment.NewLine);
                        continue;
                    }

                    Match othermatch = Regex.Match(itemraw, ".*INPUT TYPE=\"text\" NAME=\".*?\" SIZE=\"6\" VALUE=\"(.*?)\".*INPUT TYPE=\"text\" NAME=\".*?\" SIZE=\"6\" VALUE=\"(.*?)\".*", RegexOptions.IgnoreCase);
                    if (othermatch.Success)
                    {
                        qty = othermatch.Groups[1].Value;
                        price = othermatch.Groups[2].Value;
                        if ((qty == "") || (qty == null))
                        {
                            qty = "0";
                        }
                        if ((price == "") || (price == null))
                        {
                            price = "0";
                        }
                        swr.Write("Quantity   : " + qty + Environment.NewLine +
                            "Price      : " + price + Environment.NewLine);
                    }
                    else
                    {
                        swr.Write("ERROR in misc regex." + Environment.NewLine + Environment.NewLine);
                        AddStatus("Skipping 1 item in import" + Environment.NewLine);
                        continue;
                    }

                    Match listmatch = Regex.Match(itemraw, "On My <B>(.*?)</B> Wanted List", RegexOptions.IgnoreCase);
                    if (listmatch.Success)
                    {
                        list = listmatch.Groups[1].Value;
                        swr.Write("List       : " + list + Environment.NewLine);
                    }
                    else
                    {
                        swr.Write("No List found. Assuming Main." + Environment.NewLine + Environment.NewLine);
                        list = "Main";
                    }


                    wanteditems.Add(new Item()
                        {
                            type = type,
                            number = number,
                            condition = cond,
                            qty = System.Convert.ToInt32(qty),
                            price = ConvertToDecimal(price),
                            remarks = "Wanted List: " + list,
                            id = type + "-" + number,
                            extid = type + "-" + colour + "-" + number,
                            colour = colour
                        });

                    if (!wantedlists.Contains(list))
                    {
                        wantedlists.Add(list);
                    }

                    swr.Write(Environment.NewLine);
                }
            }

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                wantedListBox.Visible = true;
                wlLabel.Visible = true;
                nextButton.Text = "Import";
                unBox.ReadOnly = true;
                pwBox.ReadOnly = true;
                pwBox.Text = "";
                wantedListBox.Select();
                wantedListBox.Items.Add("All");
                wantedListBox.SelectedItem = "All";
                wantedListBox.Items.Add("Main");
                wantedlists.Sort();

                foreach (string list in wantedlists)
                {
                    if (list != "Main")
                    {
                        wantedListBox.Items.Add(list);
                    }
                }
            }));
            ImportStatus("Found " + wanteditems.Count() + " Lots in " + wantedlists.Count() + " Lists");
            AddStatus("Found " + wanteditems.Count() + " Lots in " + wantedlists.Count() + " Lists" + Environment.NewLine);
            swr.Write("Found " + wanteditems.Count() + " Lots in " + wantedlists.Count() + " Lists");
            wantedImportStep = 1;
            swr.Close();
        }
        #endregion

        #region GetWantedPage
        private string GetWantedPage()
        {
            string loginURL = "https://www.bricklink.com/login.asp";
            string wantedURL = "https://www.bricklink.com/wantedDetail.asp?viewFrom=wantedSearch&wantedSize=1000";
            string formParams = String.Format("a=a&logFrmFlag=Y&frmUsername={0}&frmPassword={1}", unBox.Text, pwBox.Text);
            string cookieHeader;
            string pageSource = null;

            try
            {
                // don't really want this referenced here directly, but it will take a bit to decouple this code
                var didLogIn = _bricklinkLoginApi.Login(cookies, unBox.Text, pwBox.Text);

                if (!didLogIn)
                {
                    return "##PageFail##";
                }

                HttpWebRequest wantedReq = (HttpWebRequest)WebRequest.Create(wantedURL);
                wantedReq.Timeout = 15000;
                wantedReq.CookieContainer = cookies;
                wantedReq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0";

                int pagefail = 0;
                bool pagesuccess = false;

                while ((pagesuccess == false) && (pagefail < 4))
                {
                    try
                    {
                        HttpWebResponse getResponse = (HttpWebResponse)wantedReq.GetResponse();
                        using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
                        {
                            pageSource = sr.ReadToEnd();
                        }
                        pagesuccess = true;
                    }
                    catch
                    {
                        ImportStatus("Retrying...", Color.Red);
                        pagefail++;
                    }
                }

                if (pagesuccess == false)
                {
                    return "##PageFail##";
                }
            }
            catch (Exception e)
            {
                ImportStatus("Failed to retrieve wanted list.", Color.Red);
                MessageBox.Show("Failed to retrieve wanted list:" + Environment.NewLine + e.Message);
                return "##PageFail##";
            }

            return pageSource;
        }
        #endregion

        #region Key pressed
        private void Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
            if (e.KeyChar == (char)Keys.Return)
            {
                if (wantedListBox.Visible == false)
                {
                    //if (unBox.Text == "")
                    //{
                    //    unBox.Select();
                    //}
                    //else if (pwBox.Text == "")
                    //{
                    //    pwBox.Select();
                    //}
                    //else
                    //{
                        nextButton_Click(sender, new EventArgs());
                    //}
                }
                else if (wantedImportStep == 1)
                {
                    if (this.ActiveControl.Name == "wantedListBox")
                    {
                        nextButton_Click(sender, new EventArgs());
                    }
                }
            }
        }
        private void Button_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
        #endregion

        #region When the window becomes visible, select the appropriate control
        private void ImportBLWanted_Shown(object sender, EventArgs e)
        {
            unBox.Text = MainWindow.settings.username;
            if (wantedImportStep == 0)
            {
                if (unBox.Text != "")
                {
                    pwBox.Select();
                }
            }
            else
            {
                wantedListBox.Select();
            }
        }
        #endregion

        #region Add Status Text
        public void AddStatus(string text)
        {
            if (AdviseParent != null)
            {
                AdviseParent(text);
            }
        }
        #endregion

        private void importWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GetAndParseWanted();
        }

        private void ImportStatus(string text, System.Drawing.Color? colour = null)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                if (colour == null)
                    colour = Color.Black;
                statusLabel.ForeColor = (System.Drawing.Color)colour;
                statusLabel.Text = text;
            }));
        }

        private void wantedListBox_DoubleClick(object sender, EventArgs e)
        {
            nextButton_Click(sender, e);
        }

        #region convert to decimal - now with region support!
        private decimal ConvertToDecimal(string text)
        {
            text = text.Replace(".", MainWindow.numberseperator);
            return Convert.ToDecimal(text);
        }
        #endregion
    }
}
