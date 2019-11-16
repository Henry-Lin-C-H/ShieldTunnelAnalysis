using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using SegmentCalcu;
//using STN_SQL;

namespace SinoTunnel
{
            

    class  SAP_DataCalculation   //SAP_DataCalculation
    {   // 載重計算 及 資料置入 EXCEL 及 啟動SAP 分析
        GetWebData p;
        ExcuteSQL dataSearch = new ExcuteSQL();
        STN_VerticalStress verticalStress;

        string sectionUID = "";
        string filePath = "";

        SAP_ExcelInput input;
        int i;
        int j;
        //************************************************************************************************************************
        double SGradiusIn;
        double SGthick;
        double SGradiusInter;
        double SGwidth;
        double SGradiusOut;
        double SGE;
        double SGU12;

        double SGAangle;
        double SGBangle;
        double SGKangle;

        int SGBnum = 2;
        int SGKnum = 1;

        double adjBoltAngle;
        double BAboltAngle;
        double BKboltAngle;
        double AAboltAngle;

        SGAngle Ring1 = new SGAngle();
        SGAngle Ring2 = new SGAngle();

        #region Constructor        
        //************************************************************************************************************************
        public SAP_DataCalculation(string filePath, string sectionUID)
        {                       
            this.filePath = filePath;
            input = new SAP_ExcelInput(filePath);  //指向EXCEL樣板檔... 
            this.sectionUID = sectionUID;
            //土層載重及建物載重計算
            verticalStress = new STN_VerticalStress(sectionUID, "winform");
            this.p = new GetWebData(sectionUID);
            //************************************************************************************************************************
            try
            {
                this.SGradiusIn = p.segmentRadiusIn;
                this.SGthick = p.segmentThickness;
                this.SGradiusInter = SGradiusIn + (SGthick / 2);
                this.SGwidth = p.segmentWidth;
                this.SGradiusOut = SGradiusIn + SGthick;
                this.SGAangle = p.segmentAAngle;
                this.SGBangle = p.segmentBAngle;
                this.SGKangle = p.segmentKAngle;
                this.adjBoltAngle = p.segmentAdjacentPoreAngle;
                this.BAboltAngle = (SGAangle - adjBoltAngle) / 2;
                this.BKboltAngle = SGBangle - adjBoltAngle - BAboltAngle;
                this.AAboltAngle = BAboltAngle;

                this.Ring1.BK_Angle = BKboltAngle;
                this.Ring1.KB_Angle = BKboltAngle + SGKangle;
                this.Ring1.BA_Angle = BKboltAngle + SGKangle + SGBangle;
                this.Ring1.AB_Angle = 360 - (SGBangle - BKboltAngle);

                this.Ring2.BA_Angle = 360 - Ring1.AB_Angle;
                this.Ring2.AB_Angle = 360 - Ring1.BA_Angle;
                this.Ring2.BK_Angle = 360 - Ring1.KB_Angle;
                this.Ring2.KB_Angle = 360 - Ring1.BK_Angle;

                this.SGE = p.segmentYoungsModulus;
                this.SGU12 = p.segmentPoissonRatio;
            }
            catch
            {
            }
            //************************************************************************************************************************
        }
        #endregion

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


        #region LoadingTerm Loading
        public void LoadingtermLoading(List<string> loadingtermPattern, List<Tuple<string, string>> loadingtermCase, List<string> NLload)
        {
            List<string> strData = new List<string>();
            for (i = 0; i < loadingtermPattern.Count; i++)
            {          
                strData.Add($"{loadingtermPattern[i]},OTHER,0");               
            }
            input.PutDataToSheet("Load Pattern Definitions", strData);

            strData.Clear(); 
            for (i = 0; i < loadingtermCase.Count; i++)
            {
                strData.Add($"{loadingtermCase[i].Item1}, {loadingtermCase[i].Item2},Zero,,,Prog Det,OTHER,Prog Det,Other,None,Yes");
            }
            input.PutDataToSheet("Load Case Definitions", strData);

            strData.Clear();
            loadingtermPattern.Insert(0, "DEAD");
            for (i = 0; i < (loadingtermPattern.Count - 1); i++)
            {
                for (j = 0; j < loadingtermPattern.Count; j++)
                {
                    if (i == 0 && j == 0)       
                        strData.Add($"DEAD, Load pattern, DEAD, 1");
                    else
                        strData.Add($"{loadingtermCase[i].Item1}, Load pattern, {loadingtermPattern[j]}, 1");
                }
            }
            input.PutDataToSheet("Case - Static 1 - Load Assigns", strData);

            strData.Clear();
            for (i = 0; i < NLload.Count; i++)
            {
                strData.Add($"{NLload[i]}, Full Load");
            }
            input.PutDataToSheet("Case - Static 2 - NL Load App", strData);
        }


        #endregion

        #region Grouting Loading
        public void GroutingLoading(List<string> loadingInput, List<string> loadCombo)
        {
            List<string> Data01 = new List<string>();
            List<string> Data02 = new List<string>();
            List<string> Data03 = new List<string>();

            for (i = 0; i < loadingInput.Count; i++)
            {
     
                Data01.Add($"{loadingInput[i]},OTHER,0");
                Data02.Add($"{loadingInput[i]},LinStatic,Zero,,,Prog Det,OTHER,Prog Det,Other,None,Yes");
                if (i == 0) Data03.Add("DEAD,LoadPattern,DEAD,1");
                Data03.Add($"{loadingInput[i]},LoadPattern,{loadingInput[i]},1");
            }

            //呼叫 SAP_ExcelInput 資料置入EXCEL   ***************************
            input.PutDataToSheet("Load Pattern Definitions", Data01);
            input.PutDataToSheet("Load Case Definitions", Data02);
            input.PutDataToSheet("Case - Static 1 - Load Assigns", Data03);
            
            Data01.Clear();
            for (i = 0; i < loadingInput.Count; i++)
            {
                Data01.Add($"{loadCombo[i]},Linear Add,No,Linear Static,DEAD,1");
                Data01.Add($"{loadCombo[i]},,,Linear Static,{loadingInput[i]},1");
            }
            input.PutDataToSheet("Combination Definitions", Data01);
        }

        public void GroutingDistributedLoad(double distributedLoad, int cutPart, List<string> loadingInput)
        {
            List<string> Data01 = new List<string>();
            for (i = 1; i < cutPart; i++)
            {
                if (i < cutPart / 2 + 1)
                {
                    Data01.Add($"{i.ToString()},{ loadingInput[0]},Local,Force,{2.ToString()},RealDist,0,1,,,{distributedLoad * -1},{distributedLoad * -1}");
                }
                Data01.Add($"{i.ToString()},{loadingInput[1]},Local,Force,{2.ToString()},RealDist,0,1,,,{distributedLoad * -1},{distributedLoad * -1}");
   
            }
            input.PutDataToSheet("Frame Loads - Distributed", Data01);  
        }


        #endregion

        #region Set Material
        public void LoadingSetMaterial(int realTimes)
        {

        }


        // name, type, unitweight, E, U12, CTE, Fc
        public void SetMaterial(List<Tuple<string, string, double, double, double, double, double>> material)
        {
            List<string> matGeneral = new List<string>();
            List<string> matBasicMech = new List<string>();
            List<string> matConcrete = new List<string>();
            for (i = 0; i < material.Count; i++)
            {
                matGeneral.Add ($"{material[i].Item1},{material[i].Item2},Isotropic,No,Red");
                matBasicMech.Add ($"{material[i].Item1},{material[i].Item3},{material[i].Item3 / input.newton},{material[i].Item4},{material[i].Item4 / (2 * (1 + material[i].Item5))},{material[i].Item5},{material[i].Item6}");
                if (material[i].Item2.ToLower()  == "concrete")
                {
                    matConcrete.Add($"{material[i].Item1},{material[i].Item7},No,Mander,Takeda,{0.00221914},{0.005},{-0.1},{0},{0}");        
                }
            }
            input.PutDataToSheet("MatProp 01 - General", matGeneral);          
            input.PutDataToSheet("MatProp 02 - Basic Mech Props", matBasicMech);
            input.PutDataToSheet("MatProp 03b - Concrete Data", matConcrete);
        }
        #endregion

        #region Set FrameSection
        //Rectangular Section [name, material, shape, T3, T2, type]
        public void SetFrameSection(List<Tuple<string, string, string, double, double, string>> sectionProp)        
        {
            List<Tuple<string, string, string, double, double>> frame = new List<Tuple<string, string, string, double, double>>();
            for (i = 0; i < sectionProp.Count; i++)
            {
                var data = Tuple.Create(sectionProp[i].Item1, sectionProp[i].Item2, sectionProp[i].Item3, sectionProp[i].Item4, sectionProp[i].Item5);
                frame.Add(data);
                if (sectionProp[i].Item6.ToLower() == "concrete")
                    ConcreteSection(sectionProp[i].Item1, sectionProp[i].Item3);
            }
            input.FrameProp01_Genearl(frame);
        }

        void ConcreteSection(string name, string shape)
        {
            List<Tuple<string, string>> concreteFame = new List<Tuple<string, string>>();
            concreteFame.Add(Tuple.Create(name, shape));
            input.FrameProp02_Concrete(concreteFame);
        }

        #endregion

        #region Grouting Coordinates
        public void GroutingCoordinates(double radiusInter, int cutPart, double degree)
        {
            List<string> groutingCoor = new List<string>();
            List<string> groutingRestraint = new List<string>();
            List<string> groutingConnectFrame = new List<string>();
            double[] xCoor = new double[cutPart];
            double[] yCoor = new double[cutPart];
            double[] zCoor = new double[cutPart];

            for (int i = 0; i < cutPart; i++)
            {
                xCoor[i] = (radiusInter * Math.Cos((180 - ((180 - degree) / 2 + degree / (cutPart - 1) * i)) * Math.PI / 180));
                yCoor[i] = 0;
                zCoor[i] = (radiusInter * Math.Sin((180 - ((180 - degree) / 2 + degree / (cutPart - 1) * i)) * Math.PI / 180));

                groutingCoor.Add($"{(i + 1).ToString()},GLOBAL, Cartesian,{xCoor[i]}, {yCoor[i]},,{zCoor[i]},No");
                if (i < cutPart - 1)
                    groutingConnectFrame.Add($"{(i + 1).ToString()},{(i + 1).ToString()},{(i + 2).ToString()},No");
                if (i == 0 || i == cutPart - 1)
                    groutingRestraint.Add($"{(i + 1).ToString()},Yes,Yes,Yes,No,No,No");
            }
            input.PutDataToSheet("Joint Coordinates", groutingCoor);
            input.PutDataToSheet("Joint Restraint Assignments", groutingRestraint);
            input.PutDataToSheet("Connectivity - Frame",groutingConnectFrame);
        }
        #endregion

