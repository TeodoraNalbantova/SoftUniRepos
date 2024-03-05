using ASP.NETCoreDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NETCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // Every method that returns IActionResult is Action
        // Every action corresponds to route
        public IActionResult Index()
        {
            // Razor има два уникални обекта
            // ViewData -> се работи с него като с Dictionary 
            // ViewBag -> се работи с него като object,динамичен
            // И двете се използват за едно и също нещо за предаване на данни между бизнес логиката т.е. контролера и view

            ViewData["MyData"] = "I am inserting data from controller in view";
            ViewBag.Result = "Бръм бръм";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [ActionName("AboutMvc")]
        public IActionResult Info()
        {
            return this.Redirect("https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
