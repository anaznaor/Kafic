using FluentValidation;
using Kafic.Models;

namespace Kafic.ModelsValidation
{
    public class KorisnikValidator : AbstractValidator<Korisnik>
    {
        public KorisnikValidator()
        {

            RuleFor(d => d.Ime)
              .NotEmpty().WithMessage("Unesite ime ")
              .MaximumLength(10).WithMessage("Ime korisnika ne smije biti veći od 10");

            RuleFor(d => d.Prezime)
              .NotEmpty().WithMessage("Unesite prezime ")
              .MaximumLength(30).WithMessage("Prezime korisnika ne smije biti veći od 20");

            RuleFor(d => d.Oib)
              .NotEmpty().WithMessage("Unesite OIB")
              .MaximumLength(11).WithMessage("Oib ne smije biti veći od 11 znakova");

            RuleFor(d => d.Spol)
              .NotEmpty().WithMessage("Unesite M ili Z")
              .MaximumLength(1).WithMessage("Potrebno unijeti jedno slovo");

            RuleFor(d => d.Adresa)
              .NotEmpty().WithMessage("Unesite adresu ")
              .MaximumLength(60).WithMessage("Adresa korisnika ne smije biti veća od 60 znakova");

            RuleFor(d => d.PostanskiBroj)
              .NotEmpty().WithMessage("Unesite poštanski broj ")
              .MaximumLength(10).WithMessage("poštanski broj ne smije biti veći od 10");

            RuleFor(d => d.Mjesto)
              .NotEmpty().WithMessage("Unesite mjesto")
              .MaximumLength(20).WithMessage("Mjesto ne smije biti veći od 11 znakova");

            RuleFor(d => d.Drzava)
              .NotEmpty().WithMessage("Unesite M ili Z")
              .MaximumLength(15).WithMessage("Drzava ne smije biti dulja od 15");

            RuleFor(d => d.KorisnickoIme)
            .NotEmpty().WithMessage("Unesite korisnicko ime")
            .MaximumLength(30).WithMessage("Korisnicko ime ne smije biti dulj3 od 30");

            RuleFor(d => d.Lozinka)
            .NotEmpty().WithMessage("Unesite lozinku")
            .MaximumLength(30).WithMessage("Lozinka ne smije biti dulj3 od 30");

            RuleFor(d => d.DatumRodenja)
                .NotEmpty().WithMessage("Unesite datum rodenja");
        }
    }
}
