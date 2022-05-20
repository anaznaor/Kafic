using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kafic.ViewModels
{
    public class KorisnikViewModel
    {
        public int IdKorisnik { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Spol { get; set; }
        public string Oib { get; set; }
        public DateTime DatumRodenja { get; set; }
        public string Adresa { get; set; }
        public string PostanskiBroj { get; set; }
        public string Mjesto { get; set; }
        public string Drzava { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public IEnumerable<RacunViewModel> Racuni { get; set; }
        public KorisnikViewModel()
        {
            this.Racuni = new List<RacunViewModel>();
        }
        [NotMapped]

        public int Position { get; set; }
    }
}
