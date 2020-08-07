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
    public partial class LoginForm : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int nFault = 0;
        int second = 10;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!Validation(Container))
            {
                MessageBox.Show("All data must be filled");
                return;
            }

            db = new DataClasses1DataContext();

            if (nFault == 3)
            {
                MessageBox.Show($"You have entered 3 times incorrect account, please wait {second} seconds");
                label3.Text = $"{second} seconds left";
                timer1.Start();
                return;
            }

            var q = db.Users.Where(x => x.Email == textBox1.Text && x.Password == Hash(textBox2.Text)).FirstOrDefault();
            if (q == null)
            {
                MessageBox.Show("Username and password incorrect");
                nFault += 1;
                return;
            }


            if (!q.Active.Value)
            {
                MessageBox.Show("Your account has been disabled");
                return;
            }

            currentUser = q;
            formLogin = this;

            UserActivity ua = new UserActivity()
            {
                UserID = currentUser.ID,
                Login = DateTime.Now
            };
            currentUserActivity = ua;
            db.UserActivities.InsertOnSubmit(ua);
            db.SubmitChanges();


            var check = db.UserActivities.Where(x => x.UserID == currentUser.ID
                                && x.ID != currentUserActivity.ID
                            ).OrderByDescending(x => x.Login).FirstOrDefault();

            if (check != null)
            {
                if (!check.Logout.HasValue)
                {
                    // No Logout Detected
                    NoLogoutDetected form = new NoLogoutDetected(check);
                    this.Hide();
                    form.Show();
                }
                else
                {
                    DoLogin(currentUser);
                }
            }
            else
            {
                DoLogin(currentUser);
            }

        }
        public void DoLogin(User u)
        {
            textBox1.Text = "";
            textBox2.Text = "";

            if (u.RoleID == 1)
            {
                AdminMenu form = new AdminMenu();
                this.Hide();
                form.Show();
            }
            else if (u.RoleID == 2)
            {
                UserMenu form = new UserMenu();
                this.Hide();
                form.Show();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            second--;
            label3.Text = $"{second} seconds left";
            if (second <= 0)
            {
                label3.Text = "";
                timer1.Stop();
                second = 10;
                nFault = 0;
            }
        }
    }
}