        #region GridLines
        public void GridLines(double radiusInter, double L)
        {
            List<string> gridLines = new List<string>();
            List<double> radius = new List<double>();
            radius.Add(radiusInter * (-1));
            radius.Add(0.0);
            radius.Add(radiusInter);
            List<string> ring = new List<string>();
            ring.Add("R1");
            ring.Add("R2");
            List<double> ringCoor = new List<double>();
            ringCoor.Add(0.0);
            ringCoor.Add(L);

            for (int i = 0; i < 3; i++)
                gridLines.Add($"GLOBAL,X,,{radius[i]},,Primary,Gary8Dark,Yes,End");
            for (int i = 0; i < 2; i++)
                gridLines.Add($"GLOBAL,Y,{ring[i]},{ringCoor[i]},,Primary,Gary8Dark,Yes,Start");
            for (int i = 0; i < 3; i++)
                gridLines.Add($"GLOBAL,Z,,{radius[i]},,Primary,Gary8Dark,Yes,Start");

            input.PutDataToSheet("Grid Lines", gridLines);  //"Grid Lines"
        }
        #endregion  

        //************************************************************************************************************************
        public int SGNum1Ring;
        public int SoilNum1Ring;
        public List<Tuple<double, double, double, double>> XYZSGRing1 = new List<Tuple<double, double, double, double>>();
        public List<Tuple<double, double, double, double>> XYZSGRing2 = new List<Tuple<double, double, double, double>>();
        public List<Tuple<double, double, double, double>> XYZSoilRing1 = new List<Tuple<double, double, double, double>>();
        public List<Tuple<double, double, double, double>> XYZSoilRing2 = new List<Tuple<double, double, double, double>>();
        public List<Tuple<double, double, double, double>> XYZSoilR1StartEnd = new List<Tuple<double, double, double, double>>();
        public List<Tuple<double, double, double, double>> XYZSoilR2StartEnd = new List<Tuple<double, double, double, double>>();

        public List<double> R1SGAngle = new List<double>();
        public List<double> R2SGAngle = new List<double>();

        public List<string> jointNameRing1 = new List<string>();
        public List<string> jointNameRing2 = new List<string>();
        public List<string> jointNameSoil1 = new List<string>();
        public List<string> jointNameSoil2 = new List<string>();

        public List<string> frameNameRing1 = new List<string>();
        public List<string> frameNameRing2 = new List<string>();
        public List<string> linkNameSoil1 = new List<string>();
        public List<string> linkNameSoil2 = new List<string>();
        public List<string> frameNameR1R2 = new List<string>();

        public List<double> SGhalfAngle = new List<double>();
        public List<double> SGhalfDistinct = new List<double>();
        public List<double> SoilHalfAngle = new List<double>();
        public List<double> SoilHalfDistinct = new List<double>();

        public void VerticalStress()
        {
            verticalStress.VerticalStress("TUNNEL", out string ls, out string ss, out string sus, out double longTermE1,
                out double shortTermE1, out double PvTop, out double longTermPh1, out double longTermPh2, out double shortTermPh1,
                out double shortTermPh2, out double U12);
        }

        #region Joint Coordinates, Joint Restraint Assignments
        public void SGJointsAndRestraint(int SGAnum, out List<string> diameterJointName)
        {
            List<Tuple<bool, double, double>> crossLayer = new List<Tuple<bool, double, double>>();
            double twoRingSpacing = 2.0;

            diameterJointName = new List<string>();

            SG sg = new SG();
            sg.angle.Add(0);
            sg.X.Add(0);
            sg.Y.Add(0);
            sg.Z.Add(SGradiusInter * Math.Cos(0 * Math.PI / 180));

            List<Tuple<double, double, double, double>> tempXYZ = new List<Tuple<double, double, double, double>>();
            List<Tuple<double, double, double, double>> tempSoilXYZ = new List<Tuple<double, double, double, double>>();

            for (int SGorSoil = 1; SGorSoil < 3; SGorSoil++)
            {
                tempXYZ.Clear();
                tempXYZ.Add(Tuple.Create(0.0, 0.0, 0.0, SGradiusInter * SGorSoil * Math.Cos(0 * Math.PI / 180))); //The first joint is at the top of the circle (also the first joint of bolt)

                //Joint for Ring1 and Ring2
                for (int k = 0; k < 2; k++)
                {
                    double angle = 0;

                    //The joint of Ring
                    for (int i = 0; i < SGAnum + SGBnum + SGKnum; i++)
                    {
                        if (k == 0) //Ring1 sequence K -> B -> A -> A -> A -> B
                        {
                            if (i == 0) angle += BKboltAngle;
                            else if (i == 1) angle += SGKangle;
                            else if (i == 2) angle += SGBangle;
                            else angle += SGAangle;

                            R1SGAngle.Add(angle);
                        }
                        else //Ring2 sequence B -> A -> A -> A -> B -> K
                        {
                            if (i == 0) angle += (adjBoltAngle + BAboltAngle);
                            else if (i < SGAnum + 1) angle += SGAangle;
                            else if (i == (SGAnum + 1)) angle += SGBangle;
                            else angle += SGKangle;

                            R2SGAngle.Add(angle);
                        }

                        double tempAngle = angle - 4;
                        for (int j = 0; j < 3; j++)
                        {
                            XYZInput(ref tempXYZ, tempAngle, SGradiusInter * SGorSoil, 0.0);
                            //var data = Tuple.Create(tempAngle, radiusInter * Math.Sin((tempAngle) * Math.PI / 180), 0.0, radiusInter * Math.Cos((tempAngle) * Math.PI / 180));
                            //Coor.Add(data);
                            tempAngle += 4;
                        }

                        if (k == 0 && i > 0) //Calculating the joint of bolt, once
                        {
                            if (i == 1) //The second joint of bolt (first one at line#44). Adding BKboltAngle (10.5degree)
                            {
                                tempAngle = angle + BKboltAngle;
                                XYZInput(ref tempXYZ, tempAngle, SGradiusInter * SGorSoil, 0.0);
                                tempAngle += adjBoltAngle; //The third joint of bolt. Adding adjacent bolt angle
                                XYZInput(ref tempXYZ, tempAngle, SGradiusInter * SGorSoil, 0.0);
                            }
                            else if (i < (1 + 1 + SGAnum)) //The fourth joint of bolt...
                            {
                                tempAngle = angle + AAboltAngle;
                                XYZInput(ref tempXYZ, tempAngle, SGradiusInter * SGorSoil, 0.0);
                                tempAngle += adjBoltAngle;
                                XYZInput(ref tempXYZ, tempAngle, SGradiusInter * SGorSoil, 0.0);
                            }
                            else
                            {
                                tempAngle = angle + BAboltAngle;
                                XYZInput(ref tempXYZ, tempAngle, SGradiusInter * SGorSoil, 0.0);
                            }
                        }
                    }
                }
                tempXYZ = tempXYZ.OrderBy(t => t.Item1).ToList();

                if (SGorSoil == 1)
                {
                    for(int j = 0; j < tempXYZ.Count; j++)
                    {
                        XYZSGRing1.Add(Tuple.Create(tempXYZ[j].Item1, tempXYZ[j].Item2, tempXYZ[j].Item3, tempXYZ[j].Item4));
                    }
                    
                }
                else
                {
                    for (int j = 0; j < tempXYZ.Count; j++)
                    {
                        tempSoilXYZ.Add(Tuple.Create(tempXYZ[j].Item1, tempXYZ[j].Item2, tempXYZ[j].Item3, tempXYZ[j].Item4));
                    }
                }
                    
            }            
            SGNum1Ring = XYZSGRing1.Count;
            
            for (int i = 0; i < SGNum1Ring; i++) //Joint Name of Ring1 and Ring2 ex: R1_Jt_SG001, R1_Jt_SG002,...
            {
                var data = Tuple.Create(XYZSGRing1[i].Item1, XYZSGRing1[i].Item2, XYZSGRing1[i].Item3 + twoRingSpacing, XYZSGRing1[i].Item4);
                XYZSGRing2.Add(data);

                jointNameRing1.Add($"R1_Jt_SG{(i + 1).ToString("D3")}");
                jointNameRing2.Add($"R2_Jt_SG{(i + 1).ToString("D3")}");

                if (XYZSGRing1[i].Item2 < 1E-5) diameterJointName.Add(jointNameRing1[i]);
            }

            bool endBool = true;
            int[] startEnd = new int[2];
            int count = 0;
            for (int i = 0; i < SGNum1Ring; i++) //Joint name of Soil1 and Soil2 ex: R1_Jt_Soil001, R1_Jt_Soil002,...
            {                                              
                if (tempSoilXYZ[i].Item1 >= 45 && tempSoilXYZ[i].Item1 <= 315)
                {
                    endBool = true;
                    count++;
                    if (count == 1) startEnd[0] = i - 1;                    

                    var data01 = Tuple.Create(tempSoilXYZ[i].Item1, tempSoilXYZ[i].Item2, tempSoilXYZ[i].Item3, tempSoilXYZ[i].Item4);
                    XYZSoilRing1.Add(data01);

                    var data02 = Tuple.Create(tempSoilXYZ[i].Item1, tempSoilXYZ[i].Item2, tempSoilXYZ[i].Item3 + twoRingSpacing, tempSoilXYZ[i].Item4);
                    XYZSoilRing2.Add(data02);

                    jointNameSoil1.Add($"R1_Jt_Soil{(i + 1).ToString("D3")}");
                    jointNameSoil2.Add($"R2_Jt_Soil{(i + 1).ToString("D3")}");
                }
                else { endBool = false; }

                if (!endBool && count > 0) { startEnd[1] = i; break; }
            }
            SoilNum1Ring = XYZSoilRing1.Count;

            //抓取土壤第一點與最後一點前後的點位
            foreach(int i in startEnd)
            {
                var data00 = Tuple.Create(tempSoilXYZ[i].Item1, tempSoilXYZ[i].Item2, 
                    tempSoilXYZ[i].Item3, tempSoilXYZ[i].Item4);
                XYZSoilR1StartEnd.Add(data00);

                var data01 = Tuple.Create(tempSoilXYZ[i].Item1, tempSoilXYZ[i].Item2,
                    tempSoilXYZ[i].Item3 + twoRingSpacing, tempSoilXYZ[i].Item4);
                XYZSoilR2StartEnd.Add(data01);
            }                        

            List<string> inputDataXYZ = new List<string>();
            JointToString(ref inputDataXYZ, XYZSGRing1, jointNameRing1);
            JointToString(ref inputDataXYZ, XYZSGRing2, jointNameRing2);
            JointToString(ref inputDataXYZ, XYZSoilRing1, jointNameSoil1);
            JointToString(ref inputDataXYZ, XYZSoilRing2, jointNameSoil2);

            List<string> inputDataXYZRestraint = new List<string>();
            RestraintToString(ref inputDataXYZRestraint, jointNameRing1, "SEGMENT");
            RestraintToString(ref inputDataXYZRestraint, jointNameRing2, "SEGMENT");
            RestraintToString(ref inputDataXYZRestraint, jointNameSoil1, "SOIL");
            RestraintToString(ref inputDataXYZRestraint, jointNameSoil2, "SOIL");

            input.PutDataToSheet("Joint Coordinates", inputDataXYZ);
            input.PutDataToSheet("Joint Restraint Assignments", inputDataXYZRestraint);

            void XYZInput(ref List<Tuple<double, double, double, double>> Coordinate, double tempAngle, double RadiusInter, double RadiusWidth)
            {
                var data = Tuple.Create(tempAngle, RadiusInter * Math.Sin((tempAngle) * Math.PI / 180), RadiusWidth, RadiusInter * Math.Cos((tempAngle) * Math.PI / 180));
                Coordinate.Add(data);
            }
            void JointToString(ref List<string> inputValue, List<Tuple<double, double, double, double>> list, List<string> str)
            {
                for(int i = 0; i < str.Count; i++)
                    inputValue.Add($"{str[i]},GLOBAL,Cartesian,{list[i].Item2},{list[i].Item3},,{list[i].Item4},Yes");                
            }
            void RestraintToString(ref List<string> inputValue, List<string> str, string condition)
            {
                switch (condition.ToUpper().ToString())
                {
                    case "SEGMENT":
                        for (int i = 0; i < str.Count; i++)
                            inputValue.Add($"{str[i]},No,Yes,No,Yes,No,Yes");
                        break;
                    case "SOIL":
                        for (int i = 0; i < str.Count; i++)
                            inputValue.Add($"{str[i]},Yes,Yes,Yes,Yes,No,Yes");
                        break;
                }
            }            
        }
        #endregion

