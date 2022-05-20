using Kafic.Models;
using System;
using System.Linq;

namespace Kafic.Extensions.Selectors
{
    public static class KonobarSort
    {
        public static IQueryable<Konobar> ApplySort(this IQueryable<Konobar> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Konobar, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.IdKonobar;
                    break;
                case 2:
                    orderSelector = d => d.DatumZaposlenja;
                    break;
                case 3:
                    orderSelector = d => d.DatumIstekaUgovora;
                    break;
                case 4:
                    orderSelector = d => d.Staz;
                    break;
                case 5:
                    orderSelector = d => d.Placa;
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