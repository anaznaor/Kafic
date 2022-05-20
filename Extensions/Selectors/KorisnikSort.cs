using Kafic.Models;
using System;
using System.Linq;

namespace Kafic.Extensions.Selectors
{
    public static class KorisnikSort
    {
        public static IQueryable<Korisnik> ApplySort(this IQueryable<Korisnik> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Korisnik, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.IdKorisnik;
                    break;
                case 2:
                    orderSelector = d => d.Ime;
                    break;
                case 3:
                    orderSelector = d => d.Prezime;
                    break;
                case 4:
                    orderSelector = d => d.Oib;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}
