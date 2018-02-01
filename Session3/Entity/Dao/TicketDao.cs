using Session3.Entity.EF;
using Session3.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Entity.Dao
{
    public class TicketDao
    {
        Session3DbContext dao;
        public TicketDao()
        {
            dao = new Session3DbContext();
        }


        public decimal Price(int cabinType, decimal price)
        {
            switch(cabinType)
            {
                case 1:
                    return price;
                case 2:
                    return (price * (35/100));
                case 3:
                    return (price * ((35 / 100) + (30 / 100)));
                default:
                    return price;
            }
            
        }


        public List<List<Route>> NumberOfStop(int From, int To)
        {
            int stop = 0;
            List<List<Route>> list = new List<List<Route>>();
            var route = (from c in dao.Routes where c.DepartureAirportID == From && c.ArrivalAirportID == To select c).First();
            if (route != null)
            {
                List<Route> b = new List<Route>();
                b.Add(route);
                list.Add(b);
                return list;
            }
            else
            {
                var from = (from a in dao.Routes where a.DepartureAirportID == From select a).ToList();
                foreach(var item in from)
                {
                    var to = (from v in dao.Routes where v.ArrivalAirportID == item.ArrivalAirportID select v).ToList();
                    if (to.Count > 0)
                    {
                        list.Add(to);
                    }
                    else
                    {
                        //NumberOfStop(item.DepartureAirportID, )
                    }
                }

            }
            return list;
        }


        public List<Schedule> GetList(string From, string To)
        {
            var routeFrom = (from c in dao.Schedules where c.Route.DepartureAirportID.Equals(From) select c).OrderBy(x => x.Route.Distance).ToList();
            var routeTo = (from c in dao.Schedules where c.Route.ArrivalAirportID.Equals(To) select c).OrderBy(x => x.Route.Distance).ToList();

            List<Schedule> list = new List<Schedule>();
            foreach (var item in routeFrom)
            {
                foreach(var sTo in routeTo)
                {
                    //lấy đk các lịch trình bay trực tiếp
                    if(item.Route.ArrivalAirportID == sTo.Route.ArrivalAirportID && item.Route.DepartureAirportID == sTo.Route.DepartureAirportID)
                    {
                        list.Add(sTo);
                    }
                    //Lấy các lịch trình bay gián tiếp?
                    else
                    {

                    }
                }
            }
            return list;
        }

        public List<TicketViewModel> GetScheduleFlight(string From, string To)
        {
            var list = new List<TicketViewModel>();
            return list;
        }

        public List<TicketViewModel> GetAllFlight()
        {
            List<TicketViewModel> list = new List<TicketViewModel>();
            var query = (from c in dao.Schedules select c).ToList();
            foreach (var obj in query)
            {
                var result = new TicketViewModel();
                var from = (from a in dao.Airports where a.ID == obj.Route.DepartureAirportID select a).First();
                result.From = from.Name;
                var to = (from a in dao.Airports where a.ID == obj.Route.ArrivalAirportID select a).First();
                result.FromID = from.ID;
                result.To = to.Name;
                result.ToID = to.ID;
                result.FlightNumber = obj.FlightNumber;
                result.Date = obj.Date;
                result.Time = obj.Time;
                result.CabinPrice = obj.EconomyPrice;
                list.Add(result);
            }
            return list;

        }

        public List<TicketViewModel> GetAllFlight(int cabinType)
        {
            List<TicketViewModel> list = new List<TicketViewModel>();
            var query = (from c in dao.Schedules select c).ToList();
            foreach (var obj in query)
            {
                var result = new TicketViewModel();
                var from = (from a in dao.Airports where a.ID == obj.Route.DepartureAirportID select a).First();
                result.From = from.Name;
                var to = (from a in dao.Airports where a.ID == obj.Route.ArrivalAirportID select a).First();
                result.FromID = from.ID;
                result.To = to.Name;
                result.ToID = to.ID;
                result.Date = obj.Date;
                result.Time = obj.Time;
                result.FlightNumber = obj.FlightNumber;
                result.CabinPrice = Price(cabinType, obj.EconomyPrice);
                result.route = NumberOfStop(from.ID, to.ID);
                list.Add(result);
            }
            return list.OrderBy(x => x.CabinPrice).ToList();
        }
    }
}
