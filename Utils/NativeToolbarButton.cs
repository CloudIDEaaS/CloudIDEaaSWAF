using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class NativeToolbarButton : ToolBarButton, IComparable<NativeToolbarButton>
    {
        internal ControlExtensions.TBBUTTON64 tbButton;
        internal ControlExtensions.TBBUTTONINFO tbButtonInfo;
        public int Index { get; set; }
        internal NativeToolbar toolbar;
        internal RECT rect;

        public new ToolBarButtonStyle Style 
        {
            get
            {
                var style = (ToolBarButtonStyle) 0;

                toolbar.Refresh(this);

                if (tbButton.fsStyle.HasFlag(ControlExtensions.ToolbarStyle.TBSTYLE_BUTTON))
                {
                    style = ToolBarButtonStyle.PushButton;
                } 
                else if (tbButton.fsStyle.HasFlag(ControlExtensions.ToolbarStyle.TBSTYLE_SEP))
                {
                    style = ToolBarButtonStyle.Separator;
                }
                else if (tbButton.fsStyle.HasFlag(ControlExtensions.ToolbarStyle.TBSTYLE_CHECK))
                {
                    style = ToolBarButtonStyle.ToggleButton;
                }
                else if (tbButton.fsStyle.HasFlag(ControlExtensions.ToolbarStyle.TBSTYLE_GROUP))
                {
                    style = ToolBarButtonStyle.DropDownButton;
                }

                return style;
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        
        public new Rectangle Rectangle 
        {
            get
            {
                toolbar.Refresh(this);

                return new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
            }
        }
        
        public new bool Pushed 
        {
            get
            {
                toolbar.Refresh(this);

                return tbButton.fsState.HasFlag(ControlExtensions.ToolbarState.TBSTATE_PRESSED) || tbButton.fsState.HasFlag(ControlExtensions.ToolbarState.TBSTATE_CHECKED);
            }

            set
            {
                var loHiWord = new LowHiWord { Low = 1 };

                if (ControlExtensions.SendMessage(toolbar.Handle, ControlExtensions.WindowsMessage.TB_CHECKBUTTON, 0, loHiWord) == IntPtr.Zero)
                {
                    DebugUtils.Break();
                }
            }
        }

        public int CompareTo(NativeToolbarButton other)
        {
            return other.tbButton.idCommand.CompareTo(this.tbButton.idCommand);
        }

        public override int GetHashCode()
        {
            return tbButton.idCommand;
        }
    }
}
