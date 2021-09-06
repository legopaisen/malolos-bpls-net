using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using GemBox.Spreadsheet;

namespace Amellar.Common.ExcelReport
{
    public class ExcelTemplate
    {

        public static bool CopyColumnStyle(ExcelWorksheet source, ExcelWorksheet destination)
        {
            int intSourceColumnWidth = source.Columns.Count;
            int intDestinationColumnWidth = destination.Columns.Count;

            for (int i = 0; i < intSourceColumnWidth; i++)
            {
                destination.Columns[i].Width = source.Columns[i].Width;
                destination.Columns[i].Hidden = source.Columns[i].Hidden;
            }

            return true;
        }

        public static int CopyCellSubrange(string strSourceFirstCell, 
            string strSourceLastCell, ExcelWorksheet source, 
            int intDestinationColumn, int intDestinationRow, ExcelWorksheet destination)
        {
            Hashtable hshProperties = new Hashtable();
            Hashtable hshReferences = new Hashtable();

            return ExcelTemplate.CopyCellSubrange(strSourceFirstCell, strSourceLastCell, source, intDestinationColumn,
                intDestinationRow, destination, hshProperties, out hshReferences);
        } 

        public static string ColumnName(string strCellName)
        {
            StringBuilder strColumnName = new StringBuilder();
            for (int i = 0; i < strCellName.Length; i++)
            {
                if (Char.IsLetter(strCellName[i]))
                {
                    strColumnName.Append(strCellName[i]);
                }
                else
                {
                    break;
                }
            }

            return strColumnName.ToString();
        }


        public static int RowNumber(string strCellName)
        {
            StringBuilder strRowNumber = new StringBuilder();
            for (int i = 0; i < strCellName.Length; i++)
            {
                if (Char.IsLetter(strCellName[i]))
                {
                    if (strRowNumber.Length > 0)
                        return -1;
                }
                else if (Char.IsNumber(strCellName[i]))
                {
                    strRowNumber.Append(strCellName[i]);
                }
                else
                {
                    return -1;
                }
            }

            int intRowNumber = -1;
            int.TryParse(strRowNumber.ToString(), out intRowNumber);

            return intRowNumber;
        }


        //
        public static string MinColumnName(string strCellName1, string strCellName2)
        {
            string strMaxColumnName = ExcelTemplate.MaxColumnName(strCellName1, strCellName2);
            if (strCellName1 == strMaxColumnName)
                return strCellName2;
            else //if (strCellName2 == strMaxColumnName)
                return strCellName1;
        }

        //
        public static string MaxColumnName(string strCellName1, string strCellName2)
        {
            string strMaxColumn = string.Empty;
            string strCellColumn1 = ExcelTemplate.ColumnName(strCellName1);
            string strCellColumn2 = ExcelTemplate.ColumnName(strCellName2);

            if (strCellColumn1.Length == strCellColumn2.Length)
            {
                if (strCellColumn1.CompareTo(strCellColumn2) >= 0)
                    return strCellName1;
                else
                    return strCellName2;
            }
            else if (strCellColumn1.Length > strCellColumn2.Length)
                return strCellName1;
            else //if (strCellColumn1.Length < strCellColumn2.Length)
                return strCellName2;
        }

        public static void Sum(ExcelWorksheet destination, List<string> lstProperties, Hashtable hshOutput,
            Hashtable hshList)
        {
            int intCount = lstProperties.Count;
            int intListCount = hshList.Count;
            ArrayList lstList = new ArrayList(hshList.Keys);
            for (int i = 0; i < intCount; i++)
            {
                if (hshOutput.ContainsKey(lstProperties[i]))
                {
                    StringBuilder strFormula = new StringBuilder();
                    for (int j = 0; j < intListCount; j++)
                    {
                        Hashtable hshTemp = (Hashtable) hshList[lstList[j]];
                        if (hshTemp.ContainsKey(lstProperties[i]))
                        {
                            if (strFormula.Length == 0)
                                strFormula.Append("=");
                            else
                                strFormula.Append("+");
                            strFormula.Append(hshTemp[lstProperties[i]].ToString());
                        }
                    }
                    if (strFormula.Length > 0)
                    {
                        ExcelTemplate.Formula(hshOutput[lstProperties[i]].ToString(), destination,
                            strFormula.ToString());
                    }
                }
            }
        }

