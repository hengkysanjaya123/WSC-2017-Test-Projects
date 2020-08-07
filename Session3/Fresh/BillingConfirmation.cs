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
    public partial class BillingConfirmation : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        List<Schedule> header = new List<Schedule>();
        List<Schedule> detail = new List<Schedule>();
        CabinType cabinType;
        List<PassengerData> listPassenger = new List<PassengerData>();

        public BillingConfirmation(List<Schedule> header, List<Schedule> detail, CabinType cabinType, List<PassengerData> listPassenger)
        {
            InitializeComponent();
            this.header = header;
            this.detail = detail;
            this.cabinType = cabinType;
            this.listPassenger = listPassenger;
        }

        private void BillingConfirmation_Load(object sender, EventArgs e)
        {
            var q1 = GetCabinPrice(header, cabinType) * listPassenger.Count;
            var q2 = GetCabinPrice(detail, cabinType) * listPassenger.Count;
            var total = q1 + q2;
            label2.Text = Math.Floor(total).ToString("C0");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string GetBookingReference()
        {
            string data = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string result = "";
            bool unique = false;
            Random rand = new Random();

            while (!unique)
            {
                for (int i = 0; i < 6; i++)
                {
                    result += data[rand.Next(0, data.Length)];
                }

                var q = db.Tickets.Where(x => x.BookingReference == result).Count();
                if (q > 0)
                {
                    result = "";
                }
                else
                {
                    unique = true;
                }
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bookingReference = GetBookingReference();
            foreach (var s in header)
            {
                foreach (var p in listPassenger)
                {
                    Ticket t = new Ticket()
                    {
                        UserID = 1,
                        ScheduleID = s.ID,
                        CabinTypeID = cabinType.ID,
                        Firstname = p.Firstname,
                        Lastname = p.Lastname,
                        Phone = p.Phone,
                        PassportNumber = p.PassportNumber,
                        PassportCountryID = p.CountryID,
                        BookingReference = bookingReference,
                        Confirmed = true
                    };
                    db.Tickets.InsertOnSubmit(t);
                    db.SubmitChanges();
                }
            }

            foreach (var s in detail)
            {
                foreach (var p in listPassenger)
                {
                    Ticket t = new Ticket()
                    {
                        UserID = 1,
                        ScheduleID = s.ID,
                        CabinTypeID = cabinType.ID,
                        Firstname = p.Firstname,
                        Lastname = p.Lastname,
                        Phone = p.Phone,
                        PassportNumber = p.PassportNumber,
                        PassportCountryID = p.CountryID,
                        BookingReference = bookingReference,
                        Confirmed = true
                    };
                    db.Tickets.InsertOnSubmit(t);
                    db.SubmitChanges();
                }
            }

            MessageBox.Show("Data Saved");
            this.DialogResult = DialogResult.OK;
        }
    }
}
