using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test02.Data;
using Test02.Models;

namespace Test02.Controllers
{
    [Authorize]
    public class PojistnaUdalostController : BaseController
    {

        public PojistnaUdalostController(Test02Context context) : base(context)
        {
        }

        //// GET: PojistnaUdalost
        //public async Task<IActionResult> Index()
        //{
        //    var Id = ZjistiUserId();
        //    return View(await _context.PojistnaUdalost
        //        .Include(a => a.Pojisteni)
        //        .Include(a => a.Zakaznik)
        //        .Where(u => u.Zakaznik.AspNetUserId == Id)
        //        .ToListAsync());
        //}

        // GET: PojistnaUdalost
        public async Task<IActionResult> Index(int? id)
        {
            var userId = ZjistiUserId();
            var pojistnaUdalost = await _context.PojistnaUdalost
                .Include(a => a.Zakaznik)
                .Where(u => u.Zakaznik.AspNetUserId == userId)
                .Where(u => u.PojisteniId == id)
                .ToListAsync();
            ViewBag.PojisteniId = id;
            //var zakaznikCeleJmeno = _context.Zakaznik
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Jmeno + " " + z.Prijmeni)
            //    .FirstOrDefault();
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZPojisteniId(id);
            //var pojisteniCelyNazev = _context.Pojisteni
            //    .Include(a => a.Zakaznik)
            //    .Where(u => u.ZakaznikId == id)
            //    .Select(z => z.PojisteniTyp + " - " + z.PredmetPojisteni)
            //    .FirstOrDefault();
            ViewBag.PojisteniCelyNazev = ZjistiNazevPojisteniZPojisteniId(id);
            return View(pojistnaUdalost);
        }

        // GET: PojistnaUdalost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PojistnaUdalost == null)
            {
                return NotFound();
            }

            var pojistnaUdalost = await _context.PojistnaUdalost
                .Include(p => p.Pojisteni)
                .Include(p => p.Zakaznik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pojistnaUdalost == null)
            {
                return NotFound();
            }

            return View(pojistnaUdalost);
        }

        //// GET: PojistnaUdalost/Create
        //public IActionResult Create()
        //{
        //    ViewData["PojisteniId"] = new SelectList(_context.Pojisteni, "Id", "PredmetPojisteni");
        //    ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email");
        //    return View();
        //}

        // GET: PojistnaUdalost/Create/5
        [Route("PojistnaUdalost/Create/{PojisteniId}")]
        public IActionResult Create(int pojisteniId)
        {
            var zakaznikId = _context.Pojisteni
                .Where(a => a.Id == pojisteniId)
                .Select(z => z.ZakaznikId)
                .FirstOrDefault();
            ViewBag.ZakaznikId = zakaznikId;
            ViewBag.PojisteniId = pojisteniId;
            var pojistnaUdalost = new PojistnaUdalost() { 
                ZakaznikId = zakaznikId, 
                PojisteniId = pojisteniId,
                VznikUdalosti = DateTime.Today
            };
            //var pojistnaUdalost = _context.PojistnaUdalost
            //    .Include(p => p.Pojisteni)
            //    .Include(p => p.Zakaznik)
            //    .Where(a => a.Id == pojisteniId)
            //    .Select(pu => pu.PopisUdalosti);
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZPojisteniId(pojisteniId);
            ViewBag.PojisteniCelyNazev = ZjistiNazevPojisteniZPojisteniId(pojisteniId);
            return View(pojistnaUdalost);
        }

        // POST: PojistnaUdalost/Create/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("PojistnaUdalost/Create/{PojisteniId}")]
        public async Task<IActionResult> Create([Bind("Id,ZakaznikId,PojisteniId,VznikUdalosti,PopisUdalosti,VyrizeniStav")] PojistnaUdalost pojistnaUdalost, int pojisteniId)
        {
            if (ModelState.IsValid)
            {
                pojistnaUdalost.PojisteniId = pojisteniId;
                _context.Add(pojistnaUdalost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = pojisteniId });
            }
            //ViewData["PojisteniId"] = new SelectList(_context.Pojisteni, "Id", "PredmetPojisteni", pojistnaUdalost.PojisteniId);
            //ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", pojistnaUdalost.ZakaznikId);
            ViewBag.PojisteniId = pojisteniId;
            ViewBag.ZakaznikId = pojistnaUdalost.ZakaznikId;
            return View(pojistnaUdalost);
        }

        // GET: PojistnaUdalost/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PojistnaUdalost == null)
            {
                return NotFound();
            }

            ViewBag.ZakaznikId = ZjistiZakaznikIdZPojistnaUdalostId(id);
            ViewBag.PojisteniId = ZjistiPojisteniIdZPojistnaUdalostId(id);
            var pojistnaUdalost = await _context.PojistnaUdalost.FindAsync(id);
            if (pojistnaUdalost == null)
            {
                return NotFound();
            }
            //ViewData["PojisteniId"] = new SelectList(_context.Pojisteni, "Id", "PredmetPojisteni", pojistnaUdalost.PojisteniId);
            //ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", pojistnaUdalost.ZakaznikId);
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZPojistnaUdalostId(id);
            ViewBag.PojisteniCelyNazev = ZjistiNazevPojisteniZPojisteniId(ZjistiPojisteniIdZPojistnaUdalostId(id));
            return View(pojistnaUdalost);
        }

        // POST: PojistnaUdalost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZakaznikId,PojisteniId,VznikUdalosti,PopisUdalosti,VyrizeniStav")] PojistnaUdalost pojistnaUdalost)
        {
            if (id != pojistnaUdalost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pojistnaUdalost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PojistnaUdalostExists(pojistnaUdalost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = pojistnaUdalost.PojisteniId});
            }
            //ViewData["PojisteniId"] = new SelectList(_context.Pojisteni, "Id", "PredmetPojisteni", pojistnaUdalost.PojisteniId);
            //ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", pojistnaUdalost.ZakaznikId);
            return View(pojistnaUdalost);
        }

        // GET: PojistnaUdalost/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZPojistnaUdalostId(id);
            ViewBag.PojisteniCelyNazev = ZjistiNazevPojisteniZPojisteniId(ZjistiPojisteniIdZPojistnaUdalostId(id));
            if (id == null || _context.PojistnaUdalost == null)
            {
                return NotFound();
            }

            var pojistnaUdalost = await _context.PojistnaUdalost
                .Include(p => p.Pojisteni)
                .Include(p => p.Zakaznik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pojistnaUdalost == null)
            {
                return NotFound();
            }

            return View(pojistnaUdalost);
        }

        // POST: PojistnaUdalost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PojistnaUdalost == null)
            {
                return Problem("Entity set 'Test02Context.PojistnaUdalost'  is null.");
            }
            var pojistnaUdalost = await _context.PojistnaUdalost.FindAsync(id);
            if (pojistnaUdalost != null)
            {
                _context.PojistnaUdalost.Remove(pojistnaUdalost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = pojistnaUdalost.PojisteniId });
        }

        private bool PojistnaUdalostExists(int id)
        {
          return (_context.PojistnaUdalost?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
