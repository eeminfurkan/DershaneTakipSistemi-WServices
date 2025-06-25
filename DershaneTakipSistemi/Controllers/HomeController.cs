// Controllers/HomeController.cs

using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services; // <-- Yard�mc� s�n�f�m�z� ekledik
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DershaneTakipSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardService _dashboardService;

        // HomeController art�k ApplicationDbContext'e de�il, DashboardService'e ba��ml�.
        public HomeController(ILogger<HomeController> logger, DashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // T�m veri �ekme i�ini tek bir metotla yard�mc� s�n�fa devrediyoruz.
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
