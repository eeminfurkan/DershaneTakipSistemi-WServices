using System.ComponentModel.DataAnnotations;

namespace DershaneTakipSistemi.Models
{
    public class DashboardViewModel
    {
        [Display(Name = "Toplam Öğrenci")]
        public int ToplamOgrenciSayisi { get; set; }

        [Display(Name = "Aktif Öğrenci")]
        public int AktifOgrenciSayisi { get; set; }

        [Display(Name = "Toplam Ödeme Tutarı")]
        [DataType(DataType.Currency)] 
        public decimal ToplamOdemeTutari { get; set; }

        [Display(Name = "Bugünkü Ödeme Sayısı")]
        public int BugunkuOdemeSayisi { get; set; }

    }
}