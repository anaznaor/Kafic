using FluentValidation;
using Kafic.Models;

namespace Kafic.ModelsValidation
{
    public class StavkaRacunaValidator : AbstractValidator<StavkaRacuna>
    {
        public StavkaRacunaValidator()
        {
            RuleFor(m => m.IdRacun)
              .NotEmpty().WithMessage("Potrebno je odabrati id racuna");

            RuleFor(m => m.IdPice)
              .NotEmpty().WithMessage("Potrebno je odabrati id pica");

            RuleFor(m => m.Kolicina)
              .NotEmpty().WithMessage("Potrebno je unijeti kolicinu");

            RuleFor(m => m.JedCijena)
              .NotEmpty().WithMessage("Potrebno je unijeti jedinicnu cijenu");

            RuleFor(m => m.Iznos)
              .NotEmpty().WithMessage("Potrebno je unijeti iznos");
        }
    }
}