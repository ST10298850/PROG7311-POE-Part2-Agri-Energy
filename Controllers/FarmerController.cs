using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    public class FarmerController : Controller
    {
        // GET: FarmerController
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
