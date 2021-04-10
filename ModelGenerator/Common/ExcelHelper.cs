/****************************************************************************
*项目名称：WEF.ModelGenerator.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.ModelGenerator.Common
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

using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Common
{
    class ExcelHelper
    {
        public static event Action OnStart;
        public static event Action<long, long> OnRunning;
        public static event Action OnStop;

        public static async Task DataTableToExcelAsync(DataTable dt, string fileName, string sheetName = "sheet1")
        {
            await Task.Yield();
            OnStart?.Invoke();
            var workbook = new HSSFWorkbook();//创建Workbook对象
            var styleTitle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;//加粗
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


        public static async Task DataTableToExcel(ConnectionModel cnn, string tableName, string fileName, string sheetName = "sheet1")
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
            font.Boldweight = (short)FontBoldWeight.Bold;//加粗
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
    }
}
