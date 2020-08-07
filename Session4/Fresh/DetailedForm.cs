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
    public partial class DetailedForm : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        List<string> listGender = new List<string>() { "Male", "Female" };
        List<string> listAge = new List<string>() { "18-24", "25-39", "40-59", "60+" };
        bool formReady = false;

        public DetailedForm()
        {
            InitializeComponent();
        }

        private void DetailedForm_Load(object sender, EventArgs e)
        {
            var timePeriod = db.Surveys.Select(x => x.SurveyDate).Distinct().ToList().Select(x => x.ToString("MMMM yyyy")).ToList();
            timePeriod.Insert(0, "All Periods");
            comboBox1.DataSource = timePeriod;

            var q1 = listGender.ToList();
            q1.Insert(0, "All Genders");
            comboBox2.DataSource = q1;

            var q2 = listAge.ToList();
            q2.Insert(0, "All Ages");
            comboBox3.DataSource = q2;

            formReady = true;
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            LoadData();
        }

        public void LoadData()
        {
            if (!formReady) return;

            var detail = db.DetailSurveys.ToList()
                        .Where(x => x.Survey.SurveyDate.ToString("MMMM yyyy") == comboBox1.SelectedValue.ToString()
                                    || comboBox1.SelectedValue.ToString() == "All Periods"
                            ).ToList();

            List<DetailData> listData = new List<DetailData>();

            foreach (var q in db.Questions.ToList())
            {
                foreach (var a in db.Answers.ToList())
                {
                    var detailed = detail.Where(x => x.QuestionID == q.ID && x.AnswerID == a.ID).ToList();

                    foreach (var gender in listGender)
                    {
                        listData.Add(new DetailData()
                        {
                            Question = q.Question1,
                            Answer = a.Answer1,
                            AnswerID = a.ID,

                            Header = "Gender",
                            Detail = gender,
                            Total = detailed.Where(x => x.Survey.Gender == gender[0].ToString()).Count()
                        });
                    }

                    foreach (var age in listAge)
                    {
                        listData.Add(new DetailData()
                        {
                            Question = q.Question1,
                            Answer = a.Answer1,
                            AnswerID = a.ID,

                            Header = "Age",
                            Detail = age,
                            Total = detailed.Where(x => x.Survey.Age.HasValue && age == GetAge(x.Survey.Age.Value)).Count()
                        });
                    }


                    foreach (var cabinType in db.Surveys.Where(x => x.CabinType != null).Select(x => x.CabinType).Distinct().ToList())
                    {
                        listData.Add(new DetailData()
                        {
                            Question = q.Question1,
                            Answer = a.Answer1,
                            AnswerID = a.ID,

                            Header = "Cabin Type",
                            Detail = cabinType,
                            Total = detailed.Where(x => x.Survey.CabinType == cabinType).Count()
                        });
                    }

                    foreach (var arrival in db.Surveys.Where(x => x.Arrival != null).Select(x => x.Arrival).Distinct().ToList())
                    {
                        listData.Add(new DetailData()
                        {
                            Question = q.Question1,
                            Answer = a.Answer1,
                            AnswerID = a.ID,

                            Header = "Destination Airport",
                            Detail = arrival,
                            Total = detailed.Where(x => x.Survey.Arrival == arrival).Count()
                        });
                    }
                }
            }


            if (checkBox1.Checked)
            {
                string gender = comboBox2.SelectedValue.ToString();

                if (gender != "All Genders")
                {
                    listData.RemoveAll(x => x.Header == "Gender" && x.Detail != gender);
                }
            }
            else
            {
                listData.RemoveAll(x => x.Header == "Gender");
            }

            if (checkBox2.Checked)
            {
                string age = comboBox3.SelectedValue.ToString();

                if (age != "All Ages")
                {
                    listData.RemoveAll(x => x.Header == "Age" && x.Detail != age);
                }
            }
            else
            {
                listData.RemoveAll(x => x.Header == "Age");
            }


            DetailDataBindingSource.DataSource = listData.ToList();
            this.reportViewer1.RefreshReport();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = checkBox1.Checked;
            LoadData();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = checkBox2.Checked;
            LoadData();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }

    public class DetailData
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int AnswerID { get; set; }
        public string Header { get; set; }
        public string Detail { get; set; }
        public int Total { get; set; }
    }
}
