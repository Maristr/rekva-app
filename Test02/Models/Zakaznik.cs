using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Test02.Models
{
    [Display(Name = "Zákazník")]
    public class Zakaznik
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Jméno")]
        public string Jmeno { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum narození")]
        public DateTime Datum { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefon")]
        public int Telefon { get; set; }

        // This property is the foreign key that references the corresponding AspNetUser
        [ForeignKey(nameof(AspNetUser))]
        public string? AspNetUserId { get; set; }
        // This property defines the relationship between the Zakaznik and AspNetUsers tables
        public IdentityUser? AspNetUser { get; set; }

        public ICollection<Adresa>? Adresy { get; set; }
        public ICollection<Pojisteni>? Pojisteni { get; set; }
        public ICollection<PojistnaUdalost>? PojistneUdalosti { get; set; }
    }
}
