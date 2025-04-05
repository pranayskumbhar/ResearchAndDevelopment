using Microsoft.AspNetCore.Mvc;
using ResearchAndDevelopment.Models;
using Repository;
using System.Diagnostics;

namespace ResearchAndDevelopment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {

            _logger = logger;
        }



        public IActionResult Index()
        {


            //TextLogWriter log = new TextLogWriter();
            //log.Log("Hellow World");
            //log.Log("Hi");
            //log.Log("Above blankskudyaksldh a;sky");


            //ExcelLogWriter log1 = new ExcelLogWriter();
            //log1.Log("Index action was called.");
            //log1.Log("Index action was called.");
            //log1.Log("sdfsdlfih");
            //log1.Log("flskdhflskdf");


             jsfhdgajs();


            return View();
        }




        void jsfhdgajs()
        {

            //TextLogWriter log = new TextLogWriter();
            //log.Log("Hellow World");
            //log.Log("Hi");
            //log.Log("Above blankskudyaksldh a;sky");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
