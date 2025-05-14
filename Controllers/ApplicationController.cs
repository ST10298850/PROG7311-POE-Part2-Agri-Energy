using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    /// <summary>
    /// Controller responsible for handling farmer application-related actions.
    /// </summary>
    public class ApplicationController : Controller
    {
        /// <summary>
        /// Displays the farmer application form.
        /// </summary>
        /// <returns>The application view.</returns>
        [HttpGet]
        public ActionResult Application()
        {
            return View();
        }
    }
}