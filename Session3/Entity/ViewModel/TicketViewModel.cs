using Session3.Entity.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Entity.ViewModel
{
    public class TicketViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int FromID { get; set; }
        public int ToID { get; set; }
        public int CabinID { get; set; }
        public string FlightNumber { get; set; }
        public decimal CabinPrice { get; set; }
        public decimal BussinesPrice
        {
            get
            {
                return Math.Round(Decimal.Multiply(CabinPrice, 1.3m), 3);
            }
        }
        public decimal FirstClassPrice
        {
            get
            {
                return Math.Round(Decimal.Multiply(CabinPrice, 1.5m), 3);
            }
        }
        public int NumberOfStop { get; set; }
        public List<List<Route>> route { get; set; }
    }
}
