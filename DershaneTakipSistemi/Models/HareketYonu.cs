using System.ComponentModel.DataAnnotations;

namespace DershaneTakipSistemi.Models
{
    public enum HareketYonu
    {
        [Display(Name = "Gelir / Giriş")]
        Giris = 1,

        [Display(Name = "Gider / Çıkış")]
        Cikis = 2
    }
}