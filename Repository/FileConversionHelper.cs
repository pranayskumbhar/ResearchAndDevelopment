using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FileConversionHelper
    {


        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OCRDocuments", "Working");
        private readonly string _rejectedFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OCRDocuments", "Rejected");
        public static string ConvertToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }





        public static bool MoteFileToRejectedFolder(string fileName)
        {
            try
            {
                string sourceFolder = Path.Combine("wwwroot", "OCRDocuments", "Working");
                string destinationFolder = Path.Combine("wwwroot", "OCRDocuments", "Rejected");

                // Construct the full paths for source and destination
                string sourceFilePath = Path.Combine(sourceFolder, fileName);
                string destinationFilePath = Path.Combine(destinationFolder, fileName);

                // Check if the file exists in the source folder
                if (File.Exists(sourceFilePath))
                {
                    // Ensure the destination folder exists
                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    // Move the file
                    try
                    {
                        File.Move(sourceFilePath, destinationFilePath);
                        Console.WriteLine($"File '{fileName}' moved successfully to the 'rejected' folder.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error moving file: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"File '{fileName}' not found in the 'working' folder.");
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool DeleteFileWhenSuccess(string fileName)
        {
            try
            {
                string sourceFolder = Path.Combine("wwwroot", "OCRDocuments", "Working");
                string destinationFolder = Path.Combine("wwwroot", "OCRDocuments", "Rejected");

                // Construct the full paths for source and destination
                string sourceFilePath = Path.Combine(sourceFolder, fileName);

                // Check if the file exists in the source folder
                if (File.Exists(sourceFilePath))
                {
                    // Move the file
                    try
                    {
                        File.Delete(sourceFilePath);
                        Console.WriteLine($"File '{fileName}' moved successfully to the 'rejected' folder.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error moving file: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"File '{fileName}' not found in the 'working' folder.");
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static string SaveBase64ToFile(string base64String)
        {
            string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OCRDocuments", "Working");
            try
            {
                // Remove any non-base64 characters (just in case there's extra data before or after)
                string cleanBase64String = base64String.Trim();

                // Decode the base64 string to get the raw bytes
                byte[] fileBytes = Convert.FromBase64String(cleanBase64String);

                // Determine the file type based on the first few bytes
                string fileExtension = GetFileExtension(fileBytes);

                if (string.IsNullOrEmpty(fileExtension))
                {
                    throw new Exception("Unable to detect the file type.");
                }

                // Generate a unique file name with the detected extension
                string uniqueFileName = Guid.NewGuid().ToString() + "." + fileExtension;

                // Combine the folder path and the unique file name to get the full file path
                string filePath = Path.Combine(_uploadFolderPath, uniqueFileName);

                // Ensure the directory exists
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write the byte array to a file
                File.WriteAllBytes(filePath, fileBytes);

                Console.WriteLine($"File saved successfully to: {filePath}");
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "";
            }
        }
        private static string GetFileExtension(byte[] fileBytes)
        {
            // Check the magic numbers (file signatures)
            if (fileBytes.Length >= 4)
            {
                // PNG file signature: 89 50 4E 47 0D 0A 1A 0A
                if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
                {
                    return "png";
                }
                // JPEG file signature: FF D8 FF
                else if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8 && fileBytes[2] == 0xFF)
                {
                    return "jpg";
                }
                // GIF file signature: 47 49 46 38 (GIF87a and GIF89a)
                else if (fileBytes[0] == 0x47 && fileBytes[1] == 0x49 && fileBytes[2] == 0x46 && fileBytes[3] == 0x38)
                {
                    return "gif";
                }
                // PDF file signature: 25 50 44 46 (PDF format)
                else if (fileBytes[0] == 0x25 && fileBytes[1] == 0x50 && fileBytes[2] == 0x44 && fileBytes[3] == 0x46)
                {
                    return "pdf";
                }
                // XLSX file signature: 50 4B 03 04 (ZIP format used by XLSX)
                else if (fileBytes[0] == 0x50 && fileBytes[1] == 0x4B && fileBytes[2] == 0x03 && fileBytes[3] == 0x04)
                {
                    return "xlsx";
                }
            }

            // Return null if no signature matches
            return null;
        }

        public static void SaveBase64ToFile1(string base64String, string filePath)
        {
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);

                // Ensure the directory exists
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write the byte array to a file
                File.WriteAllBytes(filePath, fileBytes);

                Console.WriteLine("File saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

}
