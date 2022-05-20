using Kafic.Models;
using System.Collections.Generic;

namespace Kafic.ViewModels
{
    public class RacuniViewModel
    {
        public IEnumerable<RacunViewModel> Racuni { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
