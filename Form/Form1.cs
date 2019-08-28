using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SegmentCalcu;
using Xceed.Words.NET;
using System.IO;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows.Forms.DataVisualization.Charting;

namespace SinoTunnel
{
    public partial class Form1 : Form
    {
        GeneralCalculation oGeneralCal;
        ExcuteSQL dataSearch = new ExcuteSQL();
        GetWebData p;        
        SegmentCalcu.STN_SegmentSelf external_Segment; // 外部參考
        SegmentCalcu.STN_VerticalStress external_VerticalStress; //外部參考    
        SGCalculationDepth sgCalDepth;
        SAP_200yFlood sap200yFlood;
        SAP_ConnectTunnel oConnectTunnel;
        SAP_SteelTunnel oSteelTunnel;
        SAP_SiteTunnel oSiteTunnel;

        SAP_SegmentProcess sapCal;
        string sectionUID = "0bd202ef-ddcc-4cbc-b994-bb229fe3f4c2";        

        bool excelOnly = false;

        bool eqBool = true;
        string file = "";

        double decreasedXaxisP = 0;
        double increasedZaxisP = 0;
                
        public Form1()
        {
            string str = "";
            try { str = dataSearch.GetActiveSecion(appGlobal.UserName); }
            catch { dataSearch.InsertActiveSection(appGlobal.UserName, sectionUID); }
            if (str != "") sectionUID = str;

            this.p = new GetWebData(sectionUID);
            appGlobal.projectUID = p.projectUID;

            DataTable project = dataSearch.GetByUID("STN_Project", p.projectUID);
            string projectID = project.Rows[0]["ProjectID"].ToString();
            string projectName = project.Rows[0]["ProjectName"].ToString();
            appGlobal.projectTitle = projectID + projectName;

            //this.cal02_VerticalStress = new Cal02_VerticalStress(UID, "webform");

            

            InitializeComponent();
            label12.Text = "θ";
            label13.Text = "θ";
            label14.Text = "θ";
            label15.Text = "θ";
            label16.Text = "θ";
            label17.Text = "θ";

            //FigPanel.Location = new System.Drawing.Point(0, 0);
            //FigPanel.Hide();
            //ResultPanel.Location = new System.Drawing.Point(0, 0);
            //ResultPanel.Hide();

            
            
            #region 測試用
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series("First", 1000);
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series("Second", 1000);

            series1.Color = System.Drawing.Color.Blue;
            series2.Color = System.Drawing.Color.Red;

            series1.ChartType = SeriesChartType.Line;
            series2.ChartType = SeriesChartType.Line;

            series1.IsValueShownAsLabel = true;
            series2.IsValueShownAsLabel = true;

            int[,] array = new int[,] {{ 1, 8, 9, 7, 105, 11, 50, 999, 500, 1 },{ 12,15,11,18,733,5,4,3,2,500} };
            for(int index = 0; index < 10; index++)
            {
                series1.Points.AddXY(index, array[0, index]);
                series2.Points.AddXY(index, array[1, index]);
            }
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Titles.Add("Title");

            cartesianChart1.Series = new LiveCharts.SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double>{4,6,5,2,7 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {6,7,3,4,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                },
                new LineSeries
                {
                    Title = "Test",
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(0,10),
                        new ObservablePoint(1,7),
                        new ObservablePoint(2,6),
                        new ObservablePoint(3,8)
                    },
                    PointGeometrySize = 15
                },
            };

            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Month",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });

            cartesianChart1.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Sales",
                LabelFormatter = value => value.ToString("C")
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;

            //modifying the series collection will animate and update the chart
            cartesianChart1.Series.Add(new GLineSeries
            {
                Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
                LineSmoothness = 0, //straight lines, 1 really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = System.Windows.Media.Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            cartesianChart1.Series[2].Values.Add(5d);
            cartesianChart1.DataClick += CartesianChart1OnDataClick;
            #endregion  
        }

        #region 標案選擇與更新

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "潛盾隧道分析－SinoTunnel " + Application.ProductVersion;
            lbl_User.Text = $"{appGlobal.UserID} {appGlobal.UserName} {appGlobal.UserEmail}";
            cbo_LoadingTerm.Items.Add("長期載重");           
            cbo_LoadingTerm.Items.Add("短期載重");
            cbo_LoadingTerm.Items.Add("0.33%直徑變化");
            cbo_LoadingTerm.Items.Add("地震載重");
            //UpdateData();
            ShowSectionList();
        }

        public void ShowSectionList()
        {
            lbl_ChosenProject.Text = appGlobal.projectTitle;

            DataTable dt = dataSearch.GetByProject("STN_Section", appGlobal.projectUID);

            int sectionChoose = 0;
            cbo_Section.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string UIDValue = dt.Rows[i]["UID"].ToString();
                    string text = $"【{dt.Rows[i]["SectionID"]}  {dt.Rows[i]["SectionName"]}】 {dt.Rows[i]["Description"]}";
                    cbo_Section.Items.Add(new ComboItem(UIDValue, text));
                    if (sectionUID == dt.Rows[i]["UID"].ToString()) sectionChoose = i;
                }

                cbo_Section.SelectedIndex = sectionChoose;
                sectionUID = ((ComboItem)cbo_Section.SelectedItem).UID;

