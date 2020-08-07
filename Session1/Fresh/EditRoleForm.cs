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
    public partial class EditRoleForm : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        User u;

        public EditRoleForm(User u)
        {
            InitializeComponent();
            this.u = u;
        }

        private void EditRoleForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = u.Email;
            textBox2.Text = u.FirstName;
            textBox3.Text = u.LastName;
            comboBox1.Items.Add(u.Office.Title);
            comboBox1.SelectedIndex = 0;
            if (u.RoleID == 1)
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var q = db.Users.Where(x => x.ID == u.ID).FirstOrDefault();
                q.RoleID = radioButton1.Checked ? 2 : 1;
                db.SubmitChanges();
                MessageBox.Show("Role udpated successfully");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
