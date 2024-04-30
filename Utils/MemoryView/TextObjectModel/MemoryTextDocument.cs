using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.MemoryView.TextObjectModel;
using Utils.MemoryView.TextObjectModel;
using System.ComponentModel;
using Utils;
using System.IO;
using System.Windows;
using Utils.TextObjectModel;

namespace Utils.MemoryView.TextObjectModel
{
    public abstract class MemoryTextDocument : IMemoryTextDocument
    {
        protected StoryRanges storyRanges;
        protected BaseList<ITextLine> lines;
        private TextSelection selection;
        public event EventHandlerT<ITextSelection> SelectionChanged;
        public event EventHandler<GetRectEventArgs> OnGetRect;
        public event EventHandler<GetTextEventArgs> OnGetText;
        public abstract void Read(Stream stream, int lineWidth);

        public MemoryTextDocument()
        {
            this.storyRanges = new StoryRanges();
            this.lines = new BaseList<ITextLine>();
            this.selection = new TextSelection(this);
            this.selection.SelectionChanged += new EventHandlerT<ITextSelection>(selection_SelectionChanged);
            this.selection.OnGetRect += new EventHandler<GetRectEventArgs>(selection_OnGetRect);
            this.selection.OnGetText += new EventHandler<GetTextEventArgs>(selection_OnGetText);

            storyRanges.PropertyChanged += new PropertyChangedEventHandler(StoryRangesChanged);
            lines.PropertyChanged += new PropertyChangedEventHandler(LinesChanged);
        }

        public void ApplyRange(ITextRange range)
        {
            range.OnGetRect += new EventHandler<GetRectEventArgs>(selection_OnGetRect);
            range.OnGetText += new EventHandler<GetTextEventArgs>(selection_OnGetText);
        }

        private void selection_OnGetText(object sender, GetTextEventArgs e)
        {
            OnGetText(this, e);
        }

        private void selection_OnGetRect(object sender, GetRectEventArgs e)
        {
            OnGetRect(this, e);
        }

        private void selection_SelectionChanged(object sender, EventArgs<ITextSelection> e)
        {
            SelectionChanged(this, e);
        }

        private void LinesChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void StoryRangesChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public ITextSelection Selection
        {
            get 
            {
                return selection;
            }

            internal set
            {
                var newSelection = (TextSelection) value.Duplicate;

                selection.Clear();

                foreach (var invoker in selection.SelectionChangedInvocationList)
                {
                    selection.SelectionChanged -= invoker;
                    newSelection.SelectionChanged += invoker;
                }

                foreach (var invoker in selection.OnGetRectInvocationList)
                {
                    selection.OnGetRect -= invoker;
                    newSelection.OnGetRect += invoker;
                }

                foreach (var invoker in selection.OnGetTextInvocationList)
                {
                    selection.OnGetText -= invoker;
                    newSelection.OnGetText += invoker;
                }

                selection = newSelection;
            }
        }

        public int StoryCount
        {
            get { throw new NotImplementedException(); }
        }

        public ITextStoryRanges StoryRanges
        {
            get
            {
                return storyRanges;
            }
        }

        public bool Saved
        {
            set { throw new NotImplementedException(); }
        }

        public float DefaultTabStop
        {
            set { throw new NotImplementedException(); }
        }

        public void New()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public int Freeze()
        {
            throw new NotImplementedException();
        }

        public int Unfreeze()
        {
            throw new NotImplementedException();
        }

        public void BeginEditCollection()
        {
            throw new NotImplementedException();
        }

        public void EndEditCollection()
        {
            throw new NotImplementedException();
        }

        public int Undo(int count)
        {
            throw new NotImplementedException();
        }

        public int Redo(int count)
        {
            throw new NotImplementedException();
        }

        public ITextRange Range(int start, int end)
        {
            throw new NotImplementedException();
        }

        public ITextRange RangeFromPoint(int x, int y)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public IList<ITextLine> Lines
        {
            get
            {
                return lines;
            }
        }
    }
}
