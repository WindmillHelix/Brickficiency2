using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Brickficiency.Classes;

namespace Brickficiency {
    public partial class MainWindow : Form {

        #region Start Calculation
        private void StartCalculate() {
            DisableMenu();
            statusQueueTimer.Start();
            EnableCalcStop();
            AddStatus("##Clear##");
            calcWorker.RunWorkerAsync();
        }
        #endregion
        private void calcWorker_DoWork(object sender, DoWorkEventArgs e) {
            #region Pre-calculation Stuff
            //clear all of the previous calc stuff out.
            calcitems.Clear();
            StoreDictionary.Clear();
            storeid.Clear();
            blacklistdic.Clear();
            matches = new List<FinalMatch>();
            displayreport = false;
            longcount = 0;
            storeprogresscounter = 0;
            matchesfound = false;
            calcfail = false;

            calcitems = dt2item(dt[currenttab]);

            // Slicker way to remove the unwanted elements.  CAC, 7/1/15
            for (int i = calcitems.Count - 1; i >= 0; i--) {
                Item item = calcitems[i];
                if ((item.status == "X") || (item.qty == 0)) {
                    calcitems.RemoveAt(i);
                }
            }

            if (calcWorker.CancellationPending) { return; }

            GetValidStores();

            if (calcWorker.CancellationPending) { return; }

            GetPGPages(settings.login);

            // This removes from consideration any stores that don't have any of the wanted items
            // since they cannot contribute to a solution.  This can reduce the number of stores
            // that need to be considered significantly.
            RemoveStoresWitoutAnyWantedItems(); // Added by CAC, 7/1/15

            if (calcWorker.CancellationPending) { return; }

            if (calcfail == true) {
                AddStatus(Environment.NewLine + "Some items were unavailable. Calculation cancelled." + Environment.NewLine);
                ResetProgressBar();
                EnableMenu();
                return;
            }

            if (calcWorker.CancellationPending) { return; }

            AddMissingItemsToStores();

            StoreList = StoreDictionaryToList(StoreDictionary);
            WantedItemList = new List<Item>(calcitems);

            ReportStart();
            #endregion

            switch (whichAlgToRun) {
                case RUN_OLD:
                    runTheAlgorithm(settings.minstores, settings.maxstores, settings.cont, StoreList, WantedItemList, RunOldAlgorithmOn, StandardPreProcess);
                    break;
                case RUN_NEW:
                    runTheAlgorithm(settings.minstores, settings.maxstores, settings.cont, StoreList, WantedItemList, KStoreCalc, StandardPreProcess);
                    break;
                case RUN_CUSTOM:
                    runTheAlgorithm(settings.minstores, settings.maxstores, settings.cont, StoreList, WantedItemList, CustomAlgorithm, CustomPreProcess);
                    break;
                case RUN_APPROX:
                    runApproxAlgorithm(settings.minstores, settings.maxstores, settings.approxtime * 1000, StoreList, WantedItemList, KStoreCalc, StandardPreProcess);
                    break;
                case RUN_CUSTOM_APPROX:
                    runApproxAlgorithm(settings.minstores, settings.maxstores, settings.approxtime * 1000, StoreList, WantedItemList, CustomApproximationAlgorithm, CustomApproximationPreProcess);
                    break;
            }

            #region Post-Calculation stuff
            EnableMenu();
            DisableCalcStop();
            ResetProgressBar();
            statusQueueTimer.Stop();

            if (displayreport == true) {
                try {
                    System.Diagnostics.Process.Start(outputfilename);
                } catch {
                    AddStatus("Error displaying report in your default web browser." + Environment.NewLine +
                        "Report file is available here: " + outputfilename + Environment.NewLine);
                }
            }
            AddStatus(Environment.NewLine + "Done." + Environment.NewLine + Environment.NewLine);
            #endregion
        }


        private void RemoveStoresWitoutAnyWantedItems() {
            var keysToRemove = StoreDictionary.Keys.Except(storesWithItemsList).ToList();
            foreach (var key in keysToRemove) {
                StoreDictionary.Remove(key);
            }
            AddStatus(Environment.NewLine + " Number of stores with at least one item: " + StoreDictionary.Count + Environment.NewLine);
        }

