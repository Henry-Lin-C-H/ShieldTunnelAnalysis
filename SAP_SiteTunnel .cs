using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using SegmentCalcu;
//using SAP2000v15;
using SAP2000v1;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using STN_SQL;
using SinoTunnelFile;

namespace SinoTunnel
{
    class SAP_SiteTunnel
    {   // 載重計算 及 資料置入 EXCEL 
        SAP_ExcelInput input;
        ExcuteSQL oExcuteSQL = new ExcuteSQL();
        GetWebData p;
        STN_VerticalStress verticalStress;
        Excel2Word word = new Excel2Word();
        SinoTunnelFile.UploadFile oUploadFile = new UploadFile();

        string sectionUID = "";
        string DefaultfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Normal_Case_Raw.xlsx";
        string outputPath = "";
        string inputgPath = "";

        float Thick = 0;          // p.segmentThickness;
        float Radius = 0;         // p.segmentRadiusIn;   
        float UseRadius = 0;      // = Radius + Thick / 2 ;       
        //float Aangle = 0.0f;    // A 環片夾角    
        //float Bangle = 0.0f;    // B 環片夾角         
        float Kangle = 15.0f;    // K 環片夾角  
        float ADangle = 0.0f;   // 螺栓孔夾角  
        float AD1angle = 0.0f;     // 螺栓孔夾角 / 2 = 18 之兩側之夾角  
        float AD2angle = 0.0f;    // (螺栓孔夾角-K環片夾角)/2  之另一側之夾角 
        int  iPoints = 0;


        List<string> Joints = new List <string>();  //第一環片桿件點位     
        List<string> Springs = new List<string>();  //第一環片彈簧點位
        List<string> Members = new List<string>();  //第一環片桿件

        Coord[] JointsCoord;
        List<string> JointsTheta = new List<string>();
        List<string> SpringLinks = new List<string>();     //彈簧桿件
        List<string> LinkProperty = new List<string>();    //彈簧桿件性質
        //List<string> FramePropperty = new List<string>();  //桿件混凝土強度值

        double Fc;
        double E;
        double U12;
        double UW;
        double width;
        public SAP_SiteTunnel(string sectionUID)
        {
            input = new SAP_ExcelInput(DefaultfilePath);
            this.sectionUID = sectionUID;
            this.p = new GetWebData(sectionUID);  //  取得網路變數資料
            verticalStress = new STN_VerticalStress(sectionUID, "WEBFORM");  //取得 土層楊氏係數及

            this.Fc = p.segmentFc;
            this.E = p.segmentYoungsModulus;
            this.U12 = p.segmentPoissonRatio;
            this.UW = p.segmentUnitWeight;
            this.width = p.segmentWidth;
        }

        List<string> frameName = new List<string>();
        public void Process(string xfileSavingPath, string condition, bool excelOnly)
        {
            inputgPath = xfileSavingPath.Replace(".xlsx", $"_{condition}Input1st.xlsx");
            outputPath = xfileSavingPath.Replace(".xlsx", $"_{condition}ResultFinal.xlsx");

            Radius = float.Parse(p.segmentRadiusIn.ToString());
            Thick = float.Parse(p.segmentThickness.ToString());
            UseRadius = Radius + Thick / 2;
           //Aangle = float.Parse(p.segmentAAngle.ToString());
           //Bangle = float.Parse(p.segmentBAngle.ToString());
            Kangle = float.Parse(p.segmentKAngle.ToString());
            ADangle = float.Parse(p.segmentAdjacentPoreAngle.ToString());

            Joint_Coordinates();
            Joint_Restraint_Assignments();
            Connectivity_Frame_AND_Frame_Output_Station_Assigns();
            Link_Props_01_General_AND_Link_Props_05_Gap();
            Connectivity_Link_And_Link_Property_Assignments();
            //Frame_Props_01_Generall_AND_Frame_Props_02_Concrete_Col();
            SetFrameMaterial();
            Frame_Section_Assignments();  
           //  Frame_Releases1_General();
            //MatProp_01_General_AND_MatProp_02_BasicAnd_Mech_Props_AND_MatProp_03b_Concrete_Data();
            Frame_Loads_Distributed();
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
            string[] wordtemp = xfileSavingPath.Split('\\');
            string wordpath = "";
            for (int i = 0; i < wordtemp.Length - 1; i++) { wordpath += wordtemp[i]; wordpath += "\\"; }
            wordpath += "SiteTunnel.docx";
            //string wordpath = xfileSavingPath.Replace(".xlsx", $".docx");
            word.Add(inputgPath, names, false, false);

            if (excelOnly) return;

            SAPCalculation(inputgPath, "DL+SOIL (NL)", outputPath, frameName);

            word.Add(outputPath, names, true, false);
            word.FileSaving(wordpath);
            oUploadFile.UploadToServer(sectionUID, wordpath);

            try
            {
                oExcuteSQL.DeleteDataBySectionUID("STN_SiteResult", sectionUID);
            }
            catch
            {
            }
            oExcuteSQL.InsertData("STN_SiteResult", "Section", sectionUID);
            oExcuteSQL.UpdateData("STN_SiteResult", "Section", sectionUID, "RC", RCUID);
            oExcuteSQL.UpdateData("STN_SiteResult", "Section", sectionUID, "Stirrup", stirrupUID);
        }

