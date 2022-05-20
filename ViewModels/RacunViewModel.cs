using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kafic.ViewModels
{
    public class RacunViewModel
    {
        public int IdRacun { get; set; }
        public int idKorisnik { get; set; }
        public string Korisnik { get; set; }
        public DateTime Datum { get; set; }
        public float UkupanIznos { get; set; }

        public int? idPrethodnogRacuna { get; set; }

        public string NazivPrethodnogRacuna { get; set; }

        public IEnumerable<StavkaRacunaViewModel> Stavke { get; set; }
        public RacunViewModel()
        {
            this.Stavke = new List<StavkaRacunaViewModel>();
        }

        [NotMapped]
        public int Position { get; set; }
    }
}
