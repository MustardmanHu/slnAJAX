using Microsoft.AspNetCore.Mvc;

namespace slnAJAX.Controllers
{
    public class APIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult wait()
        {
            System.Threading.Thread.Sleep(5000);
            return Content("Hello");
        }
        public IActionResult Hello(string txt)
        {
            if (String.IsNullOrEmpty(txt))
            {
                txt = "ajax";
            }
            return Content("Hello"+txt);
        }
    }
}
