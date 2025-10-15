/****************************************************************************
*项目名称：WEF.Standard.DevelopTools.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Standard.DevelopTools.Common
*类 名 称：ExcelHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/6/30 14:27:40
*描述：
*=====================================================================
*修改时间：2020/6/30 14:27:40
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.Common
{
    public class ExcelHelper
    {
        public event Action OnStart;
        public event Action<long, long> OnRunning;
        public event Action OnStop;

        public async Task DataTableToExcelAsync(DataTable dt, string fileName, string sheetName = "sheet1")
        {
            await Task.Yield();
            OnStart?.Invoke();
            var workbook = new HSSFWorkbook();//创建Workbook对象
            var styleTitle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            //font.Boldweight = (short)FontBoldWeight.Bold;//加粗
            styleTitle.SetFont(font);

            if (string.IsNullOrEmpty(sheetName) || dt == null || dt.Rows.Count < 1)
            {
                return;
            }
            //创建工作表
            ISheet sheet = workbook.CreateSheet(sheetName);
            //在工作表中添加一行
            IRow firstRow = sheet.CreateRow(0);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = firstRow.CreateCell(i);
                cell.CellStyle = styleTitle;
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1); //在工作表1中添加一行
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var rowValue = dt.Rows[i][j] == null ? "" : dt.Rows[i][j].ToString();
                    row.CreateCell(j).SetCellValue(rowValue);
                }
            }
            OnRunning?.Invoke(dt.Rows.Count, 0);
            using (var fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    ms.Position = 0;
                    var buffer = ms.ToArray();
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            OnStop?.Invoke();
        }


        public async Task DataTableToExcel(ConnectionModel cnn, string tableName, string fileName, string sheetName = "sheet1")
        {
            await Task.Yield();

            OnStart?.Invoke();


            var dbObj = DBObjectHelper.GetDBObject(cnn);

            var reader = dbObj.GetDataReader(cnn.Database, $"select * from {tableName}");

            var filedCount = reader.FieldCount;

            if (string.IsNullOrEmpty(sheetName) || filedCount < 1)
            {
                return;
            }

            var workbook = new HSSFWorkbook();//创建Workbook对象
            var styleTitle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            //font.Boldweight = (short)FontBoldWeight.Bold;//加粗
            styleTitle.SetFont(font);

           
            //创建工作表
            ISheet sheet = workbook.CreateSheet(sheetName);
            //在工作表中添加一行
            IRow firstRow = sheet.CreateRow(0);

            for (int i = 0; i < filedCount; i++)
            {
                ICell cell = firstRow.CreateCell(i);
                cell.CellStyle = styleTitle;
                cell.SetCellValue(reader.GetName(i));
            }

            var rowCount = 0;

            while (reader.Read())
            {
                rowCount++;
                IRow row = sheet.CreateRow(rowCount); //在工作表1中添加一行
                for (int j = 0; j < filedCount; j++)
                {
                    var rowValue = reader[j] == null ? "" : reader[j].ToString();
                    row.CreateCell(j).SetCellValue(rowValue);
                }
                OnRunning?.Invoke(rowCount, filedCount * rowCount);
            };
            
            using (var fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    ms.Position = 0;
                    var buffer = ms.ToArray();
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            OnStop?.Invoke();
        }


        /// <summary>
        /// 从流中读取数据
        /// </summary>
        /// <param name="excelstream"></param>
        /// <param name="v2003"></param>
        /// <param name="sheetName"></param>
        /// <param name="startRow"></param>
        /// <param name="hasHeader"></param>
        /// <param name="columnNameList"></param>
        /// <returns></returns>
        public static DataTable ImportFromStream(Stream excelstream, bool v2003 = true, string sheetName = "sheet1", int startRow = 0, bool hasHeader = true, IEnumerable<string> columnNameList = null)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            if (v2003)
            {
                workbook = new HSSFWorkbook(excelstream);
            }
            else
            {
                workbook = new XSSFWorkbook(excelstream);
            }
            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                {
                    return data;
                }
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }
            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(startRow);
                int cellCount = firstRow.LastCellNum;
                if (hasHeader)
                {
                    if (columnNameList != null && columnNameList.Any())
                    {
                        foreach (var columnName in columnNameList)
                        {
                            DataColumn column = new DataColumn(columnName);
                            data.Columns.Add(column);
                        }
                    }
                    else
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                    }


                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    for (int i = 0; i < cellCount; ++i)
                    {
                        DataColumn column = new DataColumn(i.ToString());
                        data.Columns.Add(column);
                    }
                    startRow = sheet.FirstRowNum;
                }

                //最后一列的标号
                int rowCount = sheet.LastRowNum;

                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null || row.FirstCellNum == -1) continue; //没有数据的行默认是null

                    DataRow dataRow = data.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            var cell = row.GetCell(j);
                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    dataRow[j] = "";
                                    break;
                                case CellType.Numeric:
                                    short format = cell.CellStyle.DataFormat;
                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                    if (format == 14 || format == 22 || format == 31 || format == 57 || format == 58 || format == 176)
                                    {
                                        dataRow[j] = DateTime.FromOADate(cell.NumericCellValue).ToString("yyyy/MM/dd");//cell.DateCellValue 第二次打开excel的时候会这个属性读不到会抛异常                                                                                                        
                                    }
                                    else
                                        dataRow[j] = cell.NumericCellValue;
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                            }
                        }
                        else
                        {
                            dataRow[j] = "";
                        }
                    }
                    data.Rows.Add(dataRow);
                }
            }

            return data;
        }

        /// <summary>
        /// 从文件中读取数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="startRow"></param>
        /// <param name="hasHeader"></param>
        /// <param name="columnNameList"></param>
        /// <returns></returns>
        public static DataTable ImportFromFile(string fileName, string sheetName = "sheet1", int startRow = 0, bool hasHeader = true, IEnumerable<string> columnNameList = null)
        {
            using (var fs = File.OpenRead(fileName))
            {
                if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return ImportFromStream(fs, false, sheetName, startRow, hasHeader, columnNameList);
                }
                else
                {
                    return ImportFromStream(fs, true, sheetName, startRow, hasHeader, columnNameList);
                }
            }
        }


    }
}
