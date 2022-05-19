using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Smjena
    {
        public Smjena()
        {
            KonobarSmjenas = new HashSet<KonobarSmjena>();
        }

        public int IdSmjena { get; set; }
        public TimeSpan VrijemeOd { get; set; }
        public TimeSpan VrijemeDo { get; set; }

        public virtual ICollection<KonobarSmjena> KonobarSmjenas { get; set; }
    }
}
