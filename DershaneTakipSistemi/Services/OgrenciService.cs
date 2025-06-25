// Services/OgrenciService.cs

using DershaneTakipSistemi.Data; // DbContext için gerekli
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Services
{
    // Bu bizim "yardımcı" sınıfımız. Controller'ın tüm yükünü bu sınıf alacak.
    public class OgrenciService
    {
        private readonly ApplicationDbContext _context;

        // Bu sınıfın çalışabilmesi için veritabanı context'ine ihtiyacı var.
        // Onu constructor üzerinden alıyoruz.
        public OgrenciService(ApplicationDbContext context)
        {
            _context = context;
        }

        // OgrencisController'daki Index metodunun içindeki kodun aynısı.
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

        // OgrencisController'daki Edit(GET) ve Delete(GET) için kullanılan kod.
        public async Task<Ogrenci?> GetOgrenciByIdAsync(int id)
        {
            return await _context.Ogrenciler
                .Include(o => o.Sinifi) // Sınıf bilgisini de getirelim
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // OgrencisController'daki Create(POST) metodunun kodu.
        public async Task CreateOgrenciAsync(Ogrenci ogrenci)
        {
            _context.Add(ogrenci);
            await _context.SaveChangesAsync();
        }

        // OgrencisController'daki Edit(POST) metodunun kodu.
        public async Task UpdateOgrenciAsync(Ogrenci ogrenci)
        {
            _context.Update(ogrenci);
            await _context.SaveChangesAsync();
        }

        // OgrencisController'daki DeleteConfirmed(POST) metodunun kodu.
        public async Task DeleteOgrenciAsync(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci != null)
            {
                _context.Ogrenciler.Remove(ogrenci);
                await _context.SaveChangesAsync();
            }
        }

        // OgrencisController'daki OgrenciExists metodunun kodu.
        public bool OgrenciExists(int id)
        {
            return _context.Ogrenciler.Any(e => e.Id == id);
        }

        // Dropdown listesini doldurmak için kullanılan yardımcı metodun kodu.
        public SelectList GetSinifSelectList(object? seciliSinif = null)
        {
            var siniflar = _context.Siniflar.OrderBy(s => s.Ad).ToList();
            return new SelectList(siniflar, "Id", "Ad", seciliSinif);
        }
    }
}