        //does not support cells starting with column 'DA'- hard coded
        public static int ColumnNumber(string strCellName)
        {
            char chrStartChar = 'A';
            int intColumnNumber = 0;
            int intCount = strCellName.Length;
            bool blnRunOnce = true;
            if (intCount > 0)
            {
                for (int i = 0; i < intCount; i++)
                {
                    if (Char.IsLetter(strCellName[i]))
                    {
                        blnRunOnce = false;
                        intColumnNumber += (int) ((byte)strCellName[i] - (byte)chrStartChar);
                        
                        /*if (i != 0)
                        {
                            intColumnNumber+= 26;
                        }*/
                        
                        if (i == 0 && strCellName[i] == 'A' && Char.IsLetter(strCellName[1]))
                        {
                            intColumnNumber += 26;
                        }
                        else if (i == 0 && strCellName[i] == 'B' && Char.IsLetter(strCellName[1]))
                        {
                            intColumnNumber += 51;
                        }
                        else if (i == 0 && strCellName[i] == 'C' && Char.IsLetter(strCellName[1]))
                        {
                            intColumnNumber += 76;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (blnRunOnce)
                intColumnNumber = -1;

            return intColumnNumber;
        }

        //does not support cells starting with column 'DA' - hard coded
        public static string CellName(int intColumnIndex, int intRowIndex)
        {
            char chrStartChar = 'A';
            StringBuilder strCellName = new StringBuilder();
            char chrColumnChar = ' ';

            if (intColumnIndex >= 26 && intColumnIndex < 52)
            {
                strCellName.Append("A");
                intColumnIndex -= 26;
            }
            else if (intColumnIndex >= 52 && intColumnIndex < 78)
            {
                strCellName.Append("B");
                intColumnIndex -= 52;
            }
            else if (intColumnIndex >= 78)
            {
                strCellName.Append("C");
                intColumnIndex -= 78;
            }

            chrColumnChar = (char)((byte)chrStartChar + intColumnIndex);
            strCellName.Append(chrColumnChar);
            strCellName.Append(string.Format("{0}", intRowIndex + 1));

            return strCellName.ToString();
        }


        public static bool Formula(string strCellName, ExcelWorksheet destination, string strFormula)
        {
            destination.Cells[strCellName].Formula = strFormula;
            return true;
        }

        public static int CopyCellSubrange(string strSourceFirstCell, 
            string strSourceLastCell, ExcelWorksheet source, 
            int intDestinationColumn, int intDestinationRow, ExcelWorksheet destination,
            Hashtable hshProperties, out Hashtable hshReferences)
        {
            hshReferences = new Hashtable();

            List<string> lstMergedCells = new List<string>();

            int intDestinationRowIndex = intDestinationRow;
            CellRange cellrange = source.Cells.GetSubrange(strSourceFirstCell, strSourceLastCell);
            int intSourceRowIndex = ExcelTemplate.RowNumber(strSourceFirstCell)-1;

            for (int intRowIndex = 0; intRowIndex < cellrange.Height; intRowIndex++)
            {
                destination.Rows[intDestinationRow + intRowIndex].Height = source.Rows[intSourceRowIndex + intRowIndex].Height;


                for (int intColumnIndex = 0; intColumnIndex < cellrange.Width; intColumnIndex++)
                {
                    destination.Cells[intDestinationRow + intRowIndex, intDestinationColumn + intColumnIndex].Style =
                        cellrange[intRowIndex, intColumnIndex].Style;
                    bool blnIsProperty = false;
                    if (cellrange[intRowIndex, intColumnIndex].Value != null)
                    {
                        string strValue = cellrange[intRowIndex, intColumnIndex].Value.ToString().Trim();
                        if (hshProperties.ContainsKey(strValue))
                        {
                            blnIsProperty = true;
                            destination.Cells[intDestinationRow + intRowIndex, intDestinationColumn + intColumnIndex].Value =
                                hshProperties[strValue];
                            if (!hshReferences.ContainsKey(strValue))
                            {
                                hshReferences.Add(strValue, ExcelTemplate.CellName(intDestinationColumn + intColumnIndex,
                                    intDestinationRow + intRowIndex));
                            }
                        }
                    }

                    //merged columns
                    if (cellrange[intRowIndex, intColumnIndex].MergedRange != null)
                    {
                        CellRange mergedrange = cellrange[intRowIndex, intColumnIndex].MergedRange;

                        if (lstMergedCells.IndexOf(string.Format("{0}:{1}", mergedrange.StartPosition,
                            mergedrange.EndPosition)) == -1)
                        {
                            int intRowCount = ExcelTemplate.RowNumber(mergedrange.EndPosition) -
                                ExcelTemplate.RowNumber(mergedrange.StartPosition);
                            //problem here
                            int intColumnCount = ExcelTemplate.ColumnNumber(mergedrange.EndPosition) -
                                ExcelTemplate.ColumnNumber(mergedrange.StartPosition);

                            /*
                            string strFirstCellName1 = ExcelTemplate.CellName(intColumnIndex, 
                                intRowIndex + intDestinationRow);
                            string strLastCellName1 = string.Format("{0}{1}", ExcelTemplate.ColumnName(mergedrange.EndPosition),
                                intRowIndex + 1 + intDestinationRow + intRowCount);
                            */

                            string strFirstCellName = ExcelTemplate.CellName(intDestinationColumn + intColumnIndex,
                                intRowIndex + intDestinationRow);
                            string strLastCellName = ExcelTemplate.CellName(intDestinationColumn + intColumnIndex + intColumnCount,
                                intRowIndex + intDestinationRow + intRowCount);

                            CellRange destmergerange = destination.Cells.GetSubrange(strFirstCellName,
                                strLastCellName);
                            destmergerange.Merged = true;

                            lstMergedCells.Add(string.Format("{0}:{1}", mergedrange.StartPosition,
                                mergedrange.EndPosition));
                        }
                        else
                        {
                            blnIsProperty = true;
                        }
                    }

                    if (!blnIsProperty)
                    {
                        destination.Cells[intDestinationRow + intRowIndex, intDestinationColumn + intColumnIndex].Value =
                            cellrange[intRowIndex, intColumnIndex].Value;
                    }
                }
                intDestinationRowIndex = intDestinationRow + intRowIndex+1;
            }

            return intDestinationRowIndex;
        }

    }
}
