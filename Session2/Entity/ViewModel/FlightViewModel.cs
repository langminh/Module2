using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2.Entity.ViewModel
{
    public class FlightViewModel
    {
        public int ID { get; set; }
        public DateTime DateFlight { get; set; }
        public TimeSpan TimeFlight { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Aircraft { get; set; }
        public string FlightNumber { get; set; }
        public decimal EconomyPrice { get; set; }
        public int FromID { get; set; }
        public int ToID { get; set; }
        public bool Confirm { get; set; }
        public decimal BussinesPrice {
            get {
                return Math.Round(Decimal.Multiply(EconomyPrice, 1.3m), 3);
            }
        }
        public decimal FirstClass {
            get
            {
                return Math.Round(Decimal.Multiply(EconomyPrice, 1.5m), 3);
            }
        }
    }
}
