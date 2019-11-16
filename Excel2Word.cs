using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Formatting;
using Spire.Xls;
using Spire.Xls.Collections;
using Spire.Xls.Core;

namespace SinoTunnel
{
    class Excel2Word
    {
        Document doc;
        ParagraphStyle style;
        string tempPath = @"O:\ADMIN\5028Z-3D自動化設計(II) - 潛盾隧道工程SinoTunnel\09-軟體\SinoTunnel_WinForm\template.docx";
        int numcol = 7; // how much column in a split table
        public Excel2Word()
        {
            //Initialize an instance of Document class and load template from file 
            this.doc = new Document();
            doc.LoadFromFile(tempPath);

            // Set Font Style and Size
            style = new ParagraphStyle(doc);
            style.Name = "FontStyle";
            style.CharacterFormat.FontName = "Times New Roman";
            style.CharacterFormat.FontSize = 12;
            doc.Styles.Add(style);
        }
        // Add table to word
        #region converting tables
        public void Add(string filepath, List<string> names, bool boolResult, bool boolSteel)
        {
            //Initialize an instance of Workbook class and load excel from file 
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(filepath);

            //Get all worksheets
            WorksheetsCollection worksheets = workbook.Worksheets;
            Section section1 = doc.Sections[0];
            Worksheet sheet;
            Paragraph title;
            Table table;
            List<int> ColumnList = new List<int>();

            // Fit result's column in one table
            if (boolResult)
            {
                numcol = 99;
                names.Clear();
                foreach (var item in worksheets) names.Add(item.Name);
            }

            foreach (var name in names)
            {
                int iter = 0;
                foreach (Worksheet item in worksheets)
                {
                    if (name == item.Name)
                    {
                        //Get the worksheet
                        sheet = workbook.Worksheets[iter];
                        // Get which column to export
                        ColumnList.Clear();
                        if (!boolResult) ColumnList = GetColumnList(name, boolSteel);
                        else for (int i = 1; i <= sheet.LastColumn; i++) ColumnList.Add(i);

                        if (ColumnList.Count <= numcol) // 1 part
                        {
                            //Write Name of Worksheet
                            title = section1.AddParagraph();
                            title.Text = item.Name;
                            title.ApplyStyle(style.Name);

                            //Write Table
                            table = section1.AddTable(true);
                            table.ResetCells(sheet.LastRow, ColumnList.Count);
                            if (!boolResult) table.ApplyHorizontalMerge(0, 0, (ColumnList.Count - 1));

                            //Traverse the rows and columns of table in worksheet and get the cells, call a custom function CopyStyle() to copy the font style and cell style from Excel to Word table. 
                            for (int r = 1; r <= sheet.LastRow; r++)
                            {
                                for (int c = 1; c <= ColumnList.Count; c++)
                                {
                                    int c2 = ColumnList[c - 1];
                                    CellRange xCell = sheet.Range[r, c2];
                                    TableCell wCell = table.Rows[r - 1].Cells[c - 1];
                                    //Fill data to Word table 
                                    TextRange textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                    //Copy the formatting of table to Word 
                                    CopyStyle(textRange, xCell, wCell);
                                }
                            }
                            //Set column width of Word table in Word 
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                                {
                                    table.Rows[i].Cells[j].Width = 100f;
                                }
                            }

                            //NoBreakTable(table);
                        }
                        else if (ColumnList.Count < 2 * numcol - 1) // 2 part
                        {
                            //Part 1 of 2
                            title = section1.AddParagraph();
                            title.Text = item.Name + " Part 1 of 2";
                            title.ApplyStyle(style.Name);

                            //Write Table
                            table = section1.AddTable(true);
                            table.ResetCells(sheet.LastRow, numcol);
                            if (!boolResult) table.ApplyHorizontalMerge(0, 0, numcol - 1);
                            //Traverse the rows and columns of table in worksheet and get the cells, call a custom function CopyStyle() to copy the font style and cell style from Excel to Word table. 
                            for (int r = 1; r <= sheet.LastRow; r++)
                            {
                                for (int c = 1; c <= numcol; c++)
                                {
                                    int c2 = ColumnList[c - 1];
                                    CellRange xCell = sheet.Range[r, c2];
                                    TableCell wCell = table.Rows[r - 1].Cells[c - 1];
                                    //Fill data to Word table 
                                    TextRange textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                    //Copy the formatting of table to Word 
                                    CopyStyle(textRange, xCell, wCell);
                                }
                            }
                            //Set column width of Word table in Word 
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                                {
                                    table.Rows[i].Cells[j].Width = 100f;
                                }
                            }

                            //NoBreakTable(table);

                            //Part 2 of 2
                            title = section1.AddParagraph();
                            title.Text = item.Name + " Part 2 of 2";
                            title.ApplyStyle(style.Name);

                            //Write Table
                            table = section1.AddTable(true);
                            table.ResetCells(sheet.LastRow, ColumnList.Count - numcol + 1); // first column and the rest
                            if (!boolResult) table.ApplyHorizontalMerge(0, 0, ColumnList.Count - numcol);
                            //Traverse the rows and columns of table in worksheet and get the cells, call a custom function CopyStyle() to copy the font style and cell style from Excel to Word table. 
                            for (int r = 1; r <= sheet.LastRow; r++)
                            {
                                //for c = 1
                                CellRange xCell = sheet.Range[r, 1];
                                TableCell wCell = table.Rows[r - 1].Cells[0];
                                //Fill data to Word table 
                                TextRange textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                //Copy the formatting of table to Word 
                                CopyStyle(textRange, xCell, wCell);
                                for (int c = 2; c <= ColumnList.Count - numcol + 1; c++)
                                {
                                    int c2 = ColumnList[numcol + c - 2];
                                    xCell = sheet.Range[r, c2];
                                    wCell = table.Rows[r - 1].Cells[c - 1];
                                    //Fill data to Word table 
                                    textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                    //Copy the formatting of table to Word 
                                    CopyStyle(textRange, xCell, wCell);
                                }
                            }
                            //Set column width of Word table in Word 
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                                {
                                    table.Rows[i].Cells[j].Width = 100f;
                                }
                            }

                            //NoBreakTable(table);
                        }
                        else // more than 2 part
                        {
                            double value = (double)(ColumnList.Count - numcol) / (numcol - 1);
                            double nf = Math.Ceiling(value) + 1;
                            int n = (int)nf;

                            //Part 1 of n
                            title = section1.AddParagraph();
                            title.Text = item.Name + $" Part 1 of {n}";
                            title.ApplyStyle(style.Name);

                            //Write Table
                            table = section1.AddTable(true);
                            table.ResetCells(sheet.LastRow, numcol);
                            if (!boolResult) table.ApplyHorizontalMerge(0, 0, (numcol - 1));
                            //Traverse the rows and columns of table in worksheet and get the cells, call a custom function CopyStyle() to copy the font style and cell style from Excel to Word table. 
                            for (int r = 1; r <= sheet.LastRow; r++)
                            {
                                for (int c = 1; c <= numcol; c++)
                                {
                                    int c2 = ColumnList[c - 1];
                                    CellRange xCell = sheet.Range[r, c2];
                                    TableCell wCell = table.Rows[r - 1].Cells[c - 1];
                                    //Fill data to Word table 
                                    TextRange textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                    //Copy the formatting of table to Word 
                                    CopyStyle(textRange, xCell, wCell);
                                }
                            }
                            //Set column width of Word table in Word 
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                                {
                                    table.Rows[i].Cells[j].Width = 100f;
                                }
                            }

                            //NoBreakTable(table);

                            //Part x of n (2 <= x <= n-1)
                            for (int i = 2; i < n; i++)
                            {
                                title = section1.AddParagraph();
                                title.Text = item.Name + $" Part {i} of {n}";
                                title.ApplyStyle(style.Name);

                                //Write Table
                                table = section1.AddTable(true);
                                table.ResetCells(sheet.LastRow, numcol); // first column and the rest
                                if (!boolResult) table.ApplyHorizontalMerge(0, 0, (numcol - 1));
                                //Traverse the rows and columns of table in worksheet and get the cells, call a custom function CopyStyle() to copy the font style and cell style from Excel to Word table. 
                                for (int r = 1; r <= sheet.LastRow; r++)
                                {
                                    //for c = 1
                                    CellRange xCell = sheet.Range[r, 1];
                                    TableCell wCell = table.Rows[r - 1].Cells[0];
                                    //Fill data to Word table 
                                    TextRange textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                    //Copy the formatting of table to Word 
                                    CopyStyle(textRange, xCell, wCell);
                                    for (int c = 2; c <= numcol; c++)
                                    {
                                        int c2 = ColumnList[numcol + (i - 2) * (numcol - 1) + c - 2];
                                        xCell = sheet.Range[r, c2];
                                        wCell = table.Rows[r - 1].Cells[c - 1];
                                        //Fill data to Word table 
                                        textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                        //Copy the formatting of table to Word 
                                        CopyStyle(textRange, xCell, wCell);
                                    }
                                }

                                //Set column width of Word table in Word 
                                for (int it = 0; it < table.Rows.Count; it++)
                                {
                                    for (int j = 0; j < table.Rows[it].Cells.Count; j++)
                                    {
                                        table.Rows[it].Cells[j].Width = 100f;
                                    }
                                }

                                //NoBreakTable(table);
                            }


                            //Part n of n
                            title = section1.AddParagraph();
                            title.Text = item.Name + $" Part {n} of {n}";
                            title.ApplyStyle(style.Name);

                            //Write Table
                            table = section1.AddTable(true);
                            int lastcol = ColumnList.Count - (numcol + (n - 2) * (numcol - 1));
                            table.ResetCells(sheet.LastRow, lastcol + 1); // first column and the rest
                            if (!boolResult) table.ApplyHorizontalMerge(0, 0, lastcol);
                            //Traverse the rows and columns of table in worksheet and get the cells, call a custom function CopyStyle() to copy the font style and cell style from Excel to Word table. 
                            for (int r = 1; r <= sheet.LastRow; r++)
                            {
                                //for c = 1
                                CellRange xCell = sheet.Range[r, 1];
                                TableCell wCell = table.Rows[r - 1].Cells[0];
                                //Fill data to Word table 
                                TextRange textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                //Copy the formatting of table to Word 
                                CopyStyle(textRange, xCell, wCell);
                                for (int c = 2; c <= lastcol + 1; c++)
                                {
                                    int c2 = ColumnList[numcol + (n - 2) * (numcol - 1) + c - 2];
                                    xCell = sheet.Range[r, c2];
                                    wCell = table.Rows[r - 1].Cells[c - 1];
                                    //Fill data to Word table 
                                    textRange = wCell.AddParagraph().AppendText(xCell.NumberText);
                                    //Copy the formatting of table to Word 
                                    CopyStyle(textRange, xCell, wCell);
                                }
                            }
                            //Set column width of Word table in Word 
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                                {
                                    table.Rows[i].Cells[j].Width = 100f;
                                }
                            }

