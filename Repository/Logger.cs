using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Text;

namespace Repository
{
    public static class Logger
    {
        private static string fileName;
        private static string logDirectory;
        private static string textFilePath;
        private static string excelFilePath;
        private static string csvFilePath;

        #region Constructor
        static Logger()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logging");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            fileName = "D" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        #endregion

        #region LogIntoText
        public static void LogIntoText(params string[] messages)
        {
            if (!Directory.Exists(Path.Combine(logDirectory, "Text")))
            {
                Directory.CreateDirectory(Path.Combine(logDirectory, "Text"));
            }
            textFilePath = Path.Combine(Path.Combine(logDirectory, "Text"), $"{fileName}.txt");
            #region Fetch All Information Of Caller Method
            var stackTrace = new StackTrace(true);
            // Frame 1 is the caller of LogIntoText
            var callingMethod = stackTrace.GetFrame(1).GetMethod();
            var methodName = callingMethod.Name;
            var controllerName = callingMethod.DeclaringType?.Name; // Typically ends with "Controller"

            var frame = stackTrace.GetFrame(1);    // 1 = caller of LogIntoText
            var lineNumber = frame.GetFileLineNumber();
            var callerFilePath = frame.GetFileName(); // full path to the file
            #endregion

            using (var writer = new StreamWriter(textFilePath, true))
            {
                // Log format: Timestamp | Message | Controller | File | Method | Line Number
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine($"Message: {string.Join(",", messages)}");
                writer.WriteLine($"Controller: {controllerName ?? "Unknown"}");
                writer.WriteLine($"Method: {methodName}");
                writer.WriteLine($"Line Number: {lineNumber}");
                writer.WriteLine($"Timestamp: {DateTime.Now}");
                writer.WriteLine($"File: {callerFilePath}");
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------\n");
            }
        }
        #endregion

        #region LogIntoExcel
        public static void LogIntoExcel(params string[] messages)
        {
            if (!Directory.Exists(Path.Combine(logDirectory, "Excel")))
            {
                Directory.CreateDirectory(Path.Combine(logDirectory, "Excel"));
            }
            excelFilePath = Path.Combine(Path.Combine(logDirectory, "Excel"), $"{fileName}.xlsx");


            #region Fetch All Information Of Caller Method
            var stackTrace = new StackTrace(true);
            var callingMethod = stackTrace.GetFrame(1).GetMethod();
            var methodName = callingMethod.Name;
            var controllerName = callingMethod.DeclaringType?.Name;
            var frame = stackTrace.GetFrame(1);
            var lineNumber = frame.GetFileLineNumber();
            var callerFilePath = frame.GetFileName();
            #endregion

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
                worksheet.Cells[nextRow, 2].Value = string.Join(", ", messages);
                worksheet.Cells[nextRow, 3].Value = controllerName ?? "Unknown";
                worksheet.Cells[nextRow, 4].Value = methodName;
                worksheet.Cells[nextRow, 5].Value = lineNumber;
                worksheet.Cells[nextRow, 6].Value = callerFilePath;

                package.Save();
            }
        }
        #endregion

        #region LogIntoCsv
        public static void LogIntoCsv(params string[] messages)
        {
            if (!Directory.Exists(Path.Combine(logDirectory, "Csv")))
            {
                Directory.CreateDirectory(Path.Combine(logDirectory, "Csv"));
            }
            csvFilePath = Path.Combine(Path.Combine(logDirectory, "Csv"), $"{fileName}.csv");

            #region Fetch All Information Of Caller Method
            var stackTrace = new StackTrace(true);
            var callingMethod = stackTrace.GetFrame(1).GetMethod();
            var methodName = callingMethod.Name;
            var controllerName = callingMethod.DeclaringType?.Name;
            var frame = stackTrace.GetFrame(1);
            var lineNumber = frame.GetFileLineNumber();
            var callerFilePath = frame.GetFileName();
            #endregion

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
                    string.Join(", ", messages).Replace("\"", "\"\""), // escape quotes
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
