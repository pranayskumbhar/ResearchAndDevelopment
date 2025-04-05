using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Commons.Utils;
 
namespace Repository
{
    public static class LogException
    {
        private static string fileName;
        private static string logDirectory;
        private static string textFilePath;
        private static string excelFilePath;
        private static string csvFilePath;

        #region Constructor
        static LogException()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Exception Logging");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            fileName = "D" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        #endregion

        #region LogIntoText
        public static void LogIntoText(string controllerName, string methodName, int? lineNumber, string callerFilePath, Exception ex)
        {
            if (!Directory.Exists(Path.Combine(logDirectory, "Text")))
            {
                Directory.CreateDirectory(Path.Combine(logDirectory, "Text"));
            }
            textFilePath = Path.Combine(Path.Combine(logDirectory, "Text"), $"{fileName}.txt");

            using (var writer = new StreamWriter(textFilePath, true))
            {
                // Log format: Timestamp | Message | Controller | File | Method | Line Number
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine($"Message: {ex.ToString()}");
                writer.WriteLine($"Controller: {controllerName ?? "Unknown"}");
                writer.WriteLine($"Method: {methodName}");
                writer.WriteLine($"Line Number: {lineNumber}");
                writer.WriteLine($"Timestamp: {DateTime.Now}");
                writer.WriteLine($"File: {callerFilePath}");
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }
        #endregion

        #region LogIntoExcel
        public static void LogIntoExcel(string controllerName, string methodName, int? lineNumber, string callerFilePath, Exception ex)
        {
            if (!Directory.Exists(Path.Combine(logDirectory, "Excel")))
            {
                Directory.CreateDirectory(Path.Combine(logDirectory, "Excel"));
            }
            excelFilePath = Path.Combine(Path.Combine(logDirectory, "Excel"), $"{fileName}.xlsx");



            FileInfo fileInfo = new FileInfo(excelFilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet;

                // Create worksheet if it doesn't exist
                if (package.Workbook.Worksheets.Count == 0)
                {
                    worksheet = package.Workbook.Worksheets.Add("Logs");

                    // Add header row
                    worksheet.Cells[1, 1].Value = "Timestamp";
                    worksheet.Cells[1, 2].Value = "Message";
                    worksheet.Cells[1, 3].Value = "Controller";
                    worksheet.Cells[1, 4].Value = "Method";
                    worksheet.Cells[1, 5].Value = "Line Number";
                    worksheet.Cells[1, 6].Value = "File Path";

                    using (var range = worksheet.Cells[1, 1, 1, 6])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }
                }
                else
                {
                    worksheet = package.Workbook.Worksheets["Logs"];
                }

                // Find next row
                int nextRow = worksheet.Dimension?.End.Row + 1 ?? 2;

                worksheet.Cells[nextRow, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cells[nextRow, 2].Value = string.Join(", ", ex.ToString());
                worksheet.Cells[nextRow, 3].Value = controllerName ?? "Unknown";
                worksheet.Cells[nextRow, 4].Value = methodName;
                worksheet.Cells[nextRow, 5].Value = lineNumber;
                worksheet.Cells[nextRow, 6].Value = callerFilePath;

                package.Save();
            }
        }
        #endregion

        #region LogIntoCsv
        public static void LogIntoCsv(string controllerName, string methodName, int? lineNumber, string callerFilePath, Exception ex)
        {
            if (!Directory.Exists(Path.Combine(logDirectory, "Csv")))
            {
                Directory.CreateDirectory(Path.Combine(logDirectory, "Csv"));
            }
            csvFilePath = Path.Combine(Path.Combine(logDirectory, "Csv"), $"{fileName}.csv");


            bool fileExists = File.Exists(csvFilePath);

            using (var writer = new StreamWriter(csvFilePath, true, Encoding.UTF8))
            {
                // Write header if file doesn't exist
                if (!fileExists)
                {
                    writer.WriteLine("Timestamp,Message,Controller,Method,Line Number,File Path");
                }

                var csvRow = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    string.Join(", ", ex.ToString()).Replace("\"", "\"\""), // escape quotes
                    controllerName ?? "Unknown",
                    methodName,
                    lineNumber,
                    callerFilePath?.Replace("\"", "\"\"") ?? "Unknown"
                );

                writer.WriteLine(csvRow);
            }
        }
        #endregion
    }
}
