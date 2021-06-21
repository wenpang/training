using Training.Models;
using Microsoft.AspNetCore.Mvc;

namespace Training.Controllers
{
    public class DemoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            DemoViewModel model = new DemoViewModel()
            {
                ProductID = "0000000001",
                ProductNM = "機票",
                Quantity = 100,
                IsDisable = true
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(DemoViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ToJson([FromBody] DemoViewModel model)
        {
            return Json(model);
        }
    }
}
