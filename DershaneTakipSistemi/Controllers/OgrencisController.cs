// Controllers/OgrencisController.cs

using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services; // <-- Yardımcı sınıfımızı kullanabilmek için ekledik
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Sadece Edit'teki try-catch için lazım

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OgrencisController : Controller
    {
        // Controller artık veritabanına değil, OgrenciService yardımcısına bağımlı.
        private readonly OgrenciService _ogrenciService;

        public OgrencisController(OgrenciService ogrenciService)
        {
            _ogrenciService = ogrenciService;
        }

        // GET: Ogrencis
        public async Task<IActionResult> Index(string aramaMetni)
        {
            ViewData["GecerliArama"] = aramaMetni;
            // Tüm listeleme işini yardımcı sınıfa devrediyoruz.
            var model = await _ogrenciService.GetOgrencilerAsync(aramaMetni);
            return View(model);
        }

        // GET: Ogrencis/Create
        public IActionResult Create()
        {
            // Dropdown listesini doldurma işini yardımcıya devrediyoruz.
            ViewData["SinifId"] = _ogrenciService.GetSinifSelectList();
            return View();
        }

        // POST: Ogrencis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                // Öğrenciyi veritabanına ekleme işini yardımcıya devrediyoruz.
                await _ogrenciService.CreateOgrenciAsync(ogrenci);
                return RedirectToAction(nameof(Index));
            }
            // Hata durumunda dropdown'ı tekrar dolduruyoruz.
            ViewData["SinifId"] = _ogrenciService.GetSinifSelectList(ogrenci.SinifId);
            return View(ogrenci);
        }

        // GET: Ogrencis/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Öğrenciyi bulma işini yardımcıya devrediyoruz.
            var ogrenci = await _ogrenciService.GetOgrenciByIdAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            ViewData["SinifId"] = _ogrenciService.GetSinifSelectList(ogrenci.SinifId);
            return View(ogrenci);
        }

        // POST: Ogrencis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogrenci ogrenci)
        {
            if (id != ogrenci.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Öğrenciyi güncelleme işini yardımcıya devrediyoruz.
                    await _ogrenciService.UpdateOgrenciAsync(ogrenci);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // "Exists" kontrolünü bile yardımcıya devrediyoruz.
                    if (!_ogrenciService.OgrenciExists(ogrenci.Id))
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
            ViewData["SinifId"] = _ogrenciService.GetSinifSelectList(ogrenci.SinifId);
            return View(ogrenci);
        }

        // GET: Ogrencis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ogrenci = await _ogrenciService.GetOgrenciByIdAsync(id.Value);
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
            try
            {
                // Silme işini yardımcıya devrediyoruz.
                await _ogrenciService.DeleteOgrenciAsync(id);
                TempData["SuccessMessage"] = "Öğrenci başarıyla silindi.";
            }
            catch (DbUpdateException)
            {
                // Silme işlemi başarısız olursa (ilişkili veri varsa) kullanıcıyı bilgilendir.
                TempData["ErrorMessage"] = "Bu öğrenci silinemedi. Öğrenciye ait ödeme kaydı olabilir.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
