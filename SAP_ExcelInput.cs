using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace SinoTunnel
{
    class SAP_ExcelInput
    {    //資料置入EXCEL
        string filePath = "";
        IWorkbook wb;
        ISheet ws;
        int i;
        //int j;
        //int k;
        int m;
        public double newton = 9.80665;

        //string load;
        //string material;
        string frame;
        //string joint;
        //string link;

        public SAP_ExcelInput(string filePath)
        {
            this.filePath = filePath;
            FileStream fileread = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            this.wb = new XSSFWorkbook(fileread);
        }

        public SAP_ExcelInput()
        {

        }
        

        #region Load
        public void PutDataToSheet(string SheetName, List<string> srtData) 
        {            
            ws = wb.GetSheet(SheetName);
            m = ws.LastRowNum;
            for (i = 0; i < srtData.Count; i++)
                SetRowData(ws, m + i + 1 ,srtData[i]);
        }
        #endregion

        #region Frame&Section
        public void FrameProp01_Genearl(List<Tuple<string, string, string, double, double>> section)
        {
            frame = "Frame Props 01 - General";
            ws = wb.GetSheet(frame);
            m = ws.LastRowNum;
            for (i = 0; i < section.Count; i++)
            {
                if (section[i].Item3 == "Rectangular")
                    SetRowData(ws, m + i + 1, $"{section[i].Item1},{section[i].Item2},{section[i].Item3},{section[i].Item4},{section[i].Item5},,,,,,,,,,,,,,,,,Yes,No,,,,No");
                else if (section[i].Item3 == "Circle")
                    SetRowData(ws,m+i+1,$"{section[i].Item1},{section[i].Item2},{section[i].Item3},{section[i].Item4},,,,,,,,,,,,,,,,,,Yes,No,,,,No");
            }                
        }

        public void FrameProp02_Concrete(List<Tuple<string, string>> concreteSection)
        {
            frame = "Frame Props 02 - Concrete Col";
            ws = wb.GetSheet(frame);
            m = ws.LastRowNum;
            for (i = 0; i < concreteSection.Count; i++)
            {
                if (concreteSection[i].Item2 == "Rectangular")
                    SetRowData(ws, m + i + 1, $"{concreteSection[i].Item1},A615Gr60,A615Gr60,{concreteSection[i].Item2},Ties,{0.04},{3},{3},,#9,#4,{0.15},{3},{3},Design");
                else if (concreteSection[i].Item2 == "Circle")
                    SetRowData(ws, m + i + 1, $"{concreteSection[i].Item1},A615Gr60,A615Gr60,Circular,Ties,{0.04},,,8,#9,#4,{0.15},,,Design");
            }                
        }      

        public void FrameRelease(List<string> frameProp, List<Tuple<string,string,string,string,string,string>> formerSide, List<Tuple<string,string,string,string,string,string>> latterSide)
        {
            frame = "Frame Releases 1 - General";
            ws = wb.GetSheet(frame);
            m = ws.LastRowNum;
            for(i = 0; i < frameProp.Count; i++)
                SetRowData(ws, m + i + 1, $"{frameProp[i]},{formerSide[i].Item1},{formerSide[i].Item2},{formerSide[i].Item3},{formerSide[i].Item4},{formerSide[i].Item5},{formerSide[i].Item6},{latterSide[i].Item1},{latterSide[i].Item2},{latterSide[i].Item3},{latterSide[i].Item4},{latterSide[i].Item5},{latterSide[i].Item6},No");
        }
        #endregion  
        

        public void FileSaving(string path)
        {
            FileStream fileSave = new FileStream(path, FileMode.Create);
            wb.Write(fileSave);
            fileSave.Close();
        }

        public void FileSaving(IWorkbook insertWB, string path)
        {
            FileStream fileSave = new FileStream(path, FileMode.Create);
            insertWB.Write(fileSave);
            fileSave.Close();
        }

        public void CreateResultFile(IWorkbook resultWB, List<Tuple<int, string, int>> SAPProp, List<Tuple<double,double,double,double,double,double>> SAPValue, string sheetName)
        {            
            ISheet resultWS = resultWB.CreateSheet(sheetName);            
            SetRowData(resultWS, 0, "Member,Load,JT,Axial,Shear-Y,Shear-Z,Torsion,Moment-Y,Moment-Z");            
            for (i = 0; i < SAPValue.Count; i++)
            {
                SetRowData(resultWS, i + 1, $"{SAPProp[i].Item1},{SAPProp[i].Item2},{SAPProp[i].Item3},{SAPValue[i].Item1},{SAPValue[i].Item2},{SAPValue[i].Item3},{SAPValue[i].Item4},{SAPValue[i].Item5},{SAPValue[i].Item6}");                                
            }            
        }

        public void CreateResultFile(IWorkbook resultWB, List<Tuple<string, string, string>> SAPProp, List<Tuple<double, double, double, double, double, double>> SAPValue, string sheetName)
        {
            ISheet resultWS = resultWB.CreateSheet(sheetName);
            SetRowData(resultWS, 0, "Member,Load,JT,Axial,Shear-Y,Shear-Z,Torsion,Moment-Y,Moment-Z");
            for (i = 0; i < SAPValue.Count; i++)
            {
                SetRowData(resultWS, i + 1, $"{SAPProp[i].Item1},{SAPProp[i].Item2},{SAPProp[i].Item3},{SAPValue[i].Item1},{SAPValue[i].Item2},{SAPValue[i].Item3},{SAPValue[i].Item4},{SAPValue[i].Item5},{SAPValue[i].Item6}");
            }
        }

        public void CreateDisplacementFile(IWorkbook resultWB, List<Tuple<string, string>> SAPProp, List<Tuple<double, double, double, double, double, double>> SAPValue, string sheetName)
        {
            ISheet resultWS = resultWB.CreateSheet(sheetName);
            SetRowData(resultWS, 0, "Joint,Load,U1(X)-m,U2(Y)-m,U3(Z)-m,R1-radius,R2-radius,R3-radius");
            for (i = 0; i < SAPValue.Count; i++)
            {
                SetRowData(resultWS, i + 1, $"{SAPProp[i].Item1},{SAPProp[i].Item2},{SAPValue[i].Item1},{SAPValue[i].Item2},{SAPValue[i].Item3},{SAPValue[i].Item4},{SAPValue[i].Item5},{SAPValue[i].Item6}");
            }
        }

        #region void
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rid">ROW</param>
        /// <param name="cid">COLUMN 欄位</param>
        /// <param name="value"></param>
        /// <param name="SetText"></param>
        void SetValue(ISheet ws, int rid, int cid, string value, bool SetText = false)
        {
            IRow row = ws.GetRow(rid);
            if (row == null)
                row = ws.CreateRow(rid);
            ICell cell = row.GetCell(cid);
            if (cell == null)
                cell = row.CreateCell(cid);

            if (SetText)
            {
                //強制文字輸出
                cell.SetCellType(CellType.String);
                cell.SetCellValue(value);
            }
            else
            {
                double output;
                if (double.TryParse(value, out output))
                    cell.SetCellValue(output);
                else
                {
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(value);
                }
            }
        }

        /// <summary>
        /// 資料置入EXCEL CELL
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rid">ROW</param>
        /// <param name="data"></param>
        void SetRowData(ISheet ws, int rid, string data)
        {
            string[] values = data.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                SetValue(ws, rid, i, values[i], false);
            }
        }
        #endregion
    }
}
