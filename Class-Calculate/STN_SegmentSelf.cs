using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using STN_SQL;
using System.Data;
//using Xceed.Words.NET;



namespace SinoTunnel
{
    public class STN_SegmentSelf
    {
        string sectionUID = "";
        ExcuteSQL  dataSearch = new ExcuteSQL();
        GetWebData p;

        public STN_SegmentSelf(string sectionUID)
        {
            this.sectionUID = sectionUID;
            p = new GetWebData( sectionUID);
        }

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public void Transportation(string outputCondition, out string OutputTransportation, out double Mmax, out double Vmax)
        {
            DataTable segment = dataSearch.GetByUID("STN_Section", sectionUID);
            double RadiusIn = double.Parse(segment.Rows[0]["SGRadiusIn"].ToString());
            double Thickness = double.Parse(segment.Rows[0]["SGThickness"].ToString());
            double Angle = double.Parse(segment.Rows[0]["SGAngle"].ToString());
            double AdjacentPoreAngle = double.Parse(segment.Rows[0]["AdjPoreAngle"].ToString());
            double UnitWeight = double.Parse(segment.Rows[0]["SGUnitWeight"].ToString());

            double RadiusInter = RadiusIn + Thickness / 2;
            double RadiusOut = RadiusIn + Thickness;

            #region 吊放
            double L = 2 * (RadiusInter) * Math.Sin(Angle / 2 * Math.PI / 180);//環片投影周長         
            double w = Math.PI * (Math.Pow(RadiusOut, 2) - Math.Pow(RadiusIn, 2)) * Angle / 360 * UnitWeight / L;//單位長度環片自重
            double L2 = 2 * (RadiusInter) * Math.Sin(AdjacentPoreAngle / 2 * Math.PI / 180);
            double L1 = (L - L2) / 2;

            //計算當 0 <= X <= L1 時的最大彎矩
            //double TransportationMoment01 = Math.Round(-1 * w * Math.Pow(L / 2 - RadiusInter * Math.Sin(AdjacentPoreAngle / 2 * Math.PI / 180), 2) / 2, 2);
            double TransportationMoment01 = Math.Round(-1 * w * Math.Pow(L1, 2) / 2, 2);
            //計算當 L1 <= X <= L/2 時的最大彎矩
            //double TransportationMoment02 = Math.Round((-1 * w * Math.Pow(L / 2, 2) / 2) + (L / 2 * w * (L / 2 - (L / 2 - RadiusInter * Math.Sin(AdjacentPoreAngle / 2 * Math.PI / 180)))), 2);
            double TransportationMoment02 = Math.Round((-1 * w * Math.Pow(L / 2, 2) / 2) + (L / 2 * w * (L / 2 - L1)), 2);
            double maxTransportationMoment = Math.Max(Math.Abs(TransportationMoment01), Math.Abs(TransportationMoment02));
            Mmax = maxTransportationMoment;

            double TransportationShearForce = Math.Round((w * L / 2) - (w * L1), 2);//計算最大剪力
            Vmax = TransportationShearForce;

            #endregion

            OutputTransportation = "";
            string LOut = "";
            string wOut = "";
            string L2Out = "";
            string L1Out = "";
            string Mmax1Out = "";
            string Mmax2Out = "";
            string MmaxOut = "";
            string VmaxOut = "";

            #region 吊放output
            //if (outputCondition == "winform" || outputCondition == "winForm" || outputCondition == "Winform" || outputCondition == "WinForm")
            //{
            //    LOut = string.Format("L = 2 * ({0} + {1}/2) * sin{2}\u00B0 = {3} m", RadiusIn, Thickness, AdjacentPoreAngle, Math.Round(L, 2));
            //    wOut = string.Format("w = \uD835\uDF0B * ({0}\u00b2 - {1}\u00b2) * {2}\u00B0/360\u00B0 * {3}/{4} = {5} kN/m", RadiusOut, RadiusIn, Angle, UnitWeight, Math.Round(L, 2), Math.Round(w, 2));
            //    L2Out = string.Format("L2 = 2 * ({0} + {1}/2) * sin{2}\u00B0 = {3} m", RadiusIn, Thickness, AdjacentPoreAngle / 2, Math.Round(L2, 2));
            //    L1Out = string.Format("L1 = (L-L2)/2 = {0} m", Math.Round(L1, 2));

            //    Mmax1Out = string.Format("0 \u2264 X \u2264 L1：M = -(wX\u00b2)/2 <br> &emsp; Mmax1 = {0} kN-m (X = L1)", TransportationMoment01);
            //    Mmax2Out = string.Format("L1 \u2264 X \u2264 L/2：M = -(wX\u00b2)/2 + L/2 * w *(X - L1) <br> &emsp; Mmax2 = {0} kN-m (X = L/2)", TransportationMoment02);
            //    //MmaxOut is the same string
            //    //VmaxOut = string.Format("Vmax = (w * L/2) - (w * L1) = {0} kN", TransportationShearForce);
            //    VmaxOut = $"Vmax = (w * L/2) - (w * L1) = {TransportationShearForce} kN";
            //}
            //else if (outputCondition == "webform" || outputCondition == "webForm" || outputCondition == "Webform" || outputCondition == "WebForm")
            //{
            //    LOut = string.Format("L = 2 * ({0} + <div class='frac'><span>{1}</span><span class='symbol'>/</span><span class='bottom'>2</span></div>) * sin{2}&#176 = {3} m", RadiusIn, Thickness, AdjacentPoreAngle, Math.Round(L, 2));
            //    wOut = string.Format("w = π * ({0}&#178 - {1}&#178) * <div class='frac'><span>{2}&#176</span><span class='symbol'>/</span><span class='bottom'>360&#176</span></div> * <div class='frac'><span>{3}</span><span class='symbol'>/</span><span class='bottom'>{4}</span></div> = {5} kN/m", RadiusOut, RadiusIn, Angle, UnitWeight, Math.Round(L, 2), Math.Round(w, 2));
            //    L2Out = string.Format("L2 = 2 * ({0} + <div class='frac'><span>{1}</span><span class='symbol'>/</span><span class='bottom'>2</span></div>) * sin{2}&#176 = {3} m", RadiusIn, Thickness, AdjacentPoreAngle / 2, Math.Round(L2, 2));
            //    L1Out = string.Format("L1 = <div class='frac'><span>L-L2</span><span class='symbol'>/</span><span class='bottom'>2</span></div> = {0} m", Math.Round(L1, 2));

            //    Mmax1Out = string.Format("0 &#8804 X &#8804 L1：M = <div class='frac'><span>-wX&#178</span><span class='symbol'>/</span><span class='bottom'>2</span></div> <br> &emsp; Mmax1 = {0} kN-m (X = L1)", TransportationMoment01);
            //    Mmax2Out = string.Format("L1 &#8804 X &#8804 <div class='frac'><span>L</span><span class='symbol'>/</span><span class='bottom'>2</span></div>：M = <div class='frac'><span>-wX&#178</span><span class='symbol'>/</span><span class='bottom'>2</span></div> + X * w (X - L1) <br> &emsp; Mmax2 = {0} kN-m (X = <div class='frac'><span>L</span><span class='symbol'>/</span><span class='bottom'>2</span></div>)", TransportationMoment02);
            //    MmaxOut = string.Format("Mmax = max(|Mmax1|, |Mmax2|) <br> &emsp; = {0} kN-m", maxTransportationMoment);

            //    VmaxOut = string.Format("Vmax = (w * <div class='frac'><span>L</span><span class='symbol'>/</span><span class='bottom'>2</span></div>) - ( w * L1) <br> &emsp; = {0} kN", TransportationShearForce);

            //    OutputTransportation = $"環片投影周長 L <br> &emsp; {LOut} <br> 單位長度環片自重 w <br> &emsp; {wOut} <br> &emsp; {L2Out} <br> &emsp; {L1Out} <br> 彎矩計算 <br> &emsp; {Mmax1Out} <br> &emsp; {Mmax2Out} <br> {MmaxOut} <br> {VmaxOut}";
            //}
            //else if(outputCondition == "word" || outputCondition == "Word")
            //{
            //    string template = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\template.docx";
            //    //FileStream fileread = new FileStream(template, FileMode.Open, FileAccess.ReadWrite);

            //    DocX document = DocX.Load(template);
            //    //DocX document = DocX.Create(filePath);

            //    Paragraph pa = document.InsertParagraph("吊放環片過程彎矩計算 \n").FontSize(14);
            //    pa.Append($"環片內半徑 Ri = {p.segmentRadiusIn} m\n");
            //    pa.Append($"環片厚度 t = {p.segmentThickness} m\n");
            //    pa.Append($"環片角度 θ = {p.segmentAAngle}°\n");
            //    pa.Append($"相鄰螺栓孔角度 θ = {p.segmentAdjacentPoreAngle}°\n");
            //    pa.Append($"環片單位重 γ = {p.segmentUnitWeight} kN/m²\n\n");


            //    pa.Append($"環片投影周長 L = 2 * ({p.segmentRadiusIn} + {p.segmentThickness} / 2) * sin{p.segmentAdjacentPoreAngle}° = {Math.Round(L, 2)} m\n");
            //    pa.Append($"單位長度環片自重 w = π * ({p.segmentRadiusOut}² - {p.segmentRadiusIn}²) * ({p.segmentAngle} / 360) * ({p.segmentUnitWeight} / {Math.Round(L, 2)}) = {Math.Round(w, 2)} kN/m\n");
            //    pa.Append($"    L2 = 2 * ({p.segmentRadiusIn} + {p.segmentThickness} / 2) * sin{p.segmentAdjacentPoreAngle / 2}° = {Math.Round(L2, 2)} m\n");
            //    pa.Append($"    L1 = (L - L2) / 2 = {Math.Round(L1, 2)} m\n");

            //    pa.Append($"彎矩計算\n");
            //    pa.Append($"    0 ≤ X ≤ L1：M = -(wX²)/2\n");
            //    pa.Append($"    Mmax1 = {TransportationMoment01} kN-m\n");

            //    pa.Append($"    L1 ≤ X ≤ L/2：M = -(wX²)/2 + X * w * (X - L1)\n");
            //    pa.Append($"    Mmax2 = {TransportationMoment02} kN-m\n\n");
            //    pa.Append($"Mmax = {Mmax} kN-m\n");
            //    pa.Append($"Vmax = (w * L/2) - (w * L1) = {Vmax} kN");

            //    document.SaveAs(filePath);
            //}


            switch (outputCondition.ToUpper().ToString())
            {
                case "WINFORM":
                    {
                        LOut = string.Format("L = 2 * ({0} + {1}/2) * sin{2}\u00B0 = {3} m", RadiusIn, Thickness, AdjacentPoreAngle, Math.Round(L, 2));
                        wOut = string.Format("w = \uD835\uDF0B * ({0}\u00b2 - {1}\u00b2) * {2}\u00B0/360\u00B0 * {3}/{4} = {5} kN/m", RadiusOut, RadiusIn, Angle, UnitWeight, Math.Round(L, 2), Math.Round(w, 2));
                        L2Out = string.Format("L2 = 2 * ({0} + {1}/2) * sin{2}\u00B0 = {3} m", RadiusIn, Thickness, AdjacentPoreAngle / 2, Math.Round(L2, 2));
                        L1Out = string.Format("L1 = (L-L2)/2 = {0} m", Math.Round(L1, 2));

                        Mmax1Out = string.Format("0 \u2264 X \u2264 L1：M = -(wX\u00b2)/2 <br> &emsp; Mmax1 = {0} kN-m (X = L1)", TransportationMoment01);
                        Mmax2Out = string.Format("L1 \u2264 X \u2264 L/2：M = -(wX\u00b2)/2 + L/2 * w *(X - L1) <br> &emsp; Mmax2 = {0} kN-m (X = L/2)", TransportationMoment02);
                        //MmaxOut is the same string
                        //VmaxOut = string.Format("Vmax = (w * L/2) - (w * L1) = {0} kN", TransportationShearForce);
                        VmaxOut = $"Vmax = (w * L/2) - (w * L1) = {TransportationShearForce} kN";
                    }
                    break;
                case "WEBFORM":
                    {
                        LOut = string.Format("L = 2 * ({0} + {1}/2) * sin{2}° = {3} m", RadiusIn, Thickness, AdjacentPoreAngle, Math.Round(L, 2));
                        wOut = string.Format("w = π * ({0}² - {1}²) * {2}°/360° * {3}/{4} = {5} kN/m", RadiusOut, RadiusIn, Angle, UnitWeight, Math.Round(L, 2), Math.Round(w, 2));
                        L2Out = string.Format("L2 = 2 * ({0} + {1}/2) * sin{2}° = {3} m", RadiusIn, Thickness, AdjacentPoreAngle / 2, Math.Round(L2, 2));
                        L1Out = string.Format("L1 = (L-L2)/2 = {0} m", Math.Round(L1, 2));

                        Mmax1Out = string.Format("0 ≤ X ≤ L1：M = (-wX²)/2 <br> &emsp; Mmax1 = {0} kN-m (X = L1)", TransportationMoment01);
                        Mmax2Out = string.Format("L1 ≤ X ≤ L/2：M = (-wX²)/2 + X * w (X - L1) <br> &emsp; Mmax2 = {0} kN-m (X = L/2)", TransportationMoment02);
                        MmaxOut = string.Format("Mmax = max(|Mmax1|, |Mmax2|) <br> &emsp; = {0} kN-m", maxTransportationMoment);

                        VmaxOut = string.Format("Vmax = (w * L/2) - ( w * L1) <br> &emsp; = {0} kN", TransportationShearForce);

                        OutputTransportation = $"2.2 吊放環片過程彎矩計算 <br> ";
                        OutputTransportation += $" <table style='text-align:left' border='0'> <tr> ";
                        OutputTransportation += $" <th> 環片內半徑 R </th> <th> = {RadiusIn}m </th> <tr> ";
                        OutputTransportation += $" <th> 環片厚度 TH </th> <th> = {Thickness}m </th> <tr> ";
                        OutputTransportation += $" <th> 環片角度 </th> <th> = {Angle}° </th> <tr> ";
                        OutputTransportation += $" <th> 相鄰螺栓孔角度 </th> <th> = {AdjacentPoreAngle}° </th> <tr> ";
                        OutputTransportation += $" <th> 環片單位重 γ </th> <th> = {UnitWeight}kN/m³ </th> <tr> ";
                        OutputTransportation += $" </table> ";
                        OutputTransportation += $" 環片投影周長 L <br> &emsp; {LOut} <br> 單位長度環片自重 w <br> &emsp; {wOut} <br> &emsp; {L2Out} <br> &emsp; {L1Out} <br> 彎矩計算 <br> &emsp; {Mmax1Out} <br> &emsp; {Mmax2Out} <br> {MmaxOut} <br> {VmaxOut}";
                    }
                    break;
                    /*
                case "WORD":
                    {
                        string template = "O:\\ADMIN\\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\\09-軟體\\SinoTunnel_WinForm\\template.docx";
                        //FileStream fileread = new FileStream(template, FileMode.Open, FileAccess.ReadWrite);

                        DocX document = DocX.Load(template);
                        //DocX document = DocX.Create(filePath);

                        Paragraph pa = document.InsertParagraph("吊放環片過程彎矩計算 \n").FontSize(14);
                        pa.Append($"環片內半徑 Ri = {p.segmentRadiusIn} m\n");
                        pa.Append($"環片厚度 t = {p.segmentThickness} m\n");
                        pa.Append($"環片角度 θ = {p.segmentAAngle}°\n");
                        pa.Append($"相鄰螺栓孔角度 θ = {p.segmentAdjacentPoreAngle}°\n");
                        pa.Append($"環片單位重 γ = {p.segmentUnitWeight} kN/m²\n\n");


                        pa.Append($"環片投影周長 L = 2 * ({p.segmentRadiusIn} + {p.segmentThickness} / 2) * sin{p.segmentAdjacentPoreAngle}° = {Math.Round(L, 2)} m\n");
                        pa.Append($"單位長度環片自重 w = π * ({p.segmentRadiusOut}² - {p.segmentRadiusIn}²) * ({p.segmentAngle} / 360) * ({p.segmentUnitWeight} / {Math.Round(L, 2)}) = {Math.Round(w, 2)} kN/m\n");
                        pa.Append($"    L2 = 2 * ({p.segmentRadiusIn} + {p.segmentThickness} / 2) * sin{p.segmentAdjacentPoreAngle / 2}° = {Math.Round(L2, 2)} m\n");
                        pa.Append($"    L1 = (L - L2) / 2 = {Math.Round(L1, 2)} m\n");

                        pa.Append($"彎矩計算\n");
                        pa.Append($"    0 ≤ X ≤ L1：M = -(wX²)/2\n");
                        pa.Append($"    Mmax1 = {TransportationMoment01} kN-m\n");

                        pa.Append($"    L1 ≤ X ≤ L/2：M = -(wX²)/2 + X * w * (X - L1)\n");
                        pa.Append($"    Mmax2 = {TransportationMoment02} kN-m\n\n");
                        pa.Append($"Mmax = {Mmax} kN-m\n");
                        pa.Append($"Vmax = (w * L/2) - (w * L1) = {Vmax} kN");

                        document.SaveAs(filePath);
                    }
                    break;
                    */
            }
            #endregion
        }
        /*
        public void wordTranportation(string filePath)
        {
            DocX document = DocX.Create(filePath);

            Paragraph pa = document.InsertParagraph("吊放環片過程彎矩計算 \n").FontSize(15).Font("標楷體");
            pa.Append($"環片內半徑 Ri = {p.segmentRadiusIn} m\n");
            pa.Append($"環片厚度 t = {p.segmentThickness} m\n");
            pa.Append($"環片角度 θ = {p.segmentAAngle} Deg\n");
            pa.Append($"相鄰螺栓孔角度 θ = {p.segmentAdjacentPoreAngle} Deg\n");
            pa.Append($"環片單位重 γ = {p.segmentUnitWeight} kN/m²");

            //pa.Append($"環片投影周長 L = 2 * ({p.segmentRadiusIn} + {p.segmentThickness} / 2")

            document.SaveAs(filePath);
        }
        */
        public void Stacking(string outputCondition, out string OutputStacking, out double StackingMmax, out double StackingVmax)
        {
            DataTable segment = dataSearch.GetByUID("STN_Section", sectionUID);
            double RadiusIn = double.Parse(segment.Rows[0]["SGRadiusIn"].ToString());
            double Thickness = double.Parse(segment.Rows[0]["SGThickness"].ToString());
            double AAngle = double.Parse(segment.Rows[0]["SGAAngle"].ToString());
            double BAngle = double.Parse(segment.Rows[0]["SGBAngle"].ToString());
            double KAngle = double.Parse(segment.Rows[0]["SGKAngle"].ToString());
            double UnitWeight = double.Parse(segment.Rows[0]["SGUnitWeight"].ToString());
            double StackingL1 = double.Parse(segment.Rows[0]["StackingL1"].ToString());
            double StackingL2 = double.Parse(segment.Rows[0]["StackingL2"].ToString());
            double StackingL3 = double.Parse(segment.Rows[0]["StackingL3"].ToString());

            double RadiusInter = RadiusIn + Thickness / 2;
            double RadiusOut = RadiusIn + Thickness;

            #region 現場堆置
            //環片現場堆置彎矩計算
            //B1環片檢核
            //B1環片投影周長 L
            double B1ProjectedL = 2 * RadiusInter * Math.Sin(BAngle / 2 * Math.PI / 180);
            //B1單位長度環片自重 w
            double B1UnitLengthw = Math.PI * (Math.Pow(RadiusOut, 2) - Math.Pow(RadiusIn, 2)) * (BAngle + KAngle) / 360 * UnitWeight / B1ProjectedL;

            //彎矩計算????
            double B1stackingMoment = B1UnitLengthw / 2 * B1ProjectedL / 2 * B1ProjectedL;

            //A3環片檢核
            double A3ProjectedL = 2 * RadiusInter * Math.Sin(AAngle / 2 * Math.PI / 180);
            //單位徑度環片自重
            double UnitRadWeight = Math.PI * (Math.Pow(RadiusOut, 2) - Math.Pow(RadiusIn, 2)) * (1) / 360 * UnitWeight;
            //集中荷重P
            double ConcentratedLoad = UnitRadWeight * (KAngle + 2 * BAngle + 2 * AAngle) / 2;
            //A單位長度環片自重
            double AUnitLengthWeight = Math.PI * (Math.Pow(RadiusOut, 2) - Math.Pow(RadiusIn, 2)) * AAngle / 360 * UnitWeight / A3ProjectedL;
            //支撐反力R
            double SupportReaction = ConcentratedLoad + AUnitLengthWeight * A3ProjectedL / 2;


            //彎矩計算
            //1. 端點至支撐點
            double StackingMoment01 = -1 * AUnitLengthWeight / 2 * Math.Pow((StackingL1 + StackingL2), 2) - ConcentratedLoad * StackingL2;
            //2. 端點至支撐點
            double StackingMoment02 = -1 * AUnitLengthWeight / 2 * Math.Pow(StackingL1 + StackingL2 + StackingL3, 2) - ConcentratedLoad * (StackingL2 + StackingL3) + SupportReaction * StackingL3;
            //Mmax
            StackingMmax = Math.Max(Math.Abs(StackingMoment01), Math.Abs(StackingMoment02));
            StackingMmax = Math.Round(Math.Max((StackingMmax), Math.Abs(B1stackingMoment)), 2);

            //Vmax
            StackingVmax = Math.Round(SupportReaction - AUnitLengthWeight * StackingL3, 2);

            #endregion

            OutputStacking = "";
            string B1ProjectedLOut = "";
            string B1UnitLengthwOut = "";
            string B1StackingMomentOut = "";
            string A3ProjectedLOut = "";
            string UnitRadWeightOut = "";
            string ConcentratedLoadOut = "";
            string AUnitLengthWeightOut = "";
            string SupportReactionOut = "";
            string StackingMoment01Out = "";
            string StackingMoment02Out = "";
            string StackingMomentOut = "";
            string StackingShearOut = "";

            #region 堆置output
            if (outputCondition.ToUpper().ToString() == "WEBFORM")
            {
                //B1環片投影周長 L
                B1ProjectedLOut = string.Format("L = 2 * ({0} + {1}/2) * sin{2}° = {3} m", RadiusIn, Thickness, BAngle / 2, Math.Round(B1ProjectedL, 2));
                //B1單位長度環片自重 w
                B1UnitLengthwOut = string.Format("w(B1 + K) = π * ({0}² - {1}²) * ({2}° + {3}°)/360° * {4}/{5} = {6} kN/m", RadiusOut, RadiusIn, BAngle, KAngle, UnitWeight, Math.Round(B1ProjectedL, 2), Math.Round(B1UnitLengthw, 2));

                //彎矩計算
                B1StackingMomentOut = $"Mmax1 = wL/2 * L/2 = {Math.Round(B1stackingMoment, 2)} kN-m";

                //A3環片檢核
                A3ProjectedLOut = string.Format("L = 2 * ({0} + {1}/2) * sin{2}° = {3} m", RadiusIn, Thickness, AAngle / 2, Math.Round(A3ProjectedL, 2));
                //單位徑度環片自重
                UnitRadWeightOut = string.Format("w = π * ({0}² - {1}²) * (1)/360 * {2} = {3} kN/m", RadiusOut, RadiusIn, UnitWeight, Math.Round(UnitRadWeight, 2));
                //集中荷重 P
                ConcentratedLoadOut = string.Format("P = {0} * ({1} + 2 * {2} + 2 * {3})/2 = {4} kN", Math.Round(UnitRadWeight, 2), KAngle, BAngle, AAngle, Math.Round(ConcentratedLoad, 2));
                //A單位長度環片自重 Wu
                AUnitLengthWeightOut = $"Wu = π * ({RadiusOut}² - {RadiusIn}²) * {AAngle}°/360° * {UnitWeight}/{Math.Round(A3ProjectedL, 2)} = {Math.Round(AUnitLengthWeight, 2)} kN/m";
                //支撐反力 R
                SupportReactionOut = $"R = {Math.Round(ConcentratedLoad, 2)} + {Math.Round(AUnitLengthWeight, 2)} *  {Math.Round(A3ProjectedL, 2)}/2 = {Math.Round(SupportReaction, 2)} kN";

                //彎矩計算
                //1. 端點至支撐點
                StackingMoment01Out = $"0 ≤ X ≤ L1 + L2：<br> &emsp; Mmax2 = (-wX²)/2 - P * (X - L1) <br> &emsp; &emsp; = - {Math.Round(AUnitLengthWeight, 2)}/2 * ({StackingL1} + {StackingL2})² - {Math.Round(ConcentratedLoad, 2)} * {StackingL2} <br> &emsp; &emsp;= {Math.Round(StackingMoment01, 2)} kN-m &emsp; (X = L1 + L2)";
                //2. 端點至支撐點
                StackingMoment02Out = $"(L1 + L2) ≤ X ≤ L1 + L2 + L3：<br> &emsp; Mmax3 = (-wX²)/2 - P * (X - L1) + R * (X - L1 -L2) <br> &emsp; &emsp; = - {Math.Round(AUnitLengthWeight, 2)}/2 * ({StackingL1} + {StackingL2} + {StackingL3})² - {Math.Round(ConcentratedLoad, 2)} * ({StackingL2} + {StackingL3}) + {Math.Round(SupportReaction, 2)} * {StackingL3} <br> &emsp; &emsp; = {Math.Round(StackingMoment02, 2)} kN-m  &emsp; (X = L1 + L2 + L3)";
                //最大彎矩
                StackingMomentOut = $"Mmax = max(|Mmax1|, |Mmax2|, |Mmax3|) <br> &emsp; &emsp; &emsp; = {StackingMmax} kN-m";
                //最大剪力
                StackingShearOut = $"Vmax = {Math.Round(SupportReaction, 2)} - ({Math.Round(AUnitLengthWeight, 2)} * {StackingL3}) <br> &emsp; &emsp; &emsp; = {StackingVmax} kN";

                OutputStacking = $"B1 環片檢核 <br> B1環片投影周長 L <br> &emsp; {B1ProjectedLOut} <br> B1單位長度環片自重 w <br> &emsp; {B1UnitLengthwOut} <br> 彎矩計算 <br> &emsp; {B1StackingMomentOut} <br> A3 環片檢核 <br> &emsp;{A3ProjectedLOut} <br> 單位徑度環片自重 <br> &emsp; {UnitRadWeightOut} <br> 集中荷重 P <br> &emsp; {ConcentratedLoadOut} <br> A 單位長度環片自重 Wu <br> &emsp; {AUnitLengthWeightOut} <br> 支撐反力 R <br> &emsp; {SupportReactionOut} <br> 彎矩計算 <br> L1 = {StackingL1} m &emsp; L2 = {StackingL2} m &emsp; L3 = {StackingL3} m <br> 1. 端點至支撐點 <br> &emsp; {StackingMoment01Out} <br> 2. 端點至支撐點 <br> &emsp; {StackingMoment02Out} <br> &emsp; {StackingMomentOut} <br> &emsp; {StackingShearOut}";
            }
            #endregion
        }

