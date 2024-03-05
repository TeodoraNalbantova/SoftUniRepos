

namespace ASP.NETCoreDemo.Controllers
{
    using ASP.NETCoreDemo.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    public class CarsController : Controller
    {
        private readonly ICarService carService;
        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(AddCarViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return this.View(model);
            }
            return this.RedirectToAction("Index", "Home");
        }
    }
}
