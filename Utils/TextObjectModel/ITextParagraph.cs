using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.TextObjectModel
{
    public interface ITextParagraph
    {
        ITextParagraph Duplicate { set; }
        int CanChange();
        int IsEqual(ITextParagraph pPara);
        void Reset(int Value);
        Style Style { set; }
        Alignment Alignment { set; }
        int Hyphenation { set; }
        float FirstLineIndent {  get; }
        int KeepTogether { set; }
        int KeepWithNext { set; }
        float LeftIndent {  get; }
        float LineSpacing {  get; }
        int LineSpacingRule {  get; }
        int ListAlignment { set; }
        int ListLevelIndex { set; }
        int ListStart { set; }
        float ListTab { set; }
        int ListType { set; }
        int NoLineNumber { set; }
        int PageBreakBefore { set; }
        float RightIndent { set; }
        void SetIndents(float RightIndent);
        void SetLineSpacing(float LineSpacing);
        float SpaceAfter { set; }
        float SpaceBefore { set; }
        int WidowControl { set; }
        int TabCount {  get; }
        void AddTab(int tbLeader);
        void ClearAllTabs();
        void DeleteTab(float tbPos);
        void GetTab(int iTab, out float ptbPos, out int ptbAlign, out int ptbLeader);
    }
}
