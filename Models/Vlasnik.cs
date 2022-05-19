using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Vlasnik
    {
        public int IdVlasnik { get; set; }
        public DateTime DatumKupnjeKafica { get; set; }
        public string NazivKafica { get; set; }

        public virtual Korisnik IdVlasnikNavigation { get; set; }
    }
}
