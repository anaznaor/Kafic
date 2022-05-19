using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class StavkaRacuna
    {
        public int IdRacun { get; set; }
        public int IdPice { get; set; }
        public int Kolicina { get; set; }
        public decimal JedCijena { get; set; }
        public float Iznos { get; set; }

        public virtual Pice IdPiceNavigation { get; set; }
        public virtual Racun IdRacunNavigation { get; set; }
    }
}
