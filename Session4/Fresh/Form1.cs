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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void viewResultsSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SummaryForm form = new SummaryForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Show();
            panel2.Controls.Clear();
            panel2.Controls.Add(form);
        }

        private void viewDetailedResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetailedForm form = new DetailedForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Show();
            panel2.Controls.Clear();
            panel2.Controls.Add(form);
        }
    }
}
