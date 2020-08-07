using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Reporting.WinForms;

namespace Fresh
{
    public partial class SummaryForm : core
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public SummaryForm()
        {
            InitializeComponent();
        }

        private void SummaryForm_Load(object sender, EventArgs e)
        {
            var fieldwork = db.Surveys.Min(x => x.SurveyDate).ToString("MMMM yyyy") + " - " + db.Surveys.Max(x => x.SurveyDate).ToString("MMMM yyyy");
            var sampleSize = db.Surveys.Count() + " Adults";

            var q = db.Surveys.ToList()
                .Select(x => new SummaryData()
                {
                    Gender = x.Gender == "F" ? "Female" : x.Gender == "M" ? "Male" : "",
                    Age = x.Age.HasValue ? GetAge(x.Age.Value) : "",
                    CabinType = x.CabinType == null ? "" : x.CabinType,
                    DestinationAirport = x.Arrival == null ? "" : x.Arrival
                }).ToList();

            this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Fieldwork", fieldwork));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter("SampleSize", sampleSize));
            SummaryDataBindingSource.DataSource = q;
            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog op = new OpenFileDialog())
            {
                op.Filter = "CSV Files|*.csv";
                if (op.ShowDialog() == DialogResult.OK)
                {

                    StreamReader reader = new StreamReader(op.FileName);
                    reader.ReadLine();
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Departure,Arrival,Age,Gender,CabinType,Q1,Q2,Q3,Q4

                        string[] data = line.Split(',');

                        string departure = data[0] == "" ? null : data[0];
                        string arrival = data[1] == "" ? null : data[1];
                        int? age = data[2] == "" ? null : (int?)int.Parse(data[2]);
                        string gender = data[3] == "" ? null : data[3];
                        string cabinType = data[4] == "" ? null : data[4];

                        int? a1 = data[5] == "0" ? null : (int?)int.Parse(data[5]);
                        int? a2 = data[6] == "0" ? null : (int?)int.Parse(data[6]);
                        int? a3 = data[7] == "0" ? null : (int?)int.Parse(data[7]);
                        int? a4 = data[8] == "0" ? null : (int?)int.Parse(data[8]);


                        Survey s = new Survey()
                        {
                            SurveyDate = new DateTime(2017, 7, 1),
                            Departure = departure,
                            Arrival = arrival,
                            CabinType = cabinType,
                            Age = age,
                            Gender = gender
                        };
                        db.Surveys.InsertOnSubmit(s);
                        db.SubmitChanges();

                        DetailSurvey ds1 = new DetailSurvey()
                        {
                            SurveyID = s.ID,
                            QuestionID = 1,
                            AnswerID = a1
                        };
                        db.DetailSurveys.InsertOnSubmit(ds1);
                        db.SubmitChanges();


                        DetailSurvey ds2 = new DetailSurvey()
                        {
                            SurveyID = s.ID,
                            QuestionID = 2,
                            AnswerID = a2
                        };
                        db.DetailSurveys.InsertOnSubmit(ds2);
                        db.SubmitChanges();


                        DetailSurvey ds3 = new DetailSurvey()
                        {
                            SurveyID = s.ID,
                            QuestionID = 3,
                            AnswerID = a3
                        };
                        db.DetailSurveys.InsertOnSubmit(ds3);
                        db.SubmitChanges();


                        DetailSurvey ds4 = new DetailSurvey()
                        {
                            SurveyID = s.ID,
                            QuestionID = 4,
                            AnswerID = a4
                        };
                        db.DetailSurveys.InsertOnSubmit(ds4);
                        db.SubmitChanges();
                    }

                    MessageBox.Show("Test");
                }
            }
        }
    }

    public class SummaryData
    {
        public string Gender { get; set; }
        public string Age { get; set; }
        public string CabinType { get; set; }
        public string DestinationAirport { get; set; }
    }
}
