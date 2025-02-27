using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

public static class ExcelHelper
{
    /// <summary>
    /// 顏色 設定
    /// </summary> 
    public static void SetQuickStyle(this ExcelRange range, Color foreColor, Color bgColor = default(Color), ExcelHorizontalAlignment hAlign = ExcelHorizontalAlignment.Left)
    {
        range.Style.Font.Color.SetColor(foreColor); 
        if (bgColor != default(Color))
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(bgColor);
        }
        range.Style.HorizontalAlignment = hAlign;
        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    }
        
    /// <summary>
    /// DataTable 轉 Excel For Web Download
    /// </summary>
    public static void DatatableToExcelForWeb(string sheetName, DataTable dt, ref byte[] file, bool isTitleInfo = true, string titleName = null)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage p = new ExcelPackage())
        {
            p.Encryption.Password = DateTime.Now.ToShortDateString();
            ExcelWorksheet sheet = p.Workbook.Worksheets.Add(sheetName);
            int headCount = dt.Columns.Count;
            int rowIdx = 1;

            //建立 Title Info
            if (isTitleInfo)
            {
                CreateExcelTitle(sheet, titleName, headCount);
                rowIdx++;
            }

            //建立 Data Head
            CreateDataHead(sheet, dt, rowIdx);

            //建立 Data  
            CreateDataData(sheet, dt, rowIdx + 1);

            sheet.Cells.AutoFitColumns();
            sheet.DefaultRowHeight = 20;
            file = p.GetAsByteArray();
            // p.SaveAs(new FileInfo(excelPath)); 
        }
    }

    /// <summary>
    /// 建立 Title 
    /// </summary>
    private static void CreateExcelTitle(ExcelWorksheet sheet, string title, int headCount)
    {
        sheet.Cells[1, 1, 1, headCount].Merge = true;
        sheet.Cells[1, 1].Value = title.ToString();
        sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Row(1).Height = 25;
    }
      
    /// <summary>
    /// DataTable 建立 Data Head
    /// </summary>
    private static void CreateDataHead(ExcelWorksheet sheet, DataTable dt, int rowIdx)
    {
        //建立 Data Head
        int colIdx = 1;
        if (dt == null || dt.Columns.Count == 0) return;
        foreach (DataColumn dc in dt.Columns)
        {
            sheet.Cells[rowIdx, colIdx++].Value = dc.ColumnName.ToString();
        }
        Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");
        sheet.Cells[rowIdx, 1, rowIdx, dt.Columns.Count].SetQuickStyle(Color.Black, colFromHex, ExcelHorizontalAlignment.Center);
        sheet.Row(rowIdx).Height = 25;
    }
       
    /// <summary>
    /// 建立 Data 內容
    /// </summary> 
    private static void CreateDataData(ExcelWorksheet sheet, DataTable dt, int rowIdx)
    {
        int colIdx = 1;
        foreach (DataRow dtRow in dt.Rows)
        {
            colIdx = 1;
            foreach (DataColumn dtCol in dt.Columns)
            {
                //存入檔名或目錄名
                sheet.Cells[rowIdx, colIdx].Value = dtRow[dtCol.ColumnName.ToString()].ToString();
                sheet.Cells[rowIdx, colIdx].SetQuickStyle(Color.Black);
                colIdx++;
            }
            sheet.Row(rowIdx).Height = 20;
            rowIdx++;
        }
    }


}
