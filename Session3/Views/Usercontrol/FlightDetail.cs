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

namespace Session3.Views.FlightDetail
{
    public partial class FlightDetail : UserControl
    {
        public List<TicketViewModel> list { get; set; }
        Form1 form1;
        private TicketDao dao;

        public FlightDetail(bool isReturn, List<TicketViewModel> list)
        {
            InitializeComponent();
            form1 = new Form1();
            form1.filter += Form1_filter;
            dao = new TicketDao();
            this.list = list;
            if (isReturn)
            {
                lbName.Text = "Return flight detail";
            }
            else
            {
                lbName.Text = "Outbound flight detail";
            }

            SetDataToGrid();
        }

        private void Form1_filter(object sender, Code.DelegateFilter filter)
        {
            this.list = filter.list;
            SetDataToGrid();
        }

        private void SetDataToGrid()
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
                    dataGridView1.Rows[i].Cells[5].Value = item.CabinPrice.ToString();
                    dataGridView1.Rows[i].Cells[6].Value = 0;
                    i++;
                }
            }
        }
    }
}
