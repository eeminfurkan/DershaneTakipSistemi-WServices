using System.ComponentModel.DataAnnotations; // Display attribute'u için

namespace DershaneTakipSistemi.Models
{
    public class DashboardViewModel
    {
        [Display(Name = "Toplam Öğrenci")]
        public int ToplamOgrenciSayisi { get; set; }

        [Display(Name = "Aktif Öğrenci")]
        public int AktifOgrenciSayisi { get; set; }

        [Display(Name = "Toplam Ödeme Tutarı")]
        [DataType(DataType.Currency)] // Para birimi formatı için
        public decimal ToplamOdemeTutari { get; set; }

        [Display(Name = "Bugünkü Ödeme Sayısı")]
        public int BugunkuOdemeSayisi { get; set; }

        // İleride eklenebilecekler:
        // public int BuAykiOgrenciSayisi { get; set; }
        // public decimal BuAykiOdemeTutari { get; set; }
        // public List<Ogrenci> SonKayitlar { get; set; } // Son kayıtları listelemek için
    }
}