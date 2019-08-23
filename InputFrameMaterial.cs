using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTunnel
{
    public class InputFrameMaterial
    {
        GetWebData p;
        string sectionUID;
        ExcuteSQL oExecuteSQL = new ExcuteSQL();

        string depth;
        string diameter;
        float width;
        float unitWeight;
        float Fc;
        float E;
        float U12;
        float contactDepth;

        List<string> inputDepth;

        public InputFrameMaterial(string sectionUID)
        {
            this.sectionUID = sectionUID;
            this.p = new GetWebData(sectionUID);

            this.depth = Math.Round(p.segmentThickness, 4).ToString();
            this.diameter = Math.Round(p.segmentThickness, 4).ToString();
            this.width = (float)p.segmentWidth;
            this.unitWeight = (float)p.segmentUnitWeight;
            this.Fc = (float)p.segmentFc;
            this.E = (float)p.segmentYoungsModulus;
            this.U12 = (float)p.segmentPoissonRatio;
            this.contactDepth = (float)p.segmentContacingDepth;
            this.inputDepth = new List<string> {depth, (contactDepth - 0.0225f).ToString(),
                (contactDepth - 0.0125f).ToString(), contactDepth.ToString() };
        }

        public void Precast()
        {
            Delete("Precast");
            Delete("Grouting");

            for(int i = 0; i < inputDepth.Count; i++)
            {
                oExecuteSQL.InsertFrameMaterial(sectionUID, inputDepth[i], "Concrete", "Precast", i.ToString());
            }
            oExecuteSQL.InsertFrameMaterial(sectionUID, depth, "Concrete", "Grouting", 0.ToString());            
        }

        public void Connector()
        {
            Delete("Connector");

            oExecuteSQL.InsertFrameMaterial(sectionUID, 0.3.ToString(), "Concrete", "Connector", "TR");
            oExecuteSQL.InsertFrameMaterial(sectionUID, 0.4.ToString(), "Concrete", "Connector", "BH");
        }

        public void Steel()
        {
            Delete("Steel");

            for(int i = 0; i < inputDepth.Count; i++)
            {
                oExecuteSQL.InsertFrameMaterial(sectionUID, inputDepth[i], "Concrete", "Steel", i.ToString());
            }
            double depth = 0.03;
            oExecuteSQL.InsertFrameMaterial(sectionUID, depth.ToString(), "A36", "Steel", "Origin");
            oExecuteSQL.InsertFrameMaterial(sectionUID, depth.ToString(), "A36", "Steel", "Cut");

        }

        public void Site()
        {
            Delete("Site");

            oExecuteSQL.InsertFrameMaterial(sectionUID, depth, "Concrete", "Site", 0.ToString());
        }

        void Delete(string loadType)
        {
            try { oExecuteSQL.DeleteDataBySectionUIDAndLoadType("STN_FrameMaterial", sectionUID, loadType); }
            catch { }
        }

    }
}