        #region ConnectivityFrame_FrameOutputStationAssigns
        public void SGConnectivityFrame_StationAssigns(out List<string> contactingFrameName)
        {
            contactingFrameName = new List<string>();
            List<string> ConnectivityFrame = new List<string>();
            for(int i = 0; i < SGNum1Ring; i++) //設定第一環、第二環與兩環間的frame name
            {
                if (XYZSGRing1[i].Item1 < Ring1.BK_Angle)
                    frameNameRing1.Add($"R1SG{(i + 1).ToString("D3")}_B");
                else if (XYZSGRing1[i].Item1 < Ring1.KB_Angle)
                    frameNameRing1.Add($"R1SG{(i + 1).ToString("D3")}_K");
                else if (XYZSGRing1[i].Item1 < Ring1.BA_Angle)
                    frameNameRing1.Add($"R1SG{(i + 1).ToString("D3")}_B");
                else if (XYZSGRing1[i].Item1 < Ring1.AB_Angle)
                    frameNameRing1.Add($"R1SG{(i + 1).ToString("D3")}_A");
                else
                    frameNameRing1.Add($"R1SG{(i + 1).ToString("D3")}_B");

                if (XYZSGRing2[i].Item1 < Ring2.BA_Angle)
                    frameNameRing2.Add($"R2SG{(i + 1).ToString("D3")}_B");
                else if (XYZSGRing2[i].Item1 < Ring2.AB_Angle)
                    frameNameRing2.Add($"R2SG{(i + 1).ToString("D3")}_A");
                else if (XYZSGRing2[i].Item1 < Ring2.BK_Angle)
                    frameNameRing2.Add($"R2SG{(i + 1).ToString("D3")}_B");
                else if (XYZSGRing2[i].Item1 < Ring2.KB_Angle)
                    frameNameRing2.Add($"R2SG{(i + 1).ToString("D3")}_K");
                else
                    frameNameRing2.Add($"R2SG{(i + 1).ToString("D3")}_B");

                frameNameR1R2.Add($"R1R2_SG{(i + 1).ToString("D3")}");
            }
            FrameToString(ref ConnectivityFrame, frameNameRing1, jointNameRing1);
            FrameToString(ref ConnectivityFrame, frameNameRing2, jointNameRing2);

            for(int i = 0; i < SGNum1Ring; i++) //第一環的接觸深度frame，選擇後frame(頭joint)
            {
                ConnectivityFrame.Add($"{frameNameR1R2[i]},{jointNameRing1[i]},{jointNameRing2[i]},No");

                for(int j = 0; j < R1SGAngle.Count; j++)
                {
                    if (XYZSGRing1[i].Item1 == R1SGAngle[j])
                    {
                        contactingFrameName.Add($"{frameNameRing1[i]}");
                        break;
                    }
                }                                                
            }

            for(int i = 0; i < SGNum1Ring; i++) //第二環的接觸深度frame，選擇前frame(尾joint)
            {
                for (int j = 0; j < R2SGAngle.Count; j++)
                {
                    if (XYZSGRing2[i].Item1 == R2SGAngle[j])
                    {
                        contactingFrameName.Add($"{frameNameRing2[i - 1]}");
                        break;
                    }
                }
            }

            //contactingFrameName = contactingFrameName.OrderBy(p => p).ToList(); 沒辦法使用，因為有A K B 環的名稱影響
            
            
            List<string> FrameOutputStationAssigns = new List<string>();
            for(int i = 0; i < SGNum1Ring; i++)
            {
                FrameOutputStationAssigns.Add($"{frameNameRing1[i]},MinNumSta,{2},,Yes,Yes");
                FrameOutputStationAssigns.Add($"{frameNameRing2[i]},MinNumSta,{2},,Yes,Yes");
                FrameOutputStationAssigns.Add($"{frameNameR1R2[i]},MinNumSta,{2},,Yes,Yes");
            }                                                     

            input.PutDataToSheet("Connectivity - Frame", ConnectivityFrame);
            input.PutDataToSheet("Frame Output Station Assigns", FrameOutputStationAssigns);

            //FrameReleases 前環的軸力放掉、後環的扭矩放掉
            List<Tuple<string, string, string, string, string, string>> formerSide = new List<Tuple<string, string, string, string, string, string>>();
            List<Tuple<string, string, string, string, string, string>> latterSide = new List<Tuple<string, string, string, string, string, string>>();
            for (int i = 0; i < frameNameR1R2.Count; i++)
            {
                formerSide.Add(Tuple.Create("Yes", "No", "No", "No", "No", "No"));
                latterSide.Add(Tuple.Create("No", "No", "No", "Yes", "No", "No"));
            }
            input.FrameRelease(frameNameR1R2, formerSide, latterSide);  //"Frame Releases 1 - General"

            void FrameToString(ref List<string> frame, List<string> frameName, List<string> jointName)
            {
                for(int i = 0; i < jointName.Count - 1; i++)
                    frame.Add($"{frameName[i]},{jointName[i]},{jointName[i + 1]},No");
                frame.Add($"{frameName[jointName.Count - 1]},{jointName[jointName.Count - 1]},{jointName[0]},No");
            }
        }
        
        #endregion

