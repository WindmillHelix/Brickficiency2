namespace Brickficiency
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importLDDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportWantedListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAlgorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.approximationAlgorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopCalculationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.imageTimer = new System.Windows.Forms.Timer(this.components);
            this.itemTimer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.containerList = new System.Windows.Forms.DataGridView();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.setname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLabel = new System.Windows.Forms.Label();
            this.statusQueueTimer = new System.Windows.Forms.Timer(this.components);
            this.calcWorker = new System.ComponentModel.BackgroundWorker();
            this.loadWorker = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openButton = new System.Windows.Forms.ToolStripButton();
            this.saveAsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.importLDDButton = new System.Windows.Forms.ToolStripButton();
            this.importButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.calculateButton = new System.Windows.Forms.ToolStripButton();
            this.stopCalculateButton = new System.Windows.Forms.ToolStripButton();
            this.viewReportButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.addItemToolstripButton = new System.Windows.Forms.ToolStripButton();
            this.dlWorker = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quantityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiplyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.divideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subtractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incOrDecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToPriceGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.addToToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFromToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.showBricklinkCatalogInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLotsForSaleOnBricklinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBricklinkPriceGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSetsInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.containerList)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1002, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripMenuItem,
            this.openMenuItem,
            this.saveAsMenuItem,
            this.importToolStripMenuItem1,
            this.exportToolStripMenuItem,
            this.toolStripSeparator8,
            this.exitMenuItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newFileToolStripMenuItem.Text = "New";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripButton_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openMenuItem.Text = "Open...";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsMenuItem.Text = "Save as...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // importToolStripMenuItem1
            // 
            this.importToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.importLDDToolStripMenuItem});
            this.importToolStripMenuItem1.Name = "importToolStripMenuItem1";
            this.importToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.importToolStripMenuItem1.Text = "Import";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.importToolStripMenuItem.Text = "from Bricklink Wanted List...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importBLWantedMenuItem_Click);
            // 
            // importLDDToolStripMenuItem
            // 
            this.importLDDToolStripMenuItem.Name = "importLDDToolStripMenuItem";
            this.importLDDToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.importLDDToolStripMenuItem.Text = "from LDD/LXF file...";
            this.importLDDToolStripMenuItem.Click += new System.EventHandler(this.importLDDMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportWantedListToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // exportWantedListToolStripMenuItem
            // 
            this.exportWantedListToolStripMenuItem.Name = "exportWantedListToolStripMenuItem";
            this.exportWantedListToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.exportWantedListToolStripMenuItem.Text = "to Bricklink wanted list...";
            this.exportWantedListToolStripMenuItem.Click += new System.EventHandler(this.exportWantedListToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(149, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calculateToolStripMenuItem,
            this.stopCalculationToolStripMenuItem,
            this.viewReportToolStripMenuItem,
            this.toolStripSeparator2,
            this.dlMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // calculateToolStripMenuItem
            // 
            this.calculateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAlgorithmToolStripMenuItem,
            this.approximationAlgorithmToolStripMenuItem});
            this.calculateToolStripMenuItem.Name = "calculateToolStripMenuItem";
            this.calculateToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.calculateToolStripMenuItem.Text = "Calculate";
            // 
            // newAlgorithmToolStripMenuItem
            // 
            this.newAlgorithmToolStripMenuItem.Name = "newAlgorithmToolStripMenuItem";
            this.newAlgorithmToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newAlgorithmToolStripMenuItem.Text = "Best Solution";
            this.newAlgorithmToolStripMenuItem.Click += new System.EventHandler(this.newAlgorithmToolStripMenuItem_Click);
            // 
            // approximationAlgorithmToolStripMenuItem
            // 
            this.approximationAlgorithmToolStripMenuItem.Name = "approximationAlgorithmToolStripMenuItem";
            this.approximationAlgorithmToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.approximationAlgorithmToolStripMenuItem.Text = "Approximate Solution";
            this.approximationAlgorithmToolStripMenuItem.Click += new System.EventHandler(this.approximationAlgorithmToolStripMenuItem_Click);
            // 
            // stopCalculationToolStripMenuItem
            // 
            this.stopCalculationToolStripMenuItem.Name = "stopCalculationToolStripMenuItem";
            this.stopCalculationToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.stopCalculationToolStripMenuItem.Text = "Stop Calculation";
            this.stopCalculationToolStripMenuItem.Click += new System.EventHandler(this.stopCalculationToolStripMenuItem_Click);
            // 
            // viewReportToolStripMenuItem
            // 
            this.viewReportToolStripMenuItem.Name = "viewReportToolStripMenuItem";
            this.viewReportToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.viewReportToolStripMenuItem.Text = "View Report";
            this.viewReportToolStripMenuItem.Click += new System.EventHandler(this.viewReportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(206, 6);
            // 
            // dlMenuItem
            // 
            this.dlMenuItem.Name = "dlMenuItem";
            this.dlMenuItem.Size = new System.Drawing.Size(209, 22);
            this.dlMenuItem.Text = "Redownload BrickLink DB";
            this.dlMenuItem.Click += new System.EventHandler(this.dlMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.bsx";
            this.openFileDialog.Filter = "BrickStore Files (*.bsx)|*.bsx|Bricklink XML Files (*.xml)|*.xml|All files (*.*)|" +
    "*.*";
            this.openFileDialog.Title = "Open a Brickstore File";
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(432, 2);
            this.statusBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusBox.MaxLength = 100000;
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.statusBox.Size = new System.Drawing.Size(424, 177);
            this.statusBox.TabIndex = 0;
            // 
            // imageTimer
            // 
            this.imageTimer.Interval = 50;
            this.imageTimer.Tick += new System.EventHandler(this.imageTimerNew_Tick);
            // 
            // itemTimer
            // 
            this.itemTimer.Interval = 50;
            this.itemTimer.Tick += new System.EventHandler(this.itemTimerNew_Tick);
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer.Panel1MinSize = 200;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.containerList);
            this.splitContainer.Panel2.Controls.Add(this.statusBox);
            this.splitContainer.Panel2.Controls.Add(this.imageLabel);
            this.splitContainer.Panel2MinSize = 200;
            this.splitContainer.Size = new System.Drawing.Size(1002, 467);
            this.splitContainer.SplitterDistance = 259;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 2;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // containerList
            // 
            this.containerList.AllowUserToAddRows = false;
            this.containerList.AllowUserToDeleteRows = false;
            this.containerList.AllowUserToResizeColumns = false;
            this.containerList.AllowUserToResizeRows = false;
            this.containerList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.containerList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.containerList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.containerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.containerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.qty,
            this.setname,
            this.id});
            this.containerList.Location = new System.Drawing.Point(169, 2);
            this.containerList.MultiSelect = false;
            this.containerList.Name = "containerList";
            this.containerList.ReadOnly = true;
            this.containerList.RowHeadersVisible = false;
            this.containerList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.containerList.ShowEditingIcon = false;
            this.containerList.Size = new System.Drawing.Size(258, 176);
            this.containerList.TabIndex = 2;
            this.containerList.Visible = false;
            // 
            // qty
            // 
            this.qty.HeaderText = "#";
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 39;
            // 
            // setname
            // 
            this.setname.HeaderText = "Name";
            this.setname.Name = "setname";
            this.setname.ReadOnly = true;
            this.setname.Width = 60;
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // imageLabel
            // 
            this.imageLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imageLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.imageLabel.Location = new System.Drawing.Point(3, 2);
            this.imageLabel.Name = "imageLabel";
            this.imageLabel.Size = new System.Drawing.Size(160, 177);
            this.imageLabel.TabIndex = 1;
            this.imageLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.imageLabel.Visible = false;
            // 
            // statusQueueTimer
            // 
            this.statusQueueTimer.Interval = 1000;
            this.statusQueueTimer.Tick += new System.EventHandler(this.statusQueueTimer_Tick);
            // 
            // calcWorker
            // 
            this.calcWorker.WorkerSupportsCancellation = true;
            this.calcWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.calcWorker_DoWork);
            // 
            // loadWorker
            // 
            this.loadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loadWorker_DoWork);
            this.loadWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.loadWorker_RunWorkerCompleted);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.bsx";
            this.saveFileDialog1.Filter = "BrickStore Files (*.bsx)|*.bsx";
            this.saveFileDialog1.Title = "Save BSX File";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.statusStrip1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1002, 467);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1002, 494);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.progressBar1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 445);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1002, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // progressBar1
            // 
            this.progressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressBar1.AutoSize = false;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(500, 16);
            this.progressBar1.Step = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripButton,
            this.openButton,
            this.saveAsButton,
            this.toolStripSeparator10,
            this.importLDDButton,
            this.importButton,
            this.toolStripSeparator1,
            this.calculateButton,
            this.stopCalculateButton,
            this.viewReportButton,
            this.toolStripSeparator9,
            this.addItemToolstripButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(246, 27);
            this.toolStrip1.TabIndex = 0;
            // 
            // newFileToolStripButton
            // 
            this.newFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newFileToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newFileToolStripButton.Image")));
            this.newFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newFileToolStripButton.Name = "newFileToolStripButton";
            this.newFileToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.newFileToolStripButton.Text = "New File";
            this.newFileToolStripButton.Click += new System.EventHandler(this.newFileToolStripButton_Click);
            // 
            // openButton
            // 
            this.openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openButton.Image = ((System.Drawing.Image)(resources.GetObject("openButton.Image")));
            this.openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(24, 24);
            this.openButton.Text = "Open";
            this.openButton.ToolTipText = "Open .BSX or Bricklink XML file";
            this.openButton.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveAsButton
            // 
            this.saveAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveAsButton.Image = ((System.Drawing.Image)(resources.GetObject("saveAsButton.Image")));
            this.saveAsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size(24, 24);
            this.saveAsButton.Text = "Save As...";
            this.saveAsButton.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 27);
            // 
            // importLDDButton
            // 
            this.importLDDButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importLDDButton.Image = ((System.Drawing.Image)(resources.GetObject("importLDDButton.Image")));
            this.importLDDButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importLDDButton.Name = "importLDDButton";
            this.importLDDButton.Size = new System.Drawing.Size(24, 24);
            this.importLDDButton.Text = "Import";
            this.importLDDButton.ToolTipText = "Import LDD File";
            this.importLDDButton.Click += new System.EventHandler(this.importLDDMenuItem_Click);
            // 
            // importButton
            // 
            this.importButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importButton.Image = ((System.Drawing.Image)(resources.GetObject("importButton.Image")));
            this.importButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(24, 24);
            this.importButton.Text = "Import";
            this.importButton.ToolTipText = "Import Bricklink Wanted List";
            this.importButton.Click += new System.EventHandler(this.importBLWantedMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // calculateButton
            // 
            this.calculateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.calculateButton.Image = ((System.Drawing.Image)(resources.GetObject("calculateButton.Image")));
            this.calculateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(24, 24);
            this.calculateButton.Text = "Calculate";
            this.calculateButton.ToolTipText = "Find cheapest combinations of Bricklink stores that can supply all items in the c" +
    "urrent list.";
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // stopCalculateButton
            // 
            this.stopCalculateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopCalculateButton.Image = ((System.Drawing.Image)(resources.GetObject("stopCalculateButton.Image")));
            this.stopCalculateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopCalculateButton.Name = "stopCalculateButton";
            this.stopCalculateButton.Size = new System.Drawing.Size(24, 24);
            this.stopCalculateButton.Text = "Stop Calculation";
            this.stopCalculateButton.ToolTipText = "Stop Calculation";
            this.stopCalculateButton.Click += new System.EventHandler(this.stopCalculationToolStripMenuItem_Click);
            // 
            // viewReportButton
            // 
            this.viewReportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.viewReportButton.Image = ((System.Drawing.Image)(resources.GetObject("viewReportButton.Image")));
            this.viewReportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.viewReportButton.Name = "viewReportButton";
            this.viewReportButton.Size = new System.Drawing.Size(24, 24);
            this.viewReportButton.Text = "View Report";
            this.viewReportButton.ToolTipText = "View the most recent report (Includes currently running report)";
            this.viewReportButton.Click += new System.EventHandler(this.viewReportToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 27);
            // 
            // addItemToolstripButton
            // 
            this.addItemToolstripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addItemToolstripButton.Image = ((System.Drawing.Image)(resources.GetObject("addItemToolstripButton.Image")));
            this.addItemToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addItemToolstripButton.Name = "addItemToolstripButton";
            this.addItemToolstripButton.Size = new System.Drawing.Size(24, 24);
            this.addItemToolstripButton.Text = "Add Item...";
            this.addItemToolstripButton.Click += new System.EventHandler(this.addItemToolstripButton_Click);
            // 
            // dlWorker
            // 
            this.dlWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dlWorker_DoWork);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.toolStripSeparator7,
            this.selectAllToolStripMenuItem,
            this.invertSelectionToolStripMenuItem,
            this.toolStripSeparator5,
            this.statusToolStripMenuItem,
            this.conditionToolStripMenuItem,
            this.setColourToolStripMenuItem,
            this.quantityToolStripMenuItem,
            this.priceToolStripMenuItem,
            this.commentToolStripMenuItem,
            this.remarkToolStripMenuItem,
            this.toolStripSeparator6,
            this.showBricklinkCatalogInfoToolStripMenuItem,
            this.showLotsForSaleOnBricklinkToolStripMenuItem,
            this.showBricklinkPriceGuideToolStripMenuItem,
            this.showSetsInToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(235, 330);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(231, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.invertSelectionToolStripMenuItem.Text = "Invert Selection";
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(231, 6);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.includeToolStripMenuItem,
            this.excludeToolStripMenuItem,
            this.extraToolStripMenuItem,
            this.toolStripSeparator3,
            this.toggleToolStripMenuItem});
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.statusToolStripMenuItem.Text = "Status";
            // 
            // includeToolStripMenuItem
            // 
            this.includeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("includeToolStripMenuItem.Image")));
            this.includeToolStripMenuItem.Name = "includeToolStripMenuItem";
            this.includeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.includeToolStripMenuItem.Text = "Include";
            this.includeToolStripMenuItem.Click += new System.EventHandler(this.includeToolStripMenuItem_Click);
            // 
            // excludeToolStripMenuItem
            // 
            this.excludeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("excludeToolStripMenuItem.Image")));
            this.excludeToolStripMenuItem.Name = "excludeToolStripMenuItem";
            this.excludeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.excludeToolStripMenuItem.Text = "Exclude";
            this.excludeToolStripMenuItem.Click += new System.EventHandler(this.excludeToolStripMenuItem_Click);
            // 
            // extraToolStripMenuItem
            // 
            this.extraToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("extraToolStripMenuItem.Image")));
            this.extraToolStripMenuItem.Name = "extraToolStripMenuItem";
            this.extraToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.extraToolStripMenuItem.Text = "Extra";
            this.extraToolStripMenuItem.Click += new System.EventHandler(this.extraToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(111, 6);
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            this.toggleToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.toggleToolStripMenuItem.Text = "Toggle";
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleStatusToolStripMenuItem_Click);
            // 
            // conditionToolStripMenuItem
            // 
            this.conditionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.usedToolStripMenuItem,
            this.toolStripSeparator4,
            this.toggleToolStripMenuItem1});
            this.conditionToolStripMenuItem.Name = "conditionToolStripMenuItem";
            this.conditionToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.conditionToolStripMenuItem.Text = "Condition";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // usedToolStripMenuItem
            // 
            this.usedToolStripMenuItem.Name = "usedToolStripMenuItem";
            this.usedToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.usedToolStripMenuItem.Text = "Used";
            this.usedToolStripMenuItem.Click += new System.EventHandler(this.usedToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(107, 6);
            // 
            // toggleToolStripMenuItem1
            // 
            this.toggleToolStripMenuItem1.Name = "toggleToolStripMenuItem1";
            this.toggleToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.toggleToolStripMenuItem1.Text = "Toggle";
            this.toggleToolStripMenuItem1.Click += new System.EventHandler(this.toggleCondToolStripMenuItem1_Click);
            // 
            // setColourToolStripMenuItem
            // 
            this.setColourToolStripMenuItem.Name = "setColourToolStripMenuItem";
            this.setColourToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setColourToolStripMenuItem.Text = "Set Colour...";
            this.setColourToolStripMenuItem.Click += new System.EventHandler(this.setColourToolStripMenuItem_Click);
            // 
            // quantityToolStripMenuItem
            // 
            this.quantityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiplyToolStripMenuItem,
            this.divideToolStripMenuItem,
            this.addToolStripMenuItem,
            this.subtractToolStripMenuItem});
            this.quantityToolStripMenuItem.Name = "quantityToolStripMenuItem";
            this.quantityToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.quantityToolStripMenuItem.Text = "Quantity";
            // 
            // multiplyToolStripMenuItem
            // 
            this.multiplyToolStripMenuItem.Name = "multiplyToolStripMenuItem";
            this.multiplyToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.multiplyToolStripMenuItem.Text = "Multiply...";
            this.multiplyToolStripMenuItem.Click += new System.EventHandler(this.multiplyToolStripMenuItem_Click);
            // 
            // divideToolStripMenuItem
            // 
            this.divideToolStripMenuItem.Name = "divideToolStripMenuItem";
            this.divideToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.divideToolStripMenuItem.Text = "Divide...";
            this.divideToolStripMenuItem.Click += new System.EventHandler(this.divideToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.addToolStripMenuItem.Text = "Add...";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // subtractToolStripMenuItem
            // 
            this.subtractToolStripMenuItem.Name = "subtractToolStripMenuItem";
            this.subtractToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.subtractToolStripMenuItem.Text = "Subtract...";
            this.subtractToolStripMenuItem.Click += new System.EventHandler(this.subtractToolStripMenuItem_Click);
            // 
            // priceToolStripMenuItem
            // 
            this.priceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem,
            this.incOrDecToolStripMenuItem,
            this.setToPriceGuideToolStripMenuItem});
            this.priceToolStripMenuItem.Name = "priceToolStripMenuItem";
            this.priceToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.priceToolStripMenuItem.Text = "Price";
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.setToolStripMenuItem.Text = "Set...";
            this.setToolStripMenuItem.Click += new System.EventHandler(this.setToolStripMenuItem_Click);
            // 
            // incOrDecToolStripMenuItem
            // 
            this.incOrDecToolStripMenuItem.Name = "incOrDecToolStripMenuItem";
            this.incOrDecToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.incOrDecToolStripMenuItem.Text = "Inc. or Dec...";
            this.incOrDecToolStripMenuItem.Click += new System.EventHandler(this.incOrDecToolStripMenuItem_Click);
            // 
            // setToPriceGuideToolStripMenuItem
            // 
            this.setToPriceGuideToolStripMenuItem.Name = "setToPriceGuideToolStripMenuItem";
            this.setToPriceGuideToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.setToPriceGuideToolStripMenuItem.Text = "Set to Price Guide...";
            this.setToPriceGuideToolStripMenuItem.Visible = false;
            // 
            // commentToolStripMenuItem
            // 
            this.commentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem1,
            this.addToToolStripMenuItem,
            this.removeFromToolStripMenuItem});
            this.commentToolStripMenuItem.Name = "commentToolStripMenuItem";
            this.commentToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.commentToolStripMenuItem.Text = "Comment";
            // 
            // setToolStripMenuItem1
            // 
            this.setToolStripMenuItem1.Name = "setToolStripMenuItem1";
            this.setToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.setToolStripMenuItem1.Text = "Set...";
            this.setToolStripMenuItem1.Click += new System.EventHandler(this.setToolStripMenuItem1_Click);
            // 
            // addToToolStripMenuItem
            // 
            this.addToToolStripMenuItem.Name = "addToToolStripMenuItem";
            this.addToToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.addToToolStripMenuItem.Text = "Add to...";
            this.addToToolStripMenuItem.Click += new System.EventHandler(this.addToToolStripMenuItem_Click);
            // 
            // removeFromToolStripMenuItem
            // 
            this.removeFromToolStripMenuItem.Name = "removeFromToolStripMenuItem";
            this.removeFromToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.removeFromToolStripMenuItem.Text = "Remove from...";
            this.removeFromToolStripMenuItem.Click += new System.EventHandler(this.removeFromToolStripMenuItem_Click);
            // 
            // remarkToolStripMenuItem
            // 
            this.remarkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem2,
            this.addToToolStripMenuItem1,
            this.removeFromToolStripMenuItem1});
            this.remarkToolStripMenuItem.Name = "remarkToolStripMenuItem";
            this.remarkToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.remarkToolStripMenuItem.Text = "Remark";
            // 
            // setToolStripMenuItem2
            // 
            this.setToolStripMenuItem2.Name = "setToolStripMenuItem2";
            this.setToolStripMenuItem2.Size = new System.Drawing.Size(155, 22);
            this.setToolStripMenuItem2.Text = "Set...";
            this.setToolStripMenuItem2.Click += new System.EventHandler(this.setToolStripMenuItem2_Click);
            // 
            // addToToolStripMenuItem1
            // 
            this.addToToolStripMenuItem1.Name = "addToToolStripMenuItem1";
            this.addToToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.addToToolStripMenuItem1.Text = "Add to...";
            this.addToToolStripMenuItem1.Click += new System.EventHandler(this.addToToolStripMenuItem1_Click);
            // 
            // removeFromToolStripMenuItem1
            // 
            this.removeFromToolStripMenuItem1.Name = "removeFromToolStripMenuItem1";
            this.removeFromToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.removeFromToolStripMenuItem1.Text = "Remove from...";
            this.removeFromToolStripMenuItem1.Click += new System.EventHandler(this.removeFromToolStripMenuItem1_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(231, 6);
            // 
            // showBricklinkCatalogInfoToolStripMenuItem
            // 
            this.showBricklinkCatalogInfoToolStripMenuItem.Name = "showBricklinkCatalogInfoToolStripMenuItem";
            this.showBricklinkCatalogInfoToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.showBricklinkCatalogInfoToolStripMenuItem.Text = "Show Bricklink Catalog Info...";
            this.showBricklinkCatalogInfoToolStripMenuItem.Click += new System.EventHandler(this.showBricklinkCatalogInfoToolStripMenuItem_Click);
            // 
            // showLotsForSaleOnBricklinkToolStripMenuItem
            // 
            this.showLotsForSaleOnBricklinkToolStripMenuItem.Name = "showLotsForSaleOnBricklinkToolStripMenuItem";
            this.showLotsForSaleOnBricklinkToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.showLotsForSaleOnBricklinkToolStripMenuItem.Text = "Show Lots for sale on Bricklink";
            this.showLotsForSaleOnBricklinkToolStripMenuItem.Click += new System.EventHandler(this.showLotsForSaleOnBricklinkToolStripMenuItem_Click);
            // 
            // showBricklinkPriceGuideToolStripMenuItem
            // 
            this.showBricklinkPriceGuideToolStripMenuItem.Name = "showBricklinkPriceGuideToolStripMenuItem";
            this.showBricklinkPriceGuideToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.showBricklinkPriceGuideToolStripMenuItem.Text = "Show Bricklink Price Guide";
            this.showBricklinkPriceGuideToolStripMenuItem.Click += new System.EventHandler(this.showBricklinkPriceGuideToolStripMenuItem_Click);
            // 
            // showSetsInToolStripMenuItem
            // 
            this.showSetsInToolStripMenuItem.Name = "showSetsInToolStripMenuItem";
            this.showSetsInToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.showSetsInToolStripMenuItem.Text = "Show sets containing item";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 518);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 479);
            this.Name = "MainWindow";
            this.Text = "Brickficiency";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.InitStuff);
            this.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.containerList)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer imageTimer;
        private System.Windows.Forms.Timer itemTimer;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        public System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculateToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Timer statusQueueTimer;
        private System.ComponentModel.BackgroundWorker calcWorker;
        private System.ComponentModel.BackgroundWorker loadWorker;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.ToolStripMenuItem stopCalculationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton saveAsButton;
        private System.Windows.Forms.ToolStripButton importButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton calculateButton;
        private System.Windows.Forms.ToolStripButton stopCalculateButton;
        private System.Windows.Forms.ToolStripButton viewReportButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem dlMenuItem;
        private System.ComponentModel.BackgroundWorker dlWorker;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem includeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excludeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quantityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiplyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem divideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem incOrDecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToPriceGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem addToToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeFromToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem invertSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem showBricklinkCatalogInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBricklinkPriceGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLotsForSaleOnBricklinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importLDDToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton importLDDButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem showSetsInToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton addItemToolstripButton;
        private System.Windows.Forms.ToolStripButton newFileToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.Label imageLabel;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportWantedListToolStripMenuItem;
        private System.Windows.Forms.DataGridView containerList;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn setname;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.ToolStripMenuItem newAlgorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem approximationAlgorithmToolStripMenuItem;
    }
}

