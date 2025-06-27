using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Services
{
    public class SinifService
    {
        private readonly ApplicationDbContext _context;

        public SinifService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sinif>> GetSiniflarAsync()
        {
            return await _context.Siniflar
                .Include(s => s.SorumluOgretmen)
                .Include(s => s.Ogrenciler)
                .OrderBy(s => s.Ad)
                .ToListAsync();
        }

        public async Task<Sinif?> GetSinifByIdAsync(int id)
        {
            return await _context.Siniflar.FindAsync(id);
        }

        public async Task CreateSinifAsync(Sinif sinif)
        {
            _context.Add(sinif);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSinifAsync(Sinif sinif)
        {
            _context.Update(sinif);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSinifAsync(int id)
        {
            var sinif = await _context.Siniflar.FindAsync(id);
            if (sinif != null)
            {
                _context.Siniflar.Remove(sinif);
                await _context.SaveChangesAsync();
            }
        }

        public bool SinifExists(int id)
        {
            return _context.Siniflar.Any(e => e.Id == id);
        }

        public SelectList GetPersonelSelectList(object? seciliOgretmen = null)
        {
            var personellerSorgusu = _context.Personeller
                .OrderBy(p => p.Ad)
                .ThenBy(p => p.Soyad)
                .Select(p => new {
                    Id = p.Id,
                    TamAd = p.Ad + " " + p.Soyad
                }).ToList();

            return new SelectList(personellerSorgusu, "Id", "TamAd", seciliOgretmen);
        }
    }
}
