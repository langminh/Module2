using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2.Code
{

    public delegate void InSuccessUpdate(object sender, DelegateHandler handler);
    public class DelegateHandler : EventArgs
    {
        public bool IsCompelete { get; set; }
    }
}
