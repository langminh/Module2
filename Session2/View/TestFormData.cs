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
    public partial class TestFormData : Form
    {
        public string fileName { get; set; }
        public TestFormData()
        {
            InitializeComponent();
        }

        private void TestFormData_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "CSV files (*.csv)|*.csv";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = true;

            if (choofdlog.ShowDialog() == DialogResult.OK)
                fileName = choofdlog.FileName;
            else
                fileName = string.Empty;

            textBox1.Text = fileName;

            
        }

        private List<string[]> GetResult(string fileName)
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

        private void SetData(List<string[]> data)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
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

            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetData(GetResult(fileName));
        }
    }
}