        #region FrameSectionAssigns
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SGAnum"></param>
        /// <param name="sectionAssign">[0]:正常環片, [1]:計算1~3次，採用單一接觸深度環片, [1~N]:各接縫採用不同接觸深度</param>
        public void FrameSectionAssigns(int SGAnum, List<string> sectionAssign, int realTimes, out List<double> D)
        {                       
            List<bool> R1FrameBool = new List<bool>(); //判斷是否為環片接縫處的frame
            List<bool> R2FrameBool = new List<bool>(); //判斷是否為環片接縫處的frame

            List<double> R1DepthAngle = new List<double>(); //各環接觸深度的角度，搭配XYZRing1的角度，每個節點會有兩個frame採用不同接觸深度
            List<double> R2DepthAngle = new List<double>();
            for(int i = 0; i < R1SGAngle.Count; i++)
            {
                R1DepthAngle.Add(R1SGAngle[i] - 4);
                R1DepthAngle.Add(R1SGAngle[i]);

                R2DepthAngle.Add(R2SGAngle[i] - 4);
                R2DepthAngle.Add(R2SGAngle[i]);
            }

            List<string> SGName = new List<string>(); // [0]:正常環片, [1]:計算1~3次，採用單一接觸深度環片, [1~N]:各接縫採用不同接觸深度
            foreach (string str in sectionAssign)
                SGName.Add(str);

            //switch (realTimes)
            //{
            //    case 4:
            //        {
            //            DataTable td;
            //            for (int i = 0; i < sectionAssign.Count; i++)
            //            {
            //                td = dataSearch.GetByUID("STN_Segment", sectionAssign[i]);
            //                SGName.Add(td.Rows[0]["Name"].ToString());
            //            }
            //        }
            //        break;
            //    default:
            //        {
            //            foreach (string str in sectionAssign)
            //                SGName.Add(str);
            //        }
            //        break;
            //}

            //DataTable td;
            //for (int i = 0; i < SGUID.Count; i++)
            //{
            //    td = dataSearch.GetByUID("STN_Segment", SGUID[i]);
            //    SGName.Add(td.Rows[0]["Name"].ToString());
            //}


            bool tempBool1;
            bool tempBool2;
            for (int i = 0; i < SGNum1Ring; i++)
            {
                tempBool1 = false;
                tempBool2 = false;
                for(int j = 0; j < R1DepthAngle.Count; j++)
                {
                    if (XYZSGRing1[i].Item1 == R1DepthAngle[j]) tempBool1 = true;
                    if (XYZSGRing2[i].Item1 == R2DepthAngle[j]) tempBool2 = true;
                }
                R1FrameBool.Add(tempBool1);
                R2FrameBool.Add(tempBool2);
            }

            List<string> frameAssigns = new List<string>();
            List<string> R2FrameAssign = new List<string>();
            int R2sg = 1;
            int sg = 1;
            bool tempBool = true;
            bool R2tempBool = true;
            for(int i = 0; i < SGNum1Ring; i++)
            {
                if (R1FrameBool[i]) //判斷是否為採用接觸深度之環片
                {                    
                    switch (SGName.Count)
                    {
                        case 2: //1~3次計算
                            {
                                frameAssigns.Add($"{frameNameRing1[i]},Rectangular,N.A.,{SGName[1]},{SGName[1]},Default");
                            }break;
                        default: //最終計算，一次把segment放入兩個相鄰的frame，所以bool作用為跳過一次
                            {
                                if (tempBool)
                                {
                                    frameAssigns.Add($"{frameNameRing1[i]},Rectangular,N.A.,{SGName[sg]},{SGName[sg]},Default");
                                    frameAssigns.Add($"{frameNameRing1[i+1]},Rectangular,N.A.,{SGName[sg]},{SGName[sg]},Default");
                                    sg++;
                                }
                                tempBool = !tempBool;
                            }break;
                    }
                }
                else
                {
                    frameAssigns.Add($"{frameNameRing1[i]},Rectangular,N.A.,{SGName[0]},{SGName[0]},Default");
                }

                if (R2FrameBool[i])
                {
                    switch (SGName.Count)
                    {
                        case 2:
                            R2FrameAssign.Add($"{frameNameRing2[i]},Rectangular,N.A.,{SGName[1]},{SGName[1]},Default");
                            break;
                        default:
                            {
                                if (R2tempBool)
                                {
                                    R2FrameAssign.Add($"{frameNameRing2[i]},Rectangular,N.A.,{SGName[R2sg]},{SGName[R2sg]},Default");
                                    R2FrameAssign.Add($"{frameNameRing2[i + 1]},Rectangular,N.A.,{SGName[R2sg]},{SGName[R2sg]},Default");
                                    R2sg++;
                                }
                                R2tempBool = !R2tempBool;
                            }
                            break;
                    }
                }
                else
                    R2FrameAssign.Add($"{frameNameRing2[i]},Rectangular,N.A.,{SGName[0]},{SGName[0]},Default");
            }
            input.PutDataToSheet("Frame Section Assignments", frameAssigns);
            input.PutDataToSheet("Frame Section Assignments", R2FrameAssign);

            //計算兩環間的斷面轉換資訊(直徑)            
            HalfAngleCal("SEGMENT");

                        
            
            double G = (SGE / (2 * (1 + SGU12)));

            double Ksb = G * Math.PI / 4 * (Math.Pow(SGradiusOut * 2, 2)- Math.Pow((SGradiusOut - SGthick) * 2, 2)) / (SGwidth / 2);
            List<double> KsbDis = new List<double>(); 
            foreach(double s in SGhalfDistinct)
                KsbDis.Add(Ksb * s / 360);
                        
            double Etemp = 1E10; //計算直徑(兩環的)   
            double L = 2;
            D = new List<double>();
            double inertia;
            foreach(double s in KsbDis)
            {
                inertia = s * L * L * L / (12 * Etemp);
                D.Add(Math.Round(Math.Pow(64 * inertia / Math.PI, 0.25), 4));
            }

            //抓到interRing的UID
            //DataTable material = dataSearch.GetByKeyword("STN_SegmentMaterial", "Project", p.projectUID);
            //string interRingUID = "";
            //for (int i = 0; i < material.Rows.Count; i++)
            //{
            //    if (material.Rows[i]["MaterialName"].ToString() == "Inter-Ring Shear")
            //        interRingUID = material.Rows[i]["UID"].ToString();
            //}

            //將未輸入兩環桿件資訊的data輸入至SQL資料庫中
            //List<string> inputUID = new List<string>();
            //[Name, Material, Shape, Width, Height]
            //List<Tuple<string, string, string, double, double>> frameData = new List<Tuple<string, string, string, double, double>>();


            List<string> InterRingName = new List<string>(); //針對兩環間桿件命名
            foreach (double s in SGhalfDistinct)
                InterRingName.Add($"InterRingAngle = {s}");

            //DataTable segmentFrame = dataSearch.GetBySection("STN_Segment", sectionUID, "");
            //for (int i = 0; i < SGhalfDistinct.Count; i++)
            //{
            //    bool test = true;
            //    for (int j = 0; j < segmentFrame.Rows.Count; j++)
            //    {
            //        if (InterRingName[i] == segmentFrame.Rows[j]["Name"].ToString())
            //            test = false;
            //    }
            //    if (test)
            //    {
            //        inputUID.Add(Guid.NewGuid().ToString("D"));
            //        var data = Tuple.Create(InterRingName[i], interRingUID, "Circle", 1.0, D[i]);
            //        frameData.Add(data);
            //    }
            //}
            //dataSearch.InsertFrameData(inputUID, sectionUID, frameData);

            List<string> frameSectionBetweenTwoRing = new List<string>();
            for(int i = 0; i < SGNum1Ring; i++)
            {
                for(int j = 0; j < SGhalfDistinct.Count; j++)
                {
                    if (SGhalfAngle[i] == SGhalfDistinct[j])
                        frameSectionBetweenTwoRing.Add($"{frameNameR1R2[i]},Circle,N.A.,{InterRingName[j]},{InterRingName[j]},Default");
                }
            }            
            input.PutDataToSheet("Frame Section Assignments", frameSectionBetweenTwoRing);

        }
        #endregion

        #region SGConnectivityLink
        public void SGConnectivityLink()
        {
            List<string> LinkSTR = new List<string>();
            for (int i = 0; i < SoilNum1Ring; i++)
            {
                string[] s = jointNameSoil1[i].Split('_');
                linkNameSoil1.Add($"R1_Link_{s[2]}");
                linkNameSoil2.Add($"R2_Link_{s[2]}");
            }
            LinkToString(ref LinkSTR, linkNameSoil1, jointNameRing1, jointNameSoil1);
            LinkToString(ref LinkSTR, linkNameSoil2, jointNameRing2, jointNameSoil2);

            input.PutDataToSheet("Connectivity - Link", LinkSTR);

            void LinkToString(ref List<string> link, List<string> linkName, List<string> jointNameRing, List<string> jointNameSoil)
            {
                int spacing = (jointNameRing.Count - jointNameSoil.Count - 1) / 2 + 1;
                for (int i = 0; i < linkName.Count; i++)
                {
                    link.Add($"{linkName[i]},{jointNameRing[i + spacing]},{jointNameSoil[i]}");
                }
            }
        }
        #endregion

        #region Link Prop Gap Assign
        public void LinkPropAndGapAndAssign(string condition)
        {
            HalfAngleCal("SOIL");           

            List<string> linkNameForExcel = new List<string>();
            for(int i = 0; i < SoilHalfDistinct.Count; i++)
                linkNameForExcel.Add($"Soil Spring Angle = {SoilHalfDistinct[i]}");

            //計算土壤彈簧的K
            double Em = 0;
            if (condition == "LongTerm" || condition == "VariationofDiameter" || condition == "EQofDiameter") Em = verticalStress.longTermSoilE;
            else if (condition == "ShortTerm") Em = verticalStress.shortTermSoilE;
            double soilNu = verticalStress.Nu12;
            double soilKs = Em * (1 - soilNu) / (SGradiusInter * (1 + soilNu) * (1 - 2 * soilNu));

            List<double> soilDisKs = new List<double>();
            foreach(double s in SoilHalfDistinct)
                soilDisKs.Add(soilKs * (SGwidth / 2) * s * Math.PI / 180 * SGradiusInter);


            //List<string> inputUID = new List<string>();
            //將土壤彈簧桿件資訊的data輸入至SQL資料庫中
            //inputUID.Clear();
            //List<Tuple<string, string, double>> soilLinkData = new List<Tuple<string, string, double>>();

            //try
            //{
            //    dataSearch.DeleteDataBySectionUID("STN_SegmentSoilLink", sectionUID);
            //}
            //catch
            //{
            //}

            //for (int i = 0; i < SGhalfDistinct.Count; i++)
            //{
            //    inputUID.Add(Guid.NewGuid().ToString("D"));
            //    var data = Tuple.Create(linkNameForExcel[i], "Gap", soilDisKs[i]);
            //    soilLinkData.Add(data);
            //}
            //dataSearch.InsertSoilLinkData(inputUID, sectionUID, soilLinkData);


            List<string> linkProp = new List<string>();
            List<string> linkGap = new List<string>();            

            for(int i = 0; i < linkNameForExcel.Count; i++)
            {
                linkProp.Add($"{linkNameForExcel[i]},Gap,{0.0},{0.0},{0.0001},{0.0001},{0.0001},{1},{1},{0},{0},{0},{0},Yellow");
                linkGap.Add($"{linkNameForExcel[i]},U1,No,Yes,{soilDisKs[i]},0,{soilDisKs[i]},0");
            }
            input.PutDataToSheet("Link Props 01 - General", linkProp);
            input.PutDataToSheet("Link Props 05 - Gap", linkGap);

            List<string> linkAssign = new List<string>();
            for(int k = 0; k < 2; k++)
            {
                for (int i = 0; i < SoilNum1Ring; i++)
                {
                    for (int j = 0; j < SoilHalfDistinct.Count; j++)
                    {
                        if (SoilHalfAngle[i] == SoilHalfDistinct[j])
                        {
                            if (k == 0) linkAssign.Add($"{linkNameSoil1[i]},Gap,TwoJoint,{linkNameForExcel[j]},None"); //第一環
                            else if (k== 1) linkAssign.Add($"{linkNameSoil2[i]},Gap,TwoJoint,{linkNameForExcel[j]},None"); //第二環
                        }
                    }
                }
            }
            input.PutDataToSheet("Link Property Assignments", linkAssign);
            
        }

        void HalfAngleCal(string condition)
        {


            switch (condition.ToUpper().ToString())
            {
                case "SEGMENT":
                    {
                        SGhalfAngle.Clear();
                        SGhalfDistinct.Clear();
                        for (int i = 0; i < XYZSGRing1.Count; i++) //計算環片點位角度差的一半
                        {
                            if (i == 0)
                                SGhalfAngle.Add((XYZSGRing1[1].Item1 + 360 - XYZSGRing1[XYZSGRing1.Count - 1].Item1) / 2);
                            else if (i == XYZSGRing1.Count - 1)
                                SGhalfAngle.Add((XYZSGRing1[0].Item1 + 360 - XYZSGRing1[i - 1].Item1) / 2);
                            else
                                SGhalfAngle.Add((XYZSGRing1[i + 1].Item1 - XYZSGRing1[i - 1].Item1) / 2);
                        }
                        SGhalfDistinct = SGhalfAngle.Distinct().ToList();
                    }
                    break;
                case "SOIL":
                    {
                        SoilHalfAngle.Clear();
                        SoilHalfDistinct.Clear();
                        for (int i = 0; i < XYZSoilRing1.Count; i++) //計算土壤點位角度差的一半
                        {
                            if (i == 0)
                                SoilHalfAngle.Add((XYZSoilRing1[1].Item1 - XYZSoilRing1[0].Item1) / 2 + 
                                    (XYZSoilRing1[0].Item1 - XYZSoilR1StartEnd[0].Item1) / 2); 
                            //第一點直接抓與下一點的角度差
                            else if (i == XYZSoilRing1.Count - 1) //最後一點抓與前一點的角度差
                                SoilHalfAngle.Add((XYZSoilRing1[XYZSoilRing1.Count - 1].Item1 - 
                                    XYZSoilRing1[XYZSoilRing1.Count - 2].Item1) / 2 
                                    + (XYZSoilR1StartEnd[1].Item1 - XYZSoilRing1[XYZSoilRing1.Count - 1].Item1) / 2);
                            else
                                SoilHalfAngle.Add((XYZSoilRing1[i + 1].Item1 - XYZSoilRing1[i - 1].Item1) / 2);
                        }
                        SoilHalfDistinct = SoilHalfAngle.Distinct().ToList();
                    }
                    break;
            }                        
        }
        #endregion

