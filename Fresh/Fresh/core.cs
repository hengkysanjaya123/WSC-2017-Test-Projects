using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace Fresh
{
    public partial class core : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();

        public core()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                pfc.AddFontFile(Application.StartupPath + @"\texgyreadventor-regular.otf");
                panel1.Font = new Font(pfc.Families[0], 9f);
            }
            catch
            {
            }
            panel1.BackColor = ColorTranslator.FromHtml("#196aa6");
            panel1.ForeColor = Color.White;

            SetStyle(this);
        }

        public void SetStyle(Control ctrl)
        {
            foreach (Control kontrol in ctrl.Controls)
            {
                if (kontrol is Button)
                {
                    var btn = (Button)kontrol;
                    btn.FlatStyle = FlatStyle.Flat;

                    string text = btn.Text.ToLower();
                    if (text.Contains("delete") || text.Contains("cancel"))
                    {
                        btn.BackColor = Color.Red;
                    }
                    else
                    {
                        btn.BackColor = ColorTranslator.FromHtml("#f79420");
                    }
                }
                else if (kontrol is ComboBox)
                {
                    var combo = (ComboBox)kontrol;
                    combo.DropDownStyle = ComboBoxStyle.DropDownList;
                }

                if (kontrol.HasChildren)
                {
                    SetStyle(kontrol);
                }
            }
        }

        public bool Validation(Control ctrl)
        {
            var q = ctrl.Controls.OfType<TextBox>().Where(x => x.Text == "").Count();
            if (q > 0)
            {
                return false;
            }
            return true;
        }

        public string Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] byt = md5.Hash;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < byt.Length; i++)
            {
                sb.Append(byt[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public bool IsValidEmail(string text)
        {
            bool result = false;

            try
            {
                result = new EmailAddressAttribute().IsValid(text);
            }
            catch
            {
                return false;
            }

            if (result)
            {
                foreach (var a in text)
                {
                    if (!char.IsLetter(a) && !char.IsDigit(a) && a != '@' && a != '_' && a != '.')
                    {
                        return false;
                    }
                }
            }

            return result;
        }
    }
}
