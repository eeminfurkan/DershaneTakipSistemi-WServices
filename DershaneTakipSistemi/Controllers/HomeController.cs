using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DershaneTakipSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (_context.Ogrenciler == null || _context.KasaHareketleri == null)
            {
                return View(new DashboardViewModel());
            }

            // ViewModel nesnesini olu�tur ve yeni sisteme g�re doldur
            var viewModel = new DashboardViewModel
            {
                // Toplam ��renci say�s�n� asenkron olarak al
                ToplamOgrenciSayisi = await _context.Ogrenciler.CountAsync(),

                // Aktif ��renci say�s�n� asenkron olarak al
                AktifOgrenciSayisi = await _context.Ogrenciler.CountAsync(o => o.AktifMi),

                // Toplam �deme tutar�n� KasaHareketleri'nden hesapla
                // Sadece "Giri�" olan ve kategorisi "OgrenciOdemesi" olanlar� topla
                ToplamOdemeTutari = await _context.KasaHareketleri
                                        .Where(k => k.HareketYonu == HareketYonu.Giris && k.Kategori == Kategori.OgrenciOdemesi)
                                        .SumAsync(k => k.Tutar),

                // Bug�nk� �deme say�s�n� KasaHareketleri'nden hesapla
                BugunkuOdemeSayisi = await _context.KasaHareketleri
                                        .CountAsync(k => k.Kategori == Kategori.OgrenciOdemesi && k.Tarih.Date == DateTime.Today)
            };

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