        #region Uncoupled
        public void Uncoupled()
        {
            List<int> uncoupledNum = new List<int>();
            uncoupledNum.Add(XYZSGRing1.Count / 2 + 5);
            uncoupledNum.Add(XYZSGRing1.Count / 2 - 3);

            List<string> uncoupledStr = new List<string>();
            uncoupledStr.Add($"{jointNameRing1[uncoupledNum[0]]},Local,100.0,0,0,0,0,0");
            uncoupledStr.Add($"{jointNameRing2[uncoupledNum[1]]},Local,100.0,0,0,0,0,0");

            input.PutDataToSheet("Jt Spring Assigns 1 - Uncoupled", uncoupledStr);
        }
        #endregion

        #region FrameLoad Distributed
        public void FrameLoad_Distributed(string condition)
        {
            double Pv = verticalStress.PvTop;
            double Ph1 = 0;
            double Ph2 = 0;
            double deltaPh = 0;
            double Eavg = 0;

            switch (condition)
            {
                case "LongTerm":
                case "VariationofDiameter":
                case "EQofDiameter":
                    {
                        Ph1 = verticalStress.LongTermPh1;
                        Ph2 = verticalStress.LongTermPh2;
                        Eavg = verticalStress.longTermSoilE;
                    }break;
                case "ShortTerm":
                    {
                        Ph1 = verticalStress.ShortTermPh1;
                        Ph2 = verticalStress.ShortTermPh2;
                        Eavg = verticalStress.shortTermSoilE;
                    }break;
            }
            deltaPh = Ph2 - Ph1;

            string inputCondition = "";
            switch (condition)
            {
                case "LongTerm": inputCondition = "Long Term"; break;
                case "ShortTerm": inputCondition = "Short Term"; break;
                case "VariationofDiameter": inputCondition = "0.33%Diameter"; break;
                case "EQofDiameter": inputCondition = "EQofDiameter"; break;
            }

            List<double> SGHeight = new List<double>();
            for(int i = 0; i < SGNum1Ring; i++)
                SGHeight.Add(XYZSGRing1[i].Item4 + SGradiusInter);            

            List<string> verticalInputR1 = new List<string>();
            List<string> lateralInputR1 = new List<string>();

            List<string> verticalInputR2 = new List<string>();
            List<string> lateralInputR2 = new List<string>();
            double verticalForce = Pv * SGwidth / 2;
            for (int i = 0; i < SGNum1Ring; i++)
            {
                double tempPv = verticalForce;
                //根據桿件座標與向量計算垂直力為正或負，垂直力：一二象限(力量向下)向上為正，因此力量為負，三四象限(力量向上)反之力量為正

                bool test = false;
                if (Math.Abs(XYZSGRing1[i].Item4) < 1E-5) //端點位置
                {
                    if (XYZSGRing1[i + 1].Item4 > 0) test = true; //查看下一點為正或負確認桿件所屬象限     
                    else test = false;
                }
                else if (XYZSGRing1[i].Item4 > 0) test = true; //Z > 0:一二象限
                else
                    test = false; //Z < 0:三四象限

                if(test) tempPv = tempPv * (-1) - (increasedZaxisP / 2);
                else tempPv += increasedZaxisP / 2;

                verticalInputR1.Add($"{frameNameRing1[i]},{inputCondition} Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{tempPv},{tempPv}");
                verticalInputR2.Add($"{frameNameRing2[i]},{inputCondition} Vertical,GLOBAL,Force,Z Proj, RelDist,0,1,,,{tempPv},{tempPv}");

                double tempPh1 = 0;
                double tempPh2 = 0;
                double PL1 = SGradiusInter * 2;

                try //計算桿件 top 與 bot 的水平力
                {
                    tempPh1 = (Ph1 + (deltaPh * (PL1 - SGHeight[i]) / PL1)) * SGwidth / 2;
                    tempPh2 = (Ph1 + (deltaPh * (PL1 - SGHeight[i + 1]) / PL1)) * SGwidth / 2;
                }
                catch //最後一個桿件
                {
                    tempPh1 = (Ph1 + (deltaPh * (PL1 - SGHeight[i]) / PL1)) * SGwidth / 2;
                    tempPh2 = (Ph1 + (deltaPh * (PL1 - SGHeight[0]) / PL1)) * SGwidth / 2;
                }

                //根據桿件座標與向量計算水平力為正或負，水平力：一四象限(力量向左)向右為正，因此力量為負，二三象限(力量向右)反之力量為正

                test = false;
                if (Math.Abs(XYZSGRing1[i].Item2) < 1E-5) //端點位置
                {
                    if (XYZSGRing1[i + 1].Item2 > 0) test = true; //查看下一點為正或負確認桿件所屬象限，若>0為一四、若<0為二三
                    else test = false;
                }
                else if (XYZSGRing1[i].Item2 > 0) test = true; //X > 0:一四象限                
                else test = false;//X < 0:二三象限                

                if (test)
                {
                    tempPh1 = tempPh1 * (-1) + decreasedXaxisP;
                    tempPh2 = tempPh2 * (-1) + decreasedXaxisP;
                }
                else
                {
                    tempPh1 -= decreasedXaxisP;
                    tempPh2 -= decreasedXaxisP;
                }
                lateralInputR1.Add($"{frameNameRing1[i]},{inputCondition} Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{tempPh1},{tempPh2}");
                lateralInputR2.Add($"{frameNameRing2[i]},{inputCondition} Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{tempPh1},{tempPh2}");
            }

            input.PutDataToSheet("Frame Loads - Distributed", verticalInputR1);
            input.PutDataToSheet("Frame Loads - Distributed", verticalInputR2);
            input.PutDataToSheet("Frame Loads - Distributed", lateralInputR1);
            input.PutDataToSheet("Frame Loads - Distributed", lateralInputR2);
        }
        #endregion  

        //************************************************************************************************************************

        #region Loading Coordinates
        //public List<string> LoadingCoordinates(string projectUID, double radiusOut, double thickness, double width, List<double> segmentAngle, int segmentANum, double L, double segmentWidth, string condition, List<string> segmentUID, out List<string> segmentJunctionName, out List<string> diameterPointName)
        //{
        //    List<double> Xcoor = new List<double>();
        //    List<double> Zcoor = new List<double>();
        //    List<Tuple<bool, double, double>> crossLayer = new List<Tuple<bool, double, double>>();
        //    crossLayer.Add(verticalStress.VerticalStress("TUNNEL", out string longString, out string shortString, out string surchargeString, out double longtermE1, out double shortermE1, out double Pv, out double longtermPh1, out double longtermPh2, out double shortermPh1, out double shortermPh2, out double U12)[0]);

        //    double radiusInter = radiusOut - thickness / 2;

        //    double SGAangle = segmentAngle[0];
        //    double SGBangle = segmentAngle[1];
        //    double SGKangle = segmentAngle[2];
        //    double poreAngle = segmentAngle[3];

        //    int SGAnum = segmentANum;
        //    int SGBnum = 2;
        //    int SGKnum = 1;

        //    double AporeAngle = (SGAangle - poreAngle) / 2;
        //    double BAporeAngle = AporeAngle;
        //    double BKporeAngle = SGBangle - poreAngle - BAporeAngle;

        //    string tempName = "";
        //    bool test;
        //    bool test01;

        //    List<string> ring1SoilPointName = new List<string>();
        //    List<string> ring2SoilPointName = new List<string>();
        //    List<string> ring1SegmentPointName = new List<string>();
        //    List<string> ring2SegmentPointName = new List<string>();
        //    List<string> ringSegmentPointName = new List<string>();

        //    List<double> rod = new List<double>();
        //    List<double> rodCal = new List<double>();
        //    List<string> EQdtheta = new List<string>();
        //    List<double> adjFrame = new List<double>();
        //    List<bool> frameAssignBool = new List<bool>();

        //    List<string> soildtheta = new List<string>();
        //    List<double> soilDisKs = new List<double>();

        //    List<int> uncoupledNum = new List<int>();

        //    List<double> segmentJunctionDegree = new List<double>();
        //    List<bool> segmentJunctionBool = new List<bool>();

        //    segmentJunctionName = new List<string>();
        //    diameterPointName = new List<string>();

        //    double[] radiusWidth = new double[2];
        //    radiusWidth[0] = 0.0;
        //    radiusWidth[1] = 2.0;

        //    ///<sumarry>
        //    ///環片點位
        //    /// </sumarry>
        //    //第一環與第二環
        //    for(int n = 0; n < 2; n++)
        //    {
        //        //土壤彈簧與環片
        //        for (int m = 1; m < 3; m++)
        //        {
        //            //[degre, X, Y, Z]
        //            List<Tuple<double, double, double, double>> Coor = new List<Tuple<double, double, double, double>>();
        //            List<string> CoorExcel = new List<string>();
        //            List<string> restraintExcel = new List<string>();

        //            Coor.Add(Tuple.Create(0.0, 0.0, radiusWidth[n], radiusInter * Math.Cos(0 * Math.PI / 180) * m));

        //            //環片螺栓孔處
        //            for (int k = 0; k < 2; k++)
        //            {
        //                double angle = 0;

        //                //環片與環片接縫處
        //                for (int i = 0; i < SGAnum + SGBnum + SGKnum; i++)
        //                {
        //                    if (k == 0) //第一環環片角度計算
        //                    {
        //                        if (i == 0) angle += BKporeAngle;
        //                        else if (i == 1) angle += SGKangle;
        //                        else if (i == 2) angle += SGBangle;
        //                        else angle += SGAangle;
        //                    }
        //                    else //第二環環片角度計算
        //                    {
        //                        if (i == 0) angle += (poreAngle + BAporeAngle);
        //                        else if (i < SGAnum + 1) angle += SGAangle;
        //                        else if (i == (SGAnum + 1)) angle += SGBangle;
        //                        else angle += SGKangle;
        //                    }

