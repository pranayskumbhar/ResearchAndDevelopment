using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Repository;
using ResearchAndDevelopment.Models;
using ResearchAndDevelopment.Utils;
 
namespace ResearchAndDevelopment.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private static int _exceptionIdCounter = 0; // Simple counter to generate unique IDs
        private readonly object _lock = new object();
        private readonly bool isEnc;

        public CustomExceptionFilter(IConfiguration configuration)
        {
            isEnc = bool.Parse(configuration["isEncrypted"].ToLower());

        }








        public void LogException(Exception exception, int exceptionId)
        {
            try
            {
                //Log.Save("-=------------------CustomExceptionFilter : " + Convert.ToString(exception.Message), "AgentInquiry Api ", "");

                // Get the wwwroot path dynamically
                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                // Generate the log directory and file paths
                string logDirectory = Path.Combine(wwwRootPath, "logs", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                string logFilePath = Path.Combine(logDirectory, "Exception_" + DateTime.UtcNow.ToString("yyyy-MM-dd").ToString());

                // Ensure the directory exists
                Directory.CreateDirectory(logDirectory);

                // Create log message
                string logMessage = $"ID: {exceptionId} - Time: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\n" +
                                    $"Exception Type: {exception.GetType().Name}\n" +
                                    $"Message: {exception.Message}\n" +
                                    $"Stack Trace: {exception.StackTrace}\n\n" +
                                    $"**********************************************************************************************************************************************************************************\n\n";

                // Append the log message to the file
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur while logging
                Console.WriteLine($"Failed to log exception: {ex.Message}");
            }
        }


        public void OnException(ExceptionContext context)
        {
            int exceptionId;
            lock (_lock)
            {
                // Generate a unique ID for the exception
                exceptionId = ++_exceptionIdCounter;
            }

            try
            {

                // Log the exception details to the wwwroot/logs folder
                LogException(context.Exception, exceptionId);

                // Create a RuleResponse object with error details
                EncyptedResponse ruleResponse = new EncyptedResponse();

                ruleResponse.response = $"Exception ID: {exceptionId}, Message: {context.Exception.StackTrace}";
                string response = "";
                if (isEnc)
                {
                    response = Protect.EncryptAES(JsonConvert.SerializeObject(ruleResponse), SD.AesKey);
                }
                else
                {
                    response = JsonConvert.SerializeObject(ruleResponse);
                }



                // Set the result to return the RuleResponse object
                context.Result = new JsonResult("sda")
                {
                    StatusCode = 500 // Internal Server Error
                };

                // Mark the exception as handled
                context.ExceptionHandled = true;
            }
            catch (Exception ex)
            {
                // Log any failure during logging itself
                LogException(ex, exceptionId);

                // Create a RuleResponse object with error details
                EncyptedResponse respondwith = new EncyptedResponse();
                respondwith.response = $"Exception ID: {exceptionId}, Message: {context.Exception.Message}";
                string response = "";
                if (isEnc)
                {
                    response = Protect.EncryptAES(JsonConvert.SerializeObject(respondwith), SD.AesKey);
                }
                else
                {
                    response = JsonConvert.SerializeObject(respondwith);
                }


                // Set the result to return the RuleResponse object
                context.Result = new JsonResult(response)
                {
                    StatusCode = 500 // Internal Server Error
                };

                // Mark the exception as handled
                context.ExceptionHandled = true;
            }
        }
    }

}
