using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafic.Models.Repositories
{
    public class KorisnikRepository
    {
        private KaficContext ctx;

        public KorisnikRepository(KaficContext ctx)
        {
            this.ctx = ctx;
        }

        public int InsertKorisnik(Korisnik korisnik)
        {
            ctx.Korisnik.Add(korisnik);
            ctx.SaveChanges();

            return korisnik.IdKorisnik;
        }

        public IEnumerable<Korisnik> GetKorisnik()
        {
            return ctx.Korisnik.ToList();
        }

        public Korisnik GetKorisnikById(int id)
        {
            return ctx.Korisnik.Find(id);
        }

        public void DeleteKorisnik(int id)
        {
            Korisnik korisnik = ctx.Korisnik.Find(id);
            ctx.Korisnik.Remove(korisnik);
            ctx.SaveChanges();
        }

      


    }
}
