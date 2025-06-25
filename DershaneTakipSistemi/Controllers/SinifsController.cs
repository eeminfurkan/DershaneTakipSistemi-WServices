// Controllers/SinifsController.cs

using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services; // <-- Yardımcı sınıfımızı ekledik
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SinifsController : Controller
    {
        private readonly SinifService _sinifService;

        public SinifsController(SinifService sinifService)
        {
            _sinifService = sinifService;
        }

        // GET: Sinifs
        public async Task<IActionResult> Index()
        {
            var model = await _sinifService.GetSiniflarAsync();
            return View(model);
        }

        // GET: Sinifs/Create
        public IActionResult Create()
        {
            ViewData["OgretmenPersonelId"] = _sinifService.GetPersonelSelectList();
            return View();
        }

        // POST: Sinifs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sinif sinif)
        {
            // Bu navigation property'leri model state'ten çıkarmak,
            // gereksiz validasyon hatalarını önler.
            ModelState.Remove(nameof(sinif.SorumluOgretmen));
            ModelState.Remove(nameof(sinif.Ogrenciler));

            if (ModelState.IsValid)
            {
                await _sinifService.CreateSinifAsync(sinif);
                return RedirectToAction(nameof(Index));
            }
            ViewData["OgretmenPersonelId"] = _sinifService.GetPersonelSelectList(sinif.OgretmenPersonelId);
            return View(sinif);
        }

        // GET: Sinifs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sinif = await _sinifService.GetSinifByIdAsync(id);
            if (sinif == null)
            {
                return NotFound();
            }
            ViewData["OgretmenPersonelId"] = _sinifService.GetPersonelSelectList(sinif.OgretmenPersonelId);
            return View(sinif);
        }

        // POST: Sinifs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    await _sinifService.UpdateSinifAsync(sinif);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_sinifService.SinifExists(sinif.Id))
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
            ViewData["OgretmenPersonelId"] = _sinifService.GetPersonelSelectList(sinif.OgretmenPersonelId);
            return View(sinif);
        }

        // GET: Sinifs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sinif = await _sinifService.GetSinifByIdAsync(id.Value);
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
            try
            {
                await _sinifService.DeleteSinifAsync(id);
                TempData["SuccessMessage"] = "Sınıf başarıyla silindi.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Bu sınıf silinemedi. Sınıfa kayıtlı öğrenciler olabilir.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
