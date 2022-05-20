using Kafic.Models;
using System;
using System.Linq;

namespace Kafic.Extensions.Selectors
{
    public static class StavkaRacunaSort
    {
        public static IQueryable<StavkaRacuna> ApplySort(this IQueryable<StavkaRacuna> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<StavkaRacuna, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.IdRacun;
                    break;
                case 2:
                    orderSelector = d => d.IdPice;
                    break;
                case 3:
                    orderSelector = d => d.Kolicina;
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
