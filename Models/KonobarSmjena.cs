using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class KonobarSmjena
    {
        public int IdKonobar { get; set; }
        public int IdSmjena { get; set; }
        public DateTime Datum { get; set; }

        public virtual Konobar IdKonobarNavigation { get; set; }
        public virtual Smjena IdSmjenaNavigation { get; set; }
    }
}
