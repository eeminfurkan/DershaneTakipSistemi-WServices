// Controllers/KasaHareketisController.cs

using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KasaHareketisController : Controller
    {
        private readonly KasaHareketiService _kasaHareketiService;

        public KasaHareketisController(KasaHareketiService kasaHareketiService)
        {
            _kasaHareketiService = kasaHareketiService;
        }

        // GET: KasaHareketis
        public async Task<IActionResult> Index(DateTime? baslangicTarihi, DateTime? bitisTarihi, Kategori? kategori)
        {
            var filtrelenmisListe = await _kasaHareketiService.GetKasaHareketleriAsync(baslangicTarihi, bitisTarihi, kategori);

            // Özet Hesaplamaları Controller'da yapmaya devam edebiliriz, çünkü bu bir sunum mantığıdır.
            decimal toplamGelir = filtrelenmisListe.Where(k => k.HareketYonu == HareketYonu.Giris).Sum(k => k.Tutar);
            decimal toplamGider = filtrelenmisListe.Where(k => k.HareketYonu == HareketYonu.Cikis).Sum(k => k.Tutar);
            decimal bakiye = toplamGelir - toplamGider;

            ViewBag.ToplamGelir = toplamGelir.ToString("C");
            ViewBag.ToplamGider = toplamGider.ToString("C");
            ViewBag.Bakiye = bakiye.ToString("C");

            // Filtreleme elemanlarını View'a geri gönderme
            ViewBag.KategoriListesi = new SelectList(Enum.GetValues(typeof(Kategori)), kategori);
            ViewBag.MevcutBaslangicTarihi = baslangicTarihi?.ToString("yyyy-MM-dd");
            ViewBag.MevcutBitisTarihi = bitisTarihi?.ToString("yyyy-MM-dd");

            return View(filtrelenmisListe);
        }

        // GET: KasaHareketis/Create
        public IActionResult Create()
        {
            ViewData["OgrenciId"] = _kasaHareketiService.GetOgrenciSelectList();
            ViewData["PersonelId"] = _kasaHareketiService.GetPersonelSelectList();
            ViewData["HareketYonu"] = new SelectList(Enum.GetValues(typeof(HareketYonu)));
            ViewData["Kategori"] = new SelectList(Enum.GetValues(typeof(Kategori)));
            ViewData["OdemeYontemi"] = new SelectList(Enum.GetValues(typeof(OdemeYontemi)));
            return View();
        }

        // POST: KasaHareketis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KasaHareketi kasaHareketi)
        {
            if (ModelState.IsValid)
            {
                await _kasaHareketiService.CreateKasaHareketiAsync(kasaHareketi);
                return RedirectToAction(nameof(Index));
            }
            // Hata durumunda dropdown'ları tekrar doldur
            ViewData["OgrenciId"] = _kasaHareketiService.GetOgrenciSelectList(kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = _kasaHareketiService.GetPersonelSelectList(kasaHareketi.PersonelId);
            ViewData["HareketYonu"] = new SelectList(Enum.GetValues(typeof(HareketYonu)), kasaHareketi.HareketYonu);
            ViewData["Kategori"] = new SelectList(Enum.GetValues(typeof(Kategori)), kasaHareketi.Kategori);
            ViewData["OdemeYontemi"] = new SelectList(Enum.GetValues(typeof(OdemeYontemi)), kasaHareketi.OdemeYontemi);
            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kasaHareketi = await _kasaHareketiService.GetKasaHareketiByIdAsync(id.Value);
            if (kasaHareketi == null) return NotFound();

            // --- DÜZELTME BAŞLANGICI ---
            // Edit sayfasını açarken TÜM dropdown listelerini dolduruyoruz.
            ViewData["OgrenciId"] = _kasaHareketiService.GetOgrenciSelectList(kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = _kasaHareketiService.GetPersonelSelectList(kasaHareketi.PersonelId);
            ViewData["HareketYonu"] = new SelectList(Enum.GetValues(typeof(HareketYonu)), kasaHareketi.HareketYonu);
            ViewData["Kategori"] = new SelectList(Enum.GetValues(typeof(Kategori)), kasaHareketi.Kategori);
            ViewData["OdemeYontemi"] = new SelectList(Enum.GetValues(typeof(OdemeYontemi)), kasaHareketi.OdemeYontemi);
            // --- DÜZELTME SONU ---

            return View(kasaHareketi);
        }

        // POST: KasaHareketis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KasaHareketi kasaHareketi)
        {
            if (id != kasaHareketi.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _kasaHareketiService.UpdateKasaHareketiAsync(kasaHareketi);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_kasaHareketiService.KasaHareketiExists(kasaHareketi.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // --- DÜZELTME BAŞLANGICI ---
            // Eğer model geçersizse ve form tekrar gösterilecekse,
            // TÜM dropdown listelerini tekrar dolduruyoruz.
            ViewData["OgrenciId"] = _kasaHareketiService.GetOgrenciSelectList(kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = _kasaHareketiService.GetPersonelSelectList(kasaHareketi.PersonelId);
            ViewData["HareketYonu"] = new SelectList(Enum.GetValues(typeof(HareketYonu)), kasaHareketi.HareketYonu);
            ViewData["Kategori"] = new SelectList(Enum.GetValues(typeof(Kategori)), kasaHareketi.Kategori);
            ViewData["OdemeYontemi"] = new SelectList(Enum.GetValues(typeof(OdemeYontemi)), kasaHareketi.OdemeYontemi);
            // --- DÜZELTME SONU ---

            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var kasaHareketi = await _kasaHareketiService.GetKasaHareketiByIdAsync(id.Value);
            if (kasaHareketi == null) return NotFound();

            return View(kasaHareketi);
        }

        // POST: KasaHareketis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _kasaHareketiService.DeleteKasaHareketiAsync(id);
            TempData["SuccessMessage"] = "Kasa hareketi başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
