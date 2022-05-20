using Kafic.Models;
using System;
using System.Linq;

namespace Kafic.Extensions.Selectors
{
    public static class PiceSort
    {
        public static IQueryable<Pice> ApplySort(this IQueryable<Pice> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Pice, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.IdPice;
                    break;
                case 2:
                    orderSelector = d => d.Naziv;
                    break;
                case 3:
                    orderSelector = d => d.IdVrstaPica;
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
