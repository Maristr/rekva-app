using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.InteropServices;
using Test02.Data;
using Test02.Models;

namespace Test02.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly Test02Context _context;

        private string _id = null;

        public BaseController(Test02Context context)
        {
            _context = context;
        }

        public string ZjistiUserId()
        {
            if (string.IsNullOrEmpty(_id))
            {
                var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                return userId;
            }
            return _id;
        }

        public string ZjistiJmenoZakaznikaZeZakaznikId(int? id)
        {
            var zakaznikJmeno = _context.Zakaznik
                .Where(a => a.Id == id)
                .Select(z => z.Jmeno + " " + z.Prijmeni)
                .FirstOrDefault();
            return zakaznikJmeno;
        }

        public string ZjistiJmenoZakaznikaZPojisteniId(int? id)
        {
            var zakaznikJmeno = _context.Pojisteni
                .Where(a => a.Id == id)
                .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni)
                .FirstOrDefault();
            return zakaznikJmeno;
        }

        public string ZjistiJmenoZakaznikaZPojistnaUdalostId(int? id)
        {
            var zakaznikJmeno = _context.PojistnaUdalost
                .Where(a => a.Id == id)
                .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni)
                .FirstOrDefault();
            return zakaznikJmeno;
        }

        public string ZjistiJmenoZakaznikaZAdresaId(int? id)
        {
            var zakaznikJmeno = _context.Adresa
                .Where(a => a.Id == id)
                .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni)
                .FirstOrDefault();
            return zakaznikJmeno;
        }

        public string ZjistiNazevPojisteniZPojisteniId(int? id)
        {
            var pojisteniTyp = _context.Pojisteni
                .Where(u => u.Id == id)
                .Select(z => z.PojisteniTyp)
                .FirstOrDefault();

            var pojisteniTypAtribut = typeof(TypPojisteni)
                .GetMember(pojisteniTyp.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>()
                ?.Name;

            var pojisteniPredmet = _context.Pojisteni
                .Where(u => u.Id == id)
                .Select(z => z.PredmetPojisteni)
                .FirstOrDefault();

            return pojisteniTypAtribut + " / " + pojisteniPredmet;
        }

        public int ZjistiZakaznikIdZPojistnaUdalostId(int? id)
        {
            var zakaznikId = _context.PojistnaUdalost
                .Where(a => a.Id == id)
                .Select(z => z.ZakaznikId)
                .FirstOrDefault();
            return (int)zakaznikId;
        }

        public int ZjistiPojisteniIdZPojistnaUdalostId(int? id)
        {
            var pojisteniId = _context.PojistnaUdalost
                .Where(a => a.Id == id)
                .Select(z => z.PojisteniId)
                .FirstOrDefault();
            return (int)pojisteniId;
        }
    }
}
