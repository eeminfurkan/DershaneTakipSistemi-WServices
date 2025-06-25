// Services/PersonelService.cs

using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Services
{
    // Bu bizim yeni "Personel İşleri Yardımcısı" sınıfımız.
    public class PersonelService
    {
        private readonly ApplicationDbContext _context;

        public PersonelService(ApplicationDbContext context)
        {
            _context = context;
        }

        // PersonelsController'daki Index metodunun mantığı
        public async Task<List<Personel>> GetPersonellerAsync(string aramaMetni)
        {
            var personellerSorgusu = _context.Personeller.AsQueryable();

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                var lowerAramaMetni = aramaMetni.ToLower();
                personellerSorgusu = personellerSorgusu.Where(p =>
                    (p.Ad != null && p.Ad.ToLower().Contains(lowerAramaMetni)) ||
                    (p.Soyad != null && p.Soyad.ToLower().Contains(lowerAramaMetni)) ||
                    (p.Gorevi != null && p.Gorevi.ToLower().Contains(lowerAramaMetni))
                );
            }

            return await personellerSorgusu
                .OrderBy(p => p.Ad)
                .ThenBy(p => p.Soyad)
                .ToListAsync();
        }

        // Personel bulma mantığı
        public async Task<Personel?> GetPersonelByIdAsync(int id)
        {
            return await _context.Personeller.FindAsync(id);
        }

        // Personel ekleme mantığı
        public async Task CreatePersonelAsync(Personel personel)
        {
            _context.Add(personel);
            await _context.SaveChangesAsync();
        }

        // Personel güncelleme mantığı
        public async Task UpdatePersonelAsync(Personel personel)
        {
            _context.Update(personel);
            await _context.SaveChangesAsync();
        }

        // Personel silme mantığı
        public async Task DeletePersonelAsync(int id)
        {
            var personel = await _context.Personeller.FindAsync(id);
            if (personel != null)
            {
                _context.Personeller.Remove(personel);
                // Bu metot, bir hata olursa (örn: ilişkili veri silinemezse)
                // hatayı fırlatacak ve Controller'daki try-catch bloğu bunu yakalayacak.
                await _context.SaveChangesAsync();
            }
        }

        // Personel varlık kontrolü mantığı
        public bool PersonelExists(int id)
        {
            return _context.Personeller.Any(e => e.Id == id);
        }
    }
}
