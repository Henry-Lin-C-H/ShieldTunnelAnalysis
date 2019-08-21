using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using STN_SQL;
using System.Data;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows.Forms.DataVisualization.Charting;

namespace SinoTunnel
{
    class SGCalculationDepth
    {
        string sectionUID = "";
        ExcuteSQL dataSearch = new ExcuteSQL();
        GetWebData p;

        List<string> inputUID = new List<string>();
        //List<Tuple<string, int, string, string>> figProfile = new List<Tuple<string, int, string, string>>();

        public SGCalculationDepth(string SectionUID)
        {
            this.sectionUID = SectionUID;
            p = new GetWebData(sectionUID);
        }

        public List<Tuple<string, string, int, string, double, double, double>> FigureShow(string condition, out int figNum, out List<Tuple<string, string>> figProfile)
        {
            figNum = 0;
            DataTable figData = dataSearch.GetBySection("STN_SGCalDepth", sectionUID, "");

            List<Tuple<string, string, int, string, double, double, double>> outFigData = new List<Tuple<string, string, int, string, double, double, double>>();

            //[Times of Load, DataUID, ContactDepth, UID]
            List<Tuple<int, string, double, string>> dataUID = new List<Tuple<int, string, double, string>>();
            figProfile = new List<Tuple<string, string>>();

            for (int i = 0; i < figData.Rows.Count; i++)
            {
                if (figData.Rows[i]["LoadType"].ToString() == condition && 
                    int.Parse(figData.Rows[i]["TimesOfLoad"].ToString()) == 1)
                {
                    figProfile.Add(Tuple.Create(figData.Rows[i]["Member"].ToString(), figData.Rows[i]["Joint"].ToString()));
                }
            }
            figProfile = figProfile.OrderBy(t => t.Item1).ToList();
            //figProfile.Sort();

            for (int j = 0; j < figProfile.Count; j++)
            {
                for (int k = 0; k < figData.Rows.Count; k++)
                {
                    if (figProfile[j].Item1 == figData.Rows[k]["Member"].ToString() && 
                        condition == figData.Rows[k]["LoadType"].ToString())
                    {
                        if(int.Parse(figData.Rows[k]["TimesOfload"].ToString()) < 4)
                        {
                            var data = Tuple.Create(int.Parse(figData.Rows[k]["TimesOfLoad"].ToString()), 
                                figData.Rows[k]["DataUID"].ToString(), 
                                double.Parse(figData.Rows[k]["ContactDepth"].ToString()), 
                                figData.Rows[k]["UID"].ToString());
                            dataUID.Add(data);
                        }                        
                    }
                }
            }
            dataUID = dataUID.OrderBy(t => t.Item1).ToList();

            DataTable tempTable;
            //DataTable tempSegment;
            double e;
            double tempM;
            double M;
            double depth;
            string member;
            int times;

            switch (condition)
            {
                case "Steel_Origin":
                case "Steel_Cut":
                    condition = "Steel";
                    break;
            }

            for (int i = 0; i < dataUID.Count; i++)
            {
                tempTable = dataSearch.GetByUID($"STN_{condition}Data", dataUID[i].Item2);
                //tempSegment = dataSearch.GetByUID("STN_Segment", dataUID[i].Item3);
                //depth = double.Parse(tempSegment.Rows[0]["Height"].ToString());
                depth = dataUID[i].Item3;
                e = (p.segmentContacingDepth / 2) - (depth / 3);
                tempM = Math.Abs(double.Parse(tempTable.Rows[0]["Axial"].ToString())) * e;

                M = Math.Abs(double.Parse(tempTable.Rows[0]["MomentZ"].ToString()));
                member = tempTable.Rows[0]["Member"].ToString();

                times = int.Parse(tempTable.Rows[0]["Times"].ToString());

                var data = Tuple.Create(dataUID[i].Item4, condition, times, member, depth, Math.Round(tempM, 2), 
                    Math.Round(M, 2));
                outFigData.Add(data);
                figNum += 1;
            }

            return outFigData;
        }

        public void UpdateFinalDepth(List<Tuple<string, double>> finalDepth)
        {
            for(int i = 0; i < finalDepth.Count; i++)
            {
                dataSearch.UpdateData("STN_SGCalDepth", "UID", finalDepth[i].Item1, "FinalDepth", finalDepth[i].Item2);
            }
        }