        //                    double tempAngle = angle - 4;
        //                    for (int j = 0; j < 3; j++)
        //                    {
        //                        CoorInput(ref Coor, tempAngle, radiusInter * m, radiusWidth[n]);
        //                        //var data = Tuple.Create(tempAngle, radiusInter * Math.Sin((tempAngle) * Math.PI / 180), 0.0, radiusInter * Math.Cos((tempAngle) * Math.PI / 180));
        //                        //Coor.Add(data);
        //                        if (n == 0 && m == 1)
        //                        {
        //                            //挑出不同接觸深度的環片
        //                            if (j == 0 || j == 1) adjFrame.Add(tempAngle);
        //                            //抓到環片接縫的角度
        //                            if (j == 1) segmentJunctionDegree.Add(tempAngle);
        //                        }                                                                    
        //                        tempAngle += 4;
        //                    }

        //                    if (k == 0 && i > 0) //由於兩環之螺栓孔位置應相同，只計算第一環之螺栓孔位置
        //                    {
        //                        if (i == 1)
        //                        {
        //                            tempAngle = angle + BKporeAngle;
        //                            CoorInput(ref Coor, tempAngle, radiusInter * m, radiusWidth[n]);
        //                            tempAngle += poreAngle;
        //                            CoorInput(ref Coor, tempAngle, radiusInter * m, radiusWidth[n]);
        //                        }
        //                        else if (i < (1 + 1 + SGAnum))
        //                        {
        //                            tempAngle = angle + AporeAngle;
        //                            CoorInput(ref Coor, tempAngle, radiusInter * m, radiusWidth[n]);
        //                            tempAngle += poreAngle;
        //                            CoorInput(ref Coor, tempAngle, radiusInter * m, radiusWidth[n]);
        //                        }
        //                        else
        //                        {
        //                            tempAngle = angle + BAporeAngle;
        //                            CoorInput(ref Coor, tempAngle, radiusInter * m, radiusWidth[n]);
        //                        }
        //                    }
        //                }
        //            }

        //            Coor = Coor.OrderBy(t => t.Item1).ToList();

        //            //只考慮環片部分，所以設m=1，n為前後環，所以會算兩次
        //            if (m == 1)
        //            {
        //                for(int i = 0; i < Coor.Count; i++)
        //                {  //計算環片點位的X,Z值，提供distributed load 使用
        //                    Xcoor.Add(Coor[i].Item2);
        //                    Zcoor.Add(Coor[i].Item4 + radiusInter);
        //                }
        //                //計算uncoupled用
        //                if (n == 0) uncoupledNum.Add(Coor.Count / 2 + 5);
        //                else if (n == 1) uncoupledNum.Add(Coor.Count + (Coor.Count / 2 - 3));
        //            }

        //            if(n==0 && m == 1) //只需做一次計算
        //            {   //計算環片點位角度差的一半                        
        //                for (int i = 0; i < Coor.Count; i++)
        //                {
        //                    if (i == 0)
        //                    {
        //                        rod.Add((Coor[1].Item1 + 360 - Coor[Coor.Count - 1].Item1) / 2);
        //                    }
        //                    else if (i == Coor.Count - 1)
        //                    {
        //                        rod.Add((Coor[0].Item1 + 360 - Coor[i - 1].Item1) / 2);
        //                    }
        //                    else
        //                    {
        //                        rod.Add((Coor[i + 1].Item1 - Coor[i - 1].Item1) / 2);
        //                    }                            
        //                }

        //                //挑出使用不同接觸深度的環片，用bool表示(test)
        //                //挑出環片接觸點位的角度對應的frame編號，用bool表示(test01)
        //                for(int i = 0; i < 2; i++)
        //                {
        //                    for(int j = 0; j < Coor.Count; j++)
        //                    {
        //                        test = false;
        //                        test01 = false;
        //                        if(i == 0)
        //                        {
        //                            for (int k = 0; k < adjFrame.Count / 2; k++)
        //                                if (Coor[j].Item1 == adjFrame[k]) test = true;
        //                            for (int k = 0; k < segmentJunctionDegree.Count / 2; k++)
        //                                if (Coor[j].Item1 == segmentJunctionDegree[k]) test01 = true;
        //                        }                                    
        //                        else if (i == 1)
        //                        {
        //                            for (int k = adjFrame.Count / 2; k < adjFrame.Count; k++)
        //                                if (Coor[j].Item1 == adjFrame[k]) test = true;
        //                            for (int k = segmentJunctionDegree.Count / 2; k < segmentJunctionDegree.Count; k++)
        //                                if (Coor[j].Item1 == segmentJunctionDegree[k]) test01 = true;
        //                        }                                    
        //                        frameAssignBool.Add(test);
        //                        segmentJunctionBool.Add(test01);
        //                    }
        //                }

        //                //計算兩環間桿件資訊                        
        //                rodCal.Add(rod[0]);
        //                test = false;
        //                foreach (double temp in rod)
        //                {
        //                    test = true;
        //                    foreach (double s in rodCal)
        //                    {
        //                        if (s == temp)
        //                        {
        //                            test = false;
        //                        }
        //                    }
        //                    if (test == true)
        //                    {
        //                        rodCal.Add(temp);
        //                    }
        //                }

        //                //針對此桿件命名                        
        //                for (int i = 0; i < rodCal.Count; i++)
        //                {
        //                    EQdtheta.Add($"EQ Round dTheta = {rodCal[i]}");
        //                    soildtheta.Add($"Soil Spring dTheta = {rodCal[i]}");
        //                }

        //                //計算Ksb(兩環的)
        //                double E = 31528558;
        //                double nu = 0.2;
        //                double G = (E / (2 * (1 + nu)));
        //                double Ksb = G * Math.PI / 4 * (Math.Pow(radiusOut * 2, 2) - Math.Pow((radiusOut - thickness) * 2, 2)) / (width / 2);
        //                List<double> KsbDis = new List<double>();

        //                //計算土壤彈簧的K
        //                double Em = 0;
        //                if (condition == "LongTerm" || condition == "VariationofDiameter" || condition == "EQofDiameter") Em = longtermE1;
        //                else if (condition == "ShortTerm") Em = shortermE1;
        //                double soilNu = U12;
        //                double soilKs = Em * (1 - soilNu) / (radiusInter * (1 + soilNu) * (1 - 2 * soilNu));
                        
        //                foreach (double s in rodCal)
        //                {
        //                    KsbDis.Add(Ksb * s / 360);
        //                    soilDisKs.Add(soilKs * (segmentWidth / 2) * s * Math.PI / 180 * radiusInter);
        //                }

        //                //計算直徑(兩環的)            
        //                double Etemp = 1E10;
        //                List<double> D = new List<double>();
        //                double inertia;
        //                foreach (double s in KsbDis)
        //                {
        //                    inertia = s * Math.Pow(L, 3) / (12 * Etemp);
        //                    D.Add(Math.Round(Math.Pow(64 * inertia / Math.PI, 0.25), 4));
        //                }                        

        //                //抓到interRing的UID
        //                DataTable material = dataSearch.GetByKeyword("STN_SegmentMaterial", "Project", projectUID);
        //                string interRingUID = "";
        //                for (int i = 0; i < material.Rows.Count; i++)
        //                {
        //                    if (material.Rows[i]["MaterialName"].ToString() == "Inter-Ring Shear")
        //                        interRingUID = material.Rows[i]["UID"].ToString();
        //                }

        //                //將未輸入兩環桿件資訊的data輸入至SQL資料庫中
        //                List<string> inputUID = new List<string>();
        //                //[Name, Material, Shape, Width, Height]
        //                List<Tuple<string, string, string, double, double>> frameData = new List<Tuple<string, string, string, double, double>>();

        //                DataTable segmentFrame = dataSearch.GetBySection("STN_Segment", sectionUID, "");
        //                for (int i = 0; i < rodCal.Count; i++)
        //                {
        //                    test = true;
        //                    for (int j = 0; j < segmentFrame.Rows.Count; j++)
        //                    {
        //                        if (EQdtheta[i] == segmentFrame.Rows[j]["Name"].ToString())
        //                        {
        //                            test = false;
        //                        }
        //                    }
        //                    if (test == true)
        //                    {
        //                        inputUID.Add(Guid.NewGuid().ToString("D"));
        //                        var data = Tuple.Create(EQdtheta[i], interRingUID, "Circle", 1.0, D[i]);
        //                        frameData.Add(data);
        //                    }
        //                }
        //                dataSearch.InsertFrameData(inputUID, sectionUID, frameData);

        //                //將未輸入土壤彈簧桿件資訊的data輸入至SQL資料庫中
        //                inputUID.Clear();
        //                List<Tuple<string, string, double>> soilLinkData = new List<Tuple<string, string, double>>();

        //                DataTable segmentSoilLink = dataSearch.GetBySection("STN_SegmentSoilLink", sectionUID, "");
        //                for(int i = 0; i < rodCal.Count; i++)
        //                {
        //                    test = true;
        //                    for (int j = 0; j < segmentSoilLink.Rows.Count; j++)
        //                    {
        //                        if (soildtheta[i] == segmentSoilLink.Rows[j]["Name"].ToString())
        //                            test = false;
        //                    }
        //                    if(test == true)
        //                    {
        //                        inputUID.Add(Guid.NewGuid().ToString("D"));
        //                        var data = Tuple.Create(soildtheta[i], "Gap", soilDisKs[i]);
        //                        soilLinkData.Add(data);
        //                    }
        //                }
        //                dataSearch.InsertSoilLinkData(inputUID, sectionUID, soilLinkData);
        //            }

        //            //環片與土壤彈簧點位
        //            for (int i = 0; i < Coor.Count; i++)
        //            {
        //                string tempPointName;
        //                if ((i + 1) < 10) tempPointName = $"00{i + 1}";
        //                else if ((i + 1) < 100) tempPointName = $"0{i + 1}";
        //                else tempPointName = $"{i + 1}";
        //                if (n == 0 && m == 1)
        //                {                            
        //                    tempName = $"R1_S{tempPointName}";
        //                    ring1SegmentPointName.Add($"R1_S{tempPointName}");
        //                }                            
        //                else if (n == 0 && m == 2)
        //                {
        //                    tempName = $"R1_G{tempPointName}";
        //                    ring1SoilPointName.Add($"R1_G{tempPointName}");
        //                }                            
        //                else if (n == 1 && m == 1)
        //                {
        //                    tempName = $"R2_S{tempPointName}";
        //                    ring2SegmentPointName.Add($"R2_S{tempPointName}");                            
        //                }                            
        //                else if (n == 1 && m == 2)
        //                {
        //                    tempName = $"R2_G{tempPointName}";
        //                    ring2SoilPointName.Add($"R2_G{tempPointName}");
        //                }                            

