using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SinoTunnel
{
    class DuctileCastIron
    {
        public double Rout;  // m
        public double width;
        public double UW; //kN/m²
        public double t1; // cm
        public double t2; // cm
        public double t3; // cm
        public double t; // cm
        public double theta;
        public double Fy; // kg/cm²
        public double E; // kN/m²
        public double Fat; // kg/cm² - 容許抗彎拉應力
        public double Fac; // kg/cm² - 容許抗彎壓應力
        public double Fas; // kg/cm² - 容許抗剪應力
    }

    public class GeneralCalculation
    {
        string sectionUID;
        GetWebData p;
        DuctileCastIron sg = new DuctileCastIron();
        STN_VerticalStress oSTN_VerticalStress;
        ExcuteSQL oExecuteSQL = new ExcuteSQL();

        public GeneralCalculation(string sectionUID, string condition)
        {
            this.sectionUID = sectionUID;
            this.p = new GetWebData(sectionUID);
            this.oSTN_VerticalStress = new STN_VerticalStress(sectionUID, "WEBFORM");
            sg.Rout = p.segmentRadiusOut; // m
            sg.width = p.segmentWidth * 100; // cm
            sg.UW = 71.05; //kN/m²
            sg.t1 = 2.8; // cm
            sg.t2 = 2.8; // cm
            sg.t3 = 2.8; // cm
            sg.t = 0.2 * 100; // cm
            sg.theta = 7.2; // theta
            sg.Fy = 2500; // kg/cm²
            sg.E = 1.7E8; // kN/m²
            sg.Fat = 1900; // kg/cm²
            sg.Fac = 2200; // kg/cm²
            sg.Fas = 1300; // kg/cm²
        }

        double beta;

        double Facebeff;
        double FaceAeff;
        double Faceybar;
        double Faceinertia;

        double eta = 0.83; // η
        double xi = 0.3; // ξ

        double Ks;
        double delta; // δ
        double Pv;
        double Ph1;
        double Ph2;
        double Ph4;
        double Pv5;
        double g;

        double jackP;
        int jackNum;

        double poreLatSpacing;
        double poreVerSpacing;
        int poreNum;
        public void Process(out string str)
        {
            //BetaAutoCatch();

            oSTN_VerticalStress.VerticalStress("TUNNEL", out string lt, out string st, out string surch, out double lE1,
                out double sE1, out double pt, out double lph1, out double lph2, out double sph1, out double sph2, out double uu);
            

            Props();
            delta = 0.007416;
            Pv = 344.52;
            Ph1 = 230.24;
            Ph2 = 316.66;
            Ph4 = 120.48;
            Calculation();

            // beta 查表參數
            SGCheck();

            jackP = 150; // t
            jackNum = 22;
            SGPush();

            poreLatSpacing = 0.25; // m
            poreVerSpacing = 0.075; // m
            poreNum = 4;
            SGPore();

            OutRound();
            string propStr = ExportSGProps();
            str = "";
            str += propStr;

            string tableStr = ExportSGCalculation();
            str += tableStr;

            string checkSTR = ExportSGCheck();
            str += checkSTR;

            string pushSTR = ExportSGPush();
            str += pushSTR;

            string poreSTR = ExportPoreCheck();
            str += poreSTR;
        }

        #region Props
        public void Props()
        {
            Facebeff = 25 * sg.t1;
            double B = sg.width;
            if (Facebeff > B) Facebeff = B;

            FaceAeff = Facebeff * sg.t1 + (sg.t - sg.t1) * sg.t2 * 2;
            Faceybar = (Facebeff * sg.t1 * (sg.t - (sg.t1 / 2)) + (sg.t - sg.t1) * sg.t2 * ((sg.t - sg.t1) / 2) * 2) / FaceAeff;
            Faceinertia = Facebeff * Math.Pow(sg.t1, 3) / 12 +
                Facebeff * sg.t1 * Math.Pow((sg.t - (sg.t1 / 2) - Faceybar), 2) +
                sg.t2 * Math.Pow(sg.t - sg.t1, 3) / 12 * 2 + 
                sg.t2 * (sg.t - sg.t1) * Math.Pow((Faceybar - (sg.t - sg.t1) / 2), 2) * 2; // cm⁴

            Pv = oSTN_VerticalStress.PvTop; //kN/m²
            Ph1 = oSTN_VerticalStress.LongTermPh1; //kN/m²
            Ph2 = oSTN_VerticalStress.LongTermPh2; //kN/m²

            Ks = oSTN_VerticalStress.longTermSoilE * (1 - oSTN_VerticalStress.Nu12)
                / (sg.Rout * (1 + oSTN_VerticalStress.Nu12)) / (1 - 2 * oSTN_VerticalStress.Nu12); //kN/m³            

            Pv5 = 12.85; //kN/m²
            g = Pv5 / Math.PI;
            delta = (2 * Pv - Ph1 - Ph2 + Pv5) * Math.Pow(sg.Rout, 4)
                / (24 * (eta * sg.E * Faceinertia * 1E-8 + 0.0454 * Ks * Math.Pow(sg.Rout, 4))); // m 
            Ph4 = Ks * delta; //kN/m²
        }
        #endregion

        #region Calculation
        double M1 = 0;
        double M2 = 0;
        double Mmax;
        double M1c;
        double M1j;
        double Q1 = 0;
        double Q2 = 0;
        double Qmax;
        double M2c;
        double M2j;
        double P1;
        double P2;
        List<Tuple<int, double, double, double>> PvCal = new List<Tuple<int, double, double, double>>();
        List<Tuple<int, double, double, double>> Ph1Cal = new List<Tuple<int, double, double, double>>();
        List<Tuple<int, double, double, double>> Ph2_1Cal = new List<Tuple<int, double, double, double>>();
        List<Tuple<int, double, double, double>> kdeltaCal = new List<Tuple<int, double, double, double>>();
        List<Tuple<int, double, double, double>> Pv5Cal = new List<Tuple<int, double, double, double>>();
        List<Tuple<int, double, double, double>> totalCal = new List<Tuple<int, double, double, double>>();
        public void Calculation()
        {
                     

            int j = 2;
            for (int i = 0; i <= 180 ; i += 5)
            {
                double radi = i * Math.PI / 180;
                double sinI = Math.Sin(radi);
                double cosI = Math.Cos(radi);

                double MTotal = 0;
                double NTotal = 0;
                double QTotal = 0;

                // Pv
                double M = (1 - 2 * Math.Pow(sinI, 2)) / 4 * Pv * sg.Rout * sg.Rout;
                double N = Pv * sg.Rout * Math.Pow(sinI, 2);
                double Q = Pv * sg.Rout * sinI * cosI * (-1);
                PvCal.Add(Tuple.Create(i, M, N, Q));
                MTotal += M;
                NTotal += N;
                QTotal += Q;
                //

                // Ph1
                M = ((1 - 2 * Math.Pow(cosI, 2)) / 4) * Ph1 * sg.Rout * sg.Rout;
                N = Ph1 * sg.Rout * Math.Pow(cosI, 2);
                Q = Ph1 * sg.Rout * sinI * cosI;
                Ph1Cal.Add(Tuple.Create(i, M, N, Q));
                MTotal += M;
                NTotal += N;
                QTotal += Q;
                //


                // Ph2 - Ph1
                M = (6 - 3 * cosI - 12 * Math.Pow(cosI, 2) + 4 * Math.Pow(cosI, 3)) / 48 * (Ph2 - Ph1) * sg.Rout * sg.Rout;
                N = (cosI + 8 * Math.Pow(cosI, 2) - 4 * Math.Pow(cosI, 3)) / 16 * (Ph2 - Ph1) * sg.Rout;
                Q = (sinI + 8 * sinI * cosI - 4 * sinI * Math.Pow(cosI, 2)) / 16 * (Ph2 - Ph1) * sg.Rout;
                Ph2_1Cal.Add(Tuple.Create(i, M, N, Q));
                MTotal += M;
                NTotal += N;
                QTotal += Q;
                //


                // Ph4
                if (i <= 90)
                {
                    switch (i)
                    {
                        case int a when (a < 45):
                            M = (0.2346 - 0.3536 * cosI) * Ks * delta * sg.Rout * sg.Rout;
                            N = 0.3536 * cosI * Ks * delta * sg.Rout;
                            Q = 0.3536 * sinI * Ks * delta * sg.Rout;                            
                            break;
                        case int a when (a <= 90):
                            M = (-0.3487 + 0.5 * Math.Pow(sinI, 2) + 0.2357 * Math.Pow(cosI, 3)) * Ks * delta * sg.Rout * sg.Rout;
                            N = (-0.7071 * cosI + Math.Pow(cosI, 2) + 0.7071 * Math.Pow(sinI, 2) * cosI) * Ks * delta * sg.Rout;
                            Q = (sinI * cosI - 0.7071 * Math.Pow(cosI, 2) * sinI) * Ks * delta * sg.Rout;                            
                            break;
                    }                                        
                }
                else
                {
                    if(i == 180)
                    {
                        M = kdeltaCal[0].Item2;
                        N = kdeltaCal[0].Item3;
                        Q = kdeltaCal[0].Item4;
                    }
                    else
                    {
                        M = kdeltaCal[kdeltaCal.Count - j].Item2;
                        N = kdeltaCal[kdeltaCal.Count - j].Item3;
                        Q = kdeltaCal[kdeltaCal.Count - j].Item4;
                        j += 2;
                    }                    
                }
                kdeltaCal.Add(Tuple.Create(i, M, N, Q));
                MTotal += M;
                NTotal += N;
                QTotal += Q;
                // 
                

                //Pv5
                switch (i)
                {
                    case int a when (a <= 90):
                        M = (Math.PI * 3 / 8 - radi * sinI - cosI * 5 / 6) * g * sg.Rout * sg.Rout;
                        N = (radi * sinI - cosI / 6) * g * sg.Rout;
                        Q = (radi * cosI + sinI / 6) * g * sg.Rout;
                        break;
                    default:
                        M = (Math.PI * (-1) / 8 + (Math.PI - radi) * sinI - cosI * 5 / 6 - Math.PI / 2 * Math.Pow(sinI, 2))
                            * g * Math.Pow(sg.Rout, 2);
                        N = (Math.PI * (-1) * sinI + radi * sinI + Math.PI * Math.Pow(sinI, 2) - cosI / 6) * g * sg.Rout;
                        Q = ((Math.PI - radi) * cosI - Math.PI * sinI * cosI - sinI / 6) * g * sg.Rout;
                        break;
                }
                Pv5Cal.Add(Tuple.Create(i, M, N, Q));
                MTotal += M;
                NTotal += N;
                QTotal += Q;
                // 

                totalCal.Add(Tuple.Create(i, MTotal, NTotal, QTotal));

                if (MTotal > M1) { M1 = MTotal; Mmax = MTotal; P1 = NTotal; } // kN-m
                if (MTotal < M2) { M2 = MTotal; P2 = NTotal; }// kN-m

                if (QTotal > Q1) { Q1 = QTotal; Qmax = QTotal; P2 = NTotal; } // kN
                if (QTotal < Q2) { Q2 = QTotal; P2 = NTotal; }// kN
            }

            M1c = (1 + xi) * Mmax / p.newton; // t-m
            M1j = (1 - xi) * Mmax / p.newton; // t-m
            Qmax /= p.newton; // t

            M2c = (1 + xi) * M2 / p.newton; // t-m
            M2j = (1 - xi) * M2 / p.newton; // t-m

            P1 /= p.newton; // t
            P2 /= p.newton; // t
        }
        #endregion

        #region Check
        double stressP1t;
        bool P1tBool;
        double stressP1c;
        bool P1cBool;

        double stressP2t;
        bool P2tBool;
        double stressP2c;
        bool P2cBool;

        double lx;
        double ly;
        double Vx;
        double Wx;
        double Wy;
        double CentermaxMx;
        double CentermaxMy;
        double EdgeMaxMx;
        double EdgeMaxMy;
        double ElasStressX;
        double ElasStressY;
        bool MxElasticBool;
        bool MyElasticBool;

        double alpha;
        double SeelyM;
        double SeelyStress;
        bool SeelyBool;
        public void SGCheck()
        {           
            // 鑄鐵環片外側板之檢測
            stressP1t = P1 * 1E3 / FaceAeff - M1c * 1E5 * Faceybar / Faceinertia;            
            if (stressP1t < sg.Fat) P1tBool = true;
            else P1tBool = false;

            stressP1c = P1 * 1E3 / FaceAeff + M1c * 1E5 * (sg.t - Faceybar) / Faceinertia;            
            if (stressP1c < sg.Fac) P1cBool = true;
            else P1cBool = false;

            stressP2t = P2 * 1E3 / FaceAeff + M2c * 1E5 * (sg.t - Faceybar) / Faceinertia;            
            if (stressP2t < sg.Fat) P2tBool = true;
            else P2tBool = false;

            stressP2c = P2 * 1E3 / FaceAeff - M2c * 1E5 * Faceybar / Faceinertia;            
            if (stressP2c < sg.Fac) P2cBool = true;
            else P2cBool = false;

            
            // 四邊固定支承矩形板彈性設計法
            ly = sg.width / 100; // m
            lx = sg.Rout * 2 * Math.PI * sg.theta / 360; // m

            double C = ly / lx;
            Vx = 1 - (C * C * 5 / 18 / (1 + Math.Pow(C,4)));
            double Vy = Vx;
            Wx = Pv / (Math.Pow(1 / C, 4) + 1);
            Wy = Pv / (1 + Math.Pow(C, 4));

            CentermaxMx = Vx / 24 * Wx * lx * lx;
            CentermaxMy = Vy / 24 * Wy * ly * ly;

            EdgeMaxMx = Wx * lx * lx / 12 * (-1);
            EdgeMaxMy = Wy * ly * ly / 12 * (-1);

            ElasStressX = (EdgeMaxMx / p.newton * 1E3 * 1E2 * (sg.t1 / 2)) / (lx * 100 * Math.Pow(sg.t1, 3) / 12);
            ElasStressY = (EdgeMaxMy / p.newton * 1E3 * 1E2 * (sg.t1 / 2)) / (ly * 100 * Math.Pow(sg.t1, 3) / 12);
                        
            if (ElasStressX < sg.Fat) MxElasticBool = true;
            if (ElasStressY < sg.Fat) MyElasticBool = true;

            // Seely 法
            alpha = lx / ly;

            //beta = Math.Round(0.0512 * Math.Pow(alpha, 3) - 0.0959 * Math.Pow(alpha, 2) + 0.0136 * alpha + 0.0632,2);
            DataTable dt = oExecuteSQL.GetByUID("STN_Section", sectionUID);
            beta = double.Parse(dt.Rows[0]["CastIronBeta"].ToString());
            //查圖得知，以用找點公式自動抓取

            SeelyM = beta * Pv * 2 * lx * lx; // kN-m/m
            SeelyStress = 6 * SeelyM * 100 / sg.t1 / sg.t1; // kg/cm²
                        
            if (SeelyStress < sg.Fat) SeelyBool = true;
            else SeelyBool = false;
        }

        double b1;
        double b2;
        double b3;

        double ribBeff;
        double ribB;
        double LatAeff;
        double Latybar;
        double Latinertia;
        double LatR;
        double slRatio;
        double e;
        double eachP;
        double eachM;
        double fa;
        double fb;

        double fe;
        double Cm;
        double SFCM;
        double SF;
        public void SGPush()
        {
            b1 = sg.t - sg.t1;
            b2 = b1;
            b3 = 0;

            ribBeff = 20 * sg.t1;
            ribB = sg.theta / 360 * Math.PI * sg.Rout * 2 * 100;
            if (ribBeff > ribB) ribBeff = ribB;

            LatAeff = ribBeff * sg.t1 + (b2 + b3) * sg.t3;
            Latybar = ((ribBeff * sg.t1) * (sg.t - sg.t1 / 2) + (b2 * sg.t3) * (b2 / 2) + (b3 * sg.t3) * (sg.t3 / 2)) / LatAeff;
            Latinertia = (ribBeff * Math.Pow(sg.t1, 3) / 12) + ribBeff * sg.t1 * Math.Pow(sg.t - sg.t1 / 2 - Latybar, 2) +
                sg.t3 * Math.Pow(b2, 3) / 12 + sg.t3 * b2 * Math.Pow(Latybar - b2 / 2, 2) +
                b3 * Math.Pow(sg.t3, 3) / 12 + b3 * sg.t3 * Math.Pow(Latybar - sg.t3 / 2, 2);
            LatR = Math.Pow(Latinertia / LatAeff, 0.5);
            slRatio = 1 * sg.width / LatR;

            e = Latybar - (sg.t - sg.t1 / 2) / 2; // cm

            eachP = jackP * jackNum * sg.theta / 360; // t

            eachM = eachP * e * 1000; // kg-cm 
            fa = eachP / LatAeff * 1000; // kg-cm
            fb = eachM * Latybar / Latinertia; // kg-cm

            fe = 12 * Math.PI * Math.PI * sg.E / 100 / (23 * slRatio * slRatio);

            Cm = 0.85;

            SFCM = fa / sg.Fac + Cm * fb / ((1 - fa / fe) * sg.Fat);

            SF = fa / sg.Fac + fb / sg.Fat;
        }

        List<Tuple<double, double, double, double, double, double, string>> poreCheck =
                new List<Tuple<double, double, double, double, double, double, string>>();
        public void SGPore()
        {            
            for(int i = 0; i < totalCal.Count; i++)
            {
                double m = totalCal[i].Item2 * 1.3 / p.newton / 2;
                double q = totalCal[i].Item4 * 1.3 / p.newton / 2;
                double V1 = q / poreNum;
                double V2 = m * poreVerSpacing / 2 / (4 * Math.Pow(poreVerSpacing / 2, 2) + 4 * Math.Pow(poreLatSpacing / 2, 2));
                double Vmax = Math.Pow(V1 * V1 + V2 * V2, 0.5);
                double Fv = 5.2;
                bool vCehck;
                string checkSTR;
                if (Vmax < Fv) { vCehck = true; checkSTR = "OK"; }
                else { vCehck = false; checkSTR = "NG"; }

                poreCheck.Add(Tuple.Create(m, q, V1, V2, Vmax, Fv, checkSTR));
            }
        }
        #endregion

        #region Export Web        
        
        double outPv5;                                
        double outPv;
        double outPh1;
        double outPh2;
        double outDelta;
        double outPh4;

        double outFaceAeff;
        double outFaceyBar;
        double outFaceInertia;

        double outM1c;
        double outM2c;
                       
        public void OutRound()
        {                        
            outPv5 = Math.Round(Pv5, 2);                                                
            outPv = Math.Round(Pv, 2);
            outPh1 = Math.Round(Ph1, 2);
            outPh2 = Math.Round(Ph2, 2);
            outDelta = Math.Round(delta, 6);
            outPh4 = Math.Round(Ph4, 2);

            outFaceAeff = Math.Round(FaceAeff, 2);
            outFaceyBar = Math.Round(Faceybar, 2);
            outFaceInertia = Math.Round(Faceinertia, 0);

            outM1c = Math.Round(M1c, 2);
            outM2c = Math.Round(M2c, 2);
        }

        public string ExportSGProps()
        {
            
            double outg = Math.Round(g, 2);
            double outKs = Math.Round(Ks, 2);

            double outSoilE = Math.Round(oSTN_VerticalStress.longTermSoilE, 2);
            double outSoilNu = Math.Round(oSTN_VerticalStress.Nu12, 2);

            string propStr = "";
            propStr += $"鑄鐵環片基本資料： <br> ";

            propStr += $" {emsp1()} 鑄鐵板降伏強度 fy = {sg.Fy} kg/cm² <br> ";
            propStr += $" {emsp1()} 鑄鐵板楊式模數 Ed = {sg.E} kN/m² <br> ";
            propStr += $" {emsp1()} 鑄鐵板單位重 γ = {sg.UW} kN/m³ <br> ";
            propStr += $" {emsp1()} 環片外徑 D = {sg.Rout * 2} m <br> ";
            propStr += $" {emsp1()} 環片寬度 B = {sg.width} cm <br> ";
            propStr += $" {emsp1()} 外緣面板厚度 t1 = {sg.t1} cm <br> ";
            propStr += $" {emsp1()} 主樑板厚度 t2 = {sg.t2} cm <br> ";
            propStr += $" {emsp1()} 肋板厚度 t3 = {sg.t3} cm <br> ";
            propStr += $" {emsp1()} 環片厚度 t = {sg.t} m <br> ";
            propStr += $" {emsp1()} 縱肋版相隔角度 θ = {sg.theta}° <br> ";
            //propStr += $" <br> {image("images\\鑄鐵環片切面.jpg")} <br> ";
            //propStr += $" <br> {image("images\\鑄鐵環片剖面_03修正.jpg")} <br> ";
            propStr += $" <br> {image("E:\\2019研發案\\Winform\\images\\鑄鐵環片切面.jpg")} <br> ";
            propStr += $" <br> {image("E:\\2019研發案\\Winform\\images\\鑄鐵環片剖面_03修正.jpg")} <br> ";

            propStr += $" {emsp1()} (1)有效寬度 beff <br> ";
            propStr += $" {emsp2()} beff = 25 * t1 = 25 * {sg.t1} = {25 * sg.t1} cm <br> ";
            propStr += $" {emsp2()} B = {sg.width} cm <br> ";

            string strTemp;
            if ((25 * sg.t1) > sg.width) strTemp = ">";
            else strTemp = "<";
            propStr += $" {emsp2()} beff {strTemp} B <br> ";
            propStr += $" {emsp2()} 取 beff = {Facebeff} cm <br> ";

            propStr += $" {emsp1()} (2)面積A,慣性矩I <br> ";
            propStr += $" {emsp2()} A = {sg.width}*{sg.t1} + ({sg.t} - {sg.t1})*{sg.t2}*2 = {outFaceAeff} cm² <br> ";
            propStr += $" {emsp2()} y' = [{sg.width}*{sg.t1}*{sg.t - sg.t1 / 2} + {sg.t - sg.t1}*{sg.t2}*{sg.t - sg.t1}/2*2]" +
                $"/{outFaceAeff} = {outFaceyBar} cm <br> ";
            propStr += $" {emsp2()} I = 1/12*{sg.width}*{sg.t1}³ + {sg.width}*{sg.t1}*({sg.t - sg.t1 / 2} - {outFaceyBar})² +" +
                $" 1/12*{sg.t2}*{sg.t - sg.t1}³ + {sg.t2}*{sg.t - sg.t1}*({outFaceyBar} - {sg.t - sg.t1}/2)² * 2 " +
                $" = {outFaceInertia} cm⁴ <br> ";

            propStr += $" {emsp1()} (3)環片自重(每1m) <br> ";
            propStr += $" {emsp2()} Pv5 = ~ = {outPv5} kN/m² <br> ";
            propStr += $" {emsp2()} g = Pv5 / π = {outg} <br> ";

            propStr += $" {emsp1()} (4)側向反力 Ph4 <br> ";
            propStr += $" {emsp2()} Ks = Eavg * (1 - υs) / [R * (1 + υs) * (1 - 2*υs)] = " +
                $" {outSoilE}*(1-{outSoilNu}) / [{sg.Rout}*(1 + {outSoilNu})" +
                $"*(1 - 2*{outSoilNu})] = {outKs} kN/m³ <br> ";
            propStr += $" {emsp2()} δ = (2Pv - Ph1 - Ph2 + πg)*R⁴/[24(ηEavgI + 0.0454*Ks*R⁴)] =" +
                $" (2*{outPv} - {outPh1} - {outPh2} + π*{outg})*{sg.Rout}⁴ / [24({eta}*{sg.E}*{outFaceInertia}*1E-8 +" +
                $" 0.0454*{outKs}*{sg.Rout}⁴ = {outDelta} m <br> ";
            propStr += $" {emsp2()} Ph4 = {outPh4} kN/m² (=Ks*δ) <br> ";

            return propStr;
        }

        public string ExportSGCalculation()
        {
            string calStr = "";

            calStr += $"環片受力分析(慣用計算) <br> ";

            calStr += $" {emsp1()} 環片半徑 R = {sg.Rout} m <br> ";
            calStr += $" {emsp1()} 環片厚度 = {sg.t / 100} m <br> ";
            calStr += $" {emsp1()} 頂拱垂直荷重 Pv = {outPv} kN/m² <br> ";
            calStr += $" {emsp1()} 頂拱水平荷重 Ph1 = {outPh1} kN/m² <br> ";
            calStr += $" {emsp1()} 仰拱水平荷重 Ph2 = {outPh2} kN/m² <br> ";
            calStr += $" {emsp1()} 側向反力 Ph4 = {outPh4} kN/m² <br> ";
            calStr += $" {emsp1()} 環片自重 Pv5 = {outPv5} kN/m² <br> ";
            calStr += $" {emsp1()} 環片勁度折減係數 η = {eta} <br> ";
            calStr += $" {emsp1()} 彎矩修正因子 ξ = {xi} <br> <br> ";
                       

            string PvStr = "";
            PvStr += $"1.由垂直線型荷重Pv產生之力量 <br> ";
            PvStr += $" {emsp1()} 彎矩 M = 1/4*(1 - 2*sin²θ)*Pv*R² <br> ";
            PvStr += $" {emsp1()} 軸力 N = Pv*R*sin²θ <br> ";
            PvStr += $" {emsp1()} 剪力 Q = Pv*R*sinθ*cosθ <br> ";

            string table = ForceTable(PvCal);            
            PvStr += table;

            calStr += PvStr;
            calStr += "<br>";

            string Ph1Str = "";
            Ph1Str += $"2.由水平線型荷重Ph1產生之力量 <br> ";
            Ph1Str += $" {emsp1()} 彎矩 M = 1/4*(1 - 2*cos²θ)*Ph1*R² <br> ";
            Ph1Str += $" {emsp1()} 軸力 N = Ph1*R*cos²θ <br> ";
            Ph1Str += $" {emsp1()} 剪力 Q = Ph1*R*sinθ*cosθ <br> ";
            table = ForceTable(Ph1Cal);
            Ph1Str += table;

            calStr += Ph1Str;
            calStr += " <br> ";

            string Ph2_1Str = "";
            Ph2_1Str += $"3.由水平線型荷重Ph2-Ph1產生之力量 <br> ";
            Ph2_1Str += $" {emsp1()} 彎矩 M = 1/48*(6 - 3*cosθ - 12*cos²θ + 4*cos³θ)*(Ph2 - Ph1) * R² <br> ";
            Ph2_1Str += $" {emsp1()} 軸力 N = 1/16*(cosθ + 8*cos²θ - 4*cos³θ)*(Ph2 - Ph1) * R <br> ";
            Ph2_1Str += $" {emsp1()} 剪力 Q = 1/16*(sinθ + 8*sinθ*cosθ - 4*sinθ*cos²θ)*(Ph2 - Ph1) * R <br> ";
            table = ForceTable(Ph2_1Cal);
            Ph2_1Str += table;

            calStr += Ph2_1Str;
            calStr += " <br> ";

            string Ph4Str = "";
            Ph4Str += $"4.由側向反力荷重Ph4產生之力量 <br> ";
            Ph4Str += $" {emsp1()} 當 0°≦ θ <45° <br> ";
            Ph4Str += $" {emsp1()} 彎矩 M = (0.2346 - 0.3536*cosθ) * Ph4 * R² <br> ";
            Ph4Str += $" {emsp1()} 軸力 N = 0.3536*cosθ * Ph4 * R <br> ";
            Ph4Str += $" {emsp1()} 剪力 Q = 0.3536*sinθ * Ph4 * R <br> ";

            Ph4Str += $" {emsp1()} 當 45°≦ θ ≦90° <br> ";
            Ph4Str += $" {emsp1()} 彎矩 M = (-0.3487 + 0.5*sin²θ + 0.2357*cos³θ) * Ph4 * R² <br> ";
            Ph4Str += $" {emsp1()} 軸力 N = (-0.7071*cosθ + cos²θ + 0.7071*sin²θ*cosθ) * Ph4 * R <br> ";
            Ph4Str += $" {emsp1()} 剪力 Q = (sinθ*cosθ - 0.7071*cos²θ*sinθ) * Ph4 * R <br> ";
            table = ForceTable(kdeltaCal);
            Ph4Str += table;

            calStr += Ph4Str;
            calStr += " <br> ";

            string Pv5Str = "";
            Pv5Str += $"5.由襯砌自重Pv5產生之力量 <br> ";
            Pv5Str += $" {emsp1()} 當 0°≦ θ ≦90° <br> ";
            Pv5Str += $" {emsp1()} 彎矩 M = (3/8*π - θ*sinθ - 5/6*cosθ) * g * R² <br> ";
            Pv5Str += $" {emsp1()} 軸力 N = (θ*sinθ - 1/6*cosθ) * g * R <br> ";
            Pv5Str += $" {emsp1()} 剪力 Q = (θ*cosθ + 1/6*sinθ) * g * R <br> ";

            Pv5Str += $" {emsp1()} 當 90°< θ ≦180° <br> ";
            Pv5Str += $" {emsp1()} 彎矩 M = [-1/8*π + (π - θ)*sinθ - 5/6*cosθ - 1/2*π*sin²θ] * g * R² <br> ";
            Pv5Str += $" {emsp1()} 軸力 N = (-π*sinθ + θ*sinθ + π*sin²θ - 1/6*cosθ) * g * R <br> ";
            Pv5Str += $" {emsp1()} 剪力 Q = [(π - θ)*sinθ - π*sinθ*cosθ - 1/6*sinθ) * g * R <br> ";
            table = ForceTable(Pv5Cal);
            Pv5Str += table;

            calStr += Pv5Str;
            calStr += " <br> ";

            string P6Str = "";
            P6Str += $"6.環片襯砌所受之總力(Pv,Ph2,Ph2,Ph4,Ph5) <br> ";
            table = ForceTable(totalCal);
            P6Str += table;

            calStr += P6Str;
            calStr += " <br> ";


            calStr += $" <br> M1 = {Math.Round(M1, 2)} kN-m <br> ";
            calStr += $" M2 = {Math.Round(M2, 2)} kN-m <br> ";
            calStr += $" Mmax = {Math.Round(Mmax, 2)} kN-m <br> ";
            calStr += $" M1c = (1 + ξ)M1 = {Math.Round(outM1c * p.newton, 2)} kN-m = {outM1c} t-m <br> ";
            calStr += $" M1j = (1 - ξ)M1 = {Math.Round(M1j * p.newton, 2)} kN-m = {Math.Round(M1j, 2)} t-m <br> ";
                        
            calStr += $" Q1 = {Math.Round(Q1, 2)} kN <br> ";
            calStr += $" Q2 = {Math.Round(Q2, 2)} kN <br> ";
            calStr += $" Qmax = {Math.Round(Qmax, 2)} kN <br> ";
            calStr += $" M2c = (1 + ξ)M2 = {Math.Round(outM2c * p.newton, 2)} kN-m = {outM2c} t-m <br> ";
            calStr += $" M2j = (1 + ξ)M2 = {Math.Round(M2j * p.newton, 2)} kN-m = {Math.Round(M2j, 2)} t-m <br> ";

            return calStr;
        }

        string ForceTable(List<Tuple<int, double ,double, double>> force)
        {
            string tableStart = $" {emsp1()} <table cellpadding='1' border='5' width='400'> <tr> ";
            tableStart += $" <th> 角度(°) </th> <th> 彎矩(kN-m) </th> <th> 軸力(kN) </th> <th> 剪力(kN) </th> ";
            string tableEnd = " </table> ";
            string table = "";

            table = tableStart;            
            for(int i = 0; i < force.Count; i++)
            {
                double M = Math.Round(force[i].Item2, 2);
                double N = Math.Round(force[i].Item3, 2);
                double Q = Math.Round(force[i].Item4, 2);
                table += $" <tr> <th> {force[i].Item1} </th> <th> {M} </th> <th> {N} </th> <th> {Q} </th> ";
            }
            table += tableEnd;
            return table;
        }

        public string ExportSGCheck()
        {
            double outP1 = Math.Round(P1, 2);
            double outP2 = Math.Round(P2, 2);

            double outStressP1t = Math.Round(stressP1t, 1);
            double outStressP1c = Math.Round(stressP1c, 1);
            double outStressP2t = Math.Round(stressP2t, 1);
            double outStressP2c = Math.Round(stressP2c, 1);

            double outVx = Math.Round(Vx, 4);
            double outWx = Math.Round(Wx, 4);
            double outWy = Math.Round(Wy, 4);

            double outCenterMaxMx = Math.Round(CentermaxMx, 3);
            double outCenterMaxMy = Math.Round(CentermaxMy, 3);
            double outEdgeMaxMx = Math.Round(EdgeMaxMx, 4);
            double outEdgeMaxMy = Math.Round(EdgeMaxMy, 4);
            double outElasStressX = Math.Round(ElasStressX, 3);
            double outElasStressY = Math.Round(ElasStressY, 3);

            double outAlpha = Math.Round(alpha, 3);
            double outSeelyM = Math.Round(SeelyM, 2);
            double outSeelyStress = Math.Round(SeelyStress, 2);


            string checkSTR = "";

            checkSTR += $"鑄鐵環片檢核 <br> ";
            checkSTR += $" {emsp1()} 環片外徑 D = {sg.Rout * 2} m <br> ";
            checkSTR += $" {emsp1()} 外緣面板厚度 t1 = {sg.t1} cm <br> ";
            checkSTR += $" {emsp1()} 主樑板厚度 t2 = {sg.t2} cm <br> ";
            checkSTR += $" {emsp1()} 環片厚度 t = {sg.t / 100} m <br> ";
            checkSTR += $" {emsp1()} P1 = {outP1} t <br> ";
            checkSTR += $" {emsp1()} M1 = {outM1c} t-m <br> ";
            checkSTR += $" {emsp1()} P2 = {outP2} t <br> ";
            checkSTR += $" {emsp1()} M2 = {outM2c} t-m <br> ";

            checkSTR += $" {emsp1()} be = {Facebeff} cm <br> ";
            checkSTR += $" {emsp1()} A = {FaceAeff} cm² <br> ";
            checkSTR += $" {emsp1()} y' = {Faceybar} cm <br> ";
            checkSTR += $" {emsp1()} I = {Faceinertia} cm⁴ <br> ";

            checkSTR += $" {emsp1()} 容許抗彎拉應力 = {sg.Fat} kg/cm² <br> ";
            checkSTR += $" {emsp1()} 容許抗彎壓應力 = {sg.Fac} kg/cm² <br> ";
            checkSTR += $" {emsp1()} 容許抗剪應力 = {sg.Fas} kg/cm² <br> ";

            // (1) 鑄鐵環片外側板之檢測
            checkSTR += $" {emsp1()} (1)鑄鐵環片外側板之檢測 <br> ";
            checkSTR += $" {emsp2()} σ = P1/A ± (M1*y')/I <br> ";

            string str = "";
            if (P1tBool) str = $"< {sg.Fat} kg/cm² OK ";
            else str = $"> {sg.Fat} kg/cm² NG ";
            checkSTR += $" {emsp3()} σt = {outP1}*1000/{outFaceAeff} - {outM1c}*100000*{outFaceyBar}/{outFaceInertia}" +
                $" = {outStressP1t} kg/cm² {str} <br> ";

            if (P1cBool) str = $"< {sg.Fac} kg/cm² OK ";
            else str = $"> {sg.Fac} kg/cm² NG ";
            checkSTR += $" {emsp3()} σc = {outP1}*1000/{outFaceAeff} + {outM1c}*100000*({sg.t} - {outFaceyBar})/{outFaceInertia}" +
                $" = {outStressP1c} kg/cm² {str} <br> ";

            checkSTR += $" {emsp2()} σ = P2/A ± (M2*y')/I <br> ";

            if (P2tBool) str = $"< {sg.Fat} kg/cm² OK ";
            else str = $"> {sg.Fat} kg/cm² NG ";
            checkSTR += $" {emsp3()} σt = {outP2}*1000/{outFaceAeff} - {outM2c}*100000*{outFaceyBar}/{outFaceInertia}" +
                $" = {outStressP2t} kg/cm² {str} <br> ";

            if (P2cBool) str = $"< {sg.Fac} kg/cm² OK ";
            else str = $"> {sg.Fac} kg/cm² NG ";
            checkSTR += $" {emsp3()} σc = {outP2}*1000/{outFaceAeff} + {outM2c}*100000*({sg.t} - {outFaceyBar})/{outFaceInertia}" +
                $" = {outStressP2c} kg/cm² {str} <br> ";

            // (2) 四邊固定支承矩形板彈性設計法
            checkSTR += $" {emsp1()} (2)四邊固定支承矩形板彈性設計法 <br> ";
            checkSTR += $" {emsp2()} t1 = {sg.t1} cm <br> ";
            checkSTR += $" {emsp2()} lx = {sg.Rout* 2} * π * {sg.theta}/360 = {lx} m <br> ";
            checkSTR += $" {emsp2()} ly = {ly} m <br> ";
            checkSTR += $" {emsp2()} Pv = {outPv} kN/m² <br> ";
            checkSTR += $" {emsp2()} C = ly/lx = {Math.Round(ly / lx, 4)} <br> ";
            checkSTR += $" {emsp2()} vx = vy = 1 - 5/18 * C²/(1 + C⁴) = {outVx} <br> ";
            checkSTR += $" {emsp2()} Wx = Pv/[(1/C)⁴ + 1] = {outWx} <br> ";
            checkSTR += $" {emsp2()} Wy = Pv/(1 + C⁴) = {outWy} <br> ";

            checkSTR += $" {emsp1()} 跨度中點 Mxmax = vx/24 * Wx * lx² = {outCenterMaxMx} kN-m <br> ";
            checkSTR += $" {emsp1()} 跨度中點 Mymax = vy/24 * Wy * ly² = {outCenterMaxMy} kN-m <br> ";
            checkSTR += $" {emsp1()} 固定邊平均 Mxmax = -1/12 * Wx * lx² = {outEdgeMaxMx} kN-m <br> ";
            checkSTR += $" {emsp1()} 固定邊平均 Mymax = -1/12 * Wy * ly² = {outEdgeMaxMy} kN-m <br> ";

            if (MxElasticBool) str = $"< {sg.Fat} kg/cm² OK";
            else str = $"> {sg.Fat} kg/cm² NG";
            checkSTR += $" {emsp2()} σ = My/I = {outElasStressX} {str} <br> ";

            if (MyElasticBool) str = $"< {sg.Fat} kg/cm² OK";
            else str = $"> {sg.Fat} kg/cm² NG";
            checkSTR += $" {emsp2()} σ = My/I = {outElasStressY} {str} <br> ";

            // (3)Seely 法
            checkSTR += $" {emsp1()} (3)Seely法 <br> "; 
            //checkSTR += $" <br> {image(@"images\鑄鐵環片Seely法.PNG")} <br> ";
            checkSTR += $" <br> {emsp1()} {image(@"E:\2019研發案\Winform\images\鑄鐵環片Seely法.PNG")} <br> ";
            checkSTR += $" {emsp1()} t = {sg.t1} cm <br> ";
            checkSTR += $" {emsp1()} lx = {lx} m 短邊 <br> ";
            checkSTR += $" {emsp1()} ly = {ly} m 長邊 <br> ";
            checkSTR += $" {emsp1()} Pv = {outPv} kN/m² <br> ";
            checkSTR += $" {emsp1()} α = 短邊/長邊 = lx/ly = {outAlpha} <br> ";
            checkSTR += $" {emsp1()} 查表得 β = {beta} <br> ";
            checkSTR += $" {emsp1()} M = β * Pv * lx² = {outSeelyM} kN-m/m <br> ";
            if (SeelyBool) str = $"< {sg.Fat} kg/cm² OK ";
            else str = $"> {sg.Fat} kg/cm² NG ";
            checkSTR += $" {emsp1()} σ = 6*M/t² = {outSeelyStress} kg/cm² {str} <br> ";


            return checkSTR;
        }

        public string ExportSGPush()
        {
            double outRibB = Math.Round(ribB, 1);
            double outRibBeff = Math.Round(ribBeff, 1);
            double outLatAeff = Math.Round(LatAeff, 2);
            double outLatyBar = Math.Round(Latybar, 2);
            double outLatinertia = Math.Round(Latinertia, 2);
            double outLatR = Math.Round(LatR, 2);
            double outslRatio = Math.Round(slRatio, 2);

            double oute = Math.Round(e, 2);
            double outfa = Math.Round(fa, 2);
            double outfb = Math.Round(fb, 2);

            double outfe = Math.Round(fe, 2);
            double outSFCM = Math.Round(SFCM, 3);
            double outSF = Math.Round(SF, 3);

            string pushSTR = "";            

            pushSTR += $"鑄鐵環片推力檢核 <br> ";

            pushSTR += $" {emsp1()} <table style='text-align:left' border='0'> <tr> ";

            pushSTR += $" <th> 環片外徑 D = </th> <th> {sg.Rout * 2} m </th> " +
                $"<th> 楊式模數 E = </th> <th> {sg.E / 100} kg/m² </th> <tr>";
            pushSTR += $" <th> 端點鋼板寬度 b1 = </th> <th> {b1} cm </th> " +
                $"<th> 容許抗彎拉應力 Fat = </th> <th> {sg.Fat} kg/cm² </th> <tr>";
            pushSTR += $" <th> 彎曲鋼板寬度 b2 = </th> <th> {b2} cm </th> " +
                $"<th> 容許抗彎壓應力 Fac = </th> <th> {sg.Fac} kg/cm² </th> <tr>";
            pushSTR += $" <th> 彎曲鋼板彎曲寬度 b3 = </th> <th> {b3} cm </th> " +
                $"<th> 容許抗剪應力 Fas = </th> <th> {sg.Fas} kg/cm² </th> <tr>";
            pushSTR += $" <th> 外緣面板厚度 t1 = </th> <th> {sg.t1} cm </th> <tr>";
            pushSTR += $" <th> 縱肋板厚度 t3 = </th> <th> {sg.t3} cm </th> " +
                $"<th> 單支千斤頂推力 = </th> <th> {jackP} t </th> <tr>";
            pushSTR += $" <th> 環片厚度 t = </th> <th> {sg.t} cm </th> " +
                $"<th> 千斤頂數量 = </th> <th> {jackNum} </th> <tr>";
            pushSTR += $" <th> 環片寬度 = </th> <th> {sg.width} cm </th> <tr> ";
            pushSTR += $" <th> 縱肋版相隔角度 θ = </th> <th> {sg.theta}° </th> <tr> ";

            pushSTR += " </table>";
            //pushSTR += $" {emsp1()} 環片外徑 D = {sg.Rout * 2} m <br> ";
            //pushSTR += $" {emsp1()} 端點鋼板寬度 b1 = {sg.t - sg.t1} cm <br> ";
            //pushSTR += $" {emsp1()} 彎曲鋼板寬度 b2 = {sg.t - sg.t1} cm <br> ";
            //pushSTR += $" {emsp1()} 彎曲鋼板彎曲寬度 b3 = 0 cm <br> ";
            //pushSTR += $" {emsp1()} 外緣面板厚度 t1 = {sg.t1} cm <br> ";
            //pushSTR += $" {emsp1()} 縱肋板厚度 t3 = {sg.t3} cm <br> ";

            //pushSTR += $" <br> {image(@"images\鑄鐵環片肋版參數.jpg")} <br> ";
            pushSTR += $" <br> {emsp1()} {image(@"E:\2019研發案\Winform\images\鑄鐵環片肋版參數.jpg")} <br> ";

            pushSTR += $" {emsp1()} (1)有效寬度be <br> ";
            pushSTR += $" {emsp2()} be = 20 * t1 = {20 * sg.t} cm <br> ";            
            pushSTR += $" {emsp2()} b = θ/360 * π * D = {outRibB} <br> ";
            string str = "";
            if (ribB > ribBeff) str = $">";
            else str = "<";
            pushSTR += $" {emsp2()} be {str} b, 取 be = {outRibBeff} cm <br> ";

            pushSTR += $" {emsp1()} (2)面積A, 慣性矩I, 迴轉半徑r, 長細比KL/r <br> ";
            pushSTR += $" {emsp2()} 面積 A = {outRibBeff}*{sg.t1} + ({b2} + {b3}) * {sg.t3} = {outLatAeff} cm² <br> ";
            pushSTR += $" {emsp2()} y' = [{outRibBeff}*{sg.t1}*{sg.t - sg.t1 / 2} + {b2}*{sg.t3}*({b2 / 2}/2)" +
                $" + {b3}*{sg.t3}*({sg.t3}/2)]/{outLatAeff} = {outLatyBar} cm <br> ";
            pushSTR += $" {emsp2()} 慣性矩 I = 1/12*{outRibBeff}*{sg.t1}³ + {outRibBeff}*{sg.t1}*({sg.t1 - sg.t1 / 2} - {outLatyBar})²" +
                $" + 1/12*{sg.t3}*{b2}³ + {sg.t3}*{b2}*({outLatyBar} - {b2}/2)²" +
                $" + 1/12*{b3}*{sg.t3}³ + {b3}*{sg.t3}*({outLatyBar} - {sg.t3}/2)²" +
                $" = {outLatinertia} cm⁴ <br> ";
            pushSTR += $" {emsp2()} 迴轉半徑 r = √(I/A) = {outLatR} cm <br> ";
            pushSTR += $" {emsp2()} 長細比 KL/r = 1*50/{outLatR} = {outslRatio} <br> ";


            pushSTR += $" {emsp1()} (3)偏心距 e 與受力 <br> ";
            pushSTR += $" {emsp2()} e = {outLatyBar} - {sg.t - sg.t1 / 2}/2 = {oute} cm <br> ";
            pushSTR += $" {emsp2()} 千斤頂之總推力 = {jackNum}*{jackP} = {jackNum * jackP} t <br> ";
            pushSTR += $" {emsp2()} 每隻縱肋板受力 P = {jackNum * jackP}*{sg.theta}/360 = {eachP} t <br> ";
            pushSTR += $" {emsp3()} M = P*e = {eachM} kg-cm <br> ";
            pushSTR += $" {emsp3()} fa = P/A = {outfa} kg/cm² <br> ";
            pushSTR += $" {emsp3()} fb = My/I = {outfb} kg/cm² <br> ";

            pushSTR += $" {emsp1()} (4)安全係數 <br> ";
            pushSTR += $" {emsp2()} 根據 AISC 規範 <br> ";

            //pushSTR += $" <br> {image(@"images\鑄鐵環片推力安全係數檢核.JPG")} <br> ";
            pushSTR += $" <br> {emsp1()} {image(@"E:\2019研發案\Winform\images\鑄鐵環片推力安全係數檢核.JPG")} <br> ";

            pushSTR += $" 其中 fe' = 12*π*E/[23*(KL/r)²] = {outfe} kg/cm² <br> ";
            pushSTR += $" 令 Cm = {Cm} <br> ";
            if (SFCM < 1) str = "< 1.0 OK";
            else str = "> 1.0 NG";
            pushSTR += $" fa/Fa + Cm*fb/[(1 - fa/fe')*Fb] = {outfa}/{sg.Fac} + {Cm}*{outfb}/[(1 - {outfa}/{outfe})*{sg.Fat}]" +
                $" = {outSFCM} {str} <br> ";

            if (SF < 1) str = "< 1.0 OK";
            else str = "> 1.0 NG";
            pushSTR += $" fa/Fa + fb/Fb = {outfa}/{sg.Fac} + {outfb}/{sg.Fat} = {outSF} {str} <br> ";


            return pushSTR;
        }

        public string ExportPoreCheck()
        {
            string poreSTR = "";

            poreSTR += $"鑄鐵環片螺栓檢核";

            double x1 = 0.125;
            double x2 = 0.25;
            double y1 = 0.0625;
            double y2 = 0.075;
            int n = 4;
            //poreSTR += $" <br> {image("images\\鑄鐵環片螺栓.jpg")} <br> ";
            poreSTR += $" {emsp1()} <br> {image("E:\\2019研發案\\Winform\\images\\鑄鐵環片螺栓.jpg")} <br> ";
            poreSTR += $" {emsp1()} <table style='text-align:left' border='0'> <tr> ";
            poreSTR += $" <th> t </th> <th> = {sg.t}cm </th> <tr> ";
            poreSTR += $" <th> be </th> <th> = {Facebeff}cm </th> <tr> ";
            poreSTR += $" <th> x1 </th> <th> = {x1 * 100}cm </th> <tr> ";
            poreSTR += $" <th> x2 </th> <th> = {x2 * 100}cm </th> <tr> ";
            poreSTR += $" <th> y1 </th> <th> = {y1 * 100}cm </th> <tr> ";
            poreSTR += $" <th> y2 </th> <th> = {y2 * 100}cm </th> <tr> ";
            poreSTR += $" <th> 螺栓孔數 n </th> <th> = {n} </th> <tr> ";
            poreSTR += $" </table> ";

            poreSTR += $" {emsp1()} 撓曲剪力 V1 = V/n <br> ";
            poreSTR += $" {emsp1()} 扭轉剪力 V2 = M*y/Σd² <br> ";
            poreSTR += $" {emsp1()} 最大剪力 Vmax = √(V1² + V2²) <br> ";
            poreSTR += $" {emsp1()} 容許剪力 Fv (F8T, M24, 標準孔) Fv = 5.2t <br> ";

            double deg;
            string M;
            string N;
            string Q;
            string M13;
            string Q13;
            string V1;
            string V2;
            string Vmax;
            double Fv;
            string ch;
            string table = "";
            table += $" {emsp1()} <table cellpadding='1' border='5' width='650'> <tr> ";
            table += $" <th> 角度<br>(°) </th> <th> 彎矩<br>(kN-m) </th> <th> 軸力<br>(kN) </th> <th> 剪力<br>(kN) </th> " +
                $"<th> 1.3彎矩<br>(0.5m寬)<br>(t-m) </th> <th> 1.3剪力<br>(0.5m寬)<br>(t-m) </th>" +
                $"<th> V1<br>(t) </th> <th> V2<br>(t) </th> <th> Vmax<br>(t) </th> <th> Fv<br>(t) </th> <th> check </th> <tr> ";
            for(int i = 0; i < totalCal.Count; i++)
            {
                deg = totalCal[i].Item1;
                M = Math.Round(totalCal[i].Item2, 2).ToString("F");
                N = Math.Round(totalCal[i].Item3, 2).ToString("F");
                Q = Math.Round(totalCal[i].Item4, 2).ToString("F");
                M13 = Math.Round(poreCheck[i].Item1, 2).ToString("F");
                Q13 = Math.Round(poreCheck[i].Item2, 2).ToString("F");
                V1 = Math.Round(poreCheck[i].Item3, 2).ToString("F");
                V2 = Math.Round(poreCheck[i].Item4, 2).ToString("F");
                Vmax = Math.Round(poreCheck[i].Item5, 2).ToString("F");
                Fv = poreCheck[i].Item6;
                ch = poreCheck[i].Item7;

                table += $" <th> {deg} </th> <th> {M} </th> <th> {N} </th> <th> {Q} </th> <th> {M13} </th> <th> {Q13} </th>" +
                    $" <th> {V1} </th> <th> {V2} </th> <th> {Vmax} </th> <th> {Fv} </th> <th> {ch} </th> <tr>";                
            }
            table += $" </table>";

            poreSTR += table;
            return poreSTR;
        }

        string emsp4() { return "&emsp; &emsp; &emsp; &emsp;"; }
        string emsp3() { return "&emsp; &emsp; &emsp;"; }        
        string emsp2() { return "&emsp; &emsp;"; }        
        string emsp1() { return "&emsp;"; }
        
        public string image(string str) { return $"<img src='{str}'></img> "; }
        #endregion


        public void BetaAutoCatch()
        {
            double webLy = sg.width / 100; // m
            double webLx = sg.Rout * 2 * Math.PI * sg.theta / 360; // m

            double webAlpha = webLx / webLy;
            double webBeta = Math.Round(0.0512 * Math.Pow(webAlpha, 3) - 0.0959 * Math.Pow(webAlpha, 2) + 0.0136 * webAlpha
                + 0.0632, 2);

            oExecuteSQL.UpdateData("STN_Section", "UID", sectionUID, "CastIronBeta", webBeta);
            
            
        }
    }
}
