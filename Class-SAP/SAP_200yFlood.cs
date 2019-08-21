using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SinoTunnel
{
    public class SAP_200yFlood
    {
        string sectionUID;

        GetWebData p;
        ExcuteSQL dataSearch = new ExcuteSQL();

        public SAP_200yFlood(string sectionUID)
        {
            this.sectionUID = sectionUID;  
            p = new GetWebData(sectionUID); 
        }

        public void SAP200yFloodCal(out string error)
        {
            error = "";
            try
            {
                DataTable section = dataSearch.GetByUID("STN_Section", sectionUID);
                string longTermUID = section.Rows[0]["LLI4_ChosenData"].ToString();
                string shortTermUID = section.Rows[0]["SLI4_ChosenData"].ToString();

                DataTable longTermData = dataSearch.GetByUID("STN_LongTermData", longTermUID);
                DataTable shortTermData = dataSearch.GetByUID("STN_ShortTermData", shortTermUID);

                double longTermWL = p.groundEle - p.GWL;
                double WL = p.floodEle - longTermWL;
                double increasedStress = WL * p.newton * (p.segmentRadiusOut * 2) * p.segmentWidth / 2;

                double longTermM = double.Parse(longTermData.Rows[0]["MomentZ"].ToString());
                double longTermAxial = Math.Abs(double.Parse(longTermData.Rows[0]["Axial"].ToString())) + increasedStress;
                double longTermShear = Math.Abs(double.Parse(longTermData.Rows[0]["ShearY"].ToString()));

                double shortTermWL = p.groundEle;
                double shortWL = p.floodEle - p.groundEle;
                double shortStress = shortWL * p.newton * (p.segmentRadiusOut * 2) * p.segmentWidth / 2;

                double shortTermM = double.Parse(shortTermData.Rows[0]["MomentZ"].ToString());
                double shortTermAxial = Math.Abs(double.Parse(shortTermData.Rows[0]["Axial"].ToString())) + shortStress;
                double shortTermShear = Math.Abs(double.Parse(shortTermData.Rows[0]["ShearY"].ToString()));

                List<string> condition = new List<string> { "LongTerm", "ShortTerm" };
                List<string> inputUID = new List<string>();
                List<Tuple<string, string, string>> floodProp = new List<Tuple<string, string, string>>();
                List<Tuple<double, double, double, double, double, double>> floodData = new List<Tuple<double, double, double, double, double, double>>();

                for (int i = 0; i < 2; i++)
                {
                    inputUID.Add(Guid.NewGuid().ToString("D"));
                    floodProp.Add(Tuple.Create("", condition[i], ""));
                }
                List<string> floodSection = new List<string> { "FloodL", "FloodS" };
                for (int i = 0; i < 2; i++)
                {
                    dataSearch.UpdateData("STN_Section", "UID", sectionUID, floodSection[i], inputUID[i]);
                }
                floodData.Add(Tuple.Create(longTermAxial, longTermShear, 0.0, 0.0, 0.0, longTermM));
                floodData.Add(Tuple.Create(shortTermAxial, shortTermShear, 0.0, 0.0, 0.0, shortTermM));

                try
                {
                    dataSearch.DeleteDataBySectionUID("STN_200yFloodData", sectionUID);
                }
                catch
                {
                }
                dataSearch.InsertSAPData("STN_200yFloodData", inputUID, sectionUID, floodProp, 0, floodData);
            }
            catch
            {
                error = "請先完成長期與短期載重計算";
            }

            

        }
    }
}
