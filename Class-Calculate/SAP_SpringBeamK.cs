using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTunnel.Class_Calculate
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

        #region Link Prop Gap Assign
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
            double soilKs = Em * (1 - soilNu) / (SGradiusInter * (1 + soilNu) * (1 - 2 * soilNu));

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
                            if (k == 0) linkAssign.Add($"{linkNameSoil1[i]},Gap,TwoJoint,{linkNameForExcel[j]},None"); //第一環
                            else if (k == 1) linkAssign.Add($"{linkNameSoil2[i]},Gap,TwoJoint,{linkNameForExcel[j]},None"); //第二環
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
    }
}
