using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using STN_SQL;
using System.Data;
using SAP2000v15;
using System.Data.SqlClient;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SinoTunnel
{
    class Mat
    {
        public List<string> UID;
        public List<string> name;
        public List<double> UT;
        public double E;
        public double U;
        public double CTE;
        public double Fc;
    }


    class SAP_SegmentProcess
    {
        GetWebData p;
        ExcuteSQL datasearch = new ExcuteSQL();
        int ret;
        IWorkbook wb;
        Excel2Word word = new Excel2Word();

        string sectionUID = "";
        string groutingfilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\Grouting_Input_Raw.xlsx";
        string longTermFilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\DoubleRing_LongTerm_Input_Raw.xlsx";
        //string shortTermFilePath = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\DoubleRing_ShortTerm_Input_Raw.xlsx";
		//string groutingfilePath = @"P:\8014\Grouting_Input_Raw.xlsx";
		//string longTermFilePath = @"P:\8014\DoubleRing_LongTerm_Input_Raw.xlsx";
        string STN_GroutingData = "dbo.STN_GroutingData";
        string STN_Section = "dbo.STN_Section";

        SAP_DataCalculation oSAP_DataCalculation;
        SAP_ExcelInput input = new SAP_ExcelInput();

        //[name type unitweight, E, U12, CTE, Fc]
        List<Tuple<string, string, double, double, double, double, double>> material = new List<Tuple<string, string, double, double, double, double, double>>();
        //Rectangular Section [name, material, shape, T3, T2, type]
        List<Tuple<string, string, string, double, double, string>> frame = new List<Tuple<string, string, string, double, double, string>>();

        public double resultVariation;

        private double targetVariation; //地震造成的直徑變化增加量
        public double TargetVariation
        {
            get { return targetVariation; }
            set { targetVariation = value; }
        }

        private double eqDia; //常時載重(未加地震、長期)狀態的直徑變位量
        public double EQDia
        {
            get { return eqDia; }
            set { eqDia = value; }
        }

        private List<string> precastContDepth;
        public List<string> PrecastContDepth
        {
            get { return precastContDepth; }
            //set { for (int i = 0; i < value.Count; i++) precastContDepth.Add(value[i]); }
            set { precastContDepth = value; }
            
        }

        /*
        private double decreasedXaxisP;
        public double DecreasedXaxisP
        {
            get { return decreasedXaxisP; }
            set { decreasedXaxisP = value; }
        }

        private double increasedZaxisP;
        public double IncreasedZaxisP
        {
            get { return increasedZaxisP; }
            set { increasedZaxisP = value; }
        }
        */
        double Fc;
        double E;
        double U12;
        double UT;
        double width;
        double contactDepth;
        double fullDepth;
        SAP2000v15.SapObject mySapObject;
        SAP2000v15.cSapModel mySapModel;
        public SAP_SegmentProcess(string sectionUID, string condition, double decreasedXaxisP, double increasedZaxisP)
        {
            this.sectionUID = sectionUID;

            switch (condition)//土層載重及建物載重計算  //指向EXCEL樣板檔.
            {
                case "Grouting": oSAP_DataCalculation = new SAP_DataCalculation(groutingfilePath, sectionUID); break;
                case "LongTerm":
                case "ShortTerm":
                case "VariationofDiameter":
                case "EQofDiameter":
                    oSAP_DataCalculation = new SAP_DataCalculation(longTermFilePath, sectionUID);
                    break;
            }

            this.p = new GetWebData(sectionUID);  //  取得網路變數資料
            oSAP_DataCalculation.DecreasedXaxisP = decreasedXaxisP;
            oSAP_DataCalculation.IncreasedZaxisP = increasedZaxisP;

            this.Fc = p.segmentFc;
            this.E = p.segmentYoungsModulus;
            this.U12 = p.segmentPoissonRatio;
            this.UT = p.segmentUnitWeight;
            this.width = p.segmentWidth;
            this.contactDepth = p.segmentContacingDepth;
            this.fullDepth = p.segmentThickness;

            //mySapObject = new SAP2000v15.SapObject();
            //mySapModel = mySapObject.SapModel;            
        }

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

        double maxM = 0;
        double maxP = 0;
        List<Tuple<int, int>> maxNum = new List<Tuple<int, int>>();
        List<string> inputUID = new List<string>();
        string outUID = "";


        #region Grouting Calculation
        public void GroutingCalculation(string fileSavingPath, out double Mmax, out double Pmax, bool excelOnly)
        {
            Mmax = 0.0;
            Pmax = 0.0;
            string originalPath = fileSavingPath;
            string resultPath = fileSavingPath.Replace(".xlsx", "_GroutingResult.xlsx");
            fileSavingPath = fileSavingPath.Replace(".xlsx", "_GroutingInput.xlsx");
            int groutingCutPart = int.Parse(p.segment.Rows[0]["BFG_N"].ToString());
            double groutingDegree = double.Parse(p.segment.Rows[0]["BFG_SGAngle"].ToString());
            double groutingDistributedLoad = double.Parse(p.segment.Rows[0]["BFG_W"].ToString());

            List<string> groutingCase = new List<string>();
            groutingCase.Add("GroutingHalf");
            groutingCase.Add("GroutingFull");
            List<string> groutingCombo = new List<string>();
            groutingCombo.Add($"DL+{groutingCase[0]}");
            groutingCombo.Add($"DL+{groutingCase[1]}");
            oSAP_DataCalculation.GroutingLoading(groutingCase, groutingCombo);

            //***********************************************************
            //採用新方法抓取環片材料與桿件性質
            DataTable dt = datasearch.GetBySection("STN_FrameMaterial", sectionUID, "");
            double depth = 0.0;
            for (int i = 0; i < dt.Rows.Count; i++)
                if (dt.Rows[i]["LoadType"].ToString() == "Grouting")
                    depth = double.Parse(dt.Rows[i]["Depth"].ToString());            

            string matName = $"Concrete {Fc} t={depth*100}cm T={fullDepth*100}cm";
            material.Add(Tuple.Create(matName, "Concrete", UT, E, U12, 9.9E-6, Fc * 98));
            
            string frameName = $"Segment {width * 100}x{depth * 100}cm";
            frame.Add(Tuple.Create(frameName, matName, "Rectangular", depth, width, "Concrete"));
            //***********************************************************

            //string segmentUID = p.segment.Rows[0]["BFG_Segment"].ToString();
            //DataTable sectionProp = datasearch.GetByUID("STN_Segment", segmentUID);
            //string materialUID = sectionProp.Rows[0]["Material"].ToString();
            //DataTable sectionMaterial = datasearch.GetByUID("STN_SegmentMaterial", materialUID);

            //[name type unitweight, E, U12, CTE, Fc]
            //material.Add(Tuple.Create(sectionMaterial.Rows[0]["MaterialName"].ToString(), sectionMaterial.Rows[0]["MaterialType"].ToString(), double.Parse(sectionMaterial.Rows[0]["UnitWeight"].ToString()), double.Parse(sectionMaterial.Rows[0]["YoungModulus"].ToString()), double.Parse(sectionMaterial.Rows[0]["PoissonRatio"].ToString()), double.Parse(sectionMaterial.Rows[0]["CTE"].ToString()), double.Parse(sectionMaterial.Rows[0]["Fc"].ToString())));
            oSAP_DataCalculation.SetMaterial(material);

            //Rectangular Section[name, material, shape, T3(depth), T2(width), type]
            //frame.Add(Tuple.Create(sectionProp.Rows[0]["Name"].ToString(), sectionMaterial.Rows[0]["MaterialName"].ToString(), sectionProp.Rows[0]["Shape"].ToString(), double.Parse(sectionProp.Rows[0]["Height"].ToString()), double.Parse(sectionProp.Rows[0]["Width"].ToString()), sectionMaterial.Rows[0]["MaterialType"].ToString()));
            oSAP_DataCalculation.SetFrameSection(frame);

            oSAP_DataCalculation.GroutingCoordinates(p.segmentRadiusInter, groutingCutPart, groutingDegree);
            oSAP_DataCalculation.GroutingDistributedLoad(groutingDistributedLoad, groutingCutPart, groutingCase);

            //oSAP_DataCalculation.GroutingFrameSection(groutingCutPart, sectionProp.Rows[0]["Name"].ToString(), sectionProp.Rows[0]["Shape"].ToString());
            oSAP_DataCalculation.GroutingFrameSection(groutingCutPart, frameName, "Rectangular");

            oSAP_DataCalculation.FileSave(fileSavingPath);

            //input = new SAP_ExcelInput(fileSavingPath);
            //input.FileSaving(fileSavingPath);

            // all worksheet
            List<string> names = new List<string>();
            names.Add("Joint Coordinates");
            names.Add("Joint Restraint Assignments");
            names.Add("Connectivity - Frame");
            names.Add("Frame Props 01 - General");
            names.Add("Frame Section Assignments");
            names.Add("Frame Loads - Distributed");
            names.Add("MatProp 01 - General");
            names.Add("MatProp 02 - Basic Mech Props");
            names.Add("MatProp 03b - Concrete Data");
            names.Add("Load Case Definitions");
            names.Add("Load Pattern Definitions");
            names.Add("Case - Static 1 - Load Assigns");
            names.Add("Combination Definitions");
            names.Add("Program Control");
            string wordpath = originalPath.Replace(".xlsx", $".docx");
            word.Add(fileSavingPath, names, false, false);

            if (excelOnly)
            {
                //word.FileSaving(wordpath);
                return;
            }
                

            bool temp_bool = true;

            mySapObject = new SAP2000v15.SapObject();
            mySapModel = mySapObject.SapModel;
            mySapObject.ApplicationStart(SAP2000v15.eUnits.kip_ft_F, temp_bool, "");

            ret = mySapModel.File.OpenFile(fileSavingPath);
            ret = mySapModel.Analyze.RunAnalysis();

            //double[,] groutingValue = new double[(groutingCutPart - 1)*2, 6];
            List<Tuple<double, double, double, double, double, double>> groutingValue = new List<Tuple<double, double, double, double, double, double>>();
            List<Tuple<int, string, int>> groutingProp = new List<Tuple<int, string, int>>();

            string[] groutingID = new string[2];
            groutingID[0] = "BFG_GroutingHalf";
            groutingID[1] = "BFG_GroutingFull";
            maxNum.Add(Tuple.Create(1, 1));

            //try
            //{
            //    datasearch.DeleteDataBySectionUID(STN_GroutingData, sectionUID);
            //}
            //catch
            //{
            //}

            wb = new XSSFWorkbook();
            for (int k = 0; k < groutingCombo.Count; k++)
            {
                ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
                ret = mySapModel.Results.Setup.SetComboSelectedForOutput(groutingCombo[k], true);
                groutingValue.Clear();
                groutingProp.Clear();
                inputUID.Clear();
                int m = 1;
                int n = 1;
                int s = 0;
                bool reverse = false;
                double tempM = 0;

                for (int i = 0; i < groutingCutPart - 1; i++)
                {
                    ret = mySapModel.Results.FrameForce((i + 1).ToString(), SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref ObjSta, ref elm, ref ElmSta, ref LoadCase, ref StepType_test, ref StepNum_test, ref P, ref V2, ref V3, ref T, ref M2, ref M3);
                    for (int j = 0; j < P.Length; j++)
                    {
                        inputUID.Add(Guid.NewGuid().ToString("D"));
                        var data01 = Tuple.Create(n, groutingCombo[k], m);
                        reverse = !reverse;
                        if (reverse) m++;
                        else n++;
                        groutingProp.Add(data01);
                        var data = Tuple.Create(double.Parse(P[j].ToString()), double.Parse(V2[j].ToString()), double.Parse(V3[j].ToString()), double.Parse(T[j].ToString()), double.Parse(M2[j].ToString()), double.Parse(M3[j].ToString()));
                        groutingValue.Add(data);

                        if (Math.Abs(M3[j]) > maxM)
                        {
                            maxM = Math.Abs(M3[j]);
                            maxP = Math.Abs(P[j]);
                            maxNum[0] = Tuple.Create(i, j);
                        }
                        if (Math.Abs(M3[j]) > tempM)
                        {
                            tempM = Math.Abs(M3[j]);
                            outUID = inputUID[i + j + s];
                        }
                    }
                    s++;
                }
                input.CreateResultFile(wb, groutingProp, groutingValue, groutingCombo[k]);
                datasearch.InsertSAPData(STN_GroutingData, inputUID, sectionUID, groutingProp, groutingValue);
                datasearch.UpdateData(STN_Section, "UID", sectionUID, groutingID[k], outUID);
            }
            input.FileSaving(wb, resultPath);

            word.Add(resultPath, names, true, false);
            word.FileSaving(wordpath);

            Mmax = maxM;
            Pmax = maxP;

            mySapObject.ApplicationExit(true);
        }
        #endregion

        #region Loading Calculation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileSavingPath"></param>
        /// <param name="condition">載重條件</param>
        /// <param name="times">第幾次計算</param>
        /// <param name="getResult">0.33直徑變化與地震載重之直徑變化是否符合設計要求</param>
        /// <param name="eqBool"></param>
        /// <param name="excelOnly">是否只輸出excel輸入檔</param>
        public void LoadingTerm(string fileSavingPath, string condition, int times, out bool getResult, bool eqBool, bool excelOnly)
        {
            //次數從0開始算
            int realTimes = times;
            times -= 1;

            List<string> resultPath = new List<string>();
            List<string> fileSavingProgress = new List<string>();

            if (condition == "LongTerm" || condition == "ShortTerm" || condition == "VariationofDiameter" || condition == "EQofDiameter")
            {
                fileSavingProgress.Add(fileSavingPath.Replace(".xlsx", $"_{condition}Input1st.xlsx"));
                fileSavingProgress.Add(fileSavingPath.Replace(".xlsx", $"_{condition}Input2nd.xlsx"));
                fileSavingProgress.Add(fileSavingPath.Replace(".xlsx", $"_{condition}Input3rd.xlsx"));
                fileSavingProgress.Add(fileSavingPath.Replace(".xlsx", $"_{condition}InputFinal.xlsx"));

                resultPath.Add(fileSavingPath.Replace(".xlsx", $"_{condition}Result1st.xlsx"));
                resultPath.Add(fileSavingPath.Replace(".xlsx", $"_{condition}Result2nd.xlsx"));
                resultPath.Add(fileSavingPath.Replace(".xlsx", $"_{condition}Result3rd.xlsx"));
                resultPath.Add(fileSavingPath.Replace(".xlsx", $"_{condition}ResultFinal.xlsx"));
            }

            //fileSavingPath = fileSavingPath.Replace(".xlsx", "_LongtermInput.xlsx");
            //string projectUID = p.segment.Rows[0]["Project"].ToString();

            //ContactingDepth();

            //List<double> segmentAngle = new List<double>();
            //segmentAngle.Add(p.segmentAAngle);
            //segmentAngle.Add(p.segmentBAngle);
            //segmentAngle.Add(p.segmentKAngle);
            //segmentAngle.Add(p.segmentAdjacentPoreAngle);


            //********************************************************************************************************
            List<string> distContDepth = new List<string>();
            if(realTimes == 4) distContDepth = precastContDepth.Distinct().ToList();

            List<string> sectionAssign = new List<string>();
            List<double> assignDepth = new List<double>();

            material.Clear();
            frame.Clear();
            DataTable mat = datasearch.GetBySection("STN_FrameMaterial", sectionUID, "ORDER BY TIMES ASC");

            List<double> contDepth = new List<double>();
            for (int i = 0; i < mat.Rows.Count; i++)
                if (mat.Rows[i]["LoadType"].ToString() == "Precast")
                    contDepth.Add(Math.Round(double.Parse(mat.Rows[i]["Depth"].ToString()), 4));
            string matName = "";
            double depth;
            depth = contDepth[0];
            matName = $"Concrete {Fc} t={depth * 100}cm T={fullDepth * 100}cm";
            material.Add(Tuple.Create(matName, "Concrete", UT, E, U12, 9.9E-6, Fc * 98));
            //assignDepth.Add(depth);

            string frameName = "";
            frameName = $"Segment {width * 100 / 2}x{depth * 100}cm";
            sectionAssign.Add(frameName);
            frame.Add(Tuple.Create(frameName, matName, "Rectangular", depth, width / 2, "Concrete"));

            if (realTimes == 4)
            {
                for (int i = 0; i < distContDepth.Count; i++)
                {
                    depth = double.Parse(distContDepth[i]);
                    matName = $"Concrete {Fc} t={depth * 100}cm T={fullDepth * 100}cm";
                    material.Add(Tuple.Create(matName, "Concrete", UT * fullDepth / depth, E, U12, 9.9E-6, Fc * 98));                    

                    frameName = $"Segment {width * 100 / 2}x{depth * 100}cm";
                    frame.Add(Tuple.Create(frameName, matName, "Rectangular", depth, width / 2, "Concrete"));
                }

                for(int i = 0; i < precastContDepth.Count; i++)
                {
                    depth = double.Parse(precastContDepth[i]);
                    frameName = $"Segment {width * 100 / 2}x{depth * 100}cm";
                    sectionAssign.Add(frameName);
                    assignDepth.Add(depth);
                }
            }
            else
            {
                switch (realTimes)
                {
                    case 1: depth = contDepth[1]; break;
                    case 2: depth = contDepth[2]; break;
                    case 3: depth = contDepth[3]; break;
                }
                matName = $"Concrete {Fc} t={depth * 100}cm T={fullDepth * 100}cm";
                material.Add(Tuple.Create(matName, "Concrete", UT * fullDepth / depth, E, U12, 9.9E-6, Fc * 98));
                assignDepth.Add(depth);

                frameName = $"Segment {width * 100 / 2}x{depth * 100}cm";
                frame.Add(Tuple.Create(frameName, matName, "Rectangular", depth, width / 2, "Concrete"));
                sectionAssign.Add(frameName);
                
            }
                        
            //List<string> SegmentUID = new List<string>();
            //SegmentUID.Add(datasearch.GetByUID("STN_Section", sectionUID).Rows[0]["Load_Segment"].ToString());
            //if (condition == "LongTerm" || condition == "ShortTerm" || condition == "VariationofDiameter" || condition == "EQofDiameter")
            //{
            //    if (realTimes != 4)
            //        SegmentUID.Add(datasearch.GetByUID("STN_Section", sectionUID).Rows[0][$"LLI{realTimes}_Segment"].ToString());
            //    //********************************************************************************************************
            //    else //第四次計算要針對每個環片接點輸入接觸深度
            //    {
            //        DataTable sgCalDepth = datasearch.GetBySection("STN_SGCalDepth", sectionUID, "");
            //        List<Tuple<string, string, string>> tempUID = new List<Tuple<string, string, string>>();
            //        for (int i = 0; i < sgCalDepth.Rows.Count; i++)
            //        {
            //            if (sgCalDepth.Rows[i]["LoadType"].ToString() == condition && int.Parse(sgCalDepth.Rows[i]["TimesOfLoad"].ToString()) == realTimes)
            //            {
            //                var data = Tuple.Create(sgCalDepth.Rows[i]["Member"].ToString(), sgCalDepth.Rows[i]["Joint"].ToString(), sgCalDepth.Rows[i]["SegmentUID"].ToString());
            //                tempUID.Add(data);
            //            }
            //        }
            //        tempUID = tempUID.OrderBy(t => t.Item1).ThenBy(T => T.Item2).ToList();

            //        for (int i = 0; i < tempUID.Count; i++)
            //        {
            //            SegmentUID.Add(tempUID[i].Item3);
            //        }
            //    }
            //}
            /*
            else if(condition == "VariationofDiameter")
            {
                SegmentUID.Add(datasearch.GetByUID("STN_Section", sectionUID).Rows[0]["VarDia_Segment"].ToString());
            }*/
            //int segmentANum = 3;
            double L = 2;
            int SGAnum = 3;
            oSAP_DataCalculation.VerticalStress();
            oSAP_DataCalculation.SGJointsAndRestraint(SGAnum, out List<string> diameterJointName);
            oSAP_DataCalculation.SGConnectivityFrame_StationAssigns(out List<string> contactingFrameName);
            oSAP_DataCalculation.FrameSectionAssigns(SGAnum, sectionAssign, realTimes, out List<double> D);
            oSAP_DataCalculation.SGConnectivityLink();
            oSAP_DataCalculation.LinkPropAndGapAndAssign(condition);
            oSAP_DataCalculation.Uncoupled();
            oSAP_DataCalculation.FrameLoad_Distributed(condition);


            //********************************************************************************************************
            for(int i = 0; i < oSAP_DataCalculation.SGhalfDistinct.Count; i++)
            {
                frameName = $"InterRingAngle = {oSAP_DataCalculation.SGhalfDistinct[i]}";
                frame.Add(Tuple.Create(frameName, "Inter-Ring Shear", "Circle", D[i], 0.0, "Concrete"));
            }
            //for (int i = 0; i < D.Count; i++)
            //{
            //    frameName = $"InterRingAngle = {oSAP_DataCalculation.SGhalfDistinct[i]}";
            //    frame.Add(Tuple.Create(frameName, "Inter-Ring Shear", "Circle", D[i], 0.0, "Concrete"));
            //}

            material.Add(Tuple.Create("Inter-Ring Shear", "Concrete", 0.0, 1E10, U12, 9.9E-6, Fc * 98));
            oSAP_DataCalculation.SetMaterial(material);
            oSAP_DataCalculation.SetFrameSection(frame);

            //SetFrameMaterial(realTimes, distContDepth, D);
            //********************************************************************************************************

            //---------------------------------------------------------------
            //點位佈設、均佈力加載、桿件配置、土壤彈簧配置
            List<string> ringFrameName = new List<string>();
            //ringFrameName = oSAP_DataCalculation.LoadingCoordinates(p.projectUID, p.segmentRadiusOut, p.segmentThickness, p.segmentWidth, segmentAngle, segmentANum, L, p.segmentWidth, condition, SegmentUID, out List<string> contactingFrameName, out List<string> diameterJointName);
            //---------------------------------------------------------------
            foreach (string str in oSAP_DataCalculation.frameNameRing1)
                ringFrameName.Add(str);
            foreach (string str in oSAP_DataCalculation.frameNameRing2)
                ringFrameName.Add(str);


            List<string> loadingtermPattern = new List<string>();
            List<Tuple<string, string>> loadCase = new List<Tuple<string, string>>();
            List<string> NLLoad = new List<string>();

            List<Tuple<string, string>> loadCondition = new List<Tuple<string, string>>();
            loadCondition.Add(Tuple.Create("LongTerm", "Long Term"));
            loadCondition.Add(Tuple.Create("ShortTerm", "Short Term"));
            loadCondition.Add(Tuple.Create("VariationofDiameter", "0.33%Diameter"));
            loadCondition.Add(Tuple.Create("EQofDiameter", "EQofDiameter"));

            for (int i = 0; i < loadCondition.Count; i++)
            {
                if (condition == loadCondition[i].Item1)
                {
                    loadingtermPattern.Add($"{loadCondition[i].Item2} Vertical");
                    loadingtermPattern.Add($"{loadCondition[i].Item2} Lateral");

                    loadCase.Add(Tuple.Create($"{loadCondition[i].Item2}", "LinStatic"));
                    loadCase.Add(Tuple.Create($"DL+{loadCondition[i].Item2}(NL)", "NonStatic"));

                    NLLoad.Add($"DL+{loadCondition[i].Item2}(NL)");
                    break;
                }
            }
            oSAP_DataCalculation.LoadingtermLoading(loadingtermPattern, loadCase, NLLoad);

            //********************************************************************************************************
            //frame.Clear();
            //DataTable frameData = datasearch.GetBySection("STN_Segment", sectionUID, "");
            //for (int i = 0; i < frameData.Rows.Count; i++)
            //{
            //    string materialUID = frameData.Rows[i]["Material"].ToString();
            //    string materialName = datasearch.GetByUID("STN_SegmentMaterial", materialUID).Rows[0]["MaterialName"].ToString();
            //    string materialType = datasearch.GetByUID("STN_SegmentMaterial", materialUID).Rows[0]["MaterialType"].ToString();
            //    frame.Add(Tuple.Create(frameData.Rows[i]["Name"].ToString(), materialName, frameData.Rows[i]["Shape"].ToString(), double.Parse(frameData.Rows[i]["Height"].ToString()), double.Parse(frameData.Rows[i]["Width"].ToString()), materialType));
            //}            
            //oSAP_DataCalculation.SetFrameSection(frame);
            //********************************************************************************************************

            oSAP_DataCalculation.GridLines(p.segmentRadiusInter, L);
            oSAP_DataCalculation.FileSave(fileSavingProgress[times]);
            //input.FileSaving(fileSavingProgress[times]);

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
            string wordpath = fileSavingPath.Replace(".xlsx", $".docx");
            word.Add(fileSavingProgress[times], names, false, false);

            getResult = true;//TEMP   0.33直徑與地震載重使用，只有當直徑變化過目標值後再擷取分析結果，TEMP

            if (excelOnly) return;
                
            //return;
            SAPCalculation(realTimes, fileSavingProgress, NLLoad[0], out getResult, condition,diameterJointName
                , eqBool, ringFrameName, resultPath, contactingFrameName,assignDepth, names, wordpath);
        }
        #endregion

        #region SAPCalculation
        public void SAPCalculation(int realTimes, List<string> fileSavingProgress, string loadSAPName, out bool getResult,
            string condition,List<string> diameterJointName, bool eqBool, List<string> ringFrameName
            , List<string> resultPath, List<string> contactingFrameName, List<double> assignDepth, List<string> names, string wordpath)
        {
            int times = realTimes - 1;            

            bool temp_bool = true;
            mySapObject = new SAP2000v15.SapObject();
            mySapModel = mySapObject.SapModel;
            mySapObject.ApplicationStart(SAP2000v15.eUnits.kip_ft_F, temp_bool, "");

            ret = mySapModel.File.OpenFile(fileSavingProgress[times]);
            ret = mySapModel.Analyze.RunAnalysis();

            wb = new XSSFWorkbook();

            ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
            ret = mySapModel.Results.Setup.SetCaseSelectedForOutput(loadSAPName, true);

            List<Tuple<double, double, double, double, double, double>> loadingValue = new List<Tuple<double, double, double, double, double, double>>();
            //[FrameName, Load, JointName]
            List<Tuple<string, string, string>> loadingProp = new List<Tuple<string, string, string>>();
            List<string> loadingUID = new List<string>();

            getResult = true;//0.33直徑與地震載重使用，只有當直徑變化過目標值後再擷取分析結果，

            if (condition == "VariationofDiameter" && realTimes == 3)
            {
                List<double> newDiameter = new List<double>();
                for (int i = 0; i < diameterJointName.Count; i++)
                {
                    ret = mySapModel.Results.JointDispl(diameterJointName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref elm, ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                    newDiameter.Add(U3[0]);
                }
                resultVariation = Math.Abs(newDiameter[1] - newDiameter[0]);

                if (resultVariation < targetVariation) getResult = false;
                else datasearch.UpdateData("STN_Section", "UID", sectionUID, "VarDia_Variation", resultVariation);
            }
            else if (condition == "EQofDiameter" && !eqBool)
            {
                List<double> newDiameter = new List<double>();
                for (int i = 0; i < diameterJointName.Count; i++)
                {
                    ret = mySapModel.Results.JointDispl(diameterJointName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref elm, ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                    newDiameter.Add(U3[0]);
                }
                if (realTimes == 1) //只有在第一次計算時要計算常時載重下的直徑變化
                    eqDia = (Math.Abs(newDiameter[1] - newDiameter[0]));

                double eqNewDia = Math.Abs(newDiameter[1] - newDiameter[0]); //加載地震後的直徑變化量

                getResult = false;
                if (realTimes == 3)
                {
                    resultVariation = Math.Abs(eqDia - eqNewDia); //地震造成直徑變化量 = 常時載重直徑變化量 - 加載地震後的直徑變化量
                    if (resultVariation > targetVariation) getResult = true;
                }
            }

            List<Tuple<string, string>> jointProp = new List<Tuple<string, string>>();
            List<Tuple<double, double, double, double, double, double>> jtDisplacement = new List<Tuple<double, double, double, double, double, double>>();
            
            //ret = mySapModel.Results.Setup.SetComboSelectedForOutput(NLLoad[0], true);            
            for (int i = 0; i < ringFrameName.Count; i++) //抓取分析結果的軸力彎矩
            {
                ret = mySapModel.Results.FrameForce(ringFrameName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref ObjSta, ref elm, ref ElmSta, ref LoadCase, ref StepType_test, ref StepNum_test, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

                if (i < ringFrameName.Count / 2) //第一環
                {
                    var data11 = Tuple.Create(ringFrameName[i], loadSAPName, oSAP_DataCalculation.jointNameRing1[i]);
                    loadingProp.Add(data11);

                    if (i == ringFrameName.Count / 2 - 1)
                    {
                        var data12 = Tuple.Create(ringFrameName[i], loadSAPName, oSAP_DataCalculation.jointNameRing1[0]);
                        loadingProp.Add(data12);
                    }
                    else
                    {
                        var data12 = Tuple.Create(ringFrameName[i], loadSAPName, oSAP_DataCalculation.jointNameRing1[i + 1]);
                        loadingProp.Add(data12);
                    }
                }
                else //第二環
                {
                    var data11 = Tuple.Create(ringFrameName[i], loadSAPName, oSAP_DataCalculation.jointNameRing2[i - (ringFrameName.Count / 2)]);
                    loadingProp.Add(data11);

                    if (i == ringFrameName.Count - 1)
                    {
                        var data12 = Tuple.Create(ringFrameName[i], loadSAPName, oSAP_DataCalculation.jointNameRing2[0]);
                        loadingProp.Add(data12);
                    }
                    else
                    {
                        var data12 = Tuple.Create(ringFrameName[i], loadSAPName, oSAP_DataCalculation.jointNameRing2[i - (ringFrameName.Count / 2) + 1]);
                        loadingProp.Add(data12);
                    }
                }

                //var data = Tuple.Create(ringFrameName[i], NLLoad[0], ringFrameName[i]);
                //loadingProp.Add(data);
                //try
                //{
                //    var data03 = Tuple.Create(ringFrameName[i], NLLoad[0], ringFrameName[i + 1]);
                //    loadingProp.Add(data03);
                //}
                //catch
                //{
                //    var data03 = Tuple.Create(ringFrameName[i], NLLoad[0], ringFrameName[0]);
                //    loadingProp.Add(data03);
                //}

                for (int j = 0; j < 2; j++) 
                {
                    var data01 = Tuple.Create(P[j], V2[j], V3[j], T[j], M2[j], M3[j]);
                    loadingValue.Add(data01);
                    loadingUID.Add(Guid.NewGuid().ToString("D"));
                }
                //抓取分析結果的節點變位
                //ret = mySapModel.Results.JointDispl(ringFrameName[i], SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref elm, ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                //var data02 = Tuple.Create(ringFrameName[i], loadSAPName);
                //jointProp.Add(data02);
                //var data04 = Tuple.Create(U1[0], U2[0], U3[0], R1[0], R2[0], R3[0]);
                //jtDisplacement.Add(data04);
            }

            List<string> jtName = new List<string>();
            foreach (string jt in oSAP_DataCalculation.jointNameRing1) jtName.Add(jt);
            foreach (string jt in oSAP_DataCalculation.jointNameRing2) jtName.Add(jt);

            foreach(string jt in jtName)//抓取分析結果的節點變位
            {
                ret = mySapModel.Results.JointDispl(jt, SAP2000v15.eItemTypeElm.ObjectElm, ref num, ref obj, ref elm, 
                    ref LoadCase, ref StepType_test, ref StepNum_test, ref U1, ref U2, ref U3, ref R1, ref R2, ref R3);
                var data02 = Tuple.Create(jt, loadSAPName);
                jointProp.Add(data02);
                var data04 = Tuple.Create(U1[0], U2[0], U3[0], R1[0], R2[0], R3[0]);
                jtDisplacement.Add(data04);
            }

            input.CreateResultFile(wb, loadingProp, loadingValue, loadSAPName);
            input.CreateDisplacementFile(wb, jointProp, jtDisplacement, "JointDisplacement");
            input.FileSaving(wb, resultPath[times]);

            if(realTimes == 4)
            {
                word.Add(resultPath[times], names, true, false);
                word.FileSaving(wordpath);
            }            

            maxM = 0;
            maxP = 0;
            if (getResult)
            {                
                //try
                //{
                //    datasearch.DeleteDataBySectionUIDAndTimes($"STN_{condition}Data", sectionUID, realTimes);
                //}
                //catch
                //{
                //}
                //datasearch.InsertSAPData($"STN_{condition}Data", loadingUID, sectionUID, loadingProp, realTimes, loadingValue);

                double SG_B_Vmax = 0;
                double SG_K_Vmax = 0;
                double SG_A_Vmax = 0;
                string B_VmaxUID = "";
                string K_VmaxUID = "";
                string A_VmaxUID = "";
                string AProp = "";
                string BProp = "";
                string KProp = "";
                if (realTimes == 4)
                {
                    for (int i = 0; i < loadingUID.Count; i++)
                    {
                        bool max = false; //抓取最大彎矩處，做為後續配筋參考力量
                        if (Math.Abs(loadingValue[i].Item6) > maxM)
                        {
                            max = true;
                        }
                        else if (Math.Abs(loadingValue[i].Item6) == maxM && Math.Abs(loadingValue[i].Item1) < maxP)
                        {
                            max = true;
                        }
                        if (max)
                        {
                            maxM = Math.Abs(loadingValue[i].Item6);
                            maxP = Math.Abs(loadingValue[i].Item1);
                            outUID = loadingUID[i];
                        }
                    }
                    //if (condition == "LongTerm")
                    //    datasearch.UpdateData("STN_Section", "UID", sectionUID, $"LLI{realTimes}_ChosenData", outUID);
                    //else if (condition == "ShortTerm")
                    //    datasearch.UpdateData("STN_Section", "UID", sectionUID, $"SLI{realTimes}_ChosenData", outUID);
                    //else if (condition == "VariationofDiameter")
                    //    datasearch.UpdateData("STN_Section", "UID", sectionUID, $"VarDia_ChosenData", outUID);
                    //else if (condition == "EQofDiameter")
                    //    datasearch.UpdateData("STN_Section", "UID", sectionUID, $"EQDia_ChosenData", outUID);

                    for(int i = 0; i < loadingProp.Count; i++) //抓 A B K 環片剪力最大處做剪力筋配筋參考
                    {
                        string [] sg = loadingProp[i].Item1.Split('_');
                        switch (sg[1])
                        {
                            case "B":
                                {
                                    if (Math.Abs(loadingValue[i].Item2) > Math.Abs(SG_B_Vmax))
                                    {
                                        SG_B_Vmax = loadingValue[i].Item2;
                                        B_VmaxUID = loadingUID[i];
                                        BProp = loadingProp[i].Item1;
                                    }
                                }
                                break;
                            case "K":
                                {
                                    if (Math.Abs(loadingValue[i].Item2) > Math.Abs(SG_K_Vmax))
                                    {
                                        SG_K_Vmax = loadingValue[i].Item2;
                                        K_VmaxUID = loadingUID[i];
                                        KProp = loadingProp[i].Item1;
                                    }
                                }
                                break;
                            case "A":
                                {
                                    if (Math.Abs(loadingValue[i].Item2) > Math.Abs(SG_A_Vmax))
                                    {
                                        SG_A_Vmax = loadingValue[i].Item2;
                                        A_VmaxUID = loadingUID[i];
                                        AProp = loadingProp[i].Item1;
                                    }
                                }
                                break;
                        }                  
                    }
                    //try
                    //{
                    //    datasearch.DeleteData("STN_SegmentStirrup", $"(Section={sectionUID}) AND (LoadType={condition})");
                    //    //datasearch.DeleteDataBySectionUID("STN_SegmentStirrup", sectionUID);
                    //}
                    //catch
                    //{
                    //}

                    ////string tempUID = Guid.NewGuid().ToString("D");
                    ////string insertSTR = $"(UID,Section,LoadType,SegmentA,SegmentB,SegmentK) VALUES ({tempUID},{sectionUID},{condition}," +
                    ////    $"{A_VmaxUID},{B_VmaxUID},{K_VmaxUID})";
                    ////datasearch.InsertData("STN_SegmentStirrup", insertSTR);

                    //datasearch.InsertData("STN_SegmentStirrup", "Section", sectionUID, "LoadType", condition);
                    //datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "LoadType", condition,
                    //    "SegmentA", A_VmaxUID);
                    //datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "LoadType", condition,
                    //    "SegmentB", B_VmaxUID);
                    //datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "LoadType", condition,
                    //    "SegmentK", K_VmaxUID);
                    ////datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "LoadType", condition);
                    ////datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "SegmentA", A_VmaxUID);
                    ////datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "SegmentB", B_VmaxUID);
                    ////datasearch.UpdateData("STN_SegmentStirrup", "Section", sectionUID, "SegmentK", K_VmaxUID);

                    ////Console.WriteLine($"{AProp}, {A_VmaxUID}, {SG_A_Vmax}");
                    ////Console.WriteLine($"{BProp}, {B_VmaxUID}, {SG_B_Vmax}");
                    ////Console.WriteLine($"{KProp}, {K_VmaxUID}, {SG_K_Vmax}");
                }

                //存入SGCalDepth，根據計算書，第一環取後frame的joint(頭)、第二環取前frame的joint(尾)，原因未知
                List<Tuple<string, int, string, string, string, double>> calDepth = new List<Tuple<string, int, string, string, string, double>>();
                inputUID.Clear();
                int t = 0;
                bool temp = true;
                for (int i = 0; i < loadingProp.Count; i++)
                {
                    for (int j = 0; j < contactingFrameName.Count; j++)
                    {
                        if (loadingProp[i].Item1 == contactingFrameName[j]) //frame 名稱跟接觸深度frame一樣時進入，會進入兩次
                        {                            
                            if (j < contactingFrameName.Count / 2) //第一環
                            {
                                if (temp) //後frame的頭joint
                                {
                                    switch (realTimes)
                                    {
                                        case 4:
                                            var data = Tuple.Create(condition, realTimes, loadingProp[i].Item1, 
                                                contactingFrameName[j], loadingUID[i], assignDepth[t]);                                            
                                            calDepth.Add(data);
                                            t++;
                                            break;
                                        default:
                                            var data01 = Tuple.Create(condition, realTimes, loadingProp[i].Item1,
                                                contactingFrameName[j], loadingUID[i], assignDepth[0]);
                                            calDepth.Add(data01);                                            
                                            break;
                                    }                                                                       
                                    inputUID.Add(Guid.NewGuid().ToString("D"));                                    
                                }
                                temp = !temp; //排除尾joint用
                            }
                            else //第二環
                            {
                                if (!temp) //前frame的尾joint，一開始為頭joint，先不取，等第二次進入(尾joint)再取                               
                                {
                                    switch (realTimes)
                                    {
                                        case 4:
                                            var data = Tuple.Create(condition, realTimes, loadingProp[i].Item1, 
                                                contactingFrameName[j], loadingUID[i - 1], assignDepth[t]);
                                            calDepth.Add(data);
                                            t++;
                                            break;
                                        default:
                                            var data01 = Tuple.Create(condition, realTimes, loadingProp[i].Item1,
                                                contactingFrameName[j], loadingUID[i - 1], assignDepth[0]);
                                            calDepth.Add(data01);                                            
                                            break;
                                    }                                    
                                    inputUID.Add(Guid.NewGuid().ToString("D"));
                                }
                                temp = !temp; //排除頭joint用
                            }
                            
                        }
                    }
                }
                //try
                //{
                //    datasearch.DeleteCalDepth(sectionUID, condition, realTimes);
                //}
                //catch
                //{
                //}
                //datasearch.InsertSGCalDepthData(inputUID, sectionUID, calDepth);

            }


            mySapObject.ApplicationExit(true);           
        }
        #endregion

        #region Set Material
        void SetFrameMaterial(int realTimes, List<string> distContDepth, List<double> D)
        {
            //********************************************************************************************************
            //bool tempBool = true;
            //DataTable materialData = datasearch.GetByKeyword("STN_SegmentMaterial", "Project", p.projectUID);
            //material.Clear();
            //for (int i = 0; i < materialData.Rows.Count; i++)
            //{
            //    material.Add(Tuple.Create(materialData.Rows[i]["MaterialName"].ToString(), materialData.Rows[i]["MaterialType"].ToString(), double.Parse(materialData.Rows[i]["UnitWeight"].ToString()), double.Parse(materialData.Rows[i]["YoungModulus"].ToString()), double.Parse(materialData.Rows[i]["PoissonRatio"].ToString()), double.Parse(materialData.Rows[i]["CTE"].ToString()), double.Parse(materialData.Rows[i]["Fc"].ToString())));
            //    if (materialData.Rows[i]["MaterialName"].ToString() == "Inter-Ring Shear") tempBool = false;
            //}
            //oSAP_DataCalculation.SetMaterial(material);  // 

            ////若無建置兩環桿件資訊，自動建置，名稱固定為"Inter-Ring Shear"
            //if (tempBool == true)
            //{
            //    List<string> inputUID = new List<string>();
            //    //[MaterialName, MaerialType, UnitWeight, YoungsModulus, PoissonRatio, CTE(熱膨脹係數), FC(抗壓強度)]
            //    List<Tuple<string, string, double, double, double, double, double>> materialProp = new List<Tuple<string, string, double, double, double, double, double>>();
            //    inputUID.Add(Guid.NewGuid().ToString("D"));
            //    materialProp.Add(Tuple.Create("Inter-Ring Shear", "Concrete", 0.0, 1E10, p.segmentPoissonRatio, 9.9E-06, p.segmentFc * 98.0));
            //    datasearch.InsertMaterialData(inputUID, p.projectUID, materialProp);
            //}
            

            
            
            //********************************************************************************************************
        }
        #endregion

        //自動抓取不同接觸深度計算，尚未完成
        #region Contacting Depth
        void ContactingDepth()
        {
            Mat mat = new Mat(); //抓取使用的material
            DataTable segment = datasearch.GetByUID("STN_Section", sectionUID);
            mat.UID.Add(segment.Rows[0]["Material"].ToString());
            double contacingDepth = double.Parse(segment.Rows[0]["SGContactingDepth"].ToString());
            DataTable sgMaterial = datasearch.GetByUID("STN_SegmentMaterial", mat.UID[0]);
            mat.name.Add(sgMaterial.Rows[0]["MaterialName"].ToString());
            mat.UT.Add(double.Parse(sgMaterial.Rows[0]["UnitWeight"].ToString()));
            mat.E = double.Parse(sgMaterial.Rows[0]["YoungModulus"].ToString());
            mat.U = double.Parse(sgMaterial.Rows[0]["PoissonRatio"].ToString());
            mat.CTE = double.Parse(sgMaterial.Rows[0]["CTE"].ToString());
            mat.Fc = double.Parse(sgMaterial.Rows[0]["Fc"].ToString());

            List<double> inputDepth = new List<double>(); //計算三種接觸深度
            inputDepth.Add(contacingDepth - 0.0225);
            inputDepth.Add(contacingDepth - 0.0125);
            inputDepth.Add(contacingDepth);

            List<double> inputUT = new List<double>(); //計算三種接觸深度下的環片單位重
            for (int i = 0; i < inputDepth.Count; i++)
            {
                double tempUT = Math.Round(mat.UT[0] * (p.segmentThickness / inputDepth[i]), 2);
                inputUT.Add(tempUT);
            }

            DataTable allMat = datasearch.GetByProject("STN_SegmentMaterial", p.projectUID);
            bool[] inputBool = new bool[3];

            for (int i = 0; i < 3; i++) //將所有sql data中的材料抓出，以單位重判斷是否已有建置該接觸深度的環片
            {
                inputBool[i] = true; //預設為尚未建置
                for (int j = 0; j < allMat.Rows.Count; j++)
                {
                    double tempUT = double.Parse(allMat.Rows[j]["UnitWeight"].ToString());
                    if (Math.Abs(inputUT[i] - tempUT) < 1E-3)
                    {
                        inputBool[i] = false;
                        mat.UID.Add(allMat.Rows[j]["UID"].ToString());
                        break;
                    }

                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (inputBool[i]) //若尚未建置，將新增的接觸深度material輸入至sql
                {
                    List<Tuple<string, string, double, double, double, double, double>> inputData =
                        new List<Tuple<string, string, double, double, double, double, double>>();
                    List<string> tempUID = new List<string>();
                    tempUID.Add(Guid.NewGuid().ToString("D"));
                    mat.UID.Add(tempUID[0]);
                    mat.name.Add($"{mat.name[0]} for t = {inputDepth[i] * 100} cm");
                    mat.UT.Add(inputUT[i]);

                    var data = Tuple.Create(mat.name[i + 1], "Concrete", mat.UT[i + 1], mat.E, mat.U, mat.CTE, mat.Fc);
                    inputData.Add(data);
                    datasearch.InsertMaterialData(tempUID, p.projectUID, inputData);
                }
            }
        }
        #endregion   //
        //------------------------------
    }
}
