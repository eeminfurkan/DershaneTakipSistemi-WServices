using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OgrencisController : Controller
    {
        private readonly OgrenciService _ogrenciService;

        public OgrencisController(OgrenciService ogrenciService)
        {
            _ogrenciService = ogrenciService;
        }

        // GET: Ogrencis
        public async Task<IActionResult> Index(string aramaMetni)
        {
            ViewData["GecerliArama"] = aramaMetni;
            var model = await _ogrenciService.GetOgrencilerAsync(aramaMetni);
            return View(model);
        }

        // GET: Ogrencis/Create
        public IActionResult Create()
        {
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
                await _ogrenciService.CreateOgrenciAsync(ogrenci);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SinifId"] = _ogrenciService.GetSinifSelectList(ogrenci.SinifId);
            return View(ogrenci);
        }

        // GET: Ogrencis/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
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
                    await _ogrenciService.UpdateOgrenciAsync(ogrenci);
                }
                catch (DbUpdateConcurrencyException)
                {
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
                await _ogrenciService.DeleteOgrenciAsync(id);
                TempData["SuccessMessage"] = "Öğrenci başarıyla silindi.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Bu öğrenci silinemedi. Öğrenciye ait ödeme kaydı olabilir.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
