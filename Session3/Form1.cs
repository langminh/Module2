using Session3.Code;
using Session3.Entity.Dao;
using Session3.Entity.ViewModel;
using Session3.Views.FlightDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session3
{
    public partial class Form1 : Form
    {
        private FlightDetail flightDetailReturn;
        private FlightDetail flightDetailOneWay;
        private AirportDao airportDao;
        private CabinTypeDao cabinTypeDao;
        private List<TicketViewModel> list;
        private TicketDao dao;
        private bool enableReturn;

        public event FilterByCabinType filterCabin = null;
        public event Filter filter = null;

        private struct selectFilter
        {
            public bool filterFrom;
            public bool filterTo;
            public bool filterDateOut;
            public bool filterDateReturn;
        }

        public Form1()
        {
            InitializeComponent();
            airportDao = new AirportDao();
            cabinTypeDao = new CabinTypeDao();

            dao = new TicketDao();
            list = dao.GetAllFlight();

            SetView();

            flightDetailReturn = new FlightDetail(true, list);
            
            
        }

        private void SetView()
        {
            cboFrom.Items.Add(GetItem("=====Airport=====", 0));
            cboTo.Items.Add(GetItem("=====Airport=====", 0));
            cboType.Items.Add(GetItem("====Cabin type====", 0));

            foreach (var item in airportDao.GetListAirport())
            {
                ComboboxItem cb = new ComboboxItem();
                cb.Text = item.Name;
                cb.Value = item.ID;
                cboFrom.Items.Add(cb);
                cboTo.Items.Add(cb);
            }

            foreach (var item in cabinTypeDao.GetListCabinType())
            {
                ComboboxItem cb = new ComboboxItem();
                cb.Text = item.Name;
                cb.Value = item.ID;
                cboType.Items.Add(cb);
            }

            cboFrom.SelectedIndex = 0;
            cboTo.SelectedIndex = 0;
            cboType.SelectedIndex = 0;
        }

        private ComboboxItem GetItem(string text, int value)
        {
            return new ComboboxItem() { Text = text, Value = value };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void rdoReturn_CheckedChanged(object sender, EventArgs e)
        {
            flightDetailOneWay.Dock = DockStyle.Top;
            flightDetailReturn.Dock = DockStyle.Bottom;
            flightDetailReturn.Location = new Point(0, flightDetailOneWay.Height);
            pnMainLoad.Controls.Add(flightDetailReturn);
        }

        private void rdiOneWay_CheckedChanged(object sender, EventArgs e)
        {
            if(flightDetailReturn != null)
            {
                pnMainLoad.Controls.Remove(flightDetailReturn);
                flightDetailOneWay.Dock = DockStyle.Fill;
            }
        }

        private void btnBookFlight_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private bool CheckDateValidate(TextBox t)
        {
            try
            {
                DateTime.Parse(t.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private DateTime CompareDate(DateTime begin)
        {
            return new DateTime(begin.Year, begin.Month, begin.Day);

        }

        private List<TicketViewModel> GetFilter()
        {
            selectFilter filter;
            filter.filterFrom = false;
            filter.filterTo = false;
            filter.filterDateOut = false;
            filter.filterDateReturn = false;

            ComboboxItem from = cboFrom.SelectedItem as ComboboxItem;
            ComboboxItem To = cboTo.SelectedItem as ComboboxItem;
            if (from.Value != 0)
            {
                //lọc theo nơi đi
                list = list.Where(x => (from.Value != 0) ? x.FromID == from.Value : false).ToList();
                filter.filterFrom = true;
            }
            if (To.Value != 0)
            {
                //lọc theo nơi đến
                list = list.Where(x => (To.Value != 0) ? x.ToID == To.Value : false).ToList();
                filter.filterTo = true;
            }
            if (!string.IsNullOrEmpty(txtDateOut.Text))
            {
                //nếu là định dạng ngày
                if (CheckDateValidate(txtDateOut))
                {
                    //lọc theo ngày
                    list = list.Where(x => (!string.IsNullOrEmpty(txtDateOut.Text)) ? ((x.Date.Year == CompareDate(DateTime.Parse(txtDateOut.Text)).Year) && (x.Date.Month == CompareDate(DateTime.Parse(txtDateOut.Text)).Month) && (x.Date.Day == CompareDate(DateTime.Parse(txtDateOut.Text)).Day)) : false).ToList();
                    filter.filterDateOut = true;
                }
                else if (!string.IsNullOrEmpty(txtDateOut.Text))
                {
                    MessageBox.Show("Định dạng ngày không hợp lệ!!!", "Lỗi!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }

            }
            if (!string.IsNullOrEmpty(txtDateReturn.Text) && enableReturn)
            {
                //nếu là định dạng ngày
                if (CheckDateValidate(txtDateReturn))
                {
                    //lọc theo ngày
                    list = list.Where(x => (!string.IsNullOrEmpty(txtDateReturn.Text)) ? ((x.Date.Year == CompareDate(DateTime.Parse(txtDateReturn.Text)).Year) && (x.Date.Month == CompareDate(DateTime.Parse(txtDateReturn.Text)).Month) && (x.Date.Day == CompareDate(DateTime.Parse(txtDateReturn.Text)).Day)) : false).ToList();
                    filter.filterDateReturn = true;
                }
                else if (!string.IsNullOrEmpty(txtDateOut.Text))
                {
                    MessageBox.Show("Định dạng ngày không hợp lệ!!!", "Lỗi!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            if (!filter.filterFrom && !filter.filterTo && !filter.filterDateOut && !filter.filterDateReturn)
            {
                list = dao.GetAllFlight();
                return list;
            }
            else
            {
                return list;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //List<TicketViewModel> l = GetFilter();
            //if (filter != null)
            //{
            //    filter(this, new DelegateFilter() { list = l });
            //}
        }
    }
}
