using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Test02.Models
{
    public enum TypZakaznika
    {
        [Display(Name = "Pojistník")]
        Pojistnik,
        [Display(Name = "Pojištěný")]
        Pojisteny
    }
    public enum TypPojisteni
    {
        [Display(Name = "Životní pojištění")]
        ZivostniPojisteni,
        [Display(Name = "Cestovní pojištění")]
        CestovniPojisteni,
        [Display(Name = "Pojištění vozidel")]
        PojisteniVozidla,
        [Display(Name = "Pojištění majetku")]
        PojisteniMajetku,
        [Display(Name = "Pojištění odpovědnosti")]
        PojisteniOdpovednosti
    }
    public enum UrovenPojisteni
    {
        [Display(Name = "Základní")]
        Zakladni,
        [Display(Name = "Rozšířená")]
        Rozsireni,
        [Display(Name = "Na míru")]
        NaMiru
    }

    [DisplayName("Pojištění")]
    public class Pojisteni
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Typ zákazníka")]
        [EnumDataType(typeof(TypZakaznika))]
        public TypZakaznika ZakaznikTyp { get; set; }
        [Required]
        [DisplayName("Typ pojištění")]
        [EnumDataType(typeof(TypPojisteni))]
        public TypPojisteni PojisteniTyp { get; set; }
        [Required]
        [DisplayName("Předmět pojištění")]
        public string PredmetPojisteni { get; set; } = string.Empty;
        [Required]
        [DisplayName("Úroveň pojištění")]
        [EnumDataType(typeof(UrovenPojisteni))]
        public UrovenPojisteni PojisteniUroven { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [DisplayName("Pojistná částka")]
        public decimal PojistnaCastka { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Platnost od")]
        public DateTime PlatnostOd { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Platnost do")]
        public DateTime? PlatnostDo { get; set; }

        public ICollection<PojistnaUdalost>? PojistneUdalosti { get; set; }

        [ForeignKey(nameof(Zakaznik))]
        public int? ZakaznikId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Zakaznik? Zakaznik { get; set; }
    }
}
