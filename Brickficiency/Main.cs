using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Xml.Serialization;
using System.Threading;
using System.Globalization;
using System.Data.SqlServerCe;
using Ionic.Zip;
using Brickficiency.Classes;
using System.Diagnostics;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.DependencyInjection;
using WindmillHelix.Brickficiency2.Services;
using WindmillHelix.Brickficiency2.Services.Data;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using Brickficiency.UI;

namespace Brickficiency {
    public partial class MainWindow : Form {
        //todo:
        //editing/creating files
        // -copy/paste
        // -undo
        //multiple lots that are exactly the same
        //bricklink login page
        //import multiple wanted lists
        //fix right click behavior
        //fix more than 1000 items on a wanted list
        //cached page timer? so if you leave bf on for three days it will not use a cached page
        //least favorite store list
        //supplemental report when no matches are found
        //price guide info //////////////// what does this mean??

        //put backups of code for both in gdrive
        //error when DB is out of date


        ///////////////// add &v=D to pgpage get
        ///////////////// remove the thing that is saying "skip" while downloading pages

        //done:
        //fixed bug in importing wanted lists. learned I am still really bad at creating regexes.
        //updated db url

        private readonly IColorService _colorService;
        private readonly IItemTypeService _itemTypeService;
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;
        private readonly IDataUpdateService _dataUpdateService;

        private readonly ImportWantedListForm _importWantedListForm;
        private readonly UpdateCheck _updateConfirmationForm;

        // don't really want this referenced here directly, but it will take a bit to decouple this code
        private readonly IBricklinkLoginApi _bricklinkLoginApi; 

        #region prepare some vars
        //global stuff
        public static string version =  "v2.0.0"; // CHANGE APPLICATION PROPERTIES > Assembly Information
        public static string programname = "Brickficiency";
        static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string programdata = appdata + "\\" + programname + "\\";
        public static string settingsfilename = programdata + programname + "-Settings.xml";
        public static string databasefilename = programdata + "bfdb.sdf";
        public static string databasezipfilename = programdata + "bfdb.zip";
        public static string databaseurl = "http://www.buildingoutloud.com/bf/bfdb.zip";
        string debugpgfilename = programdata + "\\debug\\Debug-priceguide.txt";
        string debugparsesource = programdata + "\\debug\\Debug-parsesource.html";
        string debugopenfilename = programdata + "\\debug\\Debug-open.txt";
        string debugdbfilename = programdata + "\\debug\\Debug-db.txt";
        string debugwebreqfilename = programdata + "\\debug\\Debug-webreq.txt";
        string debuglddimport = programdata + "\\debug\\Debug-lddimport.txt";
        public static string debugimportfilename = programdata + "\\debug\\Debug-import.txt";
        public static Settings settings = new Settings();
        public static string blacklist = "";
        public static List<DataGridView> dgv = new List<DataGridView>();
        public static List<DataTable> dt = new List<DataTable>();
        public static List<TabInfo> fileinfo = new List<TabInfo>();
        public static int currenttab = 0;
        public static Dictionary<string, DBColour> db_colours = new Dictionary<string, DBColour>();
        public static Dictionary<string, DBCat> db_categories = new Dictionary<string, DBCat>();
        public static Dictionary<string, string> db_typenames = new Dictionary<string, string>();
        public static Dictionary<string, DBBLItem> db_blitems = new Dictionary<string, DBBLItem>();
        public static Dictionary<string, string> db_countries = new Dictionary<string, string>();
        public static Dictionary<string, string> db_LDD2BL = new Dictionary<string, string>();
        public static Dictionary<string, List<DBItemContain>> db_containers = new Dictionary<string, List<DBItemContain>>();
        public static Dictionary<string, string> db_rebrickpages = new Dictionary<string, string>();
        public static Dictionary<string, string> db_rebrickaltids = new Dictionary<string, string>();
        public static Dictionary<string, string> db_blaltids = new Dictionary<string, string>();
        int imagetimercount = 0;
        int itemtimercount = 0;
        Bitmap blank = Brickficiency.Properties.Resources.blank;
        public static Queue<string> messages = new Queue<string>();
        public static string culturename = CultureInfo.CurrentCulture.Name;
        public static RegionInfo culture = new RegionInfo(culturename);
        public static string numberseperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public static string currencysymbol = culture.CurrencySymbol;
        public static Color errorcell = (Color)System.Drawing.ColorTranslator.FromHtml("#FF9999");

        //web stuff
        CookieContainer cookies = new CookieContainer();
        DateTime cookietime;
        public static string password; // figure this shit out
        public static bool loggedin = false;
        public static Object imgTimerLock = new Object();
        public static Object itemTimerLock = new Object();
        public static Object pageLock = new Object();
        public static int inLock = 0;
        public static List<ImageDL> imageDLList = new List<ImageDL>();
        public static List<ItemDL>  itemDLList =  new List<ItemDL>();
        public static string RBapiKey = "bITJSRewdX";

        //calc stuff
        public Dictionary<string, string> db_countrystores = new Dictionary<string, string>();

        public List<Item> calcitems = new List<Item>(); // The items on the wanted list


        public Dictionary<string, string> storeid = new Dictionary<string, string>();

        public List<FinalMatch> matches = new List<FinalMatch>();
        public Dictionary<string, bool> blacklistdic = new Dictionary<string, bool>();

        //-------------------------------------------------------------------
        // Fields added or significantly modified by CAC, 2015-06-24

        // Set to true when this is being used for a class and false when it is being released to the public. 
        public Boolean classroomUseMode = false;

        public List<Item> WantedItemList = new List<Item>();
        public Dictionary<string, Dictionary<string, StoreItem>> StoreDictionary = new Dictionary<string, Dictionary<string, StoreItem>>();
        public List<Store> StoreList = new List<Store>();
        private Boolean stopAlgorithmEarly = false;
        private System.Timers.Timer timeoutTimer;
        public HashSet<string> storesWithItemsList = new HashSet<string>();

        public const int RUN_OLD = 0;
        public const int RUN_NEW = 1;
        public const int RUN_APPROX = 2;
        public const int RUN_CUSTOM = 3;
        public const int RUN_CUSTOM_APPROX = 4;
        private int whichAlgToRun = 1;
        private Boolean running = false;
        //--------------------------------------------------------------------

        //int storestotal = 0;
        public bool displayreport = false;
        //public int shortcount = 0;
        public long longcount = 0;
        public bool matchesfound = false;
        public int storeprogresscounter = 0;
        public bool calcfail = false;
        string outputfilename = programdata + programname + "-Report.html";
        Object calcLock = new Object();
        public int matchesfoundcount = 0;
        public decimal matchtobeat = 0;

        About aboutWindow = new About();
        CalcOptions calcOptionsWindow = new CalcOptions();
        GetPassword getPasswordWindow = new GetPassword();
        HoverZoom hoverZoomWindow = new HoverZoom();
        AddItem addItemWindow = new AddItem();
        ChangeItem changeItemWindow = new ChangeItem();
        WantedListID wantedListWindow = new WantedListID();
        Brickficiency.ContextMenuStuff.ColourPicker colourPickerWindow = new Brickficiency.ContextMenuStuff.ColourPicker();
        Brickficiency.ContextMenuStuff.MultiplyItems multiplyItemsWindow = new Brickficiency.ContextMenuStuff.MultiplyItems();
        Brickficiency.ContextMenuStuff.DivideItems divideItemsWindow = new Brickficiency.ContextMenuStuff.DivideItems();
        Brickficiency.ContextMenuStuff.AddItems addItemsWindow = new Brickficiency.ContextMenuStuff.AddItems();
        Brickficiency.ContextMenuStuff.SubtractItems subtractItemsWindow = new Brickficiency.ContextMenuStuff.SubtractItems();
        Brickficiency.ContextMenuStuff.SetPrice setPriceWindow = new Brickficiency.ContextMenuStuff.SetPrice();
        Brickficiency.ContextMenuStuff.IncDecPrice incdecPriceWindow = new Brickficiency.ContextMenuStuff.IncDecPrice();
        Brickficiency.ContextMenuStuff.SetComments setCommentsWindow = new Brickficiency.ContextMenuStuff.SetComments();
        Brickficiency.ContextMenuStuff.SetRemarks setRemarksWindow = new Brickficiency.ContextMenuStuff.SetRemarks();
        Brickficiency.ContextMenuStuff.AddComments addCommentsWindow = new Brickficiency.ContextMenuStuff.AddComments();
        Brickficiency.ContextMenuStuff.RemoveComments removeCommentsWindow = new Brickficiency.ContextMenuStuff.RemoveComments();
        Brickficiency.ContextMenuStuff.AddRemarks addRemarksWindow = new Brickficiency.ContextMenuStuff.AddRemarks();
        Brickficiency.ContextMenuStuff.RemoveRemarks removeRemarksWindow = new Brickficiency.ContextMenuStuff.RemoveRemarks();
        #endregion


        #region Startup - read bricklink database files and create appdata folder structure
        private void InitStuff(object sender, EventArgs e) {
            DisableMenu();
            DisableCalcStop();

            // Remove two menu items that are only used when the software is being used for a class.
            customAlgorithmToolStripMenuItem.Visible = classroomUseMode;
            customApproximationAlgorithmToolStripMenuItem.Visible = classroomUseMode;
            oldAlgorithmToolStripMenuItem.Visible = classroomUseMode;

            this.splitContainer.SplitterDistance = System.Convert.ToInt32(this.Size.Height * 0.72);
            //DownloadBrickLinkDB();
            loadWorker.RunWorkerAsync();
        }

        private void loadWorker_DoWork(object sender, DoWorkEventArgs e) {

            #region Create dir structure
            if (!Directory.Exists(programdata)) {
                Directory.CreateDirectory(appdata + "\\" + programname);
                AddStatus("Preparing Brickficiency for first use..." + Environment.NewLine);
            }
            if (!Directory.Exists(programdata + "\\debug")) {
                Directory.CreateDirectory(programdata + "\\debug");
            }

            if (File.Exists(debugwebreqfilename)) {
                File.Delete(debugwebreqfilename);
            }

            List<string> crdirs = new List<string>() { "images", "images\\S", "images\\P", "images\\M", "images\\B", "images\\G", "images\\C", "images\\I", "images\\O", "images\\U" };

            foreach (string dir in crdirs) {
                if (!Directory.Exists(programdata + dir)) {
                    Directory.CreateDirectory(programdata + dir);
                }
            }
            #endregion

            PopulateLookupsFromServices();

            #region countries
            db_countries.Add("Argentina", "AR");
            db_countries.Add("Australia", "AU");
            db_countries.Add("Austria", "AT");
            db_countries.Add("Belarus", "BY");
            db_countries.Add("Belgium", "BE");
            db_countries.Add("Bolivia", "BO");
            db_countries.Add("Bosnia and Herzegovina", "BA");
            db_countries.Add("Brazil", "BR");
            db_countries.Add("Bulgaria", "BG");
            db_countries.Add("Canada", "CA");
            db_countries.Add("Chile", "CL");
            db_countries.Add("China", "CN");
            db_countries.Add("Colombia", "CO");
            db_countries.Add("Croatia", "HR");
            db_countries.Add("Czech Republic", "CZ");
            db_countries.Add("Denmark", "DK");
            db_countries.Add("Ecuador", "EC");
            db_countries.Add("El Salvador", "SV");
            db_countries.Add("Estonia", "EE");
            db_countries.Add("Finland", "FI");
            db_countries.Add("France", "FR");
            db_countries.Add("Germany", "DE");
            db_countries.Add("Greece", "GR");
            db_countries.Add("Hong Kong", "HK");
            db_countries.Add("Hungary", "HU");
            db_countries.Add("India", "IN");
            db_countries.Add("Indonesia", "ID");
            db_countries.Add("Ireland", "IE");
            db_countries.Add("Israel", "IL");
            db_countries.Add("Italy", "IT");
            db_countries.Add("Japan", "JP");
            db_countries.Add("Latvia", "LV");
            db_countries.Add("Lithuania", "LT");
            db_countries.Add("Luxembourg", "LU");
            db_countries.Add("Macau", "MO");
            db_countries.Add("Malaysia", "MY");
            db_countries.Add("Mexico", "MX");
            db_countries.Add("Monaco", "MC");
            db_countries.Add("Netherlands", "NL");
            db_countries.Add("New Zealand", "NZ");
            db_countries.Add("Norway", "NO");
            db_countries.Add("Pakistan", "PK");
            db_countries.Add("Peru", "PE");
            db_countries.Add("Philippines", "PH");
            db_countries.Add("Poland", "PL");
            db_countries.Add("Portugal", "PT");
            db_countries.Add("Romania", "RO");
            db_countries.Add("Russia", "RU");
            db_countries.Add("San Marino", "SM");
            db_countries.Add("Serbia", "RS");
            db_countries.Add("Singapore", "SG");
            db_countries.Add("Slovakia", "SK");
            db_countries.Add("Slovenia", "SI");
            db_countries.Add("South Africa", "ZA");
            db_countries.Add("South Korea", "KR");
            db_countries.Add("Spain", "ES");
            db_countries.Add("Sweden", "SE");
            db_countries.Add("Switzerland", "CH");
            db_countries.Add("Syria", "SY");
            db_countries.Add("Taiwan", "TW");
            db_countries.Add("Thailand", "TH");
            db_countries.Add("Turkey", "TR");
            db_countries.Add("Ukraine", "UA");
            db_countries.Add("United Kingdom", "UK");
            db_countries.Add("USA", "US");
            db_countries.Add("Venezuela", "VE");
            #endregion

            #region LDD colours
            db_LDD2BL.Add("26", "11");
            db_LDD2BL.Add("23", "7");
            db_LDD2BL.Add("37", "36");
            db_LDD2BL.Add("191", "110");
            db_LDD2BL.Add("226", "103");
            db_LDD2BL.Add("221", "104");
            db_LDD2BL.Add("140", "63");
            db_LDD2BL.Add("199", "85");
            db_LDD2BL.Add("308", "120");
            db_LDD2BL.Add("141", "80");
            db_LDD2BL.Add("38", "68");
            db_LDD2BL.Add("268", "89");
            db_LDD2BL.Add("154", "59");
            db_LDD2BL.Add("138", "69");
            db_LDD2BL.Add("18", "28");
            db_LDD2BL.Add("294", "159");
            db_LDD2BL.Add("28", "6");
            db_LDD2BL.Add("212", "62");
            db_LDD2BL.Add("194", "86");
            db_LDD2BL.Add("283", "90");
            db_LDD2BL.Add("222", "56");
            db_LDD2BL.Add("119", "34");
            db_LDD2BL.Add("124", "71");
            db_LDD2BL.Add("102", "42");
            db_LDD2BL.Add("312", "150");
            db_LDD2BL.Add("331", "76");
            db_LDD2BL.Add("330", "155");
            db_LDD2BL.Add("106", "4");
            db_LDD2BL.Add("148", "77");
            db_LDD2BL.Add("297", "115");
            db_LDD2BL.Add("131", "66");
            db_LDD2BL.Add("21", "5");
            db_LDD2BL.Add("192", "88");
            db_LDD2BL.Add("135", "55");
            db_LDD2BL.Add("151", "48");
            db_LDD2BL.Add("5", "2");
            db_LDD2BL.Add("111", "13");
            db_LDD2BL.Add("40", "12");
            db_LDD2BL.Add("48", "20");
            db_LDD2BL.Add("311", "16");
            db_LDD2BL.Add("49", "16");
            db_LDD2BL.Add("47", "18");
            db_LDD2BL.Add("182", "98");
            db_LDD2BL.Add("113", "107");
            db_LDD2BL.Add("126", "51");
            db_LDD2BL.Add("41", "17");
            db_LDD2BL.Add("44", "19");
            db_LDD2BL.Add("208", "49");
            db_LDD2BL.Add("1", "1");
            db_LDD2BL.Add("24", "3");
            db_LDD2BL.Add("43", "14");
            db_LDD2BL.Add("143", "74");
            db_LDD2BL.Add("42", "15");
            #endregion

            #region Settings
            if (!File.Exists(settingsfilename)) {
                settings.countries.Add("All");
                settings.nummatches = 10;
                settings.minstores = 1;
                settings.maxstores = 4;
                settings.cont = false;
                settings.sortcolour = false;
                settings.username = "";
                settings.login = false;
                settings.splitterdistance = System.Convert.ToInt32(this.Size.Height * 0.72);

                WriteSettings();
            } else {
                try {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    FileStream file = new FileStream(settingsfilename, FileMode.Open);
                    settings = (Settings)serializer.Deserialize(file);
                    file.Close();

                    List<string> tmpdelete = new List<string>();
                    foreach (string country in settings.countries) {
                        if ((!db_countries.ContainsKey(country)) && (country != "North America") && (country != "Europe") && (country != "Asia") && (country != "All")) {
                            tmpdelete.Add(country);
                        }
                    }
                    if (tmpdelete.Count > 0) {
                        foreach (string country in tmpdelete) {
                            settings.countries.Remove(country);
                        }
                    }

                    if ((settings.minstores != 1) && (settings.minstores != 2) && (settings.minstores != 3) && (settings.minstores != 4) && (settings.minstores != 5)) {
                        settings.minstores = 1;
                    }

                    if ((settings.maxstores != 1) && (settings.maxstores != 2) && (settings.maxstores != 3) && (settings.maxstores != 4) && (settings.maxstores != 5)) {
                        settings.maxstores = 4;
                    }

                    if ((settings.cont != true) && (settings.cont != false)) {
                        settings.cont = false;
                    }

                    if ((settings.sortcolour != true) && (settings.sortcolour != false)) {
                        settings.sortcolour = false;
                    }

                    if ((settings.login != true) && (settings.login != false)) {
                        settings.login = false;
                    }
                } catch (Exception exc) {
                    AddStatus("Error loading settings... using defaults (" + exc + ")");

                    settings.countries.Add("All");
                    settings.nummatches = 10;
                    settings.minstores = 1;
                    settings.maxstores = 4;
                    settings.cont = false;
                    settings.sortcolour = false;
                    settings.username = "";
                    settings.login = false;
                    settings.blacklist = "";
                    settings.splitterdistance = System.Convert.ToInt32(this.Size.Height * 0.72);
                }
            }

            this.BeginInvoke(new MethodInvoker(delegate() { this.splitContainer.SplitterDistance = settings.splitterdistance; }));
            #endregion

            DownloadBrickLinkDB();

            WriteSettings();
            EnableMenu();
            AddStatus("Done." + Environment.NewLine);
            this.BeginInvoke(new MethodInvoker(delegate() {
                imageTimer.Start();
                itemTimer.Start();
            }));


            #region open file from command line
            List<string> args = Environment.GetCommandLineArgs().ToList();

            if ((args.Count > 1) && (File.Exists(args[1]))) {
                AddStatus("Opening " + args[1] + Environment.NewLine);
                bool success = LoadFile(args[1]);
                if (success) {
                    this.BeginInvoke(new MethodInvoker(delegate() {
                        DisplayLoadedFile();
                    }));
                }
            }
            #endregion
        }

