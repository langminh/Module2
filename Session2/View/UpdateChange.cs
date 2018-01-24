using Session2.Code;
using Session2.Entity.Dao;
using Session2.Entity.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session2.View
{
    public partial class UpdateChange : Form
    {
        public string fileName { get; set; }
        private AccessData data;
        private DataResult result;
        private FlightDao dao;
        private DataTable t;
        private int checkDone = 0;
        private delegate void DELEG();
        public event InSuccessUpdate success = null;

        BackgroundWorker worker;
        public UpdateChange()
        {
            InitializeComponent();
            data = new AccessData();
            result = new DataResult();
            dao = new FlightDao();
            t = new DataTable();
            worker = new BackgroundWorker();
        }

        private void AsysLoad()
        {
            List<string[]> list = data.GetResult(fileName);
            t = data.SetData(list);
            Update();
            checkDone++;
        }

        private void SetLabel(int t1, int t2, int t3)
        {
            Label lb1 = new Label();
            Label lb2 = new Label();
            Label lb3 = new Label();

            lb1.Location = lbSuccess.Location;
            lb2.Location = lbDuplicate.Location;
            lb3.Location = lbError.Location;

            lb1.Size = lb2.Size = lb3.Size = lbSuccess.Size;
            lb1.Text = t1.ToString();
            lb2.Text = t2.ToString();
            lb3.Text = t3.ToString();
            Label[] lb = { lb1, lb2, lb3 };
            groupBox1.Controls.AddRange(lb);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
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
                if (checkDone == 1)
                {
                    result.Dispose();
                    result = new DataResult();
                    lbDuplicate.Text = "[xxx]";
                    lbError.Text = "[xxx]";
                    lbSuccess.Text = "[xxx]";
                    checkDone = 0;
                }


                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Include path file");
                }
                else
                {
                    worker.DoWork += Worker_DoWork;
                    worker.RunWorkerAsync();
                }
            }
            catch { }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Delegate del = new DELEG(AsysLoad);
            this.Invoke(del);
        }

        private CustomData CreateEntity(DataRow dr)
        {
            CustomData result = new CustomData();
            try
            {
                result.Type = dr["Type"].ToString();
                DateTime t;
                DateTime.TryParse(dr["Date"].ToString(), out t);

                result._Date = t;

                result._Time = (TimeSpan)dr["Time"];

                result.FlightNumber = dr["FlightNumber"].ToString();
                result.From = dr["From"].ToString();
                result.To = dr["To"].ToString();
                //int b;
                //int.TryParse(dr["AircraftID"].ToString(),out b);

                result.AircraftID = int.Parse(dr["AircraftID"].ToString());

                //decimal a;
                //decimal.TryParse(dr["EconomyPrice"].ToString(), out a);
                result.EconomyPrice = decimal.Parse(dr["EconomyPrice"].ToString());
                result.Comfirmed = (dr["Comfirmed"].ToString().Equals("OK")) ? true : false;
                return result;
            }
            catch { }
            return new CustomData();
        }

        private void Update()
        {
            foreach (DataRow c in t.Rows)
            {
                result = dao.UpdateChange(CreateEntity(c), result);
            }
            lbSuccess.Text = result.SuccessfulChange.ToString();
            lbDuplicate.Text = result.DuplicateRecord.ToString();
            lbError.Text = result.RecordMissing.ToString();

            //SetLabel(result.SuccessfulChange, result.DuplicateRecord, result.RecordMissing);
            MessageBox.Show("Successfully.");
        }

        private void UpdateChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(checkDone == 1 && success != null)
            {
                success(sender, new DelegateHandler() { IsCompelete = true });
            }
        }
    }
}
