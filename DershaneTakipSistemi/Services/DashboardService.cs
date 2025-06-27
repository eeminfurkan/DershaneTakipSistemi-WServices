// Services/DashboardService.cs

using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DershaneTakipSistemi.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            if (_context.Ogrenciler == null || _context.KasaHareketleri == null)
            {
                return new DashboardViewModel();
            }

            var viewModel = new DashboardViewModel
            {
                ToplamOgrenciSayisi = await _context.Ogrenciler.CountAsync(),

                AktifOgrenciSayisi = await _context.Ogrenciler.CountAsync(o => o.AktifMi),

                ToplamOdemeTutari = await _context.KasaHareketleri
                    .Where(k => k.HareketYonu == HareketYonu.Giris && k.Kategori == Kategori.OgrenciOdemesi)
                    .SumAsync(k => k.Tutar),

                BugunkuOdemeSayisi = await _context.KasaHareketleri
                    .CountAsync(k => k.Kategori == Kategori.OgrenciOdemesi && k.Tarih.Date == DateTime.Today)
            };

            return viewModel;
        }
    }
}
