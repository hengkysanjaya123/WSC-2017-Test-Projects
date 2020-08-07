using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fresh
{
    public partial class ApplyScheduleChanges : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public ApplyScheduleChanges()
        {
            InitializeComponent();
        }

        private void ApplyScheduleChanges_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog op = new OpenFileDialog())
            {
                op.Filter = "CSV Files|*.csv";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = op.FileName;

                    StreamReader reader = null;

                    try
                    {
                        reader = new StreamReader(op.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    int success = 0;
                    int duplicate = 0;
                    int missing = 0;

                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        //ADD,2017-10-04,06:25,75,AUH,RUH,1,983.00,OK
                        string[] data = line.Split(',');

                        if (data.Length != 9)
                        {
                            missing += 1;
                            continue;
                        }

                        string operation = data[0];
                        if (operation != "ADD" && operation != "EDIT")
                        {
                            missing += 1;
                            continue;
                        }

                        string[] dateFormat = { "yyyy-MM-d", "MM/dd/yyyy" };
                        DateTime departureDate = new DateTime();
                        if (!DateTime.TryParseExact(data[1], dateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out departureDate))
                        {
                            missing += 1;
                            continue;
                        }

                        TimeSpan departureTime = new TimeSpan();
                        if (!TimeSpan.TryParse(data[2], out departureTime))
                        {
                            missing += 1;
                            continue;
                        }

                        string flightNumber = data[3];
                        if (flightNumber == "" || string.IsNullOrEmpty(flightNumber) || flightNumber.Length > 10)
                        {
                            missing += 1;
                            continue;
                        }

                        string IATACodeDeparture = data[4];
                        var q1 = db.Airports.Where(x => x.IATACode == IATACodeDeparture).FirstOrDefault();
                        if (q1 == null)
                        {
                            missing += 1;
                            continue;
                        }

                        string IATACodeArrival = data[5];
                        var q2 = db.Airports.Where(x => x.IATACode == IATACodeArrival).FirstOrDefault();
                        if (q2 == null)
                        {
                            missing += 1;
                            continue;
                        }

                        var route = db.Routes.Where(x => x.DepartureAirportID == q1.ID && x.ArrivalAirportID == q2.ID).FirstOrDefault();
                        if (route == null)
                        {
                            missing += 1;
                            continue;
                        }

                        string aircraftCode = data[6];
                        var aircraft = db.Aircrafts.Where(x => x.ID.ToString() == aircraftCode).FirstOrDefault();
                        if (aircraft == null)
                        {
                            missing += 1;
                            continue;
                        }

                        decimal price = 0;
                        if (!decimal.TryParse(data[7], out price))
                        {
                            missing += 1;
                            continue;
                        }

                        if (price <= 0 || price > 922000000000000)
                        {
                            missing += 1;
                            continue;
                        }

                        string confirmation = data[8];
                        if (confirmation != "OK" && confirmation != "CANCELED" && confirmation != "CANCELLED")
                        {
                            missing += 1;
                            continue;
                        }


                        var check = db.Schedules.ToList().Where(x => x.FlightNumber == flightNumber && x.Date == departureDate).FirstOrDefault();
                        if (operation == "ADD")
                        {
                            if (check != null)
                            {
                                duplicate += 1;
                                continue;
                            }

                            try
                            {
                                Schedule s = new Schedule()
                                {
                                    Date = departureDate,
                                    Time = departureTime,
                                    AircraftID = aircraft.ID,
                                    RouteID = route.ID,
                                    EconomyPrice = price,
                                    Confirmed = true,
                                    FlightNumber = flightNumber
                                };

                                if (!CheckOverlap(s, departureDate + departureTime, departureDate + departureTime + TimeSpan.FromMinutes(route.FlightTime)))
                                {
                                    missing += 1;
                                    continue;
                                }


                                db.Schedules.InsertOnSubmit(s);
                                db.SubmitChanges();
                                success += 1;
                            }
                            catch
                            {
                                missing += 1;
                                continue;
                            }
                        }
                        else if (operation == "EDIT")
                        {
                            if (check == null)
                            {
                                missing += 1;
                                continue;
                            }


                            try
                            {
                                if (!CheckOverlap(check, departureDate + departureTime, departureDate + departureTime + TimeSpan.FromMinutes(route.FlightTime)))
                                {
                                    missing += 1;
                                    continue;
                                }

                                check.Date = departureDate;
                                check.Time = departureTime;
                                check.AircraftID = aircraft.ID;
                                check.RouteID = route.ID;
                                check.EconomyPrice = price;
                                check.Confirmed = true;
                                check.FlightNumber = flightNumber;

                                db.SubmitChanges();
                                success += 1;
                            }
                            catch (Exception ex)
                            {
                                missing += 1;
                                continue;
                            }
                        }
                    }

                    label3.Text = success.ToString();
                    label5.Text = duplicate.ToString();
                    label7.Text = missing.ToString();
                }
            }
        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ApplyScheduleChanges_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
