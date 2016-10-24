using System.Web.Mvc;

namespace ScupTel.WebUI.Controllers
{
    public class HomeController : Controller
    {

       

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            ViewBag.SubTitle = "Portal do cliente";
            return View();
        }

       
    }
}