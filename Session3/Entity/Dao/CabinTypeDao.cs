using Session3.Entity.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Entity.Dao
{
    public class CabinTypeDao
    {
        Session3DbContext dao;
        public CabinTypeDao()
        {
            dao = new Session3DbContext();
        }

        public List<CabinType> GetListCabinType()
        {
            IQueryable<CabinType> model = (IQueryable<CabinType>)from row in dao.CabinTypes select row;
            model.OrderByDescending(x => x.Name);
            return model.ToList();
        }
    }
}
