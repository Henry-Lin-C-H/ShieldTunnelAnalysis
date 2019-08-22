using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SinoTunnel
{
    public class STN_VerticalStress //Cal02_VerticalStressIn
    {
        //STN_SQL.STN_Prop p;
        string outputCondition;
        double verticalStress;
        double lateralStress;
        double trafficStress;

        //double Pv = 0;
                
        public double[] Ph1 = new double[2];
        public double[] Ph2 = new double[2];
        public double longTermSoilE = 0;
        public double shortTermSoilE = 0;
        public double soilavgUW = 0;

        public double PvTop = 0.0; //PvTop
        public double PvBot = 0.0;
        public double longTermPa1 = 0.0;
        public double longTermPa2 = 0.0;

        public double longTermPv1 = 0.0;
        public double longtermPv2 = 0.0;
        public double LongTermPh1 = 0.0;
        public double LongTermPh2 = 0.0;

        public double ShortTermPh1 = 0.0;
        public double ShortTermPh2 = 0.0;

        public double connectorNPv = 0.0;
        public double connectorNPh1 = 0.0;
        public double connectorNPh2 = 0.0;

        public double Nu12 = 0;

        public string PvBotString = "";

        GetWebData p;       //STN_SQL.STN_Prop p;   

        double TR;
        double BH;
        public STN_VerticalStress(string UID, string outputCondition)
        {   
            p = new GetWebData(UID);
            this.outputCondition = outputCondition;
            this.TR = Math.Round(p.connector.TR, 3);
            this.BH = Math.Round(p.connector.BH, 3);
        }
        


        //[是否跨層],若否為[直徑,0],若是為[第一層長度,第二層長度]
        public List<Tuple<bool, double, double>> VerticalStress(string condition, out string outputLongtermVerticalStress, out string outputShortermVerticalStress, out string outputSurchargeLoad, out double longtermE1, out double shortermE1, out double Pvtop, out double longtermPh1, out double longtermPh2, out double shortermPh1, out double shortermPh2, out double U12)
        {
            PvBot = 0.0;
            longTermPa1 = 0.0;
            longTermPa2 = 0.0;

            double coverDepth = 0;

            switch (condition.ToUpper().ToString())
            {
                case "CONNECTOR": coverDepth = p.connector.CoverDepth; break; //聯絡通道覆土深度
                default: coverDepth = p.coverDepth; break; //隧道環片覆土深度
            }

            
            double radiusOut = p.segmentRadiusIn + p.segmentThickness; //隧道外半徑

            //潛盾機外徑與隧道外徑不同，覆土深度等皆有差
            switch (condition.ToUpper().ToString())
            {
                case "SHIELDMACHINE"://覆土深度，潛盾機尺寸與隧道不同，因此覆土深度也不一致，需額外處理
                    {                        
                        coverDepth = Math.Round(coverDepth + p.segmentRadiusOut - (p.TN_M.DO / 2), 2);
                        radiusOut = p.TN_M.DO / 2; //後續計算皆採用TUNNEL，因此將環片外半徑置換為潛盾機外半徑
                    }
                    break;
            }

            bool crossLayer = false;
            
            string outputVerticalStress = "";
            outputLongtermVerticalStress = "";
            outputShortermVerticalStress = "";
            
            int Ecal = 2800;

            //長期地下水位(使用者定義)與短期地下水位(地表)
            double[] GWL = new double[2];
            GWL[0] = p.GWL;
            GWL[1] = 0;

            string temp;            

            //計算隧道位於土層位置 
            int tunnelLevel = 0;
            int tunnelBottomLevel;
            while (coverDepth > p.SL[tunnelLevel].Depth)  //  .soilDepth[tunnelLevel])
                tunnelLevel++;

            //TUNNEL(隧道):底部深度 = 覆土深度 + 環片外徑 * 2
            //CONNECTOR(聯絡通道)：底部深度 = 覆土深度 + 聯絡通道上段半徑 + 聯絡通道下段長度
            //SHIELDMACHINE(潛盾機)：底部深度 = 計算後覆土深度 + 潛盾機外半徑 * 2
            double tunnelDNHeight = 0;
            switch (condition.ToUpper().ToString())
            {
                case "TUNNEL":
                case "SHIELDMACHINE": //後續計算採用TUNNEL的function計算
                    tunnelDNHeight = coverDepth + (radiusOut * 2);
                    break;
                case "CONNECTOR":
                    tunnelDNHeight = coverDepth + TR + BH;
                    break;
            }

            //計算超載計算與 output 超載 html
            SurchargeLoadCalculation(condition, out string outputSurcharge, coverDepth, radiusOut);
            outputSurchargeLoad = outputSurcharge;


            if (tunnelDNHeight > p.SL[tunnelLevel].Depth) tunnelBottomLevel = tunnelLevel + 1;
            else tunnelBottomLevel = tunnelLevel;

            string outbuildingload = "";

            //計算隧道頂部垂直荷重，與地下水位無關，因此無須重複計算            
            double PTop = 0;
            for (int i = 0; i < tunnelLevel; i++)
            {
                if (i == 0)
                    PTop += p.SL[i].Depth * p.SL[i].γ;                  // γ = UnitWeight;
                else
                    PTop += (p.SL[i].Depth - p.SL[i - 1].Depth) * p.SL[i].γ;  
            }
            PTop += (coverDepth - p.SL[tunnelLevel - 1].Depth) * p.SL[tunnelLevel].γ + verticalStress + trafficStress;
            Pvtop = PTop;
            double PBot = 0;

            longtermE1 = 0;
            shortermE1 = 0;
            U12 = 0;

            double[] Pa1 = new double[2];
            double[] Pa2 = new double[2];
            /*
            double Pv1 = 0;
            double Pv2 = 0;
            */
            double Pv1Top = 0;
            double Pv2Bot = 0;
            double Pv1Center = 0;
            double Pv2Center = 0;

            double E1Top = 0;
            double E2Bot = 0;
            double E1Center = 0;
            double E2Center = 0;
            double Eavg = 0;
            double PL1 = 0;
            double PL2 = 0;

            float crossDepth = p.SL[tunnelLevel].Depth;   //土層深度 (m)

            string Top_SoilType = p.SL[tunnelLevel].SoilType;         //單位重 (kN/m³)
            string Bot_SoilType = p.SL[tunnelBottomLevel].SoilType;   //單位重 (kN/m³)  

            float Top_UnitWeigh = p.SL[tunnelLevel].γ;         //單位重 (kN/m³)
            float Bot_UnitWeigh = p.SL[tunnelBottomLevel].γ;   //單位重 (kN/m³)         

            float Top_Suδv = p.SL[tunnelLevel].Suδv; //Su=不排水剪力強度,  δv=垂直有效應力
            float Bot_Suδv = p.SL[tunnelBottomLevel].Suδv; //Su=不排水剪力強度,  δv=垂直有效應力

            float Top_ν = p.SL[tunnelLevel].ν;        //柏松比 
            float Bot_ν = p.SL[tunnelBottomLevel].ν; // //柏松比 

            float Top_N = p.SL[tunnelLevel].N;         //SPT-N值
            float Bot_N = p.SL[tunnelBottomLevel].N;   //SPT-N值

            float Top_φ = p.SL[tunnelLevel].φ;         //摩擦角 (°) 單位度
            float Bot_φ = p.SL[tunnelBottomLevel].φ;   //摩擦角 (°) 單位度

            //分別計算長期與短期地下水位情形的環片荷重( j = 0 :長期荷重、 j = 1: 短期荷重)
            for (int j = 0; j < 2; j++)
            {                              
                //double Ph1 = 0;
                //double Ph2 = 0;                  
                //若隧道跨越兩土層，計算Ph2 需考量兩土層，且Pv'分為Pv1'與Pv2'，計算E值時以加權平均計算
                //若隧道無跨越兩土層，Pv2為零 
                Ph1[j] = (PTop - (coverDepth - GWL[j]) * p.newton) * (1 - Math.Sin(p.SL[tunnelLevel].φ * Math.PI / 180)) + p.newton * (coverDepth - GWL[j]) + lateralStress;     
                Pa1[j] = (PTop - (coverDepth - GWL[j]) * p.newton) * Math.Pow(Math.Tan(((90/2) - (p.SL[tunnelLevel].φ / 2)) * Math.PI / 180), 2) + p.newton * (coverDepth - GWL[j]) + lateralStress;
                if (tunnelBottomLevel == tunnelLevel)
                {
                    switch (condition.ToUpper().ToString())
                    {
                        case "SHIELDMACHINE":
                        case "TUNNEL":
                            {
                                PBot = PTop + radiusOut * 2 * Top_UnitWeigh; //隧道底部垂直總土壓

                                Pa2[j] = Pa1[j] + radiusOut * 2 * (Top_UnitWeigh - p.newton) * Math.Pow(Math.Tan(((90 / 2) - (p.SL[tunnelLevel].φ / 2)) * Math.PI / 180), 2) + p.newton * radiusOut * 2;
                                Ph2[j] = Ph1[j] + radiusOut * 2 * (Top_UnitWeigh - p.newton) * (1 - Math.Sin(p.SL[tunnelLevel].φ * Math.PI / 180)) + p.newton * radiusOut * 2;
                                //Pv1 = PTop + radiusOut * Top_UnitWeigh - (coverDepth + radiusOut - GWL[j]) * p.newton;
                                Pv1Top = PTop - (coverDepth - GWL[j]) * p.newton;                                
                                Pv2Bot = PTop + (radiusOut * 2 * Top_UnitWeigh) - ((coverDepth + radiusOut * 2 - GWL[j]) * p.newton);
                                //Pv1Center = PTop + (radiusOut * Top_UnitWeigh) - (coverDepth + radiusOut - GWL[j]) * p.newton;
                                //Pv1Center = (Pv1Top + Pv2Bot) / 2;
                                Pv1Center = Pv1Top + (radiusOut * (Top_UnitWeigh - p.newton));
                                Pv2Center = Pv1Center;
                                
                                PL1 = radiusOut;
                                PL2 = radiusOut;
                            }
                            break;
                        case "CONNECTOR":
                            {
                                PBot = PTop + (BH + TR) * Top_UnitWeigh;

                                Ph2[j] = Ph1[j] + (BH + TR) * (Top_UnitWeigh - p.newton) * (1 - Math.Sin(p.SL[tunnelLevel].φ * Math.PI / 180)) + p.newton * (BH+ TR);
                                Pv1Top = PTop - (coverDepth - GWL[j]) * p.newton;
                                Pv2Bot = PTop + (tunnelDNHeight - coverDepth) * Top_UnitWeigh - (tunnelDNHeight - GWL[j]) * p.newton;                                                                
                                PL1 = BH + TR;
                                PL2 = BH + TR;
                                Pv1Center = Pv1Top + ((PL1 / 2) * (Top_UnitWeigh - p.newton));
                                Pv2Center = Pv1Center;
                            }
                            break;
                    }                                               
                    
                    soilavgUW = Top_UnitWeigh;
                    crossLayer = false;
                }
                else
                {
                    PBot = PTop + (Top_UnitWeigh * (crossDepth - coverDepth)) + (Bot_UnitWeigh * (tunnelDNHeight - crossDepth)); //底部垂直總土壓

                    Pa2[j] = (PTop + (Top_UnitWeigh * (crossDepth - coverDepth)) + (Bot_UnitWeigh * (tunnelDNHeight - crossDepth)) - ((tunnelDNHeight - GWL[j]) * p.newton)) * Math.Pow(Math.Tan(((90 / 2) - (p.SL[tunnelBottomLevel].φ / 2)) * Math.PI / 180), 2) + (p.newton * (tunnelDNHeight - GWL[j])) + lateralStress;
                    Ph2[j] = (PTop + (Top_UnitWeigh * (crossDepth - coverDepth)) + (Bot_UnitWeigh * (tunnelDNHeight - crossDepth)) - ((tunnelDNHeight - GWL[j]) * p.newton)) * (1 - Math.Sin(Bot_φ * Math.PI / 180)) + (p.newton * (tunnelDNHeight - GWL[j])) + lateralStress;

                    Pv1Top = PTop - (coverDepth - GWL[j]) * p.newton;
                    Pv2Bot = Pv1Top + (crossDepth - coverDepth) * (Top_UnitWeigh - p.newton) + (tunnelDNHeight - crossDepth) * (Bot_UnitWeigh - p.newton);

                    Pv1Center = PTop + (Top_UnitWeigh * (crossDepth - coverDepth) / 2) - (((crossDepth - coverDepth) / 2 + coverDepth - GWL[j]) * p.newton);
                    Pv2Center = PTop + (Top_UnitWeigh * (crossDepth - coverDepth)) + (Bot_UnitWeigh * ((tunnelDNHeight - crossDepth) / 2)) - ((((tunnelDNHeight - crossDepth) / 2) + crossDepth - GWL[j]) * p.newton);

                    PL1 = crossDepth - coverDepth;
                    PL2 = tunnelDNHeight - crossDepth;
                    soilavgUW = (Top_UnitWeigh * PL1 + Bot_UnitWeigh * PL2) / (PL1 + PL2);
                    crossLayer = true;
                }

                //E值計算：先判斷土層是否為CL
                if (Top_SoilType == "CL")
                {
                    if (crossLayer)
                        E1Center = 350 * Top_Suδv * Pv1Center;
                    else
                        E1Top = 350 * Top_Suδv * Pv1Top;
                }                    
                else E1Top = Ecal * Top_N;                

                if (Bot_SoilType == "CL")
                {
                    if (crossLayer)
                        E2Center = 350 * Bot_Suδv * Pv2Center;
                    else
                        E2Bot = 350 * Bot_Suδv * Pv2Bot;
                }                    
                else E2Bot = Ecal * Bot_N;

                if(crossLayer)
                    Eavg = ((E1Center * PL1) + (E2Center * PL2)) / (PL1 + PL2);
                else
                    Eavg = (E1Top + E2Bot) / 2;                
                U12 = ((Top_ν * PL1) + (Bot_ν * PL2)) / (PL1 + PL2);
                Nu12 = U12;

                //填入兩種地下水位計算的E值
                if (j == 0) longtermE1 = Eavg;                                                  
                else if (j == 1) shortermE1 = Eavg;
                
                    


                string outPvTop = "<tr> <th colspan='2'> 頂部垂直荷重 PvTop = ";
                string outPvBot = "<tr> <th colspan='2'> 底部垂直荷重 PvBot = ";
                string outPh1 = "<tr> <th colspan='2'> 頂部水平力 Ph1 = ";
                string outPh2 = "<tr> <th colspan='2'> 底部水平力 Ph2 = ";
                string outPv1Top = "<tr> <th colspan='2'> 頂部垂直有效應力 Pv1' = ";
                string outPv2Bot = "<tr> <th colspan='2'> 底部垂直有效應力 Pv2' = ";

                //跨越兩土層時使用
                string outPv1Center = "<tr> <th colspan='2'> 上半層中心垂直有效應力 Pv1' = ";
                string outPv2Center = "<tr> <th colspan='2'> 下半層中心垂直有效應力 Pv2' = ";

                string outPa1 = "<tr> <th colspan='2'> 頂部主動土壓力 Pa1 = ";
                string outPa2 = "<tr> <th colspan='2'> 底部主動土壓力 Pa2 = ";

                string outE1Top = "<tr> <th> 頂部彈性係數 E1 = ";
                string outE2Bot = "<tr> <th> 底部彈性係數 E2 = ";

                //跨越兩土層時使用
                string outE1Center = "<tr> <th> 上半層中心彈性係數 E1 = ";
                string outE2Center = "<tr> <th> 下半層中心彈性係數 E2 = ";

                string outEavg = "<tr> <th colspan='2'> Eavg = ";
                string soilStress = "";

                //開始輸出html，先將各計算四捨五入至小數點後兩位
                tunnelDNHeight = Math.Round(tunnelDNHeight, 2);
                verticalStress = Math.Round(verticalStress, 2);
                lateralStress = Math.Round(lateralStress, 2);
                trafficStress = Math.Round(trafficStress, 2);
                PTop = Math.Round(PTop, 2);
                PBot = Math.Round(PBot, 2);
                double tempPh1 = Math.Round(Ph1[j], 2);
                double tempPh2 = Math.Round(Ph2[j], 2);
                double tempPa1 = Math.Round(Pa1[j], 2);
                double tempPa2 = Math.Round(Pa2[j], 2);
                Pv1Top = Math.Round(Pv1Top, 2);
                Pv2Bot = Math.Round(Pv2Bot, 2);
                Pv1Center = Math.Round(Pv1Center, 2);
                Pv2Center = Math.Round(Pv2Center, 2);
                PL1 = Math.Round(PL1, 2);
                PL2 = Math.Round(PL2, 2);
                double tunnelK0 = Math.Round(1 - Math.Sin(Top_φ * Math.PI / 180), 2);
                double tunnelBottomK0 = Math.Round(1 - Math.Sin(Bot_φ * Math.PI / 180), 2);
                double tunnelKa = Math.Round(Math.Pow(Math.Tan(((90 / 2) - (Top_φ / 2)) * Math.PI / 180), 2), 2);
                double tunnelBottomKa = Math.Round(Math.Pow(Math.Tan(((90 / 2) - (Bot_φ / 2)) * Math.PI / 180), 2), 2);

                //先以輸出 webform 為主
                if (outputCondition.ToUpper().ToString() == "WEBFORM")
                {
                    //輸出長期與短期的 html
                    if (j == 0)
                    {
                        switch (condition.ToUpper().ToString())
                        {
                            case "TUNNEL":
                                outbuildingload = $"<span style='font-size:20px;'> <b> 環片荷重計算(長期荷重－低水位) </b> </span> <br>";
                                break;
                            case "CONNECTOR":
                                outbuildingload = $"<span style='font-size:20px;'> <b> 聯絡通道荷重計算(長期荷重－低水位) </b> </span> <br>";
                                break;
                            case "SHIELDMACHINE":
                                outbuildingload = $"<span style='font-size:20px;'> <b> 潛盾機荷重計算(長期荷重－低水位) </b> </span> <br>";
                                break;
                        }
                    }
                    else if (j == 1)
                    {
                        switch (condition.ToUpper().ToString())
                        {
                            case "TUNNEL":
                                outbuildingload = $"<span style='font-size:20px;'> <b> 環片荷重計算(短期荷重－地下水位於地表) </b> </span> <br>";
                                break;
                            case "CONNECTOR":
                                outbuildingload = $"<span style='font-size:20px;'> <b> 聯絡通道荷重計算(短期荷重－地下水位於地表) </b> </span> <br>";
                                break;
                            case "SHIELDMACHINE":
                                outbuildingload = $"<span style='font-size:20px;'> <b> 潛盾機荷重計算(短期荷重－地下水位於地表) </b> </span> <br>";
                                break;
                        }
                    }

                    string table = $"<table style='3px blaci solid; text-align:left' cellpadding='8' border='5'> <tr> ";

                    outbuildingload += table;
                    outbuildingload += $"<th style='text-align:center'>  建物載重 </th> ";
                    outbuildingload += $"<th> <font size='5'>σ</font>z = {verticalStress} kM/m² &nbsp <font size='5'>σ</font>h = {lateralStress} kN/m² </th> ";

                    outbuildingload += $"<tr> <th style='text-align:center'> 交通荷重 </th>  ";
                    outbuildingload += $"<th> <font size='5'>σ</font>z = {trafficStress} kN/m² </th> ";

                    for (int i = 0; i < tunnelLevel; i++)
                    {
                        if (i == 0) temp = $"{p.SL[i].Depth} * {p.SL[i].γ}";
                        else temp = $"{p.SL[i].Depth - p.SL[i-1].Depth} * {p.SL[i].γ}";
                        soilStress += temp + " + ";
                    }
                    soilStress += $"{Math.Round((coverDepth - p.SL[tunnelLevel - 1].Depth), 2)} * {Top_UnitWeigh} + {verticalStress} + {trafficStress}";

                    outPvTop += $"{soilStress} <br> &nbsp = {PTop} kN/m² </th> ";

                    outPh1 += $"[PvTop - ({coverDepth} - {GWL[j]}) * {Math.Round(p.newton, 2)}] * {tunnelK0} + {Math.Round(p.newton, 2)} * ({coverDepth} - {GWL[j]}) + {lateralStress} <br> &nbsp = {tempPh1} kN/m² </th> ";
                    outPa1 += $"[PvTop - ({coverDepth} - {GWL[j]}) * {Math.Round(p.newton, 2)}] * {tunnelKa} + {Math.Round(p.newton, 2)} * ({coverDepth} - {GWL[j]}) + {lateralStress} <br> &nbsp = {tempPa1} kN/m² </th> ";
                    if (!crossLayer) //當隧道跨越兩個土層斷面時，計算方式有差
                    {
                        switch (condition.ToUpper().ToString())
                        {
                            case "SHIELDMACHINE":
                            case "TUNNEL":
                                {
                                    radiusOut = Math.Round(radiusOut, 2);
                                    outPvBot += $"PvTop + {radiusOut} * 2 * {Top_UnitWeigh} <br> &nbsp = {PBot} kN/m² </th> ";

                                    outPh2 += $"Ph1 + ({radiusOut * 2} * {Top_UnitWeigh} - {radiusOut * 2} * {Math.Round(p.newton, 2)}) * {tunnelK0} + {radiusOut * 2} * {Math.Round(p.newton, 2)} <br> &nbsp = {tempPh2} kN/m² </th> ";
                                    outPa2 += $"[PvTop + ({tunnelDNHeight} - {coverDepth}) * {Bot_UnitWeigh} - ({tunnelDNHeight} - {GWL[j]}) * {Math.Round(p.newton, 2)}] * {tunnelKa} + {Math.Round(p.newton, 2)} * ({tunnelDNHeight} - {GWL[j]}) + {lateralStress} <br> &nbsp = {tempPa2} kN/m² </th> ";
                                    outPv1Top += $"PvTop - ({coverDepth} - {GWL[j]}) * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv1Top} kN/m² </th> ";
                                    outPv2Bot += $"PvTop + {radiusOut} * 2 * {Top_UnitWeigh} - ({coverDepth} + {radiusOut} * 2 - {GWL[j]}) * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv2Bot} kN/m² </th> ";
                                }
                                break;
                            case "CONNECTOR":
                                {
                                    outPvBot += $"PvTop + ({BH} + {TR}) * {Top_UnitWeigh} <br> &nbsp = {PBot} kN/m² </th> ";

                                    outPh2 += $"Ph1 + ({BH + TR} * {Top_UnitWeigh} - {BH + TR} * {Math.Round(p.newton, 2)}) * {tunnelK0} + {BH + TR} * {Math.Round(p.newton, 2)} <br> &nbsp = {tempPh2} kN/m² </th> ";
                                    outPv1Top += $"PvTop - ({coverDepth} - {GWL[j]}) * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv1Top} kN/m² </th> ";
                                    outPv2Bot += $"PvTop + {BH + TR} * {Top_UnitWeigh} - ({coverDepth} + {BH + TR} - {GWL[j]}) * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv2Bot} kN/m² </th> ";
                                }
                                break;
                        }                        
                    }
                    else
                    {
                        outPvBot += $"PvTop + ({crossDepth} - {coverDepth}) * {Top_UnitWeigh} + ({tunnelDNHeight} - {crossDepth}) * {Bot_UnitWeigh} <br> &nbsp = {PBot} kN/m² </th> ";

                        outPh2 += $"[PvTop + {Top_UnitWeigh} * ({crossDepth} - {coverDepth}) + {Bot_UnitWeigh} * ({tunnelDNHeight} - {crossDepth}) - ({tunnelDNHeight} - {GWL[j]}) * {Math.Round(p.newton, 2)}] * {tunnelBottomK0} + {Math.Round(p.newton, 2)} * ({tunnelDNHeight} - {GWL[j]}) + {lateralStress} <br> &nbsp = {tempPh2} kN/m² </th> ";
                        outPa2 += $"[PvTop + {Top_UnitWeigh} * ({crossDepth} - {coverDepth}) + {Bot_UnitWeigh} * ({tunnelDNHeight} - {crossDepth}) - ({tunnelDNHeight} - {GWL[j]}) * {Math.Round(p.newton, 2)}] * {tunnelBottomKa} + {Math.Round(p.newton, 2)} * ({tunnelDNHeight} - {GWL[j]}) + {lateralStress} <br> &nbsp = {tempPa2} kN/m² </th> ";

                        outPv1Top += $"PvTop - ({crossDepth} - {GWL[j]}) * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv1Top} kN/m² </th> ";
                        outPv2Bot += $"Pv1' + ({crossDepth} - {coverDepth}) * ({Top_UnitWeigh} - {Math.Round(p.newton, 2)}) + ({tunnelDNHeight} - {crossDepth}) * ({Bot_UnitWeigh} - {Math.Round(p.newton, 2)}) <br> &nbsp = {Pv2Bot} kN/m² </th> ";

                        outPv1Center += $"PvTop + {Top_UnitWeigh} * ({crossDepth} - {coverDepth}) - [({crossDepth} - {coverDepth})/2 + {coverDepth} - {GWL[j]}] * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv1Center} kN/m² </th> ";
                        outPv2Center += $"PvTop + {Top_UnitWeigh} * ({crossDepth} - {coverDepth}) + {Bot_UnitWeigh} * ({tunnelDNHeight} - {crossDepth})/2 - [{tunnelDNHeight} - ({tunnelDNHeight} - {crossDepth})/2 - {GWL[j]}] * {Math.Round(p.newton, 2)} <br> &nbsp = {Pv2Center} kN/m² </th> ";

                    }

                    //考量多種情況，1.隧道是否跨越兩土層 2.兩土層是否為CL，依據兩種判斷有多種輸出方式
                    if (!crossLayer) //隧道無跨越兩土層
                    {
                        if (Top_SoilType == "CL") //土層為CL
                        {                            
                            outE1Top += outputE_CL(tunnelLevel, E1Top, Pv1Top);
                            outE2Bot += outputE_CL(tunnelBottomLevel, E2Bot, Pv2Bot);
                            outEavg += outputEavg(PL1, PL2, E1Top, E2Bot, Eavg);
                        }
                        else //土層非CL
                        {
                            outE1Top = "<tr> <th colspan='2'> 頂部彈性係數 E1 = ";
                            outE1Top += outputE_NonC(tunnelLevel, E1Top);
                            outE2Bot = "<tr> <th colspan='2'> 底部彈性係數 E2 = ";
                            outE2Bot += outputE_NonC(tunnelBottomLevel, E2Bot);
                            outEavg += outputEavg(PL1, PL2, E1Top, E2Bot, Eavg);
                        }                        
                    }
                    else if (Top_SoilType == "CL" && Bot_SoilType == "CL") //兩土層皆為CL且隧道跨越兩土層
                    {
                        outE1Center += outputE_CL(tunnelLevel, E1Center, Pv1Center);
                        outE2Center += outputE_CL(tunnelBottomLevel, E2Center, Pv2Center);
                        outEavg += outputEavg(PL1, PL2, E1Center, E2Center, Eavg);
                    }                    
                    else if(Top_SoilType == "CL")//上土層為CL，下土層非CL且隧道跨越兩土層
                    {
                        outE1Center += outputE_CL(tunnelLevel, E1Center, Pv1Center);
                        outE2Center = "<tr> <th colspan='2'> 下半層彈性係數 E2 = ";
                        outE2Center += outputE_NonC(tunnelBottomLevel, E2Center);
                        outEavg += outputEavg(PL1, PL2, E1Center, E2Center, Eavg);                      
                    }     
                    else if(Bot_SoilType == "CL")//上土層非CL，下土層為CL且隧道跨越兩土層
                    {
                        outE1Center = "<tr> <th colspan='2'> 上半層彈性係數 E1 = ";
                        outE1Center += outputE_NonC(tunnelLevel, E1Center);
                        outE2Center += outputE_CL(tunnelBottomLevel, E2Center, Pv2Center);
                        outEavg += outputEavg(PL1, PL2, E1Center, E2Center, Eavg);
                    }
                    else//兩土層皆非CL且隧道跨越兩土層
                    {
                        outE1Center = "<tr> <th colspan='2'> 上半層彈性係數 E1 = ";
                        outE1Center += outputE_NonC(tunnelLevel, E1Center);
                        outE2Center = "<tr> <th colspan='2'> 下半層彈性係數 E2 = ";
                        outE2Center += outputE_NonC(tunnelBottomLevel, E2Center);
                        outEavg += outputEavg(PL1, PL2, E1Center, E2Center, Eavg);
                    }

                    string outFinal = " </table> ";
                    switch (condition.ToUpper().ToString())
                    {
                        case "SHIELDMACHINE":
                            outputVerticalStress = $"{outbuildingload} <br> {outPvTop} {outPvBot} {outPh1} {outPh2} {outPv1Top} {outPv2Bot} {outPa1} {outPa2} {outFinal}";
                            break;
                        case "TUNNEL":
                            {
                                if(!crossLayer)
                                    outputVerticalStress = $"{outbuildingload} <br> {outPvTop} {outPh1} {outPh2} {outPv1Top} {outPv2Bot} {outE1Top} {outE2Bot} {outEavg} {outFinal}";
                                else
                                    outputVerticalStress = $"{outbuildingload} <br> {outPvTop} {outPh1} {outPh2} {outPv1Center} {outPv2Center} {outE1Center} {outE2Center} {outEavg} {outFinal}";
                            }break;
                        case "CONNECTOR":
                            {
                                if(!crossLayer)
                                    outputVerticalStress = $"{outbuildingload} <br> {outPvTop} {outPh1} {outPh2} {outPv1Top} {outPv2Bot} {outE1Top} {outE2Bot} {outEavg} {outFinal}";
                                else
                                    outputVerticalStress = $"{outbuildingload} <br> {outPvTop} {outPh1} {outPh2} {outPv1Center} {outPv2Center} {outE1Center} {outE2Center} {outEavg} {outFinal}";
                            }break;                            
                    }                                       
                                        
                    if (j == 0)
                    {
                        outputLongtermVerticalStress = outputVerticalStress;
                        PvBotString = $"{table} {outPvBot} {outFinal}";
                    }                        
                    else if (j == 1) outputShortermVerticalStress = outputVerticalStress;
                }
            }
            PvTop = PTop;
            PvBot = PBot;            

            longTermPa1 = Pa1[0];
            longTermPa2 = Pa2[0];

            LongTermPh1 = Ph1[0];
            LongTermPh2 = Ph2[0];
            ShortTermPh1 = Ph1[1];
            ShortTermPh2 = Ph2[1];

            connectorNPv = PTop;
            connectorNPh1 = Ph1[0];
            connectorNPh2 = Ph2[0];

            longtermPh1 = Ph1[0];
            longtermPh2 = Ph2[0];
            shortermPh1 = Ph1[1];
            shortermPh2 = Ph2[1];
            longTermSoilE = longtermE1;
            shortTermSoilE = shortermE1;
            //soilavgUW

            List<Tuple<bool, double, double>> layer = new List<Tuple<bool, double, double>>();
            layer.Add(Tuple.Create(crossLayer, PL1, PL2));
            return layer;
        }

        string outputE_CL(int tunnelLevel, double E, double Pv)
        {
            string str = "";
            str += $"{p.soilE_c} * Su = {p.soilE_c} * ({p.SL[tunnelLevel].Suδv} * {Pv}) <br>";
            str += $"&nbsp = {Math.Round(E, 0)} kN/m² </th>";
            str += $"<th> (Su = {p.SL[tunnelLevel].Suδv} Pv') </th>";
            return str;
        }
        string outputE_NonC(int tunnelLevel, double E)
        {
            string str = "";
            str += $"{p.soilE_Nonc} * N = {p.soilE_Nonc} * {p.SL[tunnelLevel].N} <br>";
            str += $" &nbsp = {Math.Round(E, 0)} kN/m² </th> ";
            return str;
        }
        string outputEavg(double PL1, double PL2, double E1, double E2, double Eavg)
        {
            string str = "";
            if (PL1 == PL2)
                str += $"({Math.Round(E1, 0)} + {Math.Round(E2, 0)}) / 2 = {Math.Round(Eavg, 0)} <br> ";
            else
                str += $"[({Math.Round(E1, 0)} * {PL1}) + ({Math.Round(E2, 0)} * {PL2})] / ({PL1} + {PL2}) <br> ";
            str += $"&nbsp = {Math.Round(Eavg, 0)} kN/m² </th>";
            return str;
        }
        
        void SurchargeLoadCalculation(string condition, out string outputSurchargeLoad, double coverDepth, double radiusOut)
        {
            int iCount = p.LD.Length;
            double[] gamma1 = new double[iCount];
            double[] alpha1 = new double[iCount];
            double[] gamma11 = new double[iCount];
            double[] alpha11 = new double[iCount];
            double[] verticalIncrement = new double[iCount];
            double[] lateralIncrement = new double[iCount];

            string outputAngle = "";
            string outStressIncrement = "";
            string outTrafficLoad;
            outputSurchargeLoad = "";

            for (int i = 0; i < iCount; i++)
            {                               
                gamma1[i] = Math.Atan(p.LD[i].X1 / coverDepth);
                alpha1[i] = Math.Atan((p.LD[i].X1 + p.LD[i].X2) / coverDepth) - gamma1[i];

                bool surchargeBool = true;
                switch (condition.ToUpper().ToString())
                {
                    case "SHIELDMACHINE":
                    case "TUNNEL":
                        {
                            gamma11[i] = Math.Atan((p.LD[i].X1 - radiusOut) / (coverDepth + radiusOut));
                            alpha11[i] = Math.Atan((p.LD[i].X1 + p.LD[i].X2 - radiusOut) / (coverDepth + radiusOut)) - gamma11[i];
                            if (p.LD[i].X1 < radiusOut) surchargeBool = false;
                        }
                        break;
                    case "CONNECTOR":
                        {
                            gamma11[i] = Math.Atan((p.LD[i].X1 - TR) / (coverDepth + TR));
                            alpha11[i] = Math.Atan((p.LD[i].X1 + p.LD[i].X2 - TR) / (coverDepth + TR)) - gamma11[i];
                            if (p.LD[i].X1 < TR) surchargeBool = false;
                        }
                        break;
                }                

                if (surchargeBool)
                {
                    verticalIncrement[i] = (p.LD[i].P / Math.PI) * (alpha1[i] + (Math.Sin(alpha1[i]) * Math.Cos(alpha1[i] + 2 * gamma1[i])));
                    lateralIncrement[i] = (p.LD[i].P / Math.PI) * (alpha11[i] - (Math.Sin(alpha11[i]) * Math.Cos(alpha11[i] + 2 * gamma11[i])));
                }
                verticalStress += verticalIncrement[i];
                lateralStress += lateralIncrement[i];
                                
                if (outputCondition.ToUpper().ToString() == "WEBFORM")
                {
                    outputAngle = $"<span style='font-size:20px;'> <b> 建物載重計算 </b> </span> <br> <table style='3px black solid;' cellpadding='8' border='5'> <tr> <th rowspan='3'> 頂拱部分  </th> <br> ";
                    outputAngle += $"<tr> <th> <font size='3'>γ</font>1 = tan<sup>-1</sup>(x1/z) = {Math.Round(gamma1[i], 3)} (rad) </th> ";
                    outputAngle += $"<th> &nbsp <font size='3'>γ</font>1 = {Math.Round(gamma1[i] * 180 / Math.PI, 3)}&#176 </th> ";
                    outputAngle += $"<tr> <th> <font size='4'>α</font>1 = tan<sup>-1</sup>[(x1 + x2)/z] - <font size='3'>γ</font>1 = {Math.Round(alpha1[i], 3)} (rad) </th> ";
                    outputAngle += $"<th> &nbsp <font size='4'>α</font>1 = {Math.Round(alpha1[i] * 180 / Math.PI, 3)}&#176 </th> ";
                    outputAngle += $"<tr> <th rowspan='3'> 起拱線部分 </th> ";
                    outputAngle += $"<tr> <th> <font size='3'>γ</font>11 = tan<sup>-1</sup>[(x1 - R)/(z + R)] = {Math.Round(gamma11[i], 3)} (rad) </th> ";
                    outputAngle += $"<th> &nbsp <font size='3'>γ</font>11 = {Math.Round(gamma11[i] * 180 / Math.PI, 3)}&#176 </th> ";
                    outputAngle += $"<tr> <th> <font size='4'>α</font>11 = tan<sup>-1</sup>[(x1 + x2 - R)/(z + R)] - <font size='3'>γ</font>11 = {Math.Round(alpha11[i], 3)} (rad) </th> ";
                    outputAngle += $"<th> &nbsp <font size='4'>α</font>11 = {Math.Round(alpha11[i] * 180 / Math.PI, 3)}&#176 </th> </table> ";

                    outStressIncrement = $"<table style='3px: blaci solid; text-align:left' cellpadding='8' border='5'> <tr> <th colspan='2' style='text-align:center'> 隧道頂拱垂直應力增量 </th> <br> ";
                    outStressIncrement += $"<tr> <th> <font size='5'>σ</font>z = Σ(P/π) * (<font size='4'>α</font>1 + sin(<font size='4'>α</font>1) * cos(<font size='4'>α</font>1 + 2<font size='3'>γ</font>1) <br> = ({p.LD[i].P}/π) * ({Math.Round(alpha1[i], 3)} + sin{Math.Round(alpha1[i] * 180 / Math.PI, 3)}&#176 * cos({Math.Round(alpha1[i] * 180 / Math.PI, 3)}&#176 + 2 * ({Math.Round(gamma1[i] * 180 / Math.PI, 3)}&#176)) </th> ";
                    outStressIncrement += $"<th> &nbsp <font size='5'>σ</font>z = {Math.Round(verticalIncrement[i], 2)}kN/m&#178 </th> ";
                    outStressIncrement += $"<tr> <th colspan='2' style='text-align:center'> 隧道起拱線側向應力增量 </th> ";
                    outStressIncrement += $"<tr> <th> <font size='5'>σ</font>h = Σ(P/π) * (<font size='4'>α</font>11 - sin(<font size='4'>α</font>11) * cos(<font size='4'>α</font>11 + 2<font size='3'>γ</font>11) <br> = ({p.LD[i].P}/π) * ({Math.Round(alpha11[i], 3)} - sin{Math.Round(alpha11[i] * 180 / Math.PI, 3)}&#176 * cos({Math.Round(alpha11[i] * 180 / Math.PI, 3)}&#176 + 2 * ({Math.Round(gamma11[i] * 180 / Math.PI, 3)}&#176)) </th> ";
                    outStressIncrement += $"<th> &nbsp <font size='5'>σ</font>h = {Math.Round(lateralIncrement[i], 2)}kN/m&#178 </th> </table> <br> ";

                    outputSurchargeLoad += $"{outputAngle}  {outStressIncrement}";                    
                }
            }                     

            double vehicleWeight = 320;
            double vehicleWidth = 3;
            double vehicleLength = 10;

            int vehicleCapacity = (int)(p.roadWidth / vehicleWidth);
            double B = coverDepth + p.roadWidth;
            double unitWidthLoad = vehicleWeight * vehicleCapacity / (vehicleLength * p.roadWidth);
            trafficStress = unitWidthLoad * p.roadWidth / B;

            if (outputCondition.ToUpper().ToString() == "WEBFORM")
            {
                outTrafficLoad = $"<table style='border:3px black solid;' cellpadding='8' border='5'> ";
                outTrafficLoad += $"<tr> <th colspan='2'> 車輛載重計算 </th> ";
                outTrafficLoad += $"<tr> <th> 隧道頂拱深度 </th> <th> {coverDepth}m </th> ";
                outTrafficLoad += $"<tr> <th> 路寬 </th> <th> {p.roadWidth}m </th> ";
                outTrafficLoad += $"<tr> <th colspan='2'> 依 HS-20-44 規範，單一車重320kN, 車寬3m, 車長10m </th> ";
                outTrafficLoad += $"<tr> <th> 可停 </th> <th> {vehicleCapacity}部車 </th> ";
                outTrafficLoad += $"<tr> <th> B：路寬 + 頂拱深 </th> <th> {p.roadWidth + coverDepth} </th> ";
                outTrafficLoad += $"<tr> <th> 單位寬度荷重：(320 * {vehicleCapacity})/(10 * {p.roadWidth}) </th> <th> {Math.Round(unitWidthLoad, 2)}kN/m&#178 </th> ";
                outTrafficLoad += $"<tr> <th> 單位寬度地層荷重(<font size='5'>σ</font>z)：(路面荷重 * 路寬)/B = {STRFraction($"{Math.Round(unitWidthLoad, 2)} * {p.roadWidth}", $"{B}")} </th> <th> {Math.Round(trafficStress, 2)}kN/m&#178 </th> ";
                outTrafficLoad += $"</table> ";
                outputSurchargeLoad += outTrafficLoad;
            }
        }      
        
        string STRFraction(string numerator, string denominator)
        {
            return $"<div class='frac'><span>{numerator}</span><span class='symbol'>/</span><span class='bottom'>{denominator}</span></div>";
        }
    }
}
