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
    public partial class frmConsole : Form
    {
        private bool showInvisible;

        public Color Color { get; set; }

        public void Write(char value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(char[] buffer)
        {
            throw new NotImplementedException();
        }

        public void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Write(bool value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(int value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(uint value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(long value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(ulong value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(float value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(double value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(decimal value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(string value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(object value)
        {
            consoleControl.WriteOutput(value.ToString(), this.Color);
        }

        public void Write(string format, params object[] arg)
        {
            var output = string.Format(format, arg);

            consoleControl.WriteOutput(output, this.Color);
        }

        public void WriteLine()
        {
            consoleControl.WriteOutput(string.Empty, this.Color);
        }

        public void WriteLine(char value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(char[] buffer)
        {
            consoleControl.WriteOutput(buffer.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(bool value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(int value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(uint value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(long value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(ulong value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(float value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(double value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(decimal value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(string value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(object value)
        {
            consoleControl.WriteOutput(value.ToString() + "\r\n", this.Color);
        }

        public void WriteLine(string format, params object[] arg)
        {
            var output = string.Format(format, arg);

            consoleControl.WriteOutput(output.ToString() + "\r\n", this.Color);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            this.ShowInSecondaryMonitor();
        }

        public frmConsole(string title)
        {
            this.Color = Color.White;
            InitializeComponent();

            this.Text = title;
        }
    }
}