        private void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            //MessageBox.Show("here2");
            //EnableMenu();
        }

        public MainWindow(
            IColorService colorService,
            IItemTypeService itemTypeService,
            ICategoryService categoryService,
            IItemService itemService,
            IBricklinkLoginApi bricklinkLoginApi,
            ImportWantedListForm importWantedListForm,
            UpdateCheck updateConfirmationForm,
            IDataUpdateService dataUpdateService)
        {
            _colorService = colorService;
            _itemTypeService = itemTypeService;
            _categoryService = categoryService;
            _itemService = itemService;
            _dataUpdateService = dataUpdateService;

            _importWantedListForm = importWantedListForm;
            _updateConfirmationForm = updateConfirmationForm;

            // don't really want this referenced here directly, but it will take a bit to decouple this code
            _bricklinkLoginApi = bricklinkLoginApi;

            InitializeComponent();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        }

        private void OnProcessExit(object sender, EventArgs e) {
            WriteSettings();
        }
        #endregion

        private void BuildTable()
        {
            dt.Clear();
            dt.Add(new DataTable());
            dt[currenttab].Columns.Add("status", typeof(string));
            dt[currenttab].Columns.Add("number", typeof(string));
            dt[currenttab].Columns.Add("name", typeof(string));
            dt[currenttab].Columns.Add("condition", typeof(string));
            dt[currenttab].Columns.Add("colourname", typeof(string));
            dt[currenttab].Columns.Add("qty", typeof(int));
//            dt[currenttab].Columns.Add("availability", typeof(int));
dt[currenttab].Columns.Add("availstores", typeof(int));
            dt[currenttab].Columns.Add("price", typeof(decimal));
            dt[currenttab].Columns.Add("total", typeof(decimal));
            dt[currenttab].Columns.Add("comments", typeof(string));
            dt[currenttab].Columns.Add("remarks", typeof(string));
            dt[currenttab].Columns.Add("categoryname", typeof(string));
            dt[currenttab].Columns.Add("typename", typeof(string));
            dt[currenttab].Columns.Add("origqty", typeof(int));
            dt[currenttab].Columns.Add("origprice", typeof(decimal));
            dt[currenttab].Columns.Add("id", typeof(string));
            dt[currenttab].Columns.Add("extid", typeof(string));
            dt[currenttab].Columns.Add("type", typeof(string));
            dt[currenttab].Columns.Add("colour", typeof(string));
            dt[currenttab].Columns.Add("categoryid", typeof(string));

            dt[currenttab].Columns.Add("availqty", typeof(int));
            dt[currenttab].Columns.Add("imageurl", typeof(string));
            dt[currenttab].Columns.Add("largeimageurl", typeof(string));
            dt[currenttab].Columns.Add("imageloaded", typeof(string));
            dt[currenttab].Columns.Add("pgpage", typeof(string));

            dt[currenttab].PrimaryKey = new[] { dt[currenttab].Columns["extid"] };
        }

        private void FillRow(DataRow dr, Item item)
        {
            dr["status"] = item.status;
            dr["number"] = item.number;
            dr["name"] = db_blitems[item.id].name;
            dr["condition"] = item.condition;
            dr["colourname"] = db_colours[item.colour].name;
            dr["qty"] = item.qty;
            dr["availstores"] = item.availstores;
            dr["price"] = item.price;
            dr["comments"] = item.comments;
            dr["remarks"] = item.remarks;
            dr["categoryname"] = db_categories[item.categoryid].name;
            dr["typename"] = db_typenames[item.type];
            dr["origqty"] = item.origqty;
            dr["origprice"] = item.origprice;
            dr["id"] = item.id;
            dr["extid"] = item.extid;
            dr["type"] = item.type;
            dr["colour"] = item.colour;
            dr["categoryid"] = item.categoryid;
            dr["imageurl"] = item.imageurl;
            dr["largeimageurl"] = item.largeimageurl;
            dr["imageloaded"] = "n";
        }
 
