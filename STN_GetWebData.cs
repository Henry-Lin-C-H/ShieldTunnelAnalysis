using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SinoTunnel  // 讀取網路資料庫資料到變數
{

    /// <summary>
    /// 讀取網路資料庫資料到變數
    /// </summary>
    public class GetWebData  
    {
        //  STN_SQL.STN_data datasearch = new STN_SQL.STN_data();

        public string sectionUID;
        public string projectUID = "";

        public DataTable segment;
        public DataTable segmentFrame;
        public DataTable material;


        public double newton = 9.80665;
        public double soilE_Nonc = 2800;
        public double soilE_c = 350;
        public double steelEs = 2.04E06;
        //public double steelStrength = 4200;

        //STN_Section 環片內徑、厚度 算載重時會先算，不能與寬度、角度、接觸深度等一同抓取，可能會因尚未輸入而導致錯誤       
        public double segmentRadiusIn = 0;
        public double segmentThickness = 0;
        public double segmentRadiusInter = 0;
        public double segmentRadiusOut = 0;

        //STN_Section 環片寬度、接觸深度、角度
        public double segmentWidth = 0;
        public double segmentContacingDepth = 0;
        public double segmentAngle = 0;
        public double segmentAdjacentPoreAngle = 0;               
        public double segmentAAngle = 0;
        public double segmentBAngle = 0;
        public double segmentKAngle = 0;
        public double segmentAdjGroutAngle = 0;
        public double segmentStackingL1 = 0;
        public double segmentStackingL2 = 0;
        public double segmentStackingL3 = 0;

        //STN_Section 環片單位重、楊式模數、柏松比、混凝土強度、鋼筋強度
        public double segmentUnitWeight = 0;
        public double segmentYoungsModulus = 0;
        public double segmentPoissonRatio = 0;
        public double segmentFc = 0;
        public double segmentFy = 0;

        //STN_Section 地下水位與地表高程
        public double GWL = 0;
        public double groundEle = 0;

        //STN_Section 覆土深度、路寬
        public double coverDepth = 0;
        public double roadWidth = 0;

        //STN_Section 環片灌漿壓力
        public double groutPressure = 0;
                
        //STN_Section 環片三次迭代計算選用接觸深度之環片UID，目前沒有用到
        //public List<string> segmentUsedUID = new List<string>();

        //STN_Section 200年洪水位高程
        public double floodEle = 0;

        //STN_Section - 聯絡通道參數
        public Connector connector;
        //public string ConnectLoadType = "";        
        //public float ConnectTunnel_Radius = 0; //聯絡通道上半圓半徑R(m)
        //public float ConnectTunnel_High = 0;   //聯絡通道下部垂直段長度H (m)
        //public List<string> ConnectTunnelSegment = new List<string>();  //聯絡通道桿件資料
        //public double ConnectCoverDepth = 0;
        //public double ConnectorUnitWeight = 0;
        //public double ConnectorPoissonRatio = 0;
        //public double ConnectorFc = 0;
        //public double ConnectorFy = 0;

        //STN_Section - 鋼環片鋼材料單位重
        public Steel Steel;
        //public string steelMaterial = "";
        //public double steelUW = 0;
        //public double steelU12 = 0;
        //public double steelFy = 0;
        //public double steelFu = 0;
        //public double steelCutB = 0;
        //public double steelPoissonRatio = 0;

        //抓取選定之frame的性質與對應的材料性質的UID
        public string segmentUID = "";
        public string materialUID = "";


        //STN_EQProp 地震參數
        public double soilK05 = 0.5;
        public double soilK20 = 2.0;

        public double shearwaveV = 0;
        public double ODEg = 0;
        public double ODEah = 0;
        public double ODEVh = 0;
        public double MDEg = 0;
        public double MDEah = 0;
        public double MDEVh = 0;        
        public double verticalReduced = 0;
        public double lateralIncreased = 0;
        public double k05UPDN = 0;
        public double k20UPDN = 0;
        public double k05TwoSides = 0;
        public double k20TwoSides = 0;
        //public double eqDensity = 0;
        public double DVariation = 0;



        //*****************************************//

        ExcuteSQL oExcuteSQL = new ExcuteSQL();       
        public SoilLayer[] SL;     //STN_SoilLayer  //定義土層變數
        public Load[] LD;          //STN_LOAD //定義建物及交通載重變數
        public TunnelBoringData TN_M;
        //public Segment Frame;
        //public Material Material;
        public SGMainSteel SGMainSteel;

        string STRFraction(string numerator, string denominator)
        {
            return $"<div class='frac'><span>{numerator}</span><span class='symbol'>/</span><span class='bottom'>{denominator}</span></div>";
        }

        public GetWebData(string sectionUID)
        {
            this.sectionUID = sectionUID;

            if (sectionUID == "") return; 
            DataTable dt = new DataTable();
            this.segment = oExcuteSQL.GetByUID("STN_Section", sectionUID);

            try
            {                
                this.projectUID = segment.Rows[0]["Project"].ToString();
            }
            catch
            {
            }

            #region STN_Section 環片內徑、厚度
            try
            {
                this.segmentRadiusIn = double.Parse(segment.Rows[0]["SGRadiusIn"].ToString());
                this.segmentThickness = double.Parse(segment.Rows[0]["SGThickness"].ToString());
                this.segmentRadiusInter = this.segmentRadiusIn + this.segmentThickness / 2;
                this.segmentRadiusOut = this.segmentRadiusIn + this.segmentThickness;

            }
            catch
            {
            }
            #endregion

            #region  STN_Section 環片寬度、接觸深度、角度
            try
            {
                
                //this.segment = oExcuteSQL.GetByUID("STN_Section", sectionUID);
                this.segmentWidth = double.Parse(segment.Rows[0]["SGWidth"].ToString());
                this.segmentContacingDepth = double.Parse(segment.Rows[0]["SGContactingDepth"].ToString());
                this.segmentAngle = double.Parse(segment.Rows[0]["SGAngle"].ToString());
                this.segmentAdjacentPoreAngle = double.Parse(segment.Rows[0]["AdjPoreAngle"].ToString());                               
                this.segmentAAngle = double.Parse(segment.Rows[0]["SGAAngle"].ToString());
                this.segmentBAngle = double.Parse(segment.Rows[0]["SGBAngle"].ToString());
                this.segmentKAngle = double.Parse(segment.Rows[0]["SGKAngle"].ToString());
                this.segmentAdjGroutAngle = double.Parse(segment.Rows[0]["AdjGroutAngle"].ToString());
                this.segmentStackingL1 = double.Parse(segment.Rows[0]["StackingL1"].ToString());
                this.segmentStackingL2 = double.Parse(segment.Rows[0]["StackingL2"].ToString());
                this.segmentStackingL3 = double.Parse(segment.Rows[0]["StackingL3"].ToString());                                                       
            }
            catch
            {
            }
            #endregion

            #region STN_Section 環片單位重、楊式模數、柏松比
            try
            {                
                this.segmentUnitWeight = double.Parse(segment.Rows[0]["SGUnitWeight"].ToString());
                this.segmentYoungsModulus = double.Parse(segment.Rows[0]["SGYoungsModulus"].ToString());
                this.segmentPoissonRatio = double.Parse(segment.Rows[0]["SGPoissonRatio"].ToString());
                this.segmentFc = double.Parse(segment.Rows[0]["SGfc"].ToString());
                this.segmentFy = double.Parse(segment.Rows[0]["SGfy"].ToString());
            }
            catch
            {
            }
            #endregion

            #region STN_Section, STN_Segment, STN_SegmentMaterial 選定之frame的性質與對應的材料性質
            try
            {
                //this.segmentUID = segment.Rows[0]["Load_Segment"].ToString();

                //this.segmentFrame = oExcuteSQL.GetByUID("STN_Segment", segmentUID);
                //Frame = new Segment();                              
                //this.Frame.UID = segmentFrame.Rows[0]["UID"].ToString();
                //this.Frame.Section = segmentFrame.Rows[0]["Section"].ToString();
                //this.Frame.Name = segmentFrame.Rows[0]["Name"].ToString();
                //this.Frame.MaterialUID = segmentFrame.Rows[0]["Material"].ToString();
                //this.Frame.Shape = segmentFrame.Rows[0]["Shape"].ToString();
                //this.Frame.Width = double.Parse(segmentFrame.Rows[0]["Width"].ToString());
                //this.Frame.Height = double.Parse(segmentFrame.Rows[0]["Height"].ToString());

                //this.materialUID = segmentFrame.Rows[0]["Material"].ToString();
                //this.material = oExcuteSQL.GetByUID("STN_SegmentMaterial", materialUID);
                //Material = new Material();
                //this.Material.UID = material.Rows[0]["UID"].ToString();
                //this.Material.ProjectUID = material.Rows[0]["Project"].ToString();
                //this.Material.Name = material.Rows[0]["MaterialName"].ToString();
                //this.Material.UnitWeight = double.Parse(material.Rows[0]["UnitWeight"].ToString());
                //this.Material.YoungsModulus = double.Parse(material.Rows[0]["YoungModulus"].ToString());
                //this.Material.PoissonRatio = double.Parse(material.Rows[0]["PoissonRatio"].ToString());
                //this.Material.CTE = double.Parse(material.Rows[0]["CTE"].ToString());
                //this.Material.Fc = double.Parse(material.Rows[0]["Fc"].ToString());
            }
            catch
            {
            }
            #endregion

            #region STN_Section 地下水位與地表高程
            try
            {

                this.GWL = double.Parse(segment.Rows[0]["GWL"].ToString());
                this.groundEle = double.Parse(segment.Rows[0]["GroundEle"].ToString());

            }
            catch
            {
            }
            #endregion  

            #region STN_Section 覆土深度、路寬
            try
            {
                this.coverDepth = double.Parse(segment.Rows[0]["CoverDepth"].ToString());
                this.roadWidth = double.Parse(segment.Rows[0]["RoadWidth"].ToString());
            }
            catch
            {
            }
            #endregion

            #region 背填灌漿壓力
            try { this.groutPressure = double.Parse(segment.Rows[0]["BFG_W"].ToString()); }
            catch { }
            #endregion

            #region STN_Section 環片三次迭代計算選用接觸深度之環片UID，目前沒有用到
            /*
            try
            {
                this.segmentUsedUID.Add(segment.Rows[0]["LLI1_Segment"].ToString());
                this.segmentUsedUID.Add(segment.Rows[0]["LLI2_Segment"].ToString());
                this.segmentUsedUID.Add(segment.Rows[0]["LLI3_Segment"].ToString());
            }
            catch
            {
            }
            */
            #endregion

            #region STN_Section 200年洪水位高程
            try
            {
                this.floodEle = double.Parse(segment.Rows[0]["200yFloodEle"].ToString());
            }
            catch
            {
            }
            #endregion                         
            
            #region STN_Section - 聯絡通道參數
            try
            {
                
                this.connector.LoadType = segment.Rows[0]["ConnectorLoadType"].ToString();
                this.connector.TR = float.Parse(segment.Rows[0]["ConnectorTR"].ToString()); //聯絡通道上半圓半徑R(m)
                this.connector.BH = float.Parse(segment.Rows[0]["ConnectorBH"].ToString());//聯絡通道下部垂直段長度H (m)                                
                this.connector.CoverDepth = double.Parse(segment.Rows[0]["ConnectorCoverDepth"].ToString());
                this.connector.UnitWeight = double.Parse(segment.Rows[0]["ConnectorUnitW"].ToString());
                this.connector.PoissonRatio = double.Parse(segment.Rows[0]["ConnectorPoissonR"].ToString());
                this.connector.Fc = double.Parse(segment.Rows[0]["ConnectorFc"].ToString());
                this.connector.E = double.Parse(segment.Rows[0]["ConnectorE"].ToString());
                this.connector.Fy = double.Parse(segment.Rows[0]["ConnectorFy"].ToString());
                //dt = oExcuteSQL.GetBySection("STN_ConnectorFrameData", sectionUID, "");
                //ConnectTunnelSegment.Clear(); 
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{   // 取得桿件材質,形狀,厚度,混凝土強度等資料 
                //    //STN_ConnectorFrameData= $"{Name}, {Shape}, {Width}, {Height}, {Locate}, {MaterialName}, {FC}";
                //    string LISTString = "";
                //    String Name = dt.Rows[i]["Name"].ToString();  //Segment 100x30cm
                //    String Shape = dt.Rows[i]["Shape"].ToString();
                //    String MaterialID = dt.Rows[i]["Material"].ToString();
                //    float Width = float.Parse(dt.Rows[i]["Width"].ToString());
                //    float Height = float.Parse(dt.Rows[i]["Height"].ToString());
                //    String Locate = DataCheck(dt.Rows[i]["Locate"].ToString());
                //    LISTString = $"{Name}, {Shape}, {Width}, {Height}, {Locate}";              
                //    DataTable dx = oExcuteSQL.GetByUID("STN_SegmentMaterial", MaterialID);
                //    for (int j = 0; j < dx.Rows.Count; j++)
                //    {
                //        String MaterialName = DataCheck(dx.Rows[j]["MaterialName"].ToString()); //Concrete 280kg/cm2
                //        String FC = DataCheck(dx.Rows[j]["Fc"].ToString());
                //        LISTString += $", {MaterialName}, {FC}";   
                //    }
                //    ConnectTunnelSegment.Add(LISTString);
                //    //ConnectTunnelSegment=$"{Name}, {Shape}, {Width}, {Height}, {Locate}, {MaterialName}, {FC}";
                //}
            }
            catch
            {
            }
            #endregion

            #region STN_Section - 鋼環片鋼材料單位重
            try
            {
                Steel = new Steel();
                Steel.Material = segment.Rows[0]["SteelMaterial"].ToString();
                Steel.UW = double.Parse(segment.Rows[0]["SteelUnitW"].ToString());
                Steel.U12 = double.Parse(segment.Rows[0]["SteelPoissonR"].ToString());
                Steel.Fy = double.Parse(segment.Rows[0]["SteelFy"].ToString());
                Steel.Fu = double.Parse(segment.Rows[0]["SteelFu"].ToString());
                Steel.CutB = double.Parse(segment.Rows[0]["SteelCutB"].ToString());
                //this.steelMaterial = segment.Rows[0]["SteelMaterial"].ToString();
                //this.steelUW = double.Parse(segment.Rows[0]["SteelUnitW"].ToString());
                //this.steelU12 = double.Parse(segment.Rows[0]["SteelPoissonR"].ToString());
                //this.steelFy = double.Parse(segment.Rows[0]["SteelFy"].ToString());
                //this.steelFu = double.Parse(segment.Rows[0]["SteelFu"].ToString());
                //this.steelCutB = double.Parse(segment.Rows[0]["SteelCutB"].ToString());
            }
            catch { }
            #endregion


            #region STN_Load - 建物及交通載重參數 STN_Type
            try
            {
                dt = oExcuteSQL.GetBySection("STN_Load", sectionUID, "");
                LD = new Load[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LD[i].P = float.Parse(dt.Rows[i]["P"].ToString());
                    LD[i].X1 = float.Parse(dt.Rows[i]["X1"].ToString());
                    LD[i].X2 = float.Parse(dt.Rows[i]["X2"].ToString());
                }
            }
            catch
            {
            }
            #endregion

            #region STN_SoilLayer-土壤參數輸入 STN_Type
            try
            {
                
                dt = oExcuteSQL.GetBySection("STN_SoilLayer", sectionUID, "Order By [Depth]");
                SL = new SoilLayer[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SL[i].SoilType = dt.Rows[i]["SoilType"].ToString();
                    SL[i].Depth = float.Parse(dt.Rows[i]["Depth"].ToString());
                    SL[i].γ = float.Parse(dt.Rows[i]["γ"].ToString());
                    SL[i].N = float.Parse(dt.Rows[i]["N"].ToString());
                    if (dt.Rows[i]["Suδv"].ToString().Trim().ToString() != "") SL[i].Suδv = float.Parse(dt.Rows[i]["Suδv"].ToString());
                    SL[i].ν = float.Parse(dt.Rows[i]["ν"].ToString());
                    SL[i].φ = float.Parse(dt.Rows[i]["φ"].ToString());
                }
                
            }
            catch
            {
            }
            #endregion


            #region STN_EQProp 地震參數
            try
            {                
                DataTable eqData = oExcuteSQL.GetBySection("STN_EQProp", sectionUID, "");

                this.shearwaveV = double.Parse(eqData.Rows[0]["ShearWaveVelocity"].ToString());
                this.ODEg = double.Parse(eqData.Rows[0]["ODEg"].ToString());
                this.ODEah = double.Parse(eqData.Rows[0]["ODEah"].ToString());
                this.ODEVh = double.Parse(eqData.Rows[0]["ODEVh"].ToString());
                this.MDEg = double.Parse(eqData.Rows[0]["MDEg"].ToString());
                this.MDEah = double.Parse(eqData.Rows[0]["MDEah"].ToString());
                this.MDEVh = double.Parse(eqData.Rows[0]["MDEVh"].ToString());
                //this.soilK05 = double.Parse(eqData.Rows[0]["K0.5"].ToString());
                //this.soilK20 = double.Parse(eqData.Rows[0]["K2.0"].ToString());
                this.verticalReduced = double.Parse(eqData.Rows[0]["VerticalReduced"].ToString());
                this.lateralIncreased = double.Parse(eqData.Rows[0]["LateralIncreased"].ToString());
                this.k05UPDN = double.Parse(eqData.Rows[0]["K0.5UPDN"].ToString());
                this.k20UPDN = double.Parse(eqData.Rows[0]["K2.0UPDN"].ToString());
                this.k05TwoSides = double.Parse(eqData.Rows[0]["K0.5TwoSides"].ToString());
                this.k20TwoSides = double.Parse(eqData.Rows[0]["K2.0TwoSides"].ToString());                
                this.DVariation = double.Parse(eqData.Rows[0]["DVariation"].ToString());                
            }
            catch
            {
            }

            //try
            //{
            //    DataTable eqData = oExcuteSQL.GetBySection("STN_EQProp", sectionUID, "");
            //    this.eqDensity = double.Parse(eqData.Rows[0]["Density"].ToString());
            //}
            //catch
            //{
            //}
            #endregion

            #region STN_Section 環片主筋配筋參數
            try
            {
                SGMainSteel = new SGMainSteel();
                SGMainSteel.SGYBarNo1 = double.Parse(segment.Rows[0]["SGYBarNo1"].ToString());
                SGMainSteel.SGYBarNo2 = double.Parse(segment.Rows[0]["SGYBarNo2"].ToString());
                SGMainSteel.SGYBarNum = int.Parse(segment.Rows[0]["SGYBarNum"].ToString());

                DataTable tempDT = oExcuteSQL.GetDataBySQL($"SELECT * FROM SEC_RebarType WHERE BarNo = {SGMainSteel.SGYBarNo1}");
                SGMainSteel.SGYBarArea1 = double.Parse(tempDT.Rows[0]["Area"].ToString());
                tempDT = oExcuteSQL.GetDataBySQL($"SELECT * FROM SEC_RebarType WHERE BarNo = {SGMainSteel.SGYBarNo2}");
                SGMainSteel.SGYBarArea2 = double.Parse(tempDT.Rows[0]["Area"].ToString());

                SGMainSteel.Area = (SGMainSteel.SGYBarArea1 + SGMainSteel.SGYBarArea2) * SGMainSteel.SGYBarNum;
            }
            catch
            {
            }
            #endregion

            try
            {
                DataTable Shield = oExcuteSQL.GetBySection("STN_TunnelBoringData", sectionUID, "");
                TN_M = new TunnelBoringData();
                TN_M.DO = float.Parse(Shield.Rows[0]["DO"].ToString());
            }
            catch
            {
            }


        }

        public string DataCheck(string dr)
        {
            string strReturr = "";

            if ( dr.Trim() != "") strReturr = dr.Trim();

            return strReturr;
        }

    }
}
