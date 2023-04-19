using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Test02.Models
{
    public enum StavVyrizeni
    {
        [Display(Name = "Nová")]
        Nova,
        [Display(Name = "Čeká na data od zákazníka")]
        CekaNaZakaznika,
        [Display(Name = "V řešení")]
        VyrizujeSe,
        [Display(Name = "Uzavřena")]
        Uzavreno
    }
    [Display(Name = "Pojistná událost")]
    public class PojistnaUdalost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Datum Vzniku Události")]
        public DateTime VznikUdalosti { get; set; }
        [Required]
        [Display(Name = "Popis události")]
        public string PopisUdalosti { get; set; } = string.Empty;
        [Required]
        [DisplayName("Stav vyřízení")]
        [EnumDataType(typeof(StavVyrizeni))]
        public StavVyrizeni VyrizeniStav { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Datum změny stavu")]
        private DateTime? DatumZmenyStavu { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name ="Vyplacená částka")]
        private decimal? VyplacenaCastka { get; set; }

        [Display(Name = "Zákazník")]
        [ForeignKey(nameof(Zakaznik))]
        public int? ZakaznikId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Zakaznik? Zakaznik { get; set; }

        [Required]
        [Display(Name = "Pojištění (ke kterému se pojistní událost vztahuje)")]
        [ForeignKey(nameof(Pojisteni))]
        public int? PojisteniId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Pojisteni? Pojisteni { get; set; }
    }
}