        #region Load a file
        private bool LoadFile(string filename) {
            foreach (DataTable thisdt in dt) {
                thisdt.Dispose();
            }

            BuildTable();
            List<Item> items = new List<Item>();

            string rawfile;

            StreamWriter swr = new StreamWriter(debugopenfilename);

            try {
                using (StreamReader sr = new StreamReader(filename)) {
                    rawfile = sr.ReadToEnd();
                    swr.Write(rawfile + Environment.NewLine);
                }
            } catch (Exception exc) {
                swr.Write("ERROR: Could not read the file " + filename);
                swr.Close();
                MessageBox.Show("The file could not be read:" + Environment.NewLine + exc.Message);
                return false;
            }

            Match validcheck = Regex.Match(rawfile, @"\<inventory\>", RegexOptions.IgnoreCase);
            if (validcheck.Success) {
                swr.Write("recognized valid file" + Environment.NewLine);
            } else {
                swr.Write("ERROR: unrecognized file" + Environment.NewLine + Environment.NewLine);
                swr.Close();
                MessageBox.Show("File format not recognized");
                return false;
            }

            List<String> lines = rawfile.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            int tmpcount = -1;
            foreach (string line in lines) {
                swr.Write(line + Environment.NewLine);

                Match newitem = Regex.Match(line, @"\<item\>", RegexOptions.IgnoreCase);
                if (newitem.Success) {
                    swr.Write("Beginning of new item found" + Environment.NewLine + Environment.NewLine);
                    tmpcount = tmpcount + 1;
                    items.Add(new Item());
                    continue;
                }

                Match number = Regex.Match(line, @"\<itemid\>(.*)\</itemid\>", RegexOptions.IgnoreCase);
                if (number.Success) {
                    swr.Write("Item ID found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].number = number.Groups[1].Value;
                    continue;
                }

                Match typeid = Regex.Match(line, @"\<itemtypeid\>(.)\</itemtypeid\>", RegexOptions.IgnoreCase);
                if (typeid.Success) {
                    swr.Write("Item type found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].type = typeid.Groups[1].Value;
                    continue;
                }

                Match type = Regex.Match(line, @"\<itemtype\>(.)\</itemtype\>", RegexOptions.IgnoreCase);
                if (type.Success) {
                    swr.Write("Item type found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].type = type.Groups[1].Value;
                    continue;
                }

                Match colourid = Regex.Match(line, @"\<colorid\>(.*)\</colorid\>", RegexOptions.IgnoreCase);
                if (colourid.Success) {
                    swr.Write("Item colour found" + Environment.NewLine);
                    items[tmpcount].colour = colourid.Groups[1].Value;
                    swr.Write("Item colour is " + db_colours[items[tmpcount].colour] + Environment.NewLine + Environment.NewLine);
                    continue;
                }

                Match colour = Regex.Match(line, @"\<color\>(.*)\</color\>", RegexOptions.IgnoreCase);
                if (colour.Success) {
                    swr.Write("Item colour found" + Environment.NewLine);
                    items[tmpcount].colour = colour.Groups[1].Value;
                    swr.Write("Item colour is " + db_colours[items[tmpcount].colour] + Environment.NewLine + Environment.NewLine);
                    continue;
                }

                Match status = Regex.Match(line, @"\<status\>(.*)\</status\>", RegexOptions.IgnoreCase);
                if (status.Success) {
                    swr.Write("Item status found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].status = status.Groups[1].Value;
                    continue;
                }

                Match qty = Regex.Match(line, @"\<qty\>(.*)\</qty\>", RegexOptions.IgnoreCase);
                if (qty.Success) {
                    swr.Write("Item quantity found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].qty = Convert.ToInt32(qty.Groups[1].Value);
                    continue;
                }

                Match minqty = Regex.Match(line, @"\<minqty\>(.*)\</minqty\>", RegexOptions.IgnoreCase);
                if (minqty.Success) {
                    swr.Write("Item minimum quantity found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].qty = Convert.ToInt32(minqty.Groups[1].Value);
                    continue;
                }

                Match price = Regex.Match(line, @"\<price\>(.*)\</price\>", RegexOptions.IgnoreCase);
                if (price.Success) {
                    swr.Write("Item price found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].price = ConvertToDecimal(price.Groups[1].Value);
                    continue;
                }

                Match condition = Regex.Match(line, @"\<condition\>(.*)\</condition\>", RegexOptions.IgnoreCase);
                if (condition.Success) {
                    swr.Write("Item condition found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].condition = condition.Groups[1].Value;
                    continue;
                }

                Match comments = Regex.Match(line, @"\<comments\>(.*)\</comments\>", RegexOptions.IgnoreCase);
                if (comments.Success) {
                    swr.Write("Item comments found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].comments = comments.Groups[1].Value;
                    items[tmpcount].comments = StringLoadMod(items[tmpcount].comments);
                    continue;
                }

                Match remarks = Regex.Match(line, @"\<remarks\>(.*)\</remarks\>", RegexOptions.IgnoreCase);
                if (remarks.Success) {
                    swr.Write("Item remarks found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].remarks = remarks.Groups[1].Value;
                    items[tmpcount].remarks = StringLoadMod(items[tmpcount].remarks);
                    continue;
                }

                Match origprice = Regex.Match(line, @"\<origprice\>(.*)\</origprice\>", RegexOptions.IgnoreCase);
                if (origprice.Success) {
                    swr.Write("Item original price found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].origprice = ConvertToDecimal(origprice.Groups[1].Value);
                    continue;
                }

                Match origqty = Regex.Match(line, @"\<origqty\>(.*)\</origqty\>", RegexOptions.IgnoreCase);
                if (origqty.Success) {
                    swr.Write("Item original quantity found" + Environment.NewLine + Environment.NewLine);
                    items[tmpcount].origqty = Convert.ToInt32(origqty.Groups[1].Value);
                    continue;
                }
                swr.Write("WARNING: Nothing found" + Environment.NewLine + Environment.NewLine);
            }

            foreach (Item item in items)
            {
                item.type = item.type.ToUpper();
                item.id = item.type + "-" + item.number;
                item.extid = item.type + "-" + item.colour + "-" + item.number;
                if (db_blitems.ContainsKey(item.id))
                {
                    item.categoryid = db_blitems[item.id].catid;
                }
                else
                {
                    swr.Write("ERROR: Item not found in database: " + Environment.NewLine +
                        "Type: " + item.type + Environment.NewLine +
                        "ID: " + item.number + Environment.NewLine +
                        "Colour: " + item.colour + Environment.NewLine + Environment.NewLine);
                    swr.Close();
                    MessageBox.Show("ERROR: Item not found in database: " + Environment.NewLine +
                        "Type: " + item.type + Environment.NewLine +
                        "ID: " + item.number + Environment.NewLine +
                        "Colour: " + item.colour + Environment.NewLine + Environment.NewLine);
                    return false;
                }
                item.imageurl = GenerateImageURL(item.id, item.colour);
                item.largeimageurl = GenerateImageURL(item.id);

                if ((item.status != "X") && (item.status != "E") && (item.status != "I"))
                    item.status = "I";

                if ((item.type == null) || (item.number == null) || (item.colour == null))
                {
                    swr.Write("ERROR: Missing information about item: " + Environment.NewLine +
                        "Type: " + item.type + Environment.NewLine +
                        "ID: " + item.number + Environment.NewLine +
                        "Colour: " + item.colour + Environment.NewLine + Environment.NewLine);
                    swr.Close();
                    MessageBox.Show("ERROR: Missing information about item: " + Environment.NewLine +
                        "Type: " + item.type + Environment.NewLine +
                        "ID: " + item.number + Environment.NewLine +
                        "Colour: " + item.colour + Environment.NewLine + Environment.NewLine);
                    return false;
                }


                if (!dt[currenttab].Rows.Contains(item.extid)) {
                    DataRow dr = dt[currenttab].NewRow();
                    FillRow(dr, item);
                    dt[currenttab].Rows.Add(dr);
               }
                else {
                    dt[currenttab].Rows.Find(item.extid)["qty"] = (int)dt[currenttab].Rows.Find(item.extid)["qty"] + item.qty;
                }
            }

            swr.Close();
            return true;
        }
        #endregion

        #region Display File once loaded
        public void DisplayLoadedFile() {
            foreach (DataGridView thisdgv in dgv) {
                thisdgv.Dispose();
            }
            dgv.Clear();

            dgv.Add(new DataGridView());
            dgv[currenttab].DataSource = dt[currenttab];
            dgv[currenttab].Dock = System.Windows.Forms.DockStyle.Fill;
            dgv[currenttab].SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv[currenttab].AllowUserToAddRows = false;
            dgv[currenttab].Name = openFileDialog.FileName;
            dgv[currenttab].RowTemplate.Height = 30;
            dgv[currenttab].RowHeadersVisible = false;
            dgv[currenttab].AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv[currenttab].AllowUserToResizeRows = false;
            dgv[currenttab].BackgroundColor = SystemColors.Control;
            //files[currenttab].CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(CellClick);
            dgv[currenttab].MouseUp += new System.Windows.Forms.MouseEventHandler(dgv_Mouseup);
            dgv[currenttab].CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_doubleclick);
            dgv[currenttab].Sorted += new System.EventHandler(dgvSorted);
            dgv[currenttab].CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvCellEdit);
            dgv[currenttab].CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(dgvCellVal);
            dgv[currenttab].EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvEditingControlShowing);
            dgv[currenttab].DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(dgvDataError);
            //dgv[currenttab].ContextMenuStrip = this.contextMenuStrip1;
            dgv[currenttab].MouseDown += new System.Windows.Forms.MouseEventHandler(dgv_MouseDown);
            dgv[currenttab].MouseUp += new System.Windows.Forms.MouseEventHandler(dgv_MouseUp);
            dgv[currenttab].MouseMove += new System.Windows.Forms.MouseEventHandler(dgv_MouseMove);
            dgv[currenttab].CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_CellMouseEnter);
            dgv[currenttab].CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_CellMouseLeave);
            dgv[currenttab].CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgv_CellMouseMove);
            dgv[currenttab].SelectionChanged += new EventHandler(dgv_selectchange);


            DataGridViewImageColumn displayStatusCol = new DataGridViewImageColumn();
            displayStatusCol.Name = "displaystatus";
            displayStatusCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgv[currenttab].Columns.Insert(0, displayStatusCol);
            DataGridViewImageColumn displayImageCol = new DataGridViewImageColumn();
            displayImageCol.Name = "displayimage";
            displayImageCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dgv[currenttab].Columns.Insert(1, displayImageCol);

            splitContainer.Panel1.Controls.Add(dgv[currenttab]);

            dgv[currenttab].Columns["displaystatus"].HeaderText = "St";
            dgv[currenttab].Columns["displaystatus"].Width = 20;
            dgv[currenttab].Columns["displaystatus"].Resizable = DataGridViewTriState.False;
            dgv[currenttab].Columns["displaystatus"].ReadOnly = true;
            dgv[currenttab].Columns["displaystatus"].DefaultCellStyle.NullValue = Properties.Resources.blank;

            dgv[currenttab].Columns["displayimage"].HeaderText = "Image";
            dgv[currenttab].Columns["displayimage"].Width = 40;
            dgv[currenttab].Columns["displayimage"].Resizable = DataGridViewTriState.False;
            dgv[currenttab].Columns["displayimage"].ReadOnly = true;
            dgv[currenttab].Columns["displayimage"].DefaultCellStyle.NullValue = Properties.Resources.blank;

            dgv[currenttab].Columns["number"].HeaderText = "Part #";
            dgv[currenttab].Columns["number"].Width = 90;
            //dgv[currenttab].Columns["number"].ReadOnly = true;

            dgv[currenttab].Columns["name"].HeaderText = "Description";
            dgv[currenttab].Columns["name"].Width = 300;
            dgv[currenttab].Columns["name"].ReadOnly = true;

            dgv[currenttab].Columns["condition"].HeaderText = "Cond";
            dgv[currenttab].Columns["condition"].Width = 40;
            dgv[currenttab].Columns["condition"].ReadOnly = true;

            dgv[currenttab].Columns["colourname"].HeaderText = "Colour";
            dgv[currenttab].Columns["colourname"].Width = 150;
            dgv[currenttab].Columns["colourname"].ReadOnly = true;

            dgv[currenttab].Columns["qty"].HeaderText = "Qty";
            dgv[currenttab].Columns["qty"].Width = 60;
            //dgv[currenttab].Columns["qty"].ReadOnly = true;

dgv[currenttab].Columns["availstores"].HeaderText = "Availstores";
dgv[currenttab].Columns["availstores"].Width = 80;
dgv[currenttab].Columns["availstores"].DefaultCellStyle.Format = "#;Unknown;#";
dgv[currenttab].Columns["availstores"].ReadOnly = true;

            dgv[currenttab].Columns["price"].HeaderText = "Price";
            dgv[currenttab].Columns["price"].Width = 75;
            dgv[currenttab].Columns["price"].DefaultCellStyle.Format = "N3";
            //dgv[currenttab].Columns["price"].ReadOnly = true;

            dgv[currenttab].Columns["total"].HeaderText = "Total";
            dgv[currenttab].Columns["total"].Width = 75;
            dgv[currenttab].Columns["total"].ReadOnly = true;
            dgv[currenttab].Columns["total"].DefaultCellStyle.Format = "N2";

            dgv[currenttab].Columns["comments"].HeaderText = "Comments";
            dgv[currenttab].Columns["comments"].Width = 150;
            //dgv[currenttab].Columns["comments"].ReadOnly = true;

            dgv[currenttab].Columns["remarks"].HeaderText = "Remarks";
            dgv[currenttab].Columns["remarks"].Width = 150;
            //dgv[currenttab].Columns["remarks"].ReadOnly = true;

            dgv[currenttab].Columns["categoryname"].HeaderText = "Category";
            dgv[currenttab].Columns["categoryname"].Width = 125;
            dgv[currenttab].Columns["categoryname"].ReadOnly = true;

            dgv[currenttab].Columns["typename"].HeaderText = "Item Type";
            dgv[currenttab].Columns["typename"].Width = 75;
            dgv[currenttab].Columns["typename"].ReadOnly = true;

            dgv[currenttab].Columns["origqty"].Visible = false;
            dgv[currenttab].Columns["origprice"].Visible = false;
            dgv[currenttab].Columns["status"].Visible = false;
            dgv[currenttab].Columns["id"].Visible = false;
            dgv[currenttab].Columns["extid"].Visible = false;
            dgv[currenttab].Columns["type"].Visible = false;
            dgv[currenttab].Columns["colour"].Visible = false;
            dgv[currenttab].Columns["categoryid"].Visible = false;
//            dgv[currenttab].Columns["availstores"].Visible = false;
            dgv[currenttab].Columns["availqty"].Visible = false;
            dgv[currenttab].Columns["imageurl"].Visible = false;
            dgv[currenttab].Columns["largeimageurl"].Visible = false;
            dgv[currenttab].Columns["imageloaded"].Visible = false;
            dgv[currenttab].Columns["pgpage"].Visible = false;

            dgvSorted(new object(), new EventArgs());

            foreach (DataGridViewRow item in dgv[currenttab].Rows) {
                item.Cells["total"].Value = (Decimal)item.Cells["price"].Value * (int)item.Cells["qty"].Value;
                if ((int)item.Cells["qty"].Value == 0) {
                    item.Cells["qty"].Style.BackColor = errorcell;
                } else {
                    item.Cells["qty"].Style.BackColor = item.Cells["status"].Style.BackColor;
                }

                if ((String)item.Cells["status"].Value == "X") {
                    item.Cells["displaystatus"].Value = Properties.Resources.x;
                } else if ((String)item.Cells["status"].Value == "E") {
                    item.Cells["displaystatus"].Value = Properties.Resources.add;
                } else {
                    item.Cells["displaystatus"].Value = Properties.Resources.check;
                }

                dgv_GetLiveStats((string)item.Cells["id"].Value, (string)item.Cells["colour"].Value);
                dgv_ImageDisplay((string)item.Cells["id"].Value, (string)item.Cells["colour"].Value);
            }

            if (dgv[currenttab].Rows.Count != 0) {
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["number"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["name"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["colourname"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["qty"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["price"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["total"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["comments"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["remarks"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["categoryname"].Index);
                dgv[currenttab].AutoResizeColumn(dgv[currenttab].Columns["typename"].Index);
            }
        }
        #endregion

        #region image loading timer tick
        private void imageTimerNew_Tick(object sender, EventArgs e) {
            if (imagetimercount > 4) { return; }

            imagetimercount++;

            ImageDL thisimage = new ImageDL();

            lock (imgTimerLock) {
                if (imageDLList.Count == 0) {
                    imagetimercount--;
                    return;
                }

                thisimage = imageDLList.First();
                imageDLList.RemoveAt(0);
            }

            string folder = Path.GetDirectoryName(thisimage.file);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(thisimage.file)) {
                Bitmap dlImage = GetImage(thisimage.url);
                if (dlImage == null) {
                    thisimage.url = thisimage.url.Replace(".jpg", ".gif");
                    dlImage = GetImage(thisimage.url);
                }
                if (dlImage != null) {
                    dlImage.Save(thisimage.file);
                }
            }

            if (thisimage.type == "s") {
                foreach (DataGridViewRow item in dgv[currenttab].Rows) {
                    if ((String)item.Cells["extid"].Value == thisimage.extid) {
                        if ((String)item.Cells["imageloaded"].Value == "l") {
                            continue;
                        } else {
                            item.Cells["imageloaded"].Value = "l";
                        }

                        if (File.Exists(thisimage.file)) {
                            item.Cells["displayimage"].Value = Image.FromFile(thisimage.file);
                        } else {
                            item.Cells["displayimage"].Value = blank;
                        }
                    }
                }
            } else if (thisimage.type == "l") {
                if (File.Exists(thisimage.file)) {
                    Image backimage = Image.FromFile(thisimage.file);
                    hoverZoomWindow.BackgroundImage = backimage;
                    hoverZoomWindow.Width = backimage.Width;
                    hoverZoomWindow.Height = backimage.Height;
                    hoverZoomWindow.HideLabel();
                } else {
                    hoverZoomWindow.BackgroundImage = null;
                    hoverZoomWindow.Width = 100;
                    hoverZoomWindow.Height = 40;
                    hoverZoomWindow.NotFound();
                }
            }

            //dgv_selectchange(new object(), new EventArgs());

            imagetimercount--;
        }
        #endregion

        #region item loading timer tick
        private void itemTimerNew_Tick(object sender, EventArgs e) {
            if (itemtimercount > 0) { return; }

            itemtimercount++;

            ItemDL thisItem;

            lock (itemTimerLock) {
                if (itemDLList.Count == 0) {
                    itemtimercount--;
                    return;
                }

                thisItem = itemDLList.First();
                itemDLList.RemoveAt(0);
            }


            Item item = thisItem.item;
            DataTable table = dt[currenttab];

            // for some reason 'table.Rows.Find(item.extid)' does not work.
            if (!table.Rows.Contains(item.extid))
            {
                int smeg = 0;
            }

            string key = table.PrimaryKey[0].ColumnName;
            foreach (DataRow row in table.Rows)
            {
                if (row[key].Equals(item.extid))
                {
                    object smeg = row["availstores"];
                    if (row["availstores"].Equals(-1))
                    {
                        GetPriceGuideForItem(item, true);
                        row["availstores"] = item.availstores;
                    }
                }
            }

            itemtimercount--;
        }
        #endregion

        public static void dgv_GetLiveStats(string id, string colour = "0")
        {
            lock (itemTimerLock)
            {
                itemDLList.Add(new ItemDL() {
                    item = new Item() {
                        id = id,
                        extid = db_blitems[id].type + "-" + colour + "-" + db_blitems[id].number,
                        type = db_blitems[id].type,
                        number = db_blitems[id].number,
                        colour = colour
                    }
                });
            }
        }

        public static void dgv_ImageDisplay(DataGridViewRow theRow)
        {
            dgv_ImageDisplay(theRow.Cells["id"].Value.ToString(), theRow.Cells["colour"].Value.ToString());
        }

        #region display image on dgv
        public static void dgv_ImageDisplay(string id, string colour = "0") {
            string imgfilename = GenerateImageFilename(id, colour);

            if (File.Exists(imgfilename)) {
                foreach (DataGridViewRow row in dgv[currenttab].Rows) {
                    if (((string)row.Cells["id"].Value == id) && ((string)row.Cells["colour"].Value == colour)) {
                        row.Cells["displayimage"].Value = Image.FromFile(imgfilename);
                    }
                }
            } else {
                lock (imgTimerLock) {
                    imageDLList.Add(new ImageDL() {
                        extid = db_blitems[id].type + "-" + colour + "-" + db_blitems[id].number,
                        file = imgfilename,
                        url = GenerateImageURL(id, colour),
                        type = "s"
                    });
                }
            }
        }
        #endregion

        #region Import LDD file
        private void importWorker_DoWork(object sender, DoWorkEventArgs e) {
            string filename = (string)e.Argument;
            if (importLDD(filename)) {
                this.BeginInvoke(new MethodInvoker(delegate() {
                    DisplayLoadedFile();
                    EnableMenu();
                }));
            }
        }

        private bool importLDD(string filename) {
            StreamWriter swr = new StreamWriter(debuglddimport);

            using (ZipFile zip = ZipFile.Read(filename)) {
                bool extracted = false;
                string lddfile;
                string xmlfile = programdata + "IMAGE100.LXFML";
                Dictionary<string, LDDItem> extracteditems = new Dictionary<string, LDDItem>();

                foreach (ZipEntry e in zip) {
                    if (e.FileName == "IMAGE100.LXFML") {
                        e.Extract(programdata, ExtractExistingFileAction.OverwriteSilently);
                        extracted = true;
                        swr.WriteLine("IMAGE100.LXFML extracted to " + programdata);
                    } else {
                        swr.WriteLine("Skipping " + e.FileName);
                    }
                }

                if (extracted == false) {
                    AddStatus("Error extracting xml from lxf");
                    swr.WriteLine("Error extracting xml from lxf");
                    swr.Close();
                    EnableMenu();
                    return false;
                }

                try {
                    using (StreamReader sr = new StreamReader(xmlfile)) {
                        lddfile = sr.ReadToEnd();
                    }
                } catch (Exception exc) {
                    AddStatus("Error reading xml file");
                    swr.WriteLine("Error reading xml file: " + exc.Message);
                    swr.Close();
                    EnableMenu();
                    return false;
                }

                foreach (DataTable thisdt in dt) {
                    thisdt.Dispose();
                }

                BuildTable();
//                List<Item> items = new List<Item>();

                List<String> lines = lddfile.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                swr.WriteLine("");
                string brick = "";
                bool partfound = false;

                foreach (string line in lines) {
                    swr.WriteLine(line);
                    Match brickmatch = Regex.Match(line, "<Brick refID=\"" + @".*?" + "\" designID=\"" + @"(.*?)" + "\"", RegexOptions.IgnoreCase);
                    if (brickmatch.Success) {
                        brick = brickmatch.Groups[1].ToString();
                        partfound = false;
                        swr.WriteLine("brick: " + brick);
                        continue;
                    }

                    if (partfound == true) {
                        continue;
                    }

                    //                                 <Part refID= "      11       " designID= "       32062      " materials= "       21
                    Match ldditem = Regex.Match(line, "<Part refID=\"" + @".*?" + "\" designID=\"" + @"(.*?)" + "\" materials=\"" + @"(\d*)", RegexOptions.IgnoreCase);
                    if (ldditem.Success) {
                        partfound = true;
                        LDDItem item = new LDDItem();

                        item.num = ldditem.Groups[1].ToString();

                        if (item.num != brick) {
                            swr.WriteLine("part of multi-part item: " + brick);
                            item.num = brick;
                        }

                        item.col = ldditem.Groups[2].ToString();
                        item.id = item.col + "-" + item.num;
                        item.count = 1;
                        if (db_LDD2BL.ContainsKey(item.col)) {
                            item.blcol = db_LDD2BL[item.col];
                        } else {
                            swr.WriteLine("Unknown Lego colour id: " + item.col + Environment.NewLine);
                            AddStatus("Skipping Lego Element ID " + item.num + ". Unknown colour ID: " + item.col + Environment.NewLine);
                            continue;
                        }

                        swr.WriteLine("num: " + item.num + Environment.NewLine +
                            "col: " + item.col + Environment.NewLine);

                        if (extracteditems.ContainsKey(item.id)) {
                            extracteditems[item.id].count++;
                        } else {
                            extracteditems.Add(item.id, item);
                        }
                    }
                }

                AddStatus("##Clear##");
                AddStatus("Querying Rebrickable.com for Bricklink part ID's" + Environment.NewLine);
                swr.WriteLine(Environment.NewLine + "Querying Rebrickable.com for Bricklink part ID's");

                foreach (LDDItem item in extracteditems.Values) {
                    swr.WriteLine(Environment.NewLine + "ID: " + item.num);

                    //Lego and Bricklink are the same
                    if (db_blitems.ContainsKey("P-" + item.num)) {
                        swr.WriteLine("Valid BrickLink ID number");
                        item.blnum = db_blitems["P-" + item.num].number;
                    }
                        //Found Alt from Bricklink database
                    else if (db_blaltids.ContainsKey(item.num)) {
                        swr.WriteLine("Found BrickLink Alternate ID number from BrickLink");
                        item.blnum = db_blaltids[item.num];
                    }
                        //Found Alt from Rebrickable Database
                    else if (db_rebrickaltids.ContainsKey(item.num)) {
                        swr.WriteLine("Found BrickLink Alternate ID number from Rebrickable.com");
                        item.blnum = db_rebrickaltids[item.num];
                    }
                        //haven't checked rebrickable yet
                    else if (!db_rebrickpages.ContainsKey(item.num)) {
                        swr.WriteLine("Checking Rebrickable...");
                        string url = "http://rebrickable.com/api/get_part" + @"?key=" + RBapiKey + "&part_id=" + item.num + "&inc_ext=1&format=json";
                        string page = GetRebrickablePage(url);

                        if (page == null) {
                            AddStatus("Rebrickable error retrieving part ID for part: " + item.num + Environment.NewLine);
                            swr.WriteLine("Error downloading from rebrickable api");
                            continue;
                        } else {
                            db_rebrickpages.Add(item.num, page);
                            swr.WriteLine(page);
                        }

                        Match itemmatch = Regex.Match(page, "{\"bricklink\":\"" + "(.*?)" + "\"}");
                        Match rbmatch = Regex.Match(page, "\"rebrickable_part_id\":\"" + "(.*?)" + "\"");
                        if (rbmatch.Success) {
                            //found alt from rebrickable
                            if (db_blitems.ContainsKey("P-" + rbmatch.Groups[1].Value.ToString())) {
                                swr.WriteLine("Found BrickLink Alternate ID number from Rebrickable.com");
                                db_rebrickaltids.Add(item.num, rbmatch.Groups[1].Value.ToString());
                                item.blnum = rbmatch.Groups[1].Value.ToString();
                            }
                                //need to check BL alt id from rebrickable
                            else {
                                url = "http://rebrickable.com/api/get_part" + @"?key=" + RBapiKey + "&part_id=" + rbmatch.Groups[1].Value.ToString() + "&inc_ext=1&format=json";
                                page = GetRebrickablePage(url);

                                if (page == null) {
                                    AddStatus("Rebrickable error retrieving part ID for part: " + rbmatch.Groups[1].Value.ToString() + Environment.NewLine);
                                    swr.WriteLine("Error downloading from rebrickable api");
                                    continue;
                                } else {
                                    db_rebrickpages.Add(rbmatch.Groups[1].Value.ToString(), page);
                                    swr.WriteLine(page);
                                }

                                Match itemmatch2 = Regex.Match(page, "{\"bricklink\":\"" + "(.*?)" + "\"}");
                                if (itemmatch2.Success) {
                                    //found alt from rebrickable
                                    if (db_blitems.ContainsKey("P-" + itemmatch2.Groups[1].Value.ToString())) {
                                        swr.WriteLine("Found BrickLink Alternate ID number from Rebrickable.com");
                                        item.blnum = itemmatch2.Groups[1].Value.ToString();
                                        db_rebrickaltids.Add(item.num, itemmatch2.Groups[1].Value.ToString());
                                    }
                                }
                            }
                        } else if (itemmatch.Success) {
                            //found alt from rebrickable
                            if (db_blitems.ContainsKey("P-" + itemmatch.Groups[1].Value.ToString())) {
                                swr.WriteLine("Found BrickLink Alternate ID number from Rebrickable.com");
                                item.blnum = itemmatch.Groups[1].Value.ToString();
                                db_rebrickaltids.Add(item.num, itemmatch.Groups[1].Value.ToString());
                            }
                        }
                    }

                    if (item.blnum == "") {
                        AddStatus("Error retrieving Bricklink part ID: " + item.num + Environment.NewLine +
                            "Info for adding manually: " + item.count + "x " + db_colours[item.blcol].name + " " + item.num + Environment.NewLine);
                        swr.WriteLine("Error: could not find a proper ID from bricklink or rebrickable");
                        continue;
                    } else {
                        swr.WriteLine("Found: " + item.blnum + Environment.NewLine);
                    }

                    string extid = "P-" + item.blcol + "-" + item.blnum;
                    if (!dt[currenttab].Rows.Contains(extid)) {
                        DataRow dr = dt[currenttab].NewRow();
                        dr["id"] = "P-" + item.blnum;
                        dr["colour"] = item.blcol;
                        dr["number"] = item.blnum;
                        dr["extid"] = extid;
                        dr["categoryid"] = db_blitems["P-" + item.blnum].catid;
                        dr["name"] = db_blitems["P-" + item.blnum].name;
                        dr["colourname"] = db_colours[item.blcol].name;
                        dr["qty"] = item.count;
                        dr["categoryname"] = db_categories[db_blitems["P-" + item.blnum].catid].name;
                        dr["imageurl"] = GenerateImageURL("P-" + item.blnum, item.blcol);
                        dr["largeimageurl"] = GenerateImageURL("P-" + item.blnum);
                        dr["type"] = "P";
                        dr["typename"] = db_typenames["P"];
                        dr["status"] = "I";
                        dr["condition"] = "U";
                        dr["availstores"] = -1;
                        dr["price"] = "0";
                        dr["comments"] = "";
                        dr["remarks"] = "";
                        dr["origqty"] = 0;
                        dr["origprice"] = 0;
                        dr["imageloaded"] = "n";
                        dt[currenttab].Rows.Add(dr);
                    }
                    else {
                        Debug.Assert(false, "Duplicate item in DLL file, how strange???");
                        dt[currenttab].Rows.Find(extid)["qty"] = (int)dt[currenttab].Rows.Find(extid)["qty"] + item.count;
                    }
                }
            }

            AddStatus("Done." + Environment.NewLine);
            swr.Close();
            return true;
        }
        #endregion

        private static int MAX_FAILS = 4;
        #region retrieve page as string
        public string GetPage(string url, bool login = false) {
            lock (pageLock)
            {
                string cookieHeader;
                string pageSource;
                //            StreamWriter swr = new StreamWriter(debugwebreqfilename);

                if ((login == true) && ((loggedin == false) || (cookietime == null) || (DateTime.Now > cookietime.AddMinutes(10))))
                {
                    DialogResult result = getPasswordWindow.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        password = "";
                        return null;
                    }

                    // don't really want this referenced here directly, but it will take a bit to decouple this code
                    var didLogIn = _bricklinkLoginApi.Login(cookies, settings.username, password);

                    if (!didLogIn)
                    {
                        AddStatus("Unable to authenticate to Bricklink.  Check your username and password." + Environment.NewLine);
                        return null;
                    }
                    else
                    {
                        cookietime = DateTime.Now;
                        loggedin = true;
                    }
                }
                else if ((cookietime == null) || (DateTime.Now > cookietime.AddMinutes(10)))
                {
                    int pagefail = 0;
                    bool pagesuccess = false;

                    while ((pagesuccess == false) && (pagefail < MAX_FAILS))
                    {
                        try
                        {
                            string tmpcookieurl = "https://www.bricklink.com/catalogPG.asp?P=3001&colorID=48";
                            HttpWebRequest tmpreq = (HttpWebRequest)WebRequest.Create(tmpcookieurl);
                            tmpreq.Timeout = 15000;
                            tmpreq.CookieContainer = cookies;
                            tmpreq.ContentType = "application/x-www-form-urlencoded";
                            tmpreq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0";
                            HttpWebResponse tmpresp = (HttpWebResponse)tmpreq.GetResponse();
                            cookieHeader = tmpresp.Headers["Set-cookie"];
                            cookies.Add(tmpresp.Cookies);
                            cookietime = DateTime.Now;
                            pagesuccess = true;
                        }
                        catch (Exception)
                        {
                            AddStatus("Retrying..." + Environment.NewLine);
                            pagefail++;
                            //                        swr.Write(DateTime.Now.ToString() + ": " + ex + Environment.NewLine);
                        }
                    }

                    if (pagesuccess == false)
                    {
                        //                    swr.Close();
                        return "##PageFail##";
                    }
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 15000;
                req.CookieContainer = cookies;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";

                int pagefail2 = 0;
                bool pagesuccess2 = false;
                while ((pagesuccess2 == false) && (pagefail2 < MAX_FAILS))
                {
                    try
                    {
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            pageSource = sr.ReadToEnd();
                        }

                        cookietime = DateTime.Now;
                        //                    swr.Close();
                        return pageSource;
                    }
                    catch (Exception)
                    {
                        AddStatus("Retrying..." + Environment.NewLine);
                        pagefail2++;
                        //                    swr.Write(DateTime.Now.ToString() + ": " + ex + Environment.NewLine);
                    }
                }

                //           swr.Close();
                return "##PageFail##";
            }   // Release Lock
        }
        #endregion

        #region retrieve rebrickable page as string
        private string GetRebrickablePage(string url) {
            int pagefail = 0;
            bool pagesuccess = false;
            string page = null;

            while ((pagesuccess == false) && (pagefail < 4)) {
                try {
                    WebClient wc = new WebClient();
                    wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705;)");
                    page = wc.DownloadString(url);
                    pagesuccess = true;
                } catch// (Exception ex)
                {
                    pagefail++;
                }
            }

            return page;
        }
        #endregion

        #region retrieve image as bitmap
        static Bitmap GetImage(string url) {
            WebClient wc = new WebClient();
            try {
                byte[] bytes = wc.DownloadData(url);
                MemoryStream stream = new MemoryStream(bytes);
                Image img = Bitmap.FromStream(stream);
                Bitmap bmp = new Bitmap(img);
                return bmp;
            } catch (Exception exc) {
                Match notfound = Regex.Match(exc.Message, @"404", RegexOptions.IgnoreCase);
                if (!notfound.Success) {
                    //MessageBox.Show("error getting image for " + url + ":" + Environment.NewLine + exc.Message);
                    return null;
                }
                return null;
            }

        }
        #endregion

        #region deselect datagridview rows if user clicks elsewhere
        private void dgv_Mouseup(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                DataGridView.HitTestInfo hit = dgv[currenttab].HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.None) {
                    dgv[currenttab].ClearSelection();
                }
            }
        }
        #endregion

        #region Add Status Text
        public void AddStatus(string text) {
            this.BeginInvoke(new MethodInvoker(delegate() {
                if (text == "##Clear##") {
                    statusBox.Text = "";
                } else {
                    statusBox.AppendText(text);
                }
            }));
        }
        #endregion

        #region Write XML settings
        public static void WriteSettings() {
            using (StreamWriter swr = new StreamWriter(settingsfilename)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(swr, settings);
                swr.Close();
            }
        }
        #endregion

        #region enable/disable menu inputs
        private void DisableMenu() {
            this.BeginInvoke(new MethodInvoker(delegate() {
                newFileToolStripMenuItem.Enabled = false;
                newFileToolStripButton.Enabled = false;
                openMenuItem.Enabled = false;
                openButton.Enabled = false;
                saveAsMenuItem.Enabled = false;
                saveAsButton.Enabled = false;
                importToolStripMenuItem.Enabled = false;
                importButton.Enabled = false;
                importLDDButton.Enabled = false;
                importLDDToolStripMenuItem.Enabled = false;
                exportToolStripMenuItem.Enabled = false;
                calculateToolStripMenuItem.Enabled = false;
                calculateButton.Enabled = false;
                dlMenuItem.Enabled = false;
                addItemToolstripButton.Enabled = false;
            }));
        }
        void EnableMenu() {
            this.BeginInvoke(new MethodInvoker(delegate() {
                newFileToolStripMenuItem.Enabled = true;
                newFileToolStripButton.Enabled = true;
                openMenuItem.Enabled = true;
                openButton.Enabled = true;
                importToolStripMenuItem.Enabled = true;
                importButton.Enabled = true;
                importLDDButton.Enabled = true;
                importLDDToolStripMenuItem.Enabled = true;
                dlMenuItem.Enabled = true;
                if (dgv.Count > 0) {
                    calculateToolStripMenuItem.Enabled = true;
                    calculateButton.Enabled = true;
                    saveAsMenuItem.Enabled = true;
                    saveAsButton.Enabled = true;
                    addItemToolstripButton.Enabled = true;
                    exportToolStripMenuItem.Enabled = true;
                }
            }));
        }
        private void DisableCalcStop() {
            this.BeginInvoke(new MethodInvoker(delegate() {
                stopCalculationToolStripMenuItem.Enabled = false;
                stopCalculateButton.Enabled = false;
            }));
        }
        private void EnableCalcStop() {
            this.BeginInvoke(new MethodInvoker(delegate() {
                stopCalculationToolStripMenuItem.Enabled = true;
                stopCalculateButton.Enabled = true;
            }));
        }
        #endregion

        #region Progress Bar
        private void SetProgressBar(int value) {
            this.BeginInvoke(new MethodInvoker(delegate() {
                progressBar1.Value = 0;
                progressBar1.Step = 1; // Added by CAC, 2015-06-26
                progressBar1.Maximum = value;
            }));
        }
        private void SetProgressPercent(int value) {
            this.BeginInvoke(new MethodInvoker(delegate() {
                progressBar1.Value = value;
                progressBar1.Maximum = 100;
            }));
        }
        private void ResetProgressBar() {
            this.BeginInvoke(new MethodInvoker(delegate() {
                progressBar1.Value = 0;
            }));
        }
        private void AddProgress(int value) {
            this.BeginInvoke(new MethodInvoker(delegate() {
                AddStatus(value + Environment.NewLine);
                progressBar1.Value = progressBar1.Value + value;
            }));
        }
        private void Progress() {
            // This is sporadically locking up the GUI.  No idea why.
            // CAC, 2015-06-24.
            this.BeginInvoke(new MethodInvoker(delegate() {
                progressBar1.PerformStep();
            }));
        }
        private void MainWindow_SizeChanged(object sender, EventArgs e) {
            progressBar1.Width = this.Width - 35; // / 2;
        }
        #endregion

        #region Write a file
        public static void WriteFile(string file, string text) {
            using (StreamWriter swr = new StreamWriter(file)) {
                swr.Write(text);
                swr.Close();
            }
        }
        #endregion

        #region convert between Datatables and Items
        public List<Item> dt2item(DataTable thisdt) {
            List<Item> thislist = new List<Item>();
            foreach (DataRow item in thisdt.Rows) {
                thislist.Add(new Item() {
                    status = item.Field<string>("status"),
                    number = item.Field<string>("number"),
                    condition = item.Field<string>("condition"),
                    qty = item.Field<int>("qty"),
                    price = item.Field<decimal>("price"),
                    comments = item.Field<string>("comments"),
                    remarks = item.Field<string>("remarks"),
                    id = item.Field<string>("id"),
                    extid = item.Field<string>("extid"),
                    type = item.Field<string>("type"),
                    colour = item.Field<string>("colour"),
                    categoryid = item.Field<string>("categoryid"),
                    imageurl = item.Field<string>("imageurl"),
                    largeimageurl = item.Field<string>("largeimageurl")
                });
            }
            return thislist;
        }
        public static DataTable item2dt(List<Item> theseitems) {
            DataTable thisdt = new DataTable();
            thisdt.Columns.Add("status", typeof(string));
            thisdt.Columns.Add("number", typeof(string));
            thisdt.Columns.Add("name", typeof(string));
            thisdt.Columns.Add("condition", typeof(string));
            thisdt.Columns.Add("colourname", typeof(string));
            thisdt.Columns.Add("qty", typeof(int));
            thisdt.Columns.Add("price", typeof(decimal));
            thisdt.Columns.Add("total", typeof(decimal));
            thisdt.Columns.Add("comments", typeof(string));
            thisdt.Columns.Add("remarks", typeof(string));
            thisdt.Columns.Add("categoryname", typeof(string));
            thisdt.Columns.Add("typename", typeof(string));
            thisdt.Columns.Add("origqty", typeof(int));
            thisdt.Columns.Add("origprice", typeof(decimal));
            thisdt.Columns.Add("id", typeof(string));
            thisdt.Columns.Add("extid", typeof(string));
            thisdt.Columns.Add("type", typeof(string));
            thisdt.Columns.Add("colour", typeof(string));
            thisdt.Columns.Add("categoryid", typeof(string));
            thisdt.Columns.Add("availstores", typeof(int));
            thisdt.Columns.Add("availqty", typeof(int));
            thisdt.Columns.Add("imageurl", typeof(string));
            thisdt.Columns.Add("largeimageurl", typeof(string));
            thisdt.Columns.Add("imageloaded", typeof(string));
            thisdt.Columns.Add("pgpage", typeof(string));
            foreach (Item item in theseitems) {
                DataRow dr = thisdt.NewRow();
                dr.SetField<string>("status", item.status);
                dr.SetField<string>("number", item.number);
                dr.SetField<string>("name", db_blitems[item.id].name);
                dr.SetField<string>("condition", item.condition);
                dr.SetField<string>("colourname", db_colours[item.colour].name);
                dr.SetField<int>("qty", item.qty);
                dr.SetField<decimal>("price", item.price);
                dr.SetField<string>("comments", item.comments);
                dr.SetField<string>("remarks", item.remarks);
                dr.SetField<string>("categoryname", db_categories[item.categoryid].name);
                dr.SetField<string>("typename", db_typenames[item.type]);
                dr.SetField<int>("origqty", item.origqty);
                dr.SetField<decimal>("origprice", item.origprice);
                dr.SetField<string>("id", item.id);
                dr.SetField<string>("extid", item.extid);
                dr.SetField<string>("type", item.type);
                dr.SetField<string>("colour", item.colour);
                dr.SetField<string>("categoryid", item.categoryid);
                dr.SetField<int>("availstores", item.availstores);
                dr.SetField<int>("availqty", item.availqty);
                dr.SetField<string>("imageurl", item.imageurl);
                dr.SetField<string>("largeimageurl", item.largeimageurl);
                dr.SetField<string>("imageloaded", "n");
                dr.SetField<string>("pgpage", item.pgpage);
                thisdt.Rows.Add(dr);
            }
            return thisdt;
        }
        #endregion

        #region Modify string text when saving & loading
        private string StringSaveMod(string text) {
            if ((text != null) && (text != "")) {
                text = text.Replace("& ", "&amp; ");
                text = text.Replace("<", "&lt;");
            }
            return text;
        }
        private string StringLoadMod(string text) {
            if ((text != null) && (text != "")) {
                text = text.Replace("&amp; ", "& ");
                text = text.Replace("&lt;", "<");
            }
            return text;
        }
        #endregion

        #region convert to decimal - now with region support!
        private decimal ConvertToDecimal(string text) {
            text = text.Replace(".", numberseperator);
            return Convert.ToDecimal(text);
        }
        #endregion

        #region Download and read Bricklink DB
        private void dlWorker_DoWork(object sender, DoWorkEventArgs e) {
            DisableMenu();
            AddStatus("##Clear##");

            AddStatus("Refreshing catalog..." + Environment.NewLine);

            _dataUpdateService.UpdateData();
            _dataUpdateService.PrefetchData();
            PopulateLookupsFromServices();

            AddStatus("Done." + Environment.NewLine);
            EnableMenu();
        }

        private void PopulateLookupsFromServices()
        {
            var itemTypes = _itemTypeService.GetItemTypes();
            db_typenames = itemTypes.ToDictionary(x => x.ItemTypeCode, x => x.Name);

            var categories = _categoryService.GetCategories()
                .Select(x => new DBCat { id = x.CategoryId.ToString(), name = x.Name.ToString() })
                .ToDictionary(x => x.id, x => x);
            db_categories = categories;

            var colors = _colorService.GetColors().ToDictionary(x => x.id, x => x);
            db_colours = colors;

            // todo: not accounting for items that contain other items
            var items = _itemService.GetItems().Select(x => new DBBLItem()
            {
                id = string.Format("{0}-{1}", x.ItemTypeCode.ToUpperInvariant(), x.ItemId),
                number = x.ItemId, // todo: what is this??
                type = x.ItemTypeCode,
                name = x.Name,
                catid = x.CategoryId.ToString(),
            });

            db_blitems = items.ToDictionary(x => x.id, x => x);
        }

        private void DownloadBrickLinkDB()
        {
            AddStatus("Loading...");

            db_colours.Clear();
            db_categories.Clear();
            db_blitems.Clear();
            db_typenames.Clear();
            db_blaltids.Clear();

            PopulateLookupsFromServices();
        }        
        #endregion

        #region splitter moved
        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
            SplitterMoved();
        }
        private void SplitterMoved() {
            settings.splitterdistance = splitContainer.SplitterDistance;
            imageLabel.Height = splitContainer.Panel2.Height - 28;
            containerList.Height = splitContainer.Panel2.Height - 28;
            statusBox.Height = splitContainer.Panel2.Height - 28;

            if (imageLabel.Visible == false) {
                if (containerList.Visible == false) {
                    statusBox.Left = 1;
                    statusBox.Width = splitContainer.Panel2.Width - 6;
                }
            } else {
                if (containerList.Visible == false) {
                    statusBox.Left = 170;
                    statusBox.Width = splitContainer.Panel2.Width - 175;
                } else {
                    statusBox.Left = 432;
                    statusBox.Width = splitContainer.Panel2.Width - 437;
                }
            }

        }
        #endregion

        #region add item to dgv
        public static void dgv_AddItem(
            string id,
            string colour = "0",
            int quantity = 0,
            ItemCondition condition = ItemCondition.Used)
        {

            string extid = db_blitems[id].type + "-" + colour + "-" + db_blitems[id].number;
            if (!dt[currenttab].Rows.Contains(extid))
            {
                DataRow dr = dt[currenttab].NewRow();
                dr["status"] = "I";
                dr["number"] = db_blitems[id].number;
                dr["type"] = db_blitems[id].type;
                dr["typename"] = db_typenames[db_blitems[id].type];
                dr["id"] = id;
                dr["extid"] = db_blitems[id].type + "-" + colour + "-" + db_blitems[id].number;
                dr["name"] = db_blitems[id].name;
                dr["colour"] = colour;
                dr["colourname"] = db_colours[colour].name;
                dr["condition"] = condition == ItemCondition.New ? "N" : "U";
                dr["qty"] = quantity.ToString();
                dr["availstores"] = -1;
                dr["price"] = 0;
                dr["total"] = 0;
                dr["categoryid"] = db_blitems[id].catid;
                dr["categoryname"] = db_categories[db_blitems[id].catid].name;
                dr["imageurl"] = GenerateImageURL(id, colour);
                dr["largeimageurl"] = GenerateImageURL(id);
                dr["imageloaded"] = "n";
                dt[currenttab].Rows.Add(dr);

                dgv[currenttab].Rows[dgv[currenttab].Rows.Count - 1].Cells["displaystatus"].Value = Properties.Resources.check;
                if (int.Parse(dgv[currenttab].Rows[dgv[currenttab].Rows.Count - 1].Cells["qty"].Value.ToString()) <= 0)
                {
                    dgv[currenttab].Rows[dgv[currenttab].Rows.Count - 1].Cells["qty"].Style.BackColor = errorcell;
                }

                dgv_GetLiveStats(id, colour);
                dgv_ImageDisplay(id, colour);
            }
            else
            {
                foreach (DataGridViewRow row in dgv[currenttab].Rows)
                {
                    if (row.Cells["extid"].Value.ToString() == extid)
                    {
                        row.Cells["qty"].Value = (int.Parse(row.Cells["qty"].Value.ToString()) + quantity).ToString();
                    }

                    row.Selected = row.Cells["extid"].Value.ToString() == extid;
                }
            }
        }
        #endregion

        #region generate image URL
        public static string GenerateImageURL(string id, string colour = "large") {
            string imageurl;
            if (colour != "large") {
                imageurl = "https://www.bricklink.com/getPic.asp?itemType=" + db_blitems[id].type + (colour == "0" ? "" : "&colorID=" + colour) + "&itemNo=" + db_blitems[id].number;
                return imageurl;
            } else {
                imageurl = "https://www.bricklink.com/" + db_blitems[id].type + "L/" + db_blitems[id].number + ".jpg";
                return imageurl;
            }
        }
        #endregion

        #region generate image filename
        public static string GenerateImageFilename(string id, string colour = "large") {
            string filename;
            if (colour != "large") {
                if ((db_blitems[id].type == "P") || (db_blitems[id].type == "G")) {
                    filename = programdata + "images\\" + db_blitems[id].type + "\\" + db_blitems[id].number + "\\" + colour + "\\small.png";
                } else {
                    filename = programdata + "images\\" + db_blitems[id].type + "\\" + db_blitems[id].number + "\\small.png";
                }
            } else {
                filename = programdata + "images\\" + db_blitems[id].type + "\\" + db_blitems[id].number + "\\large.png";
            }
            return filename;

        }
        #endregion

        #region datagridview double click
        private void dgv_doubleclick(object sender, DataGridViewCellEventArgs e) {
            DataGridView dgv_sender = sender as DataGridView;
            Point mousePoint = dgv_sender.PointToClient(Cursor.Position);
            if (dgv[currenttab].HitTest(mousePoint.X, mousePoint.Y).Type == DataGridViewHitTestType.Cell) {
                if (dgv[currenttab].Columns[e.ColumnIndex].Name == "displaystatus") {
                    if ((String)dgv[currenttab].Rows[e.RowIndex].Cells["status"].Value == "I") {
                        dgv[currenttab].Rows[e.RowIndex].Cells["status"].Value = "E";
                        dgv[currenttab].Rows[e.RowIndex].Cells["displaystatus"].Value = Properties.Resources.add;
                    } else if ((String)dgv[currenttab].Rows[e.RowIndex].Cells["status"].Value == "E") {
                        dgv[currenttab].Rows[e.RowIndex].Cells["status"].Value = "X";
                        dgv[currenttab].Rows[e.RowIndex].Cells["displaystatus"].Value = Properties.Resources.x;
                    } else {
                        dgv[currenttab].Rows[e.RowIndex].Cells["status"].Value = "I";
                        dgv[currenttab].Rows[e.RowIndex].Cells["displaystatus"].Value = Properties.Resources.check;
                    }
                } else if (dgv[currenttab].Columns[e.ColumnIndex].Name == "displayimage") {
                    string imgfilename = GenerateImageFilename((String)dgv[currenttab].Rows[e.RowIndex].Cells["id"].Value);

                    if (!File.Exists(imgfilename)) {
                        hoverZoomWindow.BackgroundImage = null;
                        hoverZoomWindow.Width = 100;
                        hoverZoomWindow.Height = 40;
                        hoverZoomWindow.ShowLabel();

                        lock (imgTimerLock) {
                            imageDLList.Insert(0, new ImageDL() {
                                extid = (String)dgv[currenttab].Rows[e.RowIndex].Cells["extid"].Value,
                                file = imgfilename,
                                url = (String)dgv[currenttab].Rows[e.RowIndex].Cells["largeimageurl"].Value,
                                type = "l"
                            });
                        }
                    } else {
                        Image backimage = Image.FromFile(imgfilename);
                        hoverZoomWindow.BackgroundImage = backimage;
                        hoverZoomWindow.Width = backimage.Width;
                        hoverZoomWindow.Height = backimage.Height;
                        hoverZoomWindow.HideLabel();
                    }
                } else if (dgv[currenttab].Columns[e.ColumnIndex].Name == "name") {
                    changeItemWindow.DisplayItem(dgv[currenttab].Rows[e.RowIndex]);
                    changeItemWindow.BringToFront();
                    changeItemWindow.WindowState = FormWindowState.Normal;
                    changeItemWindow.Show();
                } else if (dgv[currenttab].Columns[e.ColumnIndex].Name == "condition") {
                    if ((String)dgv[currenttab].Rows[e.RowIndex].Cells["condition"].Value == "U") {
                        dgv[currenttab].Rows[e.RowIndex].Cells["condition"].Value = "N";
                    } else {
                        dgv[currenttab].Rows[e.RowIndex].Cells["condition"].Value = "U";
                    }
                } else if (dgv[currenttab].Columns[e.ColumnIndex].Name == "colourname") {
                    setColourDialog();
                }
            }
        }
        #endregion

        #region datagridview right click
        private void dgv_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                if (dgv[currenttab].HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) {
                    if (dgv[currenttab].SelectedRows.Count == 0) {
                        dgv[currenttab].Rows[dgv[currenttab].HitTest(e.X, e.Y).RowIndex].Selected = true;
                    } else if (dgv[currenttab].Rows[dgv[currenttab].HitTest(e.X, e.Y).RowIndex].Selected != true) {
                        dgv[currenttab].ClearSelection();
                        dgv[currenttab].Rows[dgv[currenttab].HitTest(e.X, e.Y).RowIndex].Selected = true;
                    }
                }
            }
        }

        private void dgv_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                //if (dgv[currenttab].HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell)
                //{
                if (dgv[currenttab].SelectedRows.Count == 0) {
                    deleteToolStripMenuItem.Visible = false;
                    statusToolStripMenuItem.Visible = false;
                    conditionToolStripMenuItem.Visible = false;
                    setColourToolStripMenuItem.Visible = false;
                    quantityToolStripMenuItem.Visible = false;
                    priceToolStripMenuItem.Visible = false;
                    commentToolStripMenuItem.Visible = false;
                    remarkToolStripMenuItem.Visible = false;
                    showBricklinkCatalogInfoToolStripMenuItem.Visible = false;
                    showBricklinkPriceGuideToolStripMenuItem.Visible = false;
                    showLotsForSaleOnBricklinkToolStripMenuItem.Visible = false;
                    toolStripSeparator5.Visible = false;
                    toolStripSeparator6.Visible = false;
                    toolStripSeparator7.Visible = false;
                } else {
                    deleteToolStripMenuItem.Visible = true;
                    statusToolStripMenuItem.Visible = true;
                    conditionToolStripMenuItem.Visible = true;
                    setColourToolStripMenuItem.Visible = true;
                    quantityToolStripMenuItem.Visible = true;
                    priceToolStripMenuItem.Visible = true;
                    commentToolStripMenuItem.Visible = true;
                    remarkToolStripMenuItem.Visible = true;
                    showBricklinkCatalogInfoToolStripMenuItem.Visible = true;
                    showBricklinkPriceGuideToolStripMenuItem.Visible = true;
                    showLotsForSaleOnBricklinkToolStripMenuItem.Visible = true;
                    toolStripSeparator5.Visible = true;
                    toolStripSeparator6.Visible = true;
                    toolStripSeparator7.Visible = true;
                }
                contextMenuStrip1.Show(Cursor.Position);
                //}
            }
        }
        private void dgv_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                if (dgv[currenttab].HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) {
                    dgv[currenttab].Rows[dgv[currenttab].HitTest(e.X, e.Y).RowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region datagridview sorted
        private void dgvSorted(object sender, EventArgs e) {
            foreach (DataGridViewRow item in dgv[currenttab].Rows) {
                string imgfilename = GenerateImageFilename((string)item.Cells["id"].Value, (string)item.Cells["colour"].Value);

                //if (((String)item.Cells["type"].Value == "P") || ((String)item.Cells["type"].Value == "G"))
                //    imgfilename = programdata + "images\\" + item.Cells["type"].Value + "\\" + item.Cells["number"].Value + "\\" + item.Cells["colour"].Value + "\\small.png";
                //else
                //    imgfilename = programdata + "images\\" + item.Cells["type"].Value + "\\" + item.Cells["number"].Value + "\\small.png";

                if (File.Exists(imgfilename)) {
                    item.Cells["displayimage"].Value = Bitmap.FromFile(imgfilename);
                    item.Cells["imageloaded"].Value = "l";
                } else {
                    item.Cells["displayimage"].Value = Properties.Resources.blank;
                }


                if ((String)item.Cells["status"].Value == "X") {
                    item.Cells["displaystatus"].Value = Properties.Resources.x;
                } else if ((String)item.Cells["status"].Value == "E") {
                    item.Cells["displaystatus"].Value = Properties.Resources.add;
                } else {
                    item.Cells["displaystatus"].Value = Properties.Resources.check;
                }
            }
        }
        #endregion

        #region datagridview cell edit
        private void dgvCellEdit(object sender, DataGridViewCellEventArgs e) {
            if ((dgv[currenttab].Columns[e.ColumnIndex].Name == "qty") || (dgv[currenttab].Columns[e.ColumnIndex].Name == "price")) {
                dgv[currenttab].Rows[e.RowIndex].Cells["total"].Value = (Decimal)dgv[currenttab].Rows[e.RowIndex].Cells["price"].Value * (int)dgv[currenttab].Rows[e.RowIndex].Cells["qty"].Value;
                if ((int)dgv[currenttab].Rows[e.RowIndex].Cells["qty"].Value == 0) {
                    dgv[currenttab].Rows[e.RowIndex].Cells["qty"].Style.BackColor = errorcell;
                } else {
                    dgv[currenttab].Rows[e.RowIndex].Cells["qty"].Style.BackColor = dgv[currenttab].Rows[e.RowIndex].Cells["status"].Style.BackColor;
                }
            }
        }
        #endregion

        #region datagridview cell validating
        private void dgvCellVal(object sender, DataGridViewCellValidatingEventArgs e) {
            if (dgv[currenttab].Columns[e.ColumnIndex].Name == "number") {
                string number = e.FormattedValue.ToString();
                string extid = dgv[currenttab].Rows[e.RowIndex].Cells["type"].Value + "-" +
                    dgv[currenttab].Rows[e.RowIndex].Cells["colour"].Value + "-" + number;
                string id = dgv[currenttab].Rows[e.RowIndex].Cells["type"].Value + "-" + number;

                if (number == dgv[currenttab].Rows[e.RowIndex].Cells["number"].Value.ToString()) {
                    return;
                } else if (db_blitems.ContainsKey(id)) {
                    dgv[currenttab].Rows[e.RowIndex].Cells["extid"].Value = extid;
                    dgv[currenttab].Rows[e.RowIndex].Cells["id"].Value = id;
                    dgv[currenttab].Rows[e.RowIndex].Cells["name"].Value = db_blitems[dgv[currenttab].Rows[e.RowIndex].Cells["id"].Value.ToString()].name;
                    dgv[currenttab].Rows[e.RowIndex].Cells["categoryid"].Value = db_blitems[dgv[currenttab].Rows[e.RowIndex].Cells["id"].Value.ToString()].catid;
                    dgv[currenttab].Rows[e.RowIndex].Cells["categoryname"].Value = db_categories[dgv[currenttab].Rows[e.RowIndex].Cells["categoryid"].Value.ToString()].name;
                    dgv[currenttab].Rows[e.RowIndex].Cells["imageloaded"].Value = "n";
                    dgv[currenttab].Rows[e.RowIndex].Cells["imageurl"].Value = GenerateImageURL(id, (string)dgv[currenttab].Rows[e.RowIndex].Cells["colour"].Value);
                    dgv[currenttab].Rows[e.RowIndex].Cells["largeimageurl"].Value = "https://www.bricklink.com/" + dgv[currenttab].Rows[e.RowIndex].Cells["type"].Value +
                        "L/" + dgv[currenttab].Rows[e.RowIndex].Cells["number"].Value + ".jpg";
                    dgv[currenttab].Rows[e.RowIndex].Cells["displayimage"].Value = Properties.Resources.blank;
                    dgv[currenttab].Rows[e.RowIndex].Cells["pgpage"].Value = null;

                    dgv_GetLiveStats(id, (string)dgv[currenttab].Rows[e.RowIndex].Cells["colour"].Value);
                    dgv_ImageDisplay(id, (string)dgv[currenttab].Rows[e.RowIndex].Cells["colour"].Value);
                } else {
                    e.Cancel = true;
                    dgv[currenttab].CancelEdit();
                    MessageBox.Show("Item " + number + " not found in database.");
                }
            }
        }
        #endregion

        #region datagridview cell keypress
        private void dgvEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            e.Control.KeyPress -= new KeyPressEventHandler(CheckOtherNumericKey);
            e.Control.KeyPress -= new KeyPressEventHandler(CheckPriceNumericKey);
            e.Control.KeyPress -= new KeyPressEventHandler(CheckNoKey);
            if (dgv[currenttab].Columns[dgv[currenttab].CurrentCell.ColumnIndex].Name == "qty") {
                e.Control.KeyPress += new KeyPressEventHandler(CheckOtherNumericKey);
            } else if (dgv[currenttab].Columns[dgv[currenttab].CurrentCell.ColumnIndex].Name == "price") {
                e.Control.KeyPress += new KeyPressEventHandler(CheckPriceNumericKey);
            } else {
                e.Control.KeyPress += new KeyPressEventHandler(CheckNoKey);
            }
        }
        private void CheckPriceNumericKey(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(numberseperator)) {
                e.Handled = true;
            }
        }
        private void CheckOtherNumericKey(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }
        private void CheckNoKey(object sender, KeyPressEventArgs e) {
        }
        #endregion

        #region datagridview mouseover
        private void dgv_CellMouseEnter(object sender, DataGridViewCellEventArgs e) {
            DataGridView dgv_sender = sender as DataGridView;
            Point mousePoint = dgv_sender.PointToClient(Cursor.Position);
            if (dgv[currenttab].HitTest(mousePoint.X, mousePoint.Y).Type == DataGridViewHitTestType.Cell) {
                if (dgv_sender.Columns[e.ColumnIndex].Name == "displayimage") {
                    DataGridViewRow dgv_MouseOverRow = dgv_sender.Rows[e.RowIndex];
                    DataGridViewCell dgv_MouseOverCell = dgv_MouseOverRow.Cells[e.ColumnIndex];
                    string id = dgv_MouseOverRow.Cells["id"].Value.ToString();
                    string colour = dgv_MouseOverRow.Cells["colour"].Value.ToString();

                    string filename = GenerateImageFilename(id, colour);

                    if (!File.Exists(filename)) {
                        hoverZoomWindow.BackgroundImage = null;
                        hoverZoomWindow.Width = 100;
                        hoverZoomWindow.Height = 50;
                        hoverZoomWindow.ShowLabel();
                    } else {
                        Bitmap bmp = (Bitmap)Image.FromFile(filename);
                        hoverZoomWindow.BackgroundImage = bmp;
                        hoverZoomWindow.Width = bmp.Width * 2;
                        hoverZoomWindow.Height = bmp.Height * 2;
                        hoverZoomWindow.HideLabel();
                    }

                    //hoverZoomWindow.Location = new System.Drawing.Point(mousePoint.X + 10, mousePoint.Y + 10);
                    //Screen screen = Screen.FromPoint(Cursor.Position);
                    //hoverZoomWindow.Location = screen.Bounds.Location;
                    hoverZoomWindow.Left = System.Windows.Forms.Cursor.Position.X + 10;
                    hoverZoomWindow.Top = System.Windows.Forms.Cursor.Position.Y + 10;

                    hoverZoomWindow.Show();
                }
            }
        }

        private void dgv_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {
            hoverZoomWindow.Hide();
            hoverZoomWindow.BackgroundImage = null;
        }

        private void dgv_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridView dgv_sender = sender as DataGridView;
            Point mousePoint = dgv_sender.PointToClient(Cursor.Position);
            if (dgv[currenttab].HitTest(mousePoint.X, mousePoint.Y).Type == DataGridViewHitTestType.Cell) {
                if (dgv_sender.Columns[e.ColumnIndex].Name == "displayimage") {
                    //Screen screen = Screen.FromPoint(Cursor.Position);
                    //hoverZoomWindow.Location = screen.Bounds.Location;
                    hoverZoomWindow.Left = System.Windows.Forms.Cursor.Position.X + 10;
                    hoverZoomWindow.Top = System.Windows.Forms.Cursor.Position.Y + 10;
                }
            }
        }
        #endregion

        #region datagridview dataerror
        private void dgvDataError(object sender, DataGridViewDataErrorEventArgs anError) {
            AddStatus("Invalid value: " + dgv[currenttab].EditingControl.Text + Environment.NewLine);
        }
        #endregion

        #region datagridview selection changed
        private void dgv_selectchange(object sender, EventArgs e) {
            if (dgv[currenttab].SelectedRows.Count == 0) {
                imageLabel.Visible = false;
                containerList.Visible = false;
                SplitterMoved();

                imageLabel.Image = Properties.Resources.blank;
                imageLabel.Text = "";
            } else if (dgv[currenttab].SelectedRows.Count == 1) {
                string qty = dgv[currenttab].SelectedRows[0].Cells["qty"].Value.ToString();
                string color = (string)dgv[currenttab].SelectedRows[0].Cells["colourname"].Value;
                string name = (string)dgv[currenttab].SelectedRows[0].Cells["name"].Value;
                Image tmpimage = (Image)dgv[currenttab].SelectedRows[0].Cells["displayimage"].Value;

                if (tmpimage != null) {
                    Bitmap image = new Bitmap(tmpimage, new Size(tmpimage.Width * 2, tmpimage.Height * 2));
                    imageLabel.Image = image;
                } else {
                    imageLabel.Image = Properties.Resources.blank;
                }

                imageLabel.TextAlign = ContentAlignment.BottomCenter;
                imageLabel.Text = qty + "x " + color + " " + name + Environment.NewLine;

                containerList.Rows.Clear();

                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                    if (db_containers.ContainsKey(row.Cells["extid"].Value.ToString())) {
                        foreach (DBItemContain itemcontain in db_containers[row.Cells["extid"].Value.ToString()]) {
                            containerList.Rows.Add(itemcontain.qty, db_blitems[itemcontain.item].name, itemcontain.item);
                        }
                    }
                }

                if (containerList.Rows.Count > 0) {
                    containerList.Sort(containerList.Columns[0], ListSortDirection.Descending);
                    containerList.Visible = true;
                } else {
                    containerList.Visible = false;
                }

                imageLabel.Visible = true;
                SplitterMoved();
            } else {
                int pieces = 0;
                int lots = 0;
                decimal price = 0;

                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                    lots++;
                    pieces = pieces + (int)row.Cells["qty"].Value;
                    price = price + (decimal)row.Cells["total"].Value;
                }

                imageLabel.Image = Properties.Resources.blank;
                imageLabel.TextAlign = ContentAlignment.MiddleCenter;
                imageLabel.Text = pieces + " pieces in " + lots + " lots" + Environment.NewLine + Environment.NewLine +
                    "Total: " + currencysymbol + price;

                imageLabel.Visible = true;
                containerList.Visible = false;
                SplitterMoved();
            }
        }
        #endregion

        #region Set Colour Dialog
        private void setColourDialog() {

            // Set the selected colour of the colourPicker to be the colour of the first selected brick.
            colourPickerWindow.num = dgv[currenttab].SelectedRows[0].Cells["colour"].Value.ToString();

            DialogResult result = colourPickerWindow.ShowDialog();
            if (result == DialogResult.OK) {
                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                    if ((row.Cells["type"].Value.ToString() == "P") || (row.Cells["type"].Value.ToString() == "G")) {
                        row.Cells["colour"].Value = colourPickerWindow.num;
                        row.Cells["colourname"].Value = db_colours[colourPickerWindow.num].name;
                        row.Cells["imageloaded"].Value = "n";
                        row.Cells["imageurl"].Value = GenerateImageURL((string)row.Cells["id"].Value, (string)row.Cells["colour"].Value);
                        row.Cells["extid"].Value = row.Cells["type"].Value + "-" + colourPickerWindow.num + "-" + row.Cells["number"].Value;
                        row.Cells["availstores"].Value = -1;
                        row.Cells["displayimage"].Value = Properties.Resources.blank;

                        dgv_GetLiveStats((string)row.Cells["id"].Value, (string)row.Cells["colour"].Value);
                        dgv_ImageDisplay((string)row.Cells["id"].Value, (string)row.Cells["colour"].Value);
                    }
                }
            }
        }
        #endregion

        #region Menu Options
        #region (File -> New)
        private void newFileToolStripButton_Click(object sender, EventArgs e) {
            foreach (DataTable thisdt in dt) {
                thisdt.Dispose();
            }

            BuildTable();
            DisplayLoadedFile();
            EnableMenu();
        }
        #endregion

        #region (File -> Open)
        private void openMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                DisableMenu();
                bool success = LoadFile(openFileDialog.FileName);
                if (success) {
                    DisplayLoadedFile();
                }
            }
            EnableMenu();
        }
        #endregion

        #region (File -> Save as)
        public void saveAsMenuItem_Click(object sender, EventArgs e) {
            if (dgv.Count < 1) {
                MessageBox.Show("Error: No file to save.");
            } else {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    string outfile;
                    outfile = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine;
                    outfile = outfile + "<!DOCTYPE BrickStoreXML>" + Environment.NewLine;
                    outfile = outfile + "<BrickStoreXML>" + Environment.NewLine;
                    outfile = outfile + " <Inventory>" + Environment.NewLine;

                    List<Item> saveitems = dt2item(dt[currenttab]);

                    foreach (Item item in saveitems) {
                        string comments = item.comments;
                        string remarks = item.remarks;
                        string name = MainWindow.db_blitems[item.id].name;
                        string typename = MainWindow.db_typenames[item.type];
                        string colourname = MainWindow.db_colours[item.colour].name;
                        string categoryid = item.categoryid;
                        string categoryname = MainWindow.db_categories[item.categoryid].name;
                        string price = Convert.ToString(item.price);
                        price = price.Replace(numberseperator, ".");

                        comments = StringSaveMod(comments);
                        remarks = StringSaveMod(remarks);
                        name = StringSaveMod(name);
                        typename = StringSaveMod(typename);
                        colourname = StringSaveMod(colourname);
                        categoryid = StringSaveMod(categoryid);
                        categoryname = StringSaveMod(categoryname);

                        outfile = outfile + "  <Item>" + Environment.NewLine;
                        outfile = outfile + "   <ItemID>" + item.number + "</ItemID>" + Environment.NewLine;
                        outfile = outfile + "   <ItemTypeID>" + item.type + "</ItemTypeID>" + Environment.NewLine;
                        outfile = outfile + "   <ColorID>" + item.colour + "</ColorID>" + Environment.NewLine;
                        outfile = outfile + "   <ItemName>" + name + "</ItemName>" + Environment.NewLine;
                        outfile = outfile + "   <ItemTypename>" + typename + "</ItemTypename>" + Environment.NewLine;
                        outfile = outfile + "   <ColorName>" + colourname + "</ColorName>" + Environment.NewLine;
                        outfile = outfile + "   <CategoryID>" + categoryid + "</CategoryID>" + Environment.NewLine;
                        outfile = outfile + "   <CategoryName>" + categoryname + "</CategoryName>" + Environment.NewLine;
                        outfile = outfile + "   <Status>" + item.status + "</Status>" + Environment.NewLine;
                        outfile = outfile + "   <Qty>" + item.qty + "</Qty>" + Environment.NewLine;
                        outfile = outfile + "   <Price>" + price + "</Price>" + Environment.NewLine;
                        outfile = outfile + "   <Condition>" + item.condition + "</Condition>" + Environment.NewLine;
                        if ((comments != null) && (comments != "")) {
                            outfile = outfile + "   <Comments>" + comments + "</Comments>" + Environment.NewLine;
                        }
                        if ((remarks != null) && (remarks != "")) {
                            outfile = outfile + "   <Remarks>" + remarks + "</Remarks>" + Environment.NewLine;
                        }
                        outfile = outfile + "  </Item>" + Environment.NewLine;
                    }

                    outfile = outfile + " </Inventory>" + Environment.NewLine;
                    outfile = outfile + "</BrickStoreXML>" + Environment.NewLine;

                    using (StreamWriter swr = new StreamWriter(saveFileDialog1.FileName)) {
                        swr.Write(outfile);
                    }
                    AddStatus("File Saved: " + saveFileDialog1.FileName.ToString() + Environment.NewLine);
                }
            }
        }
        #endregion

        #region (File -> Import -> BL Wanted) Import BrickLink Wanted List
        private void importBLWantedMenuItem_Click(object sender, EventArgs e)
        {
            if(dt.Count == 0)
            {
                BuildTable();
                DisplayLoadedFile();
                EnableMenu();
            }

            _importWantedListForm.Enabled = true;
            DialogResult result = _importWantedListForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                WriteSettings();
                DisplayLoadedFile();
                EnableMenu();
            }
        }
        #endregion

        #region (File -> Import -> LDD) Import LDD file
        private void importLDDMenuItem_Click(object sender, EventArgs e) {
            if (importLDDFileDialog.ShowDialog() == DialogResult.OK) {
                DisableMenu();
                importWorker.RunWorkerAsync(importLDDFileDialog.FileName);
            }
        }
        #endregion

        #region (File -> Export -> Wanted List)
        private void exportWantedListToolStripMenuItem_Click(object sender, EventArgs e) {
            if (wantedListWindow.ShowDialog() == DialogResult.OK) {
                string wlid = wantedListWindow.wlid;
                string wanted = "<INVENTORY>" + Environment.NewLine;

                foreach (DataGridViewRow item in dgv[currenttab].SelectedRows) {
                    wanted = wanted + " <ITEM>" + Environment.NewLine;

                    wanted = wanted + "  <ITEMTYPE>" + item.Cells["type"].Value.ToString() + "</ITEMTYPE>" + Environment.NewLine;
                    wanted = wanted + "  <ITEMID>" + item.Cells["number"].Value.ToString() + "</ITEMID>" + Environment.NewLine;

                    if ((item.Cells["type"].Value.ToString() == "P") || (item.Cells["type"].Value.ToString() == "G")) {
                        if (item.Cells["colour"].Value.ToString() != "0") {
                            wanted = wanted + "  <COLOR>" + item.Cells["colour"].Value.ToString() + "</COLOR>" + Environment.NewLine;
                        }
                    }

                    if ((decimal)item.Cells["price"].Value > 0) {
                        wanted = wanted + "  <MAXPRICE>" + item.Cells["price"].Value.ToString() + "</MAXPRICE>" + Environment.NewLine;
                    }

                    if (item.Cells["qty"].Value.ToString() != "0") {
                        wanted = wanted + "  <MINQTY>" + item.Cells["qty"].Value.ToString() + "</MINQTY>" + Environment.NewLine;
                    }

                    if (item.Cells["condition"].Value.ToString() == "N") {
                        wanted = wanted + "  <CONDITION>" + item.Cells["condition"].Value.ToString() + "</CONDITION>" + Environment.NewLine;
                    }

                    if (item.Cells["remarks"].Value.ToString() != "") {
                        wanted = wanted + "  <REMARKS>" + item.Cells["remarks"].Value.ToString() + "</REMARKS>" + Environment.NewLine;
                    }

                    if (wlid != "") {
                        wanted = wanted + "  <WANTEDLISTID>" + wlid + "</WANTEDLISTID>" + Environment.NewLine;
                    }

                    wanted = wanted + " </ITEM>" + Environment.NewLine;
                }
                wanted = wanted + "</INVENTORY>" + Environment.NewLine;

                System.Windows.Forms.Clipboard.SetText(wanted);

                string url = "https://www.bricklink.com/wantedXML.asp";
                try {
                    System.Diagnostics.Process.Start(url);
                } catch {
                    AddStatus("Error displaying Wanted List Upload page in your default web browser. The Wanted List text is still in your clipboard.");
                }
            }
        }
        #endregion

        #region (File -> Exit)
        private void exitMenuItem_Click(object sender, EventArgs e) {
            Close();
        }
        #endregion

        #region (Toolstrip -> Add)
        private void addItemToolstripButton_Click(object sender, EventArgs e) {
            addItemWindow.Show();
            addItemWindow.BringToFront();
            addItemWindow.WindowState = FormWindowState.Normal;
        }
        #endregion

        #region (Tools -> Calculate and Calculate2)

        private void oldAlgorithmToolStripMenuItem_Click(object sender, EventArgs e) {
            runTheAlgorithm(RUN_OLD);
        }

        private void newAlgorithmToolStripMenuItem_Click(object sender, EventArgs e) {
            runTheAlgorithm(RUN_NEW);
        }
        private void calculateButton_Click(object sender, EventArgs e) {
            runTheAlgorithm(RUN_NEW);
        }

        private void approximationAlgorithmToolStripMenuItem_Click(object sender, EventArgs e) {
            runTheAlgorithm(RUN_APPROX);
        }
        private void customAlgorithmToolStripMenuItem_Click(object sender, EventArgs e) {
            runTheAlgorithm(RUN_CUSTOM);
        }
        private void customApproximationAlgorithmToolStripMenuItem_Click(object sender, EventArgs e) {
            runTheAlgorithm(RUN_CUSTOM_APPROX);
        }
        //private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //}
        #endregion
        private void runTheAlgorithm(int whichAlgorithm) {
            whichAlgToRun = whichAlgorithm;
            if (dgv.Count < 1) {
                MessageBox.Show("Error: no file to calculate.");
            } else if (dgv[currenttab].Rows.Count == 0) {
                MessageBox.Show("Error: must have at least one item to search for.");
            } else {
                calcOptionsWindow.ShowApproxOptions(whichAlgorithm==RUN_APPROX || whichAlgorithm==RUN_CUSTOM_APPROX);
                DialogResult result = calcOptionsWindow.ShowDialog();
                if (result == DialogResult.OK) {
                    WriteSettings();
                    StartCalculate();
                }
            }
        }

        #region (Tools -> Stop Calculation)
        private void stopCalculationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (calcWorker.IsBusy) {
                DisableCalcStop();
                calcWorker.CancelAsync();
                //while (calcWorker.IsBusy)
                //{
                //}
                AddStatus(Environment.NewLine + "  Stopping Calculation...");
            }
        }
        #endregion

        #region (Tools -> View Report)
        private void viewReportToolStripMenuItem_Click(object sender, EventArgs e) {
            if (File.Exists(outputfilename)) {
                try {
                    System.Diagnostics.Process.Start(outputfilename);
                } catch {
                    MessageBox.Show("Error displaying report in your default web browser." + Environment.NewLine +
                        "Report file is available here: " + outputfilename + Environment.NewLine);
                }
            } else {
                MessageBox.Show("No report found.");
            }
        }
        #endregion

        #region (Tools -> Download BrickLink Database)
        private void dlMenuItem_Click(object sender, EventArgs e) {
            DialogResult result = _updateConfirmationForm.ShowDialog();
            if (result == DialogResult.OK) {
                foreach (DataTable thisdt in dt) {
                    thisdt.Dispose();
                }
                dt.Clear();
                foreach (DataGridView thisdgv in dgv) {
                    thisdgv.Dispose();
                }
                dgv.Clear();

                if (File.Exists(databasefilename)) {
                    File.Delete(databasefilename);
                }
                dlWorker.RunWorkerAsync();
            }
        }
        #endregion

        #region (Help -> About) Show the About window
        private void aboutMenuItem_Click(object sender, EventArgs e) {
            aboutWindow.ShowDialog();
        }
        #endregion

        #region (Context -> Delete)

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = dgv[currenttab].Rows.Count - 1; i >= 0; i--) {
                if (dgv[currenttab].Rows[i].Selected == true) {
                    dgv[currenttab].Rows.RemoveAt(i);
                }
            }
        }
        #endregion

        #region (Context -> Select All)
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv[currenttab].Rows) {
                row.Selected = true;
            }
        }
        #endregion

        #region (Context -> Invert Selection)
        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv[currenttab].Rows) {
                if (row.Selected == true)
                    row.Selected = false;
                else
                    row.Selected = true;
            }
        }
        #endregion

        // Delegate use for toggle
        delegate object Del(object current);

        /// <summary>
        /// Set the selected cells with the given value
        /// </summary>
        /// <param name="fieldValues">List of columns/values pairs to set for the selected records</param>
        /// <comment>
        /// If sorted by the column being written to the DataGridView has a tendency to reorder the rows mid operation
        /// And given the selected are defined by index when the rows are reordered the wrong records end up being written to
        /// This method is an attempt to ensure the correct records are written to even if the effect of writing causes the rows to be reordered
        /// </comment>
        private void SortSafeSet(List<Tuple<string, Del>> fieldValues)
        {
            // First save the ids of the selected rows
            List<string> selectedUIDs = new List<string>();
            foreach (DataGridViewRow row in dgv[currenttab].SelectedRows)
                selectedUIDs.Add(row.Cells["extid"].Value.ToString());

            foreach (string uid in selectedUIDs)
            {
                // Find the row with the given id in the table
                foreach (DataGridViewRow row in dgv[currenttab].Rows)
                {
                    if (row.Cells["extid"].Value.ToString() == uid)
                    {
                        foreach (Tuple<string, Del> pair in fieldValues)
                        {
                            row.Cells[pair.Item1].Value = pair.Item2(row.Cells[pair.Item1].Value);
                        }
                        break;
                    }
                }
            }

            // Finally ensure the correct rows are highlighted after operation is complete and redraw the images in the first column.
            foreach (DataGridViewRow row in dgv[currenttab].Rows)
            {
                row.Selected = selectedUIDs.Contains(row.Cells["extid"].Value.ToString());
            }
            dgv[currenttab].Refresh();
        }

        #region (Context -> Status -> Include)
        private void includeToolStripMenuItem_Click(object sender, EventArgs e) {

            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add( new Tuple<string, Del>("status", current => "I") );

            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Status -> Exclude)
        private void excludeToolStripMenuItem_Click(object sender, EventArgs e) {
            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add(new Tuple<string, Del>("status", current => "X"));
            valuesList.Add(new Tuple<string, Del>("displaystatus", current => Properties.Resources.x));

            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Status -> Extra)
        private void extraToolStripMenuItem_Click(object sender, EventArgs e) {
            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add(new Tuple<string, Del>("status", current => "E"));
            valuesList.Add(new Tuple<string, Del>("displaystatus", current => Properties.Resources.add));

            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Status -> Toggle)
        private void toggleStatusToolStripMenuItem_Click(object sender, EventArgs e) {
            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add(new Tuple<string, Del>("status", current => current == "I" ? "X" : "I"));
            valuesList.Add(new Tuple<string, Del>("displaystatus", current => current == Properties.Resources.x ? Properties.Resources.check
                                                                                                                : Properties.Resources.x));
            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Condition -> New)
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add(new Tuple<string, Del>("condition", current => "N"));

            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Condition -> Used)
        private void usedToolStripMenuItem_Click(object sender, EventArgs e) {
            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add(new Tuple<string, Del>("condition", current => "U"));

            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Condition -> Toggle)
        private void toggleCondToolStripMenuItem1_Click(object sender, EventArgs e) {
            List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
            valuesList.Add(new Tuple<string, Del>("condition", current => current == "U" ? "N" : "U"));

            SortSafeSet(valuesList);
        }
        #endregion

        #region (Context -> Set Colour...)
        private void setColourToolStripMenuItem_Click(object sender, EventArgs e) {
            setColourDialog();
        }
        #endregion

        #region (Context -> Quantity -> Multiply)
        private void multiplyToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult dialogresult = multiplyItemsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("qty", current => (int)current * multiplyItemsWindow.num));
                valuesList.Add(new Tuple<string, Del>("total", current => (decimal)current * multiplyItemsWindow.num));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Quantity -> Divide)
        private void divideToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult dialogresult = divideItemsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("qty", current => (int)current / multiplyItemsWindow.num));
                valuesList.Add(new Tuple<string, Del>("total", current => (decimal)current / multiplyItemsWindow.num));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Quantity -> Add)
        private void addToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult dialogresult = addItemsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {

                    row.Cells["qty"].Value = (int)row.Cells["qty"].Value + addItemsWindow.num;
                    if ((int)row.Cells["qty"].Value < 0)
                        row.Cells["qty"].Value = 0;
                    row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                }
            }
        }
        #endregion

        #region (Context -> Quantity -> Subtract)
        private void subtractToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult dialogresult = subtractItemsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                    row.Cells["qty"].Value = (int)row.Cells["qty"].Value - subtractItemsWindow.num;
                    if ((int)row.Cells["qty"].Value < 0)
                        row.Cells["qty"].Value = 0;
                    row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                }
            }
        }
        #endregion

        #region (Context -> Price -> Set)
        private void setToolStripMenuItem_Click(object sender, EventArgs e) {
            setPriceWindow.num = Convert.ToDecimal(dgv[currenttab].SelectedRows[0].Cells["price"].Value);

            DialogResult dialogresult = setPriceWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                    row.Cells["price"].Value = setPriceWindow.num;
                    row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                }
            }
        }
        #endregion

        #region (Context -> Price -> Inc or Dec)
        private void incOrDecToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                setCommentsWindow.text = row.Cells["comments"].Value.ToString();
                break;
            }
            DialogResult dialogresult = incdecPriceWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                    if (incdecPriceWindow.percent) {
                        if (incdecPriceWindow.increase) {
                            row.Cells["price"].Value = (decimal)row.Cells["price"].Value + ((decimal)row.Cells["price"].Value * incdecPriceWindow.num / 100);
                            row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                        } else {
                            row.Cells["price"].Value = (decimal)row.Cells["price"].Value - ((decimal)row.Cells["price"].Value * incdecPriceWindow.num / 100);
                            row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                        }
                    } else {
                        if (incdecPriceWindow.increase) {
                            row.Cells["price"].Value = (decimal)row.Cells["price"].Value + incdecPriceWindow.num;
                            row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                        } else {
                            row.Cells["price"].Value = (decimal)row.Cells["price"].Value - incdecPriceWindow.num;
                            row.Cells["total"].Value = (int)row.Cells["qty"].Value * (decimal)row.Cells["price"].Value;
                        }
                    }
                }
            }
        }
        #endregion

        #region (Context -> Price -> Price Guide)
        #endregion

        #region (Context -> Comments -> Set)
        private void setToolStripMenuItem1_Click(object sender, EventArgs e) {
            setCommentsWindow.text = dgv[currenttab].SelectedRows[0].Cells["comments"].Value.ToString();

            DialogResult dialogresult = setCommentsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {
                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("comments", current => setCommentsWindow.text));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Comments -> Add)
        private void addToToolStripMenuItem_Click(object sender, EventArgs e) {
            addCommentsWindow.text = "";
            DialogResult dialogresult = addCommentsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {

                Del add = current => {
                    if (current.ToString().Length == 0) 
                        return addCommentsWindow.text;
                    else
                        return current.ToString().Trim(' ') + " " + addCommentsWindow.text.Trim(' ');
                };

                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("comments", add));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Comments -> Remove)
        private void removeFromToolStripMenuItem_Click(object sender, EventArgs e) {
            removeCommentsWindow.text = "";
            DialogResult dialogresult = removeCommentsWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {

                string replace = removeCommentsWindow.text.Trim(' ');
                Del rem = current => {
                    if (current.ToString().ToUpper() == replace.ToUpper()) {
                        current = "";
                    }
                    else {
                        Match midmatch = Regex.Match(current.ToString(), replace, RegexOptions.IgnoreCase);
                        if (midmatch.Success) {
                            current = Regex.Replace(current.ToString(), " " + replace + " ", " ", RegexOptions.IgnoreCase);
                        }
                        Match beginmatch = Regex.Match(current.ToString(), "^" + replace, RegexOptions.IgnoreCase);
                        if (beginmatch.Success) {
                            current = Regex.Replace(current.ToString(), "^" + replace + " ", "", RegexOptions.IgnoreCase);
                        }
                        Match endmatch = Regex.Match(current.ToString(), replace + "$", RegexOptions.IgnoreCase);
                        if (endmatch.Success) {
                            current = Regex.Replace(current.ToString(), " " + replace + "$", "", RegexOptions.IgnoreCase);
                        }
                    }

                    return current;
                };

                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("comments", rem));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Remarks -> Set)
        private void setToolStripMenuItem2_Click(object sender, EventArgs e) {
            setRemarksWindow.text = dgv[currenttab].SelectedRows[0].Cells["remarks"].Value.ToString();
            DialogResult dialogresult = setRemarksWindow.ShowDialog(this);

            if (dialogresult == DialogResult.OK) {
                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("remarks", current => setRemarksWindow.text));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Remarks -> Add)
        private void addToToolStripMenuItem1_Click(object sender, EventArgs e) {
            addRemarksWindow.text = "";
            DialogResult dialogresult = addRemarksWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {

                Del add = current => {
                    if (current.ToString().Length == 0) 
                        return addRemarksWindow.text;
                    else
                        return current.ToString().Trim(' ') + " " + addRemarksWindow.text.Trim(' ');
                };

                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("remarks", add));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> Remarks -> Remove)
        private void removeFromToolStripMenuItem1_Click(object sender, EventArgs e) {
            removeRemarksWindow.text = "";
            DialogResult dialogresult = removeRemarksWindow.ShowDialog(this);
            if (dialogresult == DialogResult.OK) {

                string replace = removeRemarksWindow.text.Trim(' ');
                Del rem = current => {
                    if (current.ToString().ToUpper() == replace.ToUpper()) {
                        current = "";
                    }
                    else {
                        Match midmatch = Regex.Match(current.ToString(), replace, RegexOptions.IgnoreCase);
                        if (midmatch.Success) {
                            current = Regex.Replace(current.ToString(), " " + replace + " ", " ", RegexOptions.IgnoreCase);
                        }
                        Match beginmatch = Regex.Match(current.ToString(), "^" + replace, RegexOptions.IgnoreCase);
                        if (beginmatch.Success) {
                            current = Regex.Replace(current.ToString(), "^" + replace + " ", "", RegexOptions.IgnoreCase);
                        }
                        Match endmatch = Regex.Match(current.ToString(), replace + "$", RegexOptions.IgnoreCase);
                        if (endmatch.Success) {
                            current = Regex.Replace(current.ToString(), " " + replace + "$", "", RegexOptions.IgnoreCase);
                        }
                    }

                    return current;
                };

                List<Tuple<string, Del>> valuesList = new List<Tuple<string, Del>>();
                valuesList.Add(new Tuple<string, Del>("remarks", rem));

                SortSafeSet(valuesList);
            }
        }
        #endregion

        #region (Context -> BL Catalog)
        private void showBricklinkCatalogInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                string url = "https://www.bricklink.com/catalogItem.asp?" + row.Cells["type"].Value.ToString() + "=" + row.Cells["number"].Value.ToString();
                try {
                    System.Diagnostics.Process.Start(url);
                } catch {
                    AddStatus("Error displaying page in your default web browser.");
                }
            }
        }
        #endregion

        #region (Context -> BL Price Guide)
        private void showBricklinkPriceGuideToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                string url = "https://www.bricklink.com/catalogPG.asp?" + row.Cells["type"].Value.ToString() + "=" + row.Cells["number"].Value.ToString() +
                    "&colorID=" + row.Cells["colour"].Value.ToString();
                try
                {
                    System.Diagnostics.Process.Start(url);
                }
                catch
                {
                    AddStatus("Error displaying page in your default web browser.");
                }
            }
        }
        #endregion

        #region (Context -> BL 4sale)
        private void showLotsForSaleOnBricklinkToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv[currenttab].SelectedRows) {
                string url = "https://www.bricklink.com/search.asp?viewFrom=sa&itemType=" + row.Cells["type"].Value.ToString() + "&q=" +
                    row.Cells["number"].Value.ToString() + "&colorID=" + row.Cells["colour"].Value.ToString();
                try
                {
                    System.Diagnostics.Process.Start(url);
                }
                catch
                {
                    AddStatus("Error displaying page in your default web browser.");
                }
            }
        }
        #endregion
    }
        #endregion

    #region Classes




















    #endregion
}