        public void Hanging(string OutputCondition, out string OutputHanging, out double HangingMmax, out double HangingVmax)
        {
            DataTable segment = dataSearch.GetByUID("STN_Section", sectionUID);
            double RadiusIn = double.Parse(segment.Rows[0]["SGRadiusIn"].ToString());
            double Thickness = double.Parse(segment.Rows[0]["SGThickness"].ToString());
            double Angle = double.Parse(segment.Rows[0]["SGAngle"].ToString());
            double AdjGroutAngle = double.Parse(segment.Rows[0]["AdjGroutAngle"].ToString());
            double UnitWeight = double.Parse(segment.Rows[0]["SGUnitWeight"].ToString());

            double RadiusInter = RadiusIn + Thickness / 2;
            double RadiusOut = RadiusIn + Thickness;

            #region 吊裝
            //環片投影周長 L
            double HangingL = 2 * (RadiusInter) * Math.Sin(Angle / 2 * Math.PI / 180);
            //單位長度環片自重 w
            double Hangingw = Math.PI * (Math.Pow(RadiusOut, 2) - Math.Pow(RadiusIn, 2)) * Angle / 360 * UnitWeight / HangingL;
            double HangingL2 = 2 * RadiusInter * Math.Sin(AdjGroutAngle / 2 * Math.PI / 180);
            double HangingL1 = (HangingL - HangingL2) / 2;

            //彎矩計算
            double HangingMmax1 = Math.Round(-1 * Hangingw * Math.Pow(HangingL / 2 - RadiusInter * Math.Sin(AdjGroutAngle / 2 * Math.PI / 180), 2) / 2, 2);
            double HangingMmax2 = Math.Round((-1 * Hangingw * Math.Pow(HangingL / 2, 2) / 2) + (HangingL / 2 * Hangingw * (HangingL / 2 - (HangingL / 2 - RadiusInter * Math.Sin(AdjGroutAngle / 2 * Math.PI / 180)))), 2);
            HangingMmax = Math.Max(Math.Abs(HangingMmax1), Math.Abs(HangingMmax2));

            HangingVmax = Math.Round((Hangingw * HangingL / 2) - (Hangingw * HangingL2 / 2), 2);
            #endregion

            OutputHanging = "";
            string HangingLOut = "";
            string HangingwOut = "";
            string HangingL2Out = "";
            string HangingL1Out = "";
            string HangingMmax1Out = "";
            string HangingMmax2Out = "";
            string HangingMmaxOut = "";
            string HangingVmaxOut = "";

            #region 吊裝 output
            if (OutputCondition.ToUpper().ToString() == "WEBFORM")
            {
                //環片投影周長 L
                HangingLOut = $"L = 2 * ({RadiusIn} + {Thickness}/2) * sin{Angle / 2}° = {Math.Round(HangingL, 2)} m";
                //單位長度環片自重 w
                HangingwOut = $"w = π * ({RadiusOut}² - {RadiusIn}²) * {Angle}°/360° * {UnitWeight}/{Math.Round(HangingL, 2)} = {Math.Round(Hangingw, 2)} kN/m";
                HangingL2Out = $"L2 = 2 * ({RadiusIn} + {Thickness}/2) * sin{AdjGroutAngle / 2}° = {Math.Round(HangingL2, 2)} m";
                HangingL1Out = $"L1 = (L - L2)/2 = {Math.Round(HangingL1, 2)} m";

                //彎矩計算
                HangingMmax1Out = $"0 ≤ X ≤ L1：<br> &emsp; Mmax1 = (-wX²)/2 <br> &emsp; &emsp; = {Math.Round(HangingMmax1, 2)} kN-m &emsp; (X = L1 + L2)";
                HangingMmax2Out = $"L1 ≤ X ≤ L/2：<br> &emsp; Mmax2 = (-wX²)/2 + X * w (X - L1) <br> &emsp; &emsp; = {Math.Round(HangingMmax2, 2)} kN-m &emsp; (X = L/2)";
                HangingMmaxOut = $"Mmax = max(|Mmax1|, |Mmax2|) <br> &emsp; &emsp; = {Math.Round(HangingMmax, 2)} kN-m";
                HangingVmaxOut = $"VMax = (w * L/2) * (w * L1) <br> &emsp; &emsp; = {Math.Round(HangingVmax, 2)}";

                OutputHanging = $"環片投影周長 L <br> &emsp; {HangingLOut} <br> 單位長度環片自重 w <br> &emsp; {HangingwOut} <br> &emsp; {HangingL2Out} <br> &emsp; {HangingL1Out} <br> 彎矩計算 <br> {HangingMmax1Out} <br> {HangingMmax2Out} <br> {HangingMmaxOut} <br> {HangingVmaxOut}";
            }
            #endregion
        }
    }
}
