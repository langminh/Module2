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
    public partial class ConfirmDialog : Form
    {
        public ConfirmDialog(FlightViewModel temp, bool confirm)
        {
            InitializeComponent();
            if (confirm)
            {
                lbtile.Text = "Bạn có chắc chắn muốn xác nhận chuyến bay có mã số "+temp.FlightNumber+" từ " + temp.From + " tới " + temp.To + " này không?";
            }
            else
            {
                lbtile.Text = "Bạn có chắc chắn muốn hủy xác nhận chuyến bay có mã số " + temp.FlightNumber + "  từ " + temp.From + " tới " + temp.To + " này không?";

            }
        }
    }
}
