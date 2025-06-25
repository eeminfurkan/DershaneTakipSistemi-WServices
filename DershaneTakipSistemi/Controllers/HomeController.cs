// Controllers/HomeController.cs

using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services; // <-- Yardýmcý sýnýfýmýzý ekledik
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DershaneTakipSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardService _dashboardService;

        // HomeController artýk ApplicationDbContext'e deðil, DashboardService'e baðýmlý.
        public HomeController(ILogger<HomeController> logger, DashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // Tüm veri çekme iþini tek bir metotla yardýmcý sýnýfa devrediyoruz.
            var viewModel = await _dashboardService.GetDashboardDataAsync();
            return View(viewModel);
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
    }
}
