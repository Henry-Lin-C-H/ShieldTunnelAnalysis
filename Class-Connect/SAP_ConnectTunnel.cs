using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SegmentCalcu;

using SAP2000v15;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using STN_SQL;

namespace SinoTunnel
{
    class SAP_ConnectTunnel
    {   // 載重計算 及 資料置入 EXCEL 
        SAP_ExcelInput input;
        ExcuteSQL oExcuteSQL = new ExcuteSQL();
        GetWebData p;
        STN_VerticalStress verticalStress;
        Excel2Word word = new Excel2Word();


        string sectionUID = "";
        string DefaultfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Normal_Case_Raw.xlsx";
        //string DefaultfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Bauckup\\Normal_Case_Raw.xlsx";
        string outputPath = "";
        string inputgPath = "";

        double Fc;
        double E;
        double U12;
        double UT;
        double width;        

        public SAP_ConnectTunnel(string sectionUID)
        {
            input = new SAP_ExcelInput(DefaultfilePath);
            this.sectionUID = sectionUID;
            this.p = new GetWebData(sectionUID);  //  取得網路變數資料
            verticalStress = new STN_VerticalStress(sectionUID, "WEBFORM");  //取得 土層楊氏係數及

            this.Fc = p.connector.Fc;
            this.E = p.connector.E;
            this.U12 = p.connector.PoissonRatio;
            this.UT = p.connector.UnitWeight;
            this.width = 1;
        }

        float High = 0;  // p.ConnectTunnel_High;
        float Radius = 0; // p.ConnectTunnel_Radius;
        float Divide = 0.6f;  //桿件分割最大長度 0.6m    
        float SpringOffset = 1.0f;  //土壤彈簧支距長度 1.0m   
        double Angle = 0.0;   // 均分 90度圓時的角度(徑度)
        float SideLength = 0.0f;    //側牆間隔長度
        float BaseLength = 0.0f;    //底板間隔長度 

        List<string> Joints = new List <string>();  //桿件點位
        List<string> Springs = new List<string>();  //彈簧點位
        List<string> Members = new List<string>();  //桿件
        List<string> LinkProperty = new List<string>(); //彈簧桿件性質
        //List<string> FramePropperty = new List<string>(); //桿件混凝土強度值

