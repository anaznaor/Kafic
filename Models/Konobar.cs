using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Konobar
    {
        public Konobar()
        {
            KonobarSmjenas = new HashSet<KonobarSmjena>();
        }

        public int IdKonobar { get; set; }
        public DateTime DatumZaposlenja { get; set; }
        public int Staz { get; set; }
        public decimal Placa { get; set; }
        public DateTime? DatumIstekaUgovora { get; set; }

        public virtual Korisnik IdKonobarNavigation { get; set; }
        public virtual ICollection<KonobarSmjena> KonobarSmjenas { get; set; }
    }
}
