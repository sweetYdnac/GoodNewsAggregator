using Microsoft.AspNetCore.Mvc;

namespace by.Reba.Application.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
