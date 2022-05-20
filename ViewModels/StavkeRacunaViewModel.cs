using System.Collections.Generic;
using Kafic.Models;

namespace Kafic.ViewModels
{
    public class StavkeRacunaViewModel
    {
        public IEnumerable<StavkaRacunaViewModel> StavkeRacuna { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
