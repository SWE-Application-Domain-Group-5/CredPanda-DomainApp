using Microsoft.AspNetCore.Mvc;
namespace EliApp.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}