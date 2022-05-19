using Kafic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafic.Controllers
{
    public class KonobarController : Controller
    {
        private readonly KaficContext ctx;

        public KonobarController(KaficContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var konobari = ctx.Konobar
                                .AsNoTracking()
                                .OrderBy(k => k.DatumZaposlenja)
                                .ToList();
            return View(konobari);
        }
    }
}