        private List<string> frameName = new List<string>();
        public void Process(string xfileSavingPath, string condition, bool excelOnly)
        {
            inputgPath = xfileSavingPath.Replace(".xlsx", $"_{condition}Input1st.xlsx");
            outputPath = xfileSavingPath.Replace(".xlsx", $"_{condition}ResultFinal.xlsx");
            Joint_Coordinates();
            Joint_Restraint_Assignments();
            Connectivity_FrameAND_Frame_Output_Station_Assigns();
            Connectivity_Link_And_Link_Property_Assignments();
            Link_Props_01_General();
            Link_Props_05_Gap();
            //Frame_Props_01_Generall_AND_Frame_Props_02_Concrete_Col();
            SetFrameMaterial();
         //  Frame_Section_Assignments();  //Frame_Section_Assignments
            //MatProp_01_General_AND_MatProp_02_BasicAnd_Mech_Props_AND_MatProp_03b_Concrete_Data();
            Frame_Loads_Distributed_AND_Frame_Section_Assignments();
            input.FileSaving(inputgPath);

            // all worksheet
            List<string> names = new List<string>();
            names.Add("Joint Coordinates");
            names.Add("Joint Restraint Assignments");
            names.Add("Connectivity - Frame");
            names.Add("Connectivity - Link");
            names.Add("Link Property Assignments");
            names.Add("Link Props 05 - Gap");
            names.Add("Frame Props 01 - General");
            names.Add("Frame Section Assignments");
            names.Add("Frame Loads - Distributed");
            names.Add("Jt Spring Assigns 1 - Uncoupled");
            names.Add("MatProp 01 - General");
            names.Add("MatProp 02 - Basic Mech Props");
            names.Add("MatProp 03b - Concrete Data");
            names.Add("Load Case Definitions");
            names.Add("Load Pattern Definitions");
            names.Add("Case - Static 1 - Load Assigns");
            names.Add("Case - Static 2 - NL Load App");
            names.Add("Combination Definitions");
            names.Add("Program Control");
            string wordpath = xfileSavingPath.Replace(".xlsx", $".docx");
            word.Add(inputgPath, names, false, false);

            if (excelOnly) return;

            SAPCalculation(inputgPath, "DL+SOIL (NL)", outputPath, frameName, out bool getResult, "Connector", 1);

            word.Add(outputPath, names, true, false);
            word.FileSaving(wordpath);


            string TR = "TR";            
            List<Tuple<string, double, double, double>> outData = new List<Tuple<string, double, double, double>>();
            List<string> rc = new List<string>();
            outData = SAPChosenData("", frameName, BHFrame, "Main");
            rc.Add(outData[0].Item1);
            outData = SAPChosenData("", frameName, BHFrame, "Shear");
            rc.Add(outData[0].Item1);
            outData = SAPChosenData(TR, frameName, TRFrame, "Main");
            rc.Add(outData[0].Item1);
            outData = SAPChosenData(TR, frameName, TRFrame, "Shear");
            rc.Add(outData[0].Item1);

            try
            {
                oExcuteSQL.DeleteDataBySectionUID("STN_ConnectorResult", sectionUID);
            }
            catch
            {
            }
            oExcuteSQL.InsertData("STN_ConnectorResult", "Section", sectionUID);
            oExcuteSQL.UpdateData("STN_ConnectorResult", "Section", sectionUID, "BHRC", rc[0]);
            oExcuteSQL.UpdateData("STN_ConnectorResult", "Section", sectionUID, "BHStirrup", rc[1]);
            oExcuteSQL.UpdateData("STN_ConnectorResult", "Section", sectionUID, "TRRC", rc[2]);
            oExcuteSQL.UpdateData("STN_ConnectorResult", "Section", sectionUID, "TRStirrup", rc[3]);

            oExcuteSQL.UpdateData("STN_ConnectorResult", "Section", sectionUID, "BHStartMember", (BHStart+1).ToString());
            oExcuteSQL.UpdateData("STN_ConnectorResult", "Section", sectionUID, "BHEndMember", (BHEnd-1).ToString());
        }

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
                x1 = string.Format("{0:0.000}", 0 + (Radius + Offset)  * Math.Cos(Math.PI / 2 - Angle * Joint));
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

            if(JM == "")
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
            
            if(JM == "") //頂圓與側牆frame的名稱
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
            

