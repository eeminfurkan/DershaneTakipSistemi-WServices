using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DershaneTakipSistemi.Models
{
    public class KasaHareketi
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [Display(Name = "İşlem Açıklaması")]
        [StringLength(200)]
        public string Aciklama { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar pozitif bir değer olmalıdır.")]
        public decimal Tutar { get; set; }

        [Required]
        [Display(Name = "Hareket Yönü")]
        public HareketYonu HareketYonu { get; set; }

        [Required]
        [Display(Name = "Kategori")]
        public Kategori Kategori { get; set; }

        [Display(Name = "Ödeme Yöntemi")]
        public OdemeYontemi? OdemeYontemi { get; set; } // Nullable, her işlemde olmayabilir

        // İlişkili Alanlar
        [Display(Name = "İlgili Öğrenci")]
        public int? OgrenciId { get; set; } // Nullable
        [ForeignKey("OgrenciId")]
        public virtual Ogrenci? Ogrenci { get; set; }

        [Display(Name = "İlgili Personel")]
        public int? PersonelId { get; set; } // Nullable
        [ForeignKey("PersonelId")]
        public virtual Personel? Personel { get; set; }
    }
}