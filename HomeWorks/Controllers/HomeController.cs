using HomeWorks.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HomeWorks.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Logout()
        {

            // �M�� Session
            HttpContext.Session.Clear();

            // ���w�V��n�X�᪺����
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize(Policy = "PremiumUser")]
        public IActionResult PremiumContent()
        {
            return View();
        }

    }
}
