using System;
using System.Collections.Generic;

namespace Kafic.Models
{
    public partial class Korisnik
    {
        public Korisnik()
        {
            Kontakts = new HashSet<Kontakt>();
            Racuns = new HashSet<Racun>();
        }

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

        public virtual Konobar Konobar { get; set; }
        public virtual Vlasnik Vlasnik { get; set; }
        public virtual ICollection<Kontakt> Kontakts { get; set; }
        public virtual ICollection<Racun> Racuns { get; set; }
    }
}
