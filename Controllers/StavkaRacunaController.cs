using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Kafic.Extensions;
using Kafic.Extensions.Selectors;
using Kafic.Models;
using Kafic.ViewModels;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kafic.Controllers
{
    /// <summary>
    /// Kontroler za Kontakt.
    /// </summary>
    public class StavkaRacunaController : Controller
    {
        private readonly KaficContext ctx;

        private readonly AppSettings appSettings;
        public StavkaRacunaController(KaficContext ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        private async Task PrepareDropDownLists()
        {
            var pica= await ctx.Pice
                                 .OrderBy(d => d.IdPice)
                                 .Select(d => new { d.IdPice, d.Naziv })
                                 .ToListAsync();
            ViewBag.Pica = new SelectList(pica, nameof(Pice.IdPice), nameof(Pice.Naziv));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StavkaRacuna stavka)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(stavka);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Stavka {stavka.Pice} uspješno dodan. Id kontakta = {stavka.Pice}";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(stavka);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(stavka);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int IdPice, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavka = await ctx.StavkaRacuna.FindAsync(IdPice);
            if (stavka != null)
            {
                try
                {
                    string naziv = stavka.Pice.Naziv;
                    ctx.Remove(stavka);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Stavka {naziv} obrisana.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja kontakta: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji stavka {IdPice}";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavka = await ctx.StavkaRacuna.FindAsync(id);
            if (stavka == null)
            {
                return NotFound("Ne postoji stavka s ID-om: " + id);
            }
            else
            {
                StavkaRacuna model = new StavkaRacuna
                {
                    IdRacun = stavka.IdRacun,
                    IdPice = id,
                    Kolicina = stavka.Kolicina,
                    JedCijena = stavka.JedCijena,
                    Iznos = stavka.Iznos
                };

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(stavka);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                StavkaRacuna stavka = await ctx.StavkaRacuna.FindAsync(id);
                if (stavka == null)
                {
                    return NotFound($"Nema navedene {id} stavka");
                }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<StavkaRacuna>(stavka, "", d => d.IdRacun, d => d.IdPice, d => d.Kolicina, d => d.JedCijena, d => d.Iznos);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Kontakt {stavka.Pice.Naziv} uspješno ažuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        await PrepareDropDownLists();
                        return View(stavka);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o planu nabave nije moguće povezati s forme.");
                    await PrepareDropDownLists();
                    return View(stavka);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
            }
        }


        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.StavkaRacuna
                           .AsNoTracking();
            int count = await query.CountAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var stavke = await query
                                   .Select(m => new StavkaRacunaViewModel
                                   {
                                       IdRacun = m.IdRacun,
                                       Pice = m.Pice.Naziv,
                                       Kolicina = m.Kolicina,
                                       JedCijena = m.JedCijena,
                                       Iznos = m.Iznos
                                   })
                                   .Skip((page - 1) * pagesize)
                                   .Take(pagesize)
                                   .ToListAsync();

            var model = new StavkeRacunaViewModel
            {
                StavkeRacuna = stavke,
                PagingInfo = pagingInfo
            };


            return View(model);
        }
    }
}
