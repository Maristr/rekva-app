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
    public class PojisteniController : BaseController
    {

        public PojisteniController(Test02Context context) : base(context)
        {
        }

        //// GET: Pojisteni
        //public async Task<IActionResult> Index()
        //{
        //    //var test02Context = _context.Pojisteni.Include(p => p.Zakaznik);
        //    //return View(await test02Context.ToListAsync());
        //    var Id = ZjistiUserId();
        //    return View(await _context.Pojisteni
        //        .Include(a => a.Zakaznik)
        //        .Where(u => u.Zakaznik.AspNetUserId == Id)
        //        .ToListAsync());
        //}

        // GET: Pojisteni/5
        public async Task<IActionResult> Index(int? id)
        {
            var userId = ZjistiUserId();
            var pojisteni = await _context.Pojisteni
                .Include(a => a.Zakaznik)
                .Where(u => u.Zakaznik.AspNetUserId == userId)
                .Where(u => u.ZakaznikId == id)
                .ToListAsync();
            ViewBag.ZakaznikId = id;
            //var zakaznikCeleJmeno = _context.Zakaznik
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Jmeno + " " + z.Prijmeni)
            //    .FirstOrDefault();
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZeZakaznikId(id);
            return View(pojisteni);
        }

        // GET: Pojisteni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pojisteni == null)
            {
                return NotFound();
            }

            var pojisteni = await _context.Pojisteni
                .Include(p => p.Zakaznik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pojisteni == null)
            {
                return NotFound();
            }

            return View(pojisteni);
        }

        //// GET: Pojisteni/Create
        //public IActionResult Create()
        //{
        //    ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email");
        //    return View();
        //}

        // GET: Pojisteni/Create/5
        [Route("Pojisteni/Create/{ZakaznikId}")]
        public IActionResult Create(int zakaznikId)
        {
            ViewBag.ZakaznikId = zakaznikId;
            //var zakaznikCeleJmeno = _context.Zakaznik
            //    .Where(a => a.Id == zakaznikId)
            //    .Select(z => z.Jmeno + " " + z.Prijmeni);
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZeZakaznikId(zakaznikId);
            return View();
        }

        // POST: Pojisteni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Pojisteni/Create/{ZakaznikId}")]
        public async Task<IActionResult> Create([Bind("Id,ZakaznikId,PojisteniTyp,PredmetPojisteni,PojisteniUroven,PojistnaCastka,PlatnostOd,PlatnostDo")] Pojisteni pojisteni, int zakaznikId)
        {
            if (ModelState.IsValid)
            {
                pojisteni.ZakaznikId = zakaznikId;
                _context.Add(pojisteni);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = zakaznikId });
            }
            ViewBag.ZakaznikId = zakaznikId;
            return View(pojisteni);
        }

        // GET: Pojisteni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //var zakaznikCeleJmeno = _context.Pojisteni
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni);
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZPojisteniId(id);
            if (id == null || _context.Pojisteni == null)
            {
                return NotFound();
            }

            var pojisteni = await _context.Pojisteni.FindAsync(id);
            if (pojisteni == null)
            {
                return NotFound();
            }
            //ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", pojisteni.ZakaznikId);
            return View(pojisteni);
        }

        // POST: Pojisteni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZakaznikId,PojisteniTyp,PredmetPojisteni,PojisteniUroven,PojistnaCastka,PlatnostOd,PlatnostDo")] Pojisteni pojisteni)
        {
            if (id != pojisteni.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pojisteni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PojisteniExists(pojisteni.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = pojisteni.ZakaznikId });
            }
            //ViewData["ZakaznikId"] = new SelectList(_context.Zakaznik, "Id", "Email", pojisteni.ZakaznikId);
            return View(pojisteni);
        }

        // GET: Pojisteni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //var zakaznikCeleJmeno = _context.Pojisteni
            //    .Where(a => a.Id == id)
            //    .Select(z => z.Zakaznik.Jmeno + " " + z.Zakaznik.Prijmeni);
            ViewBag.ZakaznikCeleJmeno = ZjistiJmenoZakaznikaZPojisteniId(id);
            if (id == null || _context.Pojisteni == null)
            {
                return NotFound();
            }

            var pojisteni = await _context.Pojisteni
                .Include(p => p.Zakaznik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pojisteni == null)
            {
                return NotFound();
            }

            return View(pojisteni);
        }

        // POST: Pojisteni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pojisteni == null)
            {
                return Problem("Entity set 'Test02Context.Pojisteni'  is null.");
            }
            var pojisteni = await _context.Pojisteni.FindAsync(id);
            if (pojisteni != null)
            {
                _context.Pojisteni.Remove(pojisteni);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = pojisteni.ZakaznikId });
        }

        private bool PojisteniExists(int id)
        {
          return (_context.Pojisteni?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
