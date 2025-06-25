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
// ClosedXML ve IO using'leri kaldırıldı.
// Diğer using'ler (System, Linq, Task, Mvc, DbContext, Models, Authorization) zaten olmalı

namespace DershaneTakipSistemi.Controllers
{

    [Authorize(Roles = "Admin")] // <-- BU SATIRI EKLE

    public class OgrencisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OgrencisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== YENİ YARDIMCI METOT =====
        private void SinifSelectListesiniYukle(object? seciliSinif = null)
        {
            var siniflarSorgusu = _context.Siniflar.OrderBy(s => s.Ad);
            ViewData["SinifId"] = new SelectList(siniflarSorgusu, "Id", "Ad", seciliSinif);
        }
        // =============================

        // GET: Ogrencis
        // Arama parametresi eklendi: string aramaMetni
        public async Task<IActionResult> Index(string aramaMetni)
        {
            // ViewData'ya mevcut arama metnini aktaralım ki View'da gösterilebilsin.
            ViewData["GecerliArama"] = aramaMetni;

            // Başlangıçta tüm öğrencileri seçelim.
            // IQueryable<Ogrenci> sorgusu oluşturuyoruz ki veritabanına sadece
            // en son filtrelenmiş sorgu gitsin.
            var ogrencilerSorgusu = _context.Ogrenciler
                               .Include(o => o.Sinifi) // <-- Sınıf bilgisini de çek
                               .AsQueryable(); // .AsQueryable() burada opsiyonel ama iyi bir alışkanlık
            // veya: var ogrencilerSorgusu = _context.Ogrenciler.AsQueryable();

            // Eğer aramaMetni boş değilse, filtreleme yap.
            if (!String.IsNullOrEmpty(aramaMetni))
            {
                // Ad veya Soyad alanında aramaMetni'ni içeren öğrencileri bul.
                // Büyük/küçük harf duyarsız arama için ToLower() kullanıyoruz.
                // EF Core 6+ string.Contains'i SQL LIKE '%...%' sorgusuna çevirir.
                ogrencilerSorgusu = ogrencilerSorgusu.Where(o =>
                    (o.Ad != null && o.Ad.ToLower().Contains(aramaMetni.ToLower())) ||
                    (o.Soyad != null && o.Soyad.ToLower().Contains(aramaMetni.ToLower()))
                // İstersen TCKimlik veya diğer alanları da ekleyebilirsin:
                // || (o.TCKimlik != null && o.TCKimlik.Contains(aramaMetni))
                );
            }
            // Son olarak, filtrelenmiş (veya filtrelenmemiş) sorguyu sıralayıp
            // veritabanından çekip View'a gönderelim.
            var filtrelenmisOgrenciler = await ogrencilerSorgusu
                                                .OrderBy(o => o.Ad)
                                                .ThenBy(o => o.Soyad)
                                                .ToListAsync();

            return View(filtrelenmisOgrenciler);
        }

        

        // GET: Ogrencis/Create
        public IActionResult Create()
        {
            SinifSelectListesiniYukle(); // <-- Eklendi
            return View();
        }

        // POST: Ogrencis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ogrenci);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            SinifSelectListesiniYukle(ogrenci.SinifId); // <-- Eklendi (ModelState geçersizse)
            return View(ogrenci);
        }

        // GET: Ogrencis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            SinifSelectListesiniYukle(ogrenci.SinifId); // <-- Eklendi (mevcut sınıfı seçili getirmek için)
            return View(ogrenci);
        }

        // POST: Ogrencis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogrenci ogrenci)
        {
            if (id != ogrenci.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(ogrenci.Sinifi)); // Navigation property için

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogrenci);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgrenciExists(ogrenci.Id))
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
            SinifSelectListesiniYukle(ogrenci.SinifId); // <-- Eklendi (ModelState geçersizse)

            return View(ogrenci);
        }

        // GET: Ogrencis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        // POST: Ogrencis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci == null) // Öğrenci bulunamadıysa NotFound dönelim
            {
                return NotFound();
            }

            try
            {
                _context.Ogrenciler.Remove(ogrenci);
                await _context.SaveChangesAsync(); // Hata potansiyeli olan satır try içinde
                TempData["SuccessMessage"] = "Öğrenci başarıyla silindi."; // Başarı mesajı (opsiyonel)
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) // EF Core veritabanı güncelleme hatası
            {
                // Hatanın nedenini anlamaya çalışalım (InnerException genellikle asıl hatayı içerir)
                // SQL Server'da Foreign Key hatası genellikle 547 numaralı hatadır.
                // Burada daha detaylı hata loglaması da yapılabilir.
                // Şimdilik genel bir mesaj verelim:

                // Silme işlemi başarısız olduğunda kullanıcıyı tekrar Delete onay sayfasına yönlendirelim
                // ve bir hata mesajı gösterelim.
                ModelState.AddModelError(string.Empty, "Bu öğrenci silinemedi. Öğrenciye ait ödeme kaydı olabilir. Lütfen önce ilişkili kayıtları silin.");

                // Kullanıcıya gösterilecek Delete onay sayfasını tekrar hazırlamamız gerekiyor.
                // Modelimizi (ogrenci nesnesi) tekrar View'a göndermeliyiz.
                return View("Delete", ogrenci); // "Delete" view'ını ogrenci modeliyle tekrar göster
            }
            catch (Exception ex) // Beklenmedik diğer hatalar için
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Kayıt silinemedi.");
                // Loglama yapılabilir: _logger.LogError(ex, "Öğrenci silinirken hata oluştu");
                return View("Delete", ogrenci);
            }
        }

        private bool OgrenciExists(int id)
        {
            return _context.Ogrenciler.Any(e => e.Id == id);
        }

        // ===== EXCEL EXPORT ACTION KALDIRILDI =====
    }
}