        #region Download and parse store names for each country
        private void GetValidStores() {
            if (settings.countries.Contains("All"))
                return;

            StreamWriter swr = new StreamWriter(debugpgfilename);

            bool pagefail = false;

            foreach (string country in settings.countries) {
                if (!calcWorker.CancellationPending) {
                    string page;
                    int tmpstores = 0;
                    if ((country == "North America") || (country == "Europe") || (country == "Asia"))
                        continue;

                    if (!db_countrystores.ContainsKey(country)) {
                        swr.Write(DateTime.Now + "downloading http://www.bricklink.com/browseStores.asp?countryID=" + db_countries[country] + Environment.NewLine);
                        page = GetPage("http://www.bricklink.com/browseStores.asp?countryID=" + db_countries[country]);
                        db_countrystores.Add(country, page);
                        if (page == "##PageFail##") {
                            pagefail = true;
                            swr.Close();
                            break;
                        }
                    } else {
                        swr.Write(DateTime.Now + "Using cached copy of http://www.bricklink.com/browseStores.asp?countryID=" + db_countries[country] + Environment.NewLine);
                        page = db_countrystores[country];
                    }

                    List<string> chunks = page.Split(new string[] { "Open Stores" }, StringSplitOptions.None).ToList();
                    List<string> rawstores = new List<string>();
                    try {
                        rawstores = chunks[1].Split(new string[] { "<BR>" }, StringSplitOptions.None).ToList();
                    } catch {
                        rawstores.Add(page);
                    }

                    foreach (string rawstore in rawstores) {
                        Match storenamecheck = Regex.Match(rawstore, @"<A HREF='store.asp\?p=(.*?)'>(.*?)</A>", RegexOptions.IgnoreCase);
                        if (storenamecheck.Success) {
                            //validstores.Add(storenamecheck.Groups[2].ToString());
                            string storename = storenamecheck.Groups[2].ToString();
                            StoreDictionary.Add(storename, new Dictionary<string, StoreItem>());
                            tmpstores++;
                        }
                    }

                    AddStatus(tmpstores + " stores found in " + country + Environment.NewLine);
                }
            }

            swr.Close();

            if (pagefail == true) {
                return;
            }

            AddStatus(Environment.NewLine);
        }
        #endregion


        public string GetPGPage(Item item, Boolean live)
        {
            string page;

            if (!db_blitems[item.id].pgpage.ContainsKey(item.colour))
            {
                item.availqty = 0;
                item.availstores = 0;
                AddStatus("Getting Price Guide information for " + db_colours[item.colour].name + " " + db_blitems[item.id].name + Environment.NewLine);

                if (live)
                {
                    page = GetPage("http://www.bricklink.com/catalogPG.asp?" + item.type + "=" + item.number + "&colorID=" + item.colour, settings.login);
                    // Uncomment these three lines to save prices guide information so that it is used instead of live data.
                    // Comment them when done.
                    //if (!Directory.Exists("PGCache")) { Directory.CreateDirectory("PGCache"); }
                    //System.IO.StreamWriter file = new StreamWriter("PGCache\\" + item.type + "_" + item.number + "_" + item.colour + ".txt");
                    //file.Write(page);
                }
                else
                {
                    try
                    {
                        StreamReader pageReader = new StreamReader("PGCache\\" + item.type + "_" + item.number + "_" + item.colour + ".txt");
                        page = pageReader.ReadToEnd();
                    }
                    catch (Exception)
                    {
                        page = "";
                    }
                }
                if (page == null || page == "##PageFail##")
                {
                    AddStatus("Error downloading price guide" + Environment.NewLine);
                    calcfail = true;
                }

                db_blitems[item.id].pgpage.Add(item.colour, page);
            }
            else
            {
                AddStatus("Using cached Price Guide information for " + db_colours[item.colour].name + " " + db_blitems[item.id].name + Environment.NewLine);
                page = db_blitems[item.id].pgpage[item.colour];
            }

            return page;
        }

