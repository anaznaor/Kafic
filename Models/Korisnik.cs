using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Ime", Prompt = "Unesi ime")]
        [Required(ErrorMessage = "Ime je obavezno polje")]
        public string Ime { get; set; }
        [Display(Name = "Prezime", Prompt = "Unesi prezime")]
        [Required(ErrorMessage = "Prezime je obavezno polje")]
        public string Prezime { get; set; }
        [Display(Name = "Spol", Prompt = "Spol")]
        [Required]
        [RegularExpression("[MZ]")]
        public string Spol { get; set; }
        [Display(Name = "Oib", Prompt = "Unesi oib")]
        [Required(ErrorMessage = "Oib je obavezno polje")]
        [RegularExpression("[0-9]{11}")]
        public string Oib { get; set; }
        [Display(Name = "DatumRodenja", Prompt = "Unesi datum rodenja")]
        [Required(ErrorMessage = "DatumRodenja je obavezno polje")]
        public DateTime DatumRodenja { get; set; }
        [Display(Name = "Adresa", Prompt = "Unesi Adresa")]
        [Required(ErrorMessage = "Adresa je obavezno polje")]
        public string Adresa { get; set; }
        [Display(Name = "PostanskiBroj", Prompt = "Unesi PostanskiBroj")]
        [Required(ErrorMessage = "PostanskiBroj je obavezno polje")]
        public string PostanskiBroj { get; set; }
        [Display(Name = "Mjesto", Prompt = "Unesi Mjesto")]
        [Required(ErrorMessage = "Mjesto je obavezno polje")]
        public string Mjesto { get; set; }
        [Display(Name = "Drzava", Prompt = "Unesi Drzava")]
        [Required(ErrorMessage = "Drzava je obavezno polje")]
        public string Drzava { get; set; }
        [Display(Name = "KorisnickoIme", Prompt = "Unesi KorisnickoIme")]
        [Required(ErrorMessage = "KorisnickoIme je obavezno polje")]
        public string KorisnickoIme { get; set; }
        [Display(Name = "Lozinka", Prompt = "Unesi Lozinka")]
        [Required(ErrorMessage = "Lozinka je obavezno polje")]
        public string Lozinka { get; set; }

        public virtual Konobar Konobar { get; set; }
        public virtual Vlasnik Vlasnik { get; set; }
        public virtual ICollection<Kontakt> Kontakts { get; set; }
        public virtual ICollection<Racun> Racuns { get; set; }
    }
}