        #region Loading Coordinates ; Joint Restraint Assignments 
        
        public void Joint_Coordinates()
        {
            AD1angle = ADangle / 2;               // 螺栓孔夾角 / 2 = 18 之兩側之夾角  
            AD2angle = (ADangle - Kangle) / 2;    // (螺栓孔夾角-K環片夾角)/2  之另一側之夾角              
            Joints = JointsProcess("", 0);                  // 隧道點位   
            Springs = JointsProcess("S", UseRadius);       // 隧道外彈簧點位 
            JointsThetaProcess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JM">"S" =定義為土壤彈簧 </param>
        /// <param name="High"></param>
        /// <param name="Radius"></param> 
        /// <param name="Offset">彈簧桿件支距</param>
        /// <returns></returns>
        public List<string> JointsProcess(string JM, float Offset)
        {
            List<string> XXXX = new List<string>();
            string x1 = "";
            string z1 = "";
            float xRadius = UseRadius + Offset; // Offset = 土壤彈簧支距   
            float AngleValue = 0.0f; 
            int ii = 1;  //桿件編號
            int ss = 0;
            int ee = 10;  //代表 螺栓孔數目
            if (JM != "") { ss = 1; ee = 9; } //For Partial points not for all 
            for (int jj = ss; jj  < ee ; ++jj)
            {
                AngleValue = ADangle * jj;        
                switch (jj)
                {
                    case 0:   //第一個螺栓孔位置(FOR JM=""  隧道環片點位)
                    case 9:   //第一個螺栓孔位置(FOR JM="S" 土壤彈簧點位) 
                        PolarToXY(AngleValue, xRadius, ref x1, ref z1);
                        XXXX.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");  //1
                        ++ii;
                        AngleValue += AD2angle;
                        GapeFacePoints(AngleValue, xRadius, ref ii, JM, XXXX);
                        AngleValue += Kangle;
                        GapeFacePoints(AngleValue, xRadius, ref ii, JM, XXXX);
                 
                        break;
                    default:
                        PolarToXY(AngleValue, xRadius, ref x1, ref z1);
                        if (JM != "S")
                        {
                            XXXX.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
                            ++ii;
                        }
                        else
                           if (jj != 1)
                        {
                            XXXX.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
                            ++ii;
                        }
                        AngleValue += AD1angle;
                        GapeFacePoints(AngleValue, xRadius, ref ii, JM, XXXX);               
                        break;
                }
            }
            input.PutDataToSheet("Joint Coordinates", XXXX);
            iPoints = ii;
            return XXXX;          
        }

        public void PolarToXY(float AngleValue, float Length, ref string x1, ref string z1)
        {
            x1 = string.Format("{0:0.0000}", Length * Math.Cos(Math.PI / 2 - AngleValue * Math.PI / 180));
            z1 = string.Format("{0:0.0000}", Length * Math.Sin(Math.PI / 2 - AngleValue * Math.PI / 180));
        }

        public void GapeFacePoints(float AngleValue, float Length, ref int ii, string JM, List<string> Coor)
        {
            string x1 = "";
            string z1 = "";
            float DeltaAngle = 4.0f;

            float Angles = AngleValue - DeltaAngle;
            PolarToXY(Angles, Length, ref x1, ref z1);   
            Coor.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
            ++ii;
            Angles = AngleValue;
            PolarToXY(Angles, Length, ref x1, ref z1); 
            Coor.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
            ++ii;
            Angles = AngleValue + DeltaAngle; ;
            PolarToXY(Angles, Length, ref x1, ref z1); 
            Coor.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
            ++ii;
        }

        /// <summary>
        /// 點位所代表的夾角
        /// </summary>
        public void JointsThetaProcess()
        {   // 環片點位資料
            JointsCoord = null;
            JointsCoord = new Coord[Joints.Count + 1];
            double dX = 0;
            double dY = 0;
            double multi = 0;
            for (int jj = 0; jj < Joints.Count; ++jj)
            {
                string XX = Joints[jj];
                string[] aa = XX.Split(',');
                string xJoint = aa[0].ToString();
                JointsCoord[jj + 1].XorR = double.Parse(aa[3].ToString());
                JointsCoord[jj + 1].Z = double.Parse(aa[6].ToString());
            }
            for (int jj = 1; jj < JointsCoord.Length; ++jj)
            {
                if (jj == 1)
                {
                    dX = JointsCoord[jj + 1].XorR - JointsCoord[JointsCoord.Length - 1].XorR;
                    dY = JointsCoord[jj + 1].Z - JointsCoord[JointsCoord.Length - 1].Z;
                }
                else
                {
                    if (jj == JointsCoord.Length - 1)
                    {
                        dX = JointsCoord[jj].XorR - JointsCoord[1].XorR;
                        dY = JointsCoord[jj].Z - JointsCoord[1].Z;
                    }
                    else
                    {
                        dX = JointsCoord[jj + 1].XorR - JointsCoord[jj - 1].XorR;
                        dY = JointsCoord[jj + 1].Z - JointsCoord[jj - 1].Z;
                    }
                }
                multi = dX * dX + dY * dY;
                double LL = Math.Sqrt(multi) / 2;
                JointsCoord[jj].θ = Math.Round(Math.Asin(LL / Convert.ToDouble(UseRadius)) * 2 * 180 / Math.PI / 2, 2);
                JointsTheta.Add(JointsCoord[jj].θ.ToString());
            }
        }

        public void Joint_Restraint_Assignments()
        {
            List<string> JointRestraint = new List<string>();
            for (int j = 0; j < Joints.Count ; ++j)
            {
                JointRestraint.Add($"{j+1},No,Yes,No,Yes,No,Yes");
            }
            for (int j = 0; j < Springs.Count; ++j)
            {
                JointRestraint.Add($"S{j + 1},Yes,Yes,Yes,Yes,Yes,Yes");
            }
            input.PutDataToSheet("Joint Restraint Assignments", JointRestraint);
        }

        #endregion

        #region Connectivity-Frame And Frame Output Station Assigns  
        public void Connectivity_Frame_AND_Frame_Output_Station_Assigns()
        {   //  定義桿件IJ端     
            List<string> Output = new List<string>();    
            for (int j = 1; j < Joints.Count ; ++j)
            {       
                Members.Add($"{j},{j},{j + 1},No");
                frameName.Add($"{j}");
                Output.Add($"{j},MinNumSta,2,,Yes,Yes");
            }      
            Members.Add($"{Joints.Count},{Joints.Count},1, No"); //最後一隻桿件
            frameName.Add($"{Joints.Count}");
            Output.Add($"{Joints.Count},MinNumSta,2,,Yes,Yes");//最後一隻桿件
            input.PutDataToSheet("Connectivity - Frame", Members);
            input.PutDataToSheet("Frame Output Station Assigns", Output);
        }


        public void Connectivity_Link_And_Link_Property_Assignments()
        {   //土壤彈簧桿件
            Spring_Link_And_Link_Property(9, Springs);   //第一環片土壤彈簧連結
          //  Spring_Link_And_Link_Property(55, Spring2s);  //第二環片土壤彈簧連結 
            input.PutDataToSheet("Connectivity - Link", SpringLinks);
        }

        public void Spring_Link_And_Link_Property(int ii, List<string> xSprings)
        {   //土壤彈簧桿件  
            List<string> LinkPy = new List<string>();
            for (int j = 0; j < xSprings.Count; ++j)
            {
                string[] JJ = xSprings[j].Split(',');
                string LL = "L" + JJ[0].Substring(1);
                SpringLinks.Add($"{LL}, {ii}, {JJ[0]}");
                int jj = ii;
                if (jj > Joints.Count) jj = jj - Joints.Count;
                string XX = Convert.ToString(JointsCoord[jj].θ);
                string result = LinkProperty.FirstOrDefault(s => s.Contains(XX));
                LinkPy.Add($"{LL},Gap,TwoJoint,{result},None");
                ++ii;
            }
            input.PutDataToSheet("Link Property Assignments", LinkPy);
        }


        //public void Connectivity_Link_And_Link_Property_Assignments()
        //{   //土壤彈簧桿件
        //    List<string> LinkPy = new List<string>();
        //    int ii = 9;  //由第9點開始連結
        //    for (int j = 0; j < Springs.Count; ++j)
        //    {   // Connectivity_Link
        //        string[] JJ = Springs[j].Split(',');                
        //        string LL = "L" + JJ[0].Substring(1);
        //        SpringLinks.Add($"{LL}, {ii}, {JJ[0]}");
        //        switch (ii % 4)
        //        {
        //            case 0:
        //                LinkPy.Add($"{LL},Gap,TwoJoint,{LinkProperty[2]},None");
        //                break;
        //            case 1:
        //            case 3:
        //                LinkPy.Add($"{LL},Gap,TwoJoint,{LinkProperty[0]},None");
        //                break;
        //            case 2:
        //                LinkPy.Add($"{LL},Gap,TwoJoint,{LinkProperty[1]},None");
        //                break;

        //        }
        //        ++ii;
        //    }
        //    input.PutDataToSheet("Connectivity - Link", SpringLinks);
        //    input.PutDataToSheet("Link Property Assignments", LinkPy); //L2,Gap,TwoJoint,Spring-Theta=9,None
     // }
        #endregion

        #region Link Props 01 - General; Link Props 05 - Gap ; Link Property Assignments
        /// <summary>
        /// 土壤彈簧連桿性質
        /// </summary>
        public void Link_Props_01_General_AND_Link_Props_05_Gap()      //Link Props 05 - Gap
        {        
            List<string> LinkProperty05 = new List<string>();
            List<string> LinkProperty01 = new List<string>();
            float AngleValue = 0;

            verticalStress.VerticalStress("Tunnel", out string outlstr, out string sstr, out string surstr, out double longtermE1, out double shE, out double pv, out double lph1, out double lph2, out double shp1, out double shp2, out double u12);  //segmentYoungsModulus
            double Em = longtermE1;
            double Pr = u12;
            double Ks = Em * (1 - Pr) / UseRadius / (1 + Pr) / (1 - 2 * Pr);

            List<string> ThataDistinct = JointsTheta.Distinct().ToList();

            for (int jj = 0; jj < ThataDistinct.Count; ++jj)
            {
                string XX = ThataDistinct[jj];
                AngleValue = float.Parse(XX);
                LinkPropertyProcess(AngleValue, Ks, LinkProperty05, LinkProperty01);
            }
            input.PutDataToSheet("Link Props 05 - Gap", LinkProperty05);//Spring-Theta=9,U1,No,Yes,11356.83,0,11356.83,0
            input.PutDataToSheet("Link Props 01 - General", LinkProperty01); //Spring-Theta=9,Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark		
        }

        public void LinkPropertyProcess(float AngleValue, double Ks, List<string> LinkProperty05, List<string> LinkProperty01)
        {
            double KKK = 0;
            string XXX = "";
            KKK = Ks * 1.0 * UseRadius * AngleValue * Math.PI / 180;
            XXX = string.Format("{0:0.00}", KKK);
            LinkProperty.Add($"Spring-Theta={AngleValue}");
            LinkProperty05.Add($"Spring-Theta={AngleValue},U1,No,Yes,{XXX},0,{XXX},0");
            LinkProperty01.Add($"Spring-Theta={AngleValue},Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark");
        }



        //public void Link_Props_01_General_And_Link_Props_05_Gap()      //Link Props 05 - Gap
        //{
        //    List<string> LinkProperty05 = new List<string>();
        //    List<string> LinkProperty01 = new List<string>();
        //    float AngleValue = 0;

        //    verticalStress.VerticalStress("Tunnel", out string outlstr, out string sstr, out string surstr, out double longtermE1, out double shE, out double pv, out double lph1, out double lph2, out double shp1, out double shp2, out double u12);  //segmentYoungsModulus
        //    double Em = longtermE1;
        //    double Pr = u12;
        //    double Ks = Em * (1 - Pr) / UseRadius / (1 + Pr) / (1 - 2 * Pr);

        //    AngleValue = AD1angle / 2;  //第9點 或 11點兩側  9.0度
        //    LinkPropertyProcess(AngleValue, Ks, LinkProperty05, LinkProperty01);

        //    AngleValue = 4;            //第10點兩側   4度
        //    LinkPropertyProcess(AngleValue, Ks, LinkProperty05, LinkProperty01);

        //    AngleValue = (ADangle - 4 * 2) / 2;  //第12點兩側  14.0度
        //    LinkPropertyProcess(AngleValue, Ks, LinkProperty05, LinkProperty01);

        //    input.PutDataToSheet("Link Props 05 - Gap", LinkProperty05);//Spring-Theta=9,U1,No,Yes,11356.83,0,11356.83,0
        //    input.PutDataToSheet("Link Props 01 - General", LinkProperty01); //Spring-Theta=9,Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark		
        //}

        //public void LinkPropertyProcess(float AngleValue, double Ks, List<string> LinkProperty05, List<string> LinkProperty01)     
        //{   //土壤彈簧連桿 K值 
        //    double KKK = 0;
        //    string XXX = "";       
        //    KKK = Ks * 1.0 * UseRadius * AngleValue * Math.PI / 180;
        //    XXX = string.Format("{0:0.00}", KKK);
        //    LinkProperty.Add($"Spring-Theta={AngleValue}");
        //    LinkProperty05.Add($"Spring-Theta={AngleValue},U1,No,Yes,{XXX},0,{XXX},0");
        //    LinkProperty01.Add($"Spring-Theta={AngleValue},Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark");
        //}

        #endregion

        #region Frame Section Propertis 01 - General ; Frame Section Propertis 02 - Concrete Col 
        //public void Frame_Props_01_Generall_AND_Frame_Props_02_Concrete_Col()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        //{
        //    Material Segment = p.Material;
        //    Segment Frame = p.Frame;
        //    List<string> XXX = new List<string>();
        //    FramePropperty.Add($"{Frame.Name},{Segment.Name},{Frame.Shape},{Frame.Height},{Frame.Width}"); //MaterialName,Material,Shape,Width,1
        //    XXX.Add($"{Frame.Name},A615Gr60,A615Gr60,{Frame.Shape},Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
   
        //    input.PutDataToSheet("Frame Props 01 - General", FramePropperty);
        //    input.PutDataToSheet("Frame Props 02 - Concrete Col", XXX);
        //}
        #endregion

        #region Frame Section Assignments
        //1080623 ********************************************
        public void Frame_Section_Assignments()      //Frame Section Assignments
        {
            List<string> FrameAssignments = new List<string>();
            //string[] FP1 = FramePropperty[0].Split(',');  //MaterialName,Material,Shape,Width,1     
            for (int j = 0; j <  Members.Count; ++j) //L1,	Gap, TwoJoint, Spring-Vault, None
            {   //確認桿件的位置       
                string[] JJ = Members[j].Split(',');
                 FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{framePropName},{framePropName},Default");           
            }
            //1,Rectangular,N.A.,Segment 100x30cm,Segment 100x30cm,Default
            input.PutDataToSheet("Frame Section Assignments", FrameAssignments);
        }

        //// 兩環片間之連桿 ; Frame Releases 1 - General  
        //public void Frame_Releases1_General()      //Frame Section Assignments
        //{
        //    List<string> Releases = new List<string>();
        //    //兩環片間之連桿
        //    for (int j = 0; j < Links.Count; ++j)
        //    {
        //        string[] JJ = Links[j].Split(',');
        //        Releases.Add($"{JJ[0]},Yes,No,No,No,No,No,No,No,No,Yes,No,No,No");
        //    }
        //    input.PutDataToSheet("Frame Releases 1 - General", Releases);
        //}

        #endregion

        #region MatProp 01 - General; MatProp 02 - Basic Mech Props; MatProp 03b - Concrete Data
        
        //public void MatProp_01_General_AND_MatProp_02_BasicAnd_Mech_Props_AND_MatProp_03b_Concrete_Data()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        //{
        //    Material Segment = p.Material;
        //    Segment Frame = p.Frame;

        //    List<string> AAA = new List<string>();  //MatProp 01
        //    List<string> BBB = new List<string>();  //MatProp 02
        //    List<string> CCC = new List<string>();  //MatProp 03b
          
        //    string materialUID = Frame.MaterialUID;
        //    DataTable dm = oExcuteSQL.GetByUID("STN_SegmentMaterial", materialUID);
        //    if (dm.Rows.Count > 0)
        //    {   //應該只有一筆資料
        //        DataRow dr = dm.Rows[0];
        //        string MName = dr["MaterialName"].ToString();
        //        AAA.Add($"{MName},Concrete,Isotropic,No");
        //        float UnitWeight = float.Parse(dr["UnitWeight"].ToString());
        //        float UnitMass = float.Parse(dr["UnitWeight"].ToString()) / Convert.ToSingle(p.newton);
        //        BBB.Add($"{MName},{UnitWeight},{UnitMass},{dr["YoungModulus"].ToString()},,{dr["PoissonRatio"].ToString()},{dr["CTE"].ToString()}");
        //        float Fc = float.Parse(dr["Fc"].ToString()) / Convert.ToSingle(p.newton);
        //        CCC.Add($"{MName},{Fc},No,Mander,Takeda,0.00221914,0.005,-0.1,0,0");
        //    }

        //    input.PutDataToSheet("MatProp 01 - General", AAA);
        //    input.PutDataToSheet("MatProp 02 - Basic Mech Props", BBB);
        //    input.PutDataToSheet("MatProp 03b - Concrete Data", CCC);
        //}
        #endregion

        #region Frame Loads - Distributed
        public void Frame_Loads_Distributed()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        {
            List<string> HLoads = new List<string>(); //水平載重
            List<string> VLoads = new List<string>(); //垂直載重
            double Pv = verticalStress.PvTop;
            for (int j = 0; j < Members.Count; ++j)
            {   //確認桿件的位置       
                string[] JJ = Members[j].Split(',');
                string LL = JJ[0];
                string v1 = "";    //應力值
                string v2 = "";    //應力值
                string type = "";  //回傳是牆(WALL)或底板(BASE)
                LoadCalculate(JJ[1].Trim(),JJ[2].Trim(), ref v1, ref v2, ref type);
                HLoads.Add($"{LL},Soil Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{v1},{v2}"); //水平載重
                switch (type)
                {
                    case "DOWN":
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{Pv},{Pv}");//垂直載重
                        break;
                    case "UP":
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{-Pv},{-Pv}");//垂直載重
                        break;
                    default :                 
                        break;                       
                }
            }
            input.PutDataToSheet("Frame Loads - Distributed", VLoads);
            input.PutDataToSheet("Frame Loads - Distributed", HLoads);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L1">桿件第一點位</param>
        /// <param name="L2">桿件第二點位</param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="type">回傳是牆(WALL)或底板(BASE)</param>
        public void LoadCalculate(string L1, string L2, ref string v1, ref string v2, ref string type)
        {
            double Ph1 = verticalStress.LongTermPh1;
            double Ph2 = verticalStress.LongTermPh2;
            float HH = UseRadius * 2;  
            float X1 = 0;
            float Z1 = 0;
            float X2 = 0;
            float Z2 = 0;

            for (int j = 0; j < Joints.Count; ++j) //L1,	Gap, TwoJoint, Spring-Vault, None
            {   //       
                string[] JJ = Joints[j].Split(',');     
                if (L1 == JJ[0].Trim())
                {
                    X1 = float.Parse(JJ[3]);
                    Z1 = float.Parse(JJ[6]);
                    v1 = string.Format("{0:0.00}", Ph1 + (Ph2 - Ph1) / HH * (UseRadius - Z1)); //圓心Z座標為0
                }
                if (L2 == JJ[0].Trim())
                {
                    X2 = float.Parse(JJ[3]);
                    Z2 = float.Parse(JJ[6]);
                    v2 = string.Format("{0:0.00}", Ph1 + (Ph2 - Ph1) / HH * (UseRadius - Z2));//圓心Z座標為0
                }
            }

            if (Z1 > 0 || Z2 > 0)            
                type = "UP";                      
            else
                type = "DOWN";  


            if (X1 > 0 || X2 > 0)
            {
                v1 = "-" + v1;
                v2 = "-" + v2;
            }
        }
        #endregion

        #region Set Material Frame
        private string framePropName = "";
        public void SetFrameMaterial()
        {
            DataTable mat = oExcuteSQL.GetBySection("STN_FrameMaterial", sectionUID, "");
            double depth = double.Parse(mat.Rows[0]["Depth"].ToString());

            string matName = "";
            matName = $"Concrete {Fc}";
            List<string> Mat01 = new List<string>();
            List<string> Mat02 = new List<string>();
            List<string> Mat03b = new List<string>();
            Mat01.Add($"{matName},Concrete,Isotropic,No");
            Mat02.Add($"{matName},{UW},{UW / p.newton},{E},{E / 2 / (1 + U12)},{U12},{9.9E-6}");
            Mat03b.Add($"{matName},{Fc * 98},No,Mander,Takeda,0.0021914,0.005,-0.1,0,0");

            input.PutDataToSheet("MatProp 01 - General", Mat01);
            input.PutDataToSheet("MatProp 02 - Basic Mech Props", Mat02);
            input.PutDataToSheet("MatProp 03b - Concrete Data", Mat03b);
                        
            framePropName = $"Segment {width * 100}x{depth * 100}cm";
            List<string> frame01 = new List<string>();
            List<string> frame02 = new List<string>();
            frame01.Add($"{framePropName},{matName},Rectangular,{depth},{width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
            frame02.Add($"{framePropName},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");

            input.PutDataToSheet("Frame Props 01 - General", frame01);
            input.PutDataToSheet("Frame Props 02 - Concrete col", frame02);

        }
        #endregion

        #region TEMP
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="JM">"S" =定義為土壤彈簧 </param>
        ///// <param name="High"></param>
        ///// <param name="Radius"></param> 
        ///// <param name="Offset">彈簧桿件支距</param>
        ///// <returns></returns>
        //public List<string> JointsProcess(string JM, float Offset)
        //{
        //    List<string> Coor1 = new List<string>();
        //    List<string> Coor2 = new List<string>();     
        //    string x1 = "";
        //    string z1 = "";

        //    float xRadius = Radius + Thick / 2;
        //    SpringOffset = xRadius;        // 土壤彈簧支距                                        
        //    float HoleAngle = 0.0f;
        //    float AngleValue = 0.0f;
        //    float AD1angle = (Aangle- ADangle)/2;    // (A環片夾角-螺栓孔夾角) 之兩側之夾角 
        //    for (int ii = 1; ii <= iPoint; ++ii)
        //    {
        //        AngleValue = HoleAngle;
        //        switch (ii)
        //        {               
        //            case 1:   //螺栓孔
        //                PolarToXY(AngleValue, xRadius, ref x1, ref z1);
        //                Coor1.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");  //1
        //                Coor2.Add($"{JM}{ii+ iPoint},GLOBAL,Cartesian,{x1},2,,{z1}");  //47
        //                AngleValue += AD2angle;
        //                GapeFacePoints(AngleValue, xRadius,ref ii, JM, Coor1, Coor2);
        //                AngleValue += Kangle;
        //                GapeFacePoints(AngleValue, xRadius,ref ii, JM, Coor1, Coor2);
        //                HoleAngle = ADangle;
        //                break;
        //            case 8:   //螺栓孔  8-17
        //                PolarToXY(AngleValue, xRadius, ref x1, ref z1);
        //                Coor1.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");  // 8
        //                Coor2.Add($"{JM}{ii + iPoint},GLOBAL,Cartesian,{x1},2,,{z1}");  //54
        //                AngleValue += AD2angle;
        //                GapeFacePoints(AngleValue, xRadius, ref ii, JM, Coor1, Coor2);
        //                AngleValue = ADangle * 2 - AD2angle;
        //                GapeFacePoints(AngleValue, xRadius, ref ii, JM, Coor1, Coor2);
        //                HoleAngle = ADangle * 2;
        //                break;
        //            default:
        //                PolarToXY(AngleValue, xRadius, ref x1, ref z1);
        //                Coor1.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");  // 8
        //                Coor2.Add($"{JM}{ii + iPoint},GLOBAL,Cartesian,{x1},2,,{z1}");  //54
        //                AngleValue += AD1angle;
        //                GapeFacePoints(AngleValue, xRadius, ref ii, JM, Coor1, Coor2);
        //                HoleAngle += ADangle;
        //                break;
        //        }
        //    }
        //    input.PutDataToSheet("Joint Coordinates", Coor1);
        //    input.PutDataToSheet("Joint Coordinates", Coor2);
        //    return Coor1;
        //}

        //public void PolarToXY(float AngleValue, float Length,ref string x1 , ref string z1)
        //{
        //    x1 = string.Format("{0:0.000}", Length * Math.Cos(Math.PI / 2 - AngleValue * Math.PI / 180));
        //    z1 = string.Format("{0:0.000}", Length * Math.Sin(Math.PI / 2 - AngleValue * Math.PI / 180));
        //}

        //public void GapeFacePoints(float AngleValue, float Length,ref int ii, string JM, List<string> Coor1, List<string> Coor2)
        //{
        //    string x1 = "";
        //    string z1 = "";
        //    int jj = 0;
        //    float DeltaAngle = 4.0f;

        //    float Angles = AngleValue - DeltaAngle;
        //    PolarToXY(Angles, Length, ref x1, ref z1);
        //    ++ii;
        //    Coor1.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
        //    jj=ii+ iPoint;
        //    Coor2.Add($"{JM}{jj},GLOBAL,Cartesian,{x1},2,,{z1}");

        //    Angles = AngleValue;
        //    PolarToXY(Angles, Length, ref x1, ref z1);
        //    ++ii;
        //    Coor1.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
        //    jj = ii + iPoint;
        //    Coor2.Add($"{JM}{jj},GLOBAL,Cartesian,{x1},2,,{z1}");

        //    Angles = AngleValue + DeltaAngle; ;
        //    PolarToXY(Angles, Length, ref x1, ref z1);
        //    ++ii;
        //    Coor1.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");
        //    jj = ii + iPoint;
        //    Coor2.Add($"{JM}{jj},GLOBAL,Cartesian,{x1},2,,{z1}");
        //}

        #endregion

        #region SAPCalculation
        //SapObject mySapObject;
        //cSapModel mySapModel;

        cOAPI mySapObject = null;
        cHelper myHelper;
        cSapModel mySapModel;

        IWorkbook wb;
        int ret;
        int num = 0;
        string[] obj = new string[2];
        double[] ObjSta = new double[2];
        string[] elm = new string[2];
        double[] ElmSta = new double[2];
        string[] LoadCase = new string[2];
        string[] StepType_test = new string[2];
        double[] StepNum_test = new double[2];
        double[] P = new double[2];
        double[] V2 = new double[2];
        double[] V3 = new double[2];
        double[] T = new double[2];
        double[] M2 = new double[2];
        double[] M3 = new double[2];
        double[] U1 = new double[2];
        double[] U2 = new double[2];
        double[] U3 = new double[2];
        double[] R1 = new double[2];
        double[] R2 = new double[2];
        double[] R3 = new double[2];

        double MaxV = 0;
        double MaxM = 0;
        string indexV;
        string indexM;
        double[] parameterV = new double[3];
        double[] parameterM = new double[3];

        List<string> loadingUID = new List<string>();
        List<Tuple<double, double, double, double, double, double>> loadingValue = new
            List<Tuple<double, double, double, double, double, double>>();
        List<Tuple<string, string, string>> loadingProp = new List<Tuple<string, string, string>>();
        List<Tuple<string, string>> jointProp = new List<Tuple<string, string>>();
        List<Tuple<double, double, double, double, double, double>> jtDisplacement = new
            List<Tuple<double, double, double, double, double, double>>();
        string RCUID = "";
        string stirrupUID = "";

        public void SAPCalculation(string inputPath, string loadingName, string outputPath, List<string> frameName)
        {
            loadingUID.Clear();
            loadingValue.Clear();
            loadingProp.Clear();
            jointProp.Clear();
            jtDisplacement.Clear();

            myHelper = new Helper();
            mySapObject = myHelper.CreateObjectProgID("CSI.SAP2000.API.SapObject");
            ret = mySapObject.ApplicationStart();
            mySapModel = mySapObject.SapModel;
            ret = mySapModel.InitializeNewModel((eUnits.kip_in_F));

            //bool temp_bool = true;
            //mySapObject = new SapObject();
            //mySapModel = mySapObject.SapModel;
            //mySapObject.ApplicationStart(eUnits.kip_ft_F, temp_bool, "");

            ret = mySapModel.File.OpenFile(inputPath);
            ret = mySapModel.Analyze.RunAnalysis();

            ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
            ret = mySapModel.Results.Setup.SetCaseSelectedForOutput(loadingName, true);

            wb = new XSSFWorkbook();

            for (int i = 0; i < frameName.Count; i++)
            {
                ret = mySapModel.Results.FrameForce(frameName[i], eItemTypeElm.ObjectElm, ref num, ref obj, ref ObjSta,
                    ref elm, ref ElmSta, ref LoadCase, ref StepType_test, ref StepNum_test, ref P, ref V2, ref V3, ref T,
                    ref M2, ref M3);
                // I-End of frame
                var forceProp = Tuple.Create(frameName[i], loadingName, frameName[i]);
                loadingProp.Add(forceProp);
                // J-End of frame
                try
                {
                    forceProp = Tuple.Create(frameName[i], loadingName, frameName[i + 1]);
                    loadingProp.Add(forceProp);
                }
                catch // last frame
                {
                    forceProp = Tuple.Create(frameName[i], loadingName, frameName[0]);
                    loadingProp.Add(forceProp);
                }
                for (int j = 0; j < 2; j++)
                {
                    string tempUID = Guid.NewGuid().ToString("D");
                    var forceValue = Tuple.Create(P[j], V2[j], V3[j], T[j], M2[j], M3[j]);
                    loadingValue.Add(forceValue);
                    loadingUID.Add(tempUID);
                    // find maximum value
                    if (Math.Abs(V2[j]) > MaxV)
                    {
                        MaxV = Math.Abs(V2[j]);
                        indexV = (i + 1).ToString();
                        parameterV[0] = P[j];
                        parameterV[1] = V2[j];
                        parameterV[2] = M3[j];
                        stirrupUID = tempUID;
                    }
                    if (Math.Abs(M3[j]) > MaxM)
                    {
                        MaxM = Math.Abs(M3[j]);
                        indexM = (i + 1).ToString();
                        parameterM[0] = P[j];
                        parameterM[1] = V2[j];
                        parameterM[2] = M3[j];
                        RCUID = tempUID;
                    }
                }
                ret = mySapModel.Results.JointDispl(frameName[i], eItemTypeElm.ObjectElm, ref num, ref obj, ref elm,
                    ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                var dispProp = Tuple.Create(frameName[i], loadingName);
                jointProp.Add(dispProp);
                var dispValue = Tuple.Create(U1[0], U2[0], U3[0], R1[0], R2[0], R3[0]);
                jtDisplacement.Add(dispValue);
            }
            List<Tuple<string, double, double, double>> outData = new List<Tuple<string, double, double, double>>();
            outData.Add(Tuple.Create(indexV, parameterV[0], parameterV[1], parameterV[2]));
            outData.Add(Tuple.Create(indexM, parameterM[0], parameterM[1], parameterM[2]));
            input.CreateResultFile(wb, loadingProp, loadingValue, loadingName);
            input.CreateDisplacementFile(wb, jointProp, jtDisplacement, "JointDisplacement");
            input.FileSaving(wb, outputPath);
            
            try
            {
                oExcuteSQL.DeleteDataBySectionUIDAndTimes($"STN_SiteData", sectionUID, 1);
            }
            catch
            {
            }
            oExcuteSQL.InsertSAPData($"STN_SiteData", loadingUID, sectionUID, loadingProp, 1, loadingValue);

            // close Sap2000
            mySapObject.ApplicationExit(false);
            mySapModel = null;
            mySapObject = null;

            // validation
            foreach (var list in outData)
            {
                Console.WriteLine("frame {0}:\tP={1}\tV={2}\tM={3}", list.Item1, list.Item2, list.Item3, list.Item4);
            }
        }
        #endregion

    }
}
