// Services/SinifService.cs

using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Services
{
    // Bu bizim yeni "Sınıf İşleri Yardımcısı" sınıfımız.
    public class SinifService
    {
        private readonly ApplicationDbContext _context;

        public SinifService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Sınıfları sorumlu öğretmenleri ve öğrenci sayılarıyla birlikte getiren metot.
        public async Task<List<Sinif>> GetSiniflarAsync()
        {
            return await _context.Siniflar
                .Include(s => s.SorumluOgretmen) // Sorumlu Öğretmen bilgisini de çek
                .Include(s => s.Ogrenciler)       // Sınıf mevcudunu hesaplamak için öğrenci listesini çek
                .OrderBy(s => s.Ad)
                .ToListAsync();
        }

        // ID ile tek bir sınıf getiren metot.
        public async Task<Sinif?> GetSinifByIdAsync(int id)
        {
            return await _context.Siniflar.FindAsync(id);
        }

        // Yeni sınıf ekleme mantığı
        public async Task CreateSinifAsync(Sinif sinif)
        {
            _context.Add(sinif);
            await _context.SaveChangesAsync();
        }

        // Sınıf güncelleme mantığı
        public async Task UpdateSinifAsync(Sinif sinif)
        {
            _context.Update(sinif);
            await _context.SaveChangesAsync();
        }

        // Sınıf silme mantığı
        public async Task DeleteSinifAsync(int id)
        {
            var sinif = await _context.Siniflar.FindAsync(id);
            if (sinif != null)
            {
                _context.Siniflar.Remove(sinif);
                // Eğer sınıfta öğrenci varsa, bu satır DbUpdateException hatası fırlatır.
                // Bu hata Controller'da yakalanacak.
                await _context.SaveChangesAsync();
            }
        }

        // Sınıf varlık kontrolü
        public bool SinifExists(int id)
        {
            return _context.Siniflar.Any(e => e.Id == id);
        }

        // Öğretmen olarak atanabilecek personelleri listeleyen metot.
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
