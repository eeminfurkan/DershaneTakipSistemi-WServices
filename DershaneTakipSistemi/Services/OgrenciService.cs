using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Services
{
    public class OgrenciService
    {
        private readonly ApplicationDbContext _context;

        public OgrenciService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ogrenci>> GetOgrencilerAsync(string aramaMetni)
        {
            var ogrencilerSorgusu = _context.Ogrenciler.Include(o => o.Sinifi).AsQueryable();

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                ogrencilerSorgusu = ogrencilerSorgusu.Where(o =>
                    (o.Ad != null && o.Ad.ToLower().Contains(aramaMetni.ToLower())) ||
                    (o.Soyad != null && o.Soyad.ToLower().Contains(aramaMetni.ToLower()))
                );
            }

            return await ogrencilerSorgusu
                .OrderBy(o => o.Ad)
                .ThenBy(o => o.Soyad)
                .ToListAsync();
        }

        public async Task<Ogrenci?> GetOgrenciByIdAsync(int id)
        {
            return await _context.Ogrenciler
                .Include(o => o.Sinifi)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task CreateOgrenciAsync(Ogrenci ogrenci)
        {
            _context.Add(ogrenci);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOgrenciAsync(Ogrenci ogrenci)
        {
            _context.Update(ogrenci);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOgrenciAsync(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci != null)
            {
                _context.Ogrenciler.Remove(ogrenci);
                await _context.SaveChangesAsync();
            }
        }

        public bool OgrenciExists(int id)
        {
            return _context.Ogrenciler.Any(e => e.Id == id);
        }

        public SelectList GetSinifSelectList(object? seciliSinif = null)
        {
            var siniflar = _context.Siniflar.OrderBy(s => s.Ad).ToList();
            return new SelectList(siniflar, "Id", "Ad", seciliSinif);
        }
    }
}
