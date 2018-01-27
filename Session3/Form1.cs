using Session3.Code;
using Session3.Entity.Dao;
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

        public Form1()
        {
            InitializeComponent();
            airportDao = new AirportDao();
            cabinTypeDao = new CabinTypeDao();

            SetView();

            flightDetailReturn = new FlightDetail(true);
            flightDetailOneWay = new FlightDetail(false);
            flightDetailOneWay.Dock = DockStyle.Fill;
            pnMainLoad.Controls.Add(flightDetailOneWay);
            
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
    }
}
