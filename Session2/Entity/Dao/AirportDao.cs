using Session2.Entity.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2.Entity.Dao
{
    public class AirportDao
    {
        private Session2DbContext dao;
        public AirportDao()
        {
            dao = new Session2DbContext();
        }

        public List<Airport> GetListAirport(
            )
        {
            IQueryable<Airport> model = (IQueryable<Airport>)from row in dao.Airports select row;
            model.OrderByDescending(x => x.Name);
            return model.ToList();
        }
    }
}