        //                CoorExcel.Add($"{tempName},GLOBAL,Cartesian,{Coor[i].Item2},{Coor[i].Item3},,{Coor[i].Item4},Yes"); 

        //                if (m == 1)
        //                {
        //                    restraintExcel.Add($"{tempName},No,Yes,No,Yes,No,Yes");
        //                }
        //                else if (m == 2)
        //                {
        //                    restraintExcel.Add($"{tempName},Yes,Yes,Yes,Yes,No,Yes");
        //                }      
                        
        //                //提供計算環片直徑變化用，找到最高點與最低點，並且只找一環即可
        //                if(n == 0 && m == 1)
        //                {
        //                    if(Math.Abs(Coor[i].Item2) < 1E-5)
        //                    {
        //                        diameterPointName.Add(tempName);
        //                    }
        //                }
        //            }
        //            input.PutDataToSheet("Joint Coordinates",CoorExcel);  //"Joint Coordinates"   
        //            input.PutDataToSheet("Joint Restraint Assignments", restraintExcel);  //"Joint Restraint Assignments"
        //        }
        //    }

        //    ///<summary>
        //    ///環片桿件與土壤桿件
        //    /// </summary>
        //    for(int i = 0; i < 2; i++)
        //    {
        //        for(int j = 0; j < ring1SegmentPointName.Count; j++)
        //        {
        //            if (i == 0) ringSegmentPointName.Add(ring1SegmentPointName[j]);
        //            else ringSegmentPointName.Add(ring2SegmentPointName[j]);
        //        }
        //    }

        //    List<string> connectFrame = new List<string>();
        //    List<string> connectFrameName = new List<string>();
        //    List<string> connectLink = new List<string>();

        //    //環片(Segment)的桿件名稱，分為前後環與所有的
        //    List<string> ring1SGFrameName = new List<string>();
        //    List<string> ring2SGFrameName = new List<string>();
        //    List<string> ringFrameName = new List<string>();
        //    //土壤(Soil)的桿件名稱，分為前後環與所有的
        //    List<string> ring1SoilFrameName = new List<string>();
        //    List<string> ring2SoilFrameName = new List<string>();
        //    List<string> ringSoilFrameName = new List<string>();
        //    //兩環片間的桿件名稱
        //    List<string> ring1ring2SGFrameName = new List<string>();
        //    for (int m = 1; m < 3; m++)
        //    {                
        //        for (int n = 0; n < ring1SegmentPointName.Count; n++)
        //        {
        //            string tempFrameName;
        //            if ((n + 1) < 10) tempFrameName = $"00{n + 1}";
        //            else if ((n + 1) < 100) tempFrameName = $"0{n + 1}";
        //            else tempFrameName = $"{n + 1}";
        //            connectFrameName.Add($"R{m}_S{tempFrameName}");
        //            try
        //            {                        
        //                if (m == 1)
        //                {                    
        //                    connectFrame.Add($"R{m}_S{tempFrameName},{ring1SegmentPointName[n]},{ring1SegmentPointName[n + 1]},No");                                                         
        //                    ring1SGFrameName.Add($"R{m}_S{tempFrameName}");
        //                }                            
        //                else if (m == 2)
        //                {
        //                    connectFrame.Add($"R{m}_S{tempFrameName},{ring2SegmentPointName[n]},{ring2SegmentPointName[n + 1]},No");
        //                    ring2SGFrameName.Add($"R{m}_S{tempFrameName}");                            
        //                }                            
        //            }
        //            catch
        //            {
        //                if (m == 1)
        //                {
        //                    connectFrame.Add($"R{m}_S{tempFrameName},{ring1SegmentPointName[n]},{ring1SegmentPointName[0]},No");
        //                    ring1SGFrameName.Add($"R{m}_S{tempFrameName}");                            
        //                }
        //                else if (m == 2)
        //                {
        //                    connectFrame.Add($"R{m}_S{tempFrameName},{ring2SegmentPointName[n]},{ring2SegmentPointName[0]},No");
        //                    ring2SGFrameName.Add($"R{m}_S{tempFrameName}");
        //                }
        //            }
        //            if (m == 1)
        //            {
        //                connectLink.Add($"R{m}_SG{tempFrameName},{ring1SegmentPointName[n]},{ring1SoilPointName[n]}");
        //                ring1SoilFrameName.Add($"R{m}_SG{tempFrameName}");
        //            }                        
        //            else if (m == 2)
        //            {
        //                connectLink.Add($"R{m}_SG{tempFrameName},{ring2SegmentPointName[n]},{ring2SoilPointName[n]}");
        //                ring2SoilFrameName.Add($"R{m}_SG{tempFrameName}");
        //            }                      
        //        }
        //    }
            
        //    for(int n = 0; n < ring1SegmentPointName.Count; n++)
        //    {
        //        string tempPointName;
        //        if ((n + 1) < 10) tempPointName = $"00{n + 1}";
        //        else if ((n + 1) < 100) tempPointName = $"0{n + 1}";
        //        else tempPointName = $"{n + 1}";
        //        ring1ring2SGFrameName.Add($"R1R2_S{tempPointName}");
        //        connectFrame.Add($"{ring1ring2SGFrameName[n]},R1_S{tempPointName},R2_S{tempPointName},No");
        //        connectFrameName.Add($"{ring1ring2SGFrameName[n]}");
        //    }           
        //    List<string> frameName = new List<string>();
        //    for (int i = 0; i < connectFrame.Count; i++)
        //      frameName.Add($"{connectFrameName[i]},MinNumSta,{2},,Yes,Yes");  
        //    input.PutDataToSheet("Connectivity - Frame", connectFrame);  //"Connectivity - Frame"
        //    input.PutDataToSheet("Frame Output Station Assigns", frameName);  //Frame Output Station Assigns
        //    input.PutDataToSheet("Connectivity - Link", connectLink);  //"Connectivity - Link"

        //    ///<summary>
        //    ///桿件配置(環片與兩環間之配置)
        //    /// </summary>
        //    for (int i = 0; i < 2; i++)
        //    {
        //        for(int j = 0; j < ring1SGFrameName.Count; j++)
        //        {
        //            if (i == 0) ringFrameName.Add(ring1SGFrameName[j]);
        //            else ringFrameName.Add(ring2SGFrameName[j]);
        //        }

        //        for(int j = 0; j < ring1SoilFrameName.Count; j++)
        //        {
        //            if (i == 0) ringSoilFrameName.Add(ring1SoilFrameName[j]);
        //            else ringSoilFrameName.Add(ring2SoilFrameName[j]);
        //        }
        //    }                        

        //    List<string> SGUID = new List<string>();
        //    for(int i = 0; i < segmentUID.Count; i++)
        //    {
        //        SGUID.Add(segmentUID[i]);
        //    }
        //    //正常環:0
        //    //接觸深度不同之環:1
        //    List<string> SGName = new List<string>();
            
        //    for (int i = 0; i < SGUID.Count; i++)
        //    {
        //        DataTable tempdatatable;
        //        tempdatatable = dataSearch.GetByUID("STN_Segment", SGUID[i]);
        //        SGName.Add(tempdatatable.Rows[0]["Name"].ToString());
        //    }

        //    int sg = 1;
        //    bool tempbool = true;
        //    List<string> frameSection = new List<string>();
        //    List<string> frameBetweenRingSection = new List<string>();
        //    for (int i = 0; i < ringFrameName.Count; i++)
        //    {
        //        if (frameAssignBool[i] == true)
        //        {
        //            if (SGName.Count == 2)
        //            {
        //                frameSection.Add($"{ringFrameName[i]},Rectangular,N.A.,{SGName[1]},{SGName[1]},Default");
        //            }
        //            else
        //            {
        //                if (tempbool) //一次把segment放入兩個相鄰的frame，所以bool作用為跳過一次
        //                {
        //                    frameSection.Add($"{ringFrameName[i]},Rectangular,N.A.,{SGName[sg]},{SGName[sg]},Default");
        //                    frameSection.Add($"{ringFrameName[i+1]},Rectangular,N.A.,{SGName[sg]},{SGName[sg]},Default");
        //                    sg++;
        //                }
        //                tempbool = !tempbool;
        //            }
        //        }
        //        else
        //        {
        //            frameSection.Add($"{ringFrameName[i]},Rectangular,N.A.,{SGName[0]},{SGName[0]},Default");
        //        }

        //        if (segmentJunctionBool[i] == true)
        //        {
        //            segmentJunctionName.Add(ringFrameName[i]);
        //        }
        //    }

        //    //兩環間之framesection配置            
        //    for (int i = 0; i < ring1ring2SGFrameName.Count; i++)
        //    {
        //        for (int j = 0; j < rodCal.Count; j++)
        //        {
        //            if (rod[i] == rodCal[j])
        //            {
        //                frameBetweenRingSection.Add($"{ring1ring2SGFrameName[i]},Circle,N.A.,{EQdtheta[j]},{EQdtheta[j]},Default");
        //            }
        //        }
        //    }
        //    input.PutDataToSheet("Frame Section Assignments", frameSection);
        //    input.PutDataToSheet("Frame Section Assignments", frameBetweenRingSection);


        //    ///<summary>
        //    ///FrameRelease
        //    /// </summary>
        //    //FrameReleases 前環的軸力放掉、後環的扭矩放掉
        //    List<Tuple<string, string, string, string, string, string>> formerSide = new List<Tuple<string, string, string, string, string, string>>();
        //    List<Tuple<string, string, string, string, string, string>> latterSide = new List<Tuple<string, string, string, string, string, string>>();
        //    for (int i = 0; i < ring1ring2SGFrameName.Count; i++)
        //    {
        //        formerSide.Add(Tuple.Create("Yes", "No", "No", "No", "No", "No"));
        //        latterSide.Add(Tuple.Create("No", "No", "No", "Yes", "No", "No"));
        //    }
        //    input.FrameRelease(ring1ring2SGFrameName, formerSide, latterSide);  //"Frame Releases 1 - General"

