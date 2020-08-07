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
    public partial class Form1 : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        public void Init()
        {
            DateTime opened = DateTime.Now;
            LoadData();

            TimeSpan substract = DateTime.Now - opened;
            label29.Text = $"Report generated in {substract.TotalSeconds} seconds";
        }

        public void LoadData()
        {
            var date = dateTimePicker1.Value.Date;
            var lastMonth = date.AddDays(-30);

            var q = db.Schedules.ToList().Where(x =>
                            (x.Date + x.Time) >= lastMonth &&
                            (x.Date + x.Time) <= date
                        ).ToList();

            var confirmedSchedule = q.Where(x => x.Confirmed).ToList();

            var confirmed = confirmedSchedule.Count;
            var cancelled = q.Where(x => !x.Confirmed).Count();

            if (confirmedSchedule.Count > 0)
            {
                var average = confirmedSchedule.GroupBy(x => x.Date).Average(x => x.Sum(y => y.Route.FlightTime));
                label6.Text = Math.Floor(average) + " Minutes";
            }
            else
            {
                label6.Text = "0 Minutes";
            }

            label4.Text = confirmed.ToString();
            label5.Text = cancelled.ToString();

            var confirmedTicket = confirmedSchedule.SelectMany(x => x.Tickets).Where(x => x.Confirmed).ToList();

            // Top Customer
            var q1 = confirmedTicket.GroupBy(x => x.Firstname + " " + x.Lastname).OrderByDescending(x => x.Count()).ToList();
            label7.Text = q1.Count > 0 ? $"1. {q1[0].Key} ({q1[0].Count()}Tickets)" : "1. -";
            label8.Text = q1.Count > 1 ? $"2. {q1[1].Key} ({q1[1].Count()}Tickets)" : "2. -";
            label9.Text = q1.Count > 2 ? $"3. {q1[2].Key} ({q1[2].Count()}Tickets)" : "3. -";

            // Top Offices
            var q2 = confirmedTicket.GroupBy(x => x.User.Office).Select(x => new
            {
                Office = x.Key.Title,
                Revenue = x.ToList().GroupBy(y => y.Schedule).Select(y => new
                {
                    Revenue = y.ToList().GroupBy(z => z.CabinTypeID).Select(z => new
                    {
                        TotalSold = z.ToList().Count,
                        Price = z.Key == 1 ? Math.Floor(y.Key.EconomyPrice) :
                                z.Key == 2 ? Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice)) :
                                Math.Floor(1.3m * Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice))),
                        TotalSeat = z.Key == 1 ? y.Key.Aircraft.EconomySeats :
                                    z.Key == 2 ? y.Key.Aircraft.BusinessSeats :
                                    y.Key.Aircraft.TotalSeats -
                                    y.Key.Aircraft.EconomySeats -
                                    y.Key.Aircraft.BusinessSeats
                    }).Sum(z => z.TotalSold > z.TotalSeat ? z.TotalSeat * z.Price : z.TotalSold * z.Price)
                }).Sum(y => y.Revenue)
            }).OrderByDescending(x => x.Revenue).ToList();

            label10.Text = q2.Count > 0 ? $"1. {q2[0].Office}" : "1. -";
            label11.Text = q2.Count > 1 ? $"2. {q2[1].Office}" : "2. -";
            label12.Text = q2.Count > 2 ? $"3. {q2[2].Office}" : "3. -";


            // Number of passenger flying
            var q3 = confirmedTicket.GroupBy(x => x.Schedule.Date).OrderByDescending(x => x.Count()).ToList();
            label16.Text = q3.Count > 0 ? $"{q3.FirstOrDefault().Key.ToString("dd/MM")} with {q3.FirstOrDefault().Count()} flying" : "-";
            label15.Text = q3.Count > 1 ? $"{q3.LastOrDefault().Key.ToString("dd/MM")} with {q3.LastOrDefault().Count()} flying" : "-";


            // Revenue from ticket sales
            var yesterday = confirmedSchedule.Where(x => x.Date == date.AddDays(-1))
                            .SelectMany(x => x.Tickets)
                            .Where(x => x.Confirmed)
                            .GroupBy(x => x.Schedule)
                            .Select(y => new
                            {
                                Revenue = y.ToList().GroupBy(z => z.CabinTypeID).Select(z => new
                                {
                                    TotalSold = z.ToList().Count,
                                    Price = z.Key == 1 ? Math.Floor(y.Key.EconomyPrice) :
                                            z.Key == 2 ? Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice)) :
                                            Math.Floor(1.3m * Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice))),
                                    TotalSeat = z.Key == 1 ? y.Key.Aircraft.EconomySeats :
                                                z.Key == 2 ? y.Key.Aircraft.BusinessSeats :
                                                y.Key.Aircraft.TotalSeats -
                                                y.Key.Aircraft.EconomySeats -
                                                y.Key.Aircraft.BusinessSeats
                                }).Sum(z => z.TotalSold > z.TotalSeat ? z.TotalSeat * z.Price : z.TotalSold * z.Price)
                            }).Sum(y => y.Revenue);
            label17.Text = Math.Floor(yesterday).ToString("C0");


            var twoDays = confirmedSchedule.Where(x => x.Date == date.AddDays(-2))
                           .SelectMany(x => x.Tickets)
                           .Where(x => x.Confirmed)
                           .GroupBy(x => x.Schedule)
                           .Select(y => new
                           {
                               Revenue = y.ToList().GroupBy(z => z.CabinTypeID).Select(z => new
                               {
                                   TotalSold = z.ToList().Count,
                                   Price = z.Key == 1 ? Math.Floor(y.Key.EconomyPrice) :
                                           z.Key == 2 ? Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice)) :
                                           Math.Floor(1.3m * Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice))),
                                   TotalSeat = z.Key == 1 ? y.Key.Aircraft.EconomySeats :
                                               z.Key == 2 ? y.Key.Aircraft.BusinessSeats :
                                               y.Key.Aircraft.TotalSeats -
                                               y.Key.Aircraft.EconomySeats -
                                               y.Key.Aircraft.BusinessSeats
                               }).Sum(z => z.TotalSold > z.TotalSeat ? z.TotalSeat * z.Price : z.TotalSold * z.Price)
                           }).Sum(y => y.Revenue);
            label20.Text = Math.Floor(twoDays).ToString("C0");


            var threeDays = confirmedSchedule.Where(x => x.Date == date.AddDays(-3))
                           .SelectMany(x => x.Tickets)
                           .Where(x => x.Confirmed)
                           .GroupBy(x => x.Schedule)
                           .Select(y => new
                           {
                               Revenue = y.ToList().GroupBy(z => z.CabinTypeID).Select(z => new
                               {
                                   TotalSold = z.ToList().Count,
                                   Price = z.Key == 1 ? Math.Floor(y.Key.EconomyPrice) :
                                           z.Key == 2 ? Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice)) :
                                           Math.Floor(1.3m * Math.Floor(1.35m * Math.Floor(y.Key.EconomyPrice))),
                                   TotalSeat = z.Key == 1 ? y.Key.Aircraft.EconomySeats :
                                               z.Key == 2 ? y.Key.Aircraft.BusinessSeats :
                                               y.Key.Aircraft.TotalSeats -
                                               y.Key.Aircraft.EconomySeats -
                                               y.Key.Aircraft.BusinessSeats
                               }).Sum(z => z.TotalSold > z.TotalSeat ? z.TotalSeat * z.Price : z.TotalSold * z.Price)
                           }).Sum(y => y.Revenue);
            label22.Text = Math.Floor(threeDays).ToString("C0");


            // Weekly report of percentage of empty seats

            var thisWeekTotalSeat = confirmedSchedule.Where(x =>
                                            (x.Date + x.Time) <= date &&
                                            (x.Date + x.Time) >= date.AddDays(-7)
                                        ).Sum(x => x.Aircraft.TotalSeats);
            var thisWeek = confirmedSchedule.Where(x =>
                                (x.Date + x.Time) <= date &&
                                (x.Date + x.Time) >= date.AddDays(-7)
                            ).SelectMany(x => x.Tickets)
                            .Where(x => x.Confirmed)
                            .GroupBy(x => x.Schedule)
                            .Select(x => new
                            {
                                TakenSeat = x.ToList().GroupBy(y => y.CabinTypeID).Select(y => new
                                {
                                    TakenSeat = y.ToList().Count,
                                    TotalSeat = y.Key == 1 ? x.Key.Aircraft.EconomySeats :
                                                y.Key == 2 ? x.Key.Aircraft.BusinessSeats :
                                                x.Key.Aircraft.TotalSeats -
                                                x.Key.Aircraft.EconomySeats -
                                                x.Key.Aircraft.BusinessSeats
                                }).Sum(y => y.TakenSeat > y.TotalSeat ? y.TotalSeat : y.TakenSeat)
                            }).Sum(x => x.TakenSeat);

            var percentageThisWeek = ((thisWeekTotalSeat - thisWeek) * 100.0) / thisWeekTotalSeat;
            label25.Text = double.IsNaN(percentageThisWeek) ? "- %" : $"{Math.Round(percentageThisWeek)} %";

            var lastWeekTotalSeat = confirmedSchedule.Where(x =>
                                           (x.Date + x.Time) <= date.AddDays(-7) &&
                                           (x.Date + x.Time) >= date.AddDays(-14)
                                       ).Sum(x => x.Aircraft.TotalSeats);
            var lastWeek = confirmedSchedule.Where(x =>
                                            (x.Date + x.Time) <= date.AddDays(-7) &&
                                           (x.Date + x.Time) >= date.AddDays(-14)
                            ).SelectMany(x => x.Tickets)
                            .Where(x => x.Confirmed)
                            .GroupBy(x => x.Schedule)
                            .Select(x => new
                            {
                                TakenSeat = x.ToList().GroupBy(y => y.CabinTypeID).Select(y => new
                                {
                                    TakenSeat = y.ToList().Count,
                                    TotalSeat = y.Key == 1 ? x.Key.Aircraft.EconomySeats :
                                                y.Key == 2 ? x.Key.Aircraft.BusinessSeats :
                                                x.Key.Aircraft.TotalSeats -
                                                x.Key.Aircraft.EconomySeats -
                                                x.Key.Aircraft.BusinessSeats
                                }).Sum(y => y.TakenSeat > y.TotalSeat ? y.TotalSeat : y.TakenSeat)
                            }).Sum(x => x.TakenSeat);

            var percentageLastWeek = ((lastWeekTotalSeat - lastWeek) * 100.0) / lastWeekTotalSeat;
            label24.Text = double.IsNaN(percentageLastWeek) ? "- %" : $"{Math.Round(percentageLastWeek)} %";

            var twoWeekTotalSeat = confirmedSchedule.Where(x =>
                                           (x.Date + x.Time) <= date.AddDays(-14) &&
                                           (x.Date + x.Time) >= date.AddDays(-21)
                                       ).Sum(x => x.Aircraft.TotalSeats);
            var twoWeek = confirmedSchedule.Where(x =>
                                            (x.Date + x.Time) <= date.AddDays(-14) &&
                                           (x.Date + x.Time) >= date.AddDays(-21)
                            ).SelectMany(x => x.Tickets)
                            .Where(x => x.Confirmed)
                            .GroupBy(x => x.Schedule)
                            .Select(x => new
                            {
                                TakenSeat = x.ToList().GroupBy(y => y.CabinTypeID).Select(y => new
                                {
                                    TakenSeat = y.ToList().Count,
                                    TotalSeat = y.Key == 1 ? x.Key.Aircraft.EconomySeats :
                                                y.Key == 2 ? x.Key.Aircraft.BusinessSeats :
                                                x.Key.Aircraft.TotalSeats -
                                                x.Key.Aircraft.EconomySeats -
                                                x.Key.Aircraft.BusinessSeats
                                }).Sum(y => y.TakenSeat > y.TotalSeat ? y.TotalSeat : y.TakenSeat)
                            }).Sum(x => x.TakenSeat);

            var percentageTwoWeek = ((twoWeekTotalSeat - twoWeek) * 100.0) / twoWeekTotalSeat;
            label23.Text = double.IsNaN(percentageTwoWeek) ? "- %" : $"{Math.Round(percentageTwoWeek)} %";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
}
