using System.Collections.Generic;
using Kafic.Models;

namespace Kafic.ViewModels
{
    public class KorisniciViewModel
    {
        public IEnumerable<KorisnikViewModel> Korisnici { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
