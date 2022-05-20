using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Racun
    {
        public Racun()
        {
            StavkaRacunas = new HashSet<StavkaRacuna>();
        }

        public int IdRacun { get; set; }
        public int IdKorisnik { get; set; }
        public DateTime Datum { get; set; }
        public float UkupanIznos { get; set; }

        public virtual Korisnik Korisnik { get; set; }
        public virtual ICollection<StavkaRacuna> StavkaRacunas { get; set; }
    }
}
