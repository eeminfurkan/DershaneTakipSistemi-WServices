// Services/KasaHareketiService.cs

using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DershaneTakipSistemi.Services
{
    // Bu bizim yeni "Kasa İşleri Yardımcısı" sınıfımız.
    public class KasaHareketiService
    {
        private readonly ApplicationDbContext _context;

        public KasaHareketiService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index sayfasındaki tüm filtreleme ve hesaplama mantığı burada.
        public async Task<List<KasaHareketi>> GetKasaHareketleriAsync(DateTime? baslangicTarihi, DateTime? bitisTarihi, Kategori? kategori)
        {
            var sorgu = _context.KasaHareketleri
                .Include(k => k.Ogrenci)
                .Include(k => k.Personel)
                .AsQueryable();

            if (baslangicTarihi.HasValue)
            {
                sorgu = sorgu.Where(k => k.Tarih >= baslangicTarihi.Value);
            }
            if (bitisTarihi.HasValue)
            {
                // Bitiş tarihini gün sonuna ayarlayarak o günün tamamını dahil ediyoruz.
                sorgu = sorgu.Where(k => k.Tarih < bitisTarihi.Value.AddDays(1));
            }
            if (kategori.HasValue)
            {
                sorgu = sorgu.Where(k => k.Kategori == kategori.Value);
            }

            return await sorgu.OrderByDescending(k => k.Tarih).ToListAsync();
        }

        // Tek bir kasa hareketini getirme.
        public async Task<KasaHareketi?> GetKasaHareketiByIdAsync(int id)
        {
            return await _context.KasaHareketleri
                .Include(k => k.Ogrenci)
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // Yeni kasa hareketi ekleme.
        public async Task CreateKasaHareketiAsync(KasaHareketi kasaHareketi)
        {
            _context.Add(kasaHareketi);
            await _context.SaveChangesAsync();
        }

        // Kasa hareketi güncelleme.
        public async Task UpdateKasaHareketiAsync(KasaHareketi kasaHareketi)
        {
            _context.Update(kasaHareketi);
            await _context.SaveChangesAsync();
        }

        // Kasa hareketi silme.
        public async Task DeleteKasaHareketiAsync(int id)
        {
            var kasaHareketi = await _context.KasaHareketleri.FindAsync(id);
            if (kasaHareketi != null)
            {
                _context.KasaHareketleri.Remove(kasaHareketi);
                await _context.SaveChangesAsync();
            }
        }

        // Varlık kontrolü
        public bool KasaHareketiExists(int id)
        {
            return _context.KasaHareketleri.Any(e => e.Id == id);
        }

        // --- Dropdown Listeleri İçin Yardımcı Metotlar ---

        public SelectList GetOgrenciSelectList(object? seciliOgrenci = null)
        {
            return new SelectList(_context.Ogrenciler.OrderBy(p => p.Ad), "Id", "AdSoyad", seciliOgrenci);
        }

        public SelectList GetPersonelSelectList(object? seciliPersonel = null)
        {
            return new SelectList(_context.Personeller.OrderBy(p => p.Ad), "Id", "AdSoyad", seciliPersonel);
        }
    }
}
