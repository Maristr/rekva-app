using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Test02.Models
{
    public enum TypAdresy
    {
        [Display(Name = "Trvalé")]
        Trvale,
        [Display(Name = "Přechodné")]
        Prechodne
    }
    [Display(Name = "Adresa")]
    public class Adresa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Ulice")]
        public string Ulice { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Číslo popisné")]
        public int CisloPopisne { get; set; }
        [Required]
        [Display(Name = "Obec")]
        public string Obec { get; set; } = string.Empty;
        [Required]
        [Display(Name = "PSČ")]
        public int PSC { get; set; }
        [Required]
        [DisplayName("Bydliště")]
        [EnumDataType(typeof(TypAdresy))]
        public TypAdresy AdresaTyp { get; set; }
        [DisplayName("Korespondenční adresa")]
        public bool DorucovaciAdresa { get; set; }

        [ForeignKey(nameof(Zakaznik))]
        public int? ZakaznikId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Zakaznik? Zakaznik { get; set; }

        public override string ToString()
        {
            return Ulice + " " + CisloPopisne + ", " + PSC + " " + Obec;
        }
    }
}
