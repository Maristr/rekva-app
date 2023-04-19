using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test02.Models;
using Test02.Data;
using Microsoft.AspNetCore.Authorization;

namespace Test02.Controllers
{
    [Authorize]
    public class AdresaController : BaseController
    {

        public AdresaController(Test02Context context) : base(context)
        {
        }

        //// GET: Adresa
        //public async Task<IActionResult> Index()
        //{
        //    var userId = ZjistiUserId();
        //    var adresa = await _context.Adresa
        //        .Include(a => a.Zakaznik)
        //        .Where(u => u.Zakaznik.AspNetUserId == userId)
        //        .ToListAsync();
        //    return View(adresa);
        //}

        // GET: Adresa/5
        public async Task<IActionResult> Index(int? id)
        {
            var userId = ZjistiUserId();
            var adresa = await _context.Adresa
                .Include(a => a.Zakaznik)
                .Where(u => u.Zakaznik.AspNetUserId == userId)
                .Where(z => z.ZakaznikId == id)
                .ToListAsync();
            ViewBag.ZakaznikId = id;
            //var zakaznikCeleJmeno = _context.Zakaznik
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Jmeno + " " + z.Prijmeni)
            //    .FirstOrDefault();
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZeZakaznikId(id);
            return View(adresa);
        }

        // GET: Adresa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Adresa == null)
            {
                return NotFound();
            }

            var adresa = await _context.Adresa
                .Include(a => a.Zakaznik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adresa == null)
            {
                return NotFound();
            }

            return View(adresa);
        }

        // GET: Adresa/Create
        //public IActionResult Create()
        //{
        //    ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email");
        //    return View();
        //}

        // GET: Adresa/Create/5
        [Route("Adresa/Create/{ZakaznikId}")]
        public IActionResult Create(int zakaznikId)
        {
            ViewBag.ZakaznikId = zakaznikId;
            //var zakaznikCeleJmeno = _context.Zakaznik
            //    .Where(a => a.Id == zakaznikId)
            //    .Select(z => z.Jmeno + " " + z.Prijmeni)
            //    .FirstOrDefault();
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZeZakaznikId(zakaznikId);
            return View();
        }

        // POST: Adresa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Adresa/Create/{ZakaznikId}")]
        public async Task<IActionResult> Create([Bind("Id,ZakaznikId,Ulice,CisloPopisne,Obec,PSC,AdresaTyp,DorucovaciAdresa")] Adresa adresa, int zakaznikId)
        {
            if (ModelState.IsValid)
            {
                adresa.ZakaznikId = zakaznikId;
                _context.Add(adresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = zakaznikId});
            }
            ViewBag.ZakaznikId = zakaznikId;
            return View(adresa);
        }

        // GET: Adresa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //var zakaznikCeleJmeno = _context.Adresa
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni)
            //    .FirstOrDefault();
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZAdresaId(id);
            if (id == null || _context.Adresa == null)
            {
                return NotFound();
            }

            var adresa = await _context.Adresa.FindAsync(id);
            if (adresa == null)
            {
                return NotFound();
            }
            //ViewData["ZákaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", adresa.ZakaznikId);
            return View(adresa);
        }

        // POST: Adresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZakaznikId,Ulice,CisloPopisne,Obec,PSC,AdresaTyp,DorucovaciAdresa")] Adresa adresa)
        {
            if (id != adresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdresaExists(adresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = adresa.ZakaznikId });
            }
            //ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", adresa.ZakaznikId);
            return View(adresa);
        }

        // GET: Adresa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //var zakaznikCeleJmeno = _context.Adresa
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni)
            //    .FirstOrDefault();
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZAdresaId(id);
            if (id == null || _context.Adresa == null)
            {
                return NotFound();
            }

            var adresa = await _context.Adresa
                .Include(a => a.Zakaznik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adresa == null)
            {
                return NotFound();
            }

            return View(adresa);
        }

        // POST: Adresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Adresa == null)
            {
                return Problem("Entity set 'Test02Context.Adresa'  is null.");
            }
            var adresa = await _context.Adresa.FindAsync(id);
            if (adresa != null)
            {
                _context.Adresa.Remove(adresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = adresa.ZakaznikId });
        }

        private bool AdresaExists(int id)
        {
          return (_context.Adresa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
