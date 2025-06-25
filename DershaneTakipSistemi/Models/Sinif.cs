using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DershaneTakipSistemi.Models
{
    public class Sinif
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sınıf adı zorunludur.")]
        [Display(Name = "Sınıf Adı / Kodu")]
        [StringLength(100)]
        public string Ad { get; set; } = string.Empty;

        // Açıklama ve Kapasite property'leri buradan kaldırıldı.

        [Display(Name = "Sınıf Öğretmeni")]
        public int? OgretmenPersonelId { get; set; }

        [Display(Name = "Sınıf Öğretmeni")]
        [ForeignKey("OgretmenPersonelId")]
        public virtual Personel? SorumluOgretmen { get; set; }

        public virtual ICollection<Ogrenci>? Ogrenciler { get; set; }
    }
}