using System.Collections.Generic;
using Kafic.Models;

namespace Kafic.ViewModels
{
    public class PicaViewModel
    {
        public IEnumerable<PiceViewModel> Pica { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
