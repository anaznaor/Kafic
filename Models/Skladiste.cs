using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Skladiste
    {
        public int IdPice { get; set; }
        public int TrenutnaKolicina { get; set; }
        public int Kapacitet { get; set; }

        public virtual Pice IdPiceNavigation { get; set; }
    }
}
