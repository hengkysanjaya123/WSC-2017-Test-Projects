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
    public partial class AddUserForm : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public AddUserForm()
        {
            InitializeComponent();
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            var q = db.Offices.ToList();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = q;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Validation(Container))
            {
                MessageBox.Show("All data must be filled");
                return;
            }

            if (!IsValidEmail(textBox1.Text))
            {
                MessageBox.Show("Please input email with correct format");
                return;
            }

            if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please check your birthdate");
                return;
            }

            var check = db.Users.Where(x => x.Email == textBox1.Text).Count();
            if (check > 0)
            {
                MessageBox.Show("Email already exist");
                return;
            }


            int newId = 1;
            var q = db.Users.OrderByDescending(x => x.ID).FirstOrDefault();
            if (q != null)
            {
                newId = q.ID + 1;
            }

            User u = new User()
            {
                RoleID = 2,
                Email = textBox1.Text,
                Password = Hash(textBox4.Text),
                FirstName = textBox2.Text,
                LastName = textBox3.Text,
                OfficeID = int.Parse(comboBox1.SelectedValue.ToString()),
                Birthdate = dateTimePicker1.Value,
                Active = true
            };
            db.Users.InsertOnSubmit(u);
            db.SubmitChanges();

            MessageBox.Show("Data Saved");
            this.DialogResult = DialogResult.OK;

        }
    }
}
