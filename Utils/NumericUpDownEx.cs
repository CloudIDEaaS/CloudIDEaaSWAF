using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public enum NumericUpDownDirection
    {
        None,
        Up,
        Down,
        TextChange,
    }

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public class ValueChangedEventArgs : EventArgs
    {
        public NumericUpDownDirection Direction { get; }
        public bool SystemSet { get; }

        public ValueChangedEventArgs(NumericUpDownDirection direction, bool systemSet)
        {
            this.Direction = direction;
            this.SystemSet = systemSet;
        }
    }

    public class NumericUpDownEx : NumericUpDown
    {
        public NumericUpDownDirection LastDirection { get; set; }
        public new event ValueChangedEventHandler ValueChanged;
        public TextBox TextBox { get; private set; }
        private bool systemSet;

        public NumericUpDownEx()
        {
            base.ValueChanged += NumericUpDownEx_ValueChanged;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.TextBox = this.GetAllControls().OfType<TextBox>().Single();
        }

        public new decimal Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                systemSet = true;
                base.Value = value;
            }
        }

        private void NumericUpDownEx_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged(this, new ValueChangedEventArgs(this.LastDirection, systemSet));
            systemSet = false;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.LastDirection = NumericUpDownDirection.TextChange;
            base.OnTextChanged(e);
        }

        public override void UpButton()
        {
            this.LastDirection = NumericUpDownDirection.Up;
            base.UpButton();
        }

        public override void DownButton()
        {
            this.LastDirection = NumericUpDownDirection.Down;
            base.DownButton();
        }
    }
}
