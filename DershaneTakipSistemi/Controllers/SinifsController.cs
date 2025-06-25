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
using System.IO; // MemoryStream için using eklendi
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için
// Diğer using'ler (System, Linq, Task, Mvc, DbContext, Models, Authorization) zaten olmalı


namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SinifsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SinifsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== YENİ YARDIMCI METOT (Personel/Öğretmenler için) =====
        private void PersonelSelectListesiniYukle(object? seciliOgretmen = null)
        {
            // Sadece belirli bir göreve sahip personelleri öğretmen olarak listeleyebiliriz
            // Veya tüm personelleri listeleyebiliriz. Şimdilik tümünü listeleyelim.
            // İleride Personel modeline "Rol" veya "Pozisyon" alanı eklenirse filtreleme yapılabilir.
            var personellerSorgusu = _context.Personeller
                                        .OrderBy(p => p.Ad)
                                        .ThenBy(p => p.Soyad)
                                        .Select(p => new { // AdSoyad'ı birleştirelim
                                            Id = p.Id,
                                            TamAd = p.Ad + " " + p.Soyad
                                        });
            ViewData["OgretmenPersonelId"] = new SelectList(personellerSorgusu, "Id", "TamAd", seciliOgretmen);
        }

        // GET: Sinifs
        public async Task<IActionResult> Index()
        {
            var siniflar = _context.Siniflar
                                       .Include(s => s.SorumluOgretmen) // <-- Sorumlu Öğretmen bilgisini de çek
                                       .Include(s => s.Ogrenciler) // <-- BU SATIRI EKLEYİN
                                       .OrderBy(s => s.Ad);
            return View(await siniflar.ToListAsync());
        }

        

        // GET: Sinifs/Create
        public IActionResult Create()
        {
            PersonelSelectListesiniYukle(); // <-- Eklendi

            return View();
        }

        // POST: Sinifs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sinif sinif) // [Bind] attribute'unu kaldırdık veya OgretmenPersonelId'yi eklemeliyiz
        {
            ModelState.Remove(nameof(sinif.SorumluOgretmen)); // Navigation property için
            ModelState.Remove(nameof(sinif.Ogrenciler));     // Diğer navigation property için

            if (ModelState.IsValid)
            {
                _context.Add(sinif);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PersonelSelectListesiniYukle(sinif.OgretmenPersonelId); // <-- Eklendi

            return View(sinif);
        }

        // GET: Sinifs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
                var sinif = await _context.Siniflar.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            if (sinif == null)
            {
                return NotFound();
            }
            PersonelSelectListesiniYukle(sinif.OgretmenPersonelId); // <-- Eklendi

            return View(sinif);
        }

        // POST: Sinifs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind attribute'ü güncellendi. OgretmenPersonelId'yi de ekledik.
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,OgretmenPersonelId")] Sinif sinif)
        {
            if (id != sinif.Id)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(sinif.SorumluOgretmen));
            ModelState.Remove(nameof(sinif.Ogrenciler));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sinif);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinifExists(sinif.Id))
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
            PersonelSelectListesiniYukle(sinif.OgretmenPersonelId);

            return View(sinif);
        }

        // GET: Sinifs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinif = await _context.Siniflar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinif == null)
            {
                return NotFound();
            }

            return View(sinif);
        }

        // POST: Sinifs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sinif = await _context.Siniflar.FindAsync(id);
            if (sinif == null)
            {
                return NotFound();
            }

            try
            {
                _context.Siniflar.Remove(sinif);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sınıf başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */) // Exception detayını şimdilik kullanmıyoruz
            {
                // Hatanın nedenini bilerek daha spesifik bir mesaj veriyoruz.
                ModelState.AddModelError(string.Empty, "Bu sınıf silinemedi. Sınıfa kayıtlı öğrenciler olabilir. Lütfen önce bu sınıftaki öğrencileri başka bir sınıfa atayın veya öğrencilerin sınıf atamasını kaldırın.");
                // Silme başarısız olduğu için Delete onay sayfasını tekrar gösterelim.
                // Eğer sınıf detaylarını da göstermek isterseniz, 'sinif' nesnesini
                // ilişkili öğrencileriyle (.Include) tekrar çekmeniz gerekebilir.
                // Şimdilik sadece 'sinif' nesnesini gönderelim.
                return View("Delete", sinif);
            }
            catch (Exception /* ex */) // Beklenmedik diğer hatalar
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Kayıt silinemedi.");
                return View("Delete", sinif);
            }
        }

        private bool SinifExists(int id)
        {
            return _context.Siniflar.Any(e => e.Id == id);
        }
    }
}
