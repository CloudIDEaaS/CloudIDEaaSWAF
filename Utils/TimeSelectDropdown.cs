using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public partial class TimeSelectDropdown : ComboBox
    {
        public TimeSpan TimeSpanUnits { get; set; }

        public TimeSelectDropdown()
        {
            this.TimeSpanUnits = new TimeSpan(0, 30, 0);

            InitializeComponent();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            var start = DateTime.Parse("12:00 AM");
            var end = DateTime.Parse("11:59 AM").AddDays(1);

            this.Items.Clear();

            base.OnHandleCreated(e);

            for (var time = start; time <= end; time += this.TimeSpanUnits)
            {
                this.Items.Add(time.ToShortTimeString());
            }
        }

        public DateTime? Value
        {
            get
            {
                if (this.SelectedItem != null)
                {
                    return DateTime.Parse((string)this.SelectedItem);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
