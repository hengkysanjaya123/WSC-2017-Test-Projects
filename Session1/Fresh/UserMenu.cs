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
    public partial class UserMenu : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public UserMenu()
        {
            InitializeComponent();
        }

        private void UserMenu_Load(object sender, EventArgs e)
        {
            label1.Text = $"Hi {currentUser.FirstName} {currentUser.LastName}, Welcome to AMONIC Airlines";

            LoadData();
        }

        public void LoadData()
        {
            var date = dateTimePicker1.Value.Date;

            var q = db.UserActivities.ToList().Where(x => x.UserID == currentUser.ID
                                && x.ID != currentUserActivity.ID
                                && x.Login <= date
                                && x.Login >= date.AddDays(-30)
                            ).ToList();
            var timeSpent = q.Where(x => x.Logout.HasValue).Sum(x => (x.Logout.Value - x.Login).TotalHours);
            TimeSpan time = TimeSpan.FromHours(timeSpent);
            label2.Text = $"Time spent on system : {((int)time.TotalHours).ToString("00")}:{((int)time.Minutes).ToString("00")}:{((int)time.Seconds).ToString("00")}";

            var nCrash = q.Where(x => !x.Logout.HasValue).Count();
            label3.Text = $"Number of crashes : {nCrash}";

            dataGridView1.DataSource = q.OrderByDescending(x => x.Login)
                .Select(x => new
                {
                    Date = x.Login.ToString("MM/dd/yyyy"),
                    LoginTime = x.Login.ToString(@"HH\:mm"),
                    LogoutTime = x.Logout.HasValue ? $"{x.Logout.Value.ToString(@"HH\:mm")}" : "**",
                    TimeSpentOnSystem = x.Logout.HasValue ? $"{((int)(x.Logout.Value - x.Login).TotalHours).ToString("00")}:{((int)(x.Logout.Value - x.Login).Minutes).ToString("00")}" : "**",
                    UnsuccessfulLogoutReason = x.Logout.HasValue ? $"" : $"{x.Reason}"
                }).ToList();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["LogoutTime"].Value.ToString() == "**")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var q = db.UserActivities.Where(x => x.ID == currentUserActivity.ID).FirstOrDefault();
                q.Logout = DateTime.Now;
                db.SubmitChanges();
                this.Close();
                formLogin.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
