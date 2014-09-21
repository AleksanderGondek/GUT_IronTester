using System.Web.Mvc;

namespace IronTester.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllRequests()
        {
            return View();
        }
    }
}