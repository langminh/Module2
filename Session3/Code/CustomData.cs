using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Code
{
    public class CustomData
    {


        public string Type { get; set; }
        public DateTime? _Date { get; set; }
        public TimeSpan? _Time { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? AircraftID { get; set; }
        public decimal? EconomyPrice { get; set; }
        public bool Comfirmed { get; set; }
    }
    public class AccessData
    {
        public List<string[]> GetResult(string fileName)
        {
            var data = new List<string[]>();
            using (var sr = new System.IO.StreamReader(@fileName))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    data.Add(row);
                }
            }
            return data;
        }

        public DataTable SetData(List<string[]> data)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(TimeSpan));
            dt.Columns.Add("FlightNumber", typeof(string));
            dt.Columns.Add("From", typeof(string));
            dt.Columns.Add("To", typeof(string));
            dt.Columns.Add("AircraftID", typeof(string));
            dt.Columns.Add("EconomyPrice", typeof(string));
            dt.Columns.Add("Comfirmed", typeof(string));

            foreach (string[] row in data)
            {
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
