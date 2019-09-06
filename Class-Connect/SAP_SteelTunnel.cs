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
using SinoTunnelFile;

namespace SinoTunnel
{

    class SAP_SteelTunnel
    {   // 載重計算 及 資料置入 EXCEL 
        SAP_ExcelInput input;
        ExcuteSQL oExcuteSQL = new ExcuteSQL();
        GetWebData p;
        STN_VerticalStress verticalStress;
        Excel2Word word = new Excel2Word();
        SinoTunnelFile.UploadFile oUploadFile = new UploadFile();

        string sectionUID = "";
        //string DefaultfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Bauckup\\DoubleRing_LongTerm_Input_Raw.xlsx";
        //string DefaultfilePath = @"P:\8014\DoubleRing_ShortTerm_Input_Raw.xlsx";
        string DefaultfilePath = @"O:\ADMIN\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\09-軟體\SinoTunnel_WinForm\DoubleRing_ShortTerm_Input_Raw.xlsx";
        string outputPath = "";
        string inputgPath = "";

        float Thick = 0;        // p.segmentThickness;
        float Radius = 0;       // p.segmentRadiusIn;   
        float UseRadius = 0;    // = Radius + Thick / 2 ;       
        float Aangle = 0.0f;    // A 環片夾角    
        float Bangle = 0.0f;    // B 環片夾角         
        float Kangle = 0.0f;    // K 環片夾角  
        float ADangle = 0.0f;    // 螺栓孔夾角  
        float AD1angle = 0.0f;    // (A環片夾角-螺栓孔夾角) 之兩側之夾角 
        float AD2angle = 0.0f;    // (B環片夾角-螺栓孔夾角-AD1angle) 之另一側之夾角  
                

        List<string> Joint1s = new List <string>();  //第一環片桿件點位     
        List<string> Spring1s = new List<string>();  //第一環片彈簧點位
        List<string> Member1s = new List<string>();  //第一環片桿件

        List<string> Joint2s = new List<string>();   //第二環片桿件點位     
        List<string> Spring2s = new List<string>();  //第二環片彈簧點位
        List<string> Member2s = new List<string>();  //第二環片桿件                      

        Coord[] JointsCoord;
        List<string> JointsTheta = new List<string>();   
        List<string> RingLinks = new List<string>();     //兩環間之連桿
        List<string> InterRingName = new List<string>();
        List<string> SpringLinks = new List<string>();     //彈簧桿件
        List<string> LinkProperty = new List<string>();    //彈簧桿件性質
        List<string> LinkPropertyR2 = new List<string>();
        //List<string> FrameProperty = new List<string>();  //桿件混凝土強度值


        double Fc;
        double E;
        double U12;
        double UT;
        double width;
        double contactDepth;
        double fullDepth;
        string steelMaterial;
        double steelUW;
        double steelU12;
        double steelFy;
        double steelFu;
        double steelCutB;
        public SAP_SteelTunnel(string sectionUID)
        {        
            this.sectionUID = sectionUID;
            this.p = new GetWebData(sectionUID);  //  取得網路變數資料
            verticalStress = new STN_VerticalStress(sectionUID, "WEBFORM");  //取得 土層楊氏係數及

            this.Fc = p.segmentFc;
            this.E = p.segmentYoungsModulus;
            this.U12 = p.segmentPoissonRatio;
            this.UT = p.segmentUnitWeight;
            this.width = p.segmentWidth;
            this.contactDepth = p.segmentContacingDepth;
            this.fullDepth = p.segmentThickness;
            this.steelMaterial = p.Steel.Material;
            this.steelUW = p.Steel.UW;
            this.steelU12 = p.Steel.U12;
            this.steelFy = p.Steel.Fy;
            this.steelFu = p.Steel.Fu;
            this.steelCutB = p.Steel.CutB;
        }

        List<string> DistContactDepth = new List<string>();
        List<string> DepthJudge = new List<string>();
        List<float> DepthValue = new List<float>();
        List<string> DisDepthJudge = new List<string>();
        List<float> DisDepthValue = new List<float>();
        List<string> frameContactAssign = new List<string>();
        public void Process(string xfileSavingPath, string condition, bool excelOnly, int realTimes, 
            List<string> ContactDepth)
        {
            Radius = float.Parse(p.segmentRadiusIn.ToString());
            Thick = float.Parse(p.segmentThickness.ToString());
            UseRadius = Radius + Thick / 2;
            Aangle = float.Parse(p.segmentAAngle.ToString());
            Bangle = float.Parse(p.segmentBAngle.ToString());
            Kangle = float.Parse(p.segmentKAngle.ToString());
            ADangle = float.Parse(p.segmentAdjacentPoreAngle.ToString());

            DistContactDepth.Clear();
            DepthJudge.Clear();
            DepthValue.Clear();
            DisDepthJudge.Clear();
            DisDepthValue.Clear();
            frameContactAssign.Clear();
            switch (realTimes)
            {
                case 4:
                    for(int i = 0; i < ContactDepth.Count; i++)
                    {
                        string[] judge = ContactDepth[i].Split(',');                        
                        DepthValue.Add(float.Parse(judge[0]));
                        DepthJudge.Add(judge[1]);
                    }

                    DistContactDepth = ContactDepth.Distinct().ToList();
                    for(int i = 0; i < DistContactDepth.Count; i++)
                    {
                        string[] judge = DistContactDepth[i].Split(',');
                        DisDepthValue.Add(float.Parse(judge[0]));
                        DisDepthJudge.Add(judge[1]);
                    }
                    
                    AnalysisProcess(xfileSavingPath, condition, realTimes, excelOnly);
                    break;
                default:
                    AnalysisProcess(xfileSavingPath, condition, 1, excelOnly);
                    AnalysisProcess(xfileSavingPath, condition, 2, excelOnly);
                    AnalysisProcess(xfileSavingPath, condition, 3, excelOnly);
                    break;
            }
            
        }

        public void AnalysisProcess(string xfileSavingPath, string condition, int RealTimes, bool excelOnly)
        {
            Joint1s.Clear();
            Spring1s.Clear();
            Member1s.Clear();
            Joint2s.Clear();
            Spring2s.Clear();
            Member2s.Clear();
            JointsTheta.Clear();
            RingLinks.Clear();
            SpringLinks.Clear();
            LinkProperty.Clear();
            //FrameProperty.Clear();

            input = new SAP_ExcelInput(DefaultfilePath);
            switch (RealTimes)
            {
                case 4:
                    inputgPath = xfileSavingPath.Replace(".xlsx", $"_{condition}_Input_Final.xlsx");
                    outputPath = xfileSavingPath.Replace(".xlsx", $"_{condition}_Result_Final.xlsx");
                    break;
                default:
                    inputgPath = xfileSavingPath.Replace(".xlsx", $"_{condition}_Input_{RealTimes}.xlsx");
                    outputPath = xfileSavingPath.Replace(".xlsx", $"_{condition}_Result_{RealTimes}.xlsx");
                    break;
            }
            

            Joint_Coordinates();
            Joint_Restraint_Assignments();
            Connectivity_Frame_AND_Frame_Output_Station_Assigns();
            Link_Props_01_General_AND_Link_Props_05_Gap(); // 土壤彈簧連桿性質
            Connectivity_Link_And_Link_Property_Assignments();  // 土壤彈簧連桿
            Material_Frame_Props_01_Generall_AND_Frame_Props_02_Concrete_Col(RealTimes, condition);
            //Material_InterRing_A36Steel();
            Frame_Section_Assignments(RealTimes);  //Frame_Section_Assignments
            Frame_Releases1_General();
         //   MatProp_01_General_AND_MatProp_02_BasicAnd_Mech_Props_AND_MatProp_03b_Concrete_Data();
            Frame_Loads_Distributed();

            JtSpringAssignsUncoupled();
            GridLines();

            FrameNameAssign();

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
            names.Add("MatProp 03a - Steel Data");
            names.Add("MatProp 03b - Concrete Data");
            names.Add("Load Case Definitions");
            names.Add("Load Pattern Definitions");
            names.Add("Case - Static 1 - Load Assigns");
            names.Add("Case - Static 2 - NL Load App");
            names.Add("Combination Definitions");
            names.Add("Program Control");            

            if (excelOnly) return;

            string loadType = "";
            switch (condition)
            {
                case "SteelTunnelOrigin": loadType = "Steel_Origin"; break;
                case "SteelTunnelCut": loadType = "Steel_Cut"; break;
            }
            SAPCalculation(inputgPath, "DL+SOIL (NL)", outputPath, "Steel", RealTimes, loadType);

            if(RealTimes == 4)
            {
                string[] wordtemp = xfileSavingPath.Split('\\');
                string wordpath = "";
                for (int i = 0; i < wordtemp.Length - 1; i++) { wordpath += wordtemp[i]; wordpath += "\\"; }
                wordpath += $"{condition}.docx";
                //string wordpath = xfileSavingPath.Replace(".xlsx", $".docx");
                word.Add(inputgPath, names, false, true);

                word.Add(outputPath, names, true, true);
                word.FileSaving(wordpath);
                oUploadFile.UploadToServer(sectionUID, wordpath);
            }

            
        }

