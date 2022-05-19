using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Pice
    {
        public Pice()
        {
            StavkaRacunas = new HashSet<StavkaRacuna>();
        }

        public int IdPice { get; set; }
        public string Naziv { get; set; }
        public int IdVrstaPica { get; set; }
        public decimal JedCijena { get; set; }
        public decimal NabavnaCijena { get; set; }

        public virtual VrstaPica IdVrstaPicaNavigation { get; set; }
        public virtual Skladiste Skladiste { get; set; }
        public virtual ICollection<StavkaRacuna> StavkaRacunas { get; set; }
    }
}
