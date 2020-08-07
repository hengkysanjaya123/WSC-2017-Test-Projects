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
    public partial class PurchaseAmenities : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        List<Amenity> listAmenity = new List<Amenity>();
        decimal paidBefore = 0;
        Ticket currentTicket;

        public PurchaseAmenities()
        {
            InitializeComponent();
        }

        private void PurchaseAmenities_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            panel1.Controls.Clear();
            listAmenity = new List<Amenity>();
            button2.Enabled = false;
            button3.Enabled = false;
            label4.Text = "";
            label6.Text = "";
            label8.Text = "";
            label10.Text = "";
            label11.Text = "";
            label14.Text = "";
            comboBox1.DataSource = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();

            if (textBox1.Text == "")
            {
                MessageBox.Show("Please input booking reference");
                return;
            }

            var q = db.Tickets.ToList().Where(x => x.Confirmed && x.BookingReference == textBox1.Text
                                && ((x.Schedule.Date + x.Schedule.Time) - DateTime.Now).TotalHours >= 24
                            ).ToList();

            if (q.Count == 0)
            {
                MessageBox.Show("There is no data");
                return;
            }


            button2.Enabled = true;

            var q2 = q.Select(x => new
            {
                Value = x,
                Display = $"{x.Schedule.FlightNumber}, {x.Schedule.Route.Airport.IATACode}-{x.Schedule.Route.Airport1.IATACode}, {x.Schedule.Date.ToString("dd/MM/yyyy")}, {(x.Schedule.Date + x.Schedule.Time).ToString(@"HH\:mm")}"
            }).ToList();
            comboBox1.DisplayMember = "Display";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = q2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentTicket = (Ticket)comboBox1.SelectedValue;
            label4.Text = currentTicket.Firstname + " " + currentTicket.Lastname;
            label6.Text = currentTicket.CabinType.Name;
            label8.Text = currentTicket.PassportNumber;

            var booked = currentTicket.AmenitiesTickets.ToList();
            listAmenity.AddRange(booked.Select(x => x.Amenity));
            paidBefore = booked.Sum(x => x.Amenity.Price);

            var included = currentTicket.CabinType.AmenitiesCabinTypes.ToList();

            foreach (var a in db.Amenities.ToList())
            {
                CheckBox chk = new CheckBox();
                chk.AutoSize = true;
                chk.Location = new System.Drawing.Point(21, 14);
                chk.Name = "checkBox1";
                chk.Size = new System.Drawing.Size(86, 19);
                chk.TabIndex = 0;
                chk.UseVisualStyleBackColor = true;

                chk.Tag = a;
                chk.Text = $"{a.Service} ()";
            }
        }
    }
}
