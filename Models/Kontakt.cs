using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Kontakt
    {
        public int IdKorisnik { get; set; }
        public string Kontakt1 { get; set; }
        public string Vrsta { get; set; }

        public virtual Korisnik IdKorisnikNavigation { get; set; }
    }
}
