using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DershaneTakipSistemi.Models
{
    public class Ogrenci
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

        [Display(Name = "Cep Telefonu")]
        [StringLength(15)]
        public string? CepTelefonu { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Kayıt Tarihi zorunludur.")]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Sınıfı")]
        public int? SinifId { get; set; }

        [ForeignKey("SinifId")]
        [Display(Name = "Sınıfı")]
        public virtual Sinif? Sinifi { get; set; }

        // public virtual ICollection<Odeme>? Odemeler { get; set; } // <-- BU SATIR SİLİNDİ

        [NotMapped]
        [Display(Name = "Adı Soyadı")]
        public string AdSoyad => $"{Ad} {Soyad}";
    }
}