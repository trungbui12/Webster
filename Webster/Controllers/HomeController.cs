using Microsoft.AspNetCore.Mvc;

namespace Webster.Controllers
{
    public class HomeController : Controller
    {
        // Trang chủ
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Trang About
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        // Trang Contact (nếu sau này dùng)
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
    }
}