        //    ///<summary>
        //    ///土壤桿件配置
        //    /// </summary>
        //    //土壤彈簧輸入
        //    List<string> linkProp = new List<string>();
        //    List<string> linkGap = new List<string>();
        //    List<string> linkAssign = new List<string>();

        //    for (int i = 0; i < soildtheta.Count; i++)
        //    {    //soildtheta：土壤彈簧名稱、soilDisKs：土壤彈簧K值
        //        linkProp.Add($"{soildtheta[i]},Gap,{0.0},{0.0},{0.0001},{0.0001},{0.0001},{1},{1},{0},{0},{0},{0},Yellow");
        //        linkGap.Add($"{soildtheta[i]},U1,No,Yes,{soilDisKs[i]},0,{soilDisKs[i]},0");
        //    }

        //    for (int k = 0; k < 2; k++)
        //    {
        //        for (int i = 0; i < ring1SoilFrameName.Count; i++)
        //        {
        //            for (int j = 0; j < rodCal.Count; j++)
        //            {
        //                if (rod[i] == rodCal[j])
        //                {
        //                    if(k == 0)
        //                    {
        //                        linkAssign.Add($"{ring1SoilFrameName[i]},Gap,TwoJoint,{soildtheta[j]},None");
        //                    }
        //                    else if(k == 1)
        //                    {
        //                        linkAssign.Add($"{ring2SoilFrameName[i]},Gap,TwoJoint,{soildtheta[j]},None");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    input.PutDataToSheet("Link Props 01 - General", linkProp);  //$"{linkProp[i].Item1},{linkProp[i].Item2},{linkProp[i].Item3},{linkProp[i].Item4},{0.0001},{0.0001},{0.0001},{1},{1},{0},{0},{0},{0},Yellow");
        //    input.PutDataToSheet("Link Props 05 - Gap",linkGap);       //$"{linkProp[i].Item1},{linkProp[i].Item2},{linkProp[i].Item3},{linkProp[i].Item4},{linkProp[i].Item5},{0},{linkProp[i].Item5},{0}");
        //    input.PutDataToSheet("Link Property Assignments",linkAssign);   // $"{linkProp[i].Item1},{linkProp[i].Item2},{linkProp[i].Item3},{linkProp[i].Item4},None");

        //    ///<summary>
        //    ///計算E值，分為有無跨越土層狀況
        //    /// </summary>
        //    /*
        //    double PL1 = 0;
        //    double PL2 = 0;
        //    if(crossLayer[0].Item1 == false)
        //    {
        //        PL1 = crossLayer[0].Item2 - thickness / 2;
        //        PL2 = 0;
        //    }
        //    else if (crossLayer[0].Item1 == true)
        //    {
        //        PL1 = crossLayer[0].Item2 - thickness / 2;
        //        PL2 = crossLayer[0].Item3 - thickness / 2;
        //    }
        //    */            

        //    double deltaPh = 0;
        //    double Eavg = 0;
        //    double Ph1 = 0;
        //    double Ph2 = 0;
        //    if(condition == "LongTerm" || condition == "VariationofDiameter" || condition == "EQofDiameter")
        //    {
        //        Ph1 = longtermPh1;
        //        Ph2 = longtermPh2;
        //        deltaPh = longtermPh2 - longtermPh1;
        //        Eavg = longtermE1;
        //    }
        //    else if (condition == "ShortTerm")
        //    {
        //        Ph1 = shortermPh1;
        //        Ph2 = shortermPh2;
        //        deltaPh = shortermPh2 - shortermPh1;
        //        Eavg = shortermE1;
        //    }
            
        //    ///<summary>
        //    ///桿件載重加載
        //    /// </summary>
        //  //  List<Tuple<string, string, string, string, string, double, double>> frameLoadDistributed = new List<Tuple<string, string, string, string, string, double, double>>();
        //    List<string> Data01 = new List<string>();
        //    for(int i = 0; i < ringFrameName.Count; i++)
        //    {
        //        double verticalForce = Pv * segmentWidth / 2;
        //        //根據桿件座標與向量計算垂直力為正或負，垂直力：一二象限(力量向下)向上為正，因此力量為負，三四象限(力量向上)反之力量為正
        //        double tempZ = 0.0;
        //        try
        //        {
        //            tempZ = Math.Abs(Zcoor[i + 1]) - radiusInter;
        //        }
        //        catch
        //        {
        //            tempZ = Math.Abs(Zcoor[0]) - radiusInter;
        //        }


        //        if (Math.Abs(Zcoor[i]) - radiusInter < 1E-5)
        //        {
        //            if((Math.Abs(Zcoor[i+1]) - radiusInter) > 0)
        //            {
        //                verticalForce = verticalForce * (-1) - (increasedZaxisP / 2);
        //            }
        //            else
        //            {
        //                verticalForce += (increasedZaxisP / 2);
        //            }
        //        }
        //        else if(tempZ < 1E-5)
        //        {
        //            if((Math.Abs(Zcoor[i]) - radiusInter) > 0)
        //            {
        //                verticalForce = verticalForce * (-1) - (increasedZaxisP / 2);
        //            }
        //            else
        //            {
        //                verticalForce += (increasedZaxisP / 2);
        //            }
        //        }
        //        else
        //        {
        //            if((Math.Abs(Zcoor[i]) - radiusInter) > 0)
        //            {
        //                verticalForce = verticalForce * (-1) - (increasedZaxisP / 2);
        //            }
        //            else
        //            {
        //                verticalForce += (increasedZaxisP / 2);
        //            }
        //        }
        //        /*
        //        if ((Zcoor[i] - radiusInter) >= 0) verticalForce *= (-1);
        //        if (Math.Abs(Zcoor[i] - radiusInter) < 1E-5) verticalForce *= (-1);
        //        */
        //        string tempCondition = "";
        //        if (condition == "LongTerm") tempCondition = "Long Term";
        //        else if (condition == "ShortTerm") tempCondition = "Short Term";
        //        else if (condition == "VariationofDiameter") tempCondition = "0.33%Diameter";
        //        else if (condition == "EQofDiameter") tempCondition = "EQofDiameter";

        //        Data01.Add($"{ringFrameName[i]},{tempCondition} Vertical,GLOBAL,Force,Z Proj,RelDist,0,1,,,{verticalForce},{verticalForce}");                       

        //        //if (crossLayer[0].Item1 == false)
        //        if(true)
        //        {
        //            double tempForce01 = 0;
        //            double tempForce02 = 0;
        //            double PL1 = radiusInter * 2;

                    
        //            try
        //            {
        //                tempForce01 = (Ph1 + (deltaPh * (PL1 - Zcoor[i]) / PL1)) * segmentWidth / 2;
        //                tempForce02 = (Ph1 + (deltaPh * (PL1 - Zcoor[i + 1]) / PL1)) * segmentWidth / 2;
        //            }
        //            catch
        //            {
        //                tempForce01 = (Ph1 + (deltaPh * (PL1 - Zcoor[i]) / PL1)) * segmentWidth / 2;
        //                tempForce02 = (Ph1 + (deltaPh * (PL1 - Zcoor[0]) / PL1)) * segmentWidth / 2;
        //            }
                    
        //            //根據桿件座標與向量計算水平力為正或負，水平力：一四象限(力量向左)向右為正，因此力量為負，二三象限(力量向右)反之力量為正
        //            double tempX = 0.0;
        //            try
        //            {
        //                tempX = Math.Abs(Xcoor[i + 1]);
        //            }
        //            catch
        //            {
        //                tempX = Math.Abs(Xcoor[0]);
        //            }

        //            if (Math.Abs(Xcoor[i]) < 1E-5)
        //            {
        //                if (Xcoor[i + 1] > 0)
        //                {
        //                    tempForce01 = tempForce01 * (-1) + decreasedXaxisP;
        //                    tempForce02 = tempForce02 * (-1) + decreasedXaxisP;
        //                }
        //                else
        //                {
        //                    tempForce01 -= decreasedXaxisP;
        //                    tempForce02 -= decreasedXaxisP;
        //                }
        //            }
        //            else if (tempX < 1E-5)
        //            {
        //                if(Xcoor[i] > 0)
        //                {
        //                    tempForce01 = tempForce01 * (-1) + decreasedXaxisP;
        //                    tempForce02 = tempForce02 * (-1) + decreasedXaxisP;
        //                }
        //                else
        //                {
        //                    tempForce01 -= decreasedXaxisP;
        //                    tempForce02 -= decreasedXaxisP;
        //                }
        //            }
        //            else
        //            {
        //                if (Xcoor[i] > 0) tempForce01 = tempForce01 * (-1) + decreasedXaxisP;
        //                else tempForce01 -= decreasedXaxisP;
        //                if (Xcoor[i + 1] > 0) tempForce02 = tempForce02 * (-1) + decreasedXaxisP;
        //                else tempForce02 -= decreasedXaxisP;
        //            }

        //            Data01.Add($"{ringFrameName[i]},{tempCondition} Lateral,GLOBAL,Force,X Proj,RelDist,0,1,,,{tempForce01},{tempForce02}"); 
        //        }
        //    }
        //    input.PutDataToSheet("Frame Loads - Distributed", Data01);
        //    List<string> uncoupledExcel = new List<string>();
        //    foreach (int s in uncoupledNum)
        //    {
        //        uncoupledExcel.Add($"{ringFrameName[s]},Local,100.0,0,0,0,0,0");
        //    }
        //    input.PutDataToSheet("Jt Spring Assigns 1 - Uncoupled", uncoupledExcel);


        //    return ringFrameName;
        //}

        //void CoorInput(ref List<Tuple<double,double,double,double>> Coor, double tempAngle, double radiusInter, double radiusWidth)
        //{
        //    var data = Tuple.Create(tempAngle, radiusInter * Math.Sin((tempAngle) * Math.PI / 180), radiusWidth, radiusInter * Math.Cos((tempAngle) * Math.PI / 180));
        //    Coor.Add(data);            
        //}
        #endregion


        #region Grouting Frame Section
        public void GroutingFrameSection(int cutPart, string sectionName, string shape)
        {
            List<string> frameSection = new List<string>();
            List<string> frameOutput = new List<string>();
            for (i = 1; i < cutPart; i++)
            {
                frameSection.Add($"{i.ToString()},{shape},N.A.,{sectionName},{sectionName},Default");
                frameOutput.Add($"{i},MinNumSta,{2},,Yes,Yes");
            }
            input.PutDataToSheet("Frame Section Assignments", frameSection);
            input.PutDataToSheet("Frame Output Station Assigns", frameOutput);
        }
        #endregion
        
                

        public void FileSave(string path)
        {
            input.FileSaving(path);
        }
    }
}
