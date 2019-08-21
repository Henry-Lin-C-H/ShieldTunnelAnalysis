using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace SinoTunnel
{
    class SAP_EQCalculation
    {
        string sectionUID;        

        GetWebData p;
        ExcuteSQL datasearch = new ExcuteSQL();


        public SAP_EQCalculation(string sectionUID)
        {
            this.sectionUID = sectionUID;
            p = new GetWebData(sectionUID);
            
        }


        public double EQVariationCal()
        {

            STN_VerticalStress soilData = new STN_VerticalStress(sectionUID, "No");
            soilData.VerticalStress("TUNNEL", out string test01, out string test02, out string test03, out double lontTermE1, out double shortTermE1, out double Pv, out double longTermPh1, out double longTermPh2, out double shortTermPh1, out double shortTermPh2, out double U12);

            double eqVariation = 2 * p.segmentRadiusInter * p.MDEVh /100 * (1 - U12) / p.shearwaveV;
            return eqVariation;
        }

    }
}
