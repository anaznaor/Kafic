using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Datum", Prompt = "Unesi datum")]
        [Required(ErrorMessage = "Datum je obavezno polje")]
        public DateTime Datum { get; set; }
        [Display(Name = "UkupanIznos", Prompt = "Unesi ukupanIznos")]
        [Required(ErrorMessage = "UkupanIznos mora bti veći od 0")]
        [RegularExpression("^[1-9][0-9]*$")]
        public float UkupanIznos { get; set; }

        public virtual Korisnik Korisnik { get; set; }
        public virtual ICollection<StavkaRacuna> StavkaRacunas { get; set; }
    }
}