        public void LinearRegression(List<double> x, List<double> y, out double slope, out double constant)
        {
            //y = slope * x + constant
            double avgX = 0;
            double avgY = 0;
            double numerator = 0;
            double denominator = 0;
            double residual_SS = 0;
            double regression_SS = 0;

            for (int i = 0; i < x.Count; i++)
            {
                avgX += x[i];
                avgY += y[i];
            }
            avgX /= x.Count;
            avgY /= x.Count;
            for (int i = 0; i < x.Count; i++)
            {
                numerator += (x[i] - avgX) * (y[i] - avgY);
                denominator += (x[i] - avgX) * (x[i] - avgX);
            }
            slope = numerator / denominator;
            constant = avgY - slope * avgX;
            for (int i = 0; i < x.Count; i++)
            {
                residual_SS += Math.Pow(y[i] - constant - slope * x[i], 2);//剩餘平方和
                regression_SS += Math.Pow(constant + slope * x[i] - avgY, 2);//回歸平方和
            }
        }

        public void Fig(LiveCharts.WinForms.CartesianChart fig, List<double> xAxis, List<double> sap, List<double> cal)
        {
            fig.Series.Clear();
            fig.Series = new LiveCharts.SeriesCollection()
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(xAxis[0], cal[0]),
                        new ObservablePoint(xAxis[1], cal[1]),
                        new ObservablePoint(xAxis[2], cal[2]),
                    },
                    Title = "M = N(T/2 - t/3)",
                    DataLabels = true,
                    //LabelPoint = point => point.X.ToString(),
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(xAxis[0], sap[0]),
                        new ObservablePoint(xAxis[1], sap[1]),
                        new ObservablePoint(xAxis[2], sap[2]),
                    },
                    Title = "SAP2000",
                    DataLabels = true,
                },
            };
            fig.AxisX.Clear();
            fig.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "接觸深度(mm)",                
                Separator = new Separator { Step = 0.01, IsEnabled = false },                
                //ShowLabels = true,
                //Labels = new[] { "0.14", "0.15", "0.1625" },
                //Labels = new[] { xAxis[0].ToString(), xAxis[1].ToString(), xAxis[2].ToString() },
            });
            fig.AxisY.Clear();
            fig.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "彎矩(kN-m)"
            });
        }

        //public void InsertFinalDepthData(List<double> finalDepth)
        //{
        //    double initialContactingDepth = p.segmentContacingDepth;

        //    //抓一開始採用的環片資料，作為後續若要新增環片資料的參考值
        //    DataTable dt = dataSearch.GetByUID("STN_Section", sectionUID);
        //    string segmentUID = dt.Rows[0]["Load_Segment"].ToString();

        //    DataTable segment = dataSearch.GetByUID("STN_Segment", segmentUID);
        //    string materialUID = segment.Rows[0]["Material"].ToString();
        //    double segmentHeight = double.Parse(segment.Rows[0]["Height"].ToString());
        //    DataTable segmentMaterial = dataSearch.GetByUID("STN_SegmentMaterial", materialUID);
        //    string materialName = segmentMaterial.Rows[0]["MaterialName"].ToString();
        //    double materialUT = double.Parse(segmentMaterial.Rows[0]["UnitWeight"].ToString());
        //    double youngsModulus = double.Parse(segmentMaterial.Rows[0]["YoungModulus"].ToString());
        //    double poissonRatio = double.Parse(segmentMaterial.Rows[0]["PoissonRatio"].ToString());
        //    double CTE = double.Parse(segmentMaterial.Rows[0]["CTE"].ToString());
        //    double Fc = double.Parse(segmentMaterial.Rows[0]["Fc"].ToString());

        //    DataTable allMaterial = dataSearch.GetByProject("STN_SegmentMaterial", p.projectUID);
        //    List<double> existingUT = new List<double>();
        //    for(int i = 0; i < allMaterial.Rows.Count; i++)
        //    {
        //        existingUT.Add(double.Parse(allMaterial.Rows[i]["UnitWeight"].ToString()));                
        //    }

        //    List<double> inputDepth = new List<double>();
        //    for(int i = 0; i < finalDepth.Count; i++)
        //    {
        //        if (!inputDepth.Contains(finalDepth[i])) inputDepth.Add(finalDepth[i]);
        //    }

        //    //將新的接觸深度輸入sql的material
        //    List<string> materialInputUID = new List<string>();            
        //    List<bool> revised = new List<bool>();
        //    List<double> finalInputDepth = new List<double>();
        //    List<Tuple<string, string, double, double, double, double, double>> materialProp = new List<Tuple<string, string, double, double, double, double, double>>();
        //    for(int i = 0; i < inputDepth.Count; i++)
        //    {
        //        bool tempBool = true;
        //        double tempUT = 0;
        //        for(int j = 0; j < existingUT.Count; j++)
        //        {
        //            tempUT = Math.Round(materialUT * (segmentHeight / inputDepth[i]), 2);
        //            if (existingUT[j] == tempUT) tempBool = false;
        //        }
        //        if (tempBool)
        //        {
        //            materialInputUID.Add(Guid.NewGuid().ToString("D"));
                    
        //            var data = Tuple.Create($"{materialName} for t = {inputDepth[i] * 100}cm", "Concrete", tempUT, youngsModulus, poissonRatio, CTE, Fc);
        //            materialProp.Add(data);
        //            finalInputDepth.Add(inputDepth[i]);
                    
        //        }
        //    }
        //    dataSearch.InsertMaterialData(materialInputUID, p.projectUID, materialProp);

        //    //將新的接觸深度的material輸入frame中
        //    List<Tuple<string, string, string, double, double>> frameData = new List<Tuple<string, string, string, double, double>>();
        //    inputUID.Clear();

        //    DataTable segmentProp = dataSearch.GetBySection("STN_Segment", sectionUID, "");             
        //    for(int i = 0; i < allMaterial.Rows.Count; i++)
        //    {
        //        if(double.Parse(allMaterial.Rows[i]["UnitWeight"].ToString()) == 0)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            bool tempbool = true;
        //            double tempH = (segmentHeight * materialUT) / double.Parse(allMaterial.Rows[i]["UnitWeight"].ToString());
        //            for (int j = 0; j < segmentProp.Rows.Count; j++)
        //            {
        //                if (segmentProp.Rows[j]["Shape"].ToString() == "Circle") continue;
        //                else if (Math.Abs(double.Parse(segmentProp.Rows[j]["Height"].ToString()) - tempH) < 1E-4) tempbool = false;
        //            }
        //            if (tempbool)
        //            {
        //                inputUID.Add(Guid.NewGuid().ToString("D"));
        //                var data = Tuple.Create($"Segment {p.segmentWidth * 100 / 2}x{Math.Round(tempH, 3) * 100}cm", allMaterial.Rows[i]["UID"].ToString(), "Rectangular", p.segmentWidth / 2, Math.Round(tempH, 3));
        //                frameData.Add(data);
        //            }
        //        }                
        //    }
        //    dataSearch.InsertFrameData(inputUID, sectionUID, frameData);
        //}       

        //public void PutSegmentUIDtoFinalDepth(string condition, int times)
        //{
        //    DataTable sgCalDepth = dataSearch.GetBySection("STN_SGCalDepth", sectionUID, "");
        //    DataTable segment = dataSearch.GetBySection("STN_Segment", sectionUID, "");
        //    for(int i = 0; i < sgCalDepth.Rows.Count; i++)
        //    {
        //        if(condition == sgCalDepth.Rows[i]["LoadType"].ToString() && 
        //            int.Parse(sgCalDepth.Rows[i]["TimesOfLoad"].ToString()) == times)
        //        {
        //            for(int j = 0; j < segment.Rows.Count; j++)
        //            {
        //                if (segment.Rows[j]["Shape"].ToString() == "Rectangular" && 
        //                    double.Parse(segment.Rows[j]["Height"].ToString()) == 
        //                    double.Parse(sgCalDepth.Rows[i]["FinalDepth"].ToString()))
        //                {
        //                    string itemUID = sgCalDepth.Rows[i]["UID"].ToString();
        //                    string segmentUID = segment.Rows[j]["UID"].ToString();
        //                    dataSearch.UpdateData("STN_SGCalDepth", "UID", itemUID, "SegmentUID", segmentUID);
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