        public void ParsePage(string page, Item item)
        {
            if (page == null) return;

            List<string> chunks = page.Split(new string[] { "<B>Currently Available</B>" }, StringSplitOptions.None).ToList();
            List<string> rawpgitems;
            if (chunks.Count > 1)
            {
                rawpgitems = chunks[1].Split(new string[] { "</TD></TR>" }, StringSplitOptions.None).ToList();
            }
            else
            {
                AddStatus("Error downloading price guide (got page, but there was nothing on it)" + Environment.NewLine);
                calcfail = true;
                return;
            }
            rawpgitems.Add("#usedstart#");
            if (chunks.Count() > 2)
            {
                rawpgitems.AddRange(chunks[2].Split(new string[] { "</TD></TR>" }, StringSplitOptions.None).ToList());
            }

            int usedmarker = 0;

            foreach (string raw in rawpgitems)
            {
                if (raw == "#usedstart#")
                {
                    usedmarker = 1;
                    continue;
                }

                if ((item.condition == "N") && (usedmarker == 1))
                    break;

                if (raw.Contains("nbsp;(S)</"))
                {
                    AddStatus("skip ");
                    continue;
                }

                Match linematch = Regex.Match(raw, ".*<TR ALIGN=" + "\"" + @"RIGHT" + "\"" + @"><TD NOWRAP>.*?<A HREF=" + "\"" +
                    @"/store.asp\?sID=(\d*?)&.*?<IMG SRC=" + "\"" + @"/images/box16(.).png" + "\"" + @".*?TITLE=" + "\"" + @"Store: (.*?)" +
                    "\"" + @" ALIGN=" + "\"" + @"ABSMIDDLE" + "\"" + @">.*?</TD><TD>(\d*)</TD><TD.*?&nbsp;\D*([\d,]*)\.(\d+)$");

                if (linematch.Success)
                {
                    string id = linematch.Groups[1].ToString();

                    if (blacklistdic.ContainsKey(id))
                        continue;

                    string ship = linematch.Groups[2].ToString();
                    string storename = linematch.Groups[3].ToString();
                    int qty = System.Convert.ToInt32(linematch.Groups[4].ToString());
                    string price1 = linematch.Groups[5].ToString().Replace(",", "");
                    decimal price = ConvertToDecimal(price1 + "." + linematch.Groups[6].ToString());

                    if (ship == "Y")
                    {
                        if (((StoreDictionary.ContainsKey(storename)) || (settings.countries.Contains("All"))) && (true)) // blacklist
                        {
                            item.availstores++;
                            item.availqty += qty;
                            if (!storeid.ContainsKey(storename))
                                storeid.Add(storename, id);
                            if (!StoreDictionary.ContainsKey(storename))
                                StoreDictionary.Add(storename, new Dictionary<string, StoreItem>());
                            if (!StoreDictionary[storename].ContainsKey(item.extid))
                            {
                                StoreDictionary[storename].Add(item.extid, new StoreItem());
                            }
                            if (StoreDictionary[storename][item.extid].qty == 0)
                            {
                                StoreDictionary[storename][item.extid].qty = qty;
                                StoreDictionary[storename][item.extid].price = price;
                            }
                            else if (StoreDictionary[storename][item.extid].qty > 0)
                            {
                                StoreDictionary[storename][item.extid].qty = StoreDictionary[storename][item.extid].qty + qty;
                                item.availqty = item.availqty + qty;
                                if (StoreDictionary[storename][item.extid].price < price)
                                    StoreDictionary[storename][item.extid].price = price;
                            }
                            storesWithItemsList.Add(storename); // Added by CAC, 6/24/15
                        }
                    }
                }
                else
                {
                    if (Regex.Match(raw, "<TR ALIGN=" + "\"" + @"RIGHT" + "\"" + @"><TD NOWRAP>").Success)
                        AddStatus("No Match: " + raw + Environment.NewLine + Environment.NewLine);
                }
            }
        }

