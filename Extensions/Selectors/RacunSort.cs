using Kafic.Models;
using System;
using System.Linq;

namespace Kafic.Extensions.Selectors
{
    public static class RacunSort
    {
        public static IQueryable<Racun> ApplySort(this IQueryable<Racun> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Racun, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.IdRacun;
                    break;
                case 2:
                    orderSelector = d => d.IdKorisnik;
                    break;
                case 3:
                    orderSelector = d => d.Datum;
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
