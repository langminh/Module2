using Session2.Entity.Dao;
using Session2.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session2.View
{
    public partial class EditSchedule : Form
    {
        public FlightViewModel model { get; set; }
        private FlightDao flightDao;
        public EditSchedule(FlightViewModel model)
        {
            InitializeComponent();
            this.model = model;

            flightDao = new FlightDao();
            model = flightDao.GetFlightViewModel(model.ID);

            lbFrom.Text = model.From;
            lbTo.Text = model.To;
            lbAircraft.Text = model.Aircraft;

            dtpDate.Value = model.DateFlight;
            dtpTime.Value = model.DateFlight.Add(model.TimeFlight);
            txtEconomyPrice.Text = model.EconomyPrice.ToString();
        }

        private void EditSchedule_Load(object sender, EventArgs e)
        {

        }

        //private bool CheckDateTimeChange(FlightViewModel model)
        //{
        //    return 
        //}

        //private bool CheckValidate(FlightViewModel model)
        //{

        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DateTime date = dtpDate.Value;
            TimeSpan time = dtpTime.Value.TimeOfDay;
            model.DateFlight = date;
            model.TimeFlight = time;
            if(!string.IsNullOrEmpty(txtEconomyPrice.Text))
            {
                model.EconomyPrice = decimal.Parse(txtEconomyPrice.Text);
            }
            flightDao.UpdateViewModel(model);
            this.Close();
        }
    }
}
