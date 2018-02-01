using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Session3.Entity.ViewModel;
using Session3.Entity.Dao;
using Session3.Code;

namespace Session3.Views.FlightDetail
{

    public partial class FlightDetail : UserControl
    {
        public List<TicketViewModel> l { get; set; }
        Form1 form1;
        private TicketDao dao;

        // Delegate declaration 
        public delegate void OnButtonClick(string strValue);
        // Event declaration 
        public event OnButtonClick btnHandler;

        public FlightDetail(bool isReturn, List<TicketViewModel> list, Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            form1.applyHandler += Form1_applyHandler;
            form1.priceFilter += Form1_priceFilter;
            dao = new TicketDao();
            this.l = list;
            if (isReturn)
            {
                lbName.Text = "Return flight detail";
            }
            else
            {
                lbName.Text = "Outbound flight detail";
            }
            SetDataToGrid(l, 1);
        }
        //switch(cabinType)
        //    {
        //        case 1:
        //            return price;
        //        case 2:
        //            return (price * (35/100));
        //        case 3:
        //            return (price * ((35 / 100) + (30 / 100)));
        //        default:
        //            return price;
        //    }
        private void Form1_priceFilter(int cabinType)
        {
            switch(cabinType)
            {
                //Economy Price
                case 1:
                    l = l.OrderBy(x => x.CabinPrice).ToList();
                    break;
                //Business Price
                case 2:
                    l = l.OrderBy(x => x.BussinesPrice).ToList();
                    break;
                //First Class Price
                case 3:
                    l = l.OrderBy(x => x.FirstClassPrice).ToList();
                    break;
                default:
                    l = l.OrderBy(x => x.CabinPrice).ToList();
                    break;
            }
            SetDataToGrid(l, cabinType);
        }

        private void Form1_applyHandler(List<TicketViewModel> value)
        {
            this.l = value;
            SetDataToGrid(l, 1);
        }

        private void SetDataToGrid(List<TicketViewModel> list,int type)
        {
            if (list.Count > 0)
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                foreach (var item in list)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = item.From;
                    dataGridView1.Rows[i].Cells[1].Value = item.To;
                    dataGridView1.Rows[i].Cells[2].Value = item.Date.ToString("dd-MM-yyyy");
                    dataGridView1.Rows[i].Cells[3].Value = item.Time.ToString(@"hh\.mm");
                    dataGridView1.Rows[i].Cells[4].Value = item.FlightNumber;
                    switch(type)
                    {
                        case 1:
                            dataGridView1.Rows[i].Cells[5].Value = item.CabinPrice.ToString();
                            break;
                        case 2:
                            dataGridView1.Rows[i].Cells[5].Value = item.BussinesPrice.ToString();
                            break;
                        case 3:
                            dataGridView1.Rows[i].Cells[5].Value = item.FirstClassPrice.ToString();
                            break;
                        default:
                            dataGridView1.Rows[i].Cells[5].Value = item.CabinPrice.ToString();
                            break;

                    }
                    dataGridView1.Rows[i].Cells[6].Value = 0;
                    i++;
                }
            }
        }

        private void FlightDetail_Load(object sender, EventArgs e)
        {

        }

        private void ckbDay_CheckedChanged(object sender, EventArgs e)
        {
            // Check if event is null 
            if (btnHandler != null)
                btnHandler(string.Empty);
            // Write some text to output 
            Console.WriteLine("User Control’s Button Click");
        }
    }
}
