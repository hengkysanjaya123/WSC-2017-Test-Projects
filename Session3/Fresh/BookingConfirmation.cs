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
    public partial class BookingConfirmation : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        List<Schedule> header = new List<Schedule>();
        List<Schedule> detail = new List<Schedule>();
        CabinType cabinType;
        int nPassenger;


        List<PassengerData> listPassenger = new List<PassengerData>();

        public BookingConfirmation(List<Schedule> header, List<Schedule> detail, CabinType cabinType, int nPassenger)
        {
            InitializeComponent();
            this.header = header;
            this.detail = detail;
            this.cabinType = cabinType;
            this.nPassenger = nPassenger;
        }

        private void BookingConfirmation_Load(object sender, EventArgs e)
        {
            foreach (var a in header)
            {
                DestinationItem item = new DestinationItem(a, cabinType);
                flowLayoutPanel1.Controls.Add(item);
                flowLayoutPanel1.SetFlowBreak(item, true);
            }

            if (detail.Count == 0)
            {
                groupBox2.Visible = false;
            }
            else
            {
                foreach (var a in detail)
                {
                    DestinationItem item = new DestinationItem(a, cabinType);
                    flowLayoutPanel2.Controls.Add(item);
                    flowLayoutPanel2.SetFlowBreak(item, true);
                }
            }

            var q = db.Countries.ToList();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = q;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public bool IsAlphanumeric(string text)
        {
            foreach (var a in text)
            {
                if (!char.IsLetter(a) && !char.IsDigit(a))
                {
                    return false;
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listPassenger.Count == nPassenger)
            {
                MessageBox.Show("You have entered all passenger data");
                return;
            }

            if (!Validation(groupBox3))
            {
                MessageBox.Show("All data must be filled");
                return;
            }

            if (!IsAlphanumeric(textBox3.Text))
            {
                MessageBox.Show("Passport Number should be alphanumeric");
                return;
            }

            if (textBox3.Text.Length < 6)
            {
                MessageBox.Show("Passport Number should be 6-9 alphanumeric");
                return;
            }

            if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please check your birthdate");
                return;
            }

            string phone = maskedTextBox1.Text.Replace(" ", "");
            if (phone.Length < 6)
            {
                MessageBox.Show("Phone number should be 6-13 digit");
                return;
            }

            phone = "+" + phone;

            var q = listPassenger.Where(x => x.PassportNumber == textBox3.Text).Count();
            if (q > 0)
            {
                MessageBox.Show("Passport number already exist");
                return;
            }
            listPassenger.Add(new PassengerData()
            {
                Firstname = textBox1.Text,
                Lastname = textBox2.Text,
                Birthdate = dateTimePicker1.Value.Date,
                PassportNumber = textBox3.Text,
                PassportCountry = comboBox1.Text,
                CountryID = int.Parse(comboBox1.SelectedValue.ToString()),
                Phone = phone
            });
            LoadData();
        }
        public void LoadData()
        {
            dataGridView1.DataSource = listPassenger.ToList();
            dataGridView1.Columns["CountryID"].Visible = false;
            dataGridView1.Columns["Birthdate"].DefaultCellStyle.Format = "dd-MM-yyyy";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                listPassenger.RemoveAt(dataGridView1.CurrentRow.Index);
                LoadData();
            }
        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listPassenger.Count != nPassenger)
            {
                MessageBox.Show($"Please enter {nPassenger - listPassenger.Count} more passenger data");
                return;
            }

            // Billing Confirmation
            BillingConfirmation form = new BillingConfirmation(header, detail, cabinType, listPassenger);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
    public class PassengerData
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public string PassportNumber { get; set; }
        public string PassportCountry { get; set; }
        public int CountryID { get; set; }
        public string Phone { get; set; }
    }
}
