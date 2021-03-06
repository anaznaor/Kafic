using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kafic.Models;
using Microsoft.Extensions.Options;
using Kafic.Extensions;
using Kafic.ViewModels;
using Kafic.Extensions.Selectors;
using Kafic;

namespace Kafic.Controllers
{
    public class RacunController : Controller
    {
        private readonly KaficContext ctx;
        private readonly AppSettings appSettings;

        public RacunController(KaficContext ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionsSnapshot.Value;
        }
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownList();
            return View();

        }
        private void PrepareDropDownList()
        {
            var korisnici = ctx.Korisnik
                .Where(d => d.KorisnickoIme != "...")
                .OrderBy(d => d.KorisnickoIme)
                .Select(d => new { d.IdKorisnik, d.KorisnickoIme })
                .ToList();
            var gd2 = ctx.Korisnik
                .Where(d => d.KorisnickoIme == "...")
                .Select(d => new { d.IdKorisnik, d.KorisnickoIme })
                .FirstOrDefault();

            if (gd2 != null)
            {
                korisnici.Insert(0, gd2);
            }
            ViewBag.Korisnici = new SelectList(korisnici, nameof(Korisnik.IdKorisnik), nameof(Korisnik.KorisnickoIme));


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Racun racun)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(racun);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Racun {racun.IdRacun} uspjesno dodan.";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    PrepareDropDownList();
                    return View(racun);
                }
            }
            else
            {
                PrepareDropDownList();
                return View(racun);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int idRacun, int page = 1, int sort = 1, bool ascending = true)
        {
            var racun = ctx.Racun.Find(idRacun);
            if (racun == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    int id = racun.IdRacun;
                    ctx.Remove(racun);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Racun {id} uspjesno obrisan.";
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


        public IActionResult Edit(int id, int position, int page = 1, int sort = 1, bool ascending = true)
        {
            //var racun = ctx.Racun.AsNoTracking()
            //    .Where(d => d.IdRacun == id)
            //    .FirstOrDefault();
            //if (racun == null)
            //{
            //    return NotFound($"Nema navedenog {id} racuna");
            //}
            //else
            //{
            //    ViewBag.Page = page;
            //    ViewBag.Sort = sort;
            //    ViewBag.Ascending = ascending;
            //    PrepareDropDownList();
            //    return View(racun);
            //}
            return Show(id, position, page, sort, ascending, viewName: nameof(Edit));
        }


        [HttpPost, ActionName("Edit")]
        public IActionResult Edit(RacunViewModel model, int position, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            ViewBag.Position = position;
            if (ModelState.IsValid)
            {
                var racun = ctx.Racun
                                        .Include(d => d.StavkaRacunas)
                                        .Where(d => d.IdRacun == model.IdRacun)
                                        //.Select(d => new KorisnikViewModel { })
                                        .FirstOrDefault();
                //učitavanje kontakata
                var stavke = ctx.StavkaRacuna
                     .Where(s => s.IdRacun == racun.IdRacun)
                     .OrderBy(s => s.IdRacun)
                     .OrderBy(s => s.IdPice)
                     .Select(s => new StavkaRacunaViewModel
                     {
                         IdRacun = s.IdRacun,
                         Pice = s.Pice.Naziv,
                         Kolicina = s.Kolicina,
                         JedCijena = s.JedCijena,
                         Iznos = s.Iznos
                     })
                     .ToList();
                //plan.Stavke = stavkePlanaNabave;
                model.Stavke = stavke;

                if (racun == null)
                {
                    return NotFound("Ne postoji racun s id-om: " + model.IdRacun);
                }


                SetPreviousAndNext(position, sort, ascending);

                racun.IdKorisnik = model.idKorisnik;
                racun.Datum = model.Datum;
                racun.UkupanIznos = model.UkupanIznos;

                try
                {
                    List<string> idStavke = model.Stavke
                               .Where(s => s.IdRacun > 0 && s.Pice != null)
                                .Select(s => s.Pice)
                               .ToList();

                    var zaBrisanje = racun.StavkaRacunas.Where(s => !idStavke.Contains(s.Pice.Naziv));
                    ctx.RemoveRange(zaBrisanje);



                    foreach (var stavka in model.Stavke)
                    {
                        //ažuriraj postojeće i dodaj nove
                        StavkaRacuna novastavka; // potpuno nova ili dohvaćena ona koju treba izmijeniti
                        if (stavka.IdRacun > 0)
                        {
                            novastavka = racun.StavkaRacunas.First(s => s.IdRacun == stavka.IdRacun && s.Pice.Naziv.Equals(stavka.Pice));
                        }
                        else
                        {
                            novastavka = new StavkaRacuna();
                            racun.StavkaRacunas.Add(novastavka);
                        }
                        novastavka.IdPice = ctx.Pice
                            .Where(s => s.Naziv == stavka.Pice)
                            .Select(s => s.IdPice)
                            .FirstOrDefault();
                        novastavka.Kolicina = stavka.Kolicina;
                        novastavka.JedCijena = stavka.JedCijena;
                        novastavka.Iznos = stavka.Iznos;
                    }


                    ctx.SaveChanges();

                    TempData[Constants.Message] = $"Racun {racun.IdRacun} uspješno ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    PrepareDropDownList();
                    return RedirectToAction(nameof(Show), new
                    {
                        id = racun.IdRacun,
                        position,

                        page,
                        sort,
                        ascending
                    });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                SetPreviousAndNext(position, sort, ascending);
                return View(model);
            }
        }

        //[HttpPost, ActionName("Edit")]
        //public async Task<IActionResult> Update( int id, int position, int page = 1, int sort = 1, bool ascending = true)
        //{
        //    try
        //    {
        //        Racun racun = await ctx.Racun.FindAsync(id);
        //        if (racun == null)
        //        {
        //            return NotFound($"Nema navedenog {id} racuna");
        //        }
        //        ViewBag.Page = page;
        //        ViewBag.Sort = sort;
        //        ViewBag.Position = position;
        //        ViewBag.Ascending = ascending;
        //        bool ok = await TryUpdateModelAsync<Racun>(racun, "", d => d.IdRacun, d => d.IdKorisnik, d => d.Datum, d => d.UkupanIznos);
        //        if (ok)
        //        {
        //            try
        //            {
        //                TempData[Constants.Message] = $"Racun {racun.IdRacun} uspješno ažuriran.";
        //                TempData[Constants.ErrorOccurred] = false;
        //                await ctx.SaveChangesAsync();
        //                return RedirectToAction(nameof(Index), new { page, sort, ascending });
        //            }
        //            catch (Exception exc)
        //            {
        //                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
        //                PrepareDropDownList();
        //                return View(racun);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Podatke o racunu nije moguce povezati s forme.");
        //            PrepareDropDownList();
        //            return View(racun);
        //        }

        //    }
        //    catch (Exception exc)
        //    {
        //        TempData[Constants.Message] = exc.CompleteExceptionMessage();
        //        TempData[Constants.ErrorOccurred] = true;
        //        return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
        //    }
        //}



        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = ctx.Racun.AsNoTracking();

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



            var racuni = query
                .Select(d => new RacunViewModel
                {
                    IdRacun = d.IdRacun,
                    idKorisnik = d.IdKorisnik,
                    Korisnik = d.Korisnik.KorisnickoIme,
                    Datum = d.Datum,
                    UkupanIznos = d.UkupanIznos
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            for (int i = 0; i < racuni.Count; i++)
            {
                racuni[i].Position = (page - 1) * pageSize + i;
            }

            var model = new RacuniViewModel
            {
                Racuni = racuni,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        public IActionResult Show(int id, int position, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            ViewBag.Position = position;

            var racun = ctx.Racun
                .Where(d => d.IdRacun == id)
                .Select(d => new RacunViewModel
                {

                    IdRacun = d.IdRacun,
                    Korisnik = d.Korisnik.KorisnickoIme,
                    Datum = d.Datum,
                    UkupanIznos = d.UkupanIznos


                })
                .FirstOrDefault();

            if (racun == null)
            {
                return NotFound($"racun {id} ne postoji.");
            }
            else
            {
                SetPreviousAndNext(position, sort, ascending);

                if (racun.idPrethodnogRacuna.HasValue)
                {
                    racun.NazivPrethodnogRacuna = ctx.Racun
                                            .Where(d => d.IdRacun == racun.idPrethodnogRacuna)
                                            .Select(d => d.IdRacun + " " + d.Korisnik.KorisnickoIme)
                                            .FirstOrDefault();
                }


                var stavkeRacuna = ctx.StavkaRacuna
                    .Where(s => s.IdRacun == racun.IdRacun)
                    .OrderBy(s => s.IdRacun)
                    .OrderBy(s => s.IdPice)
                    .Select(s => new StavkaRacunaViewModel
                    {
                        IdRacun = s.IdRacun,
                        Pice = s.Pice.Naziv,
                        Kolicina = s.Kolicina,
                        JedCijena = s.JedCijena,
                        Iznos = s.Iznos

                    })
                    .ToList();
                racun.Stavke = stavkeRacuna;
                PrepareDropDownList();
                return View(viewName, racun);

            }
        }

        private void SetPreviousAndNext(int position, int sort, bool ascending)
        {
            var query = ctx.Racun.AsQueryable();

            //query = query.ApplySort(sort, ascending, query);
            if (position > 0)
            {
                ViewBag.Previous = query
                    .Select(d => d.IdRacun)
                    .Skip(position - 1)
                    .First();
            }
            if (position < query.Count() - 1)
            {
                ViewBag.Next = query
                                    .Select(d => d.IdRacun)
                                    .Skip(position + 1)
                                    .First();
            }
        }
    }
}
