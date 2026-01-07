using HealthBigData.Repositories.ChartRepositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthBigData.Controllers
{
    public class DashboardController(IDashboardChartRepository repository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await repository.GetDashboardChartAsync();
            return View(values);
        }
    }
}
