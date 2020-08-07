using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fresh
{
    public partial class DestinationItem : UserControl
    {
        Schedule s;
        CabinType cabinType;

        public DestinationItem(Schedule s, CabinType cabinType)
        {
            InitializeComponent();
            this.s = s;
            this.cabinType = cabinType;
        }

        private void DestinationItem_Load(object sender, EventArgs e)
        {
            label2.Text = s.Route.Airport.IATACode;
            label4.Text = s.Route.Airport1.IATACode;
            label6.Text = cabinType.Name;
            label8.Text = s.Date.ToString("dd/MM/yyyy");
            label10.Text = s.FlightNumber;
        }
    }
}
