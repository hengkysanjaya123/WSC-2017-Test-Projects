using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fresh
{
    public partial class ScheduleEdit : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Schedule s;

        public ScheduleEdit(Schedule s)
        {
            InitializeComponent();
            this.s = s;
        }

        private void ScheduleEdit_Load(object sender, EventArgs e)
        {
            label2.Text = s.Route.Airport.IATACode;
            label4.Text = s.Route.Airport1.IATACode;
            label6.Text = s.Aircraft.Name;
            dateTimePicker1.Value = s.Date.Date;
            dateTimePicker2.Value = s.Date.Date + s.Time;
            textBox1.Text = Math.Floor(s.EconomyPrice).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            decimal price = 0;
            if (!decimal.TryParse(textBox1.Text, out price))
            {
                MessageBox.Show("Please input price with correct format");
                return;
            }

            if (price <= 0 || price > 922000000000000)
            {
                MessageBox.Show("Price must be between 1 and 922.000.000.000.000");
                return;
            }


            DateTime start = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;
            DateTime end = start + TimeSpan.FromMinutes(s.Route.FlightTime);

            if (s.Confirmed)
            {
                if (!CheckOverlap(s, start, end))
                {
                    MessageBox.Show("Sorry, schedule overlap");
                    return;
                }
            }

            var q = db.Schedules.Where(x => x.ID == s.ID).FirstOrDefault();
            q.Date = dateTimePicker1.Value.Date;
            q.Time = dateTimePicker2.Value.TimeOfDay;
            q.EconomyPrice = price;
            db.SubmitChanges();

            MessageBox.Show("Data Saved");
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
