using FluentValidation;
using Kafic.Models;

namespace Kafic.ModelsValidation
{
    public class KonobarValidator : AbstractValidator<Konobar>
    {
        public KonobarValidator()
        {
            RuleFor(m => m.DatumZaposlenja)
              .NotEmpty().WithMessage("Potrebno je unijeti datum zaposlenja");

            RuleFor(m => m.Staz)
              .NotEmpty().WithMessage("Potrebno je unijeti staz");

            RuleFor(m => m.Placa)
              .NotEmpty().WithMessage("Potrebno je unijeti plaću");

            RuleFor(m => m.DatumIstekaUgovora)
                .GreaterThan(m => m.DatumZaposlenja).WithMessage("Datum isteka ugovora mora biti veći od datuma zaposlenja!");
        }
    }
}
