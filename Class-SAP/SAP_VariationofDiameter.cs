using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SinoTunnel
{
    class SAP_VariationofDiameter
    {
        string sectionUID;
        GetWebData p;

        public SAP_VariationofDiameter(string sectionUID)
        {
            this.sectionUID = sectionUID;
            p = new GetWebData(sectionUID);      
        }

        public double VariationCal()
        {
            double variationPercent = 0.33;
            double diameterInter = p.segmentRadiusInter * 2;//公尺(m)
            double targetVariation = diameterInter * variationPercent / 100;//公尺 * 比例 * 百分比 :單位為公尺
            
            return targetVariation;
        }
    }
}