        #region Loading Coordinates ; Joint Restraint Assignments 
        public void Joint_Coordinates()
        {
            AD1angle = ADangle / 2;               // 螺栓孔夾角 / 2 = 18 之兩側之夾角  
            AD2angle = (ADangle - Kangle) / 2;    // (螺栓孔夾角-K環片夾角)/2  之另一側之夾角            
            Joint1s = JointsProcess("", 0);                         // 隧道點位   
            Joint2s = Joint2sProcess("", 0, Joint1s);
            Spring1s = JointsProcess("S", UseRadius);                // 隧道外彈簧點位
            Spring2s = Joint2sProcess("S", 0, Spring1s);
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
            if (JM == "S") { ss = 1; ee = 9; } //For Partial points not for all 
            for (int jj = ss; jj < ee; ++jj)
            {
                AngleValue = ADangle * jj;
                switch (jj)
                {
                    case 0:   //第一個螺栓孔位置(FOR JM=""  隧道環片點位)
                    case 1:   //第一個螺栓孔位置(FOR JM="S" 土壤彈簧點位) 
                        PolarToXY(AngleValue, xRadius, ref x1, ref z1);
                        if (JM != "S")
                        {
                            XXXX.Add($"{JM}{ii},GLOBAL,Cartesian,{x1},0,,{z1}");  //1                           
                            ++ii;
                        }
                        AngleValue += AD2angle;  // AD2angle = (螺栓孔夾角-K環片夾角)/2  之另一側之夾角  = 10.5
                        GapeFacePoints(AngleValue, xRadius, ref ii, JM, XXXX);
                        AngleValue += Kangle;    //  Kangle = 15度
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
          //  iPoints = ii; //每環最後一點編號
            return XXXX;
        }

        public List<string> Joint2sProcess(string JM, float Offset, List<string> Jpoint)
        {   // 第二環片編號
            List<string> XXXX = new List<string>();
            for (int jj = 0; jj < Jpoint.Count; ++jj)
            {
                string XX = Jpoint[jj];
                string[] aa = XX.Split(',');
                string xItem = aa[0].ToString();
                if (JM != "") xItem = xItem.Replace(JM, "");
                string item = (int.Parse(xItem) + Jpoint.Count).ToString();
                XX = XX.Replace(JM + (jj + 1).ToString() + ",GLOBAL,", item + ",GLOBAL,"); //更新點位編號
                XX = XX.Replace(",0,,", ",2,,");  //更新Y座標...  0->2 
                XXXX.Add($"{JM}{XX}");
            }
            input.PutDataToSheet("Joint Coordinates", XXXX);
            return XXXX;
        }


        /// <summary>
        /// 點位所代表的夾角
        /// </summary>
        public void JointsThetaProcess()
        {   // 環片點位資料
            JointsCoord = null;
            JointsCoord = new Coord[Joint1s.Count + 1];
            double dX = 0;    
            double dY = 0;     
            double multi = 0; 
            for (int jj = 0; jj < Joint1s.Count; ++jj)
            {
                string XX = Joint1s[jj];
                string[] aa = XX.Split(',');
                string xJoint = aa[0].ToString();
                JointsCoord[jj+1].XorR = double.Parse(aa[3].ToString());
                JointsCoord[jj+1].Z = double.Parse(aa[6].ToString());
            }                
            for (int jj = 1; jj < JointsCoord.Length; ++jj)
            {
                if (jj == 1)
                {
                    dX = JointsCoord[jj + 1].XorR - JointsCoord[JointsCoord.Length-1].XorR;
                    dY = JointsCoord[jj + 1].Z - JointsCoord[JointsCoord.Length-1].Z;
                }
                else
                {
                    if (jj == JointsCoord.Length-1)
                    {
                        dX = JointsCoord[jj-1].XorR - JointsCoord[1].XorR;
                        dY = JointsCoord[jj-1].Z - JointsCoord[1].Z;
                    }
                    else                   
                    {
                        dX = JointsCoord[jj+1].XorR - JointsCoord[jj-1].XorR;
                        dY = JointsCoord[jj+1].Z - JointsCoord[jj-1].Z;
                    }                
                }
                multi = dX * dX + dY * dY;
                double LL = Math.Sqrt(multi) / 2;                
                JointsCoord[jj].θ = Math.Round(Math.Asin(LL / Convert.ToDouble(UseRadius)) * 2 * 180/Math.PI/2, 2);
                JointsTheta.Add(JointsCoord[jj].θ.ToString());
            }
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

        public void Joint_Restraint_Assignments()
        {
            List<string> JointRestraint = new List<string>();
            for (int j = 0; j < Joint1s.Count * 2; ++j)
            {
                JointRestraint.Add($"{j+1},No,Yes,No,Yes,No,Yes");
            }
            for (int j = 0; j < Spring1s.Count * 2; ++j)
            {
                JointRestraint.Add($"S{j+1},Yes,Yes,Yes,Yes,Yes,Yes");
            }
            input.PutDataToSheet("Joint Restraint Assignments", JointRestraint);
        }

        #endregion

        #region Material Frame : Connectivity-Frame And Frame Output Station Assigns ; Section Assignments  
        // Frame Section Propertis 02 - Concrete Col              
        public void Connectivity_Frame_AND_Frame_Output_Station_Assigns()
        {   //  定義桿件IJ端     
            List<string> Output = new List<string>();
            for (int j = 1; j < Joint1s.Count; ++j)
            {
                Member1s.Add($"{j},{j},{j + 1},No");
                Output.Add($"{j},MinNumSta,2,,Yes,Yes");
            }
            Member1s.Add($"{Joint1s.Count},{Joint1s.Count},1, No"); //最後一隻桿件
            Output.Add($"{Joint1s.Count},MinNumSta,2,,Yes,Yes");//最後一隻桿件

            int JPoints = Joint1s.Count + Joint2s.Count;   // 
            for (int j = Joint1s.Count + 1; j < JPoints; ++j)
            {
                Member2s.Add($"{j},{j},{j + 1},No");
                Output.Add($"{j},MinNumSta,2,,Yes,Yes");
            }
            Member2s.Add($"{JPoints}, {JPoints}, {Joint1s.Count + 1},No"); //最後一隻桿件
            Output.Add($"{JPoints},MinNumSta,2,,Yes,Yes");//最後一隻桿件

            //兩環片間之連桿
            for (int j = 1; j <= Joint1s.Count; ++j)
            {
                RingLinks.Add($"{JPoints + j},{j},{Joint1s.Count + j},No");
                Output.Add($"{JPoints + j},MinNumSta,2,,Yes,Yes");
            }
            input.PutDataToSheet("Connectivity - Frame", Member1s);
            input.PutDataToSheet("Connectivity - Frame", Member2s);
            input.PutDataToSheet("Connectivity - Frame", RingLinks);
            input.PutDataToSheet("Frame Output Station Assigns", Output);
        }

        //List<string> segmentUID = new List<string>();
        List<double> assignDepth = new List<double>();
        List<string> frameAssign = new List<string>();
        public void Material_Frame_Props_01_Generall_AND_Frame_Props_02_Concrete_Col(int realTimes, string condition)      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        {
            assignDepth.Clear();
            frameAssign.Clear();
            List<string> FrameProperty01 = new List<string>();
            List<string> FrameProperty02 = new List<string>();
            float AngleValue = 0;
            List<string> ThataDistinct = JointsTheta.Distinct().ToList();

            //1. 連桿 
            for (int jj = 0; jj < ThataDistinct.Count; ++jj)
            {
                string XX = ThataDistinct[jj];
                AngleValue = float.Parse(XX);
                FrameLinkProperty(AngleValue, FrameProperty01, FrameProperty02);
            }

            //segmentUID.Clear();
            //2. Segment
            //string SegmentUID = "";
            //****************************
            // 50*25 ; 50*14 ; 100*25 ; 100*14 
            //SegmentUID = oExcuteSQL.GetByUID("STN_Section", sectionUID).Rows[0]["Load_Segment"].ToString();
            //SegmentProperty(SegmentUID, FrameProperty01, FrameProperty02, true);
            //segmentUID.Add(SegmentUID);
            //if(realTimes != 4)
            //{
            //    SegmentUID = oExcuteSQL.GetByUID("STN_Section", sectionUID).Rows[0][$"LLI{realTimes}_Segment"].ToString();
            //    SegmentProperty(SegmentUID, FrameProperty01, FrameProperty02, true);
            //    segmentUID.Add(SegmentUID);
            //}

            //****************************
            List<string> mat01 = new List<string>();
            List<string> mat02 = new List<string>();
            List<string> mat03b = new List<string>();
            
            DataTable mat = oExcuteSQL.GetBySection("STN_FrameMaterial", sectionUID, "ORDER BY TIMES ASC");
            List<double> contDepth = new List<double>();
            List<double> steelDepth = new List<double>(); //理論上只有一種厚度與材料性質，分兩種純粹代表 cut origin
            List<string> steelType = new List<string>();
            for (int i = 0; i < mat.Rows.Count; i++)
            {
                if (mat.Rows[i]["LoadType"].ToString() == "Steel")
                    if (mat.Rows[i]["MaterialType"].ToString() == "Concrete")
                        contDepth.Add(Math.Round(double.Parse(mat.Rows[i]["Depth"].ToString()), 4));
                    else if (mat.Rows[i]["Times"].ToString() == "Cut")
                    {
                        steelDepth.Add(double.Parse(mat.Rows[i]["Depth"].ToString()));
                        steelType.Add(mat.Rows[i]["MaterialType"].ToString());
                    }
                    else if(mat.Rows[i]["Times"].ToString() == "Origin")
                    {
                        steelDepth.Add(double.Parse(mat.Rows[i]["Depth"].ToString()));
                        steelType.Add(mat.Rows[i]["MaterialType"].ToString());
                    }
            }
                
                        
            string matName = "";
            double depth;
            depth = contDepth[0];
            matName = $"Concrete {Fc} t={depth * 100}cm T={fullDepth * 100}cm";
            mat01.Add($"{matName},Concrete,Isotropic,No");
            mat02.Add($"{matName},{UT},{UT / p.newton},{E},{E / 2 / (1 + U12)},{U12},{9.9E-6}");
            mat03b.Add($"{matName},{Fc * 98},No,Mander,Takeda,0.0021914,0.005,-0.1,0,0");

            string frameHalf;
            string frameFull;
            frameHalf = $"Segment {width * 100 / 2}x{depth * 100}cm";
            frameFull = $"Segment {width * 100}x{depth * 100}cm";
            FrameProperty01.Add($"{frameHalf},{matName},Rectangular,{depth},{width / 2},,,,,,,,,,,,,,,,,Yes,No,,,,No");
            FrameProperty01.Add($"{frameFull},{matName},Rectangular,{depth},{width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
            FrameProperty02.Add($"{frameHalf},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
            FrameProperty02.Add($"{frameFull},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
            frameAssign.Add(frameFull);
            frameAssign.Add(frameHalf);            
            //****************************


            //****************************
            if (realTimes != 4)
            {
                switch (realTimes)
                {
                    case 1: depth = contDepth[1]; break;                        
                    case 2: depth = contDepth[2]; break;                        
                    case 3: depth = contDepth[3]; break;                        
                }
                matName = $"Concrete {Fc} t={depth * 100}cm T={fullDepth * 100}cm";
                mat01.Add($"{matName},Concrete,Isotropic,No");
                mat02.Add($"{matName},{UT},{UT / p.newton},{E},{E / 2 / (1 + U12)},{U12},{9.9E-6}");
                mat03b.Add($"{matName},{Fc * 98},No,Mander,Takeda,0.0021914,0.005,-0.1,0,0");

                frameHalf = $"Segment {width * 100 / 2}x{depth * 100}cm";
                frameFull = $"Segment {width * 100}x{depth * 100}cm";
                FrameProperty01.Add($"{frameHalf},{matName},Rectangular,{depth},{width / 2},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                FrameProperty01.Add($"{frameFull},{matName},Rectangular,{depth},{width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                FrameProperty02.Add($"{frameHalf},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
                FrameProperty02.Add($"{frameFull},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");

                assignDepth.Add(depth);
                frameAssign.Add(frameFull);
                frameAssign.Add(frameHalf);
            }
            else// realTimes == 4 (for final Depth)
            {                
                List<float> matDepth = new List<float>();
                matDepth = DepthValue.Distinct().ToList();
                
                //matDepth.Remove((float)(p.segmentContacingDepth-0.125));
                //matDepth.Remove((float)(p.segmentContacingDepth-0.225));

                //float depth;
                for (int i = 0; i < matDepth.Count; i++)
                {
                    depth = matDepth[i];
                    double ut = UT * Thick / matDepth[i];
                    matName = $"Concrete{Fc} for t={depth * 100}cm T={Thick * 100}cm";
                    mat01.Add($"{matName},Concrete,Isotropic,No");
                    mat02.Add($"{matName},{ut},{ut / p.newton},{E}," +
                        $"{E / 2 / (1 + U12)},{U12},{9.9E-6}");
                    mat03b.Add($"{matName},{Fc * 98},No,Mander,Takeda,0.00221914,0.005,-0.1,0,0");                    
                }
                
                                
                for(int i = 0; i < DistContactDepth.Count; i++)
                {
                    depth = DisDepthValue[i];
                    frameHalf = $"Segment {width * 100 / 2}x{depth * 100}cm";
                    frameFull = $"Segment {width * 100}x{depth * 100}cm";
                    matName = $"Concrete{Fc} for t={depth * 100}cm T={Thick * 100}cm";

                    switch (DisDepthJudge[i])
                    {
                        case "Half":
                            FrameProperty01.Add($"{frameHalf},{matName},Rectangular,{depth},{width/ 2},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                            FrameProperty02.Add($"{frameHalf},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");
                            break;
                        case "Full":
                            FrameProperty01.Add($"{frameFull},{matName},Rectangular,{depth},{width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                            FrameProperty02.Add($"{frameFull},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");                     
                            break;
                    }
                }

                for(int i = 0; i < DepthValue.Count; i++)
                {
                    depth = DepthValue[i];
                    frameHalf = $"Segment {width * 100 / 2}x{depth * 100}cm";
                    frameFull = $"Segment {width * 100}x{depth * 100}cm";

                    string assign = "";
                    switch (DepthJudge[i])
                    {
                        case "Half": assign = frameHalf; break;
                        case "Full": assign = frameFull; break;
                    }

                    switch (i)
                    {
                        case 0:
                        case 1:
                            break;
                        default: frameContactAssign.Add($"{assign}"); break;                            
                    }
                    frameContactAssign.Add($"{assign}");
                    assignDepth.Add(depth);
                }
            }


            //****************************


            //SegmentUID = oExcuteSQL.GetByUID("STN_Section", sectionUID).Rows[0]["Steel0_Segment"].ToString();
            //SegmentProperty(SegmentUID, FrameProperty01, FrameProperty02, false);
            //segmentUID.Add(SegmentUID);
            //if(realTimes != 4)
            //{
            //    SegmentUID = oExcuteSQL.GetByUID("STN_Section", sectionUID).Rows[0][$"Steel{realTimes}_Segment"].ToString();
            //    SegmentProperty(SegmentUID, FrameProperty01, FrameProperty02, false);
            //    segmentUID.Add(SegmentUID);
            //}

            // Inter Ring
            matName = "Inter-Ring Shear";
            mat01.Add($"{matName},Concrete,Isotropic,No");
            mat02.Add($"{matName},0,0,{1E10},{(1E10) / 2 / (1 + U12)},{U12},{9.9E-6}");
            mat03b.Add($"{matName},{Fc * 98},No,Mander,Takeda,0.00221914,0.005,-0.1,0,0");

            //Steel Material and Frame [0]:Cut [1]:Origin
            List<string> mat03a = new List<string>();

            double steelE = 2.04E8;
            matName = $"{steelType[0]}";
            mat01.Add($"{matName},Steel,Isotropic,No");
            mat02.Add($"{matName},{steelUW},{steelUW / p.newton},{steelE},{steelE / 2 / (1 + steelU12)},{steelU12},{1.17E-5}");
            mat03a.Add($"{matName},{steelFy * 98},{steelFu * 98},{steelFy * 98 * 1.5},{steelFu * 98 * 1.1}," +
                $"Simple,Kinematic,0.02,0.14,0.2,-0.1");

            double cmFulldepth = fullDepth * 100;
            double cmSteelDepth = steelDepth[0] * 100;
            double bcut = 800 / Math.Sqrt(steelFy) * cmSteelDepth;
            double effA = bcut * cmSteelDepth * 2 + (cmFulldepth - cmSteelDepth) * cmSteelDepth * 3;//有三塊
            double ybar = (bcut * cmSteelDepth * 2 * (cmFulldepth - cmSteelDepth / 2) +
                (cmFulldepth - cmSteelDepth) * cmSteelDepth * 3 * ((cmFulldepth - cmSteelDepth) / 2)) / effA;

            double t1 = bcut * Math.Pow(cmSteelDepth, 3) / 12;
            double t2 = bcut * cmSteelDepth * Math.Pow(cmFulldepth - (cmSteelDepth / 2) - ybar, 2);

            double t3 = cmSteelDepth * Math.Pow(cmFulldepth - cmSteelDepth, 3) / 12;
            double t4 = (cmFulldepth - cmSteelDepth) * cmSteelDepth * Math.Pow(ybar - ((cmFulldepth - cmSteelDepth) / 2), 2);

            double inertia = (t1 + t2) * 2 + (t3 + t4) * 3;
            //double inertia = ((1 / 12) * bcut * Math.Pow(cmSteelDepth, 3) +
            //    bcut * cmSteelDepth * Math.Pow(cmFulldepth - (cmSteelDepth / 2) - ybar, 2)) * 2 +
            //    ((1 / 12) * cmSteelDepth * Math.Pow(cmFulldepth - cmSteelDepth, 3) +
            //    (cmFulldepth - cmSteelDepth) * cmSteelDepth * Math.Pow(ybar - ((cmFulldepth - cmSteelDepth) / 2), 2)) * 3;

            FrameProperty01.Add($"Steel Segment Original,{steelType[1]},General,{steelDepth[1]},0.96,,,,," +
                $"{effA * 1E-4},0,{inertia * 1E-8},0.005369221,0.020406229,0.027851646,0.001441557,0.010502461,0.002588906," +
                "0.014205,0.073891442,0.332382004,No,No,,0,0,No,1,1,1,1,1,1,1,1");

            bcut = 1600 / Math.Sqrt(steelFy) * cmSteelDepth;
            if (bcut > steelCutB) bcut = steelCutB;
            effA = bcut * cmSteelDepth + cmSteelDepth * (cmFulldepth - cmSteelDepth) * 2; //開孔後剩兩塊
            ybar = (bcut * cmSteelDepth * (cmFulldepth - cmSteelDepth / 2) +
                (cmFulldepth - cmSteelDepth) * cmSteelDepth * ((cmFulldepth - cmSteelDepth) / 2) * 2) / effA;

            t1 = bcut * Math.Pow(cmSteelDepth, 3) / 12;
            t2 = bcut * cmSteelDepth * Math.Pow(cmFulldepth - (cmSteelDepth / 2) - ybar, 2);

            t3 = cmSteelDepth * Math.Pow(cmFulldepth - cmSteelDepth, 3) / 12;
            t4 = (cmFulldepth - cmSteelDepth) * cmSteelDepth * Math.Pow(ybar - ((cmFulldepth - cmSteelDepth) / 2), 2);

            inertia = (t1 + t2) * 1 + (t3 + t4) * 2;
            //inertia = ((1 / 12) * bcut * Math.Pow(cmSteelDepth, 3) +
            //    bcut * cmSteelDepth * Math.Pow((cmFulldepth - cmSteelDepth / 2) - ybar, 2)) +
            //    ((1 / 12) * cmSteelDepth * Math.Pow(cmFulldepth - cmSteelDepth, 3) +
            //    (cmFulldepth - cmSteelDepth) * cmSteelDepth * Math.Pow(ybar - (cmFulldepth - cmSteelDepth) / 2, 2)) * 2;

            FrameProperty01.Add($"Steel Segment Cut,{steelType[0]},General,{steelDepth[0]},0.3 ,,,,," +
                $"{effA * 1E-4},0,{inertia * 1E-8},0.000252371,0.013434904,0.008198827,0.000855947,0.001573334,0.0015135," +
                $"0.002226,0.07870857,0.106621193,No,No,,0,0,No,1,1,1,1,1,1,1,1");


            switch (condition)
            {
                case "SteelTunnelOrigin": frameAssign.Add("Steel Segment Original"); break;
                case "SteelTunnelCut": frameAssign.Add("Steel Segment Cut"); break;
            }

            input.PutDataToSheet("MatProp 01 - General", mat01);
            input.PutDataToSheet("MatProp 02 - Basic Mech Props", mat02);
            input.PutDataToSheet("MatProp 03a - Steel Data", mat03a);
            input.PutDataToSheet("MatProp 03b - Concrete Data", mat03b);

            input.PutDataToSheet("Frame Props 01 - General", FrameProperty01);
            input.PutDataToSheet("Frame Props 02 - Concrete Col", FrameProperty02);            
        }

        //public void SegmentProperty(string SegmentUID, List<string> FrameProperty01, List<string> FrameProperty02, bool mat)
        //{
        //    DataTable dt;
        //    dt = oExcuteSQL.GetByUID("STN_Segment", SegmentUID);
        //    string Name = dt.Rows[0]["Name"].ToString();
        //    string Shape = dt.Rows[0]["Shape"].ToString();
        //    string Height = dt.Rows[0]["Height"].ToString();
        //    string Width = dt.Rows[0]["Width"].ToString();

        //    FrameProperty.Add($"{Name}");
        //    FrameProperty02.Add($"{Name},A615Gr60,A615Gr60,Rectangular,Ties,0.04,3,3,,#9,#4,0.15,3,3,Design");

        //    string MaterialUID = dt.Rows[0]["Material"].ToString();
        //    List<string> AAA = new List<string>();  //MatProp 01
        //    List<string> BBB = new List<string>();  //MatProp 02
        //    List<string> CCC = new List<string>();  //MatProp 03b 
        //    DataTable dm = oExcuteSQL.GetByUID("STN_SegmentMaterial", MaterialUID);

        //    string MName = "";
        //    if (dm.Rows.Count > 0)
        //    {
        //        DataRow dr = dm.Rows[0];
        //        MName = dr["MaterialName"].ToString();
        //        AAA.Add($"{MName},Concrete,Isotropic,No");
        //        float UnitWeight = float.Parse(dr["UnitWeight"].ToString());
        //        float UnitMass = float.Parse(dr["UnitWeight"].ToString()) / Convert.ToSingle(p.newton);
        //        float G12 = float.Parse(dr["YoungModulus"].ToString()) / 2 / (float.Parse(dr["PoissonRatio"].ToString()) + 1);
        //        BBB.Add($"{MName},{UnitWeight},{UnitMass},{dr["YoungModulus"].ToString()},{G12},{dr["PoissonRatio"].ToString()},{dr["CTE"].ToString()}");
        //        float Fc = float.Parse(dr["Fc"].ToString());
        //        CCC.Add($"{MName},{Fc},No,Mander,Takeda,0.00221914,0.005,-0.1,0,0");
        //    }

        //    if (mat)
        //    {                
        //        input.PutDataToSheet("MatProp 01 - General", AAA);
        //        input.PutDataToSheet("MatProp 02 - Basic Mech Props", BBB);
        //        input.PutDataToSheet("MatProp 03b - Concrete Data", CCC);
        //    }
            
        //    FrameProperty01.Add($"{Name},{MName},{Shape},{Height},{Width},,,,,,,,,,,,,,,,,Yes,No,,,,No");
        //}

        //public void Material_InterRing_A36Steel()
        //{
        //    double U12 = p.segmentPoissonRatio;
        //    string interRing = "Inter-Ring Shear";
        //    string A36Steel = "A36";
        //    double Fc = p.segmentFc * 98;
        //    double steelU = 0.3;
        //    double steelE = 2.04E8;
        //    List<string> mat01 = new List<string>();
        //    mat01.Add($"{interRing},Concrete,Isotropic,No");
        //    mat01.Add($"{A36Steel},Steel,Isotropic,No");

        //    List<string> mat02 = new List<string>();
        //    mat02.Add($"{interRing},0,0,{1E10},{(1E10) / (2 * (1 + U12))},{U12},{9.9E-6}");
        //    mat02.Add($"{A36Steel},78,7.9538,{steelE},{steelE / (2 * (1 + steelU))},{steelU},{1.17E-5}");

        //    List<string> mat03aSteel = new List<string>();
        //    mat03aSteel.Add($"{A36Steel},248211.28,399896,372316.9,439885.6,Simple,Kinematic,0.02,0.14,0.2,-0.1");

        //    List<string> mat03bConcrete = new List<string>();
        //    mat03bConcrete.Add($"{interRing},{Fc},No,Mander,Takeda,0.00221914,0.005,-0.1,0,0");

        //    input.PutDataToSheet("MatProp 01 - General", mat01);
        //    input.PutDataToSheet("MatProp 02 - Basic Mech Props", mat02);
        //    input.PutDataToSheet("MatProp 03a - Steel Data", mat03aSteel);
        //    input.PutDataToSheet("MatProp 03b - Concrete Data", mat03bConcrete);
        //}
        
        public double PropertyTansform(float AngleValue)
        {
            //計算Ksb(兩環的連桿)
            double E = p.segmentYoungsModulus; //  31528558;
            double nu = p.segmentPoissonRatio;  // 0.2;
            double width = p.segmentWidth;
            double L = 2;
            double G = (E / (2 * (1 + nu)));
            double Ksb = G * Math.PI / 4 * (Math.Pow((Radius + Thick) * 2, 2) - Math.Pow((Radius) * 2, 2)) / (width / 2);

            //計算直徑(兩環的連桿)            
            double Etemp = 1E10; 
            double xxx = Ksb * AngleValue / 360;
            double inertia = xxx * Math.Pow(L, 3) / (12 * Etemp);
           return Math.Round(Math.Pow(64 * inertia / Math.PI, 0.25), 4);
        }

        public void FrameLinkProperty(float AngleValue, List<string> FrameProperty01, List<string> FrameProperty02)
        {
            double xxx = PropertyTansform(AngleValue);
            FrameProperty01.Add($"EQ Round-Theta={AngleValue},Inter-Ring Shear,Circle,{xxx},,,,,,,,,,,,,,,,,,Yes,No,,,,No");
            FrameProperty02.Add($"EQ Round-Theta={AngleValue},A615Gr60,A615Gr60,Circular,Ties,0.04,,,8,#9,#4,0.15,,,Design");
            InterRingName.Add($"EQ Round-Theta={AngleValue}");
        }

        public List<float> JointAngle()
        {
            List<float> angles = new List<float>();
            float AngleValue = 0.0f;
            int ss = 0;
            int ee = 10;  //代表 螺栓孔數目
            for (int jj = ss; jj < ee; ++jj)
            {
                AngleValue = ADangle * jj;
                switch (jj)
                {
                    case 0:   //第一個螺栓孔位置(FOR JM=""  隧道環片點位)
                    case 1:   //第一個螺栓孔位置(FOR JM="S" 土壤彈簧點位) 
                        angles.Add(AngleValue);                       
                        AngleValue += AD2angle;  // AD2angle = (螺栓孔夾角-K環片夾角)/2  之另一側之夾角  = 10.5
                        Gape(AngleValue, angles);
                        AngleValue += Kangle;    //  Kangle = 15度
                        Gape(AngleValue, angles);
                        break;
                    default:
                        angles.Add(AngleValue);
                        AngleValue += AD1angle;
                        Gape(AngleValue, angles);
                        break;
                }
            }
            
            return angles;
        }

        public void Gape(float AngleValue, List<float> angles)
        {
            float DeltaAngle = 4.0f;

            float Angles = AngleValue - DeltaAngle;
            angles.Add(Angles);
            Angles = AngleValue;
            angles.Add(Angles);
            Angles = AngleValue + DeltaAngle; ;
            angles.Add(Angles);
        }


        List<string> steelFrameName = new List<string>();
        public void Frame_Section_Assignments(int realTimes)  //  Frame Section Assignments
        {
            int SGAnum = 3;

            List<float> anglenew = new List<float>();
            anglenew = JointAngle();
            anglenew.Add(0f); // add the last frame
            float steelAngle = 360 - SGAnum * Aangle - Bangle;
            float R1_BS = Bangle - AD1angle;
            float R1_SA = R1_BS + steelAngle;
            float R1_AB = 360 - AD1angle;
            float R2_BK = AD2angle;
            float R2_KB = R2_BK + Kangle;
            float R2_BA = R2_KB + Bangle;
            float R2_AB = 360 - (Bangle - AD2angle);

            // segment angles
            List<float> R1Angle = new List<float>();
            List<float> R2Angle = new List<float>();
            R1Angle.Add(R1_BS);
            R1Angle.Add(R1_SA);
            for (int i = 0; i < SGAnum; i++)
            {
                R1Angle.Add(R1_SA + (i + 1) * Aangle);
            }
            R2Angle.Add(R2_BK);
            R2Angle.Add(R2_KB);
            R2Angle.Add(R2_BA);
            for (int i = 0; i < SGAnum; i++)
            {
                R2Angle.Add(R2_BA + (i + 1) * Aangle);
            }

            // frame assignment
            List<string> FrameAssignments = new List<string>();
            int k = 0;
            int frame;
            
            int segmentcount = 0;
            int counter = 0;
            
            // first ring
            for (frame = 1; frame < anglenew.Count; frame++)
            {
                if (anglenew[frame] == R1Angle[segmentcount] || anglenew[frame - 1] == R1Angle[segmentcount])
                {
                    counter++;
                    if (anglenew[frame - 1] == R1Angle[0] || anglenew[frame] == R1Angle[1]) // near steel segment
                    {
                        if (realTimes == 4) FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[2]},{frameAssign[2]},Default");
                        else FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[4]},{frameAssign[4]},Default");
                        steelFrameName.Add($"{frame}");
                        continue;
                    }
                    // Segment 100x14
                    if (realTimes == 4)
                    {
                        FrameAssignments.Add($"{frame},Rectangular,N.A.," +
                        $"{frameContactAssign[k]},{frameContactAssign[k]},Default");
                        k++;
                    }
                    else FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[2]},{frameAssign[2]},Default");
                }
                else if (anglenew[frame] > R1_BS && anglenew[frame] <= R1_SA) // Steel Segment Original
                {
                    if (realTimes == 4) FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[2]},{frameAssign[2]},Default");
                    else FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[4]},{frameAssign[4]},Default");
                    steelFrameName.Add($"{frame}");
                }
                else // Segment 100*25
                {
                    if (realTimes == 4) FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[0]},{frameAssign[0]},Default");
                    else FrameAssignments.Add($"{frame},Rectangular,N.A.,{frameAssign[0]},{frameAssign[0]},Default");
                }
                if (counter == 2) // change segment angle
                {
                    segmentcount++;
                    counter = 0;
                    if (segmentcount == R1Angle.Count) // no segment left
                    {
                        segmentcount = 0;
                    }
                }
            }
            // second ring
            int r2frame;
            for (frame = 1; frame < anglenew.Count; frame++)
            {
                r2frame = frame + anglenew.Count - 1;
                if (anglenew[frame] == R2Angle[segmentcount] || anglenew[frame - 1] == R2Angle[segmentcount])
                {
                    counter++;
                    // Segment 50*14
                    if (realTimes == 4)
                    {
                        FrameAssignments.Add($"{r2frame},Rectangular,N.A.," +
                        $"{frameContactAssign[k]},{frameContactAssign[k]},Default");
                        k++;
                    }
                    else FrameAssignments.Add($"{r2frame},Rectangular,N.A.,{frameAssign[3]},{frameAssign[3]},Default");
                }
                else // Segment 50*25  
                {                      
                    FrameAssignments.Add($"{r2frame},Rectangular,N.A.,{frameAssign[1]},{frameAssign[1]},Default");
                }
                if (counter == 2) // change segment angle
                {
                    segmentcount++;
                    counter = 0;
                    if (segmentcount == R2Angle.Count) // no segment left
                    {
                        segmentcount = 0;
                    }
                }
            }

            //int iNum = 0;
            ////int k = 0;

            //for (int j = 0; j < Member1s.Count; ++j)
            //{   //第一環桿件 Width =100cm ; FrameProperty = 100*25;100*14;50*25;50*14;Steel Segment Original
            //    // frameAssign [normal Full, normal Half, contactFull, contactHalf, Origin or Cut]
            //    string[] JJ = Member1s[j].Split(',');
            //    iNum = int.Parse(JJ[0]);
            //    switch (iNum)
            //    {
            //        case int n when (n == 9 || n == 21 || n == 28 || n == 29 || n == 36 || n == 37 || n == 44 || n == 45):
            //            // Segment 100*14
            //            if (realTimes == 4)
            //            {
            //                FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.," +
            //                $"{frameContactAssign[k]},{frameContactAssign[k]},Default");
            //                k++;
            //            }                            
            //            else FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[2]},{frameAssign[2]},Default");
            //            break;
            //        case int n when (n >= 10 &&  n<=20):  //Steel Segment Original
            //            if(realTimes == 4) FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[2]},{frameAssign[2]},Default");
            //            else FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[4]},{frameAssign[4]},Default");
            //            steelFrameName.Add($"{JJ[0]}");
            //            break;
            //        default:   // Segment 100*25
            //            if(realTimes ==4) FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[0]},{frameAssign[0]},Default");
            //            else FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[0]},{frameAssign[0]},Default");
            //            break;
            //    }            
            //}
            //for (int j = 0; j < Member2s.Count; ++j)
            //{   //第二環桿件 Width =50cm ; FrameProperty = 100*25;100*14;50*25;50*14;Steel Segment Original
            //    // frameAssign [normal Full, normal Half, contactFull, contactHalf, Origin or Cut]
            //    string[] JJ = Member2s[j].Split(',');
            //    iNum = int.Parse(JJ[0]);
            //    switch (iNum)
            //    {
            //        case int n when (n == 48 || n == 49 || n ==51 || n == 52 || n == 62 || n == 63 || n == 70 || n == 71 || n == 78 || n == 79 || n == 86 || n == 87):
            //            // Segment 50*14
            //            if (realTimes == 4)
            //            {
            //                FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.," +
            //                $"{frameContactAssign[k]},{frameContactAssign[k]},Default");
            //                k++;
            //            }
            //            else FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[3]},{frameAssign[3]},Default");
            //            break;
            //        default:   // Segment 50*25                        
            //            FrameAssignments.Add($"{JJ[0]},Rectangular,N.A.,{frameAssign[1]},{frameAssign[1]},Default");
            //            break;
            //    }
            //}

            for (int j = 0; j < RingLinks.Count; ++j)
            {   //兩環間連桿 
                string[] JJ = RingLinks[j].Split(',');                              
                int LL = int.Parse(JJ[0]) - Joint1s.Count - Joint2s.Count;  // 93-46-46 = 1==> 1 代表連桿接的點位 1.   
                string XX = Convert.ToString(JointsCoord[LL].θ);
                string result = InterRingName.FirstOrDefault(s => s.Contains(XX));         
                FrameAssignments.Add($"{JJ[0]},Circle,N.A.,{result},{result},Default");      
            }
            input.PutDataToSheet("Frame Section Assignments", FrameAssignments);
        }

        // 兩環片間之連桿 ; Frame Releases 1 - General  
        public void Frame_Releases1_General()     
        {
            List<string> Releases = new List<string>();
            //兩環片間之連桿
            for (int j = 0; j < RingLinks.Count; ++j)
            {
                string[] JJ = RingLinks[j].Split(',');
                Releases.Add($"{JJ[0]},Yes,No,No,No,No,No,No,No,No,Yes,No,No,No");
            }
            input.PutDataToSheet("Frame Releases 1 - General", Releases);
        }
        #endregion

        #region  土壤彈簧連桿性質  Link Props 05 - Gap ; Link Property Assignments ; Link Props 01 - General; Frame Section Propertis 01 - General ;  
        public void Connectivity_Link_And_Link_Property_Assignments()
        {   //土壤彈簧桿件
            Spring_Link_And_Link_Property(9, Spring1s, "R1");   //第一環片土壤彈簧連結
            Spring_Link_And_Link_Property(55, Spring2s, "R2");  //第二環片土壤彈簧連結 
            input.PutDataToSheet("Connectivity - Link", SpringLinks);
        }

        public void Spring_Link_And_Link_Property(int ii, List<string> Springs, string ring)
        {   //土壤彈簧桿件  
            List<string> LinkPy = new List<string>();
            for (int j = 0; j < Springs.Count; ++j)
            {
                string[] JJ = Springs[j].Split(',');
                string LL = "L" + JJ[0].Substring(1);
                SpringLinks.Add($"{LL}, {ii}, {JJ[0]}");     
                int jj = ii;
                if (jj > Joint1s.Count) jj = jj - Joint1s.Count;
                string XX = Convert.ToString(JointsCoord[jj].θ);
                string result = "";
                switch (ring)
                {
                    case "R1":
                        result = LinkProperty.FirstOrDefault(s => s.Contains(XX));
                        break;
                    case "R2":
                        result = LinkPropertyR2.FirstOrDefault(s => s.Contains(XX));
                        break;
                }
                
                LinkPy.Add($"{LL},Gap,TwoJoint,{result},None");
                ++ii;
            }
            input.PutDataToSheet("Link Property Assignments", LinkPy);
        }

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

            List<string> ThataDistinct = JointsTheta.Distinct().ToList() ;

            for (int jj = 0; jj < ThataDistinct.Count; ++jj)
            {
                string XX = ThataDistinct[jj];
                AngleValue = float.Parse(XX);    
                LinkPropertyProcess(AngleValue, Ks, LinkProperty05, LinkProperty01, "Half");
                LinkPropertyProcess(AngleValue, Ks, LinkProperty05, LinkProperty01, "Full");
            }
            input.PutDataToSheet("Link Props 05 - Gap", LinkProperty05);//Spring-Theta=9,U1,No,Yes,11356.83,0,11356.83,0
            input.PutDataToSheet("Link Props 01 - General", LinkProperty01); //Spring-Theta=9,Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark		
        }

        public void LinkPropertyProcess(float AngleValue, double Ks, List<string> LinkProperty05, List<string> LinkProperty01, string ForH)
        {
            double KKK = 0;
            string XXX = "";
            KKK = Ks * 1.0 * UseRadius * AngleValue * Math.PI / 180;
            switch (ForH.ToUpper())
            {
                case "HALF":
                    XXX = string.Format("{0:0.00}", KKK/2);
                    LinkPropertyR2.Add($"Spring-Theta={AngleValue} Half");
                    LinkProperty05.Add($"Spring-Theta={AngleValue} Half,U1,No,Yes,{XXX},0,{XXX},0");
                    LinkProperty01.Add($"Spring-Theta={AngleValue} Half,Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark");
                    break;
                case "FULL":
                    XXX = string.Format("{0:0.00}", KKK);
                    LinkProperty.Add($"Spring-Theta={AngleValue} Full");
                    LinkProperty05.Add($"Spring-Theta={AngleValue} Full,U1,No,Yes,{XXX},0,{XXX},0");
                    LinkProperty01.Add($"Spring-Theta={AngleValue} Full,Gap,0,0,0.0001,0.0001,0.0001,1,1,0,0,0,0,Gray8Dark");
                    break;
            }
            
        }

        #endregion

        #region Frame Loads - Distributed
        public void Frame_Loads_Distributed()      //Frame Props 01 - General  && Frame Props 02 - Concrete Col
        {
            List<string> HLoads = new List<string>(); //水平載重
            List<string> VLoads = new List<string>(); //垂直載重
            double Pv = verticalStress.PvTop;
            for (int j = 0; j < Member1s.Count; ++j)
            {   //確認桿件的位置       
                string[] JJ = Member1s[j].Split(',');
                string LL = JJ[0];
                string v1 = "";    //應力值
                string v2 = "";    //應力值
                string type = "";  //回傳是牆(WALL)或底板(BASE)
                LoadCalculate(JJ[1].Trim(), JJ[2].Trim(), ref v1, ref v2, ref type, Joint1s);
                HLoads.Add($"{LL},Soil Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{v1},{v2}"); //水平載重
                switch (type)
                {
                    case "DOWN":
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{Pv},{Pv}");//垂直載重
                        break;
                    case "UP":
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{-Pv},{-Pv}");//垂直載重
                        break;
                    default:
                        break;
                }
            }

            for (int j = 0; j < Member2s.Count; ++j)
            {   //確認桿件的位置       
                string[] JJ = Member2s[j].Split(',');
                string LL = JJ[0];
                string v1 = "";    //應力值
                string v2 = "";    //應力值
                string type = "";  //回傳是牆(WALL)或底板(BASE)
                LoadCalculate(JJ[1].Trim(), JJ[2].Trim(), ref v1, ref v2, ref type, Joint2s);
                v1 = (double.Parse(v1) / 2).ToString();
                v2 = (double.Parse(v2) / 2).ToString();
                double halfPv = Pv / 2;
                HLoads.Add($"{LL},Soil Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{v1},{v2}"); //水平載重
                switch (type)
                {
                    case "DOWN":
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{halfPv},{halfPv}");//垂直載重
                        break;
                    case "UP":
                        VLoads.Add($"{LL},Soil Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{-halfPv},{-halfPv}");//垂直載重
                        break;
                    default:
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
        public void LoadCalculate(string L1, string L2, ref string v1, ref string v2, ref string type, List<string> xJoints)
        {
            double Ph1 = verticalStress.LongTermPh1;
            double Ph2 = verticalStress.LongTermPh2;
            float HH = UseRadius * 2;
            float X1 = 0;
            float Z1 = 0;
            float X2 = 0;
            float Z2 = 0;

            for (int j = 0; j < xJoints.Count; ++j) //L1,	Gap, TwoJoint, Spring-Vault, None
            {   //       
                string[] JJ = xJoints[j].Split(',');
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

        #region Uncouple Grid Lines
        public void JtSpringAssignsUncoupled()
        {
            List<string> uncouple = new List<string>();
            uncouple.Add("31,Local,1000,0,0,0,0,0");
            uncouple.Add("69,Local,1000,0,0,0,0,0");

            input.PutDataToSheet("Jt Spring Assigns 1 - Uncoupled", uncouple);
        }

        public void GridLines()
        {
            List<string> grid = new List<string>();
            List<double> Coor = new List<double> { UseRadius * (-1), 0, UseRadius };
            for(int i = 0; i < 3; i++)
                grid.Add($"GLOBAL,X,,{Coor[i]},,Primary,Gary8Dark,Yes,End");

            grid.Add("GLOBAL,Y,R1,0,,Primary,Gary8Dark,Yes,Start");
            grid.Add("GLOBAL,Y,R2,2,,Primary,Gary8Dark,Yes,Start");

            for (int i = 0; i < 3; i++)
                grid.Add($"GLOBAL,Z,,{Coor[i]},,Primary,Gary8Dark,Yes,Start");

            input.PutDataToSheet("Grid Lines", grid);
        }
        #endregion

        #region TEMP




        #endregion

        #region SAPCalculation
        List<string> SGDegreeXYZ = new List<string>();
        List<string> SoilDegreeXYZ = new List<string>();
        List<float> R1SGAngle = new List<float>();
        List<float> R2SGAngle = new List<float>();
        List<string> frameName = new List<string>();
        List<string> R1FrameName = new List<string>();
        List<string> R2FrameName = new List<string>();
        List<string> R1contactingFrame = new List<string>();
        List<string> R2contactingFrame = new List<string>();
        int SGNumRing;
        public void FrameNameAssign()
        {            
            void XYZInput(ref List<Tuple<double, double, double, double>> Coordinate, double tempAngle, double RadiusInter, double RadiusWidth)
            {
                var data = Tuple.Create(tempAngle, RadiusInter * Math.Sin((tempAngle) * Math.PI / 180), RadiusWidth, RadiusInter * Math.Cos((tempAngle) * Math.PI / 180));
                Coordinate.Add(data);
            }
            SGDegreeXYZ.Clear();
            SoilDegreeXYZ.Clear();

            R1contactingFrame.Clear();
            R2contactingFrame.Clear();
            R1SGAngle.Clear();
            R2SGAngle.Clear();
            frameName.Clear();
            R1FrameName.Clear();
            R2FrameName.Clear();
            int SGAnum = 3;
            int SGBnum = 2;
            int SGKnum = 1;
            int sgSteelNum = 1;
            float steelAngle = 360 - SGAnum * Aangle - (SGBnum - 1) * Bangle;
            List<Tuple<double, double, double, double>> tempXYZ = new List<Tuple<double, double, double, double>>();
            List<Tuple<double, double, double, double>> tempSoilXYZ = new List<Tuple<double, double, double, double>>();

            
            for(int SGorSoil = 1; SGorSoil < 3; SGorSoil++)
            {
                tempXYZ.Clear();
                tempXYZ.Add(Tuple.Create(0.0,0.0,0.0, UseRadius * SGorSoil * Math.Cos(0 * Math.PI / 180)));
                //The first joint is at the top of the circle (also the first joint of bolt)

                float angle = 0;
                
                for(int i = 0; i < SGAnum + SGBnum - 1 + sgSteelNum; i++) //Ring1 sequence B -> Steel -> A -> A -> A
                {
                    if (i == 0) angle += (Bangle - AD1angle);
                    else if (i == 1) angle += steelAngle;
                    else angle += Aangle;

                    if(SGorSoil == 1) R1SGAngle.Add(angle);

                    double tempAngle = angle - 4;
                    for(int j = 0; j < 3; j++)
                    {
                        XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                        tempAngle += 4;
                    }

                    if (i == 0)
                    {
                        tempAngle -= 4; tempAngle += 15; tempAngle -= 4;
                        for (int j = 0; j < 3; j++)
                        {
                            XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                            tempAngle += 4;
                        }
                    }
                        
                    
                }

                angle = 0;
                for(int i = 0; i < SGAnum + SGBnum + SGKnum; i++) //Ring2 sequence B -> K -> B -> A -> A -> A
                {                    
                    if (i == 0) angle += AD2angle;
                    else if (i == 1) angle += Kangle;
                    else if (i == 2) angle += Bangle;
                    else angle += Aangle;

                    if(SGorSoil == 1) R2SGAngle.Add(angle);

                    double tempAngle = angle - 4;
                    for (int j = 0; j < 3; j++)
                    {
                        XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                        tempAngle += 4;
                    }

                    if (i > 0)
                    {
                        if (i == 1) //計算螺栓孔角度與點位
                        {
                            tempAngle = angle + AD2angle;
                            XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                            tempAngle += ADangle;
                            XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                        }
                        else if (i < (1 + 1 + SGAnum))
                        {
                            tempAngle = angle + AD1angle;
                            XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                            tempAngle += ADangle;
                            XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                        }
                        else
                        {
                            tempAngle = angle + AD1angle;
                            XYZInput(ref tempXYZ, tempAngle, UseRadius * SGorSoil, 0.0);
                        }
                    }
                    
                }

                tempXYZ = tempXYZ.OrderBy(t => t.Item1).ToList();                

                if(SGorSoil == 1)
                {
                    for (int i = 0; i < tempXYZ.Count; i++)
                        SGDegreeXYZ.Add($"{tempXYZ[i].Item1},{tempXYZ[i].Item2},{tempXYZ[i].Item3},{tempXYZ[i].Item4}");
                }
                else
                {
                    for(int i = 0; i < tempXYZ.Count; i++)
                        SoilDegreeXYZ.Add($"{tempXYZ[i].Item1},{tempXYZ[i].Item2},{tempXYZ[i].Item3},{tempXYZ[i].Item4}");
                }
            }
            SGNumRing = SGDegreeXYZ.Count;
                        
            for (int i = 0; i < Member1s.Count; i++)
            {
                string[] cut = Member1s[i].Split(',');
                frameName.Add(cut[0]);
                R1FrameName.Add(cut[0]);
            }

            for (int i = 0; i < Member2s.Count; i++)
            {
                string[] cut = Member2s[i].Split(',');
                frameName.Add(cut[0]);
                R2FrameName.Add(cut[0]);
            }

            for(int i = 0; i < SGNumRing; i++)
            {
                string[] cut = SGDegreeXYZ[i].Split(',');
                string angle = cut[0];
                for(int j = 0; j < R1SGAngle.Count; j++)
                {
                    if(R1SGAngle[j].ToString() == angle)
                    {
                        R1contactingFrame.Add(R1FrameName[i]);
                        break;
                    }
                }

                for(int j = 0; j < R2SGAngle.Count; j++)
                {
                    if(R2SGAngle[j].ToString() == angle)
                    {
                        R2contactingFrame.Add(R2FrameName[i-1]);
                        break;
                    }
                }
            }
            R1contactingFrame.Sort();
            R2contactingFrame.Sort();

            
        }

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
        //double MaxM = 0;
        //double MaxP = 0;
        //string OutUID = "";
        //double MaxV = 0;

        List<string> loadingUID = new List<string>();
        List<Tuple<double, double, double, double, double, double>> loadingValue = 
            new List<Tuple<double, double, double, double, double, double>>();
        List<Tuple<string, string, string>> loadingProp = new List<Tuple<string, string, string>>();
        List<Tuple<string, string>> jointProp = new List<Tuple<string, string>>();
        List<Tuple<double, double, double, double, double, double>> jtDisplacement = 
            new List<Tuple<double, double, double, double, double, double>>();

        public void SAPCalculation(string inputPath, string loadingName, string outputPath, string condition, 
            int realTimes, string loadType)
        {
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

            for (int i = 0; i < frameName.Count; i++)
            {
                ret = mySapModel.Results.FrameForce(frameName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref ObjSta, ref elm, ref ElmSta, ref LoadCase, ref StepType_test, ref StepNum_test, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

                var forceProp = Tuple.Create(frameName[i], loadType, frameName[i]);
                loadingProp.Add(forceProp);

                try
                {
                    forceProp = Tuple.Create(frameName[i], loadType, frameName[i + 1]);
                    loadingProp.Add(forceProp);
                }
                catch
                {
                    forceProp = Tuple.Create(frameName[i], loadType, frameName[0]);
                    loadingProp.Add(forceProp);
                }

                for (int j = 0; j < 2; j++)
                {
                    var forceValue = Tuple.Create(P[j], V2[j], V3[j], T[j], M2[j], M3[j]);
                    loadingValue.Add(forceValue);
                    loadingUID.Add(Guid.NewGuid().ToString("D"));
                }
                ret = mySapModel.Results.JointDispl(frameName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref elm, ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                var dispProp = Tuple.Create(frameName[i], loadType);
                jointProp.Add(dispProp);
                var dispValue = Tuple.Create(U1[0], U2[0], U3[0], R1[0], R2[0], R3[0]);
                jtDisplacement.Add(dispValue);
            }

            input.CreateResultFile(wb, loadingProp, loadingValue, loadType);
            input.CreateDisplacementFile(wb, jointProp, jtDisplacement, "JointDisplacement");
            input.FileSaving(wb, outputPath);

            
            try
            {
                oExcuteSQL.DeleteDataBySectionUIDAndTimesAndLoadType($"STN_{condition}Data", sectionUID, realTimes, loadType);
                //oExcuteSQL.DeleteDataBySectionUIDAndTimes($"STN_{condition}Data", sectionUID, realTimes);
            }
            catch
            {
            }
            oExcuteSQL.InsertSAPData($"STN_{condition}Data", loadingUID, sectionUID, loadingProp, realTimes, loadingValue);


            List<Tuple<string, int, string, string, string, double>> calDepth = 
                new List<Tuple<string, int, string, string, string, double>>();
            List<string> inputUID = new List<string>();

            bool temp = true;
            int t = 0;
            for (int i = 0; i < loadingProp.Count; i++)
            {
                if (R1contactingFrame.Contains(loadingProp[i].Item1))
                {
                    if (temp)
                    {

                        if (realTimes == 4)
                        {
                            var data = Tuple.Create(loadType, realTimes, loadingProp[i].Item1, loadingProp[i].Item1
                            , loadingUID[i], assignDepth[t]);
                            calDepth.Add(data);
                            t++;
                        }
                        else
                        {
                            var data = Tuple.Create(loadType, realTimes, loadingProp[i].Item1, loadingProp[i].Item1
                            , loadingUID[i], assignDepth[0]);
                            calDepth.Add(data);
                        }
                        inputUID.Add(Guid.NewGuid().ToString("D"));
                    }
                    temp = !temp;
                }

                if (R2contactingFrame.Contains(loadingProp[i].Item1))
                {
                    if (!temp)
                    {
                        if (realTimes == 4)
                        {
                            var data = Tuple.Create(loadType, realTimes, loadingProp[i].Item1, loadingProp[i].Item1
                            , loadingUID[i], assignDepth[t]);
                            calDepth.Add(data);
                            t++;
                        }
                        else
                        {
                            var data = Tuple.Create(loadType, realTimes, loadingProp[i].Item1, loadingProp[i].Item1
                            , loadingUID[i], assignDepth[0]);
                            calDepth.Add(data);
                        }
                        inputUID.Add(Guid.NewGuid().ToString("D"));
                    }
                    temp = !temp;
                }

            }
            try { oExcuteSQL.DeleteCalDepth(sectionUID, loadType, realTimes); }
            catch { }
            oExcuteSQL.InsertSGCalDepthData(inputUID, sectionUID, calDepth);

            double maxM = 0.0;
            double maxP = 0.0;
            string maxUID = "";
            if(realTimes == 4)
            {
                for(int i = 0; i < loadingProp.Count; i++)
                {
                    if (steelFrameName.Contains(loadingProp[i].Item1))
                    {
                        double tempM = Math.Abs(loadingValue[i].Item6);
                        double tempP = Math.Abs(loadingValue[i].Item1);
                        bool max = false;
                        if (tempM > maxM) max = true;
                        else if (tempM == maxM && tempP < maxP) max = true;
                        
                        if (max)
                        {
                            maxM = tempM;
                            maxP = tempP;
                            maxUID = loadingUID[i];
                        }
                    }
                }

                switch (loadType)
                {
                    case "Steel_Origin":
                        oExcuteSQL.UpdateData("STN_Section", "UID", sectionUID, $"LLI4_ChosenData", maxUID);
                        break;
                    case "Steel_Cut":
                        oExcuteSQL.UpdateData("STN_Section", "UID", sectionUID, $"SLI4_ChosenData", maxUID);
                        break;
                }
            }

            mySapObject.ApplicationExit(true);
        }

        #endregion
    }
}