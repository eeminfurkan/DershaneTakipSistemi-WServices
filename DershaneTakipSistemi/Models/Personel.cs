using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DershaneTakipSistemi.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Adı")]
        [StringLength(50)]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyadı")]
        [StringLength(50)]
        public string Soyad { get; set; } = string.Empty;

        [Display(Name = "Görevi")]
        [StringLength(100)]
        public string? Gorevi { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        public virtual ICollection<Sinif>? SorumluOlduguSiniflar { get; set; }

        [NotMapped]
        [Display(Name = "Adı Soyadı")]
        public string AdSoyad => $"{Ad} {Soyad}";
    }
}