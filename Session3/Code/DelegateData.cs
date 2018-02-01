using Session3.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Code
{
    public delegate void FilterByCabinType(object sender, DelegateFilterCabin data);
    public delegate void Filter(object sender, DelegateFilter filter);

    public class DelegateFilter : EventArgs
    {
        public List<TicketViewModel> list { get; set; }
    }
    public class DelegateFilterCabin : EventArgs
    {
        public int CabinType { get; set; }
    }
}
