using System.Diagnostics;
using System.Security.Principal;
using System;
using System.IO;
using OfficeOpenXml;

namespace Repository
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public sealed class TextLogWriter
    {
        private readonly string filePath;

        public TextLogWriter()
        {
            string baseFolder = Directory.GetCurrentDirectory();
            string logDirectory = Path.Combine(baseFolder, "Logging");
            CheckFileExists.EnsureDirectoryExists(logDirectory);
            string requestId = new UniqueId().GenerateForLog();
            filePath = Path.Combine(logDirectory, $"{requestId}.txt");
        }

        public void Log(
            string message,
             [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            string controllerName = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;

            // Trim "Controller" suffix if it's there
            if (controllerName.EndsWith("Controller"))
            {
                controllerName = controllerName.Substring(0, controllerName.Length - 10);
            }


            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("No messages to log.");
                return;
            }

            try
            {
                using (var writer = new StreamWriter(filePath, true))
                {
                    // Log format: Timestamp | Message | Controller | File | Method | Line Number
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                    writer.WriteLine($"Timestamp: {DateTime.Now}");
                    writer.WriteLine($"Message: {message}");
                    writer.WriteLine($"Controller: {controllerName ?? "Unknown"}");
                    writer.WriteLine($"File: {callerFilePath}");
                    writer.WriteLine($"Method: {callerMemberName}");
                    writer.WriteLine($"Line Number: {callerLineNumber}");
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // Handle file errors silently
                Console.WriteLine($"Error writing to text log: {ex.Message}");
            }
        }
    }




    public class CheckFileExists()
    {

        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }


    public sealed class ExcelLogWriter
    {
        private readonly string filePath;

        public ExcelLogWriter()
        {
            string baseFolder = Directory.GetCurrentDirectory();
            string logDirectory = Path.Combine(baseFolder, "Logging");
            CheckFileExists.EnsureDirectoryExists(logDirectory);

            string requestId = new UniqueId().GenerateForLog();
            filePath = Path.Combine(logDirectory, $"{requestId}.xlsx");
        }


        public void Log(
     string message,
     [CallerFilePath] string callerFilePath = null,
     [CallerMemberName] string callerMemberName = null,
     [CallerLineNumber] int callerLineNumber = 0)
        {
            string controllerName = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;

            // Trim "Controller" suffix if it's there
            if (controllerName.EndsWith("Controller"))
            {
                controllerName = controllerName.Substring(0, controllerName.Length - 10);
            }

            // Continue with the existing logging logic
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("No messages to log.");
                return;
            }

            if (!File.Exists(filePath))
            {
                // Create a new Excel file with headers
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Logs");
                    worksheet.Cells[1, 1].Value = "Timestamp";
                    worksheet.Cells[1, 2].Value = "Controller";
                    worksheet.Cells[1, 3].Value = "File";
                    worksheet.Cells[1, 4].Value = "Method";
                    worksheet.Cells[1, 5].Value = "Line Number";
                    worksheet.Cells[1, 6].Value = "Message";
                    package.SaveAs(new FileInfo(filePath));
                }
            }

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var lastRow = worksheet.Dimension?.Rows ?? 1; // Find the last used row

                    worksheet.Cells[lastRow + 1, 1].Value = DateTime.Now.ToString(); // Timestamp
                    worksheet.Cells[lastRow + 1, 2].Value = controllerName; // Controller
                    worksheet.Cells[lastRow + 1, 3].Value = callerFilePath; // File path
                    worksheet.Cells[lastRow + 1, 4].Value = callerMemberName; // Method name
                    worksheet.Cells[lastRow + 1, 5].Value = callerLineNumber; // Line number
                    worksheet.Cells[lastRow + 1, 6].Value = message; // Message

                    package.Save();
                }
            }
            catch (Exception ex)
            {
                // Handle file errors silently
                Console.WriteLine($"Error writing to Excel log: {ex.Message}");
            }
        }


    }







}