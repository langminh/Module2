using Session2.Code;
using Session2.Entity.Dao;
using Session2.Entity.EF;
using Session2.Entity.ViewModel;
using Session2.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session2
{
    public partial class Form1 : Form
    {

        private struct selectFilter
        {
            public bool filterFrom;
            public bool filterTo;
            public bool filterDate;
            public bool filterFlightNumber;
        }

        private List<FlightViewModel> list;
        private static List<FlightViewModel> listtemp = new List<FlightViewModel>();
        private FlightDao flightDao;
        private AirportDao dao;
        private Button btnCofirm;
        private int vitri = 0;
        public Form1()
        {
            InitializeComponent();

            flightDao = new FlightDao();
            list = new List<FlightViewModel>();

            list = flightDao.GetAllViewModel();

            btnCofirm = CofirmButton();
            btnCofirm.Click += BtnCofirm_Click;
            dao = new AirportDao();
            ComboboxItem combobox = new ComboboxItem();
            combobox.Text = "=====Airport=====";
            combobox.Value = 0;
            cbxFrom.Items.Add(combobox);
            cbxTo.Items.Add(combobox);

            foreach (var item in dao.GetListAirport())
            {
                ComboboxItem cb = new ComboboxItem();
                cb.Text = item.Name;
                cb.Value = item.ID;
                cbxFrom.Items.Add(cb);
                cbxTo.Items.Add(cb);
            }
            cbxFrom.SelectedIndex = 0;
            cbxTo.SelectedIndex = 0;

            cbxSort.Items.AddRange(new object[] { new ComboboxItem() { Text="Date", Value = 0}, new ComboboxItem(){Text="Time", Value = 1},
            new ComboboxItem(){Text="Economy Price", Value = 2}});
            cbxSort.SelectedIndex = 0;
            list = list.OrderByDescending(x => x.DateFlight).ToList();
        }

        private void BtnCofirm_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.SelectedRows[0].Index;

            DateTime date = DateTime.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
            string from = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string to = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string flightNumber = dataGridView1.Rows[index].Cells[4].Value.ToString();
            FlightViewModel temp = new FlightViewModel();
            temp = (listtemp.Where(x => x.DateFlight == date && x.From.Equals(from) && x.To.Equals(to) && x.FlightNumber.Equals(flightNumber)).FirstOrDefault());

            using (ConfirmDialog confirm = new ConfirmDialog(temp, true))
            {
                if (confirm.ShowDialog() == DialogResult.OK)
                {
                    //Update confirm to Schedule
                    flightDao.UpdateConfirmViewModel(temp);
                    list = GetReloadList();
                    SetDataToView(list);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ChangeBackcolor(int index)
        {
            DataGridViewRow row = dataGridView1.Rows[index];

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = Color.Red;
            style.ForeColor = Color.Black;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].Style = style;
            }
        }

        private List<FlightViewModel> GetReloadList()
        {
            return flightDao.GetAllViewModel();
        }

        private void SetDataToView(List<FlightViewModel> list)
        {
            int i = 0;
            listtemp = list;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            foreach (var item in list)
            {
                dataGridView1.Rows.Add();
                if (!item.Confirm)
                {
                    ChangeBackcolor(i);
                    dataGridView1.Rows[i].Cells[0].Value = item.DateFlight.ToString("dd/MM/yyyy");
                    dataGridView1.Rows[i].Cells[1].Value = item.TimeFlight.ToString(@"hh\.mm");
                    dataGridView1.Rows[i].Cells[2].Value = item.From;
                    dataGridView1.Rows[i].Cells[3].Value = item.To;
                    dataGridView1.Rows[i].Cells[4].Value = item.FlightNumber;
                    dataGridView1.Rows[i].Cells[5].Value = item.Aircraft;
                    dataGridView1.Rows[i].Cells[6].Value = item.EconomyPrice;
                    dataGridView1.Rows[i].Cells[7].Value = item.BussinesPrice;
                    dataGridView1.Rows[i].Cells[8].Value = item.FirstClass;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[0].Value = item.DateFlight.ToString("dd/MM/yyyy");
                    dataGridView1.Rows[i].Cells[1].Value = item.TimeFlight.ToString(@"hh\.mm");
                    dataGridView1.Rows[i].Cells[2].Value = item.From;
                    dataGridView1.Rows[i].Cells[3].Value = item.To;
                    dataGridView1.Rows[i].Cells[4].Value = item.FlightNumber;
                    dataGridView1.Rows[i].Cells[5].Value = item.Aircraft;
                    dataGridView1.Rows[i].Cells[6].Value = item.EconomyPrice;
                    dataGridView1.Rows[i].Cells[7].Value = item.BussinesPrice;
                    dataGridView1.Rows[i].Cells[8].Value = item.FirstClass;
                }
                i++;
            }
            dataGridView1.Rows[vitri].Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = vitri;
        }

        private void cbxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDataToView(list);
        }

        private DateTime CompareDate(DateTime begin)
        {
            return new DateTime(begin.Year, begin.Month, begin.Day);

        }

        private Button CofirmButton()
        {
            Button button = new Button();

            button.Text = "  Confirm flight";
            button.TextAlign = ContentAlignment.MiddleCenter;

            button.Image = (Session2.Properties.Resources.Checkmark_32px);
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.Location = btnCancel.Location;
            button.Size = btnCancel.Size;
            button.BackColor = SystemColors.Control;
            return button;

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

        private void btnApply_Click(object sender, EventArgs e)
        {
            GetFilter();
        }

        private void GetFilter()
        {
            selectFilter filter;
            filter.filterFrom = false;
            filter.filterTo = false;
            filter.filterDate = false;
            filter.filterFlightNumber = false;

            ComboboxItem from = cbxFrom.SelectedItem as ComboboxItem;
            ComboboxItem To = cbxTo.SelectedItem as ComboboxItem;
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
            if(!string.IsNullOrEmpty(txtDateTime.Text))
            {
                //nếu là định dạng ngày
                if (CheckDateValidate(txtDateTime))
                {
                    //lọc theo ngày
                    list = list.Where(x => (!string.IsNullOrEmpty(txtDateTime.Text)) ? ((x.DateFlight.Year == CompareDate(DateTime.Parse(txtDateTime.Text)).Year) && (x.DateFlight.Month == CompareDate(DateTime.Parse(txtDateTime.Text)).Month) && (x.DateFlight.Day == CompareDate(DateTime.Parse(txtDateTime.Text)).Day)) : false).ToList();
                    filter.filterDate = true;
                }
                else if (!string.IsNullOrEmpty(txtDateTime.Text))
                {
                    MessageBox.Show("Định dạng ngày không hợp lệ!!!", "Lỗi!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetDataToView(list);
                }

            }
            if(!string.IsNullOrEmpty(txtFlightNumber.Text))
            {
                //lọc theo số máy bay
                list = list.Where(x => (!string.IsNullOrEmpty(txtFlightNumber.Text)) ? x.FlightNumber == txtFlightNumber.Text : false).ToList();
                filter.filterFlightNumber = true;
            }
            if(!filter.filterFrom && !filter.filterTo && !filter.filterDate && !filter.filterFlightNumber)
            {
                list = flightDao.GetAllViewModel();
                SetDataToView(list);
                return;
            }

            SetDataToView(list);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEdit.Enabled = true;

            vitri = e.RowIndex;

            Color c = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

            if (c == Color.Red)
            {
                btnCancel.Visible = false;
                btnCofirm.Visible = true;
                btnCofirm.Enabled = true;
                panel2.Controls.Add(btnCofirm);
            }
            else
            {
                btnCancel.Visible = true;
                btnCofirm.Visible = false;
                btnCancel.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.SelectedRows[0].Index;

            DateTime date = DateTime.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
            string from = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string to = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string flightNumber = dataGridView1.Rows[index].Cells[4].Value.ToString();
            FlightViewModel temp = new FlightViewModel();
            temp = (list.Where(x => x.DateFlight == date && x.From.Equals(from) && x.To.Equals(to) && x.FlightNumber.Equals(flightNumber)).FirstOrDefault());
            using (EditSchedule edit = new EditSchedule(temp))
            {
                if(edit.ShowDialog() == DialogResult.OK)
                {
                    list = GetReloadList();
                    SetDataToView(list);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.SelectedRows[0].Index;

            DateTime date = DateTime.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
            string from = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string to = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string flightNumber = dataGridView1.Rows[index].Cells[4].Value.ToString();
            FlightViewModel temp = new FlightViewModel();
            temp = (list.Where(x => x.DateFlight == date && x.From.Equals(from) && x.To.Equals(to) && x.FlightNumber.Equals(flightNumber)).FirstOrDefault());

            using (ConfirmDialog confirm = new ConfirmDialog(temp, false))
            {
                if (confirm.ShowDialog() == DialogResult.OK)
                {
                    //Update confirm to Schedule
                    flightDao.UpdateConfirmViewModel(temp);
                    list = GetReloadList();
                    SetDataToView(list);
                }
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateChange update = new UpdateChange();
            update.ShowDialog();
            update.success += Update_success;
            

        }

        private void Update_success(object sender, DelegateHandler handler)
        {
            if (handler.IsCompelete)
            {
                list = flightDao.GetAllViewModel();
                SetDataToView(list);
            }
        }
    }
}
