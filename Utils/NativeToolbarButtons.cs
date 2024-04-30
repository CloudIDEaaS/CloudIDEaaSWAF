using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class NativeToolbarButtons : BaseDictionary<IntPtr, NativeToolbarButton>, IEnumerable<NativeToolbarButton>
    {
        private NativeToolbar toolbar;

        public NativeToolbarButtons(NativeToolbar nativeToolbar)
        {
            this.toolbar = nativeToolbar;
        }

        public override int Count
        {
            get
            {
                return (int)ControlExtensions.SendMessage(toolbar.Handle, ControlExtensions.WindowsMessage.TB_BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        internal void Refresh(NativeToolbarButton button)
        {
            if (button.tbButton.fsStyle != ControlExtensions.ToolbarStyle.TBSTYLE_SEP)
            {
                var refreshed = this[(IntPtr)button.tbButton.idCommand];

                button.tbButton = refreshed.tbButton;
                button.tbButtonInfo = refreshed.tbButtonInfo;
                button.Index = refreshed.Index;
            }
        }

        public override void Add(IntPtr key, NativeToolbarButton value)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(IntPtr key)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<KeyValuePair<IntPtr, NativeToolbarButton>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool Remove(IntPtr key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetValue(IntPtr key, out NativeToolbarButton value)
        {
            value = this.Cast<NativeToolbarButton>().SingleOrDefault(b => b.tbButton.idCommand == (int) key);

            return value != null;
        }

        protected override void SetValue(IntPtr key, NativeToolbarButton value)
        {
            throw new NotImplementedException();
        }

        IEnumerator<NativeToolbarButton> IEnumerable<NativeToolbarButton>.GetEnumerator()
        {
            for (var x = 0; x < this.Count; x++)
            {
                var buttonText = new StringBuilder(FileUtilities.MaxPath);
                var buttonInfoTip = new StringBuilder(FileUtilities.MaxPath);
                var tbButton = new ControlExtensions.TBBUTTON64();
                var tbButtonInfo = new ControlExtensions.TBBUTTONINFO();
                var infoTip = new ControlExtensions.NMTBGETINFOTIP();
                var ptr = Marshal.AllocCoTaskMem(FileUtilities.MaxPath);
                var rect = new RECT();

                tbButtonInfo.dwMask = ControlExtensions.ToolbarButtonInfoFlags.TBIF_COMMAND | ControlExtensions.ToolbarButtonInfoFlags.TBIF_STATE | ControlExtensions.ToolbarButtonInfoFlags.TBIF_STYLE | ControlExtensions.ToolbarButtonInfoFlags.TBIF_TEXT;
                tbButtonInfo.cbSize = Marshal.SizeOf<ControlExtensions.TBBUTTONINFO>();
                tbButtonInfo.cchText = FileUtilities.MaxPath;
                tbButtonInfo.pszText = ptr;

                IOExtensions.ZeroMemory(ptr, FileUtilities.MaxPath);

                if (ControlExtensions.SendMessage<ControlExtensions.TBBUTTON64>(toolbar.Handle, ControlExtensions.WindowsMessage.TB_GETBUTTON, x, ref tbButton) == IntPtr.Zero)
                {
                    DebugUtils.Break();
                }

                if (ControlExtensions.SendMessage<ControlExtensions.TBBUTTONINFO>(toolbar.Handle, ControlExtensions.WindowsMessage.TB_GETBUTTONINFOA, tbButton.idCommand, ref tbButtonInfo) == new IntPtr(-1))
                {
                    DebugUtils.Break();
                }

                for (var y = 0; y < FileUtilities.MaxPath; y++)
                {
                    var ch = (char)Marshal.ReadByte(ptr, y);

                    if (ch == 0)
                    {
                        break;
                    }

                    buttonText.Append(ch);
                }

                IOExtensions.ZeroMemory(ptr, FileUtilities.MaxPath);

                infoTip.hwndFrom = toolbar.Handle;
                infoTip.idFrom = tbButton.idCommand;
                infoTip.code = (int)ControlExtensions.ToolbarNotification.TBN_GETINFOTIPA;
                infoTip.iItem = tbButton.idCommand;
                infoTip.pszText = ptr;
                infoTip.cchTextMax = FileUtilities.MaxPath;

                ControlExtensions.SendMessage<ControlExtensions.NMTBGETINFOTIP>(toolbar.Handle, ControlExtensions.WindowsMessage.NOTIFY, 0, ref infoTip);

                for (var y = 0; y < FileUtilities.MaxPath; y++)
                {
                    var ch = (char)Marshal.ReadByte(ptr, y);

                    if (ch == 0)
                    {
                        break;
                    }

                    buttonInfoTip.Append(ch);
                }

                Marshal.FreeCoTaskMem(ptr);

                ControlExtensions.SendMessage<RECT>(toolbar.Handle, ControlExtensions.WindowsMessage.TB_GETITEMRECT, x, ref rect);

                var button = new NativeToolbarButton
                {
                    tbButton = tbButton,
                    tbButtonInfo = tbButtonInfo,
                    Index = x,
                    toolbar = toolbar,
                    rect = rect
                };

                yield return button;
            }
        }
    }
}
