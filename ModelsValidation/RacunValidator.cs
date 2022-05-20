using FluentValidation;
using Kafic.Models;

namespace Kafic.ModelsValidation
{
    public class RacunValidator : AbstractValidator<Racun>
    {
        public RacunValidator()
        {

            RuleFor(d => d.UkupanIznos)
              .NotEmpty().WithMessage("Potrebno je unijeti broj");

            RuleFor(d => d.Datum)
              .NotEmpty().WithMessage("Potrebno je unijeti datum");
        }
    }
}
