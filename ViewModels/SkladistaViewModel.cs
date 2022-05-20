using Kafic.Models;
using System.Collections.Generic;

namespace Kafic.ViewModels
{
    public class SkladistaViewModel
    {
        public IEnumerable<SkladistaViewModel> Skladiste { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
