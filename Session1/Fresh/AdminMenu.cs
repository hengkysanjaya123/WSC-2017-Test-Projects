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
    public partial class AdminMenu : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        bool formReady = false;

        public AdminMenu()
        {
            InitializeComponent();
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            var q = db.Offices.ToList();
            q.Insert(0, new Office()
            {
                ID = 0,
                Title = "All Offices"
            });
            comboBox1.DisplayMember = "Title";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = q;

            formReady = true;
            LoadData();
        }

        public void LoadData()
        {
            if (!formReady) return;

            db = new DataClasses1DataContext();
            db.Connection.Close();
            db.Connection.Open();

            var cmd = db.Connection.CreateCommand();
            cmd.CommandText = $@"select u.id,u.FirstName 'Name', u.LastName , DATEDIFF(year, u.birthdate, getdate()) 'Age',r.Title 'User Role',u.Email 'Email Addresss', o.Title 'Office' From users U	
	                            join roles r
		                            on u.roleid = r.id
	                            join Offices o	
		                            on u.OfficeID = o.ID
                                where u.OfficeID = '{comboBox1.SelectedValue.ToString()}' or '{comboBox1.SelectedValue.ToString()}' = '0'
                            ";
            var dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["id"].Visible = false;

            db.Connection.Close();

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var q = db.Users.Where(x => x.ID.ToString() == dataGridView1.Rows[i].Cells["id"].Value.ToString()).FirstOrDefault();

                if (!q.Active.Value)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else if (q.RoleID == 1)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var q = db.Users.Where(x => x.ID.ToString() == dataGridView1.CurrentRow.Cells["id"].Value.ToString()).FirstOrDefault();
                button2.Text = q.Active.Value ? "Suspend Account" : "Unsuspend Account";
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Please choose the data in the datagridview first");
                    return;
                }

                var q = db.Users.Where(x => x.ID.ToString() == dataGridView1.CurrentRow.Cells["id"].Value.ToString()).FirstOrDefault();
                if (q.ID == currentUser.ID)
                {
                    MessageBox.Show("You cannot Suspend your own account");
                    return;
                }

                q.Active = !q.Active;
                db.SubmitChanges();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please choose the data in the datagridview first");
                return;
            }

            var q = db.Users.Where(x => x.ID.ToString() == dataGridView1.CurrentRow.Cells["id"].Value.ToString()).FirstOrDefault();
            if (q.ID == currentUser.ID)
            {
                MessageBox.Show("You cannot change your own role");
                return;
            }

            // Open Edit Role Form
            EditRoleForm form = new EditRoleForm(q);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUserForm form = new AddUserForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
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
