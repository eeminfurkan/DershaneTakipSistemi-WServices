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
    [Authorize(Roles = "Admin")]

    public class PersonelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Personels
        public async Task<IActionResult> Index(string aramaMetniPersonel) // Parametre adı güncellendi
        {
            ViewData["GecerliAramaPersonel"] = aramaMetniPersonel; // ViewData anahtarı güncellendi

            var personellerSorgusu = from p in _context.Personeller select p;

            if (!String.IsNullOrEmpty(aramaMetniPersonel))
            {
                personellerSorgusu = personellerSorgusu.Where(p =>
                    (p.Ad != null && p.Ad.ToLower().Contains(aramaMetniPersonel.ToLower())) ||
                    (p.Soyad != null && p.Soyad.ToLower().Contains(aramaMetniPersonel.ToLower())) ||
                    (p.Gorevi != null && p.Gorevi.ToLower().Contains(aramaMetniPersonel.ToLower()))
                // İstersen TCKimlik veya diğer alanları da ekleyebilirsin
                );
            }

            var filtrelenmisPersoneller = await personellerSorgusu
                                                    .OrderBy(p => p.Ad)
                                                    .ThenBy(p => p.Soyad)
                                                    .ToListAsync();

            return View(filtrelenmisPersoneller);
        }

        

        // GET: Personels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind attribute'ü sadeleştirildi.
        public async Task<IActionResult> Create([Bind("Id,Ad,Soyad,Gorevi,AktifMi")] Personel personel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personel);
        }

        // GET: Personels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null)
            {
                return NotFound();
            }
            return View(personel);
        }

        // POST: Personels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind attribute'ü sadeleştirildi.
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
                    _context.Update(personel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonelExists(personel.Id))
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
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personeller
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var personel = await _context.Personeller.FindAsync(id);
            if (personel != null)
            {
                _context.Personeller.Remove(personel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonelExists(int id)
        {
            return _context.Personeller.Any(e => e.Id == id);
        }

        // ===== EXCEL EXPORT ACTION KALDIRILDI =====
    }
}