            input.PutDataToSheet("Joint Coordinates", CoorExcel);
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
            input.PutDataToSheet("Joint Restraint Assignments", JointRestraint);
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
            input.PutDataToSheet("Connectivity - Frame", Members);
            input.PutDataToSheet("Frame Output Station Assigns", Output);
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
                if (Z == 0) LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-CornerH,None");
                else
                if (Math.Abs(X) == Radius && Z == -SpringOffset) LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-CornerV,None");
                else
                if (Z == -SpringOffset) LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Base,None");
                else
                if (Math.Abs(X) == Radius + SpringOffset && Z == High) LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Tangent,None");
                else
                if (Math.Abs(X) == Radius + SpringOffset) LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Side,None");
                else
                    LinkProperty.Add($"{LL},Gap,TwoJoint,Spring-Vault,None");
            }
            input.PutDataToSheet("Connectivity - Link", Links);
            input.PutDataToSheet("Link Property Assignments", LinkProperty);
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
            input.PutDataToSheet("Link Props 01 - General", Link);
        }

        public void Link_Props_05_Gap()      //Link Props 05 - Gap
        {
            verticalStress.VerticalStress("CONNECTOR", out string outlstr, out string sstr, out string surstr, out double longtermE1, out double shE, out double pv, out double lph1, out double lph2, out double shp1, out double shp2, out double u12);  //segmentYoungsModulus
            double Em = longtermE1; 

            double Pr = u12;

            double Ks = Em * (1 - Pr) / Radius / (1 + Pr) / (1 - 2 * Pr);
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
            input.PutDataToSheet("Link Props 05 - Gap", LinkProperty);
        }
        #endregion

        #region Frame Section Propertis 01 - General ; Frame Section Propertis 02 - Concrete Col 
        //public void Frame_Props_01_Generall_AND_Frame_Props_02_Concrete_Col()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        //{
        //    //List<string> Segment = p.ConnectTunnelSegment;
        //    //STN_ConnectorFrameData= $"{Name}, {Shape}, {Width}, {Height}, {Locate}, {MaterialName}, {FC}";
        //    List<string> XXX = new List<string>();
        //    for (int j = 0; j < Segment.Count; ++j)
        //    {
        //        string[] JJ = Segment[j].Split(',');
        //        FramePropperty.Add($"{JJ[0]},{JJ[5]},{JJ[1]},{JJ[3]},1"); //MaterialName,Material,Shape,Height,1
        //        XXX.Add($"{JJ[0]},A615Gr60,A615Gr60,{JJ[1]},Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
        //    }
        //    input.PutDataToSheet("Frame Props 01 - General", FramePropperty);
        //    input.PutDataToSheet("Frame Props 02 - Concrete Col", XXX);
        //}
        #endregion

        //#region Frame Section Assignments
        ////1080623 ********************************************
        //public void Frame_Section_Assignments()      //Frame Section Assignments
        //{
        //    List<string> FrameAssignments = new List<string>();
        //    string[] FP1 = FramePropperty[0].Split(',');  //頂板及側牆  //MaterialName,Material,Shape,Width,1
        //    string[] FP2 = FramePropperty[1].Split(',');  //底板        //MaterialName,Material,Shape,Width,1
        //    for (int j = 0; j <  LinkProperty.Count; ++j) //L1,	Gap, TwoJoint, Spring-Vault, None
        //    {   //確認桿件的位置       
        //        string[] JJ = LinkProperty[j].Split(',');
        //        string LL = JJ[0].Substring(1).Replace("L", "");
        //        LL = LL.Substring(0).Replace("V", "");
        //        string xxx = "=" + JJ[3].ToString().Trim(); 
        //        switch (JJ[3].ToString().Trim())
        //        {
        //            case "Spring-Base":  //底板
        //                FrameAssignments.Add($"{LL},{FP2[2]}, N.A.,{FP2[0]},{FP2[0]},Default");
        //                break;
        //            case "Spring-CornerH":
        //                break;
        //            default:
        //                FrameAssignments.Add($"{LL},{FP1[2]}, N.A.,{FP1[0]},{FP1[0]},Default");
        //                break;
        //        }              
        //    }
        //    //1,Rectangular,N.A.,Segment 100x30cm,Segment 100x30cm,Default
        //    input.PutDataToSheet("Frame Section Assignments", FrameAssignments);
        //}
        //#endregion

        #region MatProp 01 - General; MatProp 02 - Basic Mech Props; MatProp 03b - Concrete Data
        //public void MatProp_01_General_AND_MatProp_02_BasicAnd_Mech_Props_AND_MatProp_03b_Concrete_Data()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        //{
        //    DataTable dt = oExcuteSQL.GetBySection("STN_ConnectorFrameData", sectionUID, "");
        //    DataTable dx = dt.DefaultView.ToTable(true, "Material");
        //    List<string> AAA = new List<string>();
        //    List<string> BBB = new List<string>();
        //    List<string> CCC = new List<string>();
        //    for (int j = 0; j < dx.Rows.Count; ++j)
        //    {           
        //        string materialUID =dx.Rows[j]["Material"].ToString();
        //        DataTable dm = oExcuteSQL.GetByUID("STN_SegmentMaterial", materialUID);
        //        if (dm.Rows.Count > 0)
        //        {   //應該只有一筆資料
        //            DataRow dr = dm.Rows[0];
        //            string MName = dr["MaterialName"].ToString();
        //            AAA.Add($"{MName},Concrete,Isotropic,No");
        //            float UnitWeight = float.Parse(dr["UnitWeight"].ToString());
        //            float UnitMass = float.Parse(dr["UnitWeight"].ToString())/Convert.ToSingle(p.newton);
        //            BBB.Add($"{MName},{UnitWeight},{UnitMass},{dr["YoungModulus"].ToString()},,{dr["PoissonRatio"].ToString()},{dr["CTE"].ToString()}");

        //            //float Fc = float.Parse(dr["Fc"].ToString()) / Convert.ToSingle(p.newton);
        //            float Fc = float.Parse(dr["Fc"].ToString());

        //            CCC.Add($"{MName},{Fc},No,Mander,Takeda,0.00221914,0.005,-0.1,0,0");
        //        }
        //    }
        //    input.PutDataToSheet("MatProp 01 - General", AAA);
        //    input.PutDataToSheet("MatProp 02 - Basic Mech Props", BBB);
        //    input.PutDataToSheet("MatProp 03b - Concrete Data", CCC);
        //}
        #endregion

        #region Frame Loads - Distributed
        public void Frame_Loads_Distributed_AND_Frame_Section_Assignments()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        {
            List<string> FrameAssignments = new List<string>();
            //string[] FP1 = FramePropperty[0].Split(',');  //頂板及側牆  //MaterialName,Material,Shape,Width,1
            //string[] FP2 = FramePropperty[1].Split(',');  //底板        //MaterialName,Material,Shape,Width,1

            List<string> HLoads = new List<string>(); //水平載重
            List<string> VLoads = new List<string>(); //垂直載重
            double Pv = verticalStress.connectorNPv;
                       
            //Pv = 664.08;
            EQLoad(ref Pv);

            for (int j = 0; j < Members.Count; ++j)
            {   //確認桿件的位置       
                string[] JJ = Members[j].Split(',');
                string LL = JJ[0];
                //  LL = LL.Substring(0).Replace("V", "");
                //string xxx = "=" + JJ[3].ToString().Trim();
                string v1 = "";     //應力值
                string v2 = "";    //應力值
                string type = "";  //回傳是牆(WALL)或底板(BASE)
                LoadCalculate(JJ[1].Trim(),JJ[2].Trim(), ref v1, ref v2, ref type);

                if(type != "BASE")
                {
                    double tempV1 = double.Parse(v1);
                    double tempV2 = double.Parse(v2);
                    EQLoad(ref tempV1);
                    EQLoad(ref tempV2);
                    v1 = tempV1.ToString();
                    v2 = tempV2.ToString();
                }
                

                switch (type)
                {
                    case "BASE":
                        FrameAssignments.Add($"{LL},Rectangular,N.A.,{BHframeName},{BHframeName},Default");
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{Pv},{Pv}");//垂直載重
                        break;
                    case "WALL":
                        FrameAssignments.Add($"{LL},Rectangular,N.A.,{TRframeName},{TRframeName},Default");
                        HLoads.Add($"{LL},Soil Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{v1},{v2}"); //水平載重
                        break;
                    default :
                        FrameAssignments.Add($"{LL},Rectangular,N.A.,{TRframeName},{TRframeName},Default");
                        HLoads.Add($"{LL},Soil Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{v1},{v2}"); //水平載重
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{-Pv},{-Pv}");//垂直載重
                        break;                       
                }
            }
            input.PutDataToSheet("Frame Section Assignments", FrameAssignments);
            input.PutDataToSheet("Frame Loads - Distributed", VLoads);
            input.PutDataToSheet("Frame Loads - Distributed", HLoads);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="type">回傳是牆(WALL)或底板(BASE)</param>
        public void LoadCalculate(string L1, string L2, ref string v1, ref string v2, ref string type)
        {
            double Ph1 = verticalStress.connectorNPh1;
            double Ph2 = verticalStress.connectorNPh2;
            float HH = Radius + High;  //聯絡通道總高

            //Ph1 += 4.5 * 9.81;
            //Ph2 += 4.5 * 9.81;

            //Ph1 = 510.25;
            //Ph2 = 559.62;

            float X1 = 0;
            float Z1 = 0;
            float X2 = 0;
            float Z2 = 0;

            for (int j = 0; j < Joints.Count; ++j) //L1,	Gap, TwoJoint, Spring-Vault, None
            {   //       
                string[] JJ = Joints[j].Split(',');
                //  float Z = float.Parse(JJ[6]);

                if (L1 == JJ[0].Trim())
                {
                    X1 = float.Parse(JJ[3]);
                    Z1 = float.Parse(JJ[6]);
                    v1 = string.Format("{0:0.0000}", Ph1 + (Ph2 - Ph1) / HH * (HH - Z1));
                }
                if (L2 == JJ[0].Trim())
                {
                    X2 = float.Parse(JJ[3]);
                    Z2 = float.Parse(JJ[6]);
                    v2 = string.Format("{0:0.0000}", Ph1 + (Ph2 - Ph1) / HH * (HH - Z2));
                }
            }
            if (Z1 == Z2)
            {
                v1 = "";
                v2 = "";
                type = "BASE";
                return;
            }
            if (X1 == X2) type = "WALL";
            if (X1 > 0 || X2 > 0)
            {
                v1 = "-" + v1;
                v2 = "-" + v2;
            }
        }

        public void EQLoad(ref double loadValue)
        {
            double PBOT = verticalStress.PvBot;
            double unitWeight = verticalStress.soilavgUW;
            double ODE = 0.05625 * PBOT;
            double MDE = 0.09 * PBOT;

            bool minus = false;
            if (loadValue < 0)
            {
                loadValue *= -1;
                minus = true;
            }

            switch (p.connector.LoadType.ToUpper().ToString())
            {
                case "NORMAL": loadValue *= 1.7; break;
                case "MDE": loadValue += MDE; break;
                case "ODE": loadValue = 0.75 * (1.4 * loadValue + 1.87 * ODE); break;
            }

            if (minus) loadValue *= -1;
        }
        #endregion

        #region Set Frame & Material
        string TRframeName = "";
        string BHframeName = "";
        public void SetFrameMaterial()
        {
            DataTable mat = oExcuteSQL.GetBySection("STN_FrameMaterial", sectionUID, "ORDER BY TIMES ASC");
            double BHdepth = 0;
            double TRdepth = 0;
                        
            for(int i = 0; i < mat.Rows.Count; i++)
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

            input.PutDataToSheet("MatProp 01 - General", Mat01);
            input.PutDataToSheet("MatProp 02 - Basic Mech Props", Mat02);
            input.PutDataToSheet("MatProp 03b - Concrete Data", Mat03b);

            List<string> frame01 = new List<string>();
            List<string> frame02 = new List<string>();
            List<string> frameName = new List<string>();
            List<double> inputDepth = new List<double>();
            inputDepth.Add(TRdepth);
            inputDepth.Add(BHdepth);
            frameName.Add($"Frame {width * 100}x{TRdepth * 100}cm");
            frameName.Add($"Frame {width * 100}x{BHdepth * 100}cm");
            for(int i = 0; i < frameName.Count; i++)
            {
                frame01.Add($"{frameName[i]},{matName},Rectangular,{inputDepth[i]},{width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                frame02.Add($"{frameName[i]},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
            }
            TRframeName = frameName[0];
            BHframeName = frameName[1];

            input.PutDataToSheet("Frame Props 01 - General", frame01);
            input.PutDataToSheet("Frame Props 02 - Concrete col", frame02);
            
        }
        #endregion


        #region TEMP

        #endregion

        #region SAPCalculation
        SAP2000v15.SapObject mySapObject;
        SAP2000v15.cSapModel mySapModel;

        IWorkbook wb;
        int ret;
        int num = 0;
        System.String[] obj = new string[1];
        System.Double[] ObjSta = new double[1];
        System.String[] elm = new string[1];
        System.Double[] ElmSta = new double[1];
        System.String[] LoadCase = new string[1];
        System.String[] StepType_test = new string[1];
        System.Double[] StepNum_test = new double[1];
        System.Double[] P = new double[1];
        System.Double[] V2 = new double[1];
        System.Double[] V3 = new double[1];
        System.Double[] T = new double[1];
        System.Double[] M2 = new double[1];
        System.Double[] M3 = new double[1];
        System.Double[] U1 = new double[1];
        System.Double[] U2 = new double[1];
        System.Double[] U3 = new double[1];
        System.Double[] R1 = new double[1];
        System.Double[] R2 = new double[1];
        System.Double[] R3 = new double[1];
        double MaxM = 0;
        double MaxP = 0;
        string OutUID = "";
        double MaxV = 0;
        /*
        double shearMaxV = 0;
        double shearMaxP = 0;
        string shearOutUID = "";
        */
        List<string> loadingUID = new List<string>();
        List<Tuple<double, double, double, double, double, double>> loadingValue = new List<Tuple<double, double, double, double, double, double>>();
        List<Tuple<string, string, string>> loadingProp = new List<Tuple<string, string, string>>();
        List<Tuple<string, string>> jointProp = new List<Tuple<string, string>>();
        List<Tuple<double, double, double, double, double, double>> jtDisplacement = new List<Tuple<double, double, double, double, double, double>>();

        public void SAPCalculation(string inputPath, string loadingName, string outputPath, List<string> frameName, out bool getResult, string condition, int realTimes)
        {
            //loadingName = "DL+SOIL(NL)";
            loadingUID.Clear();
            loadingValue.Clear();
            loadingProp.Clear();
            jointProp.Clear();
            jtDisplacement.Clear();


            bool temp_bool = true;
            mySapObject = new SAP2000v15.SapObject();
            mySapModel = mySapObject.SapModel;
            mySapObject.ApplicationStart(SAP2000v15.eUnits.kip_ft_F, temp_bool, "");

            ret = mySapModel.File.OpenFile(inputPath);
            ret = mySapModel.Analyze.RunAnalysis();

            ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
            ret = mySapModel.Results.Setup.SetCaseSelectedForOutput(loadingName, true);

            wb = new XSSFWorkbook();            

            for(int i = 0; i < frameName.Count; i++)
            {
                ret = mySapModel.Results.FrameForce(frameName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref ObjSta, ref elm, ref ElmSta, ref LoadCase, ref StepType_test, ref StepNum_test, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

                var forceProp = Tuple.Create(frameName[i], loadingName, frameName[i]);
                loadingProp.Add(forceProp);

                try
                {
                    forceProp = Tuple.Create(frameName[i], loadingName, frameName[i + 1]);
                    loadingProp.Add(forceProp);
                }
                catch
                {
                    forceProp = Tuple.Create(frameName[i], loadingName, frameName[0]);
                    loadingProp.Add(forceProp);
                }

                for(int j = 0; j < 2; j++)
                {
                    var forceValue = Tuple.Create(P[j], V2[j], V3[j], T[j], M2[j], M3[j]);
                    loadingValue.Add(forceValue);
                    loadingUID.Add(Guid.NewGuid().ToString("D"));
                }
                ret = mySapModel.Results.JointDispl(frameName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref elm, ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                var dispProp = Tuple.Create(frameName[i], loadingName);
                jointProp.Add(dispProp);
                var dispValue = Tuple.Create(U1[0], U2[0], U3[0], R1[0], R2[0], R3[0]);
                jtDisplacement.Add(dispValue);
            }

            input.CreateResultFile(wb, loadingProp, loadingValue, loadingName);
            input.CreateDisplacementFile(wb, jointProp, jtDisplacement, "JointDisplacement");
            input.FileSaving(wb, outputPath);

            getResult = true;//0.33直徑與地震載重使用，只有當直徑變化過目標值後再擷取分析結果，

            if (getResult)
            {             
                try
                {
                    oExcuteSQL.DeleteDataBySectionUIDAndTimes($"STN_{condition}Data", sectionUID, realTimes);
                }
                catch
                {
                }
                oExcuteSQL.InsertSAPData($"STN_{condition}Data", loadingUID, sectionUID, loadingProp, realTimes, loadingValue);
            }

            mySapObject.ApplicationExit(true);
        }

        public List<Tuple<string, double, double, double>> SAPChosenData(string type, List<string> frameName, List<string> chosenFrame, string MorV)
        {
            MaxP = 0;
            MaxV = 0;
            MaxM = 0;
            OutUID = "";
            for(int i = 0; i < frameName.Count; i++)
            {
                for(int j = 0; j < chosenFrame.Count; j++)
                {
                    if(frameName[i] == chosenFrame[j])
                    {
                        switch (MorV.ToUpper().ToString())
                        {
                            case "MAIN":
                                {                                    
                                    for(int k = 0; k < 2; k++)
                                    {
                                        if (frameName[i] == BHStart.ToString())
                                        {
                                            if (k == 1 && type == "TR") continue;
                                        }                                            
                                        else if (frameName[i] == BHEnd.ToString())
                                            if (k == 0 && type == "TR") continue;

                                        bool main = false;
                                        if (Math.Abs(loadingValue[i * 2 + k].Item6) > MaxM)
                                            main = true;
                                        else if (Math.Abs(loadingValue[i * 2 + k].Item6) == MaxM && Math.Abs(loadingValue[i * 2 + k].Item1) < MaxP)
                                            main = true;

                                        if (main)
                                        {
                                            MaxP = Math.Abs(loadingValue[i * 2 + k].Item1);
                                            MaxV = Math.Abs(loadingValue[i * 2 + k].Item2);
                                            MaxM = Math.Abs(loadingValue[i * 2 + k].Item6);                                            
                                            OutUID = loadingUID[i * 2 + k];
                                        }
                                    }
                                }break;
                            case "SHEAR":
                                {
                                    
                                    for (int k = 0; k < 2; k++)
                                    {
                                        if (frameName[i] == BHStart.ToString())
                                        {
                                            if (k == 1 && type == "TR") continue;
                                        }
                                        else if (frameName[i] == BHEnd.ToString())
                                            if (k == 0 && type == "TR") continue;

                                        bool shear = false;
                                        if (Math.Abs(loadingValue[i * 2 + k].Item2) > MaxV)
                                            shear = true;
                                        else if (Math.Abs(loadingValue[i * 2 + k].Item2) == MaxV && Math.Abs(loadingValue[i * 2 + k].Item1) < MaxP)
                                            shear = true;

                                        if (shear)
                                        {
                                            MaxP = Math.Abs(loadingValue[i * 2 + k].Item1);
                                            MaxV = Math.Abs(loadingValue[i * 2 + k].Item2);
                                            MaxM = Math.Abs(loadingValue[i * 2 + k].Item6);
                                            OutUID = loadingUID[i * 2 + k];
                                        }
                                    }
                                }break;
                        }
                        break;
                    }
                }
            }
            List<Tuple<string, double, double, double>> outData = new List<Tuple<string, double, double, double>>();
            outData.Add(Tuple.Create(OutUID, MaxP, MaxV, MaxM));
            return outData;
        }
        #endregion
    }
}
