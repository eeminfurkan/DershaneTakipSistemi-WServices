using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KasaHareketisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KasaHareketisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KasaHareketis
        public async Task<IActionResult> Index(DateTime? baslangicTarihi, DateTime? bitisTarihi, Kategori? kategori)
        {
            var sorgu = _context.KasaHareketleri
                                .Include(k => k.Ogrenci)
                                .Include(k => k.Personel)
                                .AsQueryable();

            // Filtreleme
            if (baslangicTarihi.HasValue)
            {
                sorgu = sorgu.Where(k => k.Tarih >= baslangicTarihi.Value);
            }
            if (bitisTarihi.HasValue)
            {
                sorgu = sorgu.Where(k => k.Tarih <= bitisTarihi.Value);
            }
            if (kategori.HasValue)
            {
                sorgu = sorgu.Where(k => k.Kategori == kategori.Value);
            }

            var filtrelenmisListe = await sorgu.OrderByDescending(k => k.Tarih).ToListAsync();

            // Özet Hesaplamalar
            decimal toplamGelir = filtrelenmisListe.Where(k => k.HareketYonu == HareketYonu.Giris).Sum(k => k.Tutar);
            decimal toplamGider = filtrelenmisListe.Where(k => k.HareketYonu == HareketYonu.Cikis).Sum(k => k.Tutar);
            decimal bakiye = toplamGelir - toplamGider;

            // View'a gönderilecek verileri ViewData yerine ViewBag'e atıyoruz.
            ViewBag.ToplamGelir = toplamGelir.ToString("C");
            ViewBag.ToplamGider = toplamGider.ToString("C");
            ViewBag.Bakiye = bakiye.ToString("C");
            ViewBag.KategoriListesi = new SelectList(Enum.GetValues(typeof(Kategori)), kategori); // Kategori listesini ViewBag'e atıyoruz
            ViewBag.MevcutBaslangicTarihi = baslangicTarihi?.ToString("yyyy-MM-dd");
            ViewBag.MevcutBitisTarihi = bitisTarihi?.ToString("yyyy-MM-dd");

            return View(filtrelenmisListe);
        }


        // GET: KasaHareketis/Create
        public IActionResult Create()
        {
            // Dropdown listelerini doldurmak için View'a veri gönderiyoruz.
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "AdSoyad");
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "AdSoyad");
            // Enum'lar için listeler
            ViewData["HareketYonu"] = new SelectList(Enum.GetValues(typeof(HareketYonu)));
            ViewData["Kategori"] = new SelectList(Enum.GetValues(typeof(Kategori)));
            ViewData["OdemeYontemi"] = new SelectList(Enum.GetValues(typeof(OdemeYontemi)));
            return View();
        }

        // POST: KasaHareketis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Aciklama,Tarih,Tutar,HareketYonu,Kategori,OdemeYontemi,OgrenciId,PersonelId")] KasaHareketi kasaHareketi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kasaHareketi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Hata durumunda dropdown'ları tekrar doldur
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "AdSoyad", kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "AdSoyad", kasaHareketi.PersonelId);
            ViewData["HareketYonu"] = new SelectList(Enum.GetValues(typeof(HareketYonu)), kasaHareketi.HareketYonu);
            ViewData["Kategori"] = new SelectList(Enum.GetValues(typeof(Kategori)), kasaHareketi.Kategori);
            ViewData["OdemeYontemi"] = new SelectList(Enum.GetValues(typeof(OdemeYontemi)), kasaHareketi.OdemeYontemi);
            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kasaHareketi = await _context.KasaHareketleri.FindAsync(id);
            if (kasaHareketi == null)
            {
                return NotFound();
            }
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "Ad", kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "Ad", kasaHareketi.PersonelId);
            return View(kasaHareketi);
        }

        // POST: KasaHareketis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Aciklama,Tarih,Tutar,HareketYonu,Kategori,OdemeYontemi,OgrenciId,PersonelId")] KasaHareketi kasaHareketi)
        {
            if (id != kasaHareketi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kasaHareketi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KasaHareketiExists(kasaHareketi.Id))
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
            ViewData["OgrenciId"] = new SelectList(_context.Ogrenciler, "Id", "Ad", kasaHareketi.OgrenciId);
            ViewData["PersonelId"] = new SelectList(_context.Personeller, "Id", "Ad", kasaHareketi.PersonelId);
            return View(kasaHareketi);
        }

        // GET: KasaHareketis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kasaHareketi = await _context.KasaHareketleri
                .Include(k => k.Ogrenci)
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kasaHareketi == null)
            {
                return NotFound();
            }

            return View(kasaHareketi);
        }

        // POST: KasaHareketis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kasaHareketi = await _context.KasaHareketleri.FindAsync(id);
            if (kasaHareketi != null)
            {
                _context.KasaHareketleri.Remove(kasaHareketi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KasaHareketiExists(int id)
        {
            return _context.KasaHareketleri.Any(e => e.Id == id);
        }
    }
}
