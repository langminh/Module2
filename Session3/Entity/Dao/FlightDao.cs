using Session3.Code;
using Session3.Entity;
using Session3.Entity.EF;
using Session3.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Entity.Dao
{

    public class FlightDao
    {
        private Session3DbContext dao;
        public FlightDao()
        {
            dao = new Session3DbContext();
        }


        public List<Schedule> GetAllSchedule()
        {
            var result = (from c in dao.Schedules select c);
            return result.ToList();
        }

        public List<Aircraft> GetListAircraft()
        {
            return (from c in dao.Aircrafts select c).OrderBy(x => x.Name).ToList();
        }

        public List<Route> GetListRoute()
        {
            return (from c in dao.Routes select c).ToList();
        }

        public List<Airport> GetAllAirport()
        {
            return (from c in dao.Airports select c).ToList();
        }

        public Aircraft GetAircraft(int ID)
        {
            return (Aircraft)(from cust in dao.Aircrafts
                              where cust.ID == ID
                              select cust);
        }

        public Airport GetAirport(int ID)
        {
            return (Airport)(from cust in dao.Airports
                             where cust.ID == ID
                             select cust);
        }

        public Schedule GetSchedule(DateTime date, string FlightNumber)
        {
            try
            {
                return (from c in dao.Schedules where (c.Date.Year == date.Year && c.Date.Month == date.Month && c.Date.Day == date.Day) && c.FlightNumber.Equals(FlightNumber) select c).FirstOrDefault();
            }
            catch { }
            return null;
        }

        public Route GetRoute(int ID)
        {
            return (Route)(from cust in dao.Routes
                           where cust.ID == ID
                           select cust);
        }

        public Airport GetAirport(string IATACode)
        {
            return (Airport)(from cust in dao.Airports
                             where cust.IATACode == IATACode
                             select cust).FirstOrDefault();
        }

        public int GetRouteByAirportID(int AirportIDFrom, int AirportIDTo)
        {
            var result = (Route)(from cust in dao.Routes
                                 where cust.DepartureAirportID == AirportIDFrom && cust.ArrivalAirportID == AirportIDTo
                                 select cust).FirstOrDefault();
            return result.ID;
        }


        public List<FlightViewModel> GetAllViewModel()
        {
            List<FlightViewModel> list = new List<FlightViewModel>();

            var result = GetAllSchedule();

            foreach (var item in result)
            {
                list.Add(GetFlightViewModel(item.ID));
            }
            return list.OrderByDescending(x => x.DateFlight).ToList();
        }

        public FlightViewModel GetFlightViewModel(int ID)
        {

            var result = new FlightViewModel();
            var temp = (from c in dao.Schedules where c.ID == ID select c).FirstOrDefault();
            var aircraft = (from d in dao.Aircrafts where d.ID == temp.AircraftID select d).FirstOrDefault();
            var route = (from e in dao.Routes where e.ID == temp.RouteID select e).FirstOrDefault();
            var airportFrom = (from f in dao.Airports where f.ID == route.DepartureAirportID select f).FirstOrDefault();
            var airportTo = (from g in dao.Airports where g.ID == route.ArrivalAirportID select g).FirstOrDefault();

            result.ID = temp.ID;
            result.Confirm = temp.Confirmed;
            result.DateFlight = temp.Date;
            result.TimeFlight = temp.Time;
            result.From = airportFrom.IATACode;
            result.FromID = airportFrom.ID;
            result.ToID = airportTo.ID;
            result.Aircraft = aircraft.Name;
            result.To = airportTo.IATACode;
            result.FlightNumber = temp.FlightNumber;
            result.EconomyPrice = temp.EconomyPrice;
            return result;
        }


        public DataResult UpdateChange(CustomData custom, DataResult result)
        {
            try
            {
                if (custom.Type.ToUpper().Equals("ADD"))
                {
                    //Thêm mới bản ghi

                    var temp = GetSchedule(custom._Date.Value, custom.FlightNumber);
                    if (temp == null)
                    {
                        //Nếu không có bản ghi nào sẵn trong database
                        if (AddSchedule(custom))
                        {
                            //nếu thêm bản ghi thành công
                            result.SuccessfulChange++;
                        }
                        else
                        {
                            //Nếu thêm bản ghi không thành công
                            result.RecordMissing++;
                        }
                    }

                    else
                    {
                        //Có bản ghi có sẵn trong database
                        result.DuplicateRecord++;
                    }
                }
                else if (custom.Type.ToUpper().Equals("EDIT"))
                {
                    var temp = GetSchedule(custom._Date.Value, custom.FlightNumber);
                    //Có bản ghi có sẵn trong database
                    result.DuplicateRecord++;
                    if (EditSchedule(custom, temp))
                    {
                        //Thực hiện sửa bản ghi thành công
                        result.SuccessfulChange++;
                    }
                    else
                    {
                        result.RecordMissing++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
        }

        public bool AddSchedule(CustomData custom)
        {
            Schedule result = new Schedule();
            try
            {
                result.Date = custom._Date.Value;
                result.Time = custom._Time.Value;
                result.AircraftID = custom.AircraftID.Value;
                result.EconomyPrice = custom.EconomyPrice.Value;
                result.FlightNumber = custom.FlightNumber;
                var from = GetAirport(custom.From);
                var to = GetAirport(custom.To);
                result.RouteID = GetRouteByAirportID(from.ID, to.ID);
                result.Confirmed = custom.Comfirmed;
                dao.Schedules.Add(result);
                dao.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }




        public bool EditSchedule(CustomData custom, Schedule result)
        {
            try
            {
                result.Date = custom._Date.Value;
                result.Time = custom._Time.Value;
                result.AircraftID = custom.AircraftID.Value;
                result.EconomyPrice = custom.EconomyPrice.Value;
                result.FlightNumber = custom.FlightNumber;
                var from = GetAirport(custom.From);
                var to = GetAirport(custom.To);
                result.RouteID = GetRouteByAirportID(from.ID, to.ID);
                result.Confirmed = custom.Comfirmed;
                dao.Schedules.Add(result);
                dao.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public FlightViewModel UpdateViewModel(FlightViewModel model)
        {
            var temp = (from c in dao.Schedules where c.ID == model.ID select c).FirstOrDefault();
            temp.Date = model.DateFlight;
            temp.Time = model.TimeFlight;
            temp.EconomyPrice = model.EconomyPrice;
            dao.SaveChanges();
            return GetFlightViewModel(temp.ID);
        }

        public FlightViewModel UpdateConfirmViewModel(FlightViewModel model)
        {
            var temp = (from c in dao.Schedules where c.ID == model.ID select c).FirstOrDefault();
            temp.Confirmed = !temp.Confirmed;
            dao.SaveChanges();
            return GetFlightViewModel(temp.ID);
        }
    }
}
