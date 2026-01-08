using Microsoft.AspNetCore.Mvc;

namespace HealthBigData.ViewComponets.AdminComponents
{
    public class _AdminLayoutHeadComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
