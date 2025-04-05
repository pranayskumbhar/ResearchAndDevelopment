using Microsoft.AspNetCore.Mvc;
using Repository;

namespace ResearchAndDevelopment.Controllers
{
    public class ValueController : Controller
    {
        public IActionResult Index()
        {

            Logger.LogIntoText();
            Logger.LogIntoText("Hi ", "Bro", "Whats", "Up", "Lets", "Go", "We", "conqueror");
            Logger.LogIntoText("Hare Krishna ", "Radhe Radhe");
            Logger.LogIntoText("Ganapti Bappa Morya ", "Om namah Shivay");
 
            Logger.LogIntoExcel();
            Logger.LogIntoExcel("Hi ", "Bro", "Whats", "Up", "Lets", "Go", "We", "conqueror");
            Logger.LogIntoExcel("Hare Krishna ", "Radhe Radhe");
            Logger.LogIntoExcel("Ganapti Bappa Morya ", "Om namah Shivay");
 
            
            Logger.LogIntoCsv();
            Logger.LogIntoCsv("Hi ", "Bro", "Whats", "Up", "Lets", "Go", "We", "conqueror");
            Logger.LogIntoCsv("Hare Krishna ", "Radhe Radhe");
            Logger.LogIntoCsv("Ganapti Bappa Morya ", "Om namah Shivay");
 
              
            return View();
        }
    }
}
