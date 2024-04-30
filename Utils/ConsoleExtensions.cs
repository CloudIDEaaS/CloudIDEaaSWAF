using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Utils
{
    public class ConsoleReader : TextReader
    {
        public override void Close()
        {
            base.Close();
        }

        public override int Peek()
        {
            return base.Peek();
        }

        public override int Read()
        {
            return base.Read();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return base.ReadAsync(buffer, index, count);
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return base.ReadBlock(buffer, index, count);
        }

        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            return base.ReadBlockAsync(buffer, index, count);
        }

        public override string ReadLine()
        {
            return base.ReadLine();
        }

        public override Task<string> ReadLineAsync()
        {
            return base.ReadLineAsync();
        }

        public override string ReadToEnd()
        {
            return base.ReadToEnd();
        }

        public override Task<string> ReadToEndAsync()
        {
            return base.ReadToEndAsync();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    public class ConsoleWriter : TextWriter, ILogWriter
    {
        public override IFormatProvider FormatProvider => base.FormatProvider;

        public override Encoding Encoding => throw new NotImplementedException();

        public override string NewLine { get => base.NewLine; set => base.NewLine = value; }

        public override void Close()
        {
            base.Close();
        }

        public IDisposable ErrorMode()
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            base.Flush();
        }

        public override Task FlushAsync()
        {
            return base.FlushAsync();
        }

        public override void Write(char value)
        {
            base.Write(value);
        }

        public override void Write(char[] buffer)
        {
            base.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
        }

        public override void Write(bool value)
        {
            base.Write(value);
        }

        public override void Write(int value)
        {
            base.Write(value);
        }

        public override void Write(uint value)
        {
            base.Write(value);
        }

        public override void Write(long value)
        {
            base.Write(value);
        }

        public override void Write(ulong value)
        {
            base.Write(value);
        }

        public override void Write(float value)
        {
            base.Write(value);
        }

        public override void Write(double value)
        {
            base.Write(value);
        }

        public override void Write(decimal value)
        {
            base.Write(value);
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public override void Write(object value)
        {
            base.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            base.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            base.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            base.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            base.Write(format, arg);
        }

        public override Task WriteAsync(char value)
        {
            return base.WriteAsync(value);
        }

        public override Task WriteAsync(string value)
        {
            return base.WriteAsync(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            return base.WriteAsync(buffer, index, count);
        }

        public override void WriteLine()
        {
            base.WriteLine();
        }

        public override void WriteLine(char value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            base.WriteLine(buffer);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            base.WriteLine(buffer, index, count);
        }

        public override void WriteLine(bool value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(decimal value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            base.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            base.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            base.WriteLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            base.WriteLine(format, arg);
        }

        public override Task WriteLineAsync(char value)
        {
            return base.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(string value)
        {
            return base.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return base.WriteLineAsync(buffer, index, count);
        }

        public override Task WriteLineAsync()
        {
            return base.WriteLineAsync();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    public static class ConsoleExtensions
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        private static bool isHidden;
        private const int STD_OUTPUT_HANDLE = -11;
        private const int CODE_PAGE = 437;

        static ConsoleExtensions()
        {
            Application.ApplicationExit += (sender, e) =>
            {
                FreeConsole();
            };
        }

        public static void ShowConsole(this Form form)
        {
            ShowConsole();
        }

        public static bool AttachConsole(this Process process)
        {
            return AttachConsole(process.Id);
        }

        public static bool AttachConsole()
        {
            return AttachConsole(ATTACH_PARENT_PROCESS);
        }

        public static bool FreeConsole(this Process process)
        {
            return FreeConsole();
        }

        public static frmConsole CreateConsoleForm(this Form form, string title)
        {
            var frmConsole = new frmConsole(title);

            return frmConsole;
        }

        public static void ShowConsole()
        {
            if (isHidden)
            {
                var hwndConsole = ControlExtensions.GetConsoleWindow();

                ControlExtensions.ShowWindowAsync(hwndConsole, ControlExtensions.ShowWindowCommands.Show);

                isHidden = true;
            }
            else
            {
                if (!AllocConsole())
                {
                    DebugUtils.Break();
                }
            }
        }


        public static void Write(string value)
        {
        }

        public static void Write(string format, params object[] args)
        {
            var writer = (StreamWriter) AppDomain.CurrentDomain.GetData("SecondaryConsole");

            if (writer != null)
            {
                writer.Write(format, args);
            }
            else
            {
                Console.Write(format, args);
            }
        }

        public static void WriteLine(string value)
        {
            var writer = (StreamWriter)AppDomain.CurrentDomain.GetData("SecondaryConsole");

            if (writer != null)
            {
                writer.WriteLine(value);
                writer.Flush();
            }
            else
            {
                Console.WriteLine(value);
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            var writer = (StreamWriter)AppDomain.CurrentDomain.GetData("SecondaryConsole");

            if (writer != null)
            {
                writer.WriteLine(format, args);
            }
            else
            {
                Console.WriteLine(format, args);
            }
        }

        public static void WriteLine()
        {
            var writer = (StreamWriter)AppDomain.CurrentDomain.GetData("SecondaryConsole");

            if (writer != null)
            {
                writer.WriteLine();
            }
            else
            {
                Console.WriteLine();
            }
        }

        public static StreamWriter ShowSecondaryConsole()
        {
            StreamWriter standardOutput = null;

            if (isHidden)
            {
                var hwndConsole = ControlExtensions.GetConsoleWindow();

                ControlExtensions.ShowWindowAsync(hwndConsole, ControlExtensions.ShowWindowCommands.Show);

                isHidden = true;
            }
            else
            {
                if (!AllocConsole())
                {
                    DebugUtils.Break();
                }

                var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                var safeFileHandle = new SafeFileHandle(stdHandle, true);
                var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                var encoding = System.Text.Encoding.GetEncoding(CODE_PAGE);
                standardOutput = new StreamWriter(fileStream, encoding);

                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);

                AppDomain.CurrentDomain.SetData("SecondaryConsole", standardOutput);
            }

            ControlExtensions.ShowConsoleInSecondaryMonitor(FormWindowState.Normal);

            return standardOutput;
        }

        public static void HideConsole(this Form form)
        {
            HideConsole();
        }

        public static void HideConsole()
        {
            var hwndConsole = ControlExtensions.GetConsoleWindow();

            ControlExtensions.ShowWindowAsync(hwndConsole, ControlExtensions.ShowWindowCommands.Hide);

            isHidden = true;
        }
    }
}
