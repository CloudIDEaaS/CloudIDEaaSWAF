using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.TextObjectModel;

namespace Utils.TextObjectModel
{
    public class TextLine : ITextLine
    {
        private List<string> textItems;

        public TextLine()
        {
            textItems = new List<string>();
        }

        public TextLine(string[] textItems)
        {
            this.textItems = textItems.ToList();
        }

        public ITextParagraph Duplicate
        {
            set { throw new NotImplementedException(); }
        }

        public int CanChange()
        {
            throw new NotImplementedException();
        }

        public int IsEqual(ITextParagraph pPara)
        {
            throw new NotImplementedException();
        }

        public void Reset(int Value)
        {
            throw new NotImplementedException();
        }

        public Style Style
        {
            set { throw new NotImplementedException(); }
        }

        public Alignment Alignment
        {
            set { throw new NotImplementedException(); }
        }

        public int Hyphenation
        {
            set { throw new NotImplementedException(); }
        }

        public float FirstLineIndent
        {
            get { throw new NotImplementedException(); }
        }

        public int KeepTogether
        {
            set { throw new NotImplementedException(); }
        }

        public int KeepWithNext
        {
            set { throw new NotImplementedException(); }
        }

        public float LeftIndent
        {
            get { throw new NotImplementedException(); }
        }

        public float LineSpacing
        {
            get { throw new NotImplementedException(); }
        }

        public int LineSpacingRule
        {
            get { throw new NotImplementedException(); }
        }

        public int ListAlignment
        {
            set { throw new NotImplementedException(); }
        }

        public int ListLevelIndex
        {
            set { throw new NotImplementedException(); }
        }

        public int ListStart
        {
            set { throw new NotImplementedException(); }
        }

        public float ListTab
        {
            set { throw new NotImplementedException(); }
        }

        public int ListType
        {
            set { throw new NotImplementedException(); }
        }

        public int NoLineNumber
        {
            set { throw new NotImplementedException(); }
        }

        public int PageBreakBefore
        {
            set { throw new NotImplementedException(); }
        }

        public float RightIndent
        {
            set { throw new NotImplementedException(); }
        }

        public void SetIndents(float RightIndent)
        {
            throw new NotImplementedException();
        }

        public void SetLineSpacing(float LineSpacing)
        {
            throw new NotImplementedException();
        }

        public float SpaceAfter
        {
            set { throw new NotImplementedException(); }
        }

        public float SpaceBefore
        {
            set { throw new NotImplementedException(); }
        }

        public int WidowControl
        {
            set { throw new NotImplementedException(); }
        }

        public int TabCount
        {
            get { throw new NotImplementedException(); }
        }

        public void AddTab(int tbLeader)
        {
            throw new NotImplementedException();
        }

        public void ClearAllTabs()
        {
            throw new NotImplementedException();
        }

        public void DeleteTab(float tbPos)
        {
            throw new NotImplementedException();
        }

        public void GetTab(int iTab, out float ptbPos, out int ptbAlign, out int ptbLeader)
        {
            throw new NotImplementedException();
        }

        public IList<string> TextItems
        {
            get 
            {
                return this.textItems;
            }
        }

        public override string ToString()
        {
            var result = string.Empty;

            result = this.textItems.Aggregate(result, (s1, s2) => s1 + s2);

            return result;
        }

        public static implicit operator TextLine(string[] textItems)
        {
            return new TextLine(textItems);
        }
    }
}
