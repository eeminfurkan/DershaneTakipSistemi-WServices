using DershaneTakipSistemi.Models;
using DershaneTakipSistemi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonelsController : Controller
    {
        private readonly PersonelService _personelService;

        public PersonelsController(PersonelService personelService)
        {
            _personelService = personelService;
        }

        // GET: Personels
        public async Task<IActionResult> Index(string aramaMetniPersonel)
        {
            ViewData["GecerliAramaPersonel"] = aramaMetniPersonel;
            var model = await _personelService.GetPersonellerAsync(aramaMetniPersonel);
            return View(model);
        }

        // GET: Personels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Soyad,Gorevi,AktifMi")] Personel personel)
        {
            if (ModelState.IsValid)
            {
                await _personelService.CreatePersonelAsync(personel);
                return RedirectToAction(nameof(Index));
            }
            return View(personel);
        }

        // GET: Personels/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var personel = await _personelService.GetPersonelByIdAsync(id);
            if (personel == null)
            {
                return NotFound();
            }
            return View(personel);
        }

        // POST: Personels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Soyad,Gorevi,AktifMi")] Personel personel)
        {
            if (id != personel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _personelService.UpdatePersonelAsync(personel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_personelService.PersonelExists(personel.Id))
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
            return View(personel);
        }

        // GET: Personels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var personel = await _personelService.GetPersonelByIdAsync(id.Value);
            if (personel == null)
            {
                return NotFound();
            }
            return View(personel);
        }

        // POST: Personels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _personelService.DeletePersonelAsync(id);
                TempData["SuccessMessage"] = "Personel başarıyla silindi.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Bu personel silinemedi. Personele ait başka kayıtlar olabilir.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