        #region Download and parse Price Guide pages
        public void GetPGPages(Boolean live) {
            StreamWriter swr;
            if (settings.countries.Contains("All"))
                swr = new StreamWriter(debugpgfilename);
            else
                swr = new StreamWriter(debugpgfilename, true);

            string[] tempbl;
            if (settings.blacklist != "") {
                tempbl = settings.blacklist.Split(new char[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string blentry in tempbl) {
                    if (!blacklistdic.ContainsKey(blentry))
                        blacklistdic.Add(blentry, true);
                }
            }
            SetProgressBar(calcitems.Count);
            storesWithItemsList.Clear(); // Added by CAC, 6/24/15

            foreach (Item item in calcitems) {
                if (!calcWorker.CancellationPending) {
                    string page;
                    page = GetPGPage(item, live);
                    ParsePage(page, item);

                    if (item.availqty < item.qty) {
                        AddStatus("Error: " + db_colours[item.colour].name + " " + db_blitems[item.id].name +
                            ":Either this is not available from any stores you've selected or you need to log in." + Environment.NewLine);
                    }
                    List<string> itemcolours = new List<string>();

                    if (item.colour == "0" && live)
                    {
                        if (db_blitems[item.id].pgcolourspage != null) {
                            AddStatus("Using cached colors for Price Guide for item : " + db_blitems[item.id].name + Environment.NewLine);
                            page = db_blitems[item.id].pgcolourspage;
                        }
                        else {
                            AddStatus("Finding colors for Price Guide for item : " + db_blitems[item.id].name + Environment.NewLine);
                            page = GetPage("http://alpha.bricklink.com/pages/clone/catalogitem.page?" + item.type + "=" + item.number);
                            db_blitems[item.id].pgcolourspage = page;
                        }

                        foreach (Match colourmatch in Regex.Matches(page, "http://www\\.bricklink\\.com/catalogPG\\.asp\\?" + item.type + "=" + item.number + "&colorID=([0-9]+)")) {
                            string colour = colourmatch.Groups[1].Value;
                            AddStatus(" " + db_colours[colour].name + Environment.NewLine);
                            itemcolours.Add(colour);
                        }
                        if (itemcolours.Count > 1)
                        {
                            // sometimes NA get returned as a colour
                            itemcolours.Remove("0");

                        }
                        if (itemcolours.Count == 0) {
                            AddStatus("Could not find any colors for Price Guide" + Environment.NewLine);
                            calcfail = true;
                            swr.Close();
                            goto done;
                        }
                    }
                    else {
                        itemcolours.Add(item.colour);
                    }

                    int pgsuccess = 0;
                    foreach (string colour in itemcolours)
                    {
                        if (!db_blitems[item.id].pgpage.ContainsKey(colour)) {
                            item.availqty = 0;
                            item.availstores = 0;
                            AddStatus("Getting Price Guide information for " + db_colours[colour].name + " " + db_blitems[item.id].name + Environment.NewLine);
                            swr.Write(DateTime.Now + "downloading http://www.bricklink.com/catalogPG.asp?" + item.type + "=" + item.number + "&colorID=" + colour + Environment.NewLine);

                            if (live) {
                                page = GetPage("http://www.bricklink.com/catalogPG.asp?" + item.type + "=" + item.number + "&colorID=" + colour, settings.login);
                                // Uncomment these three lines to save prices guide information so that it is used instead of live data.
                                // Comment them when done.
                                //if (!Directory.Exists("PGCache")) { Directory.CreateDirectory("PGCache"); }
                                //System.IO.StreamWriter file = new StreamWriter("PGCache\\" + item.type + "_" + item.number + "_" + colour + ".txt");
                                //file.Write(page);
                            } else {
                                try {
                                    StreamReader pageReader = new StreamReader("PGCache\\" + item.type + "_" + item.number + "_" + colour + ".txt");
                                    page = pageReader.ReadToEnd();
                                } catch (Exception) {
                                    page = "";
                                }
                            }
                            if (page == null || page == "##PageFail##") {
                                AddStatus("Error downloading price guide" + Environment.NewLine);
                                calcfail = true;
                                swr.Close();
                                goto done;
                            }

                            db_blitems[item.id].pgpage.Add(colour, page);
                        } else {
                            AddStatus("Using cached Price Guide information for " + db_colours[colour].name + " " + db_blitems[item.id].name + Environment.NewLine);
                            page = db_blitems[item.id].pgpage[colour];
                        }

                        List<string> chunks = page.Split(new string[] { "<B>Currently Available</B>" }, StringSplitOptions.None).ToList();
                        List<string> rawpgitems;
                        if (chunks.Count > 1) {
                            rawpgitems = chunks[1].Split(new string[] { "</TD></TR>" }, StringSplitOptions.None).ToList();
                        } else {
                            AddStatus("Error downloading price guide (got page, but there was nothing on it)" + Environment.NewLine);
                            continue;
                        }
                        rawpgitems.Add("#usedstart#");
                        if (chunks.Count() > 2) {
                            rawpgitems.AddRange(chunks[2].Split(new string[] { "</TD></TR>" }, StringSplitOptions.None).ToList());
                        }

                        int usedmarker = 0;

                        foreach (string raw in rawpgitems) {
                            if (raw == "#usedstart#") {
                                usedmarker = 1;
                                continue;
                            }

                            if ((item.condition == "N") && (usedmarker == 1))
                                break;

                            if (raw.Contains("nbsp;(S)</")) {
                                AddStatus("skip ");
                                continue;
                            }

                            Match linematch = Regex.Match(raw, ".*<TR ALIGN=" + "\"" + @"RIGHT" + "\"" + @"><TD NOWRAP>.*?<A HREF=" + "\"" +
                                @"/store.asp\?sID=(\d*?)&.*?<IMG SRC=" + "\"" + @"/images/box16(.).png" + "\"" + @".*?TITLE=" + "\"" + @"Store: (.*?)" +
                                "\"" + @" ALIGN=" + "\"" + @"ABSMIDDLE" + "\"" + @">.*?</TD><TD>(\d*)</TD><TD.*?&nbsp;\D*([\d,]*)\.(\d+)$");
                            if (linematch.Success) {
                                string id = linematch.Groups[1].ToString();

                                if (blacklistdic.ContainsKey(id))
                                    continue;

                                string ship = linematch.Groups[2].ToString();
                                string storename = linematch.Groups[3].ToString();
                                int qty = System.Convert.ToInt32(linematch.Groups[4].ToString());
                                string price1 = linematch.Groups[5].ToString().Replace(",", "");
                                decimal price = ConvertToDecimal(price1 + "." + linematch.Groups[6].ToString());

                                if (ship == "Y") {
                                    if (((StoreDictionary.ContainsKey(storename)) || (settings.countries.Contains("All"))) && (true)) // blacklist
                                    {
                                        item.availqty = item.availqty + qty;
                                        if (!storeid.ContainsKey(storename))
                                            storeid.Add(storename, id);
                                        if (!StoreDictionary.ContainsKey(storename))
                                            StoreDictionary.Add(storename, new Dictionary<string, StoreItem>());
                                        if (!StoreDictionary[storename].ContainsKey(item.extid)) {
                                            StoreDictionary[storename].Add(item.extid, new StoreItem());
                                        }
                                        if (StoreDictionary[storename][item.extid].qty == 0) {
                                            StoreDictionary[storename][item.extid].qty = qty;
                                            StoreDictionary[storename][item.extid].price = price;
                                            StoreDictionary[storename][item.extid].colour = colour;
                                            item.availstores++;
                                        } else if (StoreDictionary[storename][item.extid].qty > 0) {
                                            StoreDictionary[storename][item.extid].qty = StoreDictionary[storename][item.extid].qty + qty;
                                            item.availqty = item.availqty + qty;
                                            if (StoreDictionary[storename][item.extid].price > price)
                                            {
                                                StoreDictionary[storename][item.extid].price = price;
                                                StoreDictionary[storename][item.extid].colour = colour;
                                            }
                                        }
                                        storesWithItemsList.Add(storename); // Added by CAC, 6/24/15
                                    }
                                }
                            } else {
                                if (Regex.Match(raw, "<TR ALIGN=" + "\"" + @"RIGHT" + "\"" + @"><TD NOWRAP>").Success)
                                    AddStatus("No Match: " + raw + Environment.NewLine + Environment.NewLine);
                            }
                        }
                        if (item.availqty < item.qty) {
                            AddStatus("Error: " + db_colours[colour].name + " " + db_blitems[item.id].name +
                                ":Either this is not available from any stores you've selected or you need to log in." + Environment.NewLine);
                            continue;
                        } else {
                            pgsuccess++;
                            AddStatus("Available from " + item.availstores + " stores" + Environment.NewLine);
                            //Progress();
                        }
                    }

                    if(pgsuccess == 0)
                    {
                        calcfail = true;
                        ResetProgressBar();
                        swr.Close();
                        goto done;
                    }
                }
            }
        done: ;
            swr.Close();
        }
        #endregion

        #region Output html report header and item list
        private void ReportStart() {
            using (StreamWriter swr = new StreamWriter(outputfilename)) {
                swr.WriteLine("<html>");
                swr.WriteLine("<head>");
                swr.WriteLine("<title></title>");
                swr.WriteLine("");
                swr.WriteLine("<style>");
                swr.WriteLine(".header {font-family:Verdana;font-size:16;color:black;margin-left:50;display:block;font-weight:bold}");
                swr.WriteLine(".general {font-family:Verdana;font-size:14;color:black;margin-left:50;display:block}");
                swr.WriteLine(".expander {font-family:Verdana;font-size:20;color:black;text-align:center;width:16;height:18;margin-left:50;position:relative;left:-25;top:14;cursor: pointer;}");
                swr.WriteLine("</style>");
                swr.WriteLine("");
                swr.WriteLine("<script>");
                swr.WriteLine("function collapse(x){");
                swr.WriteLine("	var oTemp=eval(\"document.all.text_\"+x);");
                swr.WriteLine("	var oClick=eval(\"document.all.click_\"+x);");
                swr.WriteLine("");
                swr.WriteLine("	if(oTemp.style.display==\"block\" || oTemp.style.display==\"\"){");
                swr.WriteLine("		oTemp.style.display=\"none\";");
                swr.WriteLine("		oClick.innerHTML=\"+\";");
                swr.WriteLine("	}else{");
                swr.WriteLine("		oTemp.style.display=\"block\";");
                swr.WriteLine("		oClick.innerHTML=\"-\";");
                swr.WriteLine("	}");
                swr.WriteLine("}");
                swr.WriteLine("function replace(replace,source){");
                swr.WriteLine("	var t = document.getElementById(source);");
                swr.WriteLine("	var r = document.getElementById(replace).value;");
                swr.WriteLine("	t.value=t.value.replace(/<WANTEDLISTID>.*<\\/WANTEDLISTID>/g, \"<WANTEDLISTID>\" + r + \"</WANTEDLISTID>\");");
                swr.WriteLine("}");
                swr.WriteLine("</script>");
                swr.WriteLine("");
                swr.WriteLine("</head>");
                swr.WriteLine("<body>");
                swr.WriteLine("");

                var unavailitems = from item in calcitems where item.availqty < item.qty select item;
                if (unavailitems.Count() > 0) {
                    swr.WriteLine("<div id=\"click_nonefound\" class=\"expander\" onclick=\"collapse('nonefound')\"> - </div>");
                    swr.WriteLine("	<div class=\"header\">");
                    swr.WriteLine("		Parts with no sources");
                    swr.WriteLine("	</div>");
                    swr.WriteLine("<div id=\"text_nonefound\" class=\"general\">");
                    foreach (Item item in unavailitems) {
                        swr.WriteLine(item.availqty + " " + db_blitems[item.id].name + "");
                    }
                }

                swr.WriteLine("<div id=\"click_parts\" class=\"expander\" onclick=\"collapse('parts')\"> - </div>");
                swr.WriteLine("<div class=\"header\">");
                swr.WriteLine("	Parts list sorted by number of stores");
                swr.WriteLine("</div>");
                swr.WriteLine("<div id=\"text_parts\" class=\"general\">");

                foreach (Item item in calcitems.OrderBy(i => i.availstores)) {
                    swr.WriteLine("	<b>" + item.availstores + "</b> stores have " + db_colours[item.colour].name + " " + db_blitems[item.id].name + "<br>");
                }
                swr.WriteLine("</div>" + Environment.NewLine + Environment.NewLine);

            }
        }
        #endregion

        #region set prices to 0 if items not in StoreDictionary
        private void AddMissingItemsToStores() {
            foreach (var store in StoreDictionary) {
                // If a store does not have a given item, add it to the store with qty 0. 
                // (Makes the algorithm simpler.) 
                foreach (Item item in calcitems) {
                    if (!store.Value.ContainsKey(item.extid)) {
                        store.Value.Add(item.extid, new StoreItem());
                    }
                }
            }

        }
        #endregion

        #region stock preProcess methods
        private List<Store> StoreDictionaryToList(Dictionary<string, Dictionary<string, StoreItem>> storeDictionary) {
            List<Store> sortedStoreList = new List<Store>();
            foreach (string storeName in storeDictionary.Keys.ToList()) {
                sortedStoreList.Add(new Store(storeName, storeDictionary[storeName]));
            }
            return sortedStoreList;
        }
        #endregion

        #region check if match should be added
        private void MatchAdd(FinalMatch thismatch) {
            lock (calcLock) {
                matchesfoundcount++;
                matches.Add(thismatch);
                if (matches.Count > settings.nummatches) {
                    matches = matches.OrderBy(i => i.totalprice).Take(settings.nummatches).ToList();
                }
            }
        }
        #endregion

        #region Output html report middle
        private void ReportMiddle() {
            int num = 0;
            bool numChanged = false;
            displayreport = true;
            List<Item> wantedItemsSortedForReport;
            if (settings.sortcolour == false) {
                wantedItemsSortedForReport = calcitems.OrderBy(i => db_blitems[i.id].name).ToList();
            } else {
                wantedItemsSortedForReport = calcitems.OrderBy(i => db_colours[i.colour].name).
                    ThenBy(i => db_blitems[i.id].name).ToList();
            }
            using (StreamWriter swr = new StreamWriter(outputfilename, true)) {
                int matchfinalindex = 1;
                foreach (FinalMatch match in matches.OrderBy(i => i.totalprice)) {
                    if (match.num != num) {
                        num = match.num;
                        swr.WriteLine("<div id=\"click_" + num + "\" class=\"expander\" onclick=\"collapse('" + num + "')\"> - </div>");
                        swr.WriteLine("<div class=\"header\">");
                        swr.WriteLine(" " + num + " store solutions");
                        swr.WriteLine("</div>");
                        swr.WriteLine("<div id=\"text_" + num + "\" class=\"general\">");
                        numChanged = true;
                    } else {
                        num = match.num;
                    }

                    foreach (Item item in wantedItemsSortedForReport) {
                        foreach (string storename in match.GetStoreNames()) {
                            MatchDetails md = match.GetDetails(storename);
                            StoreItem si = md.GetItem(item.extid);
                            if (si != null) {
                                decimal qtyprice = si.price * si.qty;
                                md.list += "<TR><TD><a href=\"http://www.bricklink.com/store.asp?sID=" +
                                           storeid[storename] + "&q=" + item.number + "&colorID=" + si.colour + "\"><IMG SRC=" +
                                           (item.colour == "0" ? GenerateImageURL(item.id, si.colour) : item.imageurl) +
                                           "></a></TD><TD><a href=\"http://www.bricklink.com/store.asp?sID=" +
                                           storeid[storename] + "&q=" + item.number + "&colorID=" + si.colour + "\">" +
                                           db_colours[si.colour].name + " " + db_blitems[item.id].name +
                                           "</a></TD><TD>" + si.qty + " @ " + si.price + "</TD><TD>" + qtyprice +
                                           "</TD></TR>";
                                md.xml += " <ITEM>" + Environment.NewLine + "  <ITEMID>" + item.number + "</ITEMID>" +
                                          Environment.NewLine +
                                          "  <ITEMTYPE>" + item.type + "</ITEMTYPE>" + Environment.NewLine +
                                          "  <COLOR>" + si.colour + "</COLOR>" + Environment.NewLine +
                                          "  <MINQTY>" + si.qty + "</MINQTY>" + Environment.NewLine +
//                                          "  <WANTEDLISTID>xxxxxx</WANTEDLISTID>" + Environment.NewLine +
                                          " </ITEM>" + Environment.NewLine;
                                md.totalNumberItemsFromStore += si.qty;
                                md.totalNumberLotsFromStore++;
                            }
                        }
                    }

                    swr.WriteLine("	<div id=\"click_" + match.num + "_" + matchfinalindex + "\" class=\"expander\" onclick=\"collapse('" + match.num + "_" + matchfinalindex + "')\"> - </div>");
                    swr.WriteLine("	<div class=\"header\">");

                    swr.Write("		<b>Match # " + matchfinalindex + " : $ " + match.totalprice + "</b>");
                    if (match.num > 1) {
                        StringBuilder sb = new StringBuilder(" ($ ");
                        foreach (string storename in match.GetStoreNames()) {
                            sb.Append(match.GetDetails(storename).totalStorePrice + ", $ ");
                        }
                        sb.Remove(sb.Length - 4, 4);
                        sb.Append(")");
                        swr.WriteLine(sb.ToString());
                    }
                    swr.WriteLine("	</div><br>");
                    swr.WriteLine("	<div id=\"text_" + match.num + "_" + matchfinalindex + "\" class=\"general\">");

                    int tmpind = 0;
                    foreach (string storeName in match.GetStoreNames()) {
                        if (storeName != "") {
                            MatchDetails md = match.GetDetails(storeName);
                            swr.WriteLine("		<a href=\"http://www.bricklink.com/store.asp?sID=" + storeid[storeName] + "\" target=\"_blank\">" +
                                storeName + "</a> (" + storeid[storeName] + ") - " + md.totalStorePrice + " - " + md.totalNumberItemsFromStore + " items in " + md.totalNumberLotsFromStore + " lots<BR>");
                            swr.WriteLine("		<table border>" + md.list);
                            swr.WriteLine("		</table>");
                            swr.WriteLine("		<b>Total: " + md.totalStorePrice + "</b>");
                            swr.WriteLine("		<div id=\"click_" + match.num + "_" + matchfinalindex + "_XML" + tmpind + "\" class=\"expander\" onclick=\"collapse('" +
                                match.num + "_" + matchfinalindex + "_XML" + tmpind + "')\"> - </div>");
                            swr.WriteLine("		<div class=\"general\">");
                            swr.WriteLine("			Bricklink Wanted List XML");
                            swr.WriteLine("		</div>");
                            swr.WriteLine("		<div id=\"text_" + match.num + "_" + matchfinalindex + "_XML" + tmpind + "\" class=\"general\">");
                            swr.WriteLine("			<a href=\"http://www.bricklink.com/wantedView.asp\" target=\"_blank\">Wanted List Number</a>: " +
                                "<input type=\"text\" id=\"text_" + match.num + "_" + matchfinalindex + "_text" + tmpind + "\">");
                            swr.WriteLine("			<input type=\"button\" value=\"Go\" onclick=\"replace('text_" + match.num + "_" + matchfinalindex + "_text" + tmpind +
                                "','text_" + match.num + "_" + matchfinalindex + "_textarea" + tmpind + "')\"> <a href=\"http://www.bricklink.com/wantedXML.asp\" " +
                                "target=\"_blank\">Upload</a><br>");
                            swr.WriteLine("			<textarea id=\"text_" + match.num + "_" + matchfinalindex + "_textarea" + tmpind + "\" rows=\"12\" cols=\"75\" " +
                                "onclick=\"this.focus();this.select()\" readonly=\"readonly\">" + Environment.NewLine + "<INVENTORY>" + Environment.NewLine + md.xml + "</INVENTORY></textarea>");
                            swr.WriteLine("		</div><br><br>");
                            swr.WriteLine("		<script>window.onload=collapse('" + match.num + "_" + matchfinalindex + "_XML" + tmpind + "');</script>" + Environment.NewLine + Environment.NewLine);
                        }
                        tmpind++;
                    }

                    swr.WriteLine("	</div><br>");

                    matchfinalindex++;

                    if (matchfinalindex > settings.nummatches) {
                        break;
                    }
                    if (numChanged) {
                        swr.WriteLine("	<script>");
                        swr.WriteLine("	window.onload=collapse('" + num + "');");
                        swr.WriteLine("	</script>");
                        numChanged = false;
                    }
                }
                swr.WriteLine("</div>");
            }
        }
        #endregion

        // This method was here but isn't used.  I commented it out.  CAC, 7/8/15
        //#region find cheapest from a list
        //private List<int> FindCheapest(params decimal[] pricesin) {
        //    Dictionary<int, decimal> prices = new Dictionary<int, decimal>();
        //    int tmpcount = 0;
        //    foreach (decimal price in pricesin) {
        //        prices.Add(tmpcount, price);
        //        tmpcount++;
        //    }
        //    List<int> indexesout = new List<int>();
        //    foreach (KeyValuePair<int, decimal> index in prices.OrderBy(i => i.Value)) {
        //        indexesout.Add(index.Key);
        //    }
        //    return indexesout;
        //}
        //#endregion

        private long previousPrinted = 0;
        #region Matches Counter
        private void statusQueueTimer_Tick(object sender, EventArgs e) {
            if (running) {
                long toPrint = longcount / 1000000;
                if (toPrint != previousPrinted) {
                    AddStatus(" " + toPrint);
                    previousPrinted = toPrint;
                }
            }
        }
        #endregion

        #region write debug output to a file
        private void WriteDebug() {
            using (StreamWriter swr = new StreamWriter(programdata + "\\debug\\sorteditems.txt")) {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
                serializer.Serialize(swr, WantedItemList);
                swr.Close();
            }

            using (StreamWriter swr = new StreamWriter(programdata + "\\debug\\sortedstores.txt")) {
                foreach (var store in StoreDictionary) {
                    swr.WriteLine(store.Key);

                    foreach (KeyValuePair<string, StoreItem> item in StoreDictionary[store.Key]) {
                        swr.WriteLine(" " + item.Key);
                        swr.WriteLine("     " + StoreDictionary[store.Key][item.Key].qty + " @ " + StoreDictionary[store.Key][item.Key].price);
                    }
                }
                swr.Close();
            }

            using (StreamWriter swr = new StreamWriter(programdata + "\\debug\\matches.txt")) {
                List<FinalMatch> exportmatches = new List<FinalMatch>();

                foreach (FinalMatch match in matches) {
                    exportmatches.Add(match);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<FinalMatch>));
                serializer.Serialize(swr, exportmatches);
                swr.Close();
            }
        }
        #endregion

