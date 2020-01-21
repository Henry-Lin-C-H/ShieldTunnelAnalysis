using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SinoTunnel
{
    public class SAPOut_ConnectTunnel
    {
        //SAP_ExcelInput input;
        ExcuteSQL oExcuteSQL = new ExcuteSQL();
        GetWebData p;
        STN_VerticalStress verticalStress;
        Excel2Word word = new Excel2Word();
        //SinoTunnelFile.UploadFile oUploadFile = new UploadFile();

        string sectionUID = "";
        //string DefaultfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Normal_Case_Raw.xlsx";
        //string DefaultfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Bauckup\\Normal_Case_Raw.xlsx";
        string outputPath = "";
        string inputgPath = "";

        double Fc;
        double E;
        double U12;
        double UT;
        double width;

        DataTable resultList;
        DataTable dataList;

        public SAPOut_ConnectTunnel(string sectionUID)
        {
            //input = new SAP_ExcelInput(DefaultfilePath);
            this.sectionUID = sectionUID;
            this.p = new GetWebData(sectionUID);  //  取得網路變數資料
            verticalStress = new STN_VerticalStress(sectionUID, "WEBFORM");  //取得 土層楊氏係數及

            this.Fc = p.connector.Fc;
            this.E = p.connector.E;
            this.U12 = p.connector.PoissonRatio;
            this.UT = p.connector.UnitWeight;
            this.width = 1;

            this.resultList = oExcuteSQL.GetBySection("STN_ConnectorResult", sectionUID, "");
            this.dataList = oExcuteSQL.GetBySection("STN_ConnectorData", sectionUID, "");
        }

        float High = 0;  // p.ConnectTunnel_High;
        float Radius = 0; // p.ConnectTunnel_Radius;
        float Divide = 0.6f;  //桿件分割最大長度 0.6m    
        float SpringOffset = 1.0f;  //土壤彈簧支距長度 1.0m   
        double Angle = 0.0;   // 均分 90度圓時的角度(徑度)
        float SideLength = 0.0f;    //側牆間隔長度
        float BaseLength = 0.0f;    //底板間隔長度 

        List<string> Joints = new List<string>();  //桿件點位
        List<string> Springs = new List<string>();  //彈簧點位
        List<string> Members = new List<string>();  //桿件
        List<string> LinkProperty = new List<string>(); //彈簧桿件性質
        //List<string> FramePropperty = new List<string>(); //桿件混凝土強度值

        private List<string> frameName = new List<string>();
        #region Loading Coordinates ; Joint Restraint Assignments 

        public void Joint_Coordinates()
        {
            High = p.connector.BH;
            Radius = p.connector.TR;
            Joints = JointsProcess("", High, Radius, 0);  // 隧道點位   
            Springs = JointsProcess("S", High, Radius, 1);  // 隧道外彈簧點位 
        }

        List<string> BHFrame = new List<string>(); //底板frame的名稱
        List<string> TRFrame = new List<string>(); //頂圓與側牆frame的名稱
        int BHStart = 0; //底板frame的起始處
        int BHEnd = 0; //底板frame的終點處，側牆frame的起始處

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JM">"S" =定義為土壤彈簧 </param>
        /// <param name="High"></param>
        /// <param name="Radius"></param> 
        /// <param name="Offset">彈簧桿件支距</param>
        /// <returns></returns>
        public List<string> JointsProcess(string JM, float High, float Radius, float Offset)
        {
            List<string> CoorExcel = new List<string>();
            //點位編輯原則由圓頂順時間方向編輯
            int Joint = 0;
            string x1 = "";
            string z1 = "";

            //右半部頂板(圓/4)區間數
            int gape = Convert.ToInt16(Math.Ceiling(Radius * Math.PI / 2 / Divide) + 1); //頂板(圓/4)取最大分割區間數
            int springStart = gape / 2;
            int start = 0;
            if (JM == "") BHStart += gape; //從頂部到側牆頂
            else start = springStart;
            Angle = Math.PI / 2 / gape;   // 均分 90度圓時的角度(徑度)
            for (Joint = start; Joint <= gape; ++Joint)
            {
                x1 = string.Format("{0:0.000}", 0 + (Radius + Offset) * Math.Cos(Math.PI / 2 - Angle * Joint));
                z1 = string.Format("{0:0.000}", High + (Radius + Offset) * Math.Sin(Math.PI / 2 - Angle * Joint));
                CoorExcel.Add($"{JM}{Joint + 1},GLOBAL,Cartesian,{x1},0,,{z1}");
            }
            //右半部側牆點位區間數
            gape = Convert.ToInt16(Math.Ceiling(High / Divide) - 1);  //側牆間隔點位
            if (JM == "") BHStart += gape; //從側牆頂到側牆底
            SideLength = Convert.ToSingle(High / gape);           //側牆間隔長度
            for (int j = 1; j <= gape; ++j)
            {
                z1 = string.Format("{0:0.000}", High - SideLength * j);
                if (JM == "S" && j == gape)   //當配置彈簧時,需多新增底板兩端之垂直彈簧
                    CoorExcel.Add($"{JM}{Joint + j}H,GLOBAL,Cartesian,{Radius + Offset},0,,{z1}");
                else
                    CoorExcel.Add($"{JM}{Joint + j},GLOBAL,Cartesian,{Radius + Offset},0,,{z1}");
            }

            if (JM == "") BHEnd = BHStart;
            if (JM == "") BHEnd += (gape * 2 + 1);

            Joint += gape;
            int JSpring = 1;
            if (JM == "S") JSpring = 0; //當配置彈簧時,需多新增底板兩端之垂直彈簧
            //右半部底板點位區間數
            gape = Convert.ToInt16(Math.Ceiling(Radius / Divide));    //底板間隔點位

            if (JM == "")
            {
                for (int j = 1; j <= gape * 2; j++)
                {
                    BHFrame.Add($"{BHStart + j}"); //底板frame的名稱
                }
            }


            BaseLength = Convert.ToSingle(Radius / gape);                     //底板間隔長度         
            for (int j = JSpring; j <= gape; ++j)
            {
                x1 = string.Format("{0:0.000}", Radius - BaseLength * j);

                if (JM == "S" && j == 0)   //當配置彈簧時,需多新增底板兩端之垂直彈簧
                    CoorExcel.Add($"{JM}{Joint + j}V,GLOBAL,Cartesian,{x1},0,,{-Offset}");
                else
                    CoorExcel.Add($"{JM}{Joint + j},GLOBAL,Cartesian,{x1},0,,{-Offset}");
            }

            //  ***************  所有點左測對稱再佈點
            Joint += gape;

            //if (JM == "S") start = CoorExcel.Count - 1;
            //else start = CoorExcel.Count - 2;
            int end = 1;
            if (JM == "S") end = 0;
            for (int i = CoorExcel.Count - 2; i >= end; --i)
            {
                Joint += 1;
                string[] XX = CoorExcel[i].Split(',');
                x1 = string.Format("{0:-0.000}", float.Parse(XX[3].ToString()));

                switch (XX[0].Last().ToString())
                {
                    case "V":
                        CoorExcel.Add($"{JM}{Joint}V,GLOBAL,Cartesian,{x1},0,,{XX[6]}");
                        Joint -= 1;
                        break;
                    case "H":
                        CoorExcel.Add($"{JM}{Joint}H,GLOBAL,Cartesian,{x1},0,,0");
                        break;
                    default:
                        CoorExcel.Add($"{JM}{Joint},GLOBAL,Cartesian,{x1},0,,{XX[6]}");
                        break;
                }
            }

            if (JM == "") //頂圓與側牆frame的名稱
            {
                for (int i = 0; i < CoorExcel.Count; i++)
                {
                    bool tr = true;
                    string[] xx = CoorExcel[i].Split(',');

                    for (int j = 0; j < BHFrame.Count; j++)
                        if (BHFrame[j] == xx[0]) tr = false;

                    if (tr) TRFrame.Add(xx[0].ToString());
                }
            }


            //input.PutDataToSheet("Joint Coordinates", CoorExcel);
            return CoorExcel;
        }

        public void Joint_Restraint_Assignments()
        {
            List<string> JointRestraint = new List<string>();
            for (int j = 0; j < Joints.Count; ++j)
            {
                string[] JJ = Joints[j].Split(',');
                JointRestraint.Add($"{JJ[0]},No,Yes,No,Yes,No,Yes");
            }
            for (int j = 0; j < Springs.Count; ++j)
            {
                string[] JJ = Springs[j].Split(',');
                JointRestraint.Add($"{JJ[0]},Yes,Yes,Yes,Yes,Yes,Yes");
            }
            //input.PutDataToSheet("Joint Restraint Assignments", JointRestraint);
        }

        #endregion

        #region Connectivity-Frame And Frame Output Station Assigns ; Connectivity-Link And Link Property Assignments 
        public void Connectivity_FrameAND_Frame_Output_Station_Assigns()
        {   //  定義桿件IJ端     
            List<string> Output = new List<string>();
            for (int j = 1; j < Joints.Count; ++j)
            {
                string[] JJ = Joints[j].Split(',');
                Members.Add($"{j},{j},{j + 1},No");
                Output.Add($"{j},MinNumSta,2,,Yes,Yes");
                frameName.Add($"{j}");
            }
            Members.Add($"{Joints.Count}, {Joints.Count}, 1, No"); //最後一隻桿件
            frameName.Add($"{Joints.Count}");
            Output.Add($"{Joints.Count},MinNumSta,2,,Yes,Yes");//最後一隻桿件
            //input.PutDataToSheet("Connectivity - Frame", Members);
            //input.PutDataToSheet("Frame Output Station Assigns", Output);
        }

        public void Connectivity_Link_And_Link_Property_Assignments()
        {
            List<string> Links = new List<string>();     //彈簧桿件
            for (int j = 0; j < Springs.Count; ++j)
            {   // Connectivity_Link
                string[] JJ = Springs[j].Split(',');
                string XX = JJ[0].Substring(1).Replace("V", "");
                XX = XX.Replace("H", "");
                int NN = int.Parse(XX);
                string LL = "L" + JJ[0].Substring(1);
                Links.Add($"{LL}, {NN}, {JJ[0]}");
                
                // Link_Property_Assignments 
                float X = float.Parse(JJ[3]);
                float Z = float.Parse(JJ[6]);
                if (Z == 0) { LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-CornerH,None"); CornerHLink.Add(LL); }
                else
                if (Math.Abs(X) == Radius && Z == -SpringOffset) { LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-CornerV,None"); CornerVLink.Add(LL); }
                else
                if (Z == -SpringOffset) { LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Base,None"); BaseLink.Add(LL); }
                else
                if (Math.Abs(X) == Radius + SpringOffset && Z == High) { LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Tangent,None"); TangentLink.Add(LL); }
                else
                if (Math.Abs(X) == Radius + SpringOffset) { LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Side,None"); SideLink.Add(LL); }
                else
                { LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Vault,None"); VaultLink.Add(LL); }
            }
            //input.PutDataToSheet("Connectivity - Link", Links);
            //input.PutDataToSheet("Link Property Assignments", LinkProperty);
        }
        #endregion

        #region Link Props 01 - General; Link Props 05 - Gap ; Link Property Assignments
        public void Link_Props_01_General()      //Link Props 05 - Gap
        {
            List<string> Link = new List<string>();
            Link.Add($"Spring-Vault, Gap, 0, 0, 0.0001, 0.0001, 0.0001, 1, 1, 0, 0, 0, 0, Gray8Dark");
            Link.Add($"Spring-Tangent, Gap, 0, 0, 0.0001, 0.0001, 0.0001, 1, 1, 0, 0, 0, 0, White");
            Link.Add($"Spring-Side, Gap, 0, 0, 0.0001, 0.0001, 0.0001, 1, 1, 0, 0, 0, 0, Yellow");
            Link.Add($"Spring-CornerH, Gap, 0, 0, 0.0001, 0.0001, 0.0001, 1, 1, 0, 0, 0, 0, Gray8Dark");
            Link.Add($"Spring-CornerV, Gap, 0, 0, 0.0001, 0.0001, 0.0001, 1, 1, 0, 0, 0, 0, White");
            Link.Add($"Spring-Base, Gap, 0, 0, 0.0001, 0.0001, 0.0001, 1, 1, 0, 0, 0, 0, Yellow");
            //input.PutDataToSheet("Link Props 01 - General", Link);
        }

        double Ks;
        public void Link_Props_05_Gap()      //Link Props 05 - Gap
        {
            verticalStress.VerticalStress("CONNECTOR", out string outlstr, out string sstr, out string surstr, out double longtermE1, out double shE, out double pv, out double lph1, out double lph2, out double shp1, out double shp2, out double u12);  //segmentYoungsModulus
            double Em = longtermE1;

            double Pr = u12;

            Ks = Em * (1 - Pr) / Radius / (1 + Pr) / (1 - 2 * Pr);
            List<string> LinkProperty = new List<string>();

            double KKK = 0;
            KKK = Ks * 1.0 * Radius * Angle;
            string XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-Vault,U1,No,Yes,{XXX},0,{XXX},0");

            KKK = Ks * 1.0 * (Radius * Angle + SideLength) / 2;
            XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-Tangent,U1,No,Yes,{XXX},0,{XXX},0");

            KKK = Ks * 1.0 * SideLength;
            XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-Side,U1,No,Yes,{XXX},0,{XXX},0");

            KKK = Ks * 1.0 * SideLength / 2;
            XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-CornerH,U1,No,Yes,{XXX},0,{XXX},0");

            KKK = Ks * 1.0 * BaseLength / 2;
            XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-CornerV,U1,No,Yes,{XXX},0,{XXX},0");

            KKK = Ks * 1.0 * BaseLength;
            XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-Base,U1,No,Yes,{XXX},0,{XXX},0");
            //input.PutDataToSheet("Link Props 05 - Gap", LinkProperty);
        }
        #endregion

        #region Set Frame & Material
        string TRframeName = "";
        string BHframeName = "";
        DataTable mat;
        double BHdepth;
        double TRdepth;
        public void SetFrameMaterial()
        {
            mat = oExcuteSQL.GetBySection("STN_FrameMaterial", sectionUID, "ORDER BY TIMES ASC");
            BHdepth = 0;
            TRdepth = 0;

            for (int i = 0; i < mat.Rows.Count; i++)
            {
                if (mat.Rows[i]["LoadType"].ToString() == "Connector")
                {
                    switch (mat.Rows[i]["Times"].ToString())
                    {
                        case "BH": BHdepth = double.Parse(mat.Rows[i]["Depth"].ToString()); break;
                        case "TR": TRdepth = double.Parse(mat.Rows[i]["Depth"].ToString()); break;
                    }
                }
            }

            string matName = "";
            matName = $"Concrete {Fc}";
            List<string> Mat01 = new List<string>();
            List<string> Mat02 = new List<string>();
            List<string> Mat03b = new List<string>();
            Mat01.Add($"{matName},Concrete,Isotropic,No");
            Mat02.Add($"{matName},{UT},{UT / p.newton},{E},{E / 2 / (1 + U12)},{U12},{9.9E-6}");
            Mat03b.Add($"{matName},{Fc * 98},No,Mander,Takeda,0.0021914,0.005,-0.1,0,0");

            //input.PutDataToSheet("MatProp 01 - General", Mat01);
            //input.PutDataToSheet("MatProp 02 - Basic Mech Props", Mat02);
            //input.PutDataToSheet("MatProp 03b - Concrete Data", Mat03b);

            List<string> frame01 = new List<string>();
            List<string> frame02 = new List<string>();
            List<string> frameName = new List<string>();
            List<double> inputDepth = new List<double>();
            inputDepth.Add(TRdepth);
            inputDepth.Add(BHdepth);
            frameName.Add($"Frame {width * 100}x{TRdepth * 100}cm");
            frameName.Add($"Frame {width * 100}x{BHdepth * 100}cm");
            for (int i = 0; i < frameName.Count; i++)
            {
                frame01.Add($"{frameName[i]},{matName},Rectangular,{inputDepth[i]},{width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                frame02.Add($"{frameName[i]},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
            }
            TRframeName = frameName[0];
            BHframeName = frameName[1];

            //input.PutDataToSheet("Frame Props 01 - General", frame01);
            //input.PutDataToSheet("Frame Props 02 - Concrete col", frame02);

        }
        #endregion

        List<string> VaultLink = new List<string>();
        List<string> TangentLink = new List<string>();
        List<string> SideLink = new List<string>();
        List<string> CornerHLink = new List<string>();
        List<string> CornerVLink = new List<string>();
        List<string> BaseLink = new List<string>();
        public string ConnectTunnelSTR()
        {
            Joint_Coordinates();
            Joint_Restraint_Assignments();
            Connectivity_FrameAND_Frame_Output_Station_Assigns();
            Connectivity_Link_And_Link_Property_Assignments();
            Link_Props_01_General();
            Link_Props_05_Gap();

            string str = "";

            //str = "土壤彈簧係數計算 <br> ";
            str = "";

            double Em = Math.Round(verticalStress.longTermSoilE, 0);
            double soilNu = Math.Round(verticalStress.Nu12,1);
            string strKs = string.Format("{0:0.00}", Ks);
            str += $"土層楊氏係數 Em = {Em} kN/m <br> ";
            str += $"土層柏松比 ν = {soilNu} <br> ";
            str += $"R：聯絡通道上半圓半徑 = {Radius} m <br> ";
            str += $"Ks = Em*(1-ν)/(R*(1+ν)*(1-2*ν) = {Em}*(1-{soilNu})/({Radius}*(1+{soilNu})*(1-2*{soilNu})) = {strKs} kN/m <br> ";

            str += $"(1)上半圓彈簧 <br> ";
            for(int i = 0; i < VaultLink.Count; i++) { str += $" {VaultLink[i]},"; }
            str += $"<br> ";
            double vaultK = Ks * 1.0 * Radius * Angle;
            string strVaultK = string.Format("{0:0.00}", vaultK);
            str += $"K = {strKs} * 1.0 * {Radius} * {Angle * 180 / Math.PI * 2} / 2 * π / 180 =  {strVaultK} kN/m <br> ";

            str += $"(2)側邊彈簧 <br> ";
            for (int i = 0; i < SideLink.Count; i++) { str += $" {SideLink[i]}, "; }
            str += "<br> ";
            double sideK = Ks * 1.0 * SideLength;
            string strSideK = string.Format("{0:0.00}", sideK);
            str += $"K = {strKs} * 1.0 * {SideLength} = {strSideK} kN/m <br> ";

            str += $"(3)上半圓與側邊交界彈簧 <br> ";
            for(int i = 0; i < TangentLink.Count; i++) { str += $" {TangentLink[i]}, "; }
            str += "<br> ";
            double tangentK = vaultK / 2 + sideK / 2;
            string strTangentK = string.Format("{0:0.00}", tangentK);
            str += $"K = {strVaultK} / 2 + {strSideK} / 2 = {strTangentK} kN/m <br> ";

            str += $"(4)側邊與底邊交界水平彈簧 <br> ";
            for(int i = 0; i < CornerHLink.Count; i++) { str += $" {CornerHLink[i]}, "; }
            str += $"<br> ";
            double cornerHK = sideK / 2;
            string strCornerHK = string.Format("{0:0.00}", cornerHK);
            str += $"K = {strSideK} / 2 = {strCornerHK} kN/m <br> ";

            str += $"(5)底邊垂直彈簧 <br> ";
            for (int i = 0; i < BaseLink.Count; i++) { str += $" {BaseLink[i]}, "; }
            str += $"<br> ";
            double baseK = Ks * 1.0 * BaseLength;
            string strBaseK = string.Format("{0:0.00}", baseK);
            str += $"K = {strKs} * 1.0 * {BaseLength} = {strBaseK} kN/m <br> ";

            str += $"(6)側邊與底邊交界垂直彈簧 <br> ";
            for (int i = 0; i < CornerVLink.Count; i++) { str += $" {CornerVLink[i]}, "; }
            str += $"<br> ";
            double cornerVK = baseK / 2;
            string strCornerVK = string.Format("{0:0.00}", cornerVK);
            str += $"K = {strBaseK} / 2 = {strCornerVK} kN/m <br> ";
            
            return str;
        }

        public string ConnectTunnelSteelSTR()
        {
            SetFrameMaterial();

            string str = "";
                        
            int botMainNo = int.Parse(resultList.Rows[0]["BHYBarNo1"].ToString());
            int botMainSpacing = int.Parse(resultList.Rows[0]["BHYBarSpacing"].ToString());
            int botShearNo = int.Parse(resultList.Rows[0]["BHSBarNo"].ToString());

            int sideMainNo = int.Parse(resultList.Rows[0]["TRYBarNo1"].ToString());
            int sideMainSpacing = int.Parse(resultList.Rows[0]["TRYBarSpacing"].ToString());
            int sideShearNo = int.Parse(resultList.Rows[0]["TRSBarNo"].ToString());

            str = "聯絡通道配筋安排如下所示 <br> ";

            str += $"<table style='text-align:center' border='5' width='300'> <tr> ";
            str += $"<th> 類別 </th> <th> 使用鋼筋 </th> <tr> ";
            str += $"<th> {BHdepth * 100} cm 底板主筋 </th> <th> D{botMainNo} @ {botMainSpacing} cm </th> <tr> ";
            str += $"<th> {BHdepth * 100} cm 底板箍筋 </th> <th> D{botShearNo} {botMainSpacing} x {botMainSpacing} cm </th> <tr> ";
            str += $"<th> {TRdepth * 100} cm 側牆主筋 </th> <th> D{sideMainNo} @ {sideMainSpacing} cm </th> <tr> ";
            str += $"<th> {TRdepth * 100} cm 側牆箍筋 </th> <th> D{sideShearNo} {sideMainSpacing} x {sideMainSpacing} cm </th> <tr> ";

            str += $"</table> ";

            return str;
        }

        public string ConnectTunnelDataSTR()
        {
            string str = "";

            List<string> resultUID = new List<string>();
            resultUID.Add(resultList.Rows[0]["BHRC"].ToString());
            resultUID.Add(resultList.Rows[0]["BHStirrup"].ToString());
            resultUID.Add(resultList.Rows[0]["TRRC"].ToString());
            resultUID.Add(resultList.Rows[0]["TRStirrup"].ToString());

            List<string> item = new List<string> { "Member", "Joint", "Axial", "ShearY", "ShearZ",
                "Torsion", "MomentY", "MomentZ" };
            var find = dataList.Select($"UID='{resultUID[0]}'")[0]["Axial"].ToString();
            List<string> each = new List<string> { "底板主筋", "底板箍筋", "側牆主筋", "側牆箍筋" };

            //str = "聯絡通道分析結果 <br> ";
            str = "";

            str += $"<table style='text-align:center' border='5'> <tr> ";
            str += $"<th> 項目 </th> <th> 桿件 </th> <th> 節點 </th> <th> Axial </th> <th> Shear-Y </th> <th> Shear-Z </th> " +
                $"<th> Torsion </th> <th> Moment-Y </th> <th> Momen-Z </th> <tr> ";

            

            for(int i = 0; i < resultUID.Count; i++)
            {
                str += $"<th> {each[i]} </th> ";
                for(int j = 0; j < item.Count; j++)
                {
                    var data = dataList.Select($"UID='{resultUID[i]}'")[0][$"{item[j]}"].ToString();
                    str += $"<th> {Math.Round(double.Parse(data),4)} </th> ";
                }
                str += $"<tr> ";
            }
            str += $"</table> ";

            return str;
        }
        
    }

    
}
