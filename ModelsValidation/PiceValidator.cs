using FluentValidation;
using Kafic.Models;


namespace Kafic.ModelsValidation
{
    public class PiceValidator : AbstractValidator<Pice>
    {
        public PiceValidator()
        {
            RuleFor(d => d.Naziv)
              .NotEmpty().WithMessage("Unesite naziv pića ")
              .MaximumLength(30).WithMessage("Naziv ne smije biti veći od 30");

            RuleFor(m => m.IdVrstaPica)
                .NotEmpty().WithMessage("Potrebno je odabrati vrstu pića");

            RuleFor(m => m.JedCijena)
                .NotEmpty().WithMessage("Potrebno je unijeti cijenu");

            RuleFor(m => m.NabavnaCijena)
                .NotEmpty().WithMessage("Potrebno je unijeti nabavnu cijenu");
        }
    }
}