                UpdateData();
            }
            else
            {
                MessageBox.Show("無斷面可選擇，請於網頁平台新增斷面");
                lbl_SectionFunction.Text = "";
            }                          
        }

        private void cbo_Section_SelectionChangeCommitted(object sender, EventArgs e)
        {
            sectionUID = ((ComboItem)cbo_Section.SelectedItem).UID;
            appGlobal.sectionUID = sectionUID;
            List<LiveCharts.WinForms.CartesianChart> figList = new List<LiveCharts.WinForms.CartesianChart>
            { Fig01, Fig02, Fig03, Fig04, Fig05, Fig06, Fig07, Fig08, Fig09, Fig10, Fig11, Fig12 };
            for (int i = 0; i < figList.Count; i++)
            {
                figList[i].Series.Clear();
            }
            UpdateData();
        }

        public void UpdateData()
        {
            dataSearch.UpdateData("STN_SectionActive", "UserName", appGlobal.UserName, "Section", sectionUID);

            this.p = new GetWebData(sectionUID);
            appGlobal.projectUID = p.projectUID;

            external_Segment = new SegmentCalcu.STN_SegmentSelf(sectionUID); //外部參考            
            external_VerticalStress = new SegmentCalcu.STN_VerticalStress(sectionUID, "webform"); //外部參考

            DataTable project = dataSearch.GetByUID("STN_Project", p.projectUID);
            string projectID = project.Rows[0]["ProjectID"].ToString();
            string projectName = project.Rows[0]["ProjectName"].ToString();
            appGlobal.projectTitle = projectID + projectName;

            sgCalDepth = new SGCalculationDepth(sectionUID);
            sap200yFlood = new SAP_200yFlood(sectionUID);

            DataTable section = dataSearch.GetByUID("STN_Section", sectionUID);
            //VarDiaXPInput.Text = 0.ToString();
            //EQDiaInput.Text = 0.ToString();
            if (section.Rows[0]["VarDia_DecreasedXP"].ToString() != "")
            {
                lbl_LatDecValue.Text = section.Rows[0]["VarDia_DecreasedXP"].ToString();
                txt_LatDecInitial.Text = section.Rows[0]["VarDia_DecreasedXP"].ToString();
                txt_LatDecRepeat.Text = 0.ToString();
                //VarDiaDecreasedXP.Text = section.Rows[0]["VarDia_DecreasedXP"].ToString();
                //VarDiaXPInitial.Text = section.Rows[0]["VarDia_DecreasedXP"].ToString();
            }
            else
            {
                lbl_LatDecValue.Text = "尚未計算";
                txt_LatDecInitial.Text = 0.ToString();
                txt_LatDecRepeat.Text = 0.ToString();
                //VarDiaDecreasedXP.Text = 0.ToString();
                //VarDiaXPInitial.Text = 0.ToString();
            }

            if (section.Rows[0]["EQDia_IncreasedZP"].ToString() != "")
            {
                lbl_VerIncValue.Text = section.Rows[0]["EQDia_IncreasedZP"].ToString();
                txt_VerIncInitial.Text = section.Rows[0]["EQDia_IncreasedZP"].ToString();
                txt_VerIncRepeat.Text = 0.ToString();
                //EQDiaIncreasedZP.Text = section.Rows[0]["EQDia_IncreasedZP"].ToString();
                //EQDiaInitial.Text = section.Rows[0]["EQDia_IncreasedZP"].ToString();
            }
            else
            {
                lbl_VerIncValue.Text = "尚未計算";
                txt_VerIncInitial.Text = 0.ToString();
                txt_VerIncRepeat.Text = 0.ToString();
                //EQDiaIncreasedZP.Text = 0.ToString();
                //EQDiaInitial.Text = 0.ToString();
            }

            gBox_SGProp.Enabled = false;            
            //RadiusIn.Text = p.segmentRadiusIn.ToString();
            //Thickness.Text = p.segmentThickness.ToString();
            //Angle.Text = p.segmentAngle.ToString();
            //AAngle.Text = p.segmentAAngle.ToString();
            //BAngle.Text = p.segmentBAngle.ToString();
            //KAngle.Text = p.segmentKAngle.ToString();
            //AdjacentPoreAngle.Text = p.segmentAdjacentPoreAngle.ToString();
            //AdjacentGroutAngle.Text = p.segmentAdjGroutAngle.ToString();
            //UnitWeight.Text = p.segmentUnitWeight.ToString();

            gpConnectTunnel.Enabled = false;
            gpSteelTunnel.Enabled = false;
            gpSiteTunnel.Enabled = false;
            //GroutingInternalCal.Enabled = false;
            //btn_GroutExcel1.Enabled = false;
            gBox_Grout.Enabled = false;
            //LoadingtermInternalCal.Enabled = false;
            //btn_LoadingOutExcel.Enabled = false;
            gBox_LoadTerm.Enabled = false;
            btn_FinalCal.Enabled = false;

            checkB_Precast.Enabled = false;
            checkB_Connector_Steel.Enabled = false;
            checkB_Site.Enabled = false;
            checkB_Precast.Checked = false;
            checkB_Connector_Steel.Checked = false;
            checkB_Site.Checked = false;

            Fig_ComboBox.Items.Clear();

            switch (section.Rows[0]["FunctionType"].ToString())
            {
                case "A": //預鑄環片設計用
                    //基本參數
                    gBox_SGProp.Enabled = true;
                    lbl_SGRadiusIn.Text = p.segmentRadiusIn.ToString() + " m";
                    lbl_SGThick.Text = p.segmentThickness.ToString() + " m";
                    lbl_SGAngle.Text = p.segmentAngle.ToString() + " θ";
                    lbl_SGAAngle.Text = p.segmentAAngle.ToString() + " θ";
                    lbl_SGBAngle.Text = p.segmentBAngle.ToString() + " θ";
                    lbl_SGKAngle.Text = p.segmentKAngle.ToString() + " θ";
                    lbl_SGPoreAngle.Text = p.segmentAdjacentPoreAngle.ToString() + " θ";
                    lbl_SGGroutAngle.Text = p.segmentAdjGroutAngle.ToString() + "θ";
                    lbl_SGUW.Text = p.segmentUnitWeight.ToString() + " kN/m³";
                    lbl_SGE.Text = p.segmentYoungsModulus.ToString() + " kN/m²";
                    lbl_SGPoissonR.Text = p.segmentPoissonRatio.ToString();
                    lbl_SGFc.Text = p.segmentFc.ToString() + " kgf/cm²";
                    lbl_SGFy.Text = p.segmentFy.ToString() + " kgf/cm²";
                    lbl_GroutP.Text = p.groutPressure.ToString() + " kN/m";

                    //GroutingInternalCal.Enabled = true;
                    //btn_GroutExcel1.Enabled = true;
                    gBox_Grout.Enabled = true;
                    //LoadingtermInternalCal.Enabled = true;
                    //btn_LoadingOutExcel.Enabled = true;
                    gBox_LoadTerm.Enabled = true;

                    btn_FinalCal.Enabled = true;
                    lbl_SectionFunction.Text = "環片設計用";
                    Fig_ComboBox.Items.Add("長期載重");
                    Fig_ComboBox.Items.Add("短期載重");
                    //Fig_ComboBox.Items.Add("200年洪水位");
                    Fig_ComboBox.Items.Add("0.33%直徑變化");
                    Fig_ComboBox.Items.Add("地震載重");
                    checkB_Precast.Enabled = true;
                    break;
                case "B": //聯絡通道與鋼環片用
                    //聯絡通道基本參數                    
                    lbl_TR.Text = p.connector.TR.ToString() + " m";
                    lbl_BH.Text = p.connector.BH.ToString() + " m";
                    lbl_ConnectorUW.Text = p.connector.UnitWeight + " kN/m³";
                    lbl_ConnectorE.Text = p.connector.E + " kN/m²";
                    lbl_ConnectorPR.Text = p.connector.PoissonRatio.ToString();
                    lbl_ConnectorFc.Text = p.connector.Fc + " kgf/cm²";
                    lbl_ConnectorFy.Text = p.connector.Fy + " kgf/cm²";
                    //鋼環片基本參數
                    lbl_SteelRadiusIn.Text = p.segmentRadiusIn.ToString() + " m";
                    lbl_SteelThick.Text = p.segmentThickness.ToString() + " m";
                    lbl_SteelAAngle.Text = p.segmentAAngle.ToString() + " θ";
                    lbl_SteelBAngle.Text = p.segmentBAngle.ToString() + " θ";
                    lbl_SteelKAngle.Text = p.segmentKAngle.ToString() + " θ";
                    lbl_SteelPoreAngle.Text = p.segmentAdjacentPoreAngle.ToString() + " θ";
                    lbl_SteelConcreUW.Text = p.segmentUnitWeight + " kN/m³";
                    lbl_SteelConcreE.Text = p.segmentYoungsModulus + " kN/m²";
                    lbl_SteelConcrePR.Text = p.segmentPoissonRatio.ToString();
                    lbl_SteelConcreFc.Text = p.segmentFc + " kgf/cm²";
                    lbl_SteelConcreFy.Text = p.segmentFy + " kgf/cm²";
                    lbl_SteelMat.Text = p.Steel.Material;
                    lbl_SteelUW.Text = p.Steel.UW + " kN/m³";
                    lbl_SteelPR.Text = p.Steel.U12.ToString();
                    lbl_SteelFy.Text = p.Steel.Fy + " kgf/cm²";
                    lbl_SteelFu.Text = p.Steel.Fu + " kgf/cm²";

                    gpConnectTunnel.Enabled = true;
                    gpSteelTunnel.Enabled = true;
                    btn_FinalCal.Enabled = true;
                    lbl_SectionFunction.Text = "聯絡通道與鋼環片設計用";
                    Fig_ComboBox.Items.Add("鋼環片開孔前");
                    Fig_ComboBox.Items.Add("鋼環片開孔後");
                    checkB_Connector_Steel.Enabled = true;
                    break;
                case "C": //抗浮檢核用
                    lbl_SectionFunction.Text = "抗浮檢核用";
                    break;
                case "D": //場鑄環片用
                    //環片參數
                    lbl_SiteRadiusIn.Text = p.segmentRadiusIn.ToString() + " m";
                    lbl_SiteThick.Text = p.segmentThickness.ToString() + " m";                    
                    lbl_SiteAAngle.Text = p.segmentAAngle.ToString() + " θ";
                    lbl_SiteBAngle.Text = p.segmentBAngle.ToString() + " θ";
                    lbl_SiteKangle.Text = p.segmentKAngle.ToString() + " θ";
                    lbl_SitePoreAngle.Text = p.segmentAdjacentPoreAngle.ToString() + " θ";
                    lbl_SiteGroutAngle.Text = p.segmentAdjGroutAngle.ToString() + "θ";
                    lbl_SiteUW.Text = p.segmentUnitWeight.ToString() + " kN/m³";
                    lbl_SiteE.Text = p.segmentYoungsModulus.ToString() + " kN/m²";
                    lbl_SiteU12.Text = p.segmentPoissonRatio.ToString();
                    lbl_SiteFc.Text = p.segmentFc.ToString() + " kgf/cm²";
                    lbl_SiteFy.Text = p.segmentFy.ToString() + " kgf/cm²";

                    gpSiteTunnel.Enabled = true;
                    lbl_SectionFunction.Text = "場鑄環片設計用";
                    checkB_Site.Enabled = true;
                    break;
            }                        
        }
                
        private void 選擇標案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fProject f = new fProject();
            f.fParent = this;
            f.ShowDialog();
        }        

        private void cbo_LoadingTerm_SelectionChangeCommitted(object sender, EventArgs e)
        {
            gBox_LatDec.Enabled = false;
            gBox_VerInc.Enabled = false;
            
            switch (cbo_LoadingTerm.SelectedItem)
            {
                case "長期載重": cbo_LoadingTerm.Tag = "LongTerm"; break;
                case "短期載重": cbo_LoadingTerm.Tag = "ShortTerm"; break;
                case "0.33%直徑變化": gBox_LatDec.Enabled = true; break;
                case "地震載重": gBox_VerInc.Enabled = true; break;
            }

            //cbo_LoadingTerm.Tag = "";

        }

        #endregion  

        private void CartesianChart1OnDataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("You clicked (" + chartPoint.X + "," + chartPoint.Y + ")");
        }

        #region 環片自重計算
        private void SegmentSelf_External_Click(object sender, EventArgs e)
        {
            external_Segment.Transportation("webform", out string outputTransportation, out double transportationMmax, out double transportationVmax);
            TransportationMText.Text = transportationMmax.ToString();
            TransportationVText.Text = transportationVmax.ToString();
            external_Segment.Hanging("webform", out string outputHanging, out double hangingMmax, out double hangingVmax);
            HangingMText.Text = hangingMmax.ToString();
            HangingVText.Text = hangingVmax.ToString();
            external_Segment.Stacking("webform", out string outputStacking, out double stackingMmax, out double stackingVmax);
            StackingMText.Text = stackingMmax.ToString();
            StackingVText.Text = stackingVmax.ToString();

            web.DocumentText = outputTransportation;
        }

        private void STN_SegmentSelfInternal_Click(object sender, EventArgs e)
        {
            STN_SegmentSelf self = new STN_SegmentSelf(sectionUID);
            self.Transportation("webform", out string outputTransportation, out double transportationMmax, out double transportationVmax);
            TransportationMText.Text = transportationMmax.ToString();
            TransportationVText.Text = transportationVmax.ToString();

            self.Hanging("webform", out string outputHanging, out double hangingMmax, out double hangingVmax);
            HangingMText.Text = hangingMmax.ToString();
            HangingVText.Text = hangingVmax.ToString();

            self.Stacking("webform", out string outputStacking, out double stackingMmax, out double stackingVmax);
            StackingMText.Text = stackingMmax.ToString();
            StackingVText.Text = stackingVmax.ToString();

            SaveFileDialog fileSavePath = new SaveFileDialog();
            fileSavePath.Filter = "Word 文件|*.docx";
            fileSavePath.ShowDialog();

            self.FilePath = fileSavePath.FileName;
            self.Transportation("word", out string str, out double Mmax, out double Vmax);
            /*
            DocX document = DocX.Create(fileSavePath.FileName);

            Paragraph p = document.InsertParagraph("Test").FontSize(15).Font("標楷體").Font("Times New Roman");
            p.Append("\n 123");
            p = document.InsertParagraph("002").FontSize(14);
            p.Append("\n 321111");
            p.Append("\n 測試").FontSize(10);

            document.SaveAs(fileSavePath.FileName);
            */

        }
        #endregion

        #region SAP背填灌漿計算
        private void btn_GroutExcel_Click(object sender, EventArgs e)
        {
            excelOnly = true;
            btn_GroutSAP_Click(null, null);
        }

        private void btn_GroutSAP_Click(object sender, EventArgs e)
        {
            decreasedXaxisP = 0;
            increasedZaxisP = 0;
            sapCal = new SAP_SegmentProcess(sectionUID, "Grouting", decreasedXaxisP, increasedZaxisP);

            SaveFileDialog fileSavePath = new SaveFileDialog();
            fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
            //fileSavePath.Title = "GroutingInput";
            fileSavePath.ShowDialog();
            /*
            if(fileSavePath.ShowDialog() == DialogResult.OK)
            {
                //Stream fileStream = fileSavePath.OpenFile();
                //StreamWriter sw = new StreamWriter(fileStream);                
            }*/
            file = fileSavePath.FileName;
            if (file == "") return;

            //GroutingM.Text = file;
            //string savePath = "E:\\SAP2000API\\API_excel_grout_20190422\\";
            sapCal.GroutingCalculation(file, out double Mmax, out double Pmax, excelOnly);

            if (!excelOnly)
            {
                //GroutingM.Text = Mmax.ToString();
                //GroutingP.Text = Pmax.ToString();
                AnalysisDone();
            }
            else ExcelOutisDone();
            excelOnly = false;
        }

        private void btn_GroutExcel1_Click(object sender, EventArgs e)
        {
            
        }

        private void GroutingInternalCal_Click(object sender, EventArgs e)
        {                                   
            
        }
        #endregion

        #region SAP載重條件計算
        private void btn_LoadTermExcel_Click(object sender, EventArgs e)
        {
            excelOnly = true;
            btn_LoadTermSAP_Click(null, null);
        }

        private void btn_LoadTermSAP_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSavePath = new SaveFileDialog();
            fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
            fileSavePath.ShowDialog();
            file = fileSavePath.FileName;
            if (file == "") return;

            decreasedXaxisP = 0;
            increasedZaxisP = 0;
                        
            switch (cbo_LoadingTerm.SelectedItem)
            {
                case "長期載重":                                           
                case "短期載重":
                    string type = cbo_LoadingTerm.Tag.ToString();
                    sapCal = new SAP_SegmentProcess(sectionUID, type, decreasedXaxisP, increasedZaxisP);
                    sapCal.LoadingTerm(file, type, 1, out bool getResult01, eqBool, excelOnly);
                    sapCal = new SAP_SegmentProcess(sectionUID, type, decreasedXaxisP, increasedZaxisP);
                    sapCal.LoadingTerm(file, type, 2, out bool getResult02, eqBool, excelOnly);
                    sapCal = new SAP_SegmentProcess(sectionUID, type, decreasedXaxisP, increasedZaxisP);
                    sapCal.LoadingTerm(file, type, 3, out bool getResult03, eqBool, excelOnly);
                    break;
                case "0.33%直徑變化": VariationofDiameter(); break;
                case "地震載重": EQofDiameter(); break;
            }

            if (!excelOnly) AnalysisDone();
            else ExcelOutisDone();

            excelOnly = false;
        }

        private void btn_LoadingOutExcel_Click(object sender, EventArgs e)
        {            
            //LoadingtermInternalCal_Click(null, null);
        }

        private void LoadingtermInternalCal_Click(object sender, EventArgs e)
        {
            //SaveFileDialog fileSavePath = new SaveFileDialog();
            //fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
            //fileSavePath.ShowDialog();
            //file = fileSavePath.FileName;
            //if (file == "") return;
            ////if (file == "")
            ////{
                
            ////}

            //decreasedXaxisP = 0;
            //increasedZaxisP = 0;

            //if (LongTermInitialCheckBox.Checked)
            //{
            //    // //土層載重及建物載重計算  //指向EXCEL樣板檔...  取得網路變數資料
            //    sapCal = new SAP_SegmentProcess(sectionUID, "LongTerm", decreasedXaxisP, increasedZaxisP);

            //    //SAP 分析資料建立... 執行 SAP 分析   分析成果資料輸出到 EXCEL及網路
            //    sapCal.LoadingTerm(file, "LongTerm", 1, out bool getResult01, eqBool, excelOnly);

            //    sapCal = new SAP_SegmentProcess(sectionUID, "LongTerm", decreasedXaxisP, increasedZaxisP);
            //    sapCal.LoadingTerm(file, "LongTerm", 2, out bool getResult02, eqBool, excelOnly);

            //    sapCal = new SAP_SegmentProcess(sectionUID, "LongTerm", decreasedXaxisP, increasedZaxisP);
            //    sapCal.LoadingTerm(file, "LongTerm", 3, out bool getResult03, eqBool, excelOnly);
            //}


            //if (ShortTermInitialCheckBox.Checked)
            //{
            //    sapCal = new SAP_SegmentProcess(sectionUID, "ShortTerm", decreasedXaxisP, increasedZaxisP);
            //    sapCal.LoadingTerm(file, "ShortTerm", 1, out bool getResult04, eqBool, excelOnly);
            //    sapCal = new SAP_SegmentProcess(sectionUID, "ShortTerm", decreasedXaxisP, increasedZaxisP);
            //    sapCal.LoadingTerm(file, "ShortTerm", 2, out bool getResult05, eqBool, excelOnly);
            //    sapCal = new SAP_SegmentProcess(sectionUID, "ShortTerm", decreasedXaxisP, increasedZaxisP);
            //    sapCal.LoadingTerm(file, "ShortTerm", 3, out bool getResult06, eqBool, excelOnly);
            //}


            //if (VarDiaInitialCheckBox.Checked)
            //{
            //    VariationofDiameter();
            //}


            //if (SeismicCheckBox.Checked)
            //{
            //    EQofDiameter();
            //}

            //if (!excelOnly) AnalysisDone();
            //else ExcelOutisDone();

            //excelOnly = false;
        }

        

        private void VariationofDiameter()
        {
            SAP_VariationofDiameter var = new SAP_VariationofDiameter(sectionUID);
            double varDia = var.VariationCal();
            double decreasedValue = double.Parse(txt_LatDecRepeat.Text);

            decreasedXaxisP = double.Parse(txt_LatDecInitial.Text);
            increasedZaxisP = 0;
            for (int i = 0; i < 5; i++)
            {
                if (i != 0) decreasedXaxisP += decreasedValue;

                sapCal = new SAP_SegmentProcess(sectionUID, "VariationofDiameter", decreasedXaxisP, increasedZaxisP);
                sapCal.TargetVariation = varDia;
                sapCal.LoadingTerm(file, "VariationofDiameter", 3, out bool getResult, eqBool, excelOnly);

                if (excelOnly)
                {
                    sapCal = new SAP_SegmentProcess(sectionUID, "VariationofDiameter", decreasedXaxisP, increasedZaxisP);
                    sapCal.TargetVariation = varDia;
                    sapCal.LoadingTerm(file, "VariationofDiameter", 1, out bool getResult000, eqBool, excelOnly);

                    sapCal = new SAP_SegmentProcess(sectionUID, "VariationofDiameter", decreasedXaxisP, increasedZaxisP);
                    sapCal.TargetVariation = varDia;
                    sapCal.LoadingTerm(file, "VariationofDiameter", 2, out bool getResult001, eqBool, excelOnly);

                    //MessageBox.Show("輸出Excel表完成");
                    break;
                }
                else
                {
                    /*
                    string strXP = $"側向解壓值：{decreasedXaxisP} kN";
                    string strResult = $"分析變位：{sapCal.resultVariation}m";
                    string strTarget = $"0.33%直徑變位：{varDia}m";
                    string strOut = (strXP + Environment.NewLine + strTarget + Environment.NewLine + strResult + Environment.NewLine);

                    if (getResult)
                        MessageBox.Show(strOut + "符合變位，進行後續分析");
                    else if (i == 4)
                        MessageBox.Show(strOut + "5次迭代後仍無符合變位值，請重新檢討側向解壓值");
                    else
                    {
                        MessageBox.Show(strOut + "不符合變位，持續迭代分析" + Environment.NewLine + $"目前為第{i + 1}次迭代，至多進行5次迭代");
                    }
                    */
                    strResult(decreasedXaxisP, sapCal.resultVariation, varDia, getResult, i, "0.33%直徑變化");

                    if (getResult)
                    {
                        dataSearch.UpdateData("STN_Section", "UID", sectionUID, "VarDia_DecreasedXP", decreasedXaxisP);

                        sapCal = new SAP_SegmentProcess(sectionUID, "VariationofDiameter", decreasedXaxisP, increasedZaxisP);
                        sapCal.TargetVariation = varDia;
                        sapCal.LoadingTerm(file, "VariationofDiameter", 1, out bool getResult01, eqBool, excelOnly);

                        sapCal = new SAP_SegmentProcess(sectionUID, "VariationofDiameter", decreasedXaxisP, increasedZaxisP);
                        sapCal.TargetVariation = varDia;
                        sapCal.LoadingTerm(file, "VariationofDiameter", 2, out bool getResult02, eqBool, excelOnly);

                        break;
                    }
                    
                }

            }
        }

        private void EQofDiameter()
        {
            
            eqBool = false; //eqBool:判斷是否要計算地震載重的直徑變化，只有當載重為EQofDiameter時有用
            SAP_EQCalculation eq = new SAP_EQCalculation(sectionUID);
            double eqVar = eq.EQVariationCal();//計算地震造成的直徑變化增加量
            double eqDia; //常時載重(未加地震、長期)狀態的直徑變位量

            double increasedValue = double.Parse(txt_VerIncRepeat.Text);

            decreasedXaxisP = 0;
            increasedZaxisP = 0;
            sapCal = new SAP_SegmentProcess(sectionUID, "EQofDiameter", decreasedXaxisP, increasedZaxisP);
            sapCal.TargetVariation = eqVar; //地震造成的直徑變化增加量
            sapCal.LoadingTerm(file, "EQofDiameter", 1, out bool getResult00, eqBool, excelOnly);//先取得長期載重的直徑變化量
            eqDia = sapCal.EQDia; //常時載重(未加地震、長期)狀態的直徑變位量

            increasedZaxisP += double.Parse(txt_VerIncInitial.Text);
            for (int i = 0; i < 5; i++)
            {
                if (i != 0) increasedZaxisP += increasedValue;

                sapCal = new SAP_SegmentProcess(sectionUID, "EQofDiameter", decreasedXaxisP, increasedZaxisP);
                sapCal.TargetVariation = eqVar;
                sapCal.EQDia = eqDia;
                sapCal.LoadingTerm(file, "EQofDiameter", 3, out bool getResult, eqBool, excelOnly);
                eqBool = getResult;

                if (excelOnly)
                {
                    sapCal = new SAP_SegmentProcess(sectionUID, "EQofDiameter", decreasedXaxisP, increasedZaxisP);
                    sapCal.TargetVariation = eqVar;
                    sapCal.EQDia = eqDia;
                    sapCal.LoadingTerm(file, "EQofDiameter", 2, out bool getResult000, eqBool, excelOnly);

                    //MessageBox.Show("輸出Excel表完成");
                    break;
                }
                else
                {
                    strResult(increasedZaxisP, sapCal.resultVariation, eqVar, getResult, i, "地震造成直徑變位");

                    if (getResult)
                    {
                        dataSearch.UpdateData("STN_Section", "UID", sectionUID, "EQDia_IncreasedZP", increasedZaxisP);

                        sapCal = new SAP_SegmentProcess(sectionUID, "EQofDiameter", decreasedXaxisP, increasedZaxisP);
                        sapCal.TargetVariation = eqVar;
                        sapCal.LoadingTerm(file, "EQofDiameter", 1, out bool getResult01, eqBool, excelOnly);

                        sapCal = new SAP_SegmentProcess(sectionUID, "EQofDiameter", decreasedXaxisP, increasedZaxisP);
                        sapCal.TargetVariation = eqVar;
                        sapCal.LoadingTerm(file, "EQofDiameter", 2, out bool getResult02, eqBool, excelOnly);

                        break;
                    }
                }
            }
            
        }

        public void strResult(double pressureValue, double anaDis, double targetDis, bool getResult, int i, string condition)
        {
            string strPressure = "";
            if (condition == "0.33%直徑變化")
                strPressure = "側向解壓值";
            else if (condition == "地震造成直徑變位")
                strPressure = "垂直向加壓值";

            string strXP = $"{strPressure}：{pressureValue} kN";
            string strResult = $"分析變位：{Math.Round(anaDis,5)}m";
            string strTarget = $"{condition}：{Math.Round(targetDis, 5)}m";

            string strOut = (strXP + Environment.NewLine + strTarget + Environment.NewLine + strResult + Environment.NewLine);

            if (getResult)
                MessageBox.Show(strOut + "符合變位，進行後續分析");
            else if (i == 4)
                MessageBox.Show(strOut + $"5次迭代後仍無符合變位值，請重新檢討{strPressure}");
            else
            {
                MessageBox.Show(strOut + "不符合變位，持續迭代分析" + Environment.NewLine + $"目前為第{i + 1}次迭代，至多進行5次迭代");
            }
        }

        public void AnalysisDone()
        {
            MessageBox.Show("分析完成");
        }
        public void ExcelOutisDone()
        {
            MessageBox.Show("輸出Excel完成");
        }


        #endregion

        #region SAP接觸深度選擇與計算
        private void Fig_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string condition;
            Condition(out condition);


            //List<Tuple<string, string, int, string, double, double, double>> figData = new List<Tuple<string, string, int, string, double, double, double>>();
            //figData.Clear();
            List<Tuple<string, string, int, string, double, double, double>> figData = sgCalDepth.FigureShow(condition, out int figNum, out List<Tuple<string, string>> figProfile);
            figNum /= 3;

            List<double> x = new List<double>();
            List<double> sap = new List<double>();
            List<double> cal = new List<double>();
            List<LiveCharts.WinForms.CartesianChart> figList = new List<LiveCharts.WinForms.CartesianChart> { Fig01, Fig02, Fig03, Fig04, Fig05, Fig06, Fig07, Fig08, Fig09, Fig10, Fig11, Fig12 };
            List<TextBox> figDepth = new List<TextBox> { Fig01_Depth, Fig02_Depth, Fig03_Depth, Fig04_Depth, Fig05_Depth, Fig06_Depth, Fig07_Depth, Fig08_Depth, Fig09_Depth, Fig10_Depth, Fig11_Depth, Fig12_Depth };

            double calDepth;

            for (int i = 0; i < figNum; i++)
            {
                try
                {
                    x.Clear();
                    x.Add(figData[i].Item5);
                    x.Add(figData[i + figNum].Item5);
                    x.Add(figData[i + figNum * 2].Item5);
                    sap.Clear();
                    sap.Add(figData[i].Item7);
                    sap.Add(figData[i + figNum].Item7);
                    sap.Add(figData[i + figNum * 2].Item7);
                    cal.Clear();
                    cal.Add(figData[i].Item6);
                    cal.Add(figData[i + figNum].Item6);
                    cal.Add(figData[i + figNum * 2].Item6);

                    sgCalDepth.Fig(figList[i], x, sap, cal);
                    sgCalDepth.LinearRegression(x, sap, out double slope01, out double constant01);
                    sgCalDepth.LinearRegression(x, cal, out double slope02, out double constant02);
                    calDepth = Math.Round((-1) * (constant01 - constant02) / (slope01 - slope02), 3);
                    if (calDepth > x[2]) calDepth = x[2];
                    else if (calDepth < x[0]) calDepth = x[0];

                    figDepth[i].Text = calDepth.ToString();
                }
                catch { }

            }
        }

        /*
        private void InputFinalDepth_Click_1(object sender, EventArgs e)
        {
            string condition = "";
            Condition(out condition);

            List<Tuple<string, string, int, string, double, double, double>> figData = sgCalDepth.FigureShow(condition, out int figNum, out List<Tuple<string, string>> figProfile);
            List<Tuple<string, int, string, string, double>> finalDepth = new List<Tuple<string, int, string, string, double>>();
            List<double> inputDepth = new List<double>();
            List<string> inputUID = new List<string>();
            List<LiveCharts.WinForms.CartesianChart> figList = new List<LiveCharts.WinForms.CartesianChart> { Fig01, Fig02, Fig03, Fig04, Fig05, Fig06, Fig07, Fig08, Fig09, Fig10, Fig11, Fig12 };
            List<TextBox> figDepth = new List<TextBox> { Fig01_Depth, Fig02_Depth, Fig03_Depth, Fig04_Depth, Fig05_Depth, Fig06_Depth, Fig07_Depth, Fig08_Depth, Fig09_Depth, Fig10_Depth, Fig11_Depth, Fig12_Depth };
            for (int i = 0; i < figProfile.Count; i++)
            {
                inputUID.Add(Guid.NewGuid().ToString("D"));
                var data = Tuple.Create(condition, 4, figProfile[i].Item1, figProfile[i].Item2, double.Parse(figDepth[i].Text));
                finalDepth.Add(data);
                inputDepth.Add(double.Parse(figDepth[i].Text));
            }
            try
            {
                dataSearch.DeleteCalDepth(sectionUID, condition, 4);
            }
            catch
            {
            }
            dataSearch.InsertSGCalDepthData(inputUID, sectionUID, finalDepth);

            sgCalDepth.InsertFinalDepthData(inputDepth);
            sgCalDepth.PutSegmentUIDtoFinalDepth(condition, 4);
        }
        */

        private void btn_FinalCal_Click(object sender, EventArgs e)
        {
            FinalDepthInput();
            //InputFinalDepth_Click_1(null, null);

            if (file == "")
            {
                SaveFileDialog fileSavePath = new SaveFileDialog();
                fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
                fileSavePath.ShowDialog();
                file = fileSavePath.FileName;
            }
            if (file == "") return;
            decreasedXaxisP = 0;
            increasedZaxisP = 0;


            switch (Fig_ComboBox.Text)
            {
                case "長期載重":
                    {
                        sapCal = new SAP_SegmentProcess(sectionUID, "LongTerm", decreasedXaxisP, increasedZaxisP);
                        sapCal.PrecastContDepth = precastContDepth;
                        sapCal.LoadingTerm(file, "LongTerm", 4, out bool getResult01, eqBool, excelOnly);
                    }
                    break;
                case "短期載重":
                    {
                        sapCal = new SAP_SegmentProcess(sectionUID, "ShortTerm", decreasedXaxisP, increasedZaxisP);
                        sapCal.PrecastContDepth = precastContDepth;
                        sapCal.LoadingTerm(file, "ShortTerm", 4, out bool getResult02, eqBool, excelOnly);
                    }
                    break;
                //case "200年洪水位":
                //    sap200yFlood.SAP200yFloodCal(out string error);
                //    MessageBox.Show(error);
                //    break;
                case "0.33%直徑變化":
                    {
                        DataTable section = dataSearch.GetByUID("STN_Section", sectionUID);
                        decreasedXaxisP = double.Parse(section.Rows[0]["VarDia_DecreasedXP"].ToString());
                        sapCal = new SAP_SegmentProcess(sectionUID, "VariationofDiameter", decreasedXaxisP, increasedZaxisP);
                        sapCal.PrecastContDepth = precastContDepth;
                        sapCal.LoadingTerm(file, "VariationofDiameter", 4, out bool getResult03, eqBool, excelOnly);
                    }
                    break;
                case "地震載重":
                    {
                        DataTable section = dataSearch.GetByUID("STN_Section", sectionUID);
                        increasedZaxisP = double.Parse(section.Rows[0]["EQDia_IncreasedZP"].ToString());
                        sapCal = new SAP_SegmentProcess(sectionUID, "EQofDiameter", decreasedXaxisP, increasedZaxisP);
                        sapCal.PrecastContDepth = precastContDepth;
                        sapCal.LoadingTerm(file, "EQofDiameter", 4, out bool getResult04, eqBool, excelOnly);
                    }
                    break;
                case "鋼環片開孔前":
                    {
                        //excelOnly = true;
                        oSteelTunnel = new SAP_SteelTunnel(sectionUID);
                        oSteelTunnel.Process(file, "SteelTunnelOrigin", excelOnly, 4, steelContDepth);
                        //excelOnly = false;
                    }
                    break;
                case "鋼環片開孔後":
                    {
                        //excelOnly = true;
                        oSteelTunnel = new SAP_SteelTunnel(sectionUID);
                        oSteelTunnel.Process(file, "SteelTunnelCut", excelOnly, 4, steelContDepth);
                        //excelOnly = false;
                    }
                    break;
            }
            AnalysisDone();
        }

        List<string> steelContDepth = new List<string>();
        List<string> precastContDepth = new List<string>();
        private void FinalDepthInput()
        {
            string condition = "";
            Condition(out condition);

            List<Tuple<string, string, int, string, double, double, double>> figData = 
                sgCalDepth.FigureShow(condition, out int figNum, out List<Tuple<string, string>> figProfile);
            //List<Tuple<string, int, string, string, double>> finalDepth = new 
            //    List<Tuple<string, int, string, string, double>>();
            //List<double> inputDepth = new List<double>();
            //List<string> inputUID = new List<string>();
            List<LiveCharts.WinForms.CartesianChart> figList = new List<LiveCharts.WinForms.CartesianChart>
            { Fig01, Fig02, Fig03, Fig04, Fig05, Fig06, Fig07, Fig08, Fig09, Fig10, Fig11, Fig12 };
            List<TextBox> figDepth = new List<TextBox> { Fig01_Depth, Fig02_Depth, Fig03_Depth, Fig04_Depth,
                Fig05_Depth, Fig06_Depth, Fig07_Depth, Fig08_Depth, Fig09_Depth, Fig10_Depth, Fig11_Depth, Fig12_Depth };

            steelContDepth.Clear();
            precastContDepth.Clear();
            switch (condition)
            {
                case "Steel_Origin":
                case "Steel_Cut":
                    for(int i = 0; i < figProfile.Count; i++) // 0 ~ 10  第一環：0 ~ 4、 第二環：5 ~ 10
                    {
                        if (i < figProfile.Count / 2) steelContDepth.Add($"{figDepth[i].Text},Full");
                        else steelContDepth.Add($"{figDepth[i].Text},Half");

                    }
                    break;
                default:
                    //for (int i = 0; i < figProfile.Count; i++)
                    //{
                    //    inputUID.Add(Guid.NewGuid().ToString("D"));
                    //    var data = Tuple.Create(condition, 4, figProfile[i].Item1, figProfile[i].Item2, 
                    //        double.Parse(figDepth[i].Text));
                    //    finalDepth.Add(data);
                    //    inputDepth.Add(double.Parse(figDepth[i].Text));
                    //}
                    //try
                    //{
                    //    dataSearch.DeleteCalDepth(sectionUID, condition, 4);
                    //}
                    //catch
                    //{
                    //}
                    //dataSearch.InsertSGCalDepthData(inputUID, sectionUID, finalDepth);

                    //sgCalDepth.InsertFinalDepthData(inputDepth);
                    //sgCalDepth.PutSegmentUIDtoFinalDepth(condition, 4);

                    for(int i = 0; i < figProfile.Count; i++)
                        precastContDepth.Add($"{figDepth[i].Text}");
                    break;
            }
            
        }

        private void Condition(out string condition)
        {
            condition = "";
            switch (Fig_ComboBox.Text)
            {
                case "0.33%直徑變化":
                    condition = "VariationofDiameter";
                    break;
                case "地震載重":
                    condition = "EQofDiameter";
                    break;
                case "長期載重":
                    condition = "LongTerm";
                    break;
                case "短期載重":
                    condition = "ShortTerm";
                    break;
                //case "200年洪水位":
                //    condition = "200yFlood";
                //    break;
                case "鋼環片開孔前":
                    condition = "Steel_Origin";
                    break;
                case "鋼環片開孔後":
                    condition = "Steel_Cut";
                    break;
            }
        }       

        #endregion

        #region 應變檢核
        private void STN_StrainCheck_Internal_Click(object sender, EventArgs e)
        {
            STN_StrainCheck cal10_strain = new STN_StrainCheck(sectionUID, "winform");
            cal10_strain.VerticalStainByEQ(out string str, out double[] ODEStrain, out double[] MDEStrain);
            ODEVCStrainText.Text = ODEStrain[0].ToString();
            ODEVTStrainText.Text = ODEStrain[1].ToString();
            MDEVCStrainText.Text = MDEStrain[0].ToString();
            MDEVTStrainText.Text = MDEStrain[1].ToString();

            cal10_strain.TorsionStrainByEQ(out string torsion, out double[] rackODEStrain, out double[] rackMDEStrain);
            ODETCStrainText.Text = rackODEStrain[0].ToString();
            ODETTStrainText.Text = rackODEStrain[1].ToString();
            MDETCStrainText.Text = rackMDEStrain[0].ToString();
            MDETTStrainText.Text = rackMDEStrain[1].ToString();

        }

        private void VSExternal_Click(object sender, EventArgs e)
        {
            external_VerticalStress.VerticalStress(out string longTermVS, out string shortTermVS, out string surcharge, out double longTermE, out double shortTermE, out double Pv, out double longTermPh1, out double longTermPh2, out double shortTermPh1, out double shortTermPh2, out double U12);
            /*
            cal02_VerticalStress.VerticalStress(out string longtermVerticalStress, out string shortermVerticalStress, out string SurchargeLoad, out double longtermE1, out double shortermE1);
            LongTermE1_Ex.Text = longtermE1.ToString();
            LongtermE1_In.Text = longtermVerticalStress;
            LongTermE1_Ex.Text = SurchargeLoad;
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            STN_StrainCheck test = new STN_StrainCheck(sectionUID, "webform");
            /*test.VerticalStainByEQ(out string vertical, out double[] ODE, out double[] MDE);
            Cal03_output.Text = vertical;            
            web.DocumentText = vertical;*/
            test.VerticalStainByEQ(out string vertical, out double[] odeVertical, out double[] mdeVertical);
            test.TorsionStrainByEQ(out string torsion, out double[] ODE, out double[] MDE);
            test.TorsionStrainByEQAndExcavation(out string str, out string loose, out string flex, out string compres, out List<double> ODEstrain, out List<double> MDEstrain, out List<double> looseODE, out List<double> looseMDE);
            test.StrainWithNotCircle(out string notCircle, out List<double> notcircleODE, out List<double> notcircleMDE);
            web.DocumentText = vertical + "<br> next <br> <br>" + torsion + "<br> next <br> <br>" + str + "<br> next <br><br>" + loose + "<br> next <br><br>" + notCircle;
            Cal03_output.Text = vertical + "<br> next <br> <br>" + torsion + "<br> next <br> <br>" + str + "<br> next <br><br>" + loose + "<br> next <br><br>" + notCircle + "<br> next <br><br>" + flex + "<br> next <br><br>" + compres;
            //Cal03_output.Text = vertical;
        }
        #endregion



        private void btn_VerticalStressInternal_Click(object sender, EventArgs e)
        {
            string str = cbo_LoadingCal.Text;
            STN_VerticalStress vertical = new STN_VerticalStress(sectionUID, "Webform");
            vertical.VerticalStress(str, out string longtermVerticalStress, out string shortermVerticalStress, out string surchargeLoad, out double longtermE1, out double shortermE1, out double Pv, out double lph1, out double lph2, out double sph1, out double sph2, out double U12);
            LongtermE1_In.Text = longtermE1.ToString();
            LongtermE1_In.Text = longtermVerticalStress;
            ShortermE1_In.Text = shortermVerticalStress;
            //LongtermE1_In.Text = surchargeLoad;

            web.DocumentText = longtermVerticalStress.ToString() + "<br>" + shortermVerticalStress.ToString() + "<br>" + surchargeLoad.ToString(); 
        }

        private void ContactingDepth_Click(object sender, EventArgs e)
        {
            /*
            SidePanel.Height = ContactingDepth.Height;
            SidePanel.Top = ContactingDepth.Top;
            FigPanel.Show();
            FigPanel.BringToFront();
            */
        }                                           

        private void 結束ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
            System.Environment.Exit(1);

        }

        

        #region //聯絡通道 ; 鋼環片分析 ;  場鑄環片分析

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sss = "";
            switch (tabControl3.SelectedTab.Text)
            {
                case "聯絡通道分析":
                    if (sectionUID != "")
                    {
                        oConnectTunnel = new SAP_ConnectTunnel(sectionUID);
                        //if (p.connector.TR.ToString() == "0" || p.connector.TR.ToString() == "0") sss = "";
                        ////gpConnectTunnel.Enabled = false;
                        //else
                        //{
                        //    //txtCTunnelR.Text = p.connector.TR.ToString();
                        //    //txtCTunnelH.Text = p.connector.BH.ToString();
                        //    //gpConnectTunnel.Enabled = true;
                        //}
                    }
                    //else
                        //gpConnectTunnel.Enabled = false;
                    break;
                case "鋼環片分析":
                    if (sectionUID != "")
                    {
                        oSteelTunnel = new SAP_SteelTunnel(sectionUID);
                        //if (p.segmentAdjacentPoreAngle.ToString() == "0" || p.segmentAAngle.ToString() == "0") sss = "";
                        ////gpSteelTunnel.Enabled = false;
                        //else
                        //{
                        //    //txtRadius.Text = p.segmentRadiusIn.ToString();
                        //    //txtThick.Text = p.segmentThickness.ToString();
                        //    //txtAangle.Text = p.segmentAAngle.ToString();
                        //    //txtBangle.Text = p.segmentBAngle.ToString();
                        //    //txtKangle.Text = p.segmentKAngle.ToString();
                        //    //txtADangle.Text = p.segmentAdjacentPoreAngle.ToString();
                        //    //gpSteelTunnel.Enabled = true;
                        //}
                    }
                    //else
                        //gpSteelTunnel.Enabled = false;
                    break;

                case "場鑄環片分析":  //gpSteelTunnel
                    if (sectionUID != "")
                    {
                        oSiteTunnel = new SAP_SiteTunnel(sectionUID);
                        //if (p.segmentAdjacentPoreAngle.ToString() == "0" || p.segmentAAngle.ToString() == "0") sss = "";
                        ////gpSiteTunnel.Enabled = false;
                        //else
                        //{
                        //    //txt3Radius.Text = p.segmentRadiusIn.ToString();
                        //    //txt3Thick.Text = p.segmentThickness.ToString();
                        //    //txt3ADangle.Text = p.segmentAdjacentPoreAngle.ToString();
                        //    //gpSiteTunnel.Enabled = true;
                        //}
                    }
                    //else
                        //gpSiteTunnel.Enabled = false;
                    break; 
                default:
                    break; 
            }
        }

        private void btn_ConnectorExcel_Click(object sender, EventArgs e)
        {
            excelOnly = true;
            btn_ConnectorCal_Click(null, null);
        }

        /// <summary>
        /// 聯絡通道執行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ConnectorCal_Click(object sender, EventArgs e)
        {
            //  txtCTunnelR.Text = p.ConnectTunnel_Radius.ToString();
            //  txtCTunnelH.Text = p.ConnectTunnel_High.ToString();

            SaveFileDialog fileSavePath = new SaveFileDialog();
            fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
            fileSavePath.ShowDialog();
            file = fileSavePath.FileName;
            if (file == "") return;
            //if (file == "")
            //{

            //}
            oConnectTunnel.Process(file, "ConnectTunnel", excelOnly);
            MessageBox.Show ("OK");
            excelOnly = false;

        }

        private void btn_SteelExcel_Click(object sender, EventArgs e)
        {
            excelOnly = true;
            btn_SteelCal_Click(null, null);
        }

        private void btn_SteelCal_Click(object sender, EventArgs e)
        {
            //鋼環片分析
            SaveFileDialog fileSavePath = new SaveFileDialog();
            fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
            fileSavePath.ShowDialog();
            file = fileSavePath.FileName;
            if (file == "") return;
            //if (file == "")
            //{

            //}
            oSteelTunnel.Process(file, "SteelTunnelOrigin", excelOnly, 0, steelContDepth);
            oSteelTunnel.Process(file, "SteelTunnelCut", excelOnly, 0, steelContDepth);
            MessageBox.Show("OK");
            excelOnly = false;
        }

        //private void btn_SteelExcel_Click(object sender, EventArgs e)
        //{
            
        //    btnExcute2_Click(null, null);
        //}

        //private void btnExcute2_Click(object sender, EventArgs e)
        //{   //鋼環片分析
        //    SaveFileDialog fileSavePath = new SaveFileDialog();
        //    fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
        //    fileSavePath.ShowDialog();
        //    file = fileSavePath.FileName;
        //    if (file == "") return;
        //    //if (file == "")
        //    //{

        //    //}
        //    oSteelTunnel.Process(file, "SteelTunnelOrigin", excelOnly, 0, steelContDepth);
        //    oSteelTunnel.Process(file, "SteelTunnelCut", excelOnly, 0, steelContDepth);
        //    MessageBox.Show("OK");
        //    excelOnly = false;
        //}       

        private void btn_SiteExcel_Click(object sender, EventArgs e)
        {
            excelOnly = true;
            btn_SiteCal_Click(null, null);
        }

        private void btn_SiteCal_Click(object sender, EventArgs e)
        {   //場鑄環片分析
            SaveFileDialog fileSavePath = new SaveFileDialog();
            fileSavePath.Filter = "Excel 活頁簿|*.xlsx";
            fileSavePath.ShowDialog();
            file = fileSavePath.FileName;
            if (file == "") return;
            //if (file == "")
            //{

            //}
            oSiteTunnel.Process(file, "SiteTunnel", excelOnly);
            MessageBox.Show("OK");
            excelOnly = false;

        }




        #endregion

        private void btn_InputFrameMaterial_Click(object sender, EventArgs e)
        {
            InputFrameMaterial input = new InputFrameMaterial(sectionUID);
            if (checkB_Precast.Checked) input.Precast();
            if (checkB_Connector_Steel.Checked) input.Connector(); input.Steel();
            if (checkB_Site.Checked) input.Site();
            MessageBox.Show("輸入桿件材料完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            oGeneralCal = new GeneralCalculation(sectionUID, "");
            oGeneralCal.Process(out string str);

            web.DocumentText = str;
            textBox1.Text = str;

            //STN_StrainCheck oSTN_StrainCheck = new STN_StrainCheck(sectionUID, "win");
            //oSTN_StrainCheck.F_Dia_C_PushAutoInput();
            //oSTN_StrainCheck.Loose_F_DiaAutoInput();
        }
    }

}
