using Session3.Entity.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Entity.Dao
{
    public class AirportDao
    {
        private Session3DbContext dao;
        public AirportDao()
        {
            dao = new Session3DbContext();
        }

        public List<Airport> GetListAirport()
        {
            IQueryable<Airport> model = (IQueryable<Airport>)from row in dao.Airports select row;
            model.OrderByDescending(x => x.Name);
            return model.ToList();
        }
    }
}
