using Kafic.Extensions;
using Kafic.Extensions.Selectors;
using Kafic.Models;
using Kafic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kafic.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly KaficContext ctx;
        private readonly AppSettings appSettings;
        public KorisnikController(KaficContext ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionsSnapshot.Value;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(korisnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Korisnik {korisnik.KorisnickoIme} uspjesno dodan.";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(korisnik);
                }
            }
            else
            {
                return View(korisnik);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdKorisnik, int page = 1, int sort = 1, bool ascending = true)
        {
            var korisnik = ctx.Korisnik.Find(IdKorisnik);
            if (korisnik == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = korisnik.KorisnickoIme;
                    ctx.Remove(korisnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Korisnik {naziv} uspjesno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Greška prilikom brisanja." + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = false;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
        }
        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var korisnik = ctx.Korisnik.AsNoTracking()
                .Where(d => d.IdKorisnik == id)
                .FirstOrDefault();
            if (korisnik == null)
            {
                return NotFound($"Nema navedenog {id} korisnika");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(korisnik);
            }

        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Korisnik korisnik = await ctx.Korisnik.FindAsync(id);
                if (korisnik == null)
                {
                    return NotFound($"Nema navedenog {id} korisnika");
                }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<Korisnik>(korisnik, "", d => d.IdKorisnik, 
                    d => d.Ime, d => d.Prezime, d => d.Spol, d => d.Oib, d => d.DatumRodenja,
                    d => d.Adresa, d => d.PostanskiBroj, d => d.Mjesto, d => d.Drzava,
                    d => d.KorisnickoIme, d => d.Lozinka);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Korisnik {korisnik.KorisnickoIme} uspješno ažuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(korisnik);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o korisniku nije moguće povezati s forme.");
                    return View(korisnik);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await ctx.Korisnik
                .FirstOrDefaultAsync(m => m.IdKorisnik == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = ctx.Korisnik.AsNoTracking();

            int count = query.Count();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pageSize,
                TotalItems = count
            };

            if (page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var korisnici = query
                .Select(d => new KorisnikViewModel
                {
                    IdKorisnik = d.IdKorisnik,
                    Ime = d.Ime,
                    Prezime = d.Prezime,
                    Spol = d.Spol,
                    Oib = d.Oib,
                    DatumRodenja = d.DatumRodenja,
                    Adresa = d.Adresa,
                    PostanskiBroj = d.PostanskiBroj,
                    Mjesto = d.Mjesto,
                    Drzava = d.Drzava,
                    KorisnickoIme = d.KorisnickoIme,
                    Lozinka = d.Lozinka
                })
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();

            var model = new KorisniciViewModel
            {
                Korisnici = korisnici,
                PagingInfo = pagingInfo
            };


            return View(model);
        }
    }
}
