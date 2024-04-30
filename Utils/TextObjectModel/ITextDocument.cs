using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Utils.TextObjectModel
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("8CC497C0-A1DF-11ce-8098-00AA0047BE5D")]
    public interface IOleTextDocument
    {
        // IDispath methods (We never use them)
        int GetIDsOfNames(Guid riid, IntPtr rgszNames, uint cNames, uint lcid, ref int rgDispId);
        int GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);
        int GetTypeInfoCount(ref uint pctinfo);
        int Invoke(uint dispIdMember, Guid riid, uint lcid, uint wFlags, IntPtr pDispParams, IntPtr pvarResult, IntPtr pExcepInfo, ref uint puArgErr);

        // ITextDocument methods
        int GetName( /* [retval][out] BSTR* */ [In, Out, MarshalAs(UnmanagedType.BStr)] ref string pName);
        int GetSelection( /* [retval][out] ITextSelection** */ IntPtr ppSel);
        int GetStoryCount( /* [retval][out] */ ref int pCount);
        int GetStoryRanges( /* [retval][out] ITextStoryRanges** */ IntPtr ppStories);
        int GetSaved( /* [retval][out] */ ref int pValue);
        int SetSaved( /* [in] */ int Value);
        int GetDefaultTabStop( /* [retval][out] */ ref float pValue);
        int SetDefaultTabStop( /* [in] */ float Value);
        int New();
        int Open( /* [in] VARIANT **/ IntPtr pVar, /* [in] */ int Flags, /* [in] */ int CodePage);
        int Save( /* [in] VARIANT * */ IntPtr pVar, /* [in] */ int Flags, /* [in] */ int CodePage);
        int Freeze( /* [retval][out] */ ref int pCount);
        int Unfreeze( /* [retval][out] */ ref int pCount);
        int BeginEditCollection();
        int EndEditCollection();
        int Undo( /* [in] */ int Count, /* [retval][out] */ ref int prop);
        int Redo( /* [in] */ int Count, /* [retval][out] */ ref int prop);
        int Range( /* [in] */ int cp1, /* [in] */ int cp2, /* [retval][out] ITextRange** */ IntPtr ppRange);
        int RangeFromPoint( /* [in] */ int x, /* [in] */ int y, /* [retval][out] ITextRange** */ IntPtr ppRange);
    }

    public interface IOleTextStoryRanges //{8CC497C5-A1DF-11CE-8098-00AA0047BE5D}
    {
        ITextRange Item(Int32 Index);
        System.Collections.IEnumerator GetEnumerator();
        Int32 Count { get; }
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("8CC497C2-A1DF-11CE-8098-00AA0047BE5D")]

    public interface IOleTextRange //{8CC497C2-A1DF-11CE-8098-00AA0047BE5D}
    {
        string Text { get; set; }
        Int32 Char { get; set; }
        ITextRange Dublicate { get; }
        ITextRange FormattedText { get; set; }
        Int32 Start { get; set; }
        Int32 End { get; set; }
        object Font { get; set; }
        object Para { get; set; }
        Int32 StoryLength { get; }
        Int32 StoryType { get; }
        void Collapse(Int32 bStart);
        Int32 Expand(Int32 Unit);
        Int32 GetIndex(Int32 Unit);
        void SetIndex(Int32 Unit, Int32 Index, Int32 Extend);
        void SetRange(Int32 cpActive, Int32 cpOther);
        Int32 InRange(ITextRange pRange);
        Int32 InStory(ITextRange pRange);
        Int32 IsEqual(ITextRange pRange);
        void Select();
        Int32 StartOf(Int32 Unit, Int32 Extend);
        Int32 EndOf(Int32 Unit, Int32 Extend);
        Int32 Move(Int32 Unit, Int32 Count);
        Int32 MoveStart(Int32 Unit, Int32 Count);
        Int32 MoveEnd(Int32 Unit, Int32 Count);
        Int32 MoveWhile([MarshalAs(UnmanagedType.AsAny)] Int32 Cset, Int32 Count);
        Int32 MoveStartWhile([MarshalAs(UnmanagedType.AsAny)] Int32 Cset, Int32 Count);
        Int32 MoveEndWhile([MarshalAs(UnmanagedType.AsAny)] Int32 Cset, Int32 Count);
        Int32 MoveUntil([MarshalAs(UnmanagedType.AsAny)] Int32 Cset, Int32 Count);
        Int32 MoveStartUntil([MarshalAs(UnmanagedType.AsAny)] Int32 Cset, Int32 Count);
        Int32 MoveEndUntil([MarshalAs(UnmanagedType.AsAny)] Int32 Cset, Int32 Count);
        Int32 FindText(string bstr, Int32 cch, Int32 Flags);
        Int32 FindTextStart(string bstr, Int32 cch, Int32 Flags);
        Int32 FindTextEnd(string bstr, Int32 cch, Int32 Flags);
        Int32 Delete(Int32 Unit, Int32 Count);
        void Cut([MarshalAs(UnmanagedType.AsAny)] Int32 pVar);
        void Copy([MarshalAs(UnmanagedType.AsAny)] Int32 pVar);
        void Paste([MarshalAs(UnmanagedType.AsAny)] Int32 pVar);
        Int32 CanPaste([MarshalAs(UnmanagedType.AsAny)] Int32 pVar, Int32 Format);
        Int32 CanEdit();
        void ChangeCase(Int32 Type);
        void GetPoint(Int32 Type, Int32 px, Int32 py);
        void SetPoint(Int32 x, Int32 px, Int32 y, Int32 Type, Int32 Extend);
        void ScrollIntoView(Int32 Value);
        int GetEmbeddedObject(IntPtr ppObj);
    }
    
    public class TomConstants
    {
        public const int tomSuspend = -9999995;
        public const int tomResume = -9999994;
    }

    public interface ITextDocument
    {
        string Name { get; }
        ITextSelection Selection { get; }
        int StoryCount { get; }
        ITextStoryRanges StoryRanges { get; }
        bool Saved { set; }
        float DefaultTabStop { set; }
        void New();
        void Open();
        void Save();
        int Freeze();
        int Unfreeze();
        void BeginEditCollection();
        void EndEditCollection();
        int Undo(int count);
        int Redo(int count);
        ITextRange Range(int start, int end);
        ITextRange RangeFromPoint(int x, int y);
        void ApplyRange(ITextRange range);
    }
}
