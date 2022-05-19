using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class VrstaPica
    {
        public VrstaPica()
        {
            Pices = new HashSet<Pice>();
        }

        public int IdVrstaPica { get; set; }
        public string Vrsta { get; set; }

        public virtual ICollection<Pice> Pices { get; set; }
    }
}
