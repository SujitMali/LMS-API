using System.Web.Mvc;

namespace Lib_Mang_Sys_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

    }

}
