using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kafic;
using Kafic.Models;
using Kafic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DjecjiVrticProjekt.Controllers
{
    public class StavkaRacunaController : Controller
    {
        private readonly KaficContext _context;

        public StavkaRacunaController(KaficContext context)
        {
            _context = context;
        }

        // GET: Dijete
        public async Task<IActionResult> Index()
        {
            var kaficContext = _context.StavkaRacuna.Include(d => d.Racun);
            return View(await kaficContext.ToListAsync());
        }

        public IActionResult MasterDetail(int page = 1, int sort = 1, bool ascending = true)
        {
            var stavke = _context.StavkaRacuna
                .Include(z => z.Racun)
         
                .AsNoTracking()
                .Select(m => new StavkaRacunaViewModel
                {
                    IdRacun = m.IdRacun,
                    Pice = m.Pice.Naziv,
                    Kolicina = m.Kolicina,
                    JedCijena = m.JedCijena,
                    Iznos = m.Iznos
                })
                .ToList();
            var racun = _context.Racun
                .AsNoTracking()
                .Select(m => new RacunViewModel
                {
                    IdRacun = m.IdRacun,
                    Korisnik = m.Korisnik.KorisnickoIme,
                    Datum = m.Datum,
                    UkupanIznos = m.UkupanIznos
                });
            var childrenCount = stavke.Count();
            var parentsCount = 1;
            //int pagesize = appData.PageSize;
            


            var model = new StavkeRacunaViewModel
            {
                StavkeRacuna = stavke
               
            };



            return View(model);
        }

        public PartialViewResult Row(int idR, int idP)
        {
            var stavka = _context.StavkaRacuna
                             .Where(m => m.IdRacun == idR && m.IdPice == idP)
                              .Select(m => new StavkaRacunaViewModel
                              {
                                  IdRacun = m.IdRacun,
                                  Pice = m.Pice.Naziv,
                                  Kolicina = m.Kolicina,
                                  JedCijena = m.JedCijena,
                                  Iznos = m.Iznos
                              })
                             .SingleOrDefault();
            if (stavka != null)
            {
                return PartialView(stavka);
            }
            else
            {
                //vratiti prazan sadržaj?
                return PartialView("ErrorMessageRow", $"Neispravan id djeteta: {idR}, {idP}");
            }
        }

        public IActionResult EditPartial(int idR, int idP)
        {
            ViewData["IdRacun"] = new SelectList(_context.Racun, "IdRacun");
            var stavka = _context.StavkaRacuna
                             .AsNoTracking()
                             .Where(m => m.IdRacun == idR && m.IdPice == idP)
                             .SingleOrDefault();
            if (stavka != null)
            {

                return PartialView(stavka);
            }
            else
            {
                return NotFound($"Neispravan ključ stavke: {idR}, {idP}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPartial(StavkaRacuna stavka)
        {

            if (stavka == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = _context.StavkaRacuna.Any(m => m.IdRacun == stavka.IdRacun && m.IdPice == stavka.IdPice);
            if (!checkId)
            {
                return NotFound($"Neispravan id stavke: {stavka.IdRacun}, {stavka.IdPice}");
            }
            ViewData["IdRacun"] = new SelectList(_context.Racun, "IdRacun");
            if (ModelState.IsValid)
            {
                try
                {
                    TempData[Constants.Message] = $"Stavka uspjesno azurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    _context.Update(stavka);
                    _context.SaveChanges();

                    return StatusCode(302, Url.Action(nameof(Row), new { idR = stavka.IdRacun, idP = stavka.IdPice }));

                }
                catch (Exception exc)
                {

                    return PartialView(stavka);
                }
            }
            else
            {
                return PartialView(stavka);
            }
        }


        public IActionResult EditPartialRacun(int id)
        {
           
            var racun = _context.Racun
                             .AsNoTracking()
                             .Where(m => m.IdRacun == id)
                             .SingleOrDefault();
            if (racun != null)
            {

                return PartialView(racun);
            }
            else
            {
                return NotFound($"Neispravan id racuna: {id}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPartialRoditelj(Racun racun)
        {

            if (racun == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = _context.Racun.Any(m => m.IdRacun == racun.IdRacun);
            if (!checkId)
            {
                return NotFound($"Neispravan id racuna: {racun.IdRacun}");
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racun);
                    _context.SaveChanges();

                    return StatusCode(302, Url.Action(nameof(Row), new { id = racun.IdRacun }));

                }
                catch (Exception exc)
                {

                    return PartialView(racun);
                }
            }
            else
            {
                return PartialView(racun);
            }
        }


        // GET: Dijete/Details/5
        public async Task<IActionResult> Details(int? idR, int? idP)
        {
            if (idR == null || idP == null)
            {
                return NotFound();
            }

            var stavka = await _context.StavkaRacuna
                .Include(d => d.Racun)
                .FirstOrDefaultAsync(m => m.IdRacun == idR && m.IdPice == idP);
            if (stavka == null)
            {
                return NotFound();
            }

            return View(stavka);
        }

        private void PrepareDropDownList()
        {
            var pica = _context.Pice
                .Where(d => d.Naziv != "...")
                .OrderBy(d => d.Naziv)
                .Select(d => new { d.IdPice, d.Naziv })
                .ToList();
            var gd2 = _context.Pice
                .Where(d => d.Naziv == "...")
                .Select(d => new { d.IdPice, d.Naziv })
                .FirstOrDefault();

            if (gd2 != null)
            {
                pica.Insert(0, gd2);
            }
            ViewBag.Korisnici = new SelectList(pica, nameof(Pice.IdPice), nameof(Pice.Naziv));


        }

        public IActionResult Create()
        {
            ViewData["IdRacun"] = new SelectList(_context.Racun, "IdRacun");
            PrepareDropDownList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRacun,IdPice,Kolicina,JedCijena,Iznos")] StavkaRacuna stavka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stavka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRacun"] = new SelectList(_context.Racun, "IdRacun", stavka.IdRacun.ToString());
            PrepareDropDownList();
            return View(stavka);
        }


        public async Task<IActionResult> Edit(int? idR, int? idP)
        {
            if (idR == null || idP == null)
            {
                return NotFound();
            }

            var stavka = await _context.StavkaRacuna.FindAsync(idR, idP);
            if (stavka == null)
            {
                return NotFound();
            }
            ViewData["IdRoditelj"] = new SelectList(_context.Racun, "IdRacun", stavka.IdRacun.ToString());
            return View(stavka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idR, int idP, [Bind("IdRacun,IdPice,Kolicina,JedCijena,Iznos")] StavkaRacuna stavka)
        {
            if (idR != stavka.IdRacun || idP != stavka.IdPice)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stavka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StavkaExists(stavka.IdRacun, stavka.IdPice))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRoditelj"] = new SelectList(_context.Racun, "IdRacun", stavka.IdRacun.ToString());
            return View(stavka);
        }

        // GET: Dijete/Delete/5
        public async Task<IActionResult> Delete(int? idR, int? idP)
        {
            if (idR == null || idP == null)
            {
                return NotFound();
            }

            var stavka = await _context.StavkaRacuna
                .Include(d => d.Racun)
                .FirstOrDefaultAsync(m => m.IdRacun == idR && m.IdPice == idP);
            if (stavka == null)
            {
                return NotFound();
            }

            return View(stavka);
        }

        // POST: Dijete/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idR, int idP)
        {
            var stavka = await _context.StavkaRacuna.FindAsync(idR, idP);
            _context.StavkaRacuna.Remove(stavka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StavkaExists(int idR, int idP)
        {
            return _context.StavkaRacuna.Any(e => e.IdRacun == idR && e.IdPice == idP);
        }
    }
}