                            //NoBreakTable(table);
                        }
                        section1.AddParagraph().Text = ""; //Buffer
                        //continue;
                        break;
                    }
                    iter++;
                }
            }
        }
        #endregion

        //Get Column Number
        public List<int> GetColumnList(string name, bool boolSteel)
        {
            List<int> Column = new List<int>();
            switch (name)
            {
                // Grouting
                case "Joint Coordinates":
                    Index(Column, 1, 7);
                    break;
                case "Joint Restraint Assignments":
                    Index(Column, 1, 7);
                    break;
                case "Connectivity - Frame":
                    Index(Column, 1, 4);
                    break;
                case "Frame Props 01 - General":
                    Index(Column, 1, 5);
                    if (boolSteel) Index(Column, 10, 21);
                    break;
                case "Frame Section Assignments":
                    Index(Column, 1, 6);
                    break;
                case "Frame Loads - Distributed":
                    Index(Column, 1, 5);
                    Index(Column, 11, 12);
                    break;
                case "MatProp 01 - General":
                    Index(Column, 1, 3);
                    break;
                case "MatProp 02 - Basic Mech Props":
                    Index(Column, 1, 7);
                    break;
                case "MatProp 03b - Concrete Data":
                    Index(Column, 1, 2);
                    break;
                case "Load Case Definitions":
                    Index(Column, 1, 2);
                    Column.Add(7);
                    break;
                case "Load Pattern Definitions":
                    Index(Column, 1, 3);
                    break;
                case "Case - Static 1 - Load Assigns":
                    Index(Column, 1, 4);
                    break;
                case "Combination Definitions":
                    Index(Column, 1, 6);
                    break;
                case "Program Control":
                    Index(Column, 1, 2);
                    Column.Add(9);
                    break;
                // LongTerm, ShortTerm, VariationofDiameter, EQofDiameter, Connector, Site
                case "Connectivity - Link":
                    Index(Column, 1, 3);
                    break;
                case "Link Property Assignments":
                    Index(Column, 1, 5);
                    break;
                case "Link Props 05 - Gap":
                    Index(Column, 1, 7);
                    break;
                case "Jt Spring Assigns 1 - Uncoupled":
                    Index(Column, 1, 8);
                    break;
                case "Case - Static 2 - NL Load App":
                    Index(Column, 1, 4);
                    break;
                // Steel
                case "MatProp 03a - Steel Data":
                    Index(Column, 1, 11);
                    break;
                default:
                    break;
            }
            return Column;
        }


        //Index
        public void Index(List<int> numbers, int start, int end)
        {
            for (int i = start; i <= end; i++) numbers.Add(i);
        }

        //To prevent page break within table
        public void NoBreakTable(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Paragraph p in cell.Paragraphs)
                    {
                        p.Format.KeepFollow = true;
                    }
                }
            }
        }

        //Save document 
        public void FileSaving(string path)
        {
            doc.SaveToFile(path, Spire.Doc.FileFormat.Docx);
        }

        //The custom function CopyStyle() is defined as below 
        private static void CopyStyle(TextRange wTextRange, CellRange xCell, TableCell wCell)
        {
            //Copy font style 
            wTextRange.CharacterFormat.FontSize = 8;
            wTextRange.CharacterFormat.FontName = "Times New Roman";
            wTextRange.CharacterFormat.Bold = xCell.Style.Font.IsBold;
            wTextRange.CharacterFormat.Italic = xCell.Style.Font.IsItalic;
            //Copy text alignment 
            switch (xCell.HorizontalAlignment)
            {
                case HorizontalAlignType.Left:
                    wTextRange.OwnerParagraph.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case HorizontalAlignType.Center:
                    wTextRange.OwnerParagraph.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    break;
                case HorizontalAlignType.Right:
                    wTextRange.OwnerParagraph.Format.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
            }
        }
    }
}