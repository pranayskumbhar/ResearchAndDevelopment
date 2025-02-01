using Microsoft.AspNetCore.Mvc;
using Repository;

namespace ResearchAndDevelopment.Controllers
{
    public class ValueController : Controller
    {
        public IActionResult Index()
        {
            TextLogWriter log = new TextLogWriter();
            log.Log("Hellow World");
            log.Log("Hi");
            log.Log("Above blankskudyaksldh a;sky");

            return View();
        }
    }
}