        #region Important methods for Algorithms
        //The stopwatch object that all algorithms will use.
        System.Diagnostics.Stopwatch stopWatch;

        private void beginCalculation(int k, int numTotalStores) {
            previousPrinted = 0;
            AddStatus(Environment.NewLine + "Calculating " + k + " store solutions...");
            if (k > 2) {
                AddStatus(Environment.NewLine+"  Millions of solutions checked: ");
            }
            stopWatch = new System.Diagnostics.Stopwatch();
            //shortcount = 0;
            longcount = 0;

            // Added to this method by CAC, 7/6/15 to fix a bug related to the report.
            matches = new List<FinalMatch>();

            matchesfoundcount = 0;
            stopWatch.Start();
            SetProgressBar(numTotalStores);
            running = true;
            stopAlgorithmEarly = false;
        }
        //When a final match is found, populate a list with the names of the 
        //stores and use this method to add the correct information about the final match
        private void addFinalMatch(List<string> storeNamesInMatch) {
            matchesfound = true;

            FinalMatch theMatch = new FinalMatch();
            List<Store> storesInMatch = new List<Store>();
            foreach (string storename in storeNamesInMatch) {
                theMatch.AddStore(storename);
                storesInMatch.Add(new Store(storename, StoreDictionary[storename]));
            }

            for (int i = 0; i < WantedItemList.Count; i++) {
                Item item = WantedItemList[i];
                storesInMatch = storesInMatch.OrderBy(s => s.getPrice(item.extid)).ThenByDescending((s => s.getQty(item.extid))).ToList();
                int totalwantedqty = item.qty;
                int storeindex = 0;
                while (totalwantedqty > 0) {
                    Store currentStore = storesInMatch[storeindex];
                    int storeQty = currentStore.getQty(item.extid);
                    int actualQtyFromStore = (storeQty >= totalwantedqty) ? totalwantedqty : storeQty;
                    totalwantedqty -= actualQtyFromStore;
                    theMatch.GetDetails(currentStore.getName()).AddItem(item.extid, actualQtyFromStore, currentStore.getPrice(item.extid), currentStore.getColour(item.extid));
                    storeindex++;
                }
            }
            // Check for a solution that doesn't use one of the stores.  Reject it since it would have been
            // found for a smaller value of k.
            foreach(string storename in theMatch.GetStoreNames())
            {
                if (theMatch.GetDetails(storename).totalStorePrice == 0)
                {
                    return;
                }
            }
            MatchAdd(theMatch);
        }
        private void endCalculation(int k) {
            running = false;
            AddStatus(Environment.NewLine + "  Total solutions checked: " + longcount);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            AddStatus(Environment.NewLine);
            ResetProgressBar();

            if (calcWorker.CancellationPending || stopAlgorithmEarly)
            {
                AddStatus("  ** Not all possible combinations were tried **" + Environment.NewLine);
            }
            if (matches.Count > 0) {
                AddStatus("  "+matchesfoundcount + " Matches Found in " + elapsedTime + Environment.NewLine);
                WriteDebug();
                ReportMiddle();
            } else {
                AddStatus("  No Matches Found in " + elapsedTime + Environment.NewLine);
                WriteDebug();
            }
        }
        #endregion
    }
}
