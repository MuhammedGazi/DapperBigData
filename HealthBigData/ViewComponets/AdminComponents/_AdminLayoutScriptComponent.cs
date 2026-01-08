using Microsoft.AspNetCore.Mvc;

namespace HealthBigData.ViewComponets.AdminComponents
{
    public class _AdminLayoutScriptComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
