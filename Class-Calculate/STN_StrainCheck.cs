using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SinoTunnel
{
    public class STN_StrainCheck
    {
        GetWebData p;      
        STN_VerticalStress verticalStress;
        ExcuteSQL oExecuteSQL = new ExcuteSQL();

        public string outLooseFRatio = "";

        string sectionUID;
        double soilEm;
        double segmentE1 = 0;
        double segmentU12 = 0;
        double soilU12 = 0;

        double verticalCompressionStrain = 0.002; //縱向容許壓應變，參考CEDC A VI.B.2
        double FlexuralCompressionStrain = 0.003; //撓曲容許壓應變，參考CEDC A VI.B.5
        double tensionStrain = 0.0;

        int lineHeight = 25;
        string outputCondition;

        double Fy;
        public STN_StrainCheck(string sectionUID, string outputCondition)
        {
            this.outputCondition = outputCondition;
            p = new GetWebData(sectionUID);

            this.sectionUID = sectionUID;
            verticalStress = new STN_VerticalStress(sectionUID, "");
            verticalStress.VerticalStress("TUNNEL", out string longtermVerticalStress, out string shortermVerticalStress, out string ouputSurchargeLoad, out double longtermE1, out double shortermE1, out double Pv, out double lph1, out double lph2, out double sph1, out double sph2, out double U12);
            
            this.soilU12 = U12;

            this.segmentE1 = p.segmentYoungsModulus;
            this.segmentU12 = p.segmentPoissonRatio;

            this.ODEg = p.ODEg;
            this.MDEg = p.MDEg;

            this.ODEVmax = p.ODEVh;
            this.MDEVmax = p.MDEVh;

            this.ODEAmax = p.ODEah;
            this.MDEAmax = p.MDEah;

            this.shearWave = p.shearwaveV;

            this.tensionStrain = Math.Round(p.segmentFy / p.steelEs, 4);
            this.Fy = p.segmentFy;

            
        }        
        
        double shearWave = 0;

        double ODEg = 0;
        double ODEAmax = 0;
        double ODEVmax = 0;

        double MDEg = 0;
        double MDEAmax = 0;
        double MDEVmax = 0;

        //double steel = 4200;
        //double steelE = 2.04E06;

        bool[] tempBool = new bool[2];
        bool[] ODEbool = new bool[2];
        bool[] MDEbool = new bool[2];

        #region 地震引致之隧道縱向應變
        public void VerticalStainByEQ(out string outVerticalStain, out double[] ODEStrain, out double[] MDEStrain)
        {
            outVerticalStain = "";
            //strain = Math.Sqrt(Math.Pow(ODEVmax / 2 / shearWave / 100, 2) + Math.Pow(0.177 * RadiusInter * 100 * ODEAmax / (Math.Pow(shearWave * 100, 2)), 2));
            ODEStrain = new double[2];
            MDEStrain = new double[2];
            for(int i = 0; i < 2; i++)
            {
                ODEStrain[i] = verticalStrainCal(ODEVmax, ODEAmax);
                MDEStrain[i] = verticalStrainCal(MDEVmax, MDEAmax);
            }

            ODEbool = StrainDiscriminate(ODEStrain, out List<Tuple<string, string>> ODEoutBool);
            MDEbool = StrainDiscriminate(MDEStrain, out List<Tuple<string, string>> MDEoutBool);

            switch (outputCondition.ToUpper().ToString())
            {
                case "WEBFORM":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            ODEStrain[i] = Math.Round(ODEStrain[i], 5);
                            MDEStrain[i] = Math.Round(MDEStrain[i], 5);
                        }

                        string outPara = $"<div style='Line-Height:{lineHeight}px'> ";
                        outPara += "地震引致之隧道縱向應變計算 <br>";

                        outPara += $" {emsp1()} A、基本設計參數 <br> ";
                        outPara += $" {emsp2()} 土壤中地震剪力波速 Cs = {shearWave} m/sec <br> ";
                        outPara += $" {emsp2()} 隧道環片平均直徑 D = {p.segmentRadiusInter * 2} m <br> ";

                        outPara += $" {emsp2()} 根據CEDC第01版 <br> ";
                        outPara += $" {emsp2()} 一、一般地震力(ODE)狀況 <br> ";
                        outPara += $" {emsp3()} a = {p.ODEg} g <br> ";
                        outPara += $" {emsp3()} Amax = {ODEAmax} cm/sec<sup>2</sup> <br> ";
                        outPara += $" {emsp3()} Vmax = {ODEVmax} cm/sec <br> ";

                        outPara += $" {emsp2()} 二、最大設計地震力(MDE)狀況 <br> ";
                        outPara += $" {emsp3()} a = {p.MDEg} g <br> ";
                        outPara += $" {emsp3()} Amax = {MDEAmax} cm/sec<sup>2</sup> <br> ";
                        outPara += $" {emsp3()} Vmax = {MDEVmax} cm/sec <br> ";

                        outPara += $" {emsp1()} B、隧道縱向應變計算 <br> ";
                        outPara += $" {emsp2()} &nbsp 依 CEDC 附錄 A，公式 II-4 與隧道縱向軸立 45° 角入射之剪力波所造成之隧道縱向應變可以下式計算  <br>";
                        outPara += $"<br> {image("images\\2_11_VerticalStrain.jpg")} <br> ";

                        outPara += $" {emsp2()} 一、一般設計地震力(ODE)狀況 <br> ";
                        //outPara += $"𝝴"
                        outPara += $" {emsp3()} ε<sub>s</sub> = ±{ODEStrain[0]} <br> ";
                        outPara += $" {emsp3()} 壓應變 ε<sub>s</sub> = {ODEStrain[0]} {ODEoutBool[0].Item1} {verticalCompressionStrain} {FontRed(ODEoutBool[0].Item2)} <br> ";
                        outPara += $" {emsp3()} 張應變 ε<sub>s</sub> = -{ODEStrain[1]} {ODEoutBool[1].Item1} {tensionStrain} {FontRed(ODEoutBool[1].Item2)} <br> ";

                        outPara += $" {emsp2()} 二、最大設計地震力(MDE)狀況 <br> ";
                        //outPara += $"𝝴"
                        outPara += $" {emsp3()} ε<sub>s</sub> = ±{MDEStrain[0]} <br> ";
                        outPara += $" {emsp3()} 壓應變 ε<sub>s</sub> = {MDEStrain[0]} {MDEoutBool[0].Item1} {verticalCompressionStrain} {FontRed(MDEoutBool[0].Item2)} <br> ";
                        outPara += $" {emsp3()} 張應變 ε<sub>s</sub> = -{MDEStrain[1]} {MDEoutBool[1].Item1} {tensionStrain} {FontRed(MDEoutBool[1].Item2)} <br> ";

                        outPara += $" <br> ";
                        outPara += $" {emsp1()} 規範規定之最大承壓應變為{verticalCompressionStrain} (CDEC附錄A，VI.B.2) <br> ";
                        outPara += $" {emsp1()} (鋼筋降伏應變：fy/E = {Fy} / {p.steelEs} = {tensionStrain}) <br> ";

                        outPara += $"</div>";
                        outVerticalStain = outPara;
                    }
                    break;
            }

        }
        double verticalStrainCal(double Vmax, double Amax)
        {                                   
            return Math.Sqrt(Math.Pow(Vmax / 2 / shearWave / 100, 2) + Math.Pow(0.177 * p.segmentRadiusInter * 100 * Amax / (Math.Pow(shearWave * 100, 2)), 2));
        }
        #endregion

        #region 地震引致之隧道扭曲應變
        public void TorsionStrainByEQ(out string outTorsionStrainByEQ, out double[] ODEStrain, out double[] MDEStrain)
        {
            outTorsionStrainByEQ = "";
            ODEStrain = new double[2];
            MDEStrain = new double[2];
            /*
            double[] Vmax = new double[2];
            Vmax[0] = ODEVmax;
            Vmax[1] = MDEVmax;
            */
            ODEStrain = rackStrainCal(ODEVmax);
            MDEStrain = rackStrainCal(MDEVmax);

            ODEbool = StrainDiscriminate(ODEStrain, out List<Tuple<string, string>> ODEoutBool);
            MDEbool = StrainDiscriminate(MDEStrain, out List<Tuple<string, string>> MDEoutBool);

            switch (outputCondition.ToUpper().ToString())
            {
                case "WEBFORM":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            ODEStrain[i] = Math.Round(ODEStrain[i], 5);
                            MDEStrain[i] = Math.Round(MDEStrain[i], 5);
                        }

                        soilEm = Math.Round(soilEm, 0);


                        string outPara = $"<div style='Line-Height:{lineHeight}px'> ";
                        outPara += "地震引致之隧道扭曲應變計算 <br> ";

                        outPara += $" {emsp1()} A、基本設計參數 <br> ";
                        outPara += $" {emsp2()} 土壤中地震剪力波速 Cs = {shearWave} m/sec <br> ";
                        outPara += $" {emsp2()} 隧道環片厚度 t = {p.segmentThickness} m <br> ";
                        outPara += $" {emsp2()} 隧道環片平均半徑 R = {p.segmentRadiusIn} m <br> ";
                        outPara += $" {emsp2()} 地層之彈性模數 Em = {soilEm} kN/m<sup>2</sup> <br> ";
                        outPara += $" {emsp2()} 環片之彈性模數 E1 = {segmentE1} kN/m<sup>2</sup> <br> ";

                        outPara += $" {emsp2()} 一、一般設計地震力(ODE)狀況 <br> ";
                        outPara += $" {emsp3()} a = {p.ODEg} g <br> ";
                        outPara += $" {emsp3()} Amax = {ODEAmax} cm/sec<sup>2</sup> <br> ";
                        outPara += $" {emsp3()} Vmax = {ODEVmax} cm/sec <br> ";

                        outPara += $" {emsp2()} 二、最大設計地震力(MDE)狀況 <br> ";
                        outPara += $" {emsp3()} a = {p.MDEg} g <br> ";
                        outPara += $" {emsp3()} Amax = {MDEAmax} cm/sec<sup>2</sup> <br> ";
                        outPara += $" {emsp3()} Vmax = {MDEVmax} cm/sec <br> ";

                        outPara += $" {emsp1()} B、隧道扭曲應變計算 <br> ";
                        outPara += $" {emsp2()} 依CEDC附錄A，公式 VI-2A及2B，剪力波於隧道橫斷面方向所造成之扭曲應變，可以下式計算 <br> ";

                        outPara += $" {emsp2()} 壓應變 <br> ";
                        outPara += $" {emsp3()} {image("images\\2_12_01_RackCompressionStrain.jpg")} <br> ";
                        outPara += $" {emsp2()} 張應變 <br> ";
                        outPara += $" {emsp3()} {image("images\\2_12_02_RackTensionStrain.jpg")} <br> ";

                        outPara += $" {emsp3()} 一、一般設計地震力(ODE)狀況 <br> ";
                        outPara += $" {emsp3()} 壓應變 ε<sub>s</sub> = {ODEStrain[0]} {ODEoutBool[0].Item1} {verticalCompressionStrain} {FontRed(ODEoutBool[0].Item2)} <br> ";
                        outPara += $" {emsp3()} 張應變 ε<sub>s</sub> = {ODEStrain[1]} {ODEoutBool[1].Item1} {tensionStrain} {FontRed(ODEoutBool[1].Item2)} <br> ";

                        outPara += $" {emsp3()} 二、最大設計地震力(MDE)狀況 <br> ";
                        outPara += $" {emsp3()} 壓應變 ε<sub>s</sub> = {MDEStrain[0]} {MDEoutBool[0].Item1} {verticalCompressionStrain} {FontRed(MDEoutBool[0].Item2)} <br> ";
                        outPara += $" {emsp3()} 張應變 ε<sub>s</sub> = {MDEStrain[1]} {MDEoutBool[1].Item1} {tensionStrain} {FontRed(MDEoutBool[1].Item2)} <br> ";

                        outPara += $" <br> ";
                        outPara += $" {emsp1()} 規範規定之最大承壓應變為{verticalCompressionStrain} (CDEC附錄A，VI.B.2) <br> ";
                        outPara += $" {emsp1()} (鋼筋降伏應變：fy/E = {Fy} / {p.steelEs} = {tensionStrain}) <br> ";

                        outPara += $"</div> ";
                        outTorsionStrainByEQ = outPara;
                    }
                    break;
            }

        }

        double[] rackStrainCal(double Vmax)
        {
            soilEm = verticalStress.longTermSoilE;
            double[] strain = new double[2];
            double s01 = Vmax / (100 * shearWave);
            double s02 = 2 * p.segmentThickness / p.segmentRadiusInter;
            double s03 = (3.0 / 8.0);
            double s04 = (soilEm / segmentE1);
            double s05 = (p.segmentRadiusInter / p.segmentThickness);
            strain[0] = s01 * (s02 + s03 * s04 * s05);
            //strain[0] = Vmax / (100 * shearWave) * (((2 * p.segmentThickness) / p.segmentRadiusInter) + ((3 * soilEm * p.segmentRadiusInter) / (8 * segmentE1 * p.segmentThickness)));
            strain[1] = Vmax / 100 / shearWave * 2 * p.segmentThickness / p.segmentRadiusInter;
            return strain;

        }

        bool[] StrainDiscriminate(double[] strain, out List<Tuple<string,string>> strainOutBool)
        {
            strainOutBool = new List<Tuple<string, string>>();
            if (strain[0] < verticalCompressionStrain)
            {
                tempBool[0] = true;
                strainOutBool.Add(Tuple.Create("<", "OK"));
            }
            else
            {
                tempBool[0] = false;
                strainOutBool.Add(Tuple.Create(">", "NG"));
            }
                
            if (strain[1] < tensionStrain)
            {
                tempBool[1] = true;
                strainOutBool.Add(Tuple.Create("<", "OK"));
            }
            else
            {
                tempBool[1] = false;
                strainOutBool.Add(Tuple.Create(">", "NG"));
            }
                
            return tempBool;
        }
        #endregion

        void torsion()
        {
            TorsionStrainByEQAndExcavation(out string ss, out string sss, out string lls, out string fls,
                out List<double> ODEStrain, out List<double> MDEStrain, out List<double> looseODEStrain, out List<double> looseMDEStrain);
        }
        public void F_Dia_C_PushAutoInput()
        {
            torsion();

            double VerticalReduced = Math.Round(Math.Abs(0.067 * Math.Log(flexibilityRatio, Math.E) * (-1) - 0.3197), 2);
            double LateralIncreased = Math.Round(0.0679 * Math.Log(flexibilityRatio, Math.E) + 0.1403, 2);

            double K05UPDN = Math.Round(0.1147 * compressibilityRatio * (-1) + 0.5391, 2);
            double K20UPDN = Math.Round(0.2507 * compressibilityRatio * (-1) + 1.8622, 2);
            double K05TwoSides = Math.Round(0.1314 * compressibilityRatio * (-1) + 0.9231, 2);
            double K20TwoSides = Math.Round(0.2089 * compressibilityRatio * (-1) + 1.0945, 2);

            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "VerticalReduced", VerticalReduced);
            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "LateralIncreased", LateralIncreased);
            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "[K0.5UPDN]", K05UPDN);
            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "[K2.0UPDN]", K20UPDN);
            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "[K0.5TwoSides]", K05TwoSides);
            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "[K2.0TwoSides]", K20TwoSides);
        }

        public void Loose_F_DiaAutoInput()
        {
            torsion();
            double DVariation;
            if (F < 4) DVariation = 1.32;
            else DVariation = Math.Round(2.279 * Math.Log(F, Math.E) - 1.8136, 2);

            oExecuteSQL.UpdateData("STN_EQProp", "Section", sectionUID, "DVariation", DVariation);
        }

        #region 開挖荷重及地震扭曲引致之應變
        double flexibilityRatio;
        double compressibilityRatio;
        double F;
        public void TorsionStrainByEQAndExcavation(out string outTorsionByEQAndExcavation, out string outLooseTosionByEqAndExcavation, out string outFlexibilityRatio, out string outCompressibilityRatio, out List<double> ODEStrain, out List<double> MDEStrain, out List<double> looseODEStrain, out List<double> looseMDEStrain)
        {
            outTorsionByEQAndExcavation = "";
            outLooseTosionByEqAndExcavation = "";
            outFlexibilityRatio = "";
            outCompressibilityRatio = "";
            double Pv = verticalStress.PvTop;

            double longTermPh1 = verticalStress.Ph1[0];
            double longtermPh2 = verticalStress.Ph2[0];

            double shortTermPh1 = verticalStress.Ph1[1];
            double shortTermPh2 = verticalStress.Ph2[1];
            /*
            double longtermPh1 = verticalStress.Ph1[0];
            double shortermPh1 = verticalStress.Ph1[1];
            double longtermPh2 = verticalStress.Ph2[0];
            double shortermPh2 = verticalStress.Ph2[1];
            */
            

            double[] Phavg = new double[2];
            double[] K = new double[2];
            double[] Pv1K = new double[2];

            for (int i = 0; i < 2; i++)
            {
                Phavg[i] = (verticalStress.Ph1[i] + verticalStress.Ph2[i]) / 2;
                K[i] = Phavg[i] / Pv;
                Pv1K[i] = Pv * (1 - K[i]);
            }

            double longTermPhavg = Phavg[0];
            double shortTermPhavg = Phavg[1];

            double longTermK = K[0];
            double shortTermK = K[1];

            double longTermPv1K = Pv1K[0];
            double shortTermPv1K = Pv1K[1];

            double chosenK = 0;
            string chosenCondition = "";
            if (Pv1K[0] > Pv1K[1])
            {
                soilEm = verticalStress.longTermSoilE;
                chosenK = K[0];
                chosenCondition = "長期荷重";
            }
            else
            {
                soilEm = verticalStress.shortTermSoilE;
                chosenK = K[1];
                chosenCondition = "短期荷重";
            }

            double momentOfInertia = p.segmentWidth * Math.Pow(p.segmentThickness, 3) / 12;
            double reductionK = 1.0;
            double ss = (soilEm / (1 + soilU12));
            double kk = 6 * segmentE1 * momentOfInertia * reductionK;
            double mm = 1 - Math.Pow(segmentU12, 2);
            double rr = Math.Pow(p.segmentRadiusInter, 3);
            flexibilityRatio = ss / (kk / (mm * rr));


            double verticalReduced = p.verticalReduced;
            double lateralIncreased = p.lateralIncreased;
            double K05 = p.soilK05;

            double verticalValue = verticalReduced * (1 - chosenK) / (1 - K05) * (Pv / soilEm);
            double lateralValue = lateralIncreased * (1 - chosenK) / (1 - K05) * (Pv / soilEm);

            double verticalStrain = (3 * p.segmentThickness * verticalValue) / (2 * p.segmentRadiusInter);
            double lateralStrain = (3 * p.segmentThickness * lateralValue) / (2 * p.segmentRadiusInter);


            compressibilityRatio = (soilEm / segmentE1) * (p.segmentRadiusInter / p.segmentThickness) * ((1 - Math.Pow(segmentU12, 2)) / ((1 + soilU12) * (1 - 2 * soilU12)));

            double TUPDN = Pv * p.segmentRadiusInter * (p.k05UPDN + (p.k20UPDN - p.k05UPDN) * ((chosenK - p.soilK05) / (p.soilK20 - p.soilK05)));
            double strainUPDN = TUPDN / (segmentE1 * p.segmentThickness);

            double TTwoSides = Pv * p.segmentRadiusInter * (p.k05TwoSides + (p.k20TwoSides - p.k05TwoSides) * ((chosenK - p.soilK05) / (p.soilK20 - p.soilK05)));
            double strainTwoSides = TTwoSides / (segmentE1 * p.segmentThickness);

            TorsionStrainByEQ(out string data, out double[] torsionODEStrain, out double[] torsionMDEStrain);

            double MDEUPmaxCompression = Math.Round(torsionMDEStrain[0] + verticalStrain + strainUPDN, 6);
            double MDEUPmaxTension = Math.Round(torsionMDEStrain[1] + verticalStrain - strainUPDN, 6);
            double MDECentermaxCompression = Math.Round(torsionMDEStrain[0] + lateralStrain + strainTwoSides, 6);
            double MDECentermaxTension = Math.Round(torsionMDEStrain[1] + lateralStrain - strainTwoSides, 6);

            MDEStrain = new List<double> { MDEUPmaxCompression, MDEUPmaxTension, MDECentermaxCompression, MDECentermaxTension };

            double ODEUPmaxCompression = Math.Round(torsionODEStrain[0] + verticalStrain + strainUPDN, 6);
            double ODEUPmaxTension = Math.Round(torsionODEStrain[1] + verticalStrain - strainUPDN, 6);
            double ODECentermaxCompression = Math.Round(torsionODEStrain[0] + lateralStrain + strainTwoSides, 6);
            double ODECentermaxTension = Math.Round(torsionODEStrain[1] + lateralStrain - strainTwoSides, 6);

            ODEStrain = new List<double> { ODEUPmaxCompression, ODEUPmaxTension, ODECentermaxCompression, ODECentermaxTension };

            List<Tuple<string, string>> ODECondition = new List<Tuple<string, string>>();
            List<Tuple<string, string>> MDECondition = new List<Tuple<string, string>>();

            //double compressionStrain = 0.03;
            //double tensionStrain = 0.0021;

            for(int i = 0; i < ODEStrain.Count; i++)
            {
                if(i == 0 || i == 2)
                {
                    if (ODEStrain[i] < FlexuralCompressionStrain) ODECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));

                    if (MDEStrain[i] < FlexuralCompressionStrain) MDECondition.Add(Tuple.Create("<", "OK"));
                    else MDECondition.Add(Tuple.Create(">", "NG"));
                }
                else
                {
                    if (ODEStrain[i] < tensionStrain) ODECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));

                    if (MDEStrain[i] < tensionStrain) MDECondition.Add(Tuple.Create("<", "OK"));
                    else MDECondition.Add(Tuple.Create(">", "NG"));
                }
            }

            switch (outputCondition.ToUpper().ToString())
            {
                case "WEBFORM":
                    {
                        soilEm = Math.Round(soilEm, 0);
                        Pv = Math.Round(Pv, 2);
                        longTermPh1 = Math.Round(longTermPh1, 2);
                        longtermPh2 = Math.Round(longtermPh2, 2);
                        longTermK = Math.Round(longTermK, 6);
                        longTermPv1K = Math.Round(longTermPv1K, 2);
                        longTermPhavg = Math.Round(longTermPhavg, 2);

                        shortTermPh1 = Math.Round(shortTermPh1, 2);
                        shortTermPh2 = Math.Round(shortTermPh2, 2);
                        shortTermK = Math.Round(shortTermK, 6);
                        shortTermPv1K = Math.Round(shortTermPv1K, 2);
                        shortTermPhavg = Math.Round(shortTermPhavg, 2);

                        verticalStrain = Math.Round(verticalStrain, 6);
                        lateralStrain = Math.Round(lateralStrain, 6);
                        strainUPDN = Math.Round(strainUPDN, 6);
                        strainTwoSides = Math.Round(strainTwoSides, 6);

                        string outdiv = $"<div style='Line-Height:{lineHeight}px'> ";
                        string outPara = "";
                        outPara += outdiv;

                        outPara += "開挖荷重及地震扭曲引致之應變計算 <br> ";                        

                        outPara += $" &emsp; 一、開挖荷重引致之應變 <br> ";
                        outPara += $" &emsp; &nbsp A、分析斷面與荷重條件 <br> ";
                        outPara += $" &emsp; &emsp; 分析斷面： &emsp; 地層荷重 <br> ";

                        outPara += $" &emsp; &emsp; (1)長期荷重狀況 <br> ";
                        outPara += $" {emsp3()} Pv = {Pv} kN/m² &emsp; Ph1 = {longTermPh1} kN/m² <br> ";
                        outPara += $" {emsp3()} Phavg = {longTermPhavg} kN/m² &emsp; Ph2 = {longtermPh2} kN/m² <br> ";
                        outPara += $" {emsp3()} K = Phavg/Pv = {longTermK} &emsp; Pv(1-K) = {longTermPv1K} kN/m² <br> ";

                        outPara += $" &emsp; &emsp; (2)短期荷重狀況 <br> ";
                        outPara += $" {emsp3()} Pv = {Pv} kN/m² &emsp; Ph1 = {shortTermPh1} kN/m² <br> ";
                        outPara += $" {emsp3()} Phavg = {shortTermPhavg} kN/m² &emsp; Ph2 = {shortTermPh2} kN/m² <br> ";
                        outPara += $" {emsp3()} K = Phavg/Pv = {shortTermK} &emsp; Pv(1-K) = {shortTermPv1K} kN/m² <br> ";

                        outPara += $"<br>";

                        outPara += $" &emsp; 由以上的荷重條件 Pv(1-K)，隧道因開挖荷重引致之應變由{chosenCondition}控制 <br> ";
                                                
                        outPara += $" &emsp; &nbsp B、開挖荷重應變分析 <br> ";

                        ///<summary>
                        ///柔性比計算，另外拉出字串做為查圖用                        
                        string outFlexural = "";
                        outFlexural += $" {emsp2()} (1)柔性比 Flexibility Ratio <br> ";
                        outFlexural += $" {emsp3()} 地層之彈性係數 Em = {soilEm} kN/m² <br> ";
                        outFlexural += $" {emsp3()} 環片之彈性係數 E1 = {segmentE1} kN/m² <br> ";
                        outFlexural += $" {emsp3()} 地層之柏松比 υ = {Math.Round(soilU12, 2)} <br> ";
                        outFlexural += $" {emsp3()} 環片之柏松比 υ₁ = {segmentU12} <br> ";
                        outFlexural += $" {emsp3()} 單位長度環片慣性矩 I₁ = 1x{p.segmentThickness}³/12 = {Math.Round(momentOfInertia, 4)} m⁴/m <br> ";
                        outFlexural += $" {emsp3()} 隧道平均半徑 R = {p.segmentRadiusInter} m <br> ";
                        outFlexural += $" {emsp3()} 環片慣性矩折減係數 Ki = 1 <br> ";
                        outFlexural += $" {emsp3()} 隧道厚度 t = {p.segmentThickness} m <br> ";

                        outFlexural += $" <br> ";

                        outFlexural += $" {emsp2()} {image("images\\2_13_01_FlexibilityRatio.jpg")} <br>";
                        outFlexural += $" {emsp3()} {emsp2()} <font size='4' color='red'> = {Math.Round(flexibilityRatio, 2)} </font> <br> ";
                                                
                        //outPara += $" {emsp3()} 柔性比(F) = [Em x (1 + υ)] / [(6 x E1 x I₁ x Ki)/(1 - υ₁²) * (1/R³)] = {Math.Round(flexibilityRatio, 2)} <br> ";
                        outFlexural += $" {emsp3()} (參CEDC附錄A、VI B.5)：靜止土壓係數 K₀ = 0.5 <br> ";
                        outFlexural += $" {emsp3()} 查圖可得直徑改變量係數(Diameter Change Coefficient)與直徑改變量(ΔD/D)/(P/Em) <br> ";
                        
                        outPara += outFlexural;
                        outFlexibilityRatio = outdiv + outFlexural + "</div>";
                        /// </summary>

                        outPara += $" {emsp3()} 1.隧道冠頂垂直減少量 <br> ";
                        outPara += $" {emsp4()} (ΔD/D)<sub>v</sub>/(γH/Em) = {verticalReduced} <br> ";
                        outPara += $" {emsp4()} (ΔD/D)<sub>v</sub> = {verticalReduced}x(1-K)/(1-K₀)x(γH/Em) = {Math.Round(verticalValue, 6)} <br> ";
                        outPara += $" {emsp3()} 2.隧道起拱線水平量 <br> ";
                        outPara += $" {emsp4()} (ΔD/D)<sub>v</sub>/(γH/Em) = {lateralIncreased} <br> ";
                        outPara += $" {emsp4()} (ΔD/D)<sub>v</sub> = {lateralIncreased}x(1-K)/(1-K₀)x(γH/Em) = {Math.Round(lateralValue, 6)} <br> ";

                        outPara += $" {emsp3()} 由CEDC附錄A、VI-2C及2D公式 <br> ";
                        outPara += $" {emsp2()} {image("images\\2_13_02_Strain_VI_5.jpg")} <br> ";
                        outPara += $" {emsp3()} 1.隧道頂拱垂直應變 ε = {FontRed(Math.Round(verticalStrain, 6).ToString())} <br> ";
                        outPara += $" {emsp3()} 2.隧道中心線水平應變 ε = {FontRed(Math.Round(lateralStrain, 6).ToString())} <br> ";

                        outPara += $" <br> ";

                        ///<summary>
                        ///壓縮比計算，另外拉出一個字串做為查圖用                        
                        string outCompressibility = "";
                        outCompressibility += $" {emsp2()} (2)壓縮比 Compressibility Ratio <br> ";
                        outCompressibility += $" {emsp3()} 採用Ranken的公式及圖表 <br> ";
                        outCompressibility += $" {emsp2()} {image("images\\2_13_03_CompressibilityRatio.jpg")} <br> ";
                        outCompressibility += $" {emsp3()} &emsp; <font size='4' color='red'> = {Math.Round(compressibilityRatio, 6)} </font> <br> ";

                        outPara += outCompressibility;

                        outCompressibility += $" {emsp2()} 查圖可得 隧道冠頂及仰拱&隧道兩側中心線處K₀為2及0.5時之推力係數[Thrust Coefficient, T/(γHa)]";

                        outCompressibilityRatio = outdiv + outCompressibility + "</div>";
                        ///</summary>

                        outPara += $" {emsp3()} 1.隧道冠頂及仰拱 <br> ";
                        outPara += $" {emsp3()} &emsp; T/(γHa)1 = {p.k05UPDN} &emsp; K₀₁ = 0.5 <br> ";
                        outPara += $" {emsp3()} &emsp; T/(γHa)2 = {p.k20UPDN} &emsp; K₀₂ = 2.0 <br> ";
                        outPara += $" {emsp3()} T = Pv * R * (T/(γHa)1 + [T/(γHa)2 - T/(γHa)1] * (K - K₀₁)/(K₀₂ - K₀₁)) <br> ";
                        outPara += $" {emsp3()} &ensp; = {strainUPDN} kN <br> ";
                        outPara += $" {emsp3()} ε = T/(A * E1) = {FontRed(strainUPDN.ToString())} <br> ";                        

                        outPara += $" <br>";

                        outPara += $" {emsp3()} 2.隧道兩側中心線處 <br>";
                        outPara += $" {emsp3()} &emsp; T/(γHa)1 = {p.k05TwoSides} &emsp; K₀₁ = 0.5 <br> ";
                        outPara += $" {emsp3()} &emsp; T/(γHa)2 = {p.k20TwoSides} &emsp; K₀₂ = 2.0 <br> ";
                        outPara += $" {emsp3()} T = Pv * R * (T/(γHa)1 + [T/(γHa)2 - T/(γHa)1] * (K - K₀₁)/(K₀₂ - K₀₁)) <br> ";
                        outPara += $" {emsp3()} &ensp; = {strainTwoSides} kN <br> ";
                        outPara += $" {emsp3()} ε = T/(A * E1) = {FontRed(strainTwoSides.ToString())} <br> ";

                        outPara += $" <br> ";                       

                        outPara += $" 開挖引致之隧道應變可疊加如下所示 <br> ";
                        outPara += $" C、地震扭曲應變(ODE)與開挖應變之和 <br> ";
                        outPara += $" &emsp; 1.冠頂處最大壓應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionODEStrain[0]} + {verticalStrain} + {strainUPDN} = {ODEUPmaxCompression} {ODECondition[0].Item1} {FlexuralCompressionStrain} {FontRed(ODECondition[0].Item2)} <br> ";
                        outPara += $" &emsp; 2.冠頂處最大張應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionODEStrain[1]} + {verticalStrain} - {strainUPDN} = {ODEUPmaxTension} {ODECondition[1].Item1} {tensionStrain} {FontRed(ODECondition[1].Item2)} <br> ";
                        outPara += $" &emsp; 3.中心線處最大壓應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionODEStrain[0]} + {lateralStrain} + {strainTwoSides} = {ODECentermaxCompression} {ODECondition[2].Item1} {FlexuralCompressionStrain} {FontRed(ODECondition[2].Item2)} <br> ";
                        outPara += $" &emsp; 4.中心線處最大張應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionODEStrain[1]} + {lateralStrain} - {strainTwoSides} = {ODECentermaxTension} {ODECondition[3].Item1} {tensionStrain} {FontRed(ODECondition[3].Item2)} <br> ";

                        outPara += $" <br> ";

                        outPara += $" D、地震扭曲應變(MDE)與開挖應變之和 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionMDEStrain[0]} + {verticalStrain} + {strainUPDN} = {MDEUPmaxCompression} {MDECondition[0].Item1} {FlexuralCompressionStrain} {FontRed(MDECondition[0].Item2)} <br> ";
                        outPara += $" &emsp; 2.冠頂處最大張應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionMDEStrain[1]} + {verticalStrain} - {strainUPDN} = {MDEUPmaxTension} {MDECondition[1].Item1} {tensionStrain} {FontRed(MDECondition[1].Item2)} <br> ";
                        outPara += $" &emsp; 3.中心線處最大壓應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionMDEStrain[0]} + {lateralStrain} + {strainTwoSides} = {MDECentermaxCompression} {MDECondition[2].Item1} {FlexuralCompressionStrain} {FontRed(MDECondition[2].Item2)} <br> ";
                        outPara += $" &emsp; 4.中心線處最大張應變 <br> ";
                        outPara += $" &emsp; &emsp; ε = {torsionMDEStrain[1]} + {lateralStrain} - {strainTwoSides} = {MDECentermaxTension} {MDECondition[3].Item1} {tensionStrain} {FontRed(MDECondition[3].Item2)} <br> ";

                        outPara += $" <br> ";
                        outPara += $" {emsp1()} 規範規定之最大承壓應變為{FlexuralCompressionStrain} (CDEC附錄A，VI.B.5) <br> ";
                        outPara += $" {emsp1()} (鋼筋降伏應變：fy/E = {Fy} / {p.steelEs} = {tensionStrain}) <br> ";

                        outPara += $"</div>";

                        outTorsionByEQAndExcavation = outPara;
                    }
                    break;
            }

            //鬆動區荷重引致之應變及地震扭曲應變計算
            double n = (p.steelEs * 100) / segmentE1;

            double SGWidth = p.segmentWidth;
            double SGTH = p.segmentThickness;

            double As = p.SGMainSteel.Area;
            double density = As / (SGWidth * 100 * SGTH * 100);
            
            double n_eqDensity = density * n;

            double d = SGTH - 0.06;
            double k = Math.Pow((2 * n_eqDensity - Math.Pow(n_eqDensity, 2)), 0.5) - n_eqDensity;

            double I1 = SGWidth * (Math.Pow(k * d, 3) / 3) + n * (density * (SGTH * SGWidth)) * Math.Pow(1 - k, 2) * d * d;
            F = (soilEm / segmentE1) * (Math.Pow(p.segmentRadiusInter, 3) / (6 * I1)) * ((1 - Math.Pow(segmentU12, 2)) / (1 + soilU12));

            double P = (Math.Pow(2, 0.5) - (Math.PI / 4)) * Math.Pow(p.segmentRadiusInter, 2) * (verticalStress.soilavgUW / (p.segmentRadiusInter * Math.Pow(2,0.5)));
            double deltaDOverD = p.DVariation * P / soilEm;
            double t = k * d;

            double topStrain = 3 * deltaDOverD * t / (2 * p.segmentRadiusInter);
            double bottomStrain = -3 * deltaDOverD * ((1 - k) * d / (2 * p.segmentRadiusInter));

            double ODETopStrain = topStrain * (1 + ODEg);
            double ODEBottomStrain = bottomStrain * (1 + ODEg);

            double MDETopStrain = topStrain * (1 + MDEg);
            double MDEBottomStrain = bottomStrain * (1 + MDEg);

            

            double maxODEStrain = Math.Round(ODETopStrain + torsionODEStrain[0], 6);
            double minODEStrain = Math.Round(Math.Abs(ODEBottomStrain) + torsionODEStrain[1], 6);

            looseODEStrain = new List<double> { maxODEStrain, minODEStrain };

            double maxMDEStrain = Math.Round(MDETopStrain + torsionMDEStrain[0], 6);
            double minMDEStrain = Math.Round(Math.Abs(MDEBottomStrain) + torsionMDEStrain[1], 6);

            looseMDEStrain = new List<double> { maxMDEStrain, minMDEStrain };

            ODECondition.Clear();
            MDECondition.Clear();

            for(int i = 0; i < looseODEStrain.Count; i++)
            {
                if(i == 0)
                {
                    if (looseODEStrain[i] < FlexuralCompressionStrain) ODECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));

                    if (looseMDEStrain[i] < FlexuralCompressionStrain) MDECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));
                }
                else
                {
                    if (looseODEStrain[i] < tensionStrain) ODECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));

                    if (looseMDEStrain[i] < tensionStrain) MDECondition.Add(Tuple.Create("<", "OK"));
                    else MDECondition.Add(Tuple.Create(">", "NG"));
                }
            }

            switch (outputCondition.ToUpper().ToString())
            {
                case "WEBFORM":
                    {
                        soilEm = Math.Round(soilEm, 0);
                        n = Math.Round(n, 4);
                        n_eqDensity = Math.Round(n_eqDensity, 4);
                        k = Math.Round(k, 3);
                        I1 = Math.Round(I1, 7);
                        F = Math.Round(F, 2);
                        deltaDOverD = Math.Round(deltaDOverD, 6);
                        t = Math.Round(t, 5);
                        topStrain = Math.Round(topStrain, 6);
                        bottomStrain = Math.Round(bottomStrain, 6);
                        ODETopStrain = Math.Round(ODETopStrain, 6);
                        ODEBottomStrain = Math.Round(ODEBottomStrain, 6);
                        MDETopStrain = Math.Round(MDETopStrain, 6);
                        MDEBottomStrain = Math.Round(MDEBottomStrain, 6);
                        string outPara = $"<div style='Line-Height:{lineHeight}px'> ";
                        outPara += "鬆動區荷重引致之應變及地震扭曲應變計算 <br> ";

                        outPara += $"A、鬆動區荷重引致之應變 <br> ";
                        ///<summary>
                        ///鬆動區荷重之柔性比計算與前述不同，I值計算有差，此處之I有考量鋼筋影響，但為何前述I與這不同仍未知


                        outLooseFRatio += $" &emsp; 土層單位重 γ = {Math.Round(verticalStress.soilavgUW, 2)} kN/m³ <br> ";
                        outLooseFRatio += $" &emsp; 地層之彈性係數 Em = {soilEm} kN/m² <br> ";
                        outLooseFRatio += $" &emsp; 環片之彈性係數 E1 = {segmentE1} kN/m² <br> ";
                        outLooseFRatio += $" &emsp; 地層之柏松比 ν = {Math.Round(soilU12, 2)} <br> ";
                        outLooseFRatio += $" &emsp; 環片之柏松比 ν₁ = {segmentU12} <br> ";
                        outLooseFRatio += $" &emsp; 隧道平均半徑 R = {p.segmentRadiusInter} m <br> ";
                        outLooseFRatio += $" &emsp; 隧道厚度 t = {p.segmentThickness} m <br> ";
                        outLooseFRatio += $" <br> ";
                        outLooseFRatio += $" &emsp; n = Es/E1 = {p.steelEs*100}/{segmentE1} = {n} <br> ";
                        outLooseFRatio += $" &emsp; ρ = As/(b*h) = {As}/({SGWidth*100}*{SGTH*100}) = {density} &emsp; nρ = {n_eqDensity} <br> ";
                        outLooseFRatio += $" &emsp; d = {p.segmentThickness - 0.06} m &emsp; h = {p.segmentThickness} <br> ";
                        outLooseFRatio += $" &emsp; k = √[2nρ - (nρ)²] - nρ = {k} <br> ";
                        outLooseFRatio += $" &emsp; I₁ = b * (kd)³ / 3 + n * As * (1-k)² * d² = {I1} <br> ";
                        outLooseFRatio += $" &emsp; F(柔性比) = (Em/E1)*(R³/6I₁)*(1-ν₁²)/(1+ν) <font size='4' color='red'> = {F} </font> <br> ";

                        outPara += outLooseFRatio;
                        ///</summary>

                        outPara += $" &emsp; 由圖可查得(∆D/D)(P/Em) = {p.DVariation} <br> ";
                        outPara += $" &emsp; P = (√2 - π/4)*R²*γ/R/√2 = {Math.Round(P, 2)} kN/m² <br> ";
                        outPara += $" &emsp; (∆D/D) = {deltaDOverD} <br> ";
                        outPara += $" &emsp; t = k*d = {t} <br> ";
                        outPara += $" <br> ";
                        outPara += $" &emsp; 鬆動區荷重引致之應變可由下列式子求得 <br> ";
                        outPara += $" &emsp; εtop = 3*(∆D/D)*(t/R/2) = {topStrain} <br> ";
                        outPara += $" &emsp; εbottom = -3*(∆D/D)*[(1-k)*d/R/2] = {bottomStrain} <br> ";

                        outPara += $" <br> ";

                        outPara += $"B、鬆動區荷重與地震扭曲應變之和 <br> ";
                        outPara += $" &emsp; 1.ODEa<sub>max</sub> = {p.ODEg} g <br> ";
                        outPara += $" &emsp; &nbsp εtop = (1+{p.ODEg})*{topStrain} = {ODETopStrain} <br> ";
                        outPara += $" &emsp; &nbsp εbottom = (1+{p.ODEg})*{bottomStrain} = {ODEBottomStrain} <br> ";
                        outPara += $" <br> ";
                        outPara += $" &emsp; 2.MDEa<sub>max</sub> = {p.MDEg} g <br> ";
                        outPara += $" &emsp; &nbsp εtop = (1+{p.MDEah})*{topStrain} = {MDETopStrain} <br> ";
                        outPara += $" &emsp; &nbsp εbottom = (1+{p.MDEah})*{bottomStrain} = {MDEBottomStrain} <br> ";
                        outPara += $" <br> ";
                        outPara += $" &emsp; 應變和 = 鬆動區荷重應變 + 地震扭曲應變 <br> ";
                        outPara += $" &emsp; ODE <br> ";
                        outPara += $" &emsp; &nbsp εmax = {ODETopStrain} + {torsionODEStrain[0]} = {maxODEStrain} {ODECondition[0].Item1} {FlexuralCompressionStrain} {FontRed(ODECondition[0].Item2)} <br> ";
                        outPara += $" &emsp; &nbsp εmin = {Math.Abs(ODEBottomStrain)} + {torsionODEStrain[1]} = {minODEStrain} {ODECondition[1].Item1} {tensionStrain} {FontRed(ODECondition[1].Item2)} <br> ";
                        outPara += $" &emsp; MDE <br> ";
                        outPara += $" &emsp; &nbsp εmax = {MDETopStrain} + {torsionMDEStrain[0]} = {maxMDEStrain} {MDECondition[0].Item1} {FlexuralCompressionStrain} {FontRed(MDECondition[0].Item2)} <br> ";
                        outPara += $" &emsp; &nbsp εmin = {Math.Abs(MDEBottomStrain)} + {torsionMDEStrain[1]} = {minMDEStrain} {MDECondition[1].Item1} {tensionStrain} {FontRed(MDECondition[1].Item2)}  <br> ";

                        outPara += $" <br> ";
                        outPara += $" {emsp1()} 規範規定之最大承壓應變為{FlexuralCompressionStrain} (CDEC附錄A，VI.B.5) <br> ";
                        outPara += $" {emsp1()} (鋼筋降伏應變：fy/E = {Fy} / {p.steelEs} = {tensionStrain}) <br> ";

                        outPara += $" </div> ";

                        outLooseTosionByEqAndExcavation = outPara;
                    }
                    break;
            }

        }

        #endregion

        #region 隧道非正圓容許應變及地震扭曲應變計算
        public void StrainWithNotCircle(out string outString, out List<double> ODEStrain, out List<double> MDEStrain)
        {
            outString = "";
            double CEDMA6B7 = 0.005;

            double strain = 3 * (p.segmentThickness / (2 * p.segmentRadiusInter)) * CEDMA6B7;

            TorsionStrainByEQ(out string data, out double[] torsionODEStrain, out double[] torsionMDEStrain);

            double maxODEStrain = Math.Round(strain + torsionODEStrain[0], 6);
            double minODEStrain = Math.Round(strain + torsionODEStrain[1], 6);

            ODEStrain = new List<double> { maxODEStrain, minODEStrain };

            double maxMDEStrain = Math.Round(strain + torsionMDEStrain[0], 6);
            double minMDEStrain = Math.Round(strain + torsionMDEStrain[1], 6);

            MDEStrain = new List<double> { maxMDEStrain, minMDEStrain };

            List<Tuple<string, string>> ODECondition = new List<Tuple<string, string>>();
            List<Tuple<string, string>> MDECondition = new List<Tuple<string, string>>();

            for(int i = 0; i < ODEStrain.Count; i++)
            {
                if(i == 0)
                {
                    if (maxODEStrain < FlexuralCompressionStrain) ODECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));

                    if (maxMDEStrain < FlexuralCompressionStrain) MDECondition.Add(Tuple.Create("<", "OK"));
                    else MDECondition.Add(Tuple.Create(">", "NG"));
                }
                else
                {
                    if (minODEStrain < FlexuralCompressionStrain) ODECondition.Add(Tuple.Create("<", "OK"));
                    else ODECondition.Add(Tuple.Create(">", "NG"));

                    if (minMDEStrain < FlexuralCompressionStrain) MDECondition.Add(Tuple.Create("<", "OK"));
                    else MDECondition.Add(Tuple.Create(">", "NG"));
                }
            }

            switch (outputCondition.ToUpper().ToString())
            {
                case "WEBFORM":
                    {
                        strain = Math.Round(strain, 6);

                        string outPara = $"<div style='Line-Height:{lineHeight}px'> ";
                        outPara += $"隧道非正圓容許應變及地震扭曲應變計算 <br> ";
                        outPara += $" &emsp; A、隧道容許變化量 <br> ";
                        outPara += $" &emsp; &nbsp 隧道平均半徑 R = {p.segmentRadiusInter} m <br> ";
                        outPara += $" &emsp; &nbsp 隧道環片厚度 t = {p.segmentThickness} m <br> ";
                        outPara += $" &emsp; 非正圓之容許誤差需小於{CEDMA6B7}D(取自CEDC附錄A VI B.7) <br> ";
                        outPara += $" &emsp; (∆D/D) = {CEDMA6B7} <br> ";
                        outPara += $" &emsp; ε = ±3*(t/2/R)*(∆D/D) = ±{strain} <br> ";

                        outPara += $"<br>";

                        outPara += $" &emsp; B、隧道非正圓容許應變與地震扭曲應變之和 <br> ";
                        outPara += $" &emsp; 應變和 = 隧道非正圓容許應變 + 地震扭曲應變 <br> ";
                        outPara += $" &emsp; ODE <br> ";
                        outPara += $" &emsp; &nbsp εmax = {strain} + {torsionODEStrain[0]} = {maxODEStrain} {ODECondition[0].Item1} {FlexuralCompressionStrain} {FontRed(ODECondition[0].Item2)} <br> ";
                        outPara += $" &emsp; &nbsp εmin = {strain} + {torsionODEStrain[1]} = {minODEStrain} {ODECondition[1].Item1} {tensionStrain} {FontRed(ODECondition[1].Item2)} <br> ";
                        outPara += $" &emsp; MDE <br> ";
                        outPara += $" &emsp; &nbsp εmax = {strain} + {torsionMDEStrain[0]} = {maxMDEStrain} {MDECondition[0].Item1} {FlexuralCompressionStrain} {FontRed(MDECondition[0].Item2)} <br> ";
                        outPara += $" &emsp; &nbsp εmin = {strain} + {torsionMDEStrain[1]} = {minMDEStrain} {MDECondition[1].Item1} {tensionStrain} {FontRed(MDECondition[1].Item2)} <br> ";

                        outPara += $" <br> ";
                        outPara += $" {emsp1()} 規範規定之最大承壓應變為{FlexuralCompressionStrain} (CDEC附錄A，VI.B.5) <br> ";
                        outPara += $" {emsp1()} (鋼筋降伏應變：fy/E = {Fy} / {p.steelEs} = {tensionStrain}) <br> ";

                        outString = outPara;

                        outPara += $"</div>";
                    }
                    break;
            }

                
        }

        public string TwoColSTR(string str1, string str2)
        {
            string output = $"<tr> <th> {str1} </th> <th> {str2} </th> ";
            return output;
        }

        #endregion  

        public string FontRed(string str)
        {
            return $"<font color='red'>{str}</font>";
        }

        public string emsp4()
        {
            return "&emsp; &emsp; &emsp; &emsp;";
        }

        public string emsp3()
        {
            return "&emsp; &emsp; &emsp;";
        }

        public string emsp2()
        {
            return "&emsp; &emsp;";
        }

        public string emsp1()
        {
            return "&emsp;";
        }

        public string image(string str)
        {
            return $"<img src='{str}'></img> ";
        }


        


    }
}
