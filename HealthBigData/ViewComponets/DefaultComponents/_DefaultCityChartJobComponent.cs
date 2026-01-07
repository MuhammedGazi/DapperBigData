using HealthBigData.Repositories.HastaRepositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthBigData.ViewComponets.DefaultComponents
{
    public class _DefaultCityChartJobComponent(ICityRepository _repository) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string cityName)
        {
            var cityDescription = await _repository.GetCityAsync(cityName);
            return View(cityDescription);
        }
    }
}
