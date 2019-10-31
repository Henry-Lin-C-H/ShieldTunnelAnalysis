using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTunnel
{
    class SAP_SpringBeamK
    {
        GetWebData p;
        STN_VerticalStress verticalStress;

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

        string sectionUID;

        public SAP_SpringBeamK(string sectionUID)
        {
            this.sectionUID = sectionUID;
            verticalStress = new STN_VerticalStress(sectionUID, "webform");
            this.p = new GetWebData(sectionUID);

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
        }

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
                
        void VerticalStress()
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
                    for (int j = 0; j < tempXYZ.Count; j++)
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
            foreach (int i in startEnd)
            {
                var data00 = Tuple.Create(tempSoilXYZ[i].Item1, tempSoilXYZ[i].Item2,
                    tempSoilXYZ[i].Item3, tempSoilXYZ[i].Item4);
                XYZSoilR1StartEnd.Add(data00);

                var data01 = Tuple.Create(tempSoilXYZ[i].Item1, tempSoilXYZ[i].Item2,
                    tempSoilXYZ[i].Item3 + twoRingSpacing, tempSoilXYZ[i].Item4);
                XYZSoilR2StartEnd.Add(data01);
            }

            //List<string> inputDataXYZ = new List<string>();
            //JointToString(ref inputDataXYZ, XYZSGRing1, jointNameRing1);
            //JointToString(ref inputDataXYZ, XYZSGRing2, jointNameRing2);
            //JointToString(ref inputDataXYZ, XYZSoilRing1, jointNameSoil1);
            //JointToString(ref inputDataXYZ, XYZSoilRing2, jointNameSoil2);

            //List<string> inputDataXYZRestraint = new List<string>();
            //RestraintToString(ref inputDataXYZRestraint, jointNameRing1, "SEGMENT");
            //RestraintToString(ref inputDataXYZRestraint, jointNameRing2, "SEGMENT");
            //RestraintToString(ref inputDataXYZRestraint, jointNameSoil1, "SOIL");
            //RestraintToString(ref inputDataXYZRestraint, jointNameSoil2, "SOIL");

            //input.PutDataToSheet("Joint Coordinates", inputDataXYZ);
            //input.PutDataToSheet("Joint Restraint Assignments", inputDataXYZRestraint);

            void XYZInput(ref List<Tuple<double, double, double, double>> Coordinate, double tempAngle, double RadiusInter, double RadiusWidth)
            {
                var data = Tuple.Create(tempAngle, RadiusInter * Math.Sin((tempAngle) * Math.PI / 180), RadiusWidth, RadiusInter * Math.Cos((tempAngle) * Math.PI / 180));
                Coordinate.Add(data);
            }
            //void JointToString(ref List<string> inputValue, List<Tuple<double, double, double, double>> list, List<string> str)
            //{
            //    for (int i = 0; i < str.Count; i++)
            //        inputValue.Add($"{str[i]},GLOBAL,Cartesian,{list[i].Item2},{list[i].Item3},,{list[i].Item4},Yes");
            //}
            //void RestraintToString(ref List<string> inputValue, List<string> str, string condition)
            //{
            //    switch (condition.ToUpper().ToString())
            //    {
            //        case "SEGMENT":
            //            for (int i = 0; i < str.Count; i++)
            //                inputValue.Add($"{str[i]},No,Yes,No,Yes,No,Yes");
            //            break;
            //        case "SOIL":
            //            for (int i = 0; i < str.Count; i++)
            //                inputValue.Add($"{str[i]},Yes,Yes,Yes,Yes,No,Yes");
            //            break;
            //    }
            //}
        }
        #endregion

        #region ConnectivityFrame_FrameOutputStationAssigns
        public void SGConnectivityFrame_StationAssigns(out List<string> contactingFrameName)
        {
            contactingFrameName = new List<string>();
            List<string> ConnectivityFrame = new List<string>();
            for (int i = 0; i < SGNum1Ring; i++) //設定第一環、第二環與兩環間的frame name
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

            for (int i = 0; i < SGNum1Ring; i++) //第一環的接觸深度frame，選擇後frame(頭joint)
            {
                ConnectivityFrame.Add($"{frameNameR1R2[i]},{jointNameRing1[i]},{jointNameRing2[i]},No");

                for (int j = 0; j < R1SGAngle.Count; j++)
                {
                    if (XYZSGRing1[i].Item1 == R1SGAngle[j])
                    {
                        contactingFrameName.Add($"{frameNameRing1[i]}");
                        break;
                    }
                }
            }

            for (int i = 0; i < SGNum1Ring; i++) //第二環的接觸深度frame，選擇前frame(尾joint)
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


            //List<string> FrameOutputStationAssigns = new List<string>();
            //for (int i = 0; i < SGNum1Ring; i++)
            //{
            //    FrameOutputStationAssigns.Add($"{frameNameRing1[i]},MinNumSta,{2},,Yes,Yes");
            //    FrameOutputStationAssigns.Add($"{frameNameRing2[i]},MinNumSta,{2},,Yes,Yes");
            //    FrameOutputStationAssigns.Add($"{frameNameR1R2[i]},MinNumSta,{2},,Yes,Yes");
            //}

            //input.PutDataToSheet("Connectivity - Frame", ConnectivityFrame);
            //input.PutDataToSheet("Frame Output Station Assigns", FrameOutputStationAssigns);

            //FrameReleases 前環的軸力放掉、後環的扭矩放掉
            //List<Tuple<string, string, string, string, string, string>> formerSide = new List<Tuple<string, string, string, string, string, string>>();
            //List<Tuple<string, string, string, string, string, string>> latterSide = new List<Tuple<string, string, string, string, string, string>>();
            //for (int i = 0; i < frameNameR1R2.Count; i++)
            //{
            //    formerSide.Add(Tuple.Create("Yes", "No", "No", "No", "No", "No"));
            //    latterSide.Add(Tuple.Create("No", "No", "No", "Yes", "No", "No"));
            //}
            //input.FrameRelease(frameNameR1R2, formerSide, latterSide);  //"Frame Releases 1 - General"

            void FrameToString(ref List<string> frame, List<string> frameName, List<string> jointName)
            {
                for (int i = 0; i < jointName.Count - 1; i++)
                    frame.Add($"{frameName[i]},{jointName[i]},{jointName[i + 1]},No");
                frame.Add($"{frameName[jointName.Count - 1]},{jointName[jointName.Count - 1]},{jointName[0]},No");
            }
        }

        #endregion

        #region FrameSectionAssigns
        double G;
        double Ksb;
        List<double> twoRingInertia = new List<double>();
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
            for (int i = 0; i < R1SGAngle.Count; i++)
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
                for (int j = 0; j < R1DepthAngle.Count; j++)
                {
                    if (XYZSGRing1[i].Item1 == R1DepthAngle[j]) tempBool1 = true;
                    if (XYZSGRing2[i].Item1 == R2DepthAngle[j]) tempBool2 = true;
                }
                R1FrameBool.Add(tempBool1);
                R2FrameBool.Add(tempBool2);
            }

            //List<string> frameAssigns = new List<string>();
            //List<string> R2FrameAssign = new List<string>();
            //int R2sg = 1;
            //int sg = 1;
            //bool tempBool = true;
            //bool R2tempBool = true;
            //for (int i = 0; i < SGNum1Ring; i++)
            //{
            //    if (R1FrameBool[i]) //判斷是否為採用接觸深度之環片
            //    {
            //        switch (SGName.Count)
            //        {
            //            case 2: //1~3次計算
            //                {
            //                    frameAssigns.Add($"{frameNameRing1[i]},Rectangular,N.A.,{SGName[1]},{SGName[1]},Default");
            //                }
            //                break;
            //            default: //最終計算，一次把segment放入兩個相鄰的frame，所以bool作用為跳過一次
            //                {
            //                    if (tempBool)
            //                    {
            //                        frameAssigns.Add($"{frameNameRing1[i]},Rectangular,N.A.,{SGName[sg]},{SGName[sg]},Default");
            //                        frameAssigns.Add($"{frameNameRing1[i + 1]},Rectangular,N.A.,{SGName[sg]},{SGName[sg]},Default");
            //                        sg++;
            //                    }
            //                    tempBool = !tempBool;
            //                }
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        frameAssigns.Add($"{frameNameRing1[i]},Rectangular,N.A.,{SGName[0]},{SGName[0]},Default");
            //    }

            //    if (R2FrameBool[i])
            //    {
            //        switch (SGName.Count)
            //        {
            //            case 2:
            //                R2FrameAssign.Add($"{frameNameRing2[i]},Rectangular,N.A.,{SGName[1]},{SGName[1]},Default");
            //                break;
            //            default:
            //                {
            //                    if (R2tempBool)
            //                    {
            //                        R2FrameAssign.Add($"{frameNameRing2[i]},Rectangular,N.A.,{SGName[R2sg]},{SGName[R2sg]},Default");
            //                        R2FrameAssign.Add($"{frameNameRing2[i + 1]},Rectangular,N.A.,{SGName[R2sg]},{SGName[R2sg]},Default");
            //                        R2sg++;
            //                    }
            //                    R2tempBool = !R2tempBool;
            //                }
            //                break;
            //        }
            //    }
            //    else
            //        R2FrameAssign.Add($"{frameNameRing2[i]},Rectangular,N.A.,{SGName[0]},{SGName[0]},Default");
            //}
            //input.PutDataToSheet("Frame Section Assignments", frameAssigns);
            //input.PutDataToSheet("Frame Section Assignments", R2FrameAssign);

            //計算兩環間的斷面轉換資訊(直徑)            
            HalfAngleCal("SEGMENT");



            G = (SGE / (2 * (1 + SGU12)));

            Ksb = G * Math.PI / 4 * (Math.Pow(SGradiusOut * 2, 2) - Math.Pow((SGradiusOut - SGthick) * 2, 2)) / (SGwidth / 2);
            List<double> KsbDis = new List<double>();
            foreach (double s in SGhalfDistinct)
                KsbDis.Add(Ksb * s / 360);

            double Etemp = 1E10; //計算直徑(兩環的)   
            double L = 2;
            D = new List<double>();
            //double inertia;
            int n = 0;
            foreach (double s in KsbDis)
            {
                twoRingInertia.Add(s * L * L * L / (12 * Etemp));
                //twoRingInertia[n] = s * L * L * L / (12 * Etemp);
                D.Add(Math.Round(Math.Pow(64 * twoRingInertia[n] / Math.PI, 0.25), 4));
                twoRingInertia[n] = Math.Round(twoRingInertia[n], 7);
                n++;
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
            for (int i = 0; i < SGNum1Ring; i++)
            {
                for (int j = 0; j < SGhalfDistinct.Count; j++)
                {
                    if (SGhalfAngle[i] == SGhalfDistinct[j])
                        frameSectionBetweenTwoRing.Add($"{frameNameR1R2[i]},Circle,N.A.,{InterRingName[j]},{InterRingName[j]},Default");
                }
            }
            //input.PutDataToSheet("Frame Section Assignments", frameSectionBetweenTwoRing);

            for(int i = 0; i < frameSectionBetweenTwoRing.Count; i++)
            {
                double tempK = Ksb * SGhalfAngle[i] / 360;
                double inertia = tempK * L * L * L / (12 * Etemp);
                double tempD = Math.Round(Math.Pow(64 * inertia / Math.PI, 0.25), 4);
                var data = Tuple.Create(frameNameR1R2[i], SGhalfAngle[i], Math.Round(tempK,0), tempD);
                segmentDia.Add(data);
            }

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

            //input.PutDataToSheet("Connectivity - Link", LinkSTR);

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
        double soilKs;
        public void LinkPropAndGapAndAssign(string condition)
        {
            HalfAngleCal("SOIL");

            List<string> linkNameForExcel = new List<string>();
            for (int i = 0; i < SoilHalfDistinct.Count; i++)
                linkNameForExcel.Add($"Soil Spring Angle = {SoilHalfDistinct[i]}");

            //計算土壤彈簧的K
            double Em = 0;            
            if (condition == "LongTerm" || condition == "VariationofDiameter" || condition == "EQofDiameter") Em = verticalStress.longTermSoilE;
            else if (condition == "ShortTerm") Em = verticalStress.shortTermSoilE;
            double soilNu = verticalStress.Nu12;
            soilKs = Em * (1 - soilNu) / (SGradiusInter * (1 + soilNu) * (1 - 2 * soilNu));

            List<double> soilDisKs = new List<double>();
            foreach (double s in SoilHalfDistinct)
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

            for (int i = 0; i < linkNameForExcel.Count; i++)
            {
                linkProp.Add($"{linkNameForExcel[i]},Gap,{0.0},{0.0},{0.0001},{0.0001},{0.0001},{1},{1},{0},{0},{0},{0},Yellow");
                linkGap.Add($"{linkNameForExcel[i]},U1,No,Yes,{soilDisKs[i]},0,{soilDisKs[i]},0");
            }
            //input.PutDataToSheet("Link Props 01 - General", linkProp);
            //input.PutDataToSheet("Link Props 05 - Gap", linkGap);

            List<string> linkAssign = new List<string>();
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < SoilNum1Ring; i++)
                {
                    for (int j = 0; j < SoilHalfDistinct.Count; j++)
                    {
                        if (SoilHalfAngle[i] == SoilHalfDistinct[j])
                        {
                            if (k == 0)
                            {
                                linkAssign.Add($"{linkNameSoil1[i]},Gap,TwoJoint,{linkNameForExcel[j]},None"); //第一環

                                var data = Tuple.Create(linkNameSoil1[i], SoilHalfAngle[i], Math.Round(soilDisKs[j], 3));
                                springK.Add(data);
                            }
                            else if (k == 1)
                            {
                                linkAssign.Add($"{linkNameSoil2[i]},Gap,TwoJoint,{linkNameForExcel[j]},None"); //第二環

                                var data = Tuple.Create(linkNameSoil2[i], SoilHalfAngle[i], Math.Round(soilDisKs[j], 3));
                                springK.Add(data);
                            }
                                
                        }
                    }
                }
            }
            //input.PutDataToSheet("Link Property Assignments", linkAssign);

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

        public List<Tuple<string, double, double>> springK = new List<Tuple<string, double, double>>();
        public void process(out string str)
        {
            List<string> nothing = new List<string>();
            VerticalStress();
            SGJointsAndRestraint(3, out List<string> diameterJOintName);
            SGConnectivityFrame_StationAssigns(out List<string> contactingFrameName);
            FrameSectionAssigns(3, nothing, 1, out List<double> D);
            SGConnectivityLink();
            LinkPropAndGapAndAssign("LongTerm");

            double Em = Math.Round(verticalStress.longTermSoilE, 0);

            str = "C.土壤彈簧 <br> ";
            str += $"分析時隧道周圍之被動地層反力，以土壤彈簧來模擬，除隧道冠頂90°範圍外，其餘均勻分布於隧道四周，如下圖所示 <br> ";
            str += $"{image("混凝土環片分析土壤彈簧冠頂90度範圍.PNG")} <br> ";
            str += $"由Duddeck and ErdMann(1982)知土層的地基反力係數Ks可由其楊氏係數Em、柏松比、及襯砌環片半徑R來表示，其關係式如下 <br> ";
            str += $"Ks = [Em(1 - ν)]/[(R(1 + ν)(1 - 2ν)] = [{Em}*(1 - {SGU12})]/" +
                $"[({SGradiusIn}*(1 + {SGU12}) * (1 - 2{SGU12})] = {soilKs}kN/m³ <br> ";
            str += $"Reference:Duddeck and Erdmann(1982) 'Structural Design Models for Tunnel' Tunneling 82, The Institution" +
                $" of Mining and Metallurgy, pp83-91 <br> ";
            str += $"模擬被動土層反力彈簧之計算，如下圖所示 <br> ";

            double tempK009 = Math.Round(soilKs * 0.5 * (54 - 36) / 2 * Math.PI / 180 * SGradiusInter,3);
            str += $"以桿件R1_Link_Soil009為例，其彈簧係數可由地盤反力係數Ks除以彈簧所在範圍求得" +
                $"R1_Link_Soil009 = {Math.Round(soilKs,2)} * 0.5 * (54°-36°)/2 * (π/180) * {SGradiusInter} = {tempK009} kN/m <br> ";


            str += $"<table style='text-align:center' border='5' width='300'> <tr>";
            str += $" <th> 桿件編號 </th> <th> △θ </th> <th> Ksi </th> <tr> ";                        
            for(int i = 0; i < springK.Count; i++)
            {
                str += $" <th> {springK[i].Item1} </th> <th> {springK[i].Item2} </th> <th> {springK[i].Item3.ToString("f3")} </th> <tr> ";
            }
            str += $" </table>";

            SegmentD(out string strDia);
            str = strDia;
        }

        public List<Tuple<string, double, double, double>> segmentDia = new List<Tuple<string, double, double, double>>();
        public void SegmentD(out string strSGDia)
        {
            strSGDia = "";

            strSGDia += $"D. 決定環向連結桿之剪力勁度ksb <br> ";

            strSGDia += $" 如圖 <br> ";

            strSGDia += $"{emsp1()} 當襯砌環片受外力作用時，在環間剪力超過摩擦力之前，兩環間相對的移動將被限制，" +
                $"而僅有環片混凝土本身的剪力變形， 因此其作用力-位移曲線為一初始的斜線及一水平線所組成(如圖所示)，" +
                $"由於整個混凝土環片都會發生剪力變形，因此分析模式中垂直環片的構件皆具有剪力勁度 ，藉以模擬混凝土環片變形。 <br>";

            strSGDia += $"{emsp1()} E1 = {SGE} kN/m² <br> ";
            strSGDia += $"{emsp1()} G = E/2(1 + ν) = {SGE}/2(1 + {SGU12} = {Math.Round(G,0)} kN/m² <br> ";
            strSGDia += $"{emsp1()} τ = T/A = G*γ -> 𝛿 = L*γ <br> ";
            strSGDia += $"{emsp1()} 混凝土整體勁度Ksb可計算如下 <br> ";
            strSGDia += $"{emsp2()} Ksb = T/𝛿 = τ*A/(L*γ) = G*A/L = {Math.Round(G,0)}*π/4*({SGradiusOut}² - {SGradiusIn}²)" +
                $"/{SGwidth/2} = {Math.Round(Ksb,0)} kN/m <br> <br> ";

            strSGDia += $" 將整體勁度分配至各節點桿件所在的區域 <br> ";
            strSGDia += $" {emsp1()} 節點{jointNameRing1[0]} 桿件{segmentDia[0].Item1} <br> ";
            strSGDia += $" {emsp2()} Ksb(1) = {Math.Round(Ksb, 0)}*{segmentDia[0].Item2}/2/360 = {segmentDia[0].Item3} kN/m <br> ";
            strSGDia += $" {emsp1()} 節點{jointNameRing1[1]} 桿件{segmentDia[1].Item1} <br> ";
            strSGDia += $" {emsp2()} Ksb(2) = {Math.Round(Ksb, 0)}*{segmentDia[1].Item2}/2/360 = {segmentDia[1].Item3} kN/m <br> ";
            
            strSGDia += $" {emsp1()} 以此類推 <br> ";
            int last = jointNameRing1.Count - 1;
            strSGDia += $" {emsp1()} 節點{jointNameRing1[last]} 桿件{segmentDia[last].Item1} <br> ";
            strSGDia += $" {emsp2()} Ksb({last + 1}) = {Math.Round(Ksb, 0)}*{segmentDia[last].Item2}/2/360 = " +
                $"{segmentDia[last].Item3} kN/m <br> ";

            strSGDia += $" 定義一虛擬桿件來模擬環向節點 <br> ";

            strSGDia += $"{image("環向節點參考桿件.PNG")} <br> ";

            strSGDia += $" T = 12*E*I/L³*𝛿 <br> ";
            strSGDia += $" T/𝛿 = 12*E*I(i)/L³ = Ksb(i) <br> ";

            strSGDia += $" 假設 L=2.0m E = 1*10¹⁰ kN/m² <br> ";
            strSGDia += $" 則節點{jointNameRing1[0]} 桿件{segmentDia[0].Item1} <br> ";
            strSGDia += $" I(1) = Ksb(1) * 2³/(12*1*10¹⁰) = {segmentDia[0].Item3}*2³/(12*1*10¹⁰) = {twoRingInertia[0]} m⁴ <br> ";
            strSGDia += $" 其等值圓桿件斷面直徑 <br> ";
            strSGDia += $" {emsp1()} 由 I = π*D⁴/64 {emsp3()} D = (64*I/π)¹ᐟ⁴ <br> ";
            strSGDia += $" {emsp1()} D(1) = (64*I(1)/π)¹ᐟ⁴ = (64*{twoRingInertia[0]}/π)¹ᐟ⁴ = {segmentDia[0].Item4} m <br> ";

            strSGDia += $" {emsp1()} 因此環片1之節點{jointNameRing1[0]}與環片2之節點{jointNameRing2[0]}間，" +
                $"以一圓形的桿件{segmentDia[0].Item1}連接，而其直徑即為{segmentDia[0].Item4}m，" +
                $"當襯砌受外力時   以此桿件之變形來模擬兩環片的剪力變形 <br> ";
            strSGDia += $" 節點{jointNameRing1[1]} 桿件{segmentDia[1].Item1} <br> ";
            strSGDia += $" I(1) = Ksb(1) * 2³/(12*1*10¹⁰) = {segmentDia[1].Item3}*2³/(12*1*10¹⁰) = {twoRingInertia[1]} m⁴ <br> ";
            strSGDia += $" 其等值圓桿件斷面直徑 <br> ";
            strSGDia += $" {emsp1()} 由 I = π*D⁴/64 {emsp3()} D = (64*I/π)¹ᐟ⁴ <br> ";
            strSGDia += $" {emsp1()} D(1) = (64*I(1)/π)¹ᐟ⁴ = (64*{twoRingInertia[1]}/π)¹ᐟ⁴ = {segmentDia[1].Item4} m <br> ";
            strSGDia += $" (此為分析模式中之桿件{segmentDia[1].Item1}) <br> ";

            strSGDia += $" 以此類推剩餘之環向桿件，將環向桿件整理如下表 <br> ";

            strSGDia += $"<table style='text-align:center' border='5' width='300'> <tr> ";
            strSGDia += $"<th> 桿件編號 </th> <th> △θ </th> <th> Ki </th> <th> Di </th> <tr> ";

            for(int i = 0; i < segmentDia.Count; i++)
            {
                strSGDia += $" <th> {segmentDia[i].Item1} </th> <th> {segmentDia[i].Item2} </th> " +
                    $"<th> {segmentDia[i].Item3} </th> <th> {segmentDia[i].Item4} </th> <tr> ";
            }
            strSGDia += $"</table> ";
        }


        string emsp4() { return "&emsp; &emsp; &emsp; &emsp;"; }
        string emsp3() { return "&emsp; &emsp; &emsp;"; }
        string emsp2() { return "&emsp; &emsp;"; }
        string emsp1() { return "&emsp;"; }

        public string image(string str) { return $"<img src='{str}'></img> "; }
    }

    
}
