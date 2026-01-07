using Microsoft.AspNetCore.Mvc;

namespace HealthBigData.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCityComponent(string cityName)
        {
            return ViewComponent("_DefaultCityChartJobComponent", new { cityName = cityName });
        }
    }
}
