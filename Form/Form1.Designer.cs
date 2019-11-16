namespace SinoTunnel
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.VarDiaDecreasedXP = new System.Windows.Forms.Label();
            this.TransportationMText = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.StackingMText = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.HangingMText = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.btn_SGSelf = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.TransportationVText = new System.Windows.Forms.TextBox();
            this.StackingVText = new System.Windows.Forms.TextBox();
            this.HangingVText = new System.Windows.Forms.TextBox();
            this.Cal03_output = new System.Windows.Forms.TextBox();
            this.STN_StrainCheck_Internal = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.ODEVCStrainText = new System.Windows.Forms.TextBox();
            this.MDEVCStrainText = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.GroutingInternalCal = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.LongtermE1_In = new System.Windows.Forms.TextBox();
            this.btn_VerticalStressInternal = new System.Windows.Forms.Button();
            this.GroutingM = new System.Windows.Forms.TextBox();
            this.GroutingP = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.VSExternal = new System.Windows.Forms.Button();
            this.LongTermE1_Ex = new System.Windows.Forms.TextBox();
            this.ShortermE1_In = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.ShorTermE1 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.LoadingtermInternalCal = new System.Windows.Forms.Button();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.LongTermInitialCheckBox = new System.Windows.Forms.CheckBox();
            this.ShortTermInitialCheckBox = new System.Windows.Forms.CheckBox();
            this.VarDiaInitialCheckBox = new System.Windows.Forms.CheckBox();
            this.SeismicCheckBox = new System.Windows.Forms.CheckBox();
            this.EQDiaIncreasedZP = new System.Windows.Forms.Label();
            this.VarDiaXPInput = new System.Windows.Forms.TextBox();
            this.EQDiaInput = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.輸入參數 = new System.Windows.Forms.TabPage();
            this.checkB_Site = new System.Windows.Forms.CheckBox();
            this.checkB_Connector_Steel = new System.Windows.Forms.CheckBox();
            this.checkB_Precast = new System.Windows.Forms.CheckBox();
            this.btn_InputFrameMaterial = new System.Windows.Forms.Button();
            this.label_SectionFunction = new System.Windows.Forms.Label();
            this.ChosenProject = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.lbl_User1 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.吊放等計算 = new System.Windows.Forms.TabPage();
            this.STN_SegmentSelfInternal = new System.Windows.Forms.Button();
            this.土壤E值 = new System.Windows.Forms.TabPage();
            this.cbo_LoadingCal = new System.Windows.Forms.ComboBox();
            this.環片應變 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.ODETTStrainText = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.ODEVTStrainText = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.ODETCStrainText = new System.Windows.Forms.TextBox();
            this.MDETTStrainText = new System.Windows.Forms.TextBox();
            this.MDETCStrainText = new System.Windows.Forms.TextBox();
            this.MDEVTStrainText = new System.Windows.Forms.TextBox();
            this.環片分析 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.btn_ExcelOutput = new System.Windows.Forms.TabPage();
            this.btn_GroutExcel1 = new System.Windows.Forms.Button();
            this.btn_LoadingOutExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.RadiusIn = new System.Windows.Forms.TextBox();
            this.Thickness = new System.Windows.Forms.TextBox();
            this.Angle = new System.Windows.Forms.TextBox();
            this.AAngle = new System.Windows.Forms.TextBox();
            this.BAngle = new System.Windows.Forms.TextBox();
            this.KAngle = new System.Windows.Forms.TextBox();
            this.AdjacentPoreAngle = new System.Windows.Forms.TextBox();
            this.AdjacentGroutAngle = new System.Windows.Forms.TextBox();
            this.UnitWeight = new System.Windows.Forms.TextBox();
            this.EQDiaInitial = new System.Windows.Forms.TextBox();
            this.VarDiaXPInitial = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.穩定性檢核 = new System.Windows.Forms.TabPage();
            this.參考用 = new System.Windows.Forms.TabPage();
            this.網頁 = new System.Windows.Forms.TabPage();
            this.web = new System.Windows.Forms.WebBrowser();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabSection = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cbo_Section = new System.Windows.Forms.ComboBox();
            this.label74 = new System.Windows.Forms.Label();
            this.lbl_SectionFunction = new System.Windows.Forms.Label();
            this.lbl_ChosenProject = new System.Windows.Forms.Label();
            this.lbl_User = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.tabTunnel = new System.Windows.Forms.TabPage();
            this.gBox_LoadTerm = new System.Windows.Forms.GroupBox();
            this.btn_LoadTermSAP = new System.Windows.Forms.Button();
            this.gBox_VerInc = new System.Windows.Forms.GroupBox();
            this.txt_VerIncRepeat = new System.Windows.Forms.TextBox();
            this.txt_VerIncInitial = new System.Windows.Forms.TextBox();
            this.label100 = new System.Windows.Forms.Label();
            this.label98 = new System.Windows.Forms.Label();
            this.label96 = new System.Windows.Forms.Label();
            this.label99 = new System.Windows.Forms.Label();
            this.label97 = new System.Windows.Forms.Label();
            this.lbl_VerIncValue = new System.Windows.Forms.Label();
            this.gBox_LatDec = new System.Windows.Forms.GroupBox();
            this.txt_LatDecRepeat = new System.Windows.Forms.TextBox();
            this.txt_LatDecInitial = new System.Windows.Forms.TextBox();
            this.label94 = new System.Windows.Forms.Label();
            this.label101 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.lbl_LatDecValue = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.btn_LoadTermExcel = new System.Windows.Forms.Button();
            this.cbo_LoadingTerm = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gBox_Grout = new System.Windows.Forms.GroupBox();
            this.label91 = new System.Windows.Forms.Label();
            this.btn_GroutExcel = new System.Windows.Forms.Button();
            this.btn_GroutSAP = new System.Windows.Forms.Button();
            this.lbl_GroutP = new System.Windows.Forms.Label();
            this.gBox_SGProp = new System.Windows.Forms.GroupBox();
            this.lbl_SGFy = new System.Windows.Forms.Label();
            this.lbl_SGFc = new System.Windows.Forms.Label();
            this.lbl_SGPoissonR = new System.Windows.Forms.Label();
            this.lbl_SGE = new System.Windows.Forms.Label();
            this.lbl_SGUW = new System.Windows.Forms.Label();
            this.lbl_SGGroutAngle = new System.Windows.Forms.Label();
            this.lbl_SGPoreAngle = new System.Windows.Forms.Label();
            this.lbl_SGKAngle = new System.Windows.Forms.Label();
            this.lbl_SGBAngle = new System.Windows.Forms.Label();
            this.lbl_SGAAngle = new System.Windows.Forms.Label();
            this.lbl_SGAngle = new System.Windows.Forms.Label();
            this.lbl_SGThick = new System.Windows.Forms.Label();
            this.lbl_SGRadiusIn = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.tabConnect = new System.Windows.Forms.TabPage();
            this.gpConnectTunnel = new System.Windows.Forms.GroupBox();
            this.gBox_ConnectorCal = new System.Windows.Forms.GroupBox();
            this.btn_ConnectorExcel = new System.Windows.Forms.Button();
            this.btn_ConnectorCal = new System.Windows.Forms.Button();
            this.gBOX_ConnectorProp = new System.Windows.Forms.GroupBox();
            this.lbl_ConnectorFy = new System.Windows.Forms.Label();
            this.lbl_ConnectorFc = new System.Windows.Forms.Label();
            this.lbl_ConnectorPR = new System.Windows.Forms.Label();
            this.lbl_ConnectorE = new System.Windows.Forms.Label();
            this.lbl_TR = new System.Windows.Forms.Label();
            this.lbl_BH = new System.Windows.Forms.Label();
            this.lbl_ConnectorUW = new System.Windows.Forms.Label();
            this.label107 = new System.Windows.Forms.Label();
            this.label108 = new System.Windows.Forms.Label();
            this.label109 = new System.Windows.Forms.Label();
            this.label110 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.label112 = new System.Windows.Forms.Label();
            this.label111 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabSteelTunnel = new System.Windows.Forms.TabPage();
            this.gpSteelTunnel = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_SteelExcel = new System.Windows.Forms.Button();
            this.btn_SteelCal = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbl_SteelPoreAngle = new System.Windows.Forms.Label();
            this.lbl_SteelKAngle = new System.Windows.Forms.Label();
            this.lbl_SteelBAngle = new System.Windows.Forms.Label();
            this.lbl_SteelAAngle = new System.Windows.Forms.Label();
            this.lbl_SteelThick = new System.Windows.Forms.Label();
            this.lbl_SteelRadiusIn = new System.Windows.Forms.Label();
            this.label130 = new System.Windows.Forms.Label();
            this.label131 = new System.Windows.Forms.Label();
            this.label139 = new System.Windows.Forms.Label();
            this.label140 = new System.Windows.Forms.Label();
            this.label141 = new System.Windows.Forms.Label();
            this.label142 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbl_SteelFu = new System.Windows.Forms.Label();
            this.lbl_SteelFy = new System.Windows.Forms.Label();
            this.lbl_SteelPR = new System.Windows.Forms.Label();
            this.lbl_SteelMat = new System.Windows.Forms.Label();
            this.lbl_SteelUW = new System.Windows.Forms.Label();
            this.label117 = new System.Windows.Forms.Label();
            this.label118 = new System.Windows.Forms.Label();
            this.label119 = new System.Windows.Forms.Label();
            this.label127 = new System.Windows.Forms.Label();
            this.label121 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_SteelConcreFy = new System.Windows.Forms.Label();
            this.lbl_SteelConcreFc = new System.Windows.Forms.Label();
            this.lbl_SteelConcrePR = new System.Windows.Forms.Label();
            this.lbl_SteelConcreE = new System.Windows.Forms.Label();
            this.lbl_SteelConcreUW = new System.Windows.Forms.Label();
            this.label122 = new System.Windows.Forms.Label();
            this.label123 = new System.Windows.Forms.Label();
            this.label124 = new System.Windows.Forms.Label();
            this.label125 = new System.Windows.Forms.Label();
            this.label126 = new System.Windows.Forms.Label();
            this.tabSiteTunnel = new System.Windows.Forms.TabPage();
            this.gpSiteTunnel = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btn_SiteExcel = new System.Windows.Forms.Button();
            this.btn_SiteCal = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lbl_SiteFy = new System.Windows.Forms.Label();
            this.lbl_SiteFc = new System.Windows.Forms.Label();
            this.lbl_SiteU12 = new System.Windows.Forms.Label();
            this.lbl_SiteE = new System.Windows.Forms.Label();
            this.lbl_SiteUW = new System.Windows.Forms.Label();
            this.lbl_SiteGroutAngle = new System.Windows.Forms.Label();
            this.lbl_SitePoreAngle = new System.Windows.Forms.Label();
            this.lbl_SiteKangle = new System.Windows.Forms.Label();
            this.lbl_SiteBAngle = new System.Windows.Forms.Label();
            this.lbl_SiteAAngle = new System.Windows.Forms.Label();
            this.lbl_SiteThick = new System.Windows.Forms.Label();
            this.lbl_SiteRadiusIn = new System.Windows.Forms.Label();
            this.label114 = new System.Windows.Forms.Label();
            this.label115 = new System.Windows.Forms.Label();
            this.label116 = new System.Windows.Forms.Label();
            this.label120 = new System.Windows.Forms.Label();
            this.label128 = new System.Windows.Forms.Label();
            this.label129 = new System.Windows.Forms.Label();
            this.label132 = new System.Windows.Forms.Label();
            this.label134 = new System.Windows.Forms.Label();
            this.label135 = new System.Windows.Forms.Label();
            this.label136 = new System.Windows.Forms.Label();
            this.label137 = new System.Windows.Forms.Label();
            this.label138 = new System.Windows.Forms.Label();
            this.tabContDepth = new System.Windows.Forms.TabPage();
            this.FigPanel = new System.Windows.Forms.Panel();
            this.Fig04 = new LiveCharts.WinForms.CartesianChart();
            this.Fig02 = new LiveCharts.WinForms.CartesianChart();
            this.btn_FinalCal = new System.Windows.Forms.Button();
            this.Fig_ComboBox = new System.Windows.Forms.ComboBox();
            this.Fig12_Depth = new System.Windows.Forms.TextBox();
            this.Fig08_Depth = new System.Windows.Forms.TextBox();
            this.Fig04_Depth = new System.Windows.Forms.TextBox();
            this.Fig11_Depth = new System.Windows.Forms.TextBox();
            this.Fig07_Depth = new System.Windows.Forms.TextBox();
            this.Fig03_Depth = new System.Windows.Forms.TextBox();
            this.Fig10_Depth = new System.Windows.Forms.TextBox();
            this.Fig06_Depth = new System.Windows.Forms.TextBox();
            this.Fig02_Depth = new System.Windows.Forms.TextBox();
            this.Fig09_Depth = new System.Windows.Forms.TextBox();
            this.Fig05_Depth = new System.Windows.Forms.TextBox();
            this.Fig01_Depth = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.Fig12 = new LiveCharts.WinForms.CartesianChart();
            this.Fig08 = new LiveCharts.WinForms.CartesianChart();
            this.Fig11 = new LiveCharts.WinForms.CartesianChart();
            this.Fig07 = new LiveCharts.WinForms.CartesianChart();
            this.Fig03 = new LiveCharts.WinForms.CartesianChart();
            this.Fig10 = new LiveCharts.WinForms.CartesianChart();
            this.Fig06 = new LiveCharts.WinForms.CartesianChart();
            this.Fig09 = new LiveCharts.WinForms.CartesianChart();
            this.Fig05 = new LiveCharts.WinForms.CartesianChart();
            this.Fig01 = new LiveCharts.WinForms.CartesianChart();
            this.tabOthers = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.結束ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.選擇標案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.輸入參數.SuspendLayout();
            this.吊放等計算.SuspendLayout();
            this.土壤E值.SuspendLayout();
            this.環片應變.SuspendLayout();
            this.環片分析.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.btn_ExcelOutput.SuspendLayout();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.參考用.SuspendLayout();
            this.網頁.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabSection.SuspendLayout();
            this.tabTunnel.SuspendLayout();
            this.gBox_LoadTerm.SuspendLayout();
            this.gBox_VerInc.SuspendLayout();
            this.gBox_LatDec.SuspendLayout();
            this.gBox_Grout.SuspendLayout();
            this.gBox_SGProp.SuspendLayout();
            this.tabConnect.SuspendLayout();
            this.gpConnectTunnel.SuspendLayout();
            this.gBox_ConnectorCal.SuspendLayout();
            this.gBOX_ConnectorProp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabSteelTunnel.SuspendLayout();
            this.gpSteelTunnel.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSiteTunnel.SuspendLayout();
            this.gpSiteTunnel.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabContDepth.SuspendLayout();
            this.FigPanel.SuspendLayout();
            this.tabOthers.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // VarDiaDecreasedXP
            // 
            this.VarDiaDecreasedXP.AutoSize = true;
            this.VarDiaDecreasedXP.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.VarDiaDecreasedXP.Location = new System.Drawing.Point(550, 217);
            this.VarDiaDecreasedXP.Name = "VarDiaDecreasedXP";
            this.VarDiaDecreasedXP.Size = new System.Drawing.Size(62, 18);
            this.VarDiaDecreasedXP.TabIndex = 0;
            this.VarDiaDecreasedXP.Text = "0.33Dec";
            // 
            // TransportationMText
            // 
            this.TransportationMText.Location = new System.Drawing.Point(196, 94);
            this.TransportationMText.Name = "TransportationMText";
            this.TransportationMText.Size = new System.Drawing.Size(63, 25);
            this.TransportationMText.TabIndex = 10;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label19.Location = new System.Drawing.Point(18, 91);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(178, 25);
            this.label19.TabIndex = 0;
            this.label19.Text = "Transportation：";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label20.Location = new System.Drawing.Point(18, 127);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(116, 25);
            this.label20.TabIndex = 0;
            this.label20.Text = "Stacking：";
            // 
            // StackingMText
            // 
            this.StackingMText.Location = new System.Drawing.Point(196, 130);
            this.StackingMText.Name = "StackingMText";
            this.StackingMText.Size = new System.Drawing.Size(63, 25);
            this.StackingMText.TabIndex = 10;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label21.Location = new System.Drawing.Point(18, 164);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(117, 25);
            this.label21.TabIndex = 0;
            this.label21.Text = "Hanging：";
            // 
            // HangingMText
            // 
            this.HangingMText.Location = new System.Drawing.Point(196, 167);
            this.HangingMText.Name = "HangingMText";
            this.HangingMText.Size = new System.Drawing.Size(63, 25);
            this.HangingMText.TabIndex = 10;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label22.Location = new System.Drawing.Point(18, 34);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(108, 25);
            this.label22.TabIndex = 0;
            this.label22.Text = "Internal：";
            // 
            // btn_SGSelf
            // 
            this.btn_SGSelf.Location = new System.Drawing.Point(165, 29);
            this.btn_SGSelf.Name = "btn_SGSelf";
            this.btn_SGSelf.Size = new System.Drawing.Size(102, 30);
            this.btn_SGSelf.TabIndex = 11;
            this.btn_SGSelf.Text = "計算";
            this.btn_SGSelf.UseVisualStyleBackColor = true;
            this.btn_SGSelf.Click += new System.EventHandler(this.btn_SGSelf_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label23.Location = new System.Drawing.Point(199, 66);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 21);
            this.label23.TabIndex = 0;
            this.label23.Text = "Mmax";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label24.Location = new System.Drawing.Point(279, 66);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(54, 21);
            this.label24.TabIndex = 0;
            this.label24.Text = "Vmax";
            // 
            // TransportationVText
            // 
            this.TransportationVText.Location = new System.Drawing.Point(273, 93);
            this.TransportationVText.Name = "TransportationVText";
            this.TransportationVText.Size = new System.Drawing.Size(63, 25);
            this.TransportationVText.TabIndex = 12;
            // 
            // StackingVText
            // 
            this.StackingVText.Location = new System.Drawing.Point(273, 131);
            this.StackingVText.Name = "StackingVText";
            this.StackingVText.Size = new System.Drawing.Size(63, 25);
            this.StackingVText.TabIndex = 12;
            // 
            // HangingVText
            // 
            this.HangingVText.Location = new System.Drawing.Point(273, 167);
            this.HangingVText.Name = "HangingVText";
            this.HangingVText.Size = new System.Drawing.Size(63, 25);
            this.HangingVText.TabIndex = 12;
            // 
            // Cal03_output
            // 
            this.Cal03_output.Location = new System.Drawing.Point(268, 55);
            this.Cal03_output.Name = "Cal03_output";
            this.Cal03_output.Size = new System.Drawing.Size(100, 25);
            this.Cal03_output.TabIndex = 13;
            // 
            // STN_StrainCheck_Internal
            // 
            this.STN_StrainCheck_Internal.Location = new System.Drawing.Point(118, 14);
            this.STN_StrainCheck_Internal.Name = "STN_StrainCheck_Internal";
            this.STN_StrainCheck_Internal.Size = new System.Drawing.Size(100, 25);
            this.STN_StrainCheck_Internal.TabIndex = 14;
            this.STN_StrainCheck_Internal.Text = "環片應變_內部";
            this.STN_StrainCheck_Internal.UseVisualStyleBackColor = true;
            this.STN_StrainCheck_Internal.Click += new System.EventHandler(this.STN_StrainCheck_Internal_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label25.Location = new System.Drawing.Point(19, 91);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(134, 25);
            this.label25.TabIndex = 0;
            this.label25.Text = "ODE壓應變：";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label26.Location = new System.Drawing.Point(19, 156);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(138, 25);
            this.label26.TabIndex = 0;
            this.label26.Text = "MDE壓應變：";
            // 
            // ODEVCStrainText
            // 
            this.ODEVCStrainText.Location = new System.Drawing.Point(161, 94);
            this.ODEVCStrainText.Name = "ODEVCStrainText";
            this.ODEVCStrainText.Size = new System.Drawing.Size(63, 25);
            this.ODEVCStrainText.TabIndex = 10;
            // 
            // MDEVCStrainText
            // 
            this.MDEVCStrainText.Location = new System.Drawing.Point(161, 159);
            this.MDEVCStrainText.Name = "MDEVCStrainText";
            this.MDEVCStrainText.Size = new System.Drawing.Size(63, 25);
            this.MDEVCStrainText.TabIndex = 10;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label27.Location = new System.Drawing.Point(377, 12);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(112, 25);
            this.label27.TabIndex = 0;
            this.label27.Text = "灌漿計算：";
            // 
            // GroutingInternalCal
            // 
            this.GroutingInternalCal.Location = new System.Drawing.Point(504, 8);
            this.GroutingInternalCal.Name = "GroutingInternalCal";
            this.GroutingInternalCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.GroutingInternalCal.Size = new System.Drawing.Size(109, 29);
            this.GroutingInternalCal.TabIndex = 15;
            this.GroutingInternalCal.Text = "SAP2000分析";
            this.GroutingInternalCal.UseVisualStyleBackColor = true;
            this.GroutingInternalCal.Click += new System.EventHandler(this.GroutingInternalCal_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label28.Location = new System.Drawing.Point(15, 42);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(125, 25);
            this.label28.TabIndex = 0;
            this.label28.Text = "LontermE：";
            // 
            // LongtermE1_In
            // 
            this.LongtermE1_In.Location = new System.Drawing.Point(169, 45);
            this.LongtermE1_In.Name = "LongtermE1_In";
            this.LongtermE1_In.Size = new System.Drawing.Size(63, 25);
            this.LongtermE1_In.TabIndex = 10;
            // 
            // btn_VerticalStressInternal
            // 
            this.btn_VerticalStressInternal.Location = new System.Drawing.Point(132, 14);
            this.btn_VerticalStressInternal.Name = "btn_VerticalStressInternal";
            this.btn_VerticalStressInternal.Size = new System.Drawing.Size(100, 25);
            this.btn_VerticalStressInternal.TabIndex = 16;
            this.btn_VerticalStressInternal.Text = "荷重計算(內部)";
            this.btn_VerticalStressInternal.UseVisualStyleBackColor = true;
            this.btn_VerticalStressInternal.Click += new System.EventHandler(this.btn_VerticalStressInternal_Click);
            // 
            // GroutingM
            // 
            this.GroutingM.Location = new System.Drawing.Point(504, 41);
            this.GroutingM.Name = "GroutingM";
            this.GroutingM.Size = new System.Drawing.Size(63, 25);
            this.GroutingM.TabIndex = 10;
            // 
            // GroutingP
            // 
            this.GroutingP.Location = new System.Drawing.Point(504, 69);
            this.GroutingP.Name = "GroutingP";
            this.GroutingP.Size = new System.Drawing.Size(63, 25);
            this.GroutingP.TabIndex = 10;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label29.Location = new System.Drawing.Point(405, 41);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(93, 25);
            this.label29.TabIndex = 0;
            this.label29.Text = "Mmax：";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label30.Location = new System.Drawing.Point(412, 66);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(86, 25);
            this.label30.TabIndex = 0;
            this.label30.Text = "Pmax：";
            // 
            // VSExternal
            // 
            this.VSExternal.Location = new System.Drawing.Point(133, 121);
            this.VSExternal.Name = "VSExternal";
            this.VSExternal.Size = new System.Drawing.Size(100, 25);
            this.VSExternal.TabIndex = 17;
            this.VSExternal.Text = "VSExternal";
            this.VSExternal.UseVisualStyleBackColor = true;
            this.VSExternal.Click += new System.EventHandler(this.VSExternal_Click);
            // 
            // LongTermE1_Ex
            // 
            this.LongTermE1_Ex.Location = new System.Drawing.Point(169, 151);
            this.LongTermE1_Ex.Name = "LongTermE1_Ex";
            this.LongTermE1_Ex.Size = new System.Drawing.Size(63, 25);
            this.LongTermE1_Ex.TabIndex = 10;
            // 
            // ShortermE1_In
            // 
            this.ShortermE1_In.Location = new System.Drawing.Point(169, 77);
            this.ShortermE1_In.Name = "ShortermE1_In";
            this.ShortermE1_In.Size = new System.Drawing.Size(63, 25);
            this.ShortermE1_In.TabIndex = 10;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label31.Location = new System.Drawing.Point(15, 77);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(127, 25);
            this.label31.TabIndex = 0;
            this.label31.Text = "ShotermE：";
            // 
            // ShorTermE1
            // 
            this.ShorTermE1.Location = new System.Drawing.Point(169, 179);
            this.ShorTermE1.Name = "ShorTermE1";
            this.ShorTermE1.Size = new System.Drawing.Size(63, 25);
            this.ShorTermE1.TabIndex = 10;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label32.Location = new System.Drawing.Point(12, 149);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(125, 25);
            this.label32.TabIndex = 0;
            this.label32.Text = "LontermE：";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label33.Location = new System.Drawing.Point(12, 184);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(127, 25);
            this.label33.TabIndex = 0;
            this.label33.Text = "ShotermE：";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label34.Location = new System.Drawing.Point(377, 113);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(112, 25);
            this.label34.TabIndex = 0;
            this.label34.Text = "載重狀態：";
            // 
            // LoadingtermInternalCal
            // 
            this.LoadingtermInternalCal.Location = new System.Drawing.Point(504, 113);
            this.LoadingtermInternalCal.Name = "LoadingtermInternalCal";
            this.LoadingtermInternalCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LoadingtermInternalCal.Size = new System.Drawing.Size(109, 29);
            this.LoadingtermInternalCal.TabIndex = 18;
            this.LoadingtermInternalCal.Text = "SAP2000分析";
            this.LoadingtermInternalCal.UseVisualStyleBackColor = true;
            this.LoadingtermInternalCal.Click += new System.EventHandler(this.LoadingtermInternalCal_Click);
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(8, 12);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(326, 219);
            this.cartesianChart1.TabIndex = 19;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(9, 231);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(325, 200);
            this.chart1.TabIndex = 20;
            this.chart1.Text = "chart1";
            // 
            // LongTermInitialCheckBox
            // 
            this.LongTermInitialCheckBox.AutoSize = true;
            this.LongTermInitialCheckBox.Location = new System.Drawing.Point(400, 150);
            this.LongTermInitialCheckBox.Name = "LongTermInitialCheckBox";
            this.LongTermInitialCheckBox.Size = new System.Drawing.Size(83, 22);
            this.LongTermInitialCheckBox.TabIndex = 25;
            this.LongTermInitialCheckBox.Text = "長期載重";
            this.LongTermInitialCheckBox.UseVisualStyleBackColor = true;
            // 
            // ShortTermInitialCheckBox
            // 
            this.ShortTermInitialCheckBox.AutoSize = true;
            this.ShortTermInitialCheckBox.Location = new System.Drawing.Point(400, 174);
            this.ShortTermInitialCheckBox.Name = "ShortTermInitialCheckBox";
            this.ShortTermInitialCheckBox.Size = new System.Drawing.Size(83, 22);
            this.ShortTermInitialCheckBox.TabIndex = 25;
            this.ShortTermInitialCheckBox.Text = "短期載重";
            this.ShortTermInitialCheckBox.UseVisualStyleBackColor = true;
            // 
            // VarDiaInitialCheckBox
            // 
            this.VarDiaInitialCheckBox.AutoSize = true;
            this.VarDiaInitialCheckBox.Location = new System.Drawing.Point(400, 217);
            this.VarDiaInitialCheckBox.Name = "VarDiaInitialCheckBox";
            this.VarDiaInitialCheckBox.Size = new System.Drawing.Size(124, 22);
            this.VarDiaInitialCheckBox.TabIndex = 25;
            this.VarDiaInitialCheckBox.Text = "0.33%直徑變化";
            this.VarDiaInitialCheckBox.UseVisualStyleBackColor = true;
            // 
            // SeismicCheckBox
            // 
            this.SeismicCheckBox.AutoSize = true;
            this.SeismicCheckBox.Location = new System.Drawing.Point(400, 267);
            this.SeismicCheckBox.Name = "SeismicCheckBox";
            this.SeismicCheckBox.Size = new System.Drawing.Size(83, 22);
            this.SeismicCheckBox.TabIndex = 25;
            this.SeismicCheckBox.Text = "地震載重";
            this.SeismicCheckBox.UseVisualStyleBackColor = true;
            // 
            // EQDiaIncreasedZP
            // 
            this.EQDiaIncreasedZP.AutoSize = true;
            this.EQDiaIncreasedZP.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.EQDiaIncreasedZP.Location = new System.Drawing.Point(553, 270);
            this.EQDiaIncreasedZP.Name = "EQDiaIncreasedZP";
            this.EQDiaIncreasedZP.Size = new System.Drawing.Size(47, 18);
            this.EQDiaIncreasedZP.TabIndex = 0;
            this.EQDiaIncreasedZP.Text = "EQInc";
            // 
            // VarDiaXPInput
            // 
            this.VarDiaXPInput.Location = new System.Drawing.Point(732, 214);
            this.VarDiaXPInput.Name = "VarDiaXPInput";
            this.VarDiaXPInput.Size = new System.Drawing.Size(63, 25);
            this.VarDiaXPInput.TabIndex = 27;
            // 
            // EQDiaInput
            // 
            this.EQDiaInput.Location = new System.Drawing.Point(732, 267);
            this.EQDiaInput.Name = "EQDiaInput";
            this.EQDiaInput.Size = new System.Drawing.Size(63, 25);
            this.EQDiaInput.TabIndex = 27;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.輸入參數);
            this.tabControl1.Controls.Add(this.吊放等計算);
            this.tabControl1.Controls.Add(this.土壤E值);
            this.tabControl1.Controls.Add(this.環片應變);
            this.tabControl1.Controls.Add(this.環片分析);
            this.tabControl1.Controls.Add(this.穩定性檢核);
            this.tabControl1.Controls.Add(this.參考用);
            this.tabControl1.Controls.Add(this.網頁);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1056, 540);
            this.tabControl1.TabIndex = 28;
            // 
            // 輸入參數
            // 
            this.輸入參數.Controls.Add(this.checkB_Site);
            this.輸入參數.Controls.Add(this.checkB_Connector_Steel);
            this.輸入參數.Controls.Add(this.checkB_Precast);
            this.輸入參數.Controls.Add(this.btn_InputFrameMaterial);
            this.輸入參數.Controls.Add(this.label_SectionFunction);
            this.輸入參數.Controls.Add(this.ChosenProject);
            this.輸入參數.Controls.Add(this.label69);
            this.輸入參數.Controls.Add(this.lbl_User1);
            this.輸入參數.Controls.Add(this.label73);
            this.輸入參數.Controls.Add(this.label60);
            this.輸入參數.Controls.Add(this.label59);
            this.輸入參數.Location = new System.Drawing.Point(4, 26);
            this.輸入參數.Name = "輸入參數";
            this.輸入參數.Padding = new System.Windows.Forms.Padding(3);
            this.輸入參數.Size = new System.Drawing.Size(1048, 510);
            this.輸入參數.TabIndex = 0;
            this.輸入參數.Text = "斷面選單";
            this.輸入參數.UseVisualStyleBackColor = true;
            // 
            // checkB_Site
            // 
            this.checkB_Site.AutoSize = true;
            this.checkB_Site.Location = new System.Drawing.Point(850, 108);
            this.checkB_Site.Name = "checkB_Site";
            this.checkB_Site.Size = new System.Drawing.Size(83, 22);
            this.checkB_Site.TabIndex = 13;
            this.checkB_Site.Text = "場鑄環片";
            this.checkB_Site.UseVisualStyleBackColor = true;
            // 
            // checkB_Connector_Steel
            // 
            this.checkB_Connector_Steel.AutoSize = true;
            this.checkB_Connector_Steel.Location = new System.Drawing.Point(850, 80);
            this.checkB_Connector_Steel.Name = "checkB_Connector_Steel";
            this.checkB_Connector_Steel.Size = new System.Drawing.Size(139, 22);
            this.checkB_Connector_Steel.TabIndex = 13;
            this.checkB_Connector_Steel.Text = "聯絡通道與鋼環片";
            this.checkB_Connector_Steel.UseVisualStyleBackColor = true;
            // 
            // checkB_Precast
            // 
            this.checkB_Precast.AutoSize = true;
            this.checkB_Precast.Location = new System.Drawing.Point(850, 52);
            this.checkB_Precast.Name = "checkB_Precast";
            this.checkB_Precast.Size = new System.Drawing.Size(163, 22);
            this.checkB_Precast.TabIndex = 13;
            this.checkB_Precast.Text = "預鑄環片(含背填灌漿)";
            this.checkB_Precast.UseVisualStyleBackColor = true;
            // 
            // btn_InputFrameMaterial
            // 
            this.btn_InputFrameMaterial.Location = new System.Drawing.Point(850, 12);
            this.btn_InputFrameMaterial.Name = "btn_InputFrameMaterial";
            this.btn_InputFrameMaterial.Size = new System.Drawing.Size(112, 34);
            this.btn_InputFrameMaterial.TabIndex = 12;
            this.btn_InputFrameMaterial.Text = "輸入材料桿件";
            this.btn_InputFrameMaterial.UseVisualStyleBackColor = true;
            this.btn_InputFrameMaterial.Click += new System.EventHandler(this.btn_InputFrameMaterial_Click);
            // 
            // label_SectionFunction
            // 
            this.label_SectionFunction.AutoSize = true;
            this.label_SectionFunction.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label_SectionFunction.Location = new System.Drawing.Point(118, 122);
            this.label_SectionFunction.Name = "label_SectionFunction";
            this.label_SectionFunction.Size = new System.Drawing.Size(228, 25);
            this.label_SectionFunction.TabIndex = 11;
            this.label_SectionFunction.Text = "label_SectionFunction";
            // 
            // ChosenProject
            // 
            this.ChosenProject.AutoSize = true;
            this.ChosenProject.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.ChosenProject.Location = new System.Drawing.Point(118, 47);
            this.ChosenProject.Name = "ChosenProject";
            this.ChosenProject.Size = new System.Drawing.Size(153, 25);
            this.ChosenProject.TabIndex = 11;
            this.ChosenProject.Text = "ChosenProject";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label69.Location = new System.Drawing.Point(6, 122);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(112, 25);
            this.label69.TabIndex = 0;
            this.label69.Text = "斷面用途：";
            // 
            // lbl_User1
            // 
            this.lbl_User1.AutoSize = true;
            this.lbl_User1.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.lbl_User1.Location = new System.Drawing.Point(104, 12);
            this.lbl_User1.Name = "lbl_User1";
            this.lbl_User1.Size = new System.Drawing.Size(55, 25);
            this.lbl_User1.TabIndex = 0;
            this.lbl_User1.Text = "User";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label73.Location = new System.Drawing.Point(6, 12);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(92, 25);
            this.label73.TabIndex = 0;
            this.label73.Text = "使用者：";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label60.Location = new System.Drawing.Point(6, 47);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(112, 25);
            this.label60.TabIndex = 0;
            this.label60.Text = "選定標案：";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label59.Location = new System.Drawing.Point(4, 81);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(112, 25);
            this.label59.TabIndex = 0;
            this.label59.Text = "選擇斷面：";
            // 
            // 吊放等計算
            // 
            this.吊放等計算.Controls.Add(this.STN_SegmentSelfInternal);
            this.吊放等計算.Controls.Add(this.btn_SGSelf);
            this.吊放等計算.Controls.Add(this.label19);
            this.吊放等計算.Controls.Add(this.label22);
            this.吊放等計算.Controls.Add(this.label23);
            this.吊放等計算.Controls.Add(this.label24);
            this.吊放等計算.Controls.Add(this.label20);
            this.吊放等計算.Controls.Add(this.label21);
            this.吊放等計算.Controls.Add(this.TransportationMText);
            this.吊放等計算.Controls.Add(this.StackingMText);
            this.吊放等計算.Controls.Add(this.HangingMText);
            this.吊放等計算.Controls.Add(this.TransportationVText);
            this.吊放等計算.Controls.Add(this.StackingVText);
            this.吊放等計算.Controls.Add(this.HangingVText);
            this.吊放等計算.Location = new System.Drawing.Point(4, 26);
            this.吊放等計算.Name = "吊放等計算";
            this.吊放等計算.Padding = new System.Windows.Forms.Padding(3);
            this.吊放等計算.Size = new System.Drawing.Size(1048, 510);
            this.吊放等計算.TabIndex = 1;
            this.吊放等計算.Text = "吊放等計算";
            this.吊放等計算.UseVisualStyleBackColor = true;
            // 
            // STN_SegmentSelfInternal
            // 
            this.STN_SegmentSelfInternal.Location = new System.Drawing.Point(165, 214);
            this.STN_SegmentSelfInternal.Name = "STN_SegmentSelfInternal";
            this.STN_SegmentSelfInternal.Size = new System.Drawing.Size(102, 29);
            this.STN_SegmentSelfInternal.TabIndex = 13;
            this.STN_SegmentSelfInternal.Text = "winform內部";
            this.STN_SegmentSelfInternal.UseVisualStyleBackColor = true;
            this.STN_SegmentSelfInternal.Click += new System.EventHandler(this.STN_SegmentSelfInternal_Click);
            // 
            // 土壤E值
            // 
            this.土壤E值.Controls.Add(this.cbo_LoadingCal);
            this.土壤E值.Controls.Add(this.btn_VerticalStressInternal);
            this.土壤E值.Controls.Add(this.label28);
            this.土壤E值.Controls.Add(this.label31);
            this.土壤E值.Controls.Add(this.label32);
            this.土壤E值.Controls.Add(this.label33);
            this.土壤E值.Controls.Add(this.LongtermE1_In);
            this.土壤E值.Controls.Add(this.ShortermE1_In);
            this.土壤E值.Controls.Add(this.LongTermE1_Ex);
            this.土壤E值.Controls.Add(this.VSExternal);
            this.土壤E值.Controls.Add(this.ShorTermE1);
            this.土壤E值.Location = new System.Drawing.Point(4, 26);
            this.土壤E值.Name = "土壤E值";
            this.土壤E值.Size = new System.Drawing.Size(1048, 515);
            this.土壤E值.TabIndex = 2;
            this.土壤E值.Text = "土壤E值";
            this.土壤E值.UseVisualStyleBackColor = true;
            // 
            // cbo_LoadingCal
            // 
            this.cbo_LoadingCal.FormattingEnabled = true;
            this.cbo_LoadingCal.Items.AddRange(new object[] {
            "TUNNEL",
            "CONNECTOR",
            "SHIELDMACHINE"});
            this.cbo_LoadingCal.Location = new System.Drawing.Point(250, 14);
            this.cbo_LoadingCal.Name = "cbo_LoadingCal";
            this.cbo_LoadingCal.Size = new System.Drawing.Size(121, 25);
            this.cbo_LoadingCal.TabIndex = 19;
            // 
            // 環片應變
            // 
            this.環片應變.Controls.Add(this.button1);
            this.環片應變.Controls.Add(this.STN_StrainCheck_Internal);
            this.環片應變.Controls.Add(this.label54);
            this.環片應變.Controls.Add(this.label47);
            this.環片應變.Controls.Add(this.label53);
            this.環片應變.Controls.Add(this.label52);
            this.環片應變.Controls.Add(this.label49);
            this.環片應變.Controls.Add(this.ODETTStrainText);
            this.環片應變.Controls.Add(this.label25);
            this.環片應變.Controls.Add(this.label51);
            this.環片應變.Controls.Add(this.ODEVTStrainText);
            this.環片應變.Controls.Add(this.label50);
            this.環片應變.Controls.Add(this.label48);
            this.環片應變.Controls.Add(this.ODETCStrainText);
            this.環片應變.Controls.Add(this.label26);
            this.環片應變.Controls.Add(this.MDETTStrainText);
            this.環片應變.Controls.Add(this.ODEVCStrainText);
            this.環片應變.Controls.Add(this.MDETCStrainText);
            this.環片應變.Controls.Add(this.MDEVTStrainText);
            this.環片應變.Controls.Add(this.MDEVCStrainText);
            this.環片應變.Controls.Add(this.Cal03_output);
            this.環片應變.Location = new System.Drawing.Point(4, 26);
            this.環片應變.Name = "環片應變";
            this.環片應變.Size = new System.Drawing.Size(1048, 515);
            this.環片應變.TabIndex = 3;
            this.環片應變.Text = "環片應變";
            this.環片應變.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(282, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label54.Location = new System.Drawing.Point(14, 304);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(134, 25);
            this.label54.TabIndex = 0;
            this.label54.Text = "ODE張應變：";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label47.Location = new System.Drawing.Point(19, 122);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(134, 25);
            this.label47.TabIndex = 0;
            this.label47.Text = "ODE張應變：";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label53.Location = new System.Drawing.Point(3, 237);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(232, 25);
            this.label53.TabIndex = 0;
            this.label53.Text = "地震引致之隧道扭曲應變";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label52.Location = new System.Drawing.Point(14, 273);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(134, 25);
            this.label52.TabIndex = 0;
            this.label52.Text = "ODE壓應變：";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label49.Location = new System.Drawing.Point(8, 55);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(232, 25);
            this.label49.TabIndex = 0;
            this.label49.Text = "地震引致之隧道縱向應變";
            // 
            // ODETTStrainText
            // 
            this.ODETTStrainText.Location = new System.Drawing.Point(156, 307);
            this.ODETTStrainText.Name = "ODETTStrainText";
            this.ODETTStrainText.Size = new System.Drawing.Size(63, 25);
            this.ODETTStrainText.TabIndex = 10;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label51.Location = new System.Drawing.Point(13, 369);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(138, 25);
            this.label51.TabIndex = 0;
            this.label51.Text = "MDE張應變：";
            // 
            // ODEVTStrainText
            // 
            this.ODEVTStrainText.Location = new System.Drawing.Point(161, 125);
            this.ODEVTStrainText.Name = "ODEVTStrainText";
            this.ODEVTStrainText.Size = new System.Drawing.Size(63, 25);
            this.ODEVTStrainText.TabIndex = 10;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label50.Location = new System.Drawing.Point(14, 338);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(138, 25);
            this.label50.TabIndex = 0;
            this.label50.Text = "MDE壓應變：";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label48.Location = new System.Drawing.Point(18, 187);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(138, 25);
            this.label48.TabIndex = 0;
            this.label48.Text = "MDE張應變：";
            // 
            // ODETCStrainText
            // 
            this.ODETCStrainText.Location = new System.Drawing.Point(156, 276);
            this.ODETCStrainText.Name = "ODETCStrainText";
            this.ODETCStrainText.Size = new System.Drawing.Size(63, 25);
            this.ODETCStrainText.TabIndex = 10;
            // 
            // MDETTStrainText
            // 
            this.MDETTStrainText.Location = new System.Drawing.Point(155, 372);
            this.MDETTStrainText.Name = "MDETTStrainText";
            this.MDETTStrainText.Size = new System.Drawing.Size(63, 25);
            this.MDETTStrainText.TabIndex = 10;
            // 
            // MDETCStrainText
            // 
            this.MDETCStrainText.Location = new System.Drawing.Point(156, 341);
            this.MDETCStrainText.Name = "MDETCStrainText";
            this.MDETCStrainText.Size = new System.Drawing.Size(63, 25);
            this.MDETCStrainText.TabIndex = 10;
            // 
            // MDEVTStrainText
            // 
            this.MDEVTStrainText.Location = new System.Drawing.Point(160, 190);
            this.MDEVTStrainText.Name = "MDEVTStrainText";
            this.MDEVTStrainText.Size = new System.Drawing.Size(63, 25);
            this.MDEVTStrainText.TabIndex = 10;
            // 
            // 環片分析
            // 
            this.環片分析.Controls.Add(this.tabControl2);
            this.環片分析.Location = new System.Drawing.Point(4, 26);
            this.環片分析.Name = "環片分析";
            this.環片分析.Size = new System.Drawing.Size(1048, 515);
            this.環片分析.TabIndex = 4;
            this.環片分析.Text = "環片分析";
            this.環片分析.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.btn_ExcelOutput);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1048, 515);
            this.tabControl2.TabIndex = 28;
            // 
            // btn_ExcelOutput
            // 
            this.btn_ExcelOutput.Controls.Add(this.btn_GroutExcel1);
            this.btn_ExcelOutput.Controls.Add(this.btn_LoadingOutExcel);
            this.btn_ExcelOutput.Controls.Add(this.label1);
            this.btn_ExcelOutput.Controls.Add(this.label2);
            this.btn_ExcelOutput.Controls.Add(this.label3);
            this.btn_ExcelOutput.Controls.Add(this.label4);
            this.btn_ExcelOutput.Controls.Add(this.label5);
            this.btn_ExcelOutput.Controls.Add(this.label6);
            this.btn_ExcelOutput.Controls.Add(this.label7);
            this.btn_ExcelOutput.Controls.Add(this.label8);
            this.btn_ExcelOutput.Controls.Add(this.label9);
            this.btn_ExcelOutput.Controls.Add(this.label10);
            this.btn_ExcelOutput.Controls.Add(this.label11);
            this.btn_ExcelOutput.Controls.Add(this.label12);
            this.btn_ExcelOutput.Controls.Add(this.label13);
            this.btn_ExcelOutput.Controls.Add(this.label14);
            this.btn_ExcelOutput.Controls.Add(this.label15);
            this.btn_ExcelOutput.Controls.Add(this.label16);
            this.btn_ExcelOutput.Controls.Add(this.label17);
            this.btn_ExcelOutput.Controls.Add(this.label18);
            this.btn_ExcelOutput.Controls.Add(this.RadiusIn);
            this.btn_ExcelOutput.Controls.Add(this.Thickness);
            this.btn_ExcelOutput.Controls.Add(this.Angle);
            this.btn_ExcelOutput.Controls.Add(this.AAngle);
            this.btn_ExcelOutput.Controls.Add(this.BAngle);
            this.btn_ExcelOutput.Controls.Add(this.KAngle);
            this.btn_ExcelOutput.Controls.Add(this.AdjacentPoreAngle);
            this.btn_ExcelOutput.Controls.Add(this.AdjacentGroutAngle);
            this.btn_ExcelOutput.Controls.Add(this.UnitWeight);
            this.btn_ExcelOutput.Controls.Add(this.label27);
            this.btn_ExcelOutput.Controls.Add(this.GroutingM);
            this.btn_ExcelOutput.Controls.Add(this.EQDiaInitial);
            this.btn_ExcelOutput.Controls.Add(this.EQDiaInput);
            this.btn_ExcelOutput.Controls.Add(this.EQDiaIncreasedZP);
            this.btn_ExcelOutput.Controls.Add(this.SeismicCheckBox);
            this.btn_ExcelOutput.Controls.Add(this.GroutingP);
            this.btn_ExcelOutput.Controls.Add(this.VarDiaXPInitial);
            this.btn_ExcelOutput.Controls.Add(this.VarDiaXPInput);
            this.btn_ExcelOutput.Controls.Add(this.label58);
            this.btn_ExcelOutput.Controls.Add(this.label57);
            this.btn_ExcelOutput.Controls.Add(this.label56);
            this.btn_ExcelOutput.Controls.Add(this.label55);
            this.btn_ExcelOutput.Controls.Add(this.VarDiaDecreasedXP);
            this.btn_ExcelOutput.Controls.Add(this.VarDiaInitialCheckBox);
            this.btn_ExcelOutput.Controls.Add(this.GroutingInternalCal);
            this.btn_ExcelOutput.Controls.Add(this.ShortTermInitialCheckBox);
            this.btn_ExcelOutput.Controls.Add(this.label30);
            this.btn_ExcelOutput.Controls.Add(this.LongTermInitialCheckBox);
            this.btn_ExcelOutput.Controls.Add(this.LoadingtermInternalCal);
            this.btn_ExcelOutput.Controls.Add(this.label34);
            this.btn_ExcelOutput.Controls.Add(this.label29);
            this.btn_ExcelOutput.Location = new System.Drawing.Point(4, 26);
            this.btn_ExcelOutput.Name = "btn_ExcelOutput";
            this.btn_ExcelOutput.Padding = new System.Windows.Forms.Padding(3);
            this.btn_ExcelOutput.Size = new System.Drawing.Size(1040, 485);
            this.btn_ExcelOutput.TabIndex = 0;
            this.btn_ExcelOutput.Text = "SAP分析";
            this.btn_ExcelOutput.UseVisualStyleBackColor = true;
            // 
            // btn_GroutExcel1
            // 
            this.btn_GroutExcel1.Location = new System.Drawing.Point(633, 8);
            this.btn_GroutExcel1.Name = "btn_GroutExcel1";
            this.btn_GroutExcel1.Size = new System.Drawing.Size(108, 29);
            this.btn_GroutExcel1.TabIndex = 56;
            this.btn_GroutExcel1.Text = "輸出Excel表";
            this.btn_GroutExcel1.UseVisualStyleBackColor = true;
            this.btn_GroutExcel1.Click += new System.EventHandler(this.btn_GroutExcel1_Click);
            // 
            // btn_LoadingOutExcel
            // 
            this.btn_LoadingOutExcel.Location = new System.Drawing.Point(633, 113);
            this.btn_LoadingOutExcel.Name = "btn_LoadingOutExcel";
            this.btn_LoadingOutExcel.Size = new System.Drawing.Size(99, 29);
            this.btn_LoadingOutExcel.TabIndex = 55;
            this.btn_LoadingOutExcel.Text = "輸出Excel表";
            this.btn_LoadingOutExcel.UseVisualStyleBackColor = true;
            this.btn_LoadingOutExcel.Click += new System.EventHandler(this.btn_LoadingOutExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 25);
            this.label1.TabIndex = 28;
            this.label1.Text = "環片內半徑：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(16, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 25);
            this.label2.TabIndex = 45;
            this.label2.Text = "環片厚度：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(16, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 25);
            this.label3.TabIndex = 44;
            this.label3.Text = "環片內角度：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(16, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 25);
            this.label4.TabIndex = 43;
            this.label4.Text = "A環片角度：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(16, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 25);
            this.label5.TabIndex = 42;
            this.label5.Text = "B環片角度：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(16, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 25);
            this.label6.TabIndex = 40;
            this.label6.Text = "K環片角度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(16, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 25);
            this.label7.TabIndex = 39;
            this.label7.Text = "相鄰螺栓孔角度：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(16, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(172, 25);
            this.label8.TabIndex = 38;
            this.label8.Text = "相鄰灌漿孔角度：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(16, 328);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 25);
            this.label9.TabIndex = 41;
            this.label9.Text = "環片單位重：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(275, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 21);
            this.label10.TabIndex = 36;
            this.label10.Text = "m";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(275, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 21);
            this.label11.TabIndex = 35;
            this.label11.Text = "m";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(275, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(25, 21);
            this.label12.TabIndex = 34;
            this.label12.Text = "m";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(275, 132);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 21);
            this.label13.TabIndex = 33;
            this.label13.Text = "m";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(275, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 21);
            this.label14.TabIndex = 37;
            this.label14.Text = "m";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(275, 211);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 21);
            this.label15.TabIndex = 32;
            this.label15.Text = "m";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(275, 248);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(25, 21);
            this.label16.TabIndex = 31;
            this.label16.Text = "m";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(275, 289);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(25, 21);
            this.label17.TabIndex = 30;
            this.label17.Text = "m";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(275, 332);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(76, 21);
            this.label18.TabIndex = 29;
            this.label18.Text = "kN/m^3";
            // 
            // RadiusIn
            // 
            this.RadiusIn.Location = new System.Drawing.Point(196, 15);
            this.RadiusIn.Name = "RadiusIn";
            this.RadiusIn.Size = new System.Drawing.Size(73, 25);
            this.RadiusIn.TabIndex = 46;
            // 
            // Thickness
            // 
            this.Thickness.Location = new System.Drawing.Point(196, 51);
            this.Thickness.Name = "Thickness";
            this.Thickness.Size = new System.Drawing.Size(73, 25);
            this.Thickness.TabIndex = 47;
            // 
            // Angle
            // 
            this.Angle.Location = new System.Drawing.Point(196, 88);
            this.Angle.Name = "Angle";
            this.Angle.Size = new System.Drawing.Size(73, 25);
            this.Angle.TabIndex = 48;
            // 
            // AAngle
            // 
            this.AAngle.Location = new System.Drawing.Point(196, 128);
            this.AAngle.Name = "AAngle";
            this.AAngle.Size = new System.Drawing.Size(73, 25);
            this.AAngle.TabIndex = 49;
            // 
            // BAngle
            // 
            this.BAngle.Location = new System.Drawing.Point(196, 167);
            this.BAngle.Name = "BAngle";
            this.BAngle.Size = new System.Drawing.Size(73, 25);
            this.BAngle.TabIndex = 50;
            // 
            // KAngle
            // 
            this.KAngle.Location = new System.Drawing.Point(196, 207);
            this.KAngle.Name = "KAngle";
            this.KAngle.Size = new System.Drawing.Size(73, 25);
            this.KAngle.TabIndex = 51;
            // 
            // AdjacentPoreAngle
            // 
            this.AdjacentPoreAngle.Location = new System.Drawing.Point(196, 247);
            this.AdjacentPoreAngle.Name = "AdjacentPoreAngle";
            this.AdjacentPoreAngle.Size = new System.Drawing.Size(73, 25);
            this.AdjacentPoreAngle.TabIndex = 52;
            // 
            // AdjacentGroutAngle
            // 
            this.AdjacentGroutAngle.Location = new System.Drawing.Point(195, 288);
            this.AdjacentGroutAngle.Name = "AdjacentGroutAngle";
            this.AdjacentGroutAngle.Size = new System.Drawing.Size(74, 25);
            this.AdjacentGroutAngle.TabIndex = 53;
            // 
            // UnitWeight
            // 
            this.UnitWeight.Location = new System.Drawing.Point(195, 328);
            this.UnitWeight.Name = "UnitWeight";
            this.UnitWeight.Size = new System.Drawing.Size(74, 25);
            this.UnitWeight.TabIndex = 54;
            // 
            // EQDiaInitial
            // 
            this.EQDiaInitial.Location = new System.Drawing.Point(659, 267);
            this.EQDiaInitial.Name = "EQDiaInitial";
            this.EQDiaInitial.Size = new System.Drawing.Size(63, 25);
            this.EQDiaInitial.TabIndex = 27;
            // 
            // VarDiaXPInitial
            // 
            this.VarDiaXPInitial.Location = new System.Drawing.Point(659, 214);
            this.VarDiaXPInitial.Name = "VarDiaXPInitial";
            this.VarDiaXPInitial.Size = new System.Drawing.Size(63, 25);
            this.VarDiaXPInitial.TabIndex = 27;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label58.Location = new System.Drawing.Point(668, 193);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(50, 18);
            this.label58.TabIndex = 0;
            this.label58.Text = "初始值";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label57.Location = new System.Drawing.Point(741, 193);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(50, 18);
            this.label57.TabIndex = 0;
            this.label57.Text = "迭代值";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label56.Location = new System.Drawing.Point(550, 246);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(122, 18);
            this.label56.TabIndex = 0;
            this.label56.Text = "垂直向加壓值(kN)";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label55.Location = new System.Drawing.Point(550, 194);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(108, 18);
            this.label55.TabIndex = 0;
            this.label55.Text = "側向解壓值(kN)";
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.splitContainer1);
            this.tabPage9.Location = new System.Drawing.Point(4, 26);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1040, 485);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "SAP接觸深度";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1034, 479);
            this.splitContainer1.SplitterDistance = 642;
            this.splitContainer1.TabIndex = 0;
            // 
            // 穩定性檢核
            // 
            this.穩定性檢核.Location = new System.Drawing.Point(4, 26);
            this.穩定性檢核.Name = "穩定性檢核";
            this.穩定性檢核.Size = new System.Drawing.Size(1048, 515);
            this.穩定性檢核.TabIndex = 8;
            this.穩定性檢核.Text = "穩定性檢核";
            this.穩定性檢核.UseVisualStyleBackColor = true;
            // 
            // 參考用
            // 
            this.參考用.Controls.Add(this.cartesianChart1);
            this.參考用.Controls.Add(this.chart1);
            this.參考用.Location = new System.Drawing.Point(4, 26);
            this.參考用.Name = "參考用";
            this.參考用.Size = new System.Drawing.Size(1048, 515);
            this.參考用.TabIndex = 6;
            this.參考用.Text = "參考用";
            this.參考用.UseVisualStyleBackColor = true;
            // 
            // 網頁
            // 
            this.網頁.Controls.Add(this.web);
            this.網頁.Location = new System.Drawing.Point(4, 26);
            this.網頁.Name = "網頁";
            this.網頁.Size = new System.Drawing.Size(1048, 515);
            this.網頁.TabIndex = 7;
            this.網頁.Text = "網頁";
            this.網頁.UseVisualStyleBackColor = true;
            // 
            // web
            // 
            this.web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.web.Location = new System.Drawing.Point(0, 0);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(1048, 515);
            this.web.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 24);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Panel1Collapsed = true;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer2.Size = new System.Drawing.Size(1070, 575);
            this.splitContainer2.SplitterDistance = 150;
            this.splitContainer2.TabIndex = 29;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabSection);
            this.tabControl3.Controls.Add(this.tabTunnel);
            this.tabControl3.Controls.Add(this.tabConnect);
            this.tabControl3.Controls.Add(this.tabSteelTunnel);
            this.tabControl3.Controls.Add(this.tabSiteTunnel);
            this.tabControl3.Controls.Add(this.tabContDepth);
            this.tabControl3.Controls.Add(this.tabOthers);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1070, 575);
            this.tabControl3.TabIndex = 29;
            this.tabControl3.SelectedIndexChanged += new System.EventHandler(this.tabControl3_SelectedIndexChanged);
            // 
            // tabSection
            // 
            this.tabSection.BackColor = System.Drawing.Color.Transparent;
            this.tabSection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabSection.BackgroundImage")));
            this.tabSection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabSection.Controls.Add(this.textBox1);
            this.tabSection.Controls.Add(this.button2);
            this.tabSection.Controls.Add(this.cbo_Section);
            this.tabSection.Controls.Add(this.label74);
            this.tabSection.Controls.Add(this.lbl_SectionFunction);
            this.tabSection.Controls.Add(this.lbl_ChosenProject);
            this.tabSection.Controls.Add(this.lbl_User);
            this.tabSection.Controls.Add(this.label61);
            this.tabSection.Controls.Add(this.label75);
            this.tabSection.Controls.Add(this.label76);
            this.tabSection.Controls.Add(this.label77);
            this.tabSection.Location = new System.Drawing.Point(4, 25);
            this.tabSection.Name = "tabSection";
            this.tabSection.Size = new System.Drawing.Size(1062, 546);
            this.tabSection.TabIndex = 4;
            this.tabSection.Text = "斷面選單";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(725, 329);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(233, 23);
            this.textBox1.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(810, 300);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbo_Section
            // 
            this.cbo_Section.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbo_Section.FormattingEnabled = true;
            this.cbo_Section.ItemHeight = 17;
            this.cbo_Section.Location = new System.Drawing.Point(148, 175);
            this.cbo_Section.Name = "cbo_Section";
            this.cbo_Section.Size = new System.Drawing.Size(348, 25);
            this.cbo_Section.TabIndex = 10;
            this.cbo_Section.SelectionChangeCommitted += new System.EventHandler(this.cbo_Section_SelectionChangeCommitted);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label74.ForeColor = System.Drawing.Color.White;
            this.label74.Location = new System.Drawing.Point(29, 216);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(112, 25);
            this.label74.TabIndex = 1;
            this.label74.Text = "斷面用途：";
            // 
            // lbl_SectionFunction
            // 
            this.lbl_SectionFunction.AutoSize = true;
            this.lbl_SectionFunction.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.lbl_SectionFunction.ForeColor = System.Drawing.Color.White;
            this.lbl_SectionFunction.Location = new System.Drawing.Point(143, 216);
            this.lbl_SectionFunction.Name = "lbl_SectionFunction";
            this.lbl_SectionFunction.Size = new System.Drawing.Size(177, 25);
            this.lbl_SectionFunction.TabIndex = 2;
            this.lbl_SectionFunction.Text = "Section Function";
            // 
            // lbl_ChosenProject
            // 
            this.lbl_ChosenProject.AutoSize = true;
            this.lbl_ChosenProject.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.lbl_ChosenProject.ForeColor = System.Drawing.Color.White;
            this.lbl_ChosenProject.Location = new System.Drawing.Point(143, 141);
            this.lbl_ChosenProject.Name = "lbl_ChosenProject";
            this.lbl_ChosenProject.Size = new System.Drawing.Size(153, 25);
            this.lbl_ChosenProject.TabIndex = 2;
            this.lbl_ChosenProject.Text = "ChosenProject";
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.lbl_User.ForeColor = System.Drawing.Color.White;
            this.lbl_User.Location = new System.Drawing.Point(143, 106);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(55, 25);
            this.lbl_User.TabIndex = 2;
            this.lbl_User.Text = "User";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Bold);
            this.label61.ForeColor = System.Drawing.Color.White;
            this.label61.Location = new System.Drawing.Point(49, 37);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(342, 50);
            this.label61.TabIndex = 2;
            this.label61.Text = "潛盾隧道分析設計";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label75.ForeColor = System.Drawing.Color.White;
            this.label75.Location = new System.Drawing.Point(29, 106);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(92, 25);
            this.label75.TabIndex = 2;
            this.label75.Text = "使用者：";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label76.ForeColor = System.Drawing.Color.White;
            this.label76.Location = new System.Drawing.Point(29, 141);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(112, 25);
            this.label76.TabIndex = 3;
            this.label76.Text = "選定標案：";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label77.ForeColor = System.Drawing.Color.White;
            this.label77.Location = new System.Drawing.Point(27, 175);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(112, 25);
            this.label77.TabIndex = 4;
            this.label77.Text = "選擇斷面：";
            // 
            // tabTunnel
            // 
            this.tabTunnel.Controls.Add(this.gBox_LoadTerm);
            this.tabTunnel.Controls.Add(this.groupBox2);
            this.tabTunnel.Controls.Add(this.gBox_Grout);
            this.tabTunnel.Controls.Add(this.gBox_SGProp);
            this.tabTunnel.Location = new System.Drawing.Point(4, 25);
            this.tabTunnel.Name = "tabTunnel";
            this.tabTunnel.Size = new System.Drawing.Size(1062, 546);
            this.tabTunnel.TabIndex = 5;
            this.tabTunnel.Text = "隧道環片分析";
            this.tabTunnel.UseVisualStyleBackColor = true;
            // 
            // gBox_LoadTerm
            // 
            this.gBox_LoadTerm.Controls.Add(this.btn_LoadTermSAP);
            this.gBox_LoadTerm.Controls.Add(this.gBox_VerInc);
            this.gBox_LoadTerm.Controls.Add(this.gBox_LatDec);
            this.gBox_LoadTerm.Controls.Add(this.btn_LoadTermExcel);
            this.gBox_LoadTerm.Controls.Add(this.cbo_LoadingTerm);
            this.gBox_LoadTerm.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBox_LoadTerm.Location = new System.Drawing.Point(566, 13);
            this.gBox_LoadTerm.Name = "gBox_LoadTerm";
            this.gBox_LoadTerm.Size = new System.Drawing.Size(475, 212);
            this.gBox_LoadTerm.TabIndex = 1;
            this.gBox_LoadTerm.TabStop = false;
            this.gBox_LoadTerm.Text = "不同載重狀態計算：雙環模式";
            // 
            // btn_LoadTermSAP
            // 
            this.btn_LoadTermSAP.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_LoadTermSAP.Location = new System.Drawing.Point(26, 138);
            this.btn_LoadTermSAP.Name = "btn_LoadTermSAP";
            this.btn_LoadTermSAP.Size = new System.Drawing.Size(120, 33);
            this.btn_LoadTermSAP.TabIndex = 48;
            this.btn_LoadTermSAP.Text = "SAP2000分析";
            this.btn_LoadTermSAP.UseVisualStyleBackColor = true;
            this.btn_LoadTermSAP.Click += new System.EventHandler(this.btn_LoadTermSAP_Click);
            // 
            // gBox_VerInc
            // 
            this.gBox_VerInc.Controls.Add(this.txt_VerIncRepeat);
            this.gBox_VerInc.Controls.Add(this.txt_VerIncInitial);
            this.gBox_VerInc.Controls.Add(this.label100);
            this.gBox_VerInc.Controls.Add(this.label98);
            this.gBox_VerInc.Controls.Add(this.label96);
            this.gBox_VerInc.Controls.Add(this.label99);
            this.gBox_VerInc.Controls.Add(this.label97);
            this.gBox_VerInc.Controls.Add(this.lbl_VerIncValue);
            this.gBox_VerInc.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBox_VerInc.Location = new System.Drawing.Point(324, 24);
            this.gBox_VerInc.Name = "gBox_VerInc";
            this.gBox_VerInc.Size = new System.Drawing.Size(136, 182);
            this.gBox_VerInc.TabIndex = 1;
            this.gBox_VerInc.TabStop = false;
            this.gBox_VerInc.Text = "軸向加壓(kN)";
            // 
            // txt_VerIncRepeat
            // 
            this.txt_VerIncRepeat.Location = new System.Drawing.Point(69, 145);
            this.txt_VerIncRepeat.Name = "txt_VerIncRepeat";
            this.txt_VerIncRepeat.Size = new System.Drawing.Size(61, 25);
            this.txt_VerIncRepeat.TabIndex = 3;
            // 
            // txt_VerIncInitial
            // 
            this.txt_VerIncInitial.Location = new System.Drawing.Point(69, 112);
            this.txt_VerIncInitial.Name = "txt_VerIncInitial";
            this.txt_VerIncInitial.Size = new System.Drawing.Size(61, 25);
            this.txt_VerIncInitial.TabIndex = 3;
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label100.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label100.Location = new System.Drawing.Point(26, 30);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(86, 21);
            this.label100.TabIndex = 1;
            this.label100.Text = "計算結果值";
            // 
            // label98
            // 
            this.label98.AutoSize = true;
            this.label98.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label98.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label98.Location = new System.Drawing.Point(26, 87);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(86, 21);
            this.label98.TabIndex = 1;
            this.label98.Text = "重新計算值";
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label96.Location = new System.Drawing.Point(6, 60);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(64, 18);
            this.label96.TabIndex = 1;
            this.label96.Text = "加壓值：";
            // 
            // label99
            // 
            this.label99.AutoSize = true;
            this.label99.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label99.Location = new System.Drawing.Point(6, 149);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(64, 18);
            this.label99.TabIndex = 1;
            this.label99.Text = "迭代值：";
            // 
            // label97
            // 
            this.label97.AutoSize = true;
            this.label97.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label97.Location = new System.Drawing.Point(6, 114);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(64, 18);
            this.label97.TabIndex = 1;
            this.label97.Text = "初始值：";
            // 
            // lbl_VerIncValue
            // 
            this.lbl_VerIncValue.AutoSize = true;
            this.lbl_VerIncValue.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_VerIncValue.Location = new System.Drawing.Point(74, 60);
            this.lbl_VerIncValue.Name = "lbl_VerIncValue";
            this.lbl_VerIncValue.Size = new System.Drawing.Size(47, 18);
            this.lbl_VerIncValue.TabIndex = 1;
            this.lbl_VerIncValue.Text = "Value";
            // 
            // gBox_LatDec
            // 
            this.gBox_LatDec.Controls.Add(this.txt_LatDecRepeat);
            this.gBox_LatDec.Controls.Add(this.txt_LatDecInitial);
            this.gBox_LatDec.Controls.Add(this.label94);
            this.gBox_LatDec.Controls.Add(this.label101);
            this.gBox_LatDec.Controls.Add(this.label95);
            this.gBox_LatDec.Controls.Add(this.label93);
            this.gBox_LatDec.Controls.Add(this.lbl_LatDecValue);
            this.gBox_LatDec.Controls.Add(this.label92);
            this.gBox_LatDec.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBox_LatDec.Location = new System.Drawing.Point(171, 24);
            this.gBox_LatDec.Name = "gBox_LatDec";
            this.gBox_LatDec.Size = new System.Drawing.Size(136, 182);
            this.gBox_LatDec.TabIndex = 1;
            this.gBox_LatDec.TabStop = false;
            this.gBox_LatDec.Text = "側向解壓(kN)";
            // 
            // txt_LatDecRepeat
            // 
            this.txt_LatDecRepeat.Location = new System.Drawing.Point(69, 145);
            this.txt_LatDecRepeat.Name = "txt_LatDecRepeat";
            this.txt_LatDecRepeat.Size = new System.Drawing.Size(61, 25);
            this.txt_LatDecRepeat.TabIndex = 2;
            // 
            // txt_LatDecInitial
            // 
            this.txt_LatDecInitial.Location = new System.Drawing.Point(69, 110);
            this.txt_LatDecInitial.Name = "txt_LatDecInitial";
            this.txt_LatDecInitial.Size = new System.Drawing.Size(61, 25);
            this.txt_LatDecInitial.TabIndex = 2;
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label94.Location = new System.Drawing.Point(6, 148);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(64, 18);
            this.label94.TabIndex = 1;
            this.label94.Text = "迭代值：";
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label101.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label101.Location = new System.Drawing.Point(18, 30);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(86, 21);
            this.label101.TabIndex = 1;
            this.label101.Text = "計算結果值";
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label95.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label95.Location = new System.Drawing.Point(18, 86);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(86, 21);
            this.label95.TabIndex = 1;
            this.label95.Text = "重新計算值";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label93.Location = new System.Drawing.Point(6, 113);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(64, 18);
            this.label93.TabIndex = 1;
            this.label93.Text = "初始值：";
            // 
            // lbl_LatDecValue
            // 
            this.lbl_LatDecValue.AutoSize = true;
            this.lbl_LatDecValue.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_LatDecValue.Location = new System.Drawing.Point(66, 59);
            this.lbl_LatDecValue.Name = "lbl_LatDecValue";
            this.lbl_LatDecValue.Size = new System.Drawing.Size(47, 18);
            this.lbl_LatDecValue.TabIndex = 1;
            this.lbl_LatDecValue.Text = "Value";
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.label92.Location = new System.Drawing.Point(6, 59);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(64, 18);
            this.label92.TabIndex = 1;
            this.label92.Text = "解壓值：";
            // 
            // btn_LoadTermExcel
            // 
            this.btn_LoadTermExcel.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_LoadTermExcel.Location = new System.Drawing.Point(26, 84);
            this.btn_LoadTermExcel.Name = "btn_LoadTermExcel";
            this.btn_LoadTermExcel.Size = new System.Drawing.Size(120, 33);
            this.btn_LoadTermExcel.TabIndex = 47;
            this.btn_LoadTermExcel.Text = "輸出Excel分析表";
            this.btn_LoadTermExcel.UseVisualStyleBackColor = true;
            this.btn_LoadTermExcel.Click += new System.EventHandler(this.btn_LoadTermExcel_Click);
            // 
            // cbo_LoadingTerm
            // 
            this.cbo_LoadingTerm.FormattingEnabled = true;
            this.cbo_LoadingTerm.Location = new System.Drawing.Point(19, 32);
            this.cbo_LoadingTerm.Name = "cbo_LoadingTerm";
            this.cbo_LoadingTerm.Size = new System.Drawing.Size(135, 25);
            this.cbo_LoadingTerm.TabIndex = 0;
            this.cbo_LoadingTerm.SelectionChangeCommitted += new System.EventHandler(this.cbo_LoadingTerm_SelectionChangeCommitted);
            // 
            // groupBox2
            // 
            this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox2.Location = new System.Drawing.Point(393, 289);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 133);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox1";
            // 
            // gBox_Grout
            // 
            this.gBox_Grout.Controls.Add(this.label91);
            this.gBox_Grout.Controls.Add(this.btn_GroutExcel);
            this.gBox_Grout.Controls.Add(this.btn_GroutSAP);
            this.gBox_Grout.Controls.Add(this.lbl_GroutP);
            this.gBox_Grout.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBox_Grout.Location = new System.Drawing.Point(340, 13);
            this.gBox_Grout.Name = "gBox_Grout";
            this.gBox_Grout.Size = new System.Drawing.Size(208, 210);
            this.gBox_Grout.TabIndex = 1;
            this.gBox_Grout.TabStop = false;
            this.gBox_Grout.Text = "背填灌漿計算";
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label91.Location = new System.Drawing.Point(6, 32);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(112, 25);
            this.label91.TabIndex = 1;
            this.label91.Text = "灌漿壓力：";
            // 
            // btn_GroutExcel
            // 
            this.btn_GroutExcel.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_GroutExcel.Location = new System.Drawing.Point(55, 84);
            this.btn_GroutExcel.Name = "btn_GroutExcel";
            this.btn_GroutExcel.Size = new System.Drawing.Size(120, 33);
            this.btn_GroutExcel.TabIndex = 0;
            this.btn_GroutExcel.Text = "輸出Excel分析表";
            this.btn_GroutExcel.UseVisualStyleBackColor = true;
            this.btn_GroutExcel.Click += new System.EventHandler(this.btn_GroutExcel_Click);
            // 
            // btn_GroutSAP
            // 
            this.btn_GroutSAP.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_GroutSAP.Location = new System.Drawing.Point(55, 138);
            this.btn_GroutSAP.Name = "btn_GroutSAP";
            this.btn_GroutSAP.Size = new System.Drawing.Size(120, 33);
            this.btn_GroutSAP.TabIndex = 0;
            this.btn_GroutSAP.Text = "SAP2000分析";
            this.btn_GroutSAP.UseVisualStyleBackColor = true;
            this.btn_GroutSAP.Click += new System.EventHandler(this.btn_GroutSAP_Click);
            // 
            // lbl_GroutP
            // 
            this.lbl_GroutP.AutoSize = true;
            this.lbl_GroutP.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_GroutP.Location = new System.Drawing.Point(115, 34);
            this.lbl_GroutP.Name = "lbl_GroutP";
            this.lbl_GroutP.Size = new System.Drawing.Size(82, 23);
            this.lbl_GroutP.TabIndex = 46;
            this.lbl_GroutP.Text = "Pressure";
            this.lbl_GroutP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gBox_SGProp
            // 
            this.gBox_SGProp.Controls.Add(this.lbl_SGFy);
            this.gBox_SGProp.Controls.Add(this.lbl_SGFc);
            this.gBox_SGProp.Controls.Add(this.lbl_SGPoissonR);
            this.gBox_SGProp.Controls.Add(this.lbl_SGE);
            this.gBox_SGProp.Controls.Add(this.lbl_SGUW);
            this.gBox_SGProp.Controls.Add(this.lbl_SGGroutAngle);
            this.gBox_SGProp.Controls.Add(this.lbl_SGPoreAngle);
            this.gBox_SGProp.Controls.Add(this.lbl_SGKAngle);
            this.gBox_SGProp.Controls.Add(this.lbl_SGBAngle);
            this.gBox_SGProp.Controls.Add(this.lbl_SGAAngle);
            this.gBox_SGProp.Controls.Add(this.lbl_SGAngle);
            this.gBox_SGProp.Controls.Add(this.lbl_SGThick);
            this.gBox_SGProp.Controls.Add(this.lbl_SGRadiusIn);
            this.gBox_SGProp.Controls.Add(this.label78);
            this.gBox_SGProp.Controls.Add(this.label79);
            this.gBox_SGProp.Controls.Add(this.label89);
            this.gBox_SGProp.Controls.Add(this.label88);
            this.gBox_SGProp.Controls.Add(this.label90);
            this.gBox_SGProp.Controls.Add(this.label87);
            this.gBox_SGProp.Controls.Add(this.label86);
            this.gBox_SGProp.Controls.Add(this.label80);
            this.gBox_SGProp.Controls.Add(this.label85);
            this.gBox_SGProp.Controls.Add(this.label81);
            this.gBox_SGProp.Controls.Add(this.label84);
            this.gBox_SGProp.Controls.Add(this.label82);
            this.gBox_SGProp.Controls.Add(this.label83);
            this.gBox_SGProp.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBox_SGProp.Location = new System.Drawing.Point(8, 13);
            this.gBox_SGProp.Name = "gBox_SGProp";
            this.gBox_SGProp.Size = new System.Drawing.Size(326, 528);
            this.gBox_SGProp.TabIndex = 0;
            this.gBox_SGProp.TabStop = false;
            this.gBox_SGProp.Text = "環片參數";
            // 
            // lbl_SGFy
            // 
            this.lbl_SGFy.AutoSize = true;
            this.lbl_SGFy.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGFy.Location = new System.Drawing.Point(172, 492);
            this.lbl_SGFy.Name = "lbl_SGFy";
            this.lbl_SGFy.Size = new System.Drawing.Size(28, 23);
            this.lbl_SGFy.TabIndex = 46;
            this.lbl_SGFy.Text = "Fy";
            this.lbl_SGFy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGFc
            // 
            this.lbl_SGFc.AutoSize = true;
            this.lbl_SGFc.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGFc.Location = new System.Drawing.Point(172, 454);
            this.lbl_SGFc.Name = "lbl_SGFc";
            this.lbl_SGFc.Size = new System.Drawing.Size(28, 23);
            this.lbl_SGFc.TabIndex = 46;
            this.lbl_SGFc.Text = "Fc";
            this.lbl_SGFc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGPoissonR
            // 
            this.lbl_SGPoissonR.AutoSize = true;
            this.lbl_SGPoissonR.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGPoissonR.Location = new System.Drawing.Point(172, 417);
            this.lbl_SGPoissonR.Name = "lbl_SGPoissonR";
            this.lbl_SGPoissonR.Size = new System.Drawing.Size(87, 23);
            this.lbl_SGPoissonR.TabIndex = 46;
            this.lbl_SGPoissonR.Text = "PoissonR";
            this.lbl_SGPoissonR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGE
            // 
            this.lbl_SGE.AutoSize = true;
            this.lbl_SGE.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGE.Location = new System.Drawing.Point(172, 384);
            this.lbl_SGE.Name = "lbl_SGE";
            this.lbl_SGE.Size = new System.Drawing.Size(20, 23);
            this.lbl_SGE.TabIndex = 46;
            this.lbl_SGE.Text = "E";
            this.lbl_SGE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGUW
            // 
            this.lbl_SGUW.AutoSize = true;
            this.lbl_SGUW.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGUW.Location = new System.Drawing.Point(172, 348);
            this.lbl_SGUW.Name = "lbl_SGUW";
            this.lbl_SGUW.Size = new System.Drawing.Size(41, 23);
            this.lbl_SGUW.TabIndex = 46;
            this.lbl_SGUW.Text = "UW";
            this.lbl_SGUW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGGroutAngle
            // 
            this.lbl_SGGroutAngle.AutoSize = true;
            this.lbl_SGGroutAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGGroutAngle.Location = new System.Drawing.Point(172, 308);
            this.lbl_SGGroutAngle.Name = "lbl_SGGroutAngle";
            this.lbl_SGGroutAngle.Size = new System.Drawing.Size(59, 23);
            this.lbl_SGGroutAngle.TabIndex = 46;
            this.lbl_SGGroutAngle.Text = "Grout";
            this.lbl_SGGroutAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGPoreAngle
            // 
            this.lbl_SGPoreAngle.AutoSize = true;
            this.lbl_SGPoreAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGPoreAngle.Location = new System.Drawing.Point(172, 267);
            this.lbl_SGPoreAngle.Name = "lbl_SGPoreAngle";
            this.lbl_SGPoreAngle.Size = new System.Drawing.Size(49, 23);
            this.lbl_SGPoreAngle.TabIndex = 46;
            this.lbl_SGPoreAngle.Text = "Pore";
            this.lbl_SGPoreAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGKAngle
            // 
            this.lbl_SGKAngle.AutoSize = true;
            this.lbl_SGKAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGKAngle.Location = new System.Drawing.Point(172, 227);
            this.lbl_SGKAngle.Name = "lbl_SGKAngle";
            this.lbl_SGKAngle.Size = new System.Drawing.Size(70, 23);
            this.lbl_SGKAngle.TabIndex = 46;
            this.lbl_SGKAngle.Text = "KAngle";
            this.lbl_SGKAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGBAngle
            // 
            this.lbl_SGBAngle.AutoSize = true;
            this.lbl_SGBAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGBAngle.Location = new System.Drawing.Point(172, 187);
            this.lbl_SGBAngle.Name = "lbl_SGBAngle";
            this.lbl_SGBAngle.Size = new System.Drawing.Size(70, 23);
            this.lbl_SGBAngle.TabIndex = 46;
            this.lbl_SGBAngle.Text = "BAngle";
            this.lbl_SGBAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGAAngle
            // 
            this.lbl_SGAAngle.AutoSize = true;
            this.lbl_SGAAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGAAngle.Location = new System.Drawing.Point(172, 148);
            this.lbl_SGAAngle.Name = "lbl_SGAAngle";
            this.lbl_SGAAngle.Size = new System.Drawing.Size(71, 23);
            this.lbl_SGAAngle.TabIndex = 46;
            this.lbl_SGAAngle.Text = "AAngle";
            this.lbl_SGAAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGAngle
            // 
            this.lbl_SGAngle.AutoSize = true;
            this.lbl_SGAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGAngle.Location = new System.Drawing.Point(172, 108);
            this.lbl_SGAngle.Name = "lbl_SGAngle";
            this.lbl_SGAngle.Size = new System.Drawing.Size(59, 23);
            this.lbl_SGAngle.TabIndex = 46;
            this.lbl_SGAngle.Text = "Angle";
            this.lbl_SGAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGThick
            // 
            this.lbl_SGThick.AutoSize = true;
            this.lbl_SGThick.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGThick.Location = new System.Drawing.Point(172, 71);
            this.lbl_SGThick.Name = "lbl_SGThick";
            this.lbl_SGThick.Size = new System.Drawing.Size(52, 23);
            this.lbl_SGThick.TabIndex = 46;
            this.lbl_SGThick.Text = "thick";
            this.lbl_SGThick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SGRadiusIn
            // 
            this.lbl_SGRadiusIn.AutoSize = true;
            this.lbl_SGRadiusIn.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SGRadiusIn.Location = new System.Drawing.Point(172, 35);
            this.lbl_SGRadiusIn.Name = "lbl_SGRadiusIn";
            this.lbl_SGRadiusIn.Size = new System.Drawing.Size(62, 23);
            this.lbl_SGRadiusIn.TabIndex = 46;
            this.lbl_SGRadiusIn.Text = "radius";
            this.lbl_SGRadiusIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label78
            // 
            this.label78.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label78.Location = new System.Drawing.Point(5, 35);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(172, 25);
            this.label78.TabIndex = 46;
            this.label78.Text = "環片內半徑：";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label79
            // 
            this.label79.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label79.Location = new System.Drawing.Point(5, 71);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(172, 25);
            this.label79.TabIndex = 54;
            this.label79.Text = "環片厚度：";
            this.label79.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label89
            // 
            this.label89.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label89.Location = new System.Drawing.Point(5, 492);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(172, 25);
            this.label89.TabIndex = 50;
            this.label89.Text = "鋼筋降伏強度：";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label88.Location = new System.Drawing.Point(5, 454);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(172, 25);
            this.label88.TabIndex = 50;
            this.label88.Text = "混凝土強度：";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label90
            // 
            this.label90.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label90.Location = new System.Drawing.Point(4, 417);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(172, 25);
            this.label90.TabIndex = 50;
            this.label90.Text = "柏松比：";
            this.label90.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label87
            // 
            this.label87.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label87.Location = new System.Drawing.Point(5, 384);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(172, 25);
            this.label87.TabIndex = 50;
            this.label87.Text = "楊式模數：";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label86
            // 
            this.label86.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label86.Location = new System.Drawing.Point(5, 348);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(172, 25);
            this.label86.TabIndex = 50;
            this.label86.Text = "混凝土單位重：";
            this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label80
            // 
            this.label80.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label80.Location = new System.Drawing.Point(5, 108);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(172, 25);
            this.label80.TabIndex = 53;
            this.label80.Text = "環片內角度：";
            this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label85
            // 
            this.label85.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label85.Location = new System.Drawing.Point(5, 308);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(172, 25);
            this.label85.TabIndex = 47;
            this.label85.Text = "相鄰灌漿孔角度：";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label81
            // 
            this.label81.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label81.Location = new System.Drawing.Point(5, 148);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(172, 25);
            this.label81.TabIndex = 52;
            this.label81.Text = "A環片角度：";
            this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label84
            // 
            this.label84.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label84.Location = new System.Drawing.Point(5, 267);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(172, 25);
            this.label84.TabIndex = 48;
            this.label84.Text = "相鄰螺栓孔角度：";
            this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label82
            // 
            this.label82.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label82.Location = new System.Drawing.Point(5, 187);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(172, 25);
            this.label82.TabIndex = 51;
            this.label82.Text = "B環片角度：";
            this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label83
            // 
            this.label83.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label83.Location = new System.Drawing.Point(5, 227);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(172, 25);
            this.label83.TabIndex = 49;
            this.label83.Text = "K環片角度：";
            this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabConnect
            // 
            this.tabConnect.Controls.Add(this.gpConnectTunnel);
            this.tabConnect.Location = new System.Drawing.Point(4, 25);
            this.tabConnect.Name = "tabConnect";
            this.tabConnect.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnect.Size = new System.Drawing.Size(1062, 546);
            this.tabConnect.TabIndex = 1;
            this.tabConnect.Text = "聯絡通道分析";
            this.tabConnect.UseVisualStyleBackColor = true;
            // 
            // gpConnectTunnel
            // 
            this.gpConnectTunnel.Controls.Add(this.gBox_ConnectorCal);
            this.gpConnectTunnel.Controls.Add(this.gBOX_ConnectorProp);
            this.gpConnectTunnel.Controls.Add(this.pictureBox1);
            this.gpConnectTunnel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpConnectTunnel.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.gpConnectTunnel.ForeColor = System.Drawing.Color.Blue;
            this.gpConnectTunnel.Location = new System.Drawing.Point(3, 3);
            this.gpConnectTunnel.Name = "gpConnectTunnel";
            this.gpConnectTunnel.Size = new System.Drawing.Size(1056, 540);
            this.gpConnectTunnel.TabIndex = 12;
            this.gpConnectTunnel.TabStop = false;
            this.gpConnectTunnel.Text = "聯絡通道分析 :";
            // 
            // gBox_ConnectorCal
            // 
            this.gBox_ConnectorCal.Controls.Add(this.btn_ConnectorExcel);
            this.gBox_ConnectorCal.Controls.Add(this.btn_ConnectorCal);
            this.gBox_ConnectorCal.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBox_ConnectorCal.ForeColor = System.Drawing.Color.Black;
            this.gBox_ConnectorCal.Location = new System.Drawing.Point(351, 32);
            this.gBox_ConnectorCal.Name = "gBox_ConnectorCal";
            this.gBox_ConnectorCal.Size = new System.Drawing.Size(200, 133);
            this.gBox_ConnectorCal.TabIndex = 25;
            this.gBox_ConnectorCal.TabStop = false;
            this.gBox_ConnectorCal.Text = "聯絡通道計算";
            // 
            // btn_ConnectorExcel
            // 
            this.btn_ConnectorExcel.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_ConnectorExcel.ForeColor = System.Drawing.Color.Black;
            this.btn_ConnectorExcel.Location = new System.Drawing.Point(34, 34);
            this.btn_ConnectorExcel.Name = "btn_ConnectorExcel";
            this.btn_ConnectorExcel.Size = new System.Drawing.Size(109, 29);
            this.btn_ConnectorExcel.TabIndex = 23;
            this.btn_ConnectorExcel.Text = "輸出Excel";
            this.btn_ConnectorExcel.UseVisualStyleBackColor = true;
            this.btn_ConnectorExcel.Click += new System.EventHandler(this.btn_ConnectorExcel_Click);
            // 
            // btn_ConnectorCal
            // 
            this.btn_ConnectorCal.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_ConnectorCal.ForeColor = System.Drawing.Color.Black;
            this.btn_ConnectorCal.Location = new System.Drawing.Point(34, 86);
            this.btn_ConnectorCal.Name = "btn_ConnectorCal";
            this.btn_ConnectorCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_ConnectorCal.Size = new System.Drawing.Size(109, 29);
            this.btn_ConnectorCal.TabIndex = 19;
            this.btn_ConnectorCal.Text = "執行分析";
            this.btn_ConnectorCal.UseVisualStyleBackColor = true;
            this.btn_ConnectorCal.Click += new System.EventHandler(this.btn_ConnectorCal_Click);
            // 
            // gBOX_ConnectorProp
            // 
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_ConnectorFy);
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_ConnectorFc);
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_ConnectorPR);
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_ConnectorE);
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_TR);
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_BH);
            this.gBOX_ConnectorProp.Controls.Add(this.lbl_ConnectorUW);
            this.gBOX_ConnectorProp.Controls.Add(this.label107);
            this.gBOX_ConnectorProp.Controls.Add(this.label108);
            this.gBOX_ConnectorProp.Controls.Add(this.label109);
            this.gBOX_ConnectorProp.Controls.Add(this.label110);
            this.gBOX_ConnectorProp.Controls.Add(this.label113);
            this.gBOX_ConnectorProp.Controls.Add(this.label112);
            this.gBOX_ConnectorProp.Controls.Add(this.label111);
            this.gBOX_ConnectorProp.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.gBOX_ConnectorProp.ForeColor = System.Drawing.Color.Black;
            this.gBOX_ConnectorProp.Location = new System.Drawing.Point(6, 32);
            this.gBOX_ConnectorProp.Name = "gBOX_ConnectorProp";
            this.gBOX_ConnectorProp.Size = new System.Drawing.Size(326, 326);
            this.gBOX_ConnectorProp.TabIndex = 24;
            this.gBOX_ConnectorProp.TabStop = false;
            this.gBOX_ConnectorProp.Text = "參數資料";
            // 
            // lbl_ConnectorFy
            // 
            this.lbl_ConnectorFy.AutoSize = true;
            this.lbl_ConnectorFy.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_ConnectorFy.Location = new System.Drawing.Point(173, 251);
            this.lbl_ConnectorFy.Name = "lbl_ConnectorFy";
            this.lbl_ConnectorFy.Size = new System.Drawing.Size(28, 23);
            this.lbl_ConnectorFy.TabIndex = 51;
            this.lbl_ConnectorFy.Text = "Fy";
            this.lbl_ConnectorFy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ConnectorFc
            // 
            this.lbl_ConnectorFc.AutoSize = true;
            this.lbl_ConnectorFc.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_ConnectorFc.Location = new System.Drawing.Point(173, 213);
            this.lbl_ConnectorFc.Name = "lbl_ConnectorFc";
            this.lbl_ConnectorFc.Size = new System.Drawing.Size(28, 23);
            this.lbl_ConnectorFc.TabIndex = 52;
            this.lbl_ConnectorFc.Text = "Fc";
            this.lbl_ConnectorFc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ConnectorPR
            // 
            this.lbl_ConnectorPR.AutoSize = true;
            this.lbl_ConnectorPR.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_ConnectorPR.Location = new System.Drawing.Point(173, 176);
            this.lbl_ConnectorPR.Name = "lbl_ConnectorPR";
            this.lbl_ConnectorPR.Size = new System.Drawing.Size(87, 23);
            this.lbl_ConnectorPR.TabIndex = 53;
            this.lbl_ConnectorPR.Text = "PoissonR";
            this.lbl_ConnectorPR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ConnectorE
            // 
            this.lbl_ConnectorE.AutoSize = true;
            this.lbl_ConnectorE.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_ConnectorE.Location = new System.Drawing.Point(173, 143);
            this.lbl_ConnectorE.Name = "lbl_ConnectorE";
            this.lbl_ConnectorE.Size = new System.Drawing.Size(20, 23);
            this.lbl_ConnectorE.TabIndex = 54;
            this.lbl_ConnectorE.Text = "E";
            this.lbl_ConnectorE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_TR
            // 
            this.lbl_TR.AutoSize = true;
            this.lbl_TR.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_TR.Location = new System.Drawing.Point(173, 34);
            this.lbl_TR.Name = "lbl_TR";
            this.lbl_TR.Size = new System.Drawing.Size(32, 23);
            this.lbl_TR.TabIndex = 55;
            this.lbl_TR.Text = "TR";
            this.lbl_TR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_BH
            // 
            this.lbl_BH.AutoSize = true;
            this.lbl_BH.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_BH.Location = new System.Drawing.Point(173, 72);
            this.lbl_BH.Name = "lbl_BH";
            this.lbl_BH.Size = new System.Drawing.Size(35, 23);
            this.lbl_BH.TabIndex = 55;
            this.lbl_BH.Text = "BH";
            this.lbl_BH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ConnectorUW
            // 
            this.lbl_ConnectorUW.AutoSize = true;
            this.lbl_ConnectorUW.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_ConnectorUW.Location = new System.Drawing.Point(173, 107);
            this.lbl_ConnectorUW.Name = "lbl_ConnectorUW";
            this.lbl_ConnectorUW.Size = new System.Drawing.Size(41, 23);
            this.lbl_ConnectorUW.TabIndex = 55;
            this.lbl_ConnectorUW.Text = "UW";
            this.lbl_ConnectorUW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label107
            // 
            this.label107.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label107.Location = new System.Drawing.Point(6, 251);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(172, 25);
            this.label107.TabIndex = 56;
            this.label107.Text = "鋼筋降伏強度：";
            this.label107.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label108
            // 
            this.label108.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label108.Location = new System.Drawing.Point(6, 213);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(172, 25);
            this.label108.TabIndex = 57;
            this.label108.Text = "混凝土強度：";
            this.label108.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label109
            // 
            this.label109.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label109.Location = new System.Drawing.Point(5, 176);
            this.label109.Name = "label109";
            this.label109.Size = new System.Drawing.Size(172, 25);
            this.label109.TabIndex = 58;
            this.label109.Text = "柏松比：";
            this.label109.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label110
            // 
            this.label110.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label110.Location = new System.Drawing.Point(6, 143);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(172, 25);
            this.label110.TabIndex = 59;
            this.label110.Text = "楊式模數：";
            this.label110.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label113
            // 
            this.label113.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label113.Location = new System.Drawing.Point(6, 31);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(172, 25);
            this.label113.TabIndex = 60;
            this.label113.Text = "側牆半徑：";
            this.label113.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label112
            // 
            this.label112.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label112.Location = new System.Drawing.Point(6, 70);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(172, 25);
            this.label112.TabIndex = 60;
            this.label112.Text = "底板高：";
            this.label112.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label111
            // 
            this.label111.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label111.Location = new System.Drawing.Point(6, 107);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(172, 25);
            this.label111.TabIndex = 60;
            this.label111.Text = "混凝土單位重：";
            this.label111.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(601, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(387, 326);
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // tabSteelTunnel
            // 
            this.tabSteelTunnel.Controls.Add(this.gpSteelTunnel);
            this.tabSteelTunnel.Location = new System.Drawing.Point(4, 25);
            this.tabSteelTunnel.Name = "tabSteelTunnel";
            this.tabSteelTunnel.Size = new System.Drawing.Size(1062, 546);
            this.tabSteelTunnel.TabIndex = 2;
            this.tabSteelTunnel.Text = "鋼環片分析";
            this.tabSteelTunnel.UseVisualStyleBackColor = true;
            // 
            // gpSteelTunnel
            // 
            this.gpSteelTunnel.Controls.Add(this.groupBox5);
            this.gpSteelTunnel.Controls.Add(this.groupBox4);
            this.gpSteelTunnel.Controls.Add(this.groupBox3);
            this.gpSteelTunnel.Controls.Add(this.groupBox1);
            this.gpSteelTunnel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpSteelTunnel.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.gpSteelTunnel.ForeColor = System.Drawing.Color.Blue;
            this.gpSteelTunnel.Location = new System.Drawing.Point(0, 0);
            this.gpSteelTunnel.Name = "gpSteelTunnel";
            this.gpSteelTunnel.Size = new System.Drawing.Size(1062, 546);
            this.gpSteelTunnel.TabIndex = 15;
            this.gpSteelTunnel.TabStop = false;
            this.gpSteelTunnel.Text = "鋼環片分析 :";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_SteelExcel);
            this.groupBox5.Controls.Add(this.btn_SteelCal);
            this.groupBox5.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox5.ForeColor = System.Drawing.Color.Black;
            this.groupBox5.Location = new System.Drawing.Point(401, 30);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 133);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "鋼環片計算";
            // 
            // btn_SteelExcel
            // 
            this.btn_SteelExcel.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_SteelExcel.ForeColor = System.Drawing.Color.Black;
            this.btn_SteelExcel.Location = new System.Drawing.Point(44, 35);
            this.btn_SteelExcel.Name = "btn_SteelExcel";
            this.btn_SteelExcel.Size = new System.Drawing.Size(109, 29);
            this.btn_SteelExcel.TabIndex = 31;
            this.btn_SteelExcel.Text = "輸出Excel";
            this.btn_SteelExcel.UseVisualStyleBackColor = true;
            this.btn_SteelExcel.Click += new System.EventHandler(this.btn_SteelExcel_Click);
            // 
            // btn_SteelCal
            // 
            this.btn_SteelCal.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_SteelCal.ForeColor = System.Drawing.Color.Black;
            this.btn_SteelCal.Location = new System.Drawing.Point(44, 83);
            this.btn_SteelCal.Name = "btn_SteelCal";
            this.btn_SteelCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_SteelCal.Size = new System.Drawing.Size(109, 29);
            this.btn_SteelCal.TabIndex = 19;
            this.btn_SteelCal.Text = "執行分析";
            this.btn_SteelCal.UseVisualStyleBackColor = true;
            this.btn_SteelCal.Click += new System.EventHandler(this.btn_SteelCal_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbl_SteelPoreAngle);
            this.groupBox4.Controls.Add(this.lbl_SteelKAngle);
            this.groupBox4.Controls.Add(this.lbl_SteelBAngle);
            this.groupBox4.Controls.Add(this.lbl_SteelAAngle);
            this.groupBox4.Controls.Add(this.lbl_SteelThick);
            this.groupBox4.Controls.Add(this.lbl_SteelRadiusIn);
            this.groupBox4.Controls.Add(this.label130);
            this.groupBox4.Controls.Add(this.label131);
            this.groupBox4.Controls.Add(this.label139);
            this.groupBox4.Controls.Add(this.label140);
            this.groupBox4.Controls.Add(this.label141);
            this.groupBox4.Controls.Add(this.label142);
            this.groupBox4.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox4.ForeColor = System.Drawing.Color.Black;
            this.groupBox4.Location = new System.Drawing.Point(8, 30);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(326, 273);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "隧道與環片參數";
            // 
            // lbl_SteelPoreAngle
            // 
            this.lbl_SteelPoreAngle.AutoSize = true;
            this.lbl_SteelPoreAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelPoreAngle.Location = new System.Drawing.Point(172, 223);
            this.lbl_SteelPoreAngle.Name = "lbl_SteelPoreAngle";
            this.lbl_SteelPoreAngle.Size = new System.Drawing.Size(49, 23);
            this.lbl_SteelPoreAngle.TabIndex = 46;
            this.lbl_SteelPoreAngle.Text = "Pore";
            this.lbl_SteelPoreAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelKAngle
            // 
            this.lbl_SteelKAngle.AutoSize = true;
            this.lbl_SteelKAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelKAngle.Location = new System.Drawing.Point(172, 183);
            this.lbl_SteelKAngle.Name = "lbl_SteelKAngle";
            this.lbl_SteelKAngle.Size = new System.Drawing.Size(70, 23);
            this.lbl_SteelKAngle.TabIndex = 46;
            this.lbl_SteelKAngle.Text = "KAngle";
            this.lbl_SteelKAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelBAngle
            // 
            this.lbl_SteelBAngle.AutoSize = true;
            this.lbl_SteelBAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelBAngle.Location = new System.Drawing.Point(172, 143);
            this.lbl_SteelBAngle.Name = "lbl_SteelBAngle";
            this.lbl_SteelBAngle.Size = new System.Drawing.Size(70, 23);
            this.lbl_SteelBAngle.TabIndex = 46;
            this.lbl_SteelBAngle.Text = "BAngle";
            this.lbl_SteelBAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelAAngle
            // 
            this.lbl_SteelAAngle.AutoSize = true;
            this.lbl_SteelAAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelAAngle.Location = new System.Drawing.Point(172, 104);
            this.lbl_SteelAAngle.Name = "lbl_SteelAAngle";
            this.lbl_SteelAAngle.Size = new System.Drawing.Size(71, 23);
            this.lbl_SteelAAngle.TabIndex = 46;
            this.lbl_SteelAAngle.Text = "AAngle";
            this.lbl_SteelAAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelThick
            // 
            this.lbl_SteelThick.AutoSize = true;
            this.lbl_SteelThick.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelThick.Location = new System.Drawing.Point(172, 71);
            this.lbl_SteelThick.Name = "lbl_SteelThick";
            this.lbl_SteelThick.Size = new System.Drawing.Size(52, 23);
            this.lbl_SteelThick.TabIndex = 46;
            this.lbl_SteelThick.Text = "thick";
            this.lbl_SteelThick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelRadiusIn
            // 
            this.lbl_SteelRadiusIn.AutoSize = true;
            this.lbl_SteelRadiusIn.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelRadiusIn.Location = new System.Drawing.Point(172, 35);
            this.lbl_SteelRadiusIn.Name = "lbl_SteelRadiusIn";
            this.lbl_SteelRadiusIn.Size = new System.Drawing.Size(62, 23);
            this.lbl_SteelRadiusIn.TabIndex = 46;
            this.lbl_SteelRadiusIn.Text = "radius";
            this.lbl_SteelRadiusIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label130
            // 
            this.label130.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label130.Location = new System.Drawing.Point(5, 35);
            this.label130.Name = "label130";
            this.label130.Size = new System.Drawing.Size(172, 25);
            this.label130.TabIndex = 46;
            this.label130.Text = "環片內半徑：";
            this.label130.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label131
            // 
            this.label131.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label131.Location = new System.Drawing.Point(5, 71);
            this.label131.Name = "label131";
            this.label131.Size = new System.Drawing.Size(172, 25);
            this.label131.TabIndex = 54;
            this.label131.Text = "環片厚度：";
            this.label131.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label139
            // 
            this.label139.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label139.Location = new System.Drawing.Point(5, 104);
            this.label139.Name = "label139";
            this.label139.Size = new System.Drawing.Size(172, 25);
            this.label139.TabIndex = 52;
            this.label139.Text = "A環片角度：";
            this.label139.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label140
            // 
            this.label140.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label140.Location = new System.Drawing.Point(5, 223);
            this.label140.Name = "label140";
            this.label140.Size = new System.Drawing.Size(172, 25);
            this.label140.TabIndex = 48;
            this.label140.Text = "相鄰螺栓孔角度：";
            this.label140.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label141
            // 
            this.label141.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label141.Location = new System.Drawing.Point(5, 143);
            this.label141.Name = "label141";
            this.label141.Size = new System.Drawing.Size(172, 25);
            this.label141.TabIndex = 51;
            this.label141.Text = "B環片角度：";
            this.label141.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label142
            // 
            this.label142.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label142.Location = new System.Drawing.Point(5, 183);
            this.label142.Name = "label142";
            this.label142.Size = new System.Drawing.Size(172, 25);
            this.label142.TabIndex = 49;
            this.label142.Text = "K環片角度：";
            this.label142.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbl_SteelFu);
            this.groupBox3.Controls.Add(this.lbl_SteelFy);
            this.groupBox3.Controls.Add(this.lbl_SteelPR);
            this.groupBox3.Controls.Add(this.lbl_SteelMat);
            this.groupBox3.Controls.Add(this.lbl_SteelUW);
            this.groupBox3.Controls.Add(this.label117);
            this.groupBox3.Controls.Add(this.label118);
            this.groupBox3.Controls.Add(this.label119);
            this.groupBox3.Controls.Add(this.label127);
            this.groupBox3.Controls.Add(this.label121);
            this.groupBox3.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(340, 309);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(326, 214);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "鋼環片參數";
            // 
            // lbl_SteelFu
            // 
            this.lbl_SteelFu.AutoSize = true;
            this.lbl_SteelFu.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelFu.Location = new System.Drawing.Point(174, 178);
            this.lbl_SteelFu.Name = "lbl_SteelFu";
            this.lbl_SteelFu.Size = new System.Drawing.Size(30, 23);
            this.lbl_SteelFu.TabIndex = 46;
            this.lbl_SteelFu.Text = "Fu";
            this.lbl_SteelFu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelFy
            // 
            this.lbl_SteelFy.AutoSize = true;
            this.lbl_SteelFy.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelFy.Location = new System.Drawing.Point(174, 140);
            this.lbl_SteelFy.Name = "lbl_SteelFy";
            this.lbl_SteelFy.Size = new System.Drawing.Size(28, 23);
            this.lbl_SteelFy.TabIndex = 46;
            this.lbl_SteelFy.Text = "Fy";
            this.lbl_SteelFy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelPR
            // 
            this.lbl_SteelPR.AutoSize = true;
            this.lbl_SteelPR.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelPR.Location = new System.Drawing.Point(174, 103);
            this.lbl_SteelPR.Name = "lbl_SteelPR";
            this.lbl_SteelPR.Size = new System.Drawing.Size(87, 23);
            this.lbl_SteelPR.TabIndex = 46;
            this.lbl_SteelPR.Text = "PoissonR";
            this.lbl_SteelPR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelMat
            // 
            this.lbl_SteelMat.AutoSize = true;
            this.lbl_SteelMat.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelMat.Location = new System.Drawing.Point(173, 33);
            this.lbl_SteelMat.Name = "lbl_SteelMat";
            this.lbl_SteelMat.Size = new System.Drawing.Size(81, 23);
            this.lbl_SteelMat.TabIndex = 46;
            this.lbl_SteelMat.Text = "Material";
            this.lbl_SteelMat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelUW
            // 
            this.lbl_SteelUW.AutoSize = true;
            this.lbl_SteelUW.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelUW.Location = new System.Drawing.Point(173, 66);
            this.lbl_SteelUW.Name = "lbl_SteelUW";
            this.lbl_SteelUW.Size = new System.Drawing.Size(41, 23);
            this.lbl_SteelUW.TabIndex = 46;
            this.lbl_SteelUW.Text = "UW";
            this.lbl_SteelUW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label117
            // 
            this.label117.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label117.Location = new System.Drawing.Point(7, 178);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(172, 25);
            this.label117.TabIndex = 50;
            this.label117.Text = "抗拉強度：";
            this.label117.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label118
            // 
            this.label118.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label118.Location = new System.Drawing.Point(7, 140);
            this.label118.Name = "label118";
            this.label118.Size = new System.Drawing.Size(172, 25);
            this.label118.TabIndex = 50;
            this.label118.Text = "降伏強度：";
            this.label118.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label119
            // 
            this.label119.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label119.Location = new System.Drawing.Point(6, 103);
            this.label119.Name = "label119";
            this.label119.Size = new System.Drawing.Size(172, 25);
            this.label119.TabIndex = 50;
            this.label119.Text = "柏松比：";
            this.label119.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label127
            // 
            this.label127.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label127.Location = new System.Drawing.Point(5, 33);
            this.label127.Name = "label127";
            this.label127.Size = new System.Drawing.Size(172, 25);
            this.label127.TabIndex = 50;
            this.label127.Text = "鋼環片性質：";
            this.label127.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label121
            // 
            this.label121.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label121.Location = new System.Drawing.Point(6, 66);
            this.label121.Name = "label121";
            this.label121.Size = new System.Drawing.Size(172, 25);
            this.label121.TabIndex = 50;
            this.label121.Text = "單位重：";
            this.label121.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_SteelConcreFy);
            this.groupBox1.Controls.Add(this.lbl_SteelConcreFc);
            this.groupBox1.Controls.Add(this.lbl_SteelConcrePR);
            this.groupBox1.Controls.Add(this.lbl_SteelConcreE);
            this.groupBox1.Controls.Add(this.lbl_SteelConcreUW);
            this.groupBox1.Controls.Add(this.label122);
            this.groupBox1.Controls.Add(this.label123);
            this.groupBox1.Controls.Add(this.label124);
            this.groupBox1.Controls.Add(this.label125);
            this.groupBox1.Controls.Add(this.label126);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(8, 309);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 214);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "混凝土環片參數";
            // 
            // lbl_SteelConcreFy
            // 
            this.lbl_SteelConcreFy.AutoSize = true;
            this.lbl_SteelConcreFy.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelConcreFy.Location = new System.Drawing.Point(173, 177);
            this.lbl_SteelConcreFy.Name = "lbl_SteelConcreFy";
            this.lbl_SteelConcreFy.Size = new System.Drawing.Size(28, 23);
            this.lbl_SteelConcreFy.TabIndex = 46;
            this.lbl_SteelConcreFy.Text = "Fy";
            this.lbl_SteelConcreFy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelConcreFc
            // 
            this.lbl_SteelConcreFc.AutoSize = true;
            this.lbl_SteelConcreFc.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelConcreFc.Location = new System.Drawing.Point(173, 139);
            this.lbl_SteelConcreFc.Name = "lbl_SteelConcreFc";
            this.lbl_SteelConcreFc.Size = new System.Drawing.Size(28, 23);
            this.lbl_SteelConcreFc.TabIndex = 46;
            this.lbl_SteelConcreFc.Text = "Fc";
            this.lbl_SteelConcreFc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelConcrePR
            // 
            this.lbl_SteelConcrePR.AutoSize = true;
            this.lbl_SteelConcrePR.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelConcrePR.Location = new System.Drawing.Point(173, 102);
            this.lbl_SteelConcrePR.Name = "lbl_SteelConcrePR";
            this.lbl_SteelConcrePR.Size = new System.Drawing.Size(87, 23);
            this.lbl_SteelConcrePR.TabIndex = 46;
            this.lbl_SteelConcrePR.Text = "PoissonR";
            this.lbl_SteelConcrePR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelConcreE
            // 
            this.lbl_SteelConcreE.AutoSize = true;
            this.lbl_SteelConcreE.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelConcreE.Location = new System.Drawing.Point(173, 69);
            this.lbl_SteelConcreE.Name = "lbl_SteelConcreE";
            this.lbl_SteelConcreE.Size = new System.Drawing.Size(20, 23);
            this.lbl_SteelConcreE.TabIndex = 46;
            this.lbl_SteelConcreE.Text = "E";
            this.lbl_SteelConcreE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SteelConcreUW
            // 
            this.lbl_SteelConcreUW.AutoSize = true;
            this.lbl_SteelConcreUW.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SteelConcreUW.Location = new System.Drawing.Point(173, 33);
            this.lbl_SteelConcreUW.Name = "lbl_SteelConcreUW";
            this.lbl_SteelConcreUW.Size = new System.Drawing.Size(41, 23);
            this.lbl_SteelConcreUW.TabIndex = 46;
            this.lbl_SteelConcreUW.Text = "UW";
            this.lbl_SteelConcreUW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label122
            // 
            this.label122.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label122.Location = new System.Drawing.Point(6, 177);
            this.label122.Name = "label122";
            this.label122.Size = new System.Drawing.Size(172, 25);
            this.label122.TabIndex = 50;
            this.label122.Text = "鋼筋降伏強度：";
            this.label122.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label123
            // 
            this.label123.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label123.Location = new System.Drawing.Point(6, 139);
            this.label123.Name = "label123";
            this.label123.Size = new System.Drawing.Size(172, 25);
            this.label123.TabIndex = 50;
            this.label123.Text = "混凝土強度：";
            this.label123.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label124
            // 
            this.label124.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label124.Location = new System.Drawing.Point(5, 102);
            this.label124.Name = "label124";
            this.label124.Size = new System.Drawing.Size(172, 25);
            this.label124.TabIndex = 50;
            this.label124.Text = "柏松比：";
            this.label124.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label125
            // 
            this.label125.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label125.Location = new System.Drawing.Point(6, 69);
            this.label125.Name = "label125";
            this.label125.Size = new System.Drawing.Size(172, 25);
            this.label125.TabIndex = 50;
            this.label125.Text = "楊式模數：";
            this.label125.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label126
            // 
            this.label126.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label126.Location = new System.Drawing.Point(6, 33);
            this.label126.Name = "label126";
            this.label126.Size = new System.Drawing.Size(172, 25);
            this.label126.TabIndex = 50;
            this.label126.Text = "混凝土單位重：";
            this.label126.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabSiteTunnel
            // 
            this.tabSiteTunnel.Controls.Add(this.gpSiteTunnel);
            this.tabSiteTunnel.Location = new System.Drawing.Point(4, 25);
            this.tabSiteTunnel.Name = "tabSiteTunnel";
            this.tabSiteTunnel.Size = new System.Drawing.Size(1062, 546);
            this.tabSiteTunnel.TabIndex = 3;
            this.tabSiteTunnel.Text = "場鑄環片分析";
            this.tabSiteTunnel.UseVisualStyleBackColor = true;
            // 
            // gpSiteTunnel
            // 
            this.gpSiteTunnel.Controls.Add(this.groupBox7);
            this.gpSiteTunnel.Controls.Add(this.groupBox6);
            this.gpSiteTunnel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpSiteTunnel.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.gpSiteTunnel.ForeColor = System.Drawing.Color.Blue;
            this.gpSiteTunnel.Location = new System.Drawing.Point(0, 0);
            this.gpSiteTunnel.Name = "gpSiteTunnel";
            this.gpSiteTunnel.Size = new System.Drawing.Size(1062, 546);
            this.gpSiteTunnel.TabIndex = 14;
            this.gpSiteTunnel.TabStop = false;
            this.gpSiteTunnel.Text = "場鑄環片分析 :";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btn_SiteExcel);
            this.groupBox7.Controls.Add(this.btn_SiteCal);
            this.groupBox7.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox7.ForeColor = System.Drawing.Color.Black;
            this.groupBox7.Location = new System.Drawing.Point(363, 29);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 133);
            this.groupBox7.TabIndex = 33;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "場鑄環片計算";
            // 
            // btn_SiteExcel
            // 
            this.btn_SiteExcel.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_SiteExcel.ForeColor = System.Drawing.Color.Black;
            this.btn_SiteExcel.Location = new System.Drawing.Point(39, 35);
            this.btn_SiteExcel.Name = "btn_SiteExcel";
            this.btn_SiteExcel.Size = new System.Drawing.Size(109, 29);
            this.btn_SiteExcel.TabIndex = 31;
            this.btn_SiteExcel.Text = "輸出Excel";
            this.btn_SiteExcel.UseVisualStyleBackColor = true;
            this.btn_SiteExcel.Click += new System.EventHandler(this.btn_SiteExcel_Click);
            // 
            // btn_SiteCal
            // 
            this.btn_SiteCal.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_SiteCal.ForeColor = System.Drawing.Color.Black;
            this.btn_SiteCal.Location = new System.Drawing.Point(39, 82);
            this.btn_SiteCal.Name = "btn_SiteCal";
            this.btn_SiteCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_SiteCal.Size = new System.Drawing.Size(109, 29);
            this.btn_SiteCal.TabIndex = 19;
            this.btn_SiteCal.Text = "執行分析";
            this.btn_SiteCal.UseVisualStyleBackColor = true;
            this.btn_SiteCal.Click += new System.EventHandler(this.btn_SiteCal_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lbl_SiteFy);
            this.groupBox6.Controls.Add(this.lbl_SiteFc);
            this.groupBox6.Controls.Add(this.lbl_SiteU12);
            this.groupBox6.Controls.Add(this.lbl_SiteE);
            this.groupBox6.Controls.Add(this.lbl_SiteUW);
            this.groupBox6.Controls.Add(this.lbl_SiteGroutAngle);
            this.groupBox6.Controls.Add(this.lbl_SitePoreAngle);
            this.groupBox6.Controls.Add(this.lbl_SiteKangle);
            this.groupBox6.Controls.Add(this.lbl_SiteBAngle);
            this.groupBox6.Controls.Add(this.lbl_SiteAAngle);
            this.groupBox6.Controls.Add(this.lbl_SiteThick);
            this.groupBox6.Controls.Add(this.lbl_SiteRadiusIn);
            this.groupBox6.Controls.Add(this.label114);
            this.groupBox6.Controls.Add(this.label115);
            this.groupBox6.Controls.Add(this.label116);
            this.groupBox6.Controls.Add(this.label120);
            this.groupBox6.Controls.Add(this.label128);
            this.groupBox6.Controls.Add(this.label129);
            this.groupBox6.Controls.Add(this.label132);
            this.groupBox6.Controls.Add(this.label134);
            this.groupBox6.Controls.Add(this.label135);
            this.groupBox6.Controls.Add(this.label136);
            this.groupBox6.Controls.Add(this.label137);
            this.groupBox6.Controls.Add(this.label138);
            this.groupBox6.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox6.ForeColor = System.Drawing.Color.Black;
            this.groupBox6.Location = new System.Drawing.Point(6, 29);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(326, 495);
            this.groupBox6.TabIndex = 32;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "環片參數";
            // 
            // lbl_SiteFy
            // 
            this.lbl_SiteFy.AutoSize = true;
            this.lbl_SiteFy.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteFy.Location = new System.Drawing.Point(172, 452);
            this.lbl_SiteFy.Name = "lbl_SiteFy";
            this.lbl_SiteFy.Size = new System.Drawing.Size(28, 23);
            this.lbl_SiteFy.TabIndex = 46;
            this.lbl_SiteFy.Text = "Fy";
            this.lbl_SiteFy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteFc
            // 
            this.lbl_SiteFc.AutoSize = true;
            this.lbl_SiteFc.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteFc.Location = new System.Drawing.Point(172, 414);
            this.lbl_SiteFc.Name = "lbl_SiteFc";
            this.lbl_SiteFc.Size = new System.Drawing.Size(28, 23);
            this.lbl_SiteFc.TabIndex = 46;
            this.lbl_SiteFc.Text = "Fc";
            this.lbl_SiteFc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteU12
            // 
            this.lbl_SiteU12.AutoSize = true;
            this.lbl_SiteU12.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteU12.Location = new System.Drawing.Point(172, 377);
            this.lbl_SiteU12.Name = "lbl_SiteU12";
            this.lbl_SiteU12.Size = new System.Drawing.Size(87, 23);
            this.lbl_SiteU12.TabIndex = 46;
            this.lbl_SiteU12.Text = "PoissonR";
            this.lbl_SiteU12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteE
            // 
            this.lbl_SiteE.AutoSize = true;
            this.lbl_SiteE.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteE.Location = new System.Drawing.Point(172, 344);
            this.lbl_SiteE.Name = "lbl_SiteE";
            this.lbl_SiteE.Size = new System.Drawing.Size(20, 23);
            this.lbl_SiteE.TabIndex = 46;
            this.lbl_SiteE.Text = "E";
            this.lbl_SiteE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteUW
            // 
            this.lbl_SiteUW.AutoSize = true;
            this.lbl_SiteUW.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteUW.Location = new System.Drawing.Point(172, 308);
            this.lbl_SiteUW.Name = "lbl_SiteUW";
            this.lbl_SiteUW.Size = new System.Drawing.Size(41, 23);
            this.lbl_SiteUW.TabIndex = 46;
            this.lbl_SiteUW.Text = "UW";
            this.lbl_SiteUW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteGroutAngle
            // 
            this.lbl_SiteGroutAngle.AutoSize = true;
            this.lbl_SiteGroutAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteGroutAngle.Location = new System.Drawing.Point(172, 268);
            this.lbl_SiteGroutAngle.Name = "lbl_SiteGroutAngle";
            this.lbl_SiteGroutAngle.Size = new System.Drawing.Size(59, 23);
            this.lbl_SiteGroutAngle.TabIndex = 46;
            this.lbl_SiteGroutAngle.Text = "Grout";
            this.lbl_SiteGroutAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SitePoreAngle
            // 
            this.lbl_SitePoreAngle.AutoSize = true;
            this.lbl_SitePoreAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SitePoreAngle.Location = new System.Drawing.Point(172, 227);
            this.lbl_SitePoreAngle.Name = "lbl_SitePoreAngle";
            this.lbl_SitePoreAngle.Size = new System.Drawing.Size(49, 23);
            this.lbl_SitePoreAngle.TabIndex = 46;
            this.lbl_SitePoreAngle.Text = "Pore";
            this.lbl_SitePoreAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteKangle
            // 
            this.lbl_SiteKangle.AutoSize = true;
            this.lbl_SiteKangle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteKangle.Location = new System.Drawing.Point(172, 187);
            this.lbl_SiteKangle.Name = "lbl_SiteKangle";
            this.lbl_SiteKangle.Size = new System.Drawing.Size(70, 23);
            this.lbl_SiteKangle.TabIndex = 46;
            this.lbl_SiteKangle.Text = "KAngle";
            this.lbl_SiteKangle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteBAngle
            // 
            this.lbl_SiteBAngle.AutoSize = true;
            this.lbl_SiteBAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteBAngle.Location = new System.Drawing.Point(172, 147);
            this.lbl_SiteBAngle.Name = "lbl_SiteBAngle";
            this.lbl_SiteBAngle.Size = new System.Drawing.Size(70, 23);
            this.lbl_SiteBAngle.TabIndex = 46;
            this.lbl_SiteBAngle.Text = "BAngle";
            this.lbl_SiteBAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteAAngle
            // 
            this.lbl_SiteAAngle.AutoSize = true;
            this.lbl_SiteAAngle.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteAAngle.Location = new System.Drawing.Point(172, 108);
            this.lbl_SiteAAngle.Name = "lbl_SiteAAngle";
            this.lbl_SiteAAngle.Size = new System.Drawing.Size(71, 23);
            this.lbl_SiteAAngle.TabIndex = 46;
            this.lbl_SiteAAngle.Text = "AAngle";
            this.lbl_SiteAAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteThick
            // 
            this.lbl_SiteThick.AutoSize = true;
            this.lbl_SiteThick.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteThick.Location = new System.Drawing.Point(172, 71);
            this.lbl_SiteThick.Name = "lbl_SiteThick";
            this.lbl_SiteThick.Size = new System.Drawing.Size(52, 23);
            this.lbl_SiteThick.TabIndex = 46;
            this.lbl_SiteThick.Text = "thick";
            this.lbl_SiteThick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SiteRadiusIn
            // 
            this.lbl_SiteRadiusIn.AutoSize = true;
            this.lbl_SiteRadiusIn.Font = new System.Drawing.Font("微軟正黑體", 13F);
            this.lbl_SiteRadiusIn.Location = new System.Drawing.Point(172, 35);
            this.lbl_SiteRadiusIn.Name = "lbl_SiteRadiusIn";
            this.lbl_SiteRadiusIn.Size = new System.Drawing.Size(62, 23);
            this.lbl_SiteRadiusIn.TabIndex = 46;
            this.lbl_SiteRadiusIn.Text = "radius";
            this.lbl_SiteRadiusIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label114
            // 
            this.label114.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label114.Location = new System.Drawing.Point(5, 35);
            this.label114.Name = "label114";
            this.label114.Size = new System.Drawing.Size(172, 25);
            this.label114.TabIndex = 46;
            this.label114.Text = "環片內半徑：";
            this.label114.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label115
            // 
            this.label115.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label115.Location = new System.Drawing.Point(5, 71);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(172, 25);
            this.label115.TabIndex = 54;
            this.label115.Text = "環片厚度：";
            this.label115.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label116
            // 
            this.label116.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label116.Location = new System.Drawing.Point(5, 452);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(172, 25);
            this.label116.TabIndex = 50;
            this.label116.Text = "鋼筋降伏強度：";
            this.label116.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label120
            // 
            this.label120.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label120.Location = new System.Drawing.Point(5, 414);
            this.label120.Name = "label120";
            this.label120.Size = new System.Drawing.Size(172, 25);
            this.label120.TabIndex = 50;
            this.label120.Text = "混凝土強度：";
            this.label120.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label128
            // 
            this.label128.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label128.Location = new System.Drawing.Point(4, 377);
            this.label128.Name = "label128";
            this.label128.Size = new System.Drawing.Size(172, 25);
            this.label128.TabIndex = 50;
            this.label128.Text = "柏松比：";
            this.label128.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label129
            // 
            this.label129.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label129.Location = new System.Drawing.Point(5, 344);
            this.label129.Name = "label129";
            this.label129.Size = new System.Drawing.Size(172, 25);
            this.label129.TabIndex = 50;
            this.label129.Text = "楊式模數：";
            this.label129.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label132
            // 
            this.label132.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label132.Location = new System.Drawing.Point(5, 308);
            this.label132.Name = "label132";
            this.label132.Size = new System.Drawing.Size(172, 25);
            this.label132.TabIndex = 50;
            this.label132.Text = "混凝土單位重：";
            this.label132.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label134
            // 
            this.label134.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label134.Location = new System.Drawing.Point(5, 268);
            this.label134.Name = "label134";
            this.label134.Size = new System.Drawing.Size(172, 25);
            this.label134.TabIndex = 47;
            this.label134.Text = "相鄰灌漿孔角度：";
            this.label134.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label135
            // 
            this.label135.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label135.Location = new System.Drawing.Point(5, 108);
            this.label135.Name = "label135";
            this.label135.Size = new System.Drawing.Size(172, 25);
            this.label135.TabIndex = 52;
            this.label135.Text = "A環片角度：";
            this.label135.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label136
            // 
            this.label136.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label136.Location = new System.Drawing.Point(5, 227);
            this.label136.Name = "label136";
            this.label136.Size = new System.Drawing.Size(172, 25);
            this.label136.TabIndex = 48;
            this.label136.Text = "相鄰螺栓孔角度：";
            this.label136.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label137
            // 
            this.label137.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label137.Location = new System.Drawing.Point(5, 147);
            this.label137.Name = "label137";
            this.label137.Size = new System.Drawing.Size(172, 25);
            this.label137.TabIndex = 51;
            this.label137.Text = "B環片角度：";
            this.label137.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label138
            // 
            this.label138.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold);
            this.label138.Location = new System.Drawing.Point(5, 187);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(172, 25);
            this.label138.TabIndex = 49;
            this.label138.Text = "K環片角度：";
            this.label138.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabContDepth
            // 
            this.tabContDepth.Controls.Add(this.FigPanel);
            this.tabContDepth.Location = new System.Drawing.Point(4, 25);
            this.tabContDepth.Name = "tabContDepth";
            this.tabContDepth.Size = new System.Drawing.Size(1062, 546);
            this.tabContDepth.TabIndex = 6;
            this.tabContDepth.Text = "接觸深度分析";
            this.tabContDepth.UseVisualStyleBackColor = true;
            // 
            // FigPanel
            // 
            this.FigPanel.Controls.Add(this.Fig04);
            this.FigPanel.Controls.Add(this.Fig02);
            this.FigPanel.Controls.Add(this.btn_FinalCal);
            this.FigPanel.Controls.Add(this.Fig_ComboBox);
            this.FigPanel.Controls.Add(this.Fig12_Depth);
            this.FigPanel.Controls.Add(this.Fig08_Depth);
            this.FigPanel.Controls.Add(this.Fig04_Depth);
            this.FigPanel.Controls.Add(this.Fig11_Depth);
            this.FigPanel.Controls.Add(this.Fig07_Depth);
            this.FigPanel.Controls.Add(this.Fig03_Depth);
            this.FigPanel.Controls.Add(this.Fig10_Depth);
            this.FigPanel.Controls.Add(this.Fig06_Depth);
            this.FigPanel.Controls.Add(this.Fig02_Depth);
            this.FigPanel.Controls.Add(this.Fig09_Depth);
            this.FigPanel.Controls.Add(this.Fig05_Depth);
            this.FigPanel.Controls.Add(this.Fig01_Depth);
            this.FigPanel.Controls.Add(this.label46);
            this.FigPanel.Controls.Add(this.label42);
            this.FigPanel.Controls.Add(this.label38);
            this.FigPanel.Controls.Add(this.label45);
            this.FigPanel.Controls.Add(this.label41);
            this.FigPanel.Controls.Add(this.label37);
            this.FigPanel.Controls.Add(this.label44);
            this.FigPanel.Controls.Add(this.label40);
            this.FigPanel.Controls.Add(this.label36);
            this.FigPanel.Controls.Add(this.label43);
            this.FigPanel.Controls.Add(this.label39);
            this.FigPanel.Controls.Add(this.label35);
            this.FigPanel.Controls.Add(this.Fig12);
            this.FigPanel.Controls.Add(this.Fig08);
            this.FigPanel.Controls.Add(this.Fig11);
            this.FigPanel.Controls.Add(this.Fig07);
            this.FigPanel.Controls.Add(this.Fig03);
            this.FigPanel.Controls.Add(this.Fig10);
            this.FigPanel.Controls.Add(this.Fig06);
            this.FigPanel.Controls.Add(this.Fig09);
            this.FigPanel.Controls.Add(this.Fig05);
            this.FigPanel.Controls.Add(this.Fig01);
            this.FigPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FigPanel.Location = new System.Drawing.Point(0, 0);
            this.FigPanel.Name = "FigPanel";
            this.FigPanel.Size = new System.Drawing.Size(1062, 546);
            this.FigPanel.TabIndex = 25;
            // 
            // Fig04
            // 
            this.Fig04.Location = new System.Drawing.Point(775, 44);
            this.Fig04.Name = "Fig04";
            this.Fig04.Size = new System.Drawing.Size(246, 138);
            this.Fig04.TabIndex = 0;
            this.Fig04.Text = "Fig01";
            // 
            // Fig02
            // 
            this.Fig02.Location = new System.Drawing.Point(271, 44);
            this.Fig02.Name = "Fig02";
            this.Fig02.Size = new System.Drawing.Size(246, 138);
            this.Fig02.TabIndex = 0;
            this.Fig02.Text = "Fig01";
            // 
            // btn_FinalCal
            // 
            this.btn_FinalCal.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_FinalCal.Location = new System.Drawing.Point(193, 7);
            this.btn_FinalCal.Name = "btn_FinalCal";
            this.btn_FinalCal.Size = new System.Drawing.Size(101, 31);
            this.btn_FinalCal.TabIndex = 5;
            this.btn_FinalCal.Text = "最終計算";
            this.btn_FinalCal.UseVisualStyleBackColor = true;
            this.btn_FinalCal.Click += new System.EventHandler(this.btn_FinalCal_Click);
            // 
            // Fig_ComboBox
            // 
            this.Fig_ComboBox.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.Fig_ComboBox.FormattingEnabled = true;
            this.Fig_ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Fig_ComboBox.Location = new System.Drawing.Point(35, 7);
            this.Fig_ComboBox.Name = "Fig_ComboBox";
            this.Fig_ComboBox.Size = new System.Drawing.Size(138, 28);
            this.Fig_ComboBox.TabIndex = 3;
            this.Fig_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Fig_ComboBox_SelectedIndexChanged);
            // 
            // Fig12_Depth
            // 
            this.Fig12_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig12_Depth.Location = new System.Drawing.Point(950, 519);
            this.Fig12_Depth.Name = "Fig12_Depth";
            this.Fig12_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig12_Depth.TabIndex = 2;
            // 
            // Fig08_Depth
            // 
            this.Fig08_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig08_Depth.Location = new System.Drawing.Point(950, 349);
            this.Fig08_Depth.Name = "Fig08_Depth";
            this.Fig08_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig08_Depth.TabIndex = 2;
            // 
            // Fig04_Depth
            // 
            this.Fig04_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig04_Depth.Location = new System.Drawing.Point(950, 182);
            this.Fig04_Depth.Name = "Fig04_Depth";
            this.Fig04_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig04_Depth.TabIndex = 2;
            // 
            // Fig11_Depth
            // 
            this.Fig11_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig11_Depth.Location = new System.Drawing.Point(699, 519);
            this.Fig11_Depth.Name = "Fig11_Depth";
            this.Fig11_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig11_Depth.TabIndex = 2;
            // 
            // Fig07_Depth
            // 
            this.Fig07_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig07_Depth.Location = new System.Drawing.Point(699, 349);
            this.Fig07_Depth.Name = "Fig07_Depth";
            this.Fig07_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig07_Depth.TabIndex = 2;
            // 
            // Fig03_Depth
            // 
            this.Fig03_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig03_Depth.Location = new System.Drawing.Point(699, 180);
            this.Fig03_Depth.Name = "Fig03_Depth";
            this.Fig03_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig03_Depth.TabIndex = 2;
            // 
            // Fig10_Depth
            // 
            this.Fig10_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig10_Depth.Location = new System.Drawing.Point(428, 521);
            this.Fig10_Depth.Name = "Fig10_Depth";
            this.Fig10_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig10_Depth.TabIndex = 2;
            // 
            // Fig06_Depth
            // 
            this.Fig06_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig06_Depth.Location = new System.Drawing.Point(428, 351);
            this.Fig06_Depth.Name = "Fig06_Depth";
            this.Fig06_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig06_Depth.TabIndex = 2;
            // 
            // Fig02_Depth
            // 
            this.Fig02_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig02_Depth.Location = new System.Drawing.Point(428, 182);
            this.Fig02_Depth.Name = "Fig02_Depth";
            this.Fig02_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig02_Depth.TabIndex = 2;
            // 
            // Fig09_Depth
            // 
            this.Fig09_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig09_Depth.Location = new System.Drawing.Point(193, 520);
            this.Fig09_Depth.Name = "Fig09_Depth";
            this.Fig09_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig09_Depth.TabIndex = 2;
            // 
            // Fig05_Depth
            // 
            this.Fig05_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig05_Depth.Location = new System.Drawing.Point(193, 350);
            this.Fig05_Depth.Name = "Fig05_Depth";
            this.Fig05_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig05_Depth.TabIndex = 2;
            // 
            // Fig01_Depth
            // 
            this.Fig01_Depth.Font = new System.Drawing.Font("Calibri", 10F);
            this.Fig01_Depth.Location = new System.Drawing.Point(193, 181);
            this.Fig01_Depth.Name = "Fig01_Depth";
            this.Fig01_Depth.Size = new System.Drawing.Size(45, 24);
            this.Fig01_Depth.TabIndex = 2;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label46.Location = new System.Drawing.Point(834, 520);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(121, 20);
            this.label46.TabIndex = 1;
            this.label46.Text = "選擇接觸深度：";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label42.Location = new System.Drawing.Point(834, 350);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(121, 20);
            this.label42.TabIndex = 1;
            this.label42.Text = "選擇接觸深度：";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label38.Location = new System.Drawing.Point(834, 183);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(121, 20);
            this.label38.TabIndex = 1;
            this.label38.Text = "選擇接觸深度：";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label45.Location = new System.Drawing.Point(583, 520);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(121, 20);
            this.label45.TabIndex = 1;
            this.label45.Text = "選擇接觸深度：";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label41.Location = new System.Drawing.Point(583, 350);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(121, 20);
            this.label41.TabIndex = 1;
            this.label41.Text = "選擇接觸深度：";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label37.Location = new System.Drawing.Point(583, 181);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(121, 20);
            this.label37.TabIndex = 1;
            this.label37.Text = "選擇接觸深度：";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label44.Location = new System.Drawing.Point(312, 522);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(121, 20);
            this.label44.TabIndex = 1;
            this.label44.Text = "選擇接觸深度：";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label40.Location = new System.Drawing.Point(312, 352);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(121, 20);
            this.label40.TabIndex = 1;
            this.label40.Text = "選擇接觸深度：";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label36.Location = new System.Drawing.Point(312, 183);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(121, 20);
            this.label36.TabIndex = 1;
            this.label36.Text = "選擇接觸深度：";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label43.Location = new System.Drawing.Point(77, 521);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(121, 20);
            this.label43.TabIndex = 1;
            this.label43.Text = "選擇接觸深度：";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label39.Location = new System.Drawing.Point(77, 351);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(121, 20);
            this.label39.TabIndex = 1;
            this.label39.Text = "選擇接觸深度：";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label35.Location = new System.Drawing.Point(77, 182);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(121, 20);
            this.label35.TabIndex = 1;
            this.label35.Text = "選擇接觸深度：";
            // 
            // Fig12
            // 
            this.Fig12.Location = new System.Drawing.Point(775, 381);
            this.Fig12.Name = "Fig12";
            this.Fig12.Size = new System.Drawing.Size(246, 138);
            this.Fig12.TabIndex = 0;
            this.Fig12.Text = "Fig01";
            // 
            // Fig08
            // 
            this.Fig08.Location = new System.Drawing.Point(775, 210);
            this.Fig08.Name = "Fig08";
            this.Fig08.Size = new System.Drawing.Size(246, 138);
            this.Fig08.TabIndex = 0;
            this.Fig08.Text = "Fig01";
            // 
            // Fig11
            // 
            this.Fig11.Location = new System.Drawing.Point(523, 381);
            this.Fig11.Name = "Fig11";
            this.Fig11.Size = new System.Drawing.Size(246, 138);
            this.Fig11.TabIndex = 0;
            this.Fig11.Text = "Fig01";
            // 
            // Fig07
            // 
            this.Fig07.Location = new System.Drawing.Point(523, 210);
            this.Fig07.Name = "Fig07";
            this.Fig07.Size = new System.Drawing.Size(246, 138);
            this.Fig07.TabIndex = 0;
            this.Fig07.Text = "Fig01";
            // 
            // Fig03
            // 
            this.Fig03.Location = new System.Drawing.Point(523, 44);
            this.Fig03.Name = "Fig03";
            this.Fig03.Size = new System.Drawing.Size(246, 138);
            this.Fig03.TabIndex = 0;
            this.Fig03.Text = "Fig01";
            // 
            // Fig10
            // 
            this.Fig10.Location = new System.Drawing.Point(271, 381);
            this.Fig10.Name = "Fig10";
            this.Fig10.Size = new System.Drawing.Size(246, 138);
            this.Fig10.TabIndex = 0;
            this.Fig10.Text = "Fig01";
            // 
            // Fig06
            // 
            this.Fig06.Location = new System.Drawing.Point(271, 210);
            this.Fig06.Name = "Fig06";
            this.Fig06.Size = new System.Drawing.Size(246, 138);
            this.Fig06.TabIndex = 0;
            this.Fig06.Text = "Fig01";
            // 
            // Fig09
            // 
            this.Fig09.Location = new System.Drawing.Point(19, 381);
            this.Fig09.Name = "Fig09";
            this.Fig09.Size = new System.Drawing.Size(246, 138);
            this.Fig09.TabIndex = 0;
            this.Fig09.Text = "Fig01";
            // 
            // Fig05
            // 
            this.Fig05.Location = new System.Drawing.Point(19, 210);
            this.Fig05.Name = "Fig05";
            this.Fig05.Size = new System.Drawing.Size(246, 138);
            this.Fig05.TabIndex = 0;
            this.Fig05.Text = "Fig01";
            // 
            // Fig01
            // 
            this.Fig01.Location = new System.Drawing.Point(19, 44);
            this.Fig01.Name = "Fig01";
            this.Fig01.Size = new System.Drawing.Size(246, 138);
            this.Fig01.TabIndex = 0;
            this.Fig01.Text = "Fig01";
            // 
            // tabOthers
            // 
            this.tabOthers.Controls.Add(this.tabControl1);
            this.tabOthers.Location = new System.Drawing.Point(4, 25);
            this.tabOthers.Name = "tabOthers";
            this.tabOthers.Padding = new System.Windows.Forms.Padding(3);
            this.tabOthers.Size = new System.Drawing.Size(1062, 546);
            this.tabOthers.TabIndex = 0;
            this.tabOthers.Text = "其他分析";
            this.tabOthers.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.結束ToolStripMenuItem,
            this.選擇標案ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1070, 24);
            this.menuStrip1.TabIndex = 30;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 結束ToolStripMenuItem
            // 
            this.結束ToolStripMenuItem.Name = "結束ToolStripMenuItem";
            this.結束ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.結束ToolStripMenuItem.Text = "結束";
            this.結束ToolStripMenuItem.Click += new System.EventHandler(this.結束ToolStripMenuItem_Click);
            // 
            // 選擇標案ToolStripMenuItem
            // 
            this.選擇標案ToolStripMenuItem.Name = "選擇標案ToolStripMenuItem";
            this.選擇標案ToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.選擇標案ToolStripMenuItem.Text = "標案選單";
            this.選擇標案ToolStripMenuItem.Click += new System.EventHandler(this.選擇標案ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "SteelTunnelX.png");
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 599);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SinoTunnel";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.輸入參數.ResumeLayout(false);
            this.輸入參數.PerformLayout();
            this.吊放等計算.ResumeLayout(false);
            this.吊放等計算.PerformLayout();
            this.土壤E值.ResumeLayout(false);
            this.土壤E值.PerformLayout();
            this.環片應變.ResumeLayout(false);
            this.環片應變.PerformLayout();
            this.環片分析.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.btn_ExcelOutput.ResumeLayout(false);
            this.btn_ExcelOutput.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.參考用.ResumeLayout(false);
            this.網頁.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabSection.ResumeLayout(false);
            this.tabSection.PerformLayout();
            this.tabTunnel.ResumeLayout(false);
            this.gBox_LoadTerm.ResumeLayout(false);
            this.gBox_VerInc.ResumeLayout(false);
            this.gBox_VerInc.PerformLayout();
            this.gBox_LatDec.ResumeLayout(false);
            this.gBox_LatDec.PerformLayout();
            this.gBox_Grout.ResumeLayout(false);
            this.gBox_Grout.PerformLayout();
            this.gBox_SGProp.ResumeLayout(false);
            this.gBox_SGProp.PerformLayout();
            this.tabConnect.ResumeLayout(false);
            this.gpConnectTunnel.ResumeLayout(false);
            this.gBox_ConnectorCal.ResumeLayout(false);
            this.gBOX_ConnectorProp.ResumeLayout(false);
            this.gBOX_ConnectorProp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabSteelTunnel.ResumeLayout(false);
            this.gpSteelTunnel.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabSiteTunnel.ResumeLayout(false);
            this.gpSiteTunnel.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabContDepth.ResumeLayout(false);
            this.FigPanel.ResumeLayout(false);
            this.FigPanel.PerformLayout();
            this.tabOthers.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label VarDiaDecreasedXP;
        private System.Windows.Forms.TextBox TransportationMText;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox StackingMText;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox HangingMText;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btn_SGSelf;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox TransportationVText;
        private System.Windows.Forms.TextBox StackingVText;
        private System.Windows.Forms.TextBox HangingVText;
        private System.Windows.Forms.TextBox Cal03_output;
        private System.Windows.Forms.Button STN_StrainCheck_Internal;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox ODEVCStrainText;
        private System.Windows.Forms.TextBox MDEVCStrainText;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button GroutingInternalCal;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox LongtermE1_In;
        private System.Windows.Forms.Button btn_VerticalStressInternal;
        private System.Windows.Forms.TextBox GroutingM;
        private System.Windows.Forms.TextBox GroutingP;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button VSExternal;
        private System.Windows.Forms.TextBox LongTermE1_Ex;
        private System.Windows.Forms.TextBox ShortermE1_In;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox ShorTermE1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Button LoadingtermInternalCal;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox LongTermInitialCheckBox;
        private System.Windows.Forms.CheckBox ShortTermInitialCheckBox;
        private System.Windows.Forms.CheckBox VarDiaInitialCheckBox;
        private System.Windows.Forms.CheckBox SeismicCheckBox;
        private System.Windows.Forms.Label EQDiaIncreasedZP;
        private System.Windows.Forms.TextBox VarDiaXPInput;
        private System.Windows.Forms.TextBox EQDiaInput;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 輸入參數;
        private System.Windows.Forms.TabPage 吊放等計算;
        private System.Windows.Forms.TabPage 土壤E值;
        private System.Windows.Forms.TabPage 環片應變;
        private System.Windows.Forms.TabPage 環片分析;
        private System.Windows.Forms.TabPage 參考用;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox ODEVTStrainText;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox MDEVTStrainText;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox ODETTStrainText;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox ODETCStrainText;
        private System.Windows.Forms.TextBox MDETTStrainText;
        private System.Windows.Forms.TextBox MDETCStrainText;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage btn_ExcelOutput;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 結束ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 選擇標案ToolStripMenuItem;
        private System.Windows.Forms.TabPage 網頁;
        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button STN_SegmentSelfInternal;
        private System.Windows.Forms.TabPage 穩定性檢核;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox EQDiaInitial;
        private System.Windows.Forms.TextBox VarDiaXPInitial;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label ChosenProject;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.ComboBox cbo_LoadingCal;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabOthers;
        private System.Windows.Forms.TabPage tabConnect;
        private System.Windows.Forms.GroupBox gpConnectTunnel;
        private System.Windows.Forms.Button btn_ConnectorCal;
        private System.Windows.Forms.TabPage tabSteelTunnel;
        private System.Windows.Forms.TabPage tabSiteTunnel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox RadiusIn;
        private System.Windows.Forms.TextBox Thickness;
        private System.Windows.Forms.TextBox Angle;
        private System.Windows.Forms.TextBox AAngle;
        private System.Windows.Forms.TextBox BAngle;
        private System.Windows.Forms.TextBox KAngle;
        private System.Windows.Forms.TextBox AdjacentPoreAngle;
        private System.Windows.Forms.TextBox AdjacentGroutAngle;
        private System.Windows.Forms.TextBox UnitWeight;
        private System.Windows.Forms.Button btn_LoadingOutExcel;
        private System.Windows.Forms.GroupBox gpSiteTunnel;
        private System.Windows.Forms.Button btn_SiteCal;
        private System.Windows.Forms.Button btn_ConnectorExcel;
        private System.Windows.Forms.Button btn_SiteExcel;
        private System.Windows.Forms.Button btn_GroutExcel1;
        private System.Windows.Forms.Label label_SectionFunction;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.CheckBox checkB_Precast;
        private System.Windows.Forms.Button btn_InputFrameMaterial;
        private System.Windows.Forms.CheckBox checkB_Connector_Steel;
        private System.Windows.Forms.CheckBox checkB_Site;
        private System.Windows.Forms.Label lbl_User1;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.TabPage tabSection;
        private System.Windows.Forms.TabPage tabTunnel;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.TabPage tabContDepth;
        private System.Windows.Forms.ComboBox cbo_Section;
        private System.Windows.Forms.Label lbl_ChosenProject;
        private System.Windows.Forms.Label lbl_User;
        private System.Windows.Forms.Label lbl_SectionFunction;
        private System.Windows.Forms.GroupBox gBox_SGProp;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label lbl_SGFy;
        private System.Windows.Forms.Label lbl_SGFc;
        private System.Windows.Forms.Label lbl_SGE;
        private System.Windows.Forms.Label lbl_SGUW;
        private System.Windows.Forms.Label lbl_SGGroutAngle;
        private System.Windows.Forms.Label lbl_SGPoreAngle;
        private System.Windows.Forms.Label lbl_SGKAngle;
        private System.Windows.Forms.Label lbl_SGBAngle;
        private System.Windows.Forms.Label lbl_SGAAngle;
        private System.Windows.Forms.Label lbl_SGAngle;
        private System.Windows.Forms.Label lbl_SGThick;
        private System.Windows.Forms.Label lbl_SGRadiusIn;
        private System.Windows.Forms.Label lbl_SGPoissonR;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox gBox_Grout;
        private System.Windows.Forms.Button btn_GroutExcel;
        private System.Windows.Forms.Button btn_GroutSAP;
        private System.Windows.Forms.GroupBox gBox_LoadTerm;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.Label lbl_GroutP;
        private System.Windows.Forms.ComboBox cbo_LoadingTerm;
        private System.Windows.Forms.Button btn_LoadTermSAP;
        private System.Windows.Forms.Button btn_LoadTermExcel;
        private System.Windows.Forms.GroupBox gBox_VerInc;
        private System.Windows.Forms.GroupBox gBox_LatDec;
        private System.Windows.Forms.TextBox txt_LatDecRepeat;
        private System.Windows.Forms.TextBox txt_LatDecInitial;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label lbl_LatDecValue;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.TextBox txt_VerIncRepeat;
        private System.Windows.Forms.TextBox txt_VerIncInitial;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.Label lbl_VerIncValue;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.GroupBox gBOX_ConnectorProp;
        private System.Windows.Forms.Label lbl_ConnectorFy;
        private System.Windows.Forms.Label lbl_ConnectorFc;
        private System.Windows.Forms.Label lbl_ConnectorPR;
        private System.Windows.Forms.Label lbl_ConnectorE;
        private System.Windows.Forms.Label lbl_TR;
        private System.Windows.Forms.Label lbl_BH;
        private System.Windows.Forms.Label lbl_ConnectorUW;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.Label label109;
        private System.Windows.Forms.Label label110;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.GroupBox gBox_ConnectorCal;
        private System.Windows.Forms.Panel FigPanel;
        private LiveCharts.WinForms.CartesianChart Fig04;
        private LiveCharts.WinForms.CartesianChart Fig02;
        private System.Windows.Forms.Button btn_FinalCal;
        private System.Windows.Forms.ComboBox Fig_ComboBox;
        private System.Windows.Forms.TextBox Fig12_Depth;
        private System.Windows.Forms.TextBox Fig08_Depth;
        private System.Windows.Forms.TextBox Fig04_Depth;
        private System.Windows.Forms.TextBox Fig11_Depth;
        private System.Windows.Forms.TextBox Fig07_Depth;
        private System.Windows.Forms.TextBox Fig03_Depth;
        private System.Windows.Forms.TextBox Fig10_Depth;
        private System.Windows.Forms.TextBox Fig06_Depth;
        private System.Windows.Forms.TextBox Fig02_Depth;
        private System.Windows.Forms.TextBox Fig09_Depth;
        private System.Windows.Forms.TextBox Fig05_Depth;
        private System.Windows.Forms.TextBox Fig01_Depth;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label35;
        private LiveCharts.WinForms.CartesianChart Fig12;
        private LiveCharts.WinForms.CartesianChart Fig08;
        private LiveCharts.WinForms.CartesianChart Fig11;
        private LiveCharts.WinForms.CartesianChart Fig07;
        private LiveCharts.WinForms.CartesianChart Fig03;
        private LiveCharts.WinForms.CartesianChart Fig10;
        private LiveCharts.WinForms.CartesianChart Fig06;
        private LiveCharts.WinForms.CartesianChart Fig09;
        private LiveCharts.WinForms.CartesianChart Fig05;
        private LiveCharts.WinForms.CartesianChart Fig01;
        private System.Windows.Forms.GroupBox gpSteelTunnel;
        private System.Windows.Forms.Button btn_SteelExcel;
        private System.Windows.Forms.Button btn_SteelCal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbl_SteelFu;
        private System.Windows.Forms.Label lbl_SteelFy;
        private System.Windows.Forms.Label lbl_SteelPR;
        private System.Windows.Forms.Label lbl_SteelMat;
        private System.Windows.Forms.Label lbl_SteelUW;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.Label label118;
        private System.Windows.Forms.Label label119;
        private System.Windows.Forms.Label label127;
        private System.Windows.Forms.Label label121;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_SteelConcreFy;
        private System.Windows.Forms.Label lbl_SteelConcreFc;
        private System.Windows.Forms.Label lbl_SteelConcrePR;
        private System.Windows.Forms.Label lbl_SteelConcreE;
        private System.Windows.Forms.Label lbl_SteelConcreUW;
        private System.Windows.Forms.Label label122;
        private System.Windows.Forms.Label label123;
        private System.Windows.Forms.Label label124;
        private System.Windows.Forms.Label label125;
        private System.Windows.Forms.Label label126;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lbl_SteelPoreAngle;
        private System.Windows.Forms.Label lbl_SteelKAngle;
        private System.Windows.Forms.Label lbl_SteelBAngle;
        private System.Windows.Forms.Label lbl_SteelAAngle;
        private System.Windows.Forms.Label lbl_SteelThick;
        private System.Windows.Forms.Label lbl_SteelRadiusIn;
        private System.Windows.Forms.Label label130;
        private System.Windows.Forms.Label label131;
        private System.Windows.Forms.Label label139;
        private System.Windows.Forms.Label label140;
        private System.Windows.Forms.Label label141;
        private System.Windows.Forms.Label label142;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lbl_SiteFy;
        private System.Windows.Forms.Label lbl_SiteFc;
        private System.Windows.Forms.Label lbl_SiteU12;
        private System.Windows.Forms.Label lbl_SiteE;
        private System.Windows.Forms.Label lbl_SiteUW;
        private System.Windows.Forms.Label lbl_SiteGroutAngle;
        private System.Windows.Forms.Label lbl_SitePoreAngle;
        private System.Windows.Forms.Label lbl_SiteKangle;
        private System.Windows.Forms.Label lbl_SiteBAngle;
        private System.Windows.Forms.Label lbl_SiteAAngle;
        private System.Windows.Forms.Label lbl_SiteThick;
        private System.Windows.Forms.Label lbl_SiteRadiusIn;
        private System.Windows.Forms.Label label114;
        private System.Windows.Forms.Label label115;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.Label label120;
        private System.Windows.Forms.Label label128;
        private System.Windows.Forms.Label label129;
        private System.Windows.Forms.Label label132;
        private System.Windows.Forms.Label label134;
        private System.Windows.Forms.Label label135;
        private System.Windows.Forms.Label label136;
        private System.Windows.Forms.Label label137;
        private System.Windows.Forms.Label label138;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label61;
    }
}

