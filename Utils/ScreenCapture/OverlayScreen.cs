using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Utils.Controls.ScreenCapture
{
    public partial class OverlayScreen : OverlayScreenBase
    {
        public enum CursPos : int
        {
            WithinSelectionArea = 0,
            OutsideSelectionArea,
            TopLine,
            BottomLine,
            LeftLine,
            RightLine,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        public enum ClickAction : int
        {
            NoClick = 0,
            Dragging,
            Outside,
            TopSizing,
            BottomSizing,
            LeftSizing,
            TopLeftSizing,
            BottomLeftSizing,
            RightSizing,
            TopRightSizing,
            BottomRightSizing
        }

        public ClickAction CurrentAction;
        public bool LeftButtonDown = false;
        public bool RectangleDrawn = false;
        public bool ReadyToDrag = false;

        public Point ClickPoint = new Point();
        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        public Point DragClickRelative = new Point();
        public Bitmap Image { get; private set; }
        public int RectangleHeight = new int();
        public int RectangleWidth = new int();
        Graphics graphics;
        Pen pen = new Pen(Color.Red, 3);
        SolidBrush TransparentBrush = new SolidBrush(Color.White);
        Pen eraserPen = new Pen(Color.FromArgb(255, 255, 192), 3);
        SolidBrush eraserBrush = new SolidBrush(Color.FromArgb(255, 255, 192));
        TipWindow tipWindow;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e = null;
            }

            base.OnMouseClick(e);
        }

        private Form m_InstanceRef = null;
        public Form InstanceRef
        {
            get
            {
                return m_InstanceRef;
            }
            set
            {
                m_InstanceRef = value;
            }
        }


        public OverlayScreen(Screen initialScreen) : base(initialScreen)
        {
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(mouse_Click);
            this.MouseDoubleClick += new MouseEventHandler(mouse_DoubleClick);
            this.MouseUp += new MouseEventHandler(mouse_Up);
            this.MouseMove += new MouseEventHandler(mouse_Move);
            this.KeyUp += new KeyEventHandler(OverlayScreen_KeyUp);
        }

        public void SaveSelection(bool showCursor)
        {
            var curPos = new Point(this.CursorPoint.X - CurrentTopLeft.X, this.CursorPoint.Y - CurrentTopLeft.Y);
            var curSize = new Size();
            var screenTopLeft = this.PointToScreen(this.CurrentTopLeft);
            var screenBottomRight = this.PointToScreen(this.CurrentBottomRight);

            curSize.Height = Cursor.Current.Size.Height;
            curSize.Width = Cursor.Current.Size.Width;

            //Allow 250 milliseconds for the screen to repaint itself (we don't want to include this form in the capture)
            System.Threading.Thread.Sleep(250);

            Point StartPoint = new Point(screenTopLeft.X, screenTopLeft.Y);
            Rectangle bounds = new Rectangle(screenTopLeft.X, screenTopLeft.Y, screenBottomRight.X - screenTopLeft.X, screenBottomRight.Y - screenTopLeft.Y);

            this.Hide();
            this.DoEvents();

            this.Image = ScreenShot.CaptureImage(showCursor, curSize, curPos, StartPoint, Point.Empty, bounds);

            this.InstanceRef.Show();
            this.Close();
        }

        public void OverlayScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "S" && (RectangleDrawn && (CursorPosition() == CursPos.WithinSelectionArea || CursorPosition() == CursPos.OutsideSelectionArea)))
            {
                SaveSelection(true);
            }
            else if (e.KeyCode.ToString() == "Escape")
            {
                this.InstanceRef.Show();
                this.Close();
            }
        }

        private void mouse_DoubleClick(object sender, MouseEventArgs e)
        {
            if (RectangleDrawn && (CursorPosition() == CursPos.WithinSelectionArea || CursorPosition() == CursPos.OutsideSelectionArea))
            {
                SaveSelection(false);
            }
        }

        private void mouse_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetClickAction();
                LeftButtonDown = true;
                ClickPoint = new Point(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);

                this.ClickPoint = this.PointToClient(this.ClickPoint);

                if (RectangleDrawn)
                {
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    DragClickRelative.X = this.CursorPoint.X - CurrentTopLeft.X;
                    DragClickRelative.Y = this.CursorPoint.Y - CurrentTopLeft.Y;
                }
            }
        }

        private void mouse_Up(object sender, MouseEventArgs e)
        {
            RectangleDrawn = true;
            LeftButtonDown = false;
            CurrentAction = ClickAction.NoClick;

            tipWindow.Tip = "Drag it to move the selection area.  It  can also be resized at its four sides and four corners.  Once you are satisfied with the selection area double click to continue.";
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            if (LeftButtonDown && !RectangleDrawn)
            {
                DrawSelection();
            }

            if (RectangleDrawn)
            {
                CursorPosition();

                if (CurrentAction == ClickAction.Dragging)
                {
                    DragSelection();
                }

                if (CurrentAction != ClickAction.Dragging && CurrentAction != ClickAction.Outside)
                {
                    ResizeSelection();
                }
            }
        }

        private CursPos CursorPosition()
        {
            if (((this.CursorPoint.X > CurrentTopLeft.X - 10 && this.CursorPoint.X < CurrentTopLeft.X + 10)) && ((this.CursorPoint.Y > CurrentTopLeft.Y + 10) && (this.CursorPoint.Y < CurrentBottomRight.Y - 10)))
            {
                this.Cursor = Cursors.SizeWE;
                return CursPos.LeftLine;
            }

            if (((this.CursorPoint.X >= CurrentTopLeft.X - 10 && this.CursorPoint.X <= CurrentTopLeft.X + 10)) && ((this.CursorPoint.Y >= CurrentTopLeft.Y - 10) && (this.CursorPoint.Y <= CurrentTopLeft.Y + 10)))
            {
                this.Cursor = Cursors.SizeNWSE;
                return CursPos.TopLeft;
            }

            if (((this.CursorPoint.X >= CurrentTopLeft.X - 10 && this.CursorPoint.X <= CurrentTopLeft.X + 10)) && ((this.CursorPoint.Y >= CurrentBottomRight.Y - 10) && (this.CursorPoint.Y <= CurrentBottomRight.Y + 10)))
            {
                this.Cursor = Cursors.SizeNESW;
                return CursPos.BottomLeft;

            }

            if (((this.CursorPoint.X > CurrentBottomRight.X - 10 && this.CursorPoint.X < CurrentBottomRight.X + 10)) && ((this.CursorPoint.Y > CurrentTopLeft.Y + 10) && (this.CursorPoint.Y < CurrentBottomRight.Y - 10)))
            {
                this.Cursor = Cursors.SizeWE;
                return CursPos.RightLine;
            }

            if (((this.CursorPoint.X >= CurrentBottomRight.X - 10 && this.CursorPoint.X <= CurrentBottomRight.X + 10)) && ((this.CursorPoint.Y >= CurrentTopLeft.Y - 10) && (this.CursorPoint.Y <= CurrentTopLeft.Y + 10)))
            {
                this.Cursor = Cursors.SizeNESW;
                return CursPos.TopRight;
            }

            if (((this.CursorPoint.X >= CurrentBottomRight.X - 10 && this.CursorPoint.X <= CurrentBottomRight.X + 10)) && ((this.CursorPoint.Y >= CurrentBottomRight.Y - 10) && (this.CursorPoint.Y <= CurrentBottomRight.Y + 10)))
            {
                this.Cursor = Cursors.SizeNWSE;
                return CursPos.BottomRight;
            }

            if (((this.CursorPoint.Y > CurrentTopLeft.Y - 10) && (this.CursorPoint.Y < CurrentTopLeft.Y + 10)) && ((this.CursorPoint.X > CurrentTopLeft.X + 10 && this.CursorPoint.X < CurrentBottomRight.X - 10)))
            {
                this.Cursor = Cursors.SizeNS;
                return CursPos.TopLine;
            }

            if (((this.CursorPoint.Y > CurrentBottomRight.Y - 10) && (this.CursorPoint.Y < CurrentBottomRight.Y + 10)) && ((this.CursorPoint.X > CurrentTopLeft.X + 10 && this.CursorPoint.X < CurrentBottomRight.X - 10)))
            {
                this.Cursor = Cursors.SizeNS;
                return CursPos.BottomLine;
            }

            if ((this.CursorPoint.X >= CurrentTopLeft.X + 10 && this.CursorPoint.X <= CurrentBottomRight.X - 10) && (this.CursorPoint.Y >= CurrentTopLeft.Y + 10 && this.CursorPoint.Y <= CurrentBottomRight.Y - 10))
            {
                this.Cursor = Cursors.Hand;
                return CursPos.WithinSelectionArea;
            }

            this.Cursor = Cursors.No;

            return CursPos.OutsideSelectionArea;
        }

        private void SetClickAction()
        {
            switch (CursorPosition())
            {
                case CursPos.BottomLine:
                    CurrentAction = ClickAction.BottomSizing;
                    break;
                case CursPos.TopLine:
                    CurrentAction = ClickAction.TopSizing;
                    break;
                case CursPos.LeftLine:
                    CurrentAction = ClickAction.LeftSizing;
                    break;
                case CursPos.TopLeft:
                    CurrentAction = ClickAction.TopLeftSizing;
                    break;
                case CursPos.BottomLeft:
                    CurrentAction = ClickAction.BottomLeftSizing;
                    break;
                case CursPos.RightLine:
                    CurrentAction = ClickAction.RightSizing;
                    break;
                case CursPos.TopRight:
                    CurrentAction = ClickAction.TopRightSizing;
                    break;
                case CursPos.BottomRight:
                    CurrentAction = ClickAction.BottomRightSizing;
                    break;
                case CursPos.WithinSelectionArea:
                    CurrentAction = ClickAction.Dragging;
                    break;
                case CursPos.OutsideSelectionArea:
                    CurrentAction = ClickAction.Outside;
                    break;
            }
        }

        private void ResizeSelection()
        {
            if (CurrentAction == ClickAction.LeftSizing)
            {
                if (this.CursorPoint.X < CurrentBottomRight.X - 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = this.CursorPoint.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.TopLeftSizing)
            {
                if (this.CursorPoint.X < CurrentBottomRight.X - 10 && this.CursorPoint.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = this.CursorPoint.X;
                    CurrentTopLeft.Y = this.CursorPoint.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.BottomLeftSizing)
            {
                if (this.CursorPoint.X < CurrentBottomRight.X - 10 && this.CursorPoint.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = this.CursorPoint.X;
                    CurrentBottomRight.Y = this.CursorPoint.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.RightSizing)
            {
                if (this.CursorPoint.X > CurrentTopLeft.X + 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = this.CursorPoint.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.TopRightSizing)
            {
                if (this.CursorPoint.X > CurrentTopLeft.X + 10 && this.CursorPoint.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = this.CursorPoint.X;
                    CurrentTopLeft.Y = this.CursorPoint.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.BottomRightSizing)
            {
                if (this.CursorPoint.X > CurrentTopLeft.X + 10 && this.CursorPoint.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = this.CursorPoint.X;
                    CurrentBottomRight.Y = this.CursorPoint.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.TopSizing)
            {
                if (this.CursorPoint.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.Y = this.CursorPoint.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }

            if (CurrentAction == ClickAction.BottomSizing)
            {
                if (this.CursorPoint.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.Y = this.CursorPoint.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                }
            }
        }

        private void DragSelection()
        {
            //Ensure that the rectangle stays within the bounds of the screen

            //Erase the previous rectangle
            graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

            if (this.CursorPoint.X - DragClickRelative.X > 0 && this.CursorPoint.X - DragClickRelative.X + RectangleWidth < screen.Bounds.Width)
            {
                CurrentTopLeft.X = this.CursorPoint.X - DragClickRelative.X;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
            }
            else if (this.CursorPoint.X - DragClickRelative.X > 0)
            {
                CurrentTopLeft.X = screen.Bounds.Width - RectangleWidth;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
            }
            //Selection area has reached the left side of the screen
            else
            {
                CurrentTopLeft.X = 0;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
            }

            if (this.CursorPoint.Y - DragClickRelative.Y > 0 && this.CursorPoint.Y - DragClickRelative.Y + RectangleHeight < screen.Bounds.Height)
            {
                CurrentTopLeft.Y = this.CursorPoint.Y - DragClickRelative.Y;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
            }
            else if (this.CursorPoint.Y - DragClickRelative.Y > 0)
            {
                CurrentTopLeft.Y = screen.Bounds.Height - RectangleHeight;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
            }
            else
            {
                CurrentTopLeft.Y = 0;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
            }

            //Draw a new rectangle
            graphics.DrawRectangle(pen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
        }

        private Point CursorPoint
        {
            get
            {
                return this.PointToClient(Cursor.Position);
            }
        }

        private void DrawSelection()
        {
            this.Cursor = Cursors.Arrow;
            Rectangle rectangle;

            //Erase the previous rectangle
            graphics.DrawRectangle(eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

            //Calculate X Coordinates

            if (this.CursorPoint.X < ClickPoint.X)
            {
                CurrentTopLeft.X = this.CursorPoint.X;
                CurrentBottomRight.X = ClickPoint.X;
            }
            else
            {
                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = this.CursorPoint.X;
            }

            //Calculate Y Coordinates
            // 
            if (this.CursorPoint.Y < ClickPoint.Y)
            {
                CurrentTopLeft.Y = this.CursorPoint.Y;
                CurrentBottomRight.Y = ClickPoint.Y;
            }
            else
            {
                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = this.CursorPoint.Y;
            }

            rectangle = new Rectangle(CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

            Debug.WriteLine(rectangle);

            //Draw a new rectangle
            graphics.DrawRectangle(pen, rectangle);
        }

        private void OverlayScreen_Load(object sender, EventArgs e)
        {
            this.CenterOver(screen);
            this.WindowState = FormWindowState.Maximized;

            this.DoEvents();

            graphics = this.CreateGraphics();

            tipWindow = new TipWindow(this);

            this.DelayInvoke(300, () => tipWindow.Show());
        }

        private void OverlayScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            tipWindow.Close();
        }
    }
}