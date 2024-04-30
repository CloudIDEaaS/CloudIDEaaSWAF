// enums.h

#pragma once

#include "Stdafx.h"
#include <TOM.h>

using namespace System;

namespace NAMESPACE_DECL {

	/// <summary>
	/// Mutually exclusive options for opening/saving a document in the Text Object Model.
	/// </summary>
	public enum class TextOpenSave {
		/// <summary>
		/// Gives read/write access and read/write sharing, open always, and 
		/// automatic recognition of the file format (unrecognized file formats 
		/// are treated as text).
		/// </summary>
		Default = 0,
		/// <summary>Create a new file. Fail if the file already exists.</summary>
		CreateNew = tomCreateNew,
		/// <summary>Create a new file. Destroy the existing file if it exists.</summary>
		CreateAlways = tomCreateAlways,
		/// <summary>Open an existing file. Fail if the file does not exist.</summary>
		OpenExisting = tomOpenExisting,
		/// <summary>Open an existing file. Create a new file if the file does not exist.</summary>
		OpenAlways = tomOpenAlways,
		/// <summary>Open an existing file, but truncate it to zero length.</summary>
		TruncateExisting = tomTruncateExisting,
		/// <summary>Open as RTF.</summary>
		RTF = tomRTF,
		/// <summary>Open as text ANSI or Unicode.</summary>
		Text = tomText,
		/// <summary>Open as HTML.</summary>
		HTML = tomHTML,
		/// <summary>Open as Word document.</summary>
		WordDocument = tomWordDocument
	};

	/// <summary>
	/// Additional options for opening a document in the Text Object Model.
	/// </summary>
	[Flags]
	public enum class TextOpenFlags {
		/// <summary>No additional options.</summary>
		None = 0,
		/// <summary>Read only.</summary>
		ReadOnly = tomReadOnly,
		/// <summary>Other programs cannot read.</summary>
		ShareDenyRead = tomShareDenyRead,
		/// <summary>Other programs cannot write.</summary>
		ShareDenyWrite = tomShareDenyWrite,
		/// <summary>Replace the selection with a file.</summary>
		PasteFile = tomPasteFile
	};

	/// <summary>
	/// Additional options for saving a document in the Text Object Model.
	/// </summary>
	[Flags]
	public enum class TextSaveFlags {
		/// <summary>No additional options.</summary>
		None = 0,
		/// <summary>Other programs cannot read.</summary>
		ShareDenyRead = tomShareDenyRead,
		/// <summary>Other programs cannot write.</summary>
		ShareDenyWrite = tomShareDenyWrite
	};

	/// <summary>
	/// Case changes supported by the Text Object Model.
	/// </summary>
	public enum class TextCasing {
		/// <summary>Sets all text to lowercase.</summary>
		LowerCase = tomLowerCase,
		/// <summary>Sets all text to uppercase. </summary>
		UpperCase = tomUpperCase,
		/// <summary>Capitalizes the first letter of each word.</summary>
		TitleCase = tomTitleCase,
		/// <summary>Capitalizes the first letter of each sentence.</summary>
		SentenceCase = tomSentenceCase,
		/// <summary>Toggles the case of each letter.</summary>
		ToggleCase = tomToggleCase
	};

	/// <summary>
	/// The endpoints of a range in the Text Object Model.
	/// </summary>
	public enum class RangePosition {
		/// <summary>Start of the range.</summary>
		Start = tomStart,
		/// <summary>End of the range.</summary>
		End = tomEnd
	};

	/// <summary>
	/// Types of units within the Text Object Model.
	/// </summary>
	public enum class TextUnit {
		/// <summary>Character.</summary>
		Character = tomCharacter,
		/// <summary>Word.</summary>
		Word = tomWord,
		/// <summary>Sentence.</summary>
		Sentence = tomSentence,
		/// <summary>Paragraph.</summary>
		Paragraph = tomParagraph,
		/// <summary>Line (on display).</summary>
		Line = tomLine,
		/// <summary>Story.</summary>
		Story = tomStory,
		/// <summary>Screen (as for PAGE UP/PAGE DOWN).</summary>
		Screen = tomScreen,
		/// <summary>Section.</summary>
		Section = tomSection,
		/// <summary>Table column.</summary>
		Column = tomColumn,
		/// <summary>Table row.</summary>
		Row = tomRow,
		/// <summary>Upper-left or lower-right corner of the window.</summary>
		Window = tomWindow,
		/// <summary>Table cell.</summary>
		Cell = tomCell,
		/// <summary>Run of constant character formatting.</summary>
		CharFormat = tomCharFormat,
		/// <summary>Run of constant paragraph formatting.</summary>
		ParaFormat = tomParaFormat,
		/// <summary>Table.</summary>
		Table = tomTable,
		/// <summary>Embedded object.</summary>
		Object = tomObject
	};

	/// <summary>
	/// How the start or end of a range can be shifted.
	/// </summary>
	public enum class RangeShiftType {
		/// <summary>Collapses a nondegenerate range by moving the insertion point.</summary>
		Move = tomMove,
		/// <summary>Moves to the end of the overlapping unit.</summary>
		Extend = tomExtend
	};

	/// <summary>
	/// Flags governing comparisons in a text range.
	/// </summary>
	[Flags]
	public enum class RangeMatchType {
		/// <summary>Default.</summary>
		None = 0,
		/// <summary>Matches whole words.</summary>
		MatchWord = tomMatchWord,
		/// <summary>Matches case.</summary>
		MatchCase = tomMatchCase,
		/// <summary>Matches regular expressions.</summary>
		MatchPattern = tomMatchPattern
	};

	/// <summary>
	/// Flags indicating how to retrieve the position for a range.
	/// </summary>
	[Flags]
	public enum class RangePointFlags {
		/// <summary>Default.</summary>
		None = 0,
		/// <summary>Allow points outside of the client area.</summary>
		AllowOffClient = tomAllowOffClient,
		/// <summary>Use client coordinates instead of screen coordinates.</summary>
		ClientCoord = tomClientCoord,
		/// <summary>Get a point inside an inline object argument; for example, inside the numerator of a fraction.</summary>
		ObjectArg = tomObjectArg,
		/// <summary>Transform coordinates using a world transform (XFORM) supplied by the host application.</summary>
		Transform = tomTransform
	};

	/// <summary>
	/// Flags indicating the attributes of a selection.
	/// </summary>
	[Flags]
	public enum class TextSelectionFlags {
		/// <summary>None.</summary>
		None = 0,
		/// <summary>Start end is active.</summary>
		SelStartActive = tomSelStartActive,
		/// <summary>
		/// For degenerate selections, the ambiguous character position 
		/// corresponding to both the beginning of a line and the end 
		/// of the preceding line should have the caret displayed at 
		/// the end of the preceding line.
		/// </summary>
		SelAtEOL = tomSelAtEOL,
		/// <summary>Insert/Overtype mode is set to overtype. </summary>
		SelOvertype = tomSelOvertype,
		/// <summary>Selection is active.</summary>
		SelActive = tomSelActive,
		/// <summary>Typing and pasting replaces selection.</summary>
		SelReplace = tomSelReplace
	};

	/// <summary>
	/// The type of a selection.
	/// </summary>
	public enum class TextSelectionType {
		/// <summary>No selection and no insertion point.</summary>
		None = tomNoSelection,
		/// <summary>Insertion point.</summary>
		InsertionPoint = tomSelectionIP,
		/// <summary>Single nondegenerate range.</summary>
		Normal = tomSelectionNormal,
		/// <summary>Frame.</summary>
		Frame = tomSelectionFrame,
		/// <summary>Table column.</summary>
		Column = tomSelectionColumn,
		/// <summary>Table rows.</summary>
		Row = tomSelectionRow,
		/// <summary>Block selection.</summary>
		Block = tomSelectionBlock,
		/// <summary>Picture.</summary>
		InlineShape = tomSelectionInlineShape,
		/// <summary>Shape.</summary>
		Shape = tomSelectionShape
	};

	/// <summary>
	/// Types of underlining supported by the Text Object Model.
	/// </summary>
	public enum class TextUnderlineStyle {
		/// <summary>No underlining.</summary>
		None = tomNone,
		/// <summary>Single underline.</summary>
		Single = tomSingle,
		/// <summary>Underline words only.</summary>
		Words = tomWords,
		/// <summary>Double underline.</summary>
		Double = tomDouble,
		/// <summary>Dotted underline.</summary>
		Dotted = tomDotted,
		/// <summary>Dash underline.</summary>
		Dash = tomDash,
		/// <summary>Dash dot underline.</summary>
		DashDot = tomDashDot,
		/// <summary>Dash dot dot underline.</summary>
		DashDotDot = tomDashDotDot,
		/// <summary>Wave underline.</summary>
		Wave = tomWave,
		/// <summary>Thick underline.</summary>
		Thick = tomThick,
		/// <summary>Hair underline.</summary>
		Hair = tomHair
	};

	/// <summary>
	/// The underline colors supported by the Text Object Model.
	/// </summary>
	public enum class TextUnderlineColor {
		/// <summary>Black/automatic.</summary>
		Black = 0x0,
		/// <summary>Blue.</summary>
		Blue = 0x1,
		/// <summary>Cyan.</summary>
		Cyan = 0x2,
		/// <summary>Lime green.</summary>
		LimeGreen = 0x3,
		/// <summary>Magenta.</summary>
		Magenta = 0x4,
		/// <summary>Red.</summary>
		Red = 0x5,
		/// <summary>Yellow.</summary>
		Yellow = 0x6,
		/// <summary>White.</summary>
		White = 0x7,
		/// <summary>Dark blue.</summary>
		DarkBlue = 0x8,
		/// <summary>Dark cyan.</summary>
		DarkCyan = 0x9,
		/// <summary>Green.</summary>
		Green = 0xA,
		/// <summary>Dark magenta.</summary>
		DarkMagenta = 0xB,
		/// <summary>Brown.</summary>
		Brown = 0xC,
		/// <summary>Olive green.</summary>
		OliveGreen = 0xD,
		/// <summary>Dark gray.</summary>
		DarkGray = 0xE,
		/// <summary>Gray.</summary>
		Gray = 0xF
	};

	/// <summary>
	/// Font weights supported by the Text Object Model.
	/// </summary>
	public enum class FontWeight {
		/// <summary>Not set / don't care.</summary>
		NotSet = FW_DONTCARE,
		/// <summary>Thin (100).</summary>
		Thin = FW_THIN,
		/// <summary>Extra light (200).</summary>
		ExtraLight = FW_EXTRALIGHT,
		/// <summary>Light (300).</summary>
		Light = FW_LIGHT,
		/// <summary>Normal (400).</summary>
		Normal = FW_NORMAL,
		/// <summary>Medium (500).</summary>
		Medium = FW_MEDIUM,
		/// <summary>Semibold (600).</summary>
		SemiBold = FW_SEMIBOLD,
		/// <summary>Bold (700).</summary>
		Bold = FW_BOLD,
		/// <summary>Extra bold (800).</summary>
		ExtraBold = FW_EXTRABOLD,
		/// <summary>Heavy (900).</summary>
		Heavy = FW_HEAVY,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// How text is aligned in a paragraph or cell.
	/// </summary>
	public enum class TextAlignment {
		/// <summary>Text aligns with the left margin.</summary>
		Left = tomAlignLeft,
		/// <summary>Text is centered between the margins.</summary>
		Center = tomAlignCenter,
		/// <summary>Text aligns with the right margin.</summary>
		Right = tomAlignRight,
		/// <summary>
		/// Text starts at the left margin and, if the line extends 
		/// beyond the right margin, all the spaces in the line are 
		/// adjusted to be even.
		/// </summary>
		Justify = tomAlignJustify,
		/// <summary>Text alignment is undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Built-in list paragraph styles supported by the Text Object Model.
	/// </summary>
	public enum class ListType {
		/// <summary>Not a list paragraph.</summary>
		None = tomListNone,
		/// <summary>List uses bullets (0x2022).</summary>
		Bullet = tomListBullet,
		/// <summary>List is numbered with Arabic numerals (0, 1, 2, ...).</summary>
		AsArabic = tomListNumberAsArabic,
		/// <summary>List is ordered with lowercase letters (a, b, c, ...).</summary>
		AsLCLetter = tomListNumberAsLCLetter,
		/// <summary>List is ordered with uppercase Arabic letters (A, B, C, ...).</summary>
		AsUCLetter= tomListNumberAsUCLetter,
		/// <summary>List is ordered with lowercase Roman letters (i, ii, iii, ...).</summary>
		AsLCRoman = tomListNumberAsLCRoman,
		/// <summary>List is ordered with uppercase Roman letters (I, II, III, ...).</summary>
		AsUCRoman = tomListNumberAsUCRoman,
		/// <summary>The value returned by ListStart is treated as the first code in a Unicode sequence.</summary>
		AsSequence = tomListNumberAsSequence,
		/// <summary>List is ordered with Unicode circled numbers.</summary>
		Circle = tomListNumberedCircle,
		/// <summary>List is ordered with Wingdings black circled digits.</summary>
		BlackCircleWingding = tomListNumberedBlackCircleWingding,
		/// <summary>List is ordered with Wingdings white circled digits.</summary>
		WhiteCircleWingding = tomListNumberedWhiteCircleWingding,
		/// <summary>Full-width ASCII (0, 1, 2, 3, …).</summary>
		ArabicWide = tomListNumberedArabicWide,
		/// <summary>Simplified Chinese.</summary>
		ChS = tomListNumberedChS,
		/// <summary>Traditional Chinese</summary>
		ChT = tomListNumberedChT,
		/// <summary>Japanese.</summary>
		JpnChS = tomListNumberedJpnChS,
		/// <summary>Korean.</summary>
		JpnKor = tomListNumberedJpnKor,
		/// <summary>Arabic alphabetic.</summary>
		Arabic1 = tomListNumberedArabic1,
		/// <summary>Arabic abjadi.</summary>
		Arabic2 = tomListNumberedArabic2,
		/// <summary>Hebrew alphabet.</summary>
		Hebrew = tomListNumberedHebrew,
		/// <summary>Thai alphabetic.</summary>
		ThaiAlpha = tomListNumberedThaiAlpha,
		/// <summary>Thai numbers.</summary>
		ThaiNum = tomListNumberedThaiNum,
		/// <summary>Hindi vowels.</summary>
		HindiAlpha = tomListNumberedHindiAlpha,
		/// <summary>Hindi consonants.</summary>
		HindiAlpha1 = tomListNumberedHindiAlpha1,
		/// <summary>Hindi numbers.</summary>
		HindiNum = tomListNumberedHindiNum,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Other types of formatting for a numbered list.
	/// </summary>
	public enum class ListNumberingFormat {
		/// <summary>Uses the default format for list numbering.</summary>
		Normal = 0,
		/// <summary>Encloses the number in parentheses, as in: (1).</summary>
		Parentheses = tomListParentheses,
		/// <summary>Follows the number with a period.</summary>
		Period = tomListPeriod,
		/// <summary>Uses the number alone.</summary>
		Plain = tomListPlain,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Relative tab positions.
	/// </summary>
	public enum class TabRelativePosition {
		/// <summary>Gets the tab previous to the specified position.</summary>
		Back = tomTabBack,
		/// <summary>Gets the tab following the specified position.</summary>
		Next = tomTabNext,
		/// <summary>Gets the tab at the specified position</summary>
		Here = tomTabHere
	};

	/// <summary>
	/// Options for appending text to a range.
	/// </summary>
	public enum class RangeAppendMode {
		/// <summary>Collapse the range to its new end.</summary>
		Collapse,
		/// <summary>Expand the range to include the new text.</summary>
		Expand,
		/// <summary>Preserve the original character positions of the range.</summary>
		Preserve
	};

	/// <summary>
	/// Options for inserting text into a range.
	/// </summary>
	public enum class RangeInsertMode {
		/// <summary>Expand the range to include the inserted text.</summary>
		Expand,
		/// <summary>Move the start and end positions of the range to those of the inserted text.</summary>
		Intersect
	};

#ifdef _TOM2

	/// <summary>
	/// Caret types.
	/// </summary>
	public enum class CaretType {
		/// <summary>No caret.</summary>
		Null = tomNullCaret,
		/// <summary>Normal caret.</summary>
		Normal = tomNormalCaret,
		/// <summary>Korean block caret.</summary>
		KoreanBlock = tomKoreanBlockCaret
	};

	/// <summary>
	/// Flags indicating how to retrieve the client rectangle for a document.
	/// </summary>
	[Flags]
	public enum class ClientRectangleFlags {
		/// <summary>Default.</summary>
		None = 0,
		/// <summary>Use client coordinates instead of screen coordinates.</summary>
		ClientCoord = tomClientCoord,
		/// <summary>Transform coordinates using a world transform (XFORM) supplied by the host application.</summary>
		Transform = tomTransform,
		/// <summary>Add left and top insets to the left and top coordinates of the client rectangle, and subtract right and bottom insets from the right and bottom coordinates.</summary>
		IncludeInset = tomIncludeInset
	};

	/// <summary>
	/// East Asian flags.
	/// </summary>
	[Flags]
	public enum class EastAsianFlags {
		/// <summary>No flags.</summary>
		None = 0,
		/// <summary>Use Microsoft Rich Edit 1.0 mode.</summary>
		RE10Mode = tomRE10Mode,
		/// <summary>
		/// Use a font with a name that starts with @, for CJK vertical text. 
		/// When rendered vertically, the characters in such a font are rotated 
		/// 90 degrees so that they look upright instead of sideways.
		/// </summary>
		UseAtFont = tomUseAtFont,
		/// <summary>Ordinary left-to-right horizontal text.</summary>
		TextFlowES = tomTextFlowES,
		/// <summary>Ordinary East Asian vertical text.</summary>
		TextFlowSW = tomTextFlowSW,
		/// <summary>Alternative orientation.</summary>
		TextFlowWN = tomTextFlowWN,
		/// <summary>Alternative orientation.</summary>
		TextFlowNE = tomTextFlowNE,
		/// <summary>Disables the IME operation.</summary>
		NoIME = tomNoIME,
		/// <summary>Directs the rich edit control to allow the application to handle all IME operations.</summary>
		SelfIME = tomSelfIME
	};

	/// <summary>
	/// Display-mode alignment for math.
	/// </summary>
	public enum class MathDispAlign {
		/// <summary>Default alignment.</summary>
		Default = 0,
		/// <summary>Center (default) alignment.</summary>
		Center = tomMathDispAlignCenter,
		/// <summary>Left alignment.</summary>
		Left = tomMathDispAlignLeft,
		/// <summary>Right alignment.</summary>
		Right = tomMathDispAlignRight
	};

	/// <summary>
	/// Empty arguments display style for math.
	/// </summary>
	public enum class MathDocEmptyArg {
		/// <summary>Automatically use a dotted square to denote empty arguments, if necessary.</summary>
		Auto = tomMathDocEmptyArgAuto,
		/// <summary>Always use a dotted square to denote empty arguments.</summary>
		Always = tomMathDocEmptyArgAlways,
		/// <summary>Don't denote empty arguments.</summary>
		Never = tomMathDocEmptyArgNever
	};

	/// <summary>
	/// Style for math differentials.
	/// </summary>
	public enum class MathDocDiff {
		/// <summary>Default style.</summary>
		Default = 0,
		/// <summary>Use italic (default) for math differentials.</summary>
		Italic = tomMathDocDiffItalic,
		/// <summary>Use an upright font for math differentials.</summary>
		Upright = tomMathDocDiffUpright,
		/// <summary>Use open italic (default) for math differentials.</summary>
		OpenItalic = tomMathDocDiffOpenItalic
	};

	/// <summary>
	/// Math equation line break type.
	/// </summary>
	public enum class MathBrkBin {
		/// <summary>Break before binary/relational operator.</summary>
		MathBrkBinBefore = tomMathBrkBinBefore,
		/// <summary>Break after binary/relational operator.</summary>
		MathBrkBinAfter = tomMathBrkBinAfter,
		/// <summary>Duplicate binary/relational operator before/after.</summary>
		MathBrkBinDup = tomMathBrkBinDup
	};

	/// <summary>
	/// Duplicate mask for minus operator.
	/// </summary>
	public enum class MathBrkBinSub {
		/// <summary>- - (minus on both lines).</summary>
		MinusMinus = tomMathBrkBinSubMM,
		/// <summary>+ -</summary>
		PlusMinus = tomMathBrkBinSubPM,
		/// <summary>- +</summary>
		MinusPlus = tomMathBrkBinSubMP
	};

	/// <summary>
	/// Character repertoire indices.
	/// </summary>
	public enum class CharRepertoire {
		Aboriginal = tomAboriginal,
		/// <summary>Latin 1</summary>
		Ansi = tomAnsi,
		Arabic = tomArabic,
		Armenian = tomArmenian,
		/// <summary>From Latin 1 and 2</summary>
		Baltic = tomBaltic,
		Bengali = tomBengali,
		/// <summary>Traditional Chinese</summary>
		BIG5 = tomBIG5,
		Braille = tomBraille,
		Cherokee = tomCherokee,
		Cyrillic = tomCyrillic,
		/// <summary>Default character repertoire</summary>		
		Default = tomDefaultCharRep,
		Devanagari = tomDevanagari,
		/// <summary>From Latin 1 and 2</summary>
		EastEurope = tomEastEurope,
		Emoji = tomEmoji,
		Ethiopic = tomEthiopic,
		/// <summary>Simplified Chinese</summary>
		GB2312 = tomGB2312,
		Georgian = tomGeorgian,
		Greek = tomGreek,
		Gujarati = tomGujarati,
		Gurmukhi = tomGurmukhi,
		Hangul = tomHangul,
		Hebrew = tomHebrew,
		Jamo = tomJamo,
		Kannada = tomKannada,
		Kayahli = tomKayahli,
		Kharoshthi = tomKharoshthi,
		Khmer = tomKhmer,
		Lao = tomLao,
		Limbu = tomLimbu,
		Mac = tomMac,
		Malayalam = tomMalayalam,
		Mongolian = tomMongolian,
		Myanmar = tomMyanmar,
		NewTaiLue = tomNewTaiLue,
		OEM = tomOEM,
		Ogham = tomOgham,
		Oriya = tomOriya,
		/// <summary>PC437 character set (DOS)</summary>
		PC437 = tomPC437,
		Runic = tomRunic,
		/// <summary>Japanese</summary>
		ShiftJIS = tomShiftJIS,
		Sinhala = tomSinhala,
		SylotiNagri = tomSylotiNagri,
		/// <summary>Symbol character set (not Unicode)</summary>
		Symbol = tomSymbol,
		Syriac = tomSyriac,
		TaiLe = tomTaiLe,
		Tamil = tomTamil,
		Telugu = tomTelugu,
		Thaana = tomThaana,
		Thai = tomThai,
		Tibetan = tomTibetan,
		/// <summary>Turkish (Latin 1 + dotless i, ...)</summary>
		Turkish = tomTurkish,
		/// <summary>Unicode symbol</summary>
		Usymbol = tomUsymbol,
		/// <summary>Latin 1 with some combining marks</summary>
		Vietnamese = tomVietnamese,
		Yi = tomYi
	};

	/// <summary>
	/// Preferred font options.
	/// </summary>
	[Flags]
	public enum class PreferredFontFlags {
		/// <summary>No options.</summary>
		None = 0,
		/// <summary>Ignore the font that is active at a particular character position.</summary>
		IgnoreCurrentFont = tomIgnoreCurrentFont,
		/// <summary>Match the current character repertoire.</summary>
		MatchCharRep = tomMatchCharRep,
		/// <summary>Match the current font signature.</summary>
		MatchFontSignature = tomMatchFontSignature,
		/// <summary>Use the current font if its character repertoire is <see cref="CharRepertoire::Ansi"/>.</summary>
		MatchAscii = tomMatchAscii,
		/// <summary>Gets the height.</summary>
		GetHeightOnly = tomGetHeightOnly,
		/// <summary>Match a math font.</summary>
		MatchMathFont = tomMatchMathFont,
		/// <summary>Use twips for floating-point measurements.</summary>
		UseTwips = tomUseTwips<<16
	};

	/// <summary>
	/// Active states for stories in the Text Object Model.
	/// </summary>
	public enum class StoryActiveState {
		/// <summary>Story has display, but no UI.</summary>
		Display = tomStoryActiveDisplay,
		/// <summary>Story has display and UI activity.</summary>
		DisplayUI = tomStoryActiveDisplayUI,
		/// <summary>Story is inactive.</summary>
		Inactive = tomStoryInactive,
		/// <summary>Story is UI active; that is, it receives keyboard and mouse input.</summary>
		UI = tomStoryActiveUI
	};

	/// <summary>
	/// Flags affecting the conversion from rich text to plain text.
	/// </summary>
	[Flags]
	public enum class PlainTextFlags {
		/// <summary>Default conversion flags.</summary>
		Default = 0,
		/// <summary>Adjust CR/LFs at the start.</summary>
		AdjustCRLF = tomAdjustCRLF,
		/// <summary>Allow a final end-of-paragraph (EOP) marker.</summary>
		AllowFinalEOP = tomAllowFinalEOP,
		/// <summary>Fold math alphanumerics to ASCII/Greek.</summary>
		FoldMathAlpha = tomFoldMathAlpha,
		/// <summary>Include list numbers.</summary>
		IncludeNumbering = tomIncludeNumbering,
		/// <summary>Don't include hidden text.</summary>
		NoHidden = tomNoHidden,
		/// <summary>Don't include math zone brackets.</summary>
		NoMathZoneBrackets = tomNoMathZoneBrackets,
		/// <summary>Get the BCP-47 language tag for this range.</summary>
		LanguageTag = tomLanguageTag,
		/// <summary>Copy up to 0xFFFC (OLE object).</summary>
		Textize = tomTextize,
		/// <summary>Replace table row delimiter characters with spaces.</summary>
		TranslateTableCell = tomTranslateTableCell,
		/// <summary>Use CR/LF in place of a carriage return or a line feed.</summary>
		UseCRLF = tomUseCRLF
	};

	/// <summary>
	/// Flags affecting the conversion from plain text to rich text.
	/// </summary>
	[Flags]
	public enum class RichTextFlags {
		/// <summary>Default conversion flags.</summary>
		Default = 0,
		/// <summary>Obey the current text limit instead of increasing the text to fit.</summary>
		CheckTextLimit = tomCheckTextLimit,
		/// <summary>Check math-zone character formatting.</summary>
		MathCFCheck = tomMathCFCheck,
		/// <summary>Don't insert as hidden text.</summary>
		Unhide = tomUnhide,
		/// <summary>Use the Unicode bidirectional (bidi) algorithm.</summary>
		UnicodeBiDi = tomUnicodeBiDi,
		/// <summary>Don't include text as part of a hyperlink.</summary>
		Unlink = tomUnlink
	};

	/// <summary>
	/// Story types in the Text Object Model.
	/// </summary>
	public enum class StoryType {
		/// <summary>The story used for comments.</summary>
		Comments = tomCommentsStory,
		/// <summary>The story used for endnotes.</summary>
		Endnotes = tomEndnotesStory,
		/// <summary>The story containing footers for even pages.</summary>
		EvenPagesFooter = tomEvenPagesFooterStory,
		/// <summary>The story containing headers for even pages.</summary>
		EvenPagesHeader = tomEvenPagesHeaderStory,
		/// <summary>The story used for a Find dialog.</summary>
		Find = tomFindStory,
		/// <summary>The story containing the footer for the first page.</summary>
		FirstPageFooter = tomFirstPageFooterStory,
		/// <summary>The story containing the header for the first page.</summary>
		FirstPageHeader = tomFirstPageHeaderStory,
		/// <summary>The story used for footnotes.</summary>
		Footnotes = tomFootnotesStory,
		/// <summary>The main story always exists for a rich edit control.</summary>
		MainText = tomMainTextStory,
		/// <summary>The story containing footers for odd pages.</summary>
		PrimaryFooter = tomPrimaryFooterStory,
		/// <summary>The story containing headers for odd pages.</summary>
		PrimaryHeader = tomPrimaryHeaderStory,
		/// <summary>The story used for a Replace dialog.</summary>
		Replace = tomReplaceStory,
		/// <summary>The scratch story.</summary>
		Scratch = tomScratchStory,
		/// <summary>The story used for a text box.</summary>
		TextFrame = tomTextFrameStory,
		/// <summary>No special type.</summary>
		Unknown = tomUnknownStory
	};

	/// <summary>
	/// Options for building math.
	/// </summary>
	[Flags]
	public enum class MathBuildFlags {
		/// <summary>No options.</summary>
		None = 0,
		/// <summary>Internal math autocorrect standard math \ keywords.</summary>
		MathAutoCorrect = NAMESPACE::tomMathAutoCorrect, 
		/// <summary>Enables TeX syntax for build up/down operations.</summary>
		TeX = NAMESPACE::tomTeX, 
		/// <summary>Use math alphabetics for English/Greek letters except for math function names.</summary>
		MathAlphabetics = NAMESPACE::tomMathAlphabetics
	};

	/// <summary>
	/// Options for linearizing math.
	/// </summary>
	[Flags]
	public enum class MathLinearizeFlags {
		/// <summary>No options.</summary>
		None = 0,
		/// <summary>Enables TeX syntax for build up/down operations.</summary>
		TeX = NAMESPACE::tomTeX, 
		/// <summary>Use math alphabetics for English/Greek letters except for math function names.</summary>
		MathAlphabetics = NAMESPACE::tomMathAlphabetics,
		/// <summary>Build down only outermost objects.</summary>
		MathBuildDownOutermost = NAMESPACE::tomMathBuildDownOutermost,
		/// <summary>Build up insertion points argument or zone.</summary>
		MathBuildUpArgOrZone = NAMESPACE::tomMathBuildUpArgOrZone,
		/// <summary>Build down the outermost object without its characters.</summary>
		MathRemoveOutermost = NAMESPACE::tomMathRemoveOutermost,
		/// <summary>Build down for plain text.</summary>
		Plain = NAMESPACE::tomPlain
	};

	/// <summary>
	/// Ellipsis modes.
	/// </summary>
	public enum class TextEllipsisMode {
		/// <summary>Ellipsis is disabled.</summary>
		None = tomEllipsisNone,
		/// <summary>An ellipsis forces a break anywhere in the line.</summary>
		End = tomEllipsisEnd,
		/// <summary>An ellipsis forces a break between words.</summary>
		Word = tomEllipsisWord
	};

	/// <summary>
	/// Ellipsis states.
	/// </summary>
	public enum class TextEllipsisState {
		/// <summary>Ellipsis is absent.</summary>
		Absent = tomEllipsisNone,
		/// <summary>Ellipsis is present.</summary>
		Present = tomEllipsisPresent
	};

	/// <summary>
	/// Options for converting linear-format math to a built-up form.
	/// </summary>
	[Flags]
	public enum class MathBuildUpFlags {
		/// <summary>No options.</summary>
		None = 0,
		/// <summary>Use chemical formula conversions.</summary>
		ChemicalFormula = NAMESPACE::tomChemicalFormula,
		/// <summary>A delimiter follows insertion points (formula automatic buildup).</summary>
		HaveDelimiter = NAMESPACE::tomHaveDelimiter,
		/// <summary>Use math alphabetics for English/Greek letters except for math function names.</summary>
		Alphabetics = NAMESPACE::tomMathAlphabetics,
		/// <summary>Apply an object template to a range.</summary>
		ApplyTemplate = NAMESPACE::tomMathApplyTemplate,
		/// <summary>Use Arabic math alphabetics for variables.</summary>
		ArabicAlphabetics = NAMESPACE::tomMathArabicAlphabetics,
		/// <summary>Internal math autocorrect standard math \ keywords.</summary>
		AutoCorrect = NAMESPACE::tomMathAutoCorrect,
		/// <summary>Invoke external autocorrect in manual build up.</summary>
		AutoCorrectExt = NAMESPACE::tomMathAutoCorrectExt,
		/// <summary>Autocorrect operator pairs.</summary>
		AutoCorrectOpPairs = NAMESPACE::tomMathAutoCorrectOpPairs,
		/// <summary>Handle the Backspace key inside a math object.</summary>
		Backspace = NAMESPACE::tomMathBackspace,
		/// <summary>Build down instead of up.</summary>
		BuildDown = NAMESPACE::tomMathBuildDown,
		/// <summary>Build down only outermost objects.</summary>
		BuildDownOutermost = NAMESPACE::tomMathBuildDownOutermost,
		/// <summary>Build up insertion points argument or zone.</summary>
		BuildUpArgOrZone = NAMESPACE::tomMathBuildUpArgOrZone,
		/// <summary>Enable recursive build up.</summary>
		BuildUpRecurse = NAMESPACE::tomMathBuildUpRecurse,
		/// <summary>Collapse the selection after build up or build down.</summary>
		CollapseSel = NAMESPACE::tomMathCollapseSel,
		/// <summary>Delete the argument at the start of the range.</summary>
		DeleteArg = NAMESPACE::tomMathDeleteArg,
		/// <summary>Delete argument 1 (0-based count).</summary>
		DeleteArg1 = NAMESPACE::tomMathDeleteArg1,
		/// <summary>Delete argument 2 (0-based count).</summary>
		DeleteArg2 = NAMESPACE::tomMathDeleteArg2,
		/// <summary>Delete a column.</summary>
		DeleteCol = NAMESPACE::tomMathDeleteCol,
		/// <summary>Delete a row.</summary>
		DeleteRow = NAMESPACE::tomMathDeleteRow,
		/// <summary>Handle the Enter key inside a math object.</summary>
		Enter = NAMESPACE::tomMathEnter,
		/// <summary>Insert a column or separator after the current argument.</summary>
		InsColAfter = NAMESPACE::tomMathInsColAfter,
		/// <summary>Insert a column or separator before the current argument.</summary>
		InsColBefore = NAMESPACE::tomMathInsColBefore,
		/// <summary>Insert a row below the insertion point.</summary>
		InsRowAfter = NAMESPACE::tomMathInsRowAfter,
		/// <summary>Insert a row above the insertion point.</summary>
		InsRowBefore = NAMESPACE::tomMathInsRowBefore,
		/// <summary>Change to a linear fraction.</summary>
		MakeFracLinear = NAMESPACE::tomMathMakeFracLinear,
		/// <summary>Change to a slashed fraction.</summary>
		MakeFracSlashed = NAMESPACE::tomMathMakeFracSlashed,
		/// <summary>Change to a stacked fraction.</summary>
		MakeFracStacked = NAMESPACE::tomMathMakeFracStacked,
		/// <summary>Change from superscript/subscript to left superscript/subscript.</summary>
		MakeLeftSubSup = NAMESPACE::tomMathMakeLeftSubSup,
		/// <summary>Change left superscript/subscript to superscript/subscript.</summary>
		MakeSubSup = NAMESPACE::tomMathMakeSubSup,
		/// <summary>Build down the outermost object without its characters.</summary>
		RemoveOutermost = NAMESPACE::tomMathRemoveOutermost,
		/// <summary>Handle minor differences between the rich edit control and Word.</summary>
		RichEdit = NAMESPACE::tomMathRichEdit,
		/// <summary>Handle the Shift+Tab key combination inside a math object.</summary>
		ShiftTab = NAMESPACE::tomMathShiftTab,
		/// <summary>Single character typed for build up.</summary>
		SingleChar = NAMESPACE::tomMathSingleChar,
		/// <summary>Handle the Ctrl+= key combination in a math zone.</summary>
		Subscript = NAMESPACE::tomMathSubscript,
		/// <summary>Handle the Ctrl+Shift+= key combination in a math zone.</summary>
		Superscript = NAMESPACE::tomMathSuperscript,
		/// <summary>Handle the Tab key inside a math object.</summary>
		Tab = NAMESPACE::tomMathTab,
		/// <summary>A terminating operator is needed. Only used with a degenerate range for formula autobuildup.</summary>
		NeedTermOp = NAMESPACE::tomNeedTermOp,
		/// <summary>Build down for plain text.</summary>
		Plain = NAMESPACE::tomPlain,
		/// <summary>Don't hide empty argument placeholders on build-up.</summary>
		ShowEmptyArgPlaceholders = NAMESPACE::tomShowEmptyArgPlaceholders,
		/// <summary>Enables TeX syntax for build up/down operations.</summary>
		TeX = NAMESPACE::tomTeX
	};

	/// <summary>
	/// Gravity values for a range.
	/// </summary>
	public enum class RangeGravity {
		/// <summary>Use selection user interface rules.</summary>
		UI = tomGravityUI,
		/// <summary>Both ends have backward gravity.</summary>
		Back = tomGravityBack,
		/// <summary>Both ends have forward gravity.</summary>
		Fore = tomGravityFore,
		/// <summary>Inward gravity; that is, the start is forward, and the end is backward.</summary>
		In = tomGravityIn,
		/// <summary>Outward gravity; that is, the start is backward, and the end is forward.</summary>
		Out = tomGravityOut
	};

	/// <summary>
	/// Math function types.
	/// </summary>
	public enum class MathFunctionType {
		/// <summary>Not in the function list.</summary>
		None = tomFunctionTypeNone,
		/// <summary>An ordinary math function that takes arguments.</summary>
		TakesArg = tomFunctionTypeTakesArg,
		/// <summary>Use the lower limit for _, and so on.</summary>
		TakesLim = tomFunctionTypeTakesLim,
		/// <summary>Turn the preceding FA into an NBSP.</summary>
		TakesLim2 = tomFunctionTypeTakesLim2,
		/// <summary>A "lim" function.</summary>
		IsLim = tomFunctionTypeIsLim
	};

	/// <summary>
	/// Vertical positions.
	/// </summary>
	public enum class VerticalPosition {
		/// <summary>Top.</summary>
		Top = TA_TOP,
		/// <summary>Baseline.</summary>
		Baseline = TA_BASELINE,
		/// <summary>Bottom.</summary>
		Bottom = TA_BOTTOM
	};

	/// <summary>
	/// Horizontal positions.
	/// </summary>
	public enum class HorizontalPosition {
		/// <summary>Left.</summary>
		Left = TA_LEFT,
		/// <summary>Center.</summary>
		Center = TA_CENTER,
		/// <summary>Right.</summary>
		Right = TA_RIGHT
	};

	/// <summary>
	/// Types of inline/math objects.
	/// </summary>
	public enum class InlineObjectType {
		/// <summary>Not an inline object.</summary>
		None = -1,
		/// <summary>Not an inline function.</summary>
		SimpleText = tomSimpleText,
		/// <summary>Base text with ruby annotation.</summary>
		Ruby = tomRuby,
		/// <summary>Text flows horizontally in a vertically oriented document.</summary>
		HorzVert = tomHorzVert,
		/// <summary>A Warichu "2 lines in one" comment.</summary>
		Warichu = tomWarichu,
		/// <summary>Accent (combining mark).</summary>
		Accent = tomAccent,
		/// <summary>Abstract box with properties.</summary>
		Box = tomBox,
		/// <summary>Encloses the argument in a rectangle.</summary>
		BoxedFormula = tomBoxedFormula,
		/// <summary>Encloses the argument in brackets, parentheses, and so on.</summary>
		Brackets = tomBrackets,
		/// <summary>Encloses the argument in brackets, parentheses, and so on, and with separators.</summary>
		BracketsWithSeps = tomBracketsWithSeps,
		/// <summary>An RTF Eq (equation) field.</summary>
		Eq = tomEq,
		/// <summary>Column of aligned equations.</summary>
		EquationArray = tomEquationArray,
		/// <summary>Fraction.</summary>
		Fraction = tomFraction,
		/// <summary>Function apply.</summary>
		FunctionApply = tomFunctionApply,
		/// <summary>Left subscript or superscript.</summary>
		LeftSubSup = tomLeftSubSup,
		/// <summary>Second argument below the first.</summary>
		LowerLimit = tomLowerLimit,
		/// <summary>Matrix.</summary>
		Matrix = tomMatrix,
		/// <summary>General n-ary expression.</summary>
		Nary = tomNary,
		/// <summary>Internal use for no-build operators.</summary>
		OpChar = tomOpChar,
		/// <summary>Overscores argument.</summary>
		Overbar = tomOverbar,
		/// <summary>Special spacing.</summary>
		Phantom = tomPhantom,
		/// <summary>Square root, and so on.</summary>
		Radical = tomRadical,
		/// <summary>Skewed and built-up linear fractions.</summary>
		SlashedFraction = tomSlashedFraction,
		/// <summary>"Fraction" with no divide bar.</summary>
		Stack = tomStack,
		/// <summary>Stretch character horizontally over or under argument.</summary>
		StretchStack = tomStretchStack,
		/// <summary>Subscript.</summary>
		Subscript = tomSubscript,
		/// <summary>Subscript and superscript combination.</summary>
		SubSup = tomSubSup,
		/// <summary>Superscript.</summary>
		Superscript = tomSuperscript,
		/// <summary>Underscores the argument.</summary>
		Underbar = tomUnderbar,
		/// <summary>Second argument above the first.</summary>
		UpperLimit = tomUpperLimit
	};

	/// <summary>
	/// TeX styles for math.
	/// </summary>
	public enum class MathTeXStyle {
		/// <summary>The math handler determines the style.</summary>
		Default = tomStyleDefault,
		/// <summary>The 2nd and higher level subscript superscript size, cramped.</summary>
		ScriptScriptCramped = tomStyleScriptScriptCramped,
		/// <summary>The 2nd and higher level subscript superscript size.</summary>
		ScriptScript = tomStyleScriptScript,
		/// <summary>The 1st level subscript superscript size, cramped.</summary>
		ScriptCramped = tomStyleScriptCramped,
		/// <summary>The 1st level subscript superscript size.</summary>
		Script = tomStyleScript,
		/// <summary>Text size cramped, for example, inside a square root.</summary>
		TextCramped = tomStyleTextCramped,
		/// <summary>The standard inline text size.</summary>
		Text = tomStyleText,
		/// <summary>Display style cramped.</summary>
		DisplayCramped = tomStyleDisplayCramped,
		/// <summary>Display style.</summary>
		Display = tomStyleDisplay
	};

	/// <summary>
	/// Alignment values for ruby objects (<see cref="InlineObjectType::Ruby"/>).
	/// </summary>
	public enum class RubyAlign {
		/// <summary>Below.</summary>
		Below = tomRubyBelow,
		/// <summary>Center alignment.</summary>
		AlignCenter = tomRubyAlignCenter,
		/// <summary>010 alignment.</summary>
		Align010 = tomRubyAlign010,
		/// <summary>121 alignment.</summary>
		Align121 = tomRubyAlign121,
		/// <summary>Left alignment.</summary>
		AlignLeft = tomRubyAlignLeft,
		/// <summary>Right alignment.</summary>
		AlignRight = tomRubyAlignRight
	};

	/// <summary>
	/// Alignment values for box objects (<see cref="InlineObjectType::Box"/>).
	/// </summary>
	public enum class BoxAlign {
		/// <summary>Vertically align with center on baseline.</summary>
		BoxAlignCenter = tomBoxAlignCenter,
		/// <summary>Default spacing</summary>
		SpaceDefault = tomSpaceDefault,
		/// <summary>Space the object as if it were a unary operator.</summary>
		SpaceUnary = tomSpaceUnary,
		/// <summary>Space the object as if it were a binary operator.</summary>
		SpaceBinary = tomSpaceBinary,
		/// <summary>Space the object as if it were a relational operator.</summary>
		SpaceRelational = tomSpaceRelational,
		/// <summary>Space the object as if it were a unary operator.</summary>
		SpaceSkip = tomSpaceSkip,
		/// <summary>Space the object as if it were an ordinal operator.</summary>
		SpaceOrd = tomSpaceOrd,
		/// <summary>Space the object as if it were a differential operator.</summary>
		SpaceDifferential = tomSpaceDifferential,
		/// <summary>Treat as text size.</summary>
		SizeText = tomSizeText,
		/// <summary>Treat as script size (approximately 73% of text size).</summary>
		SizeScript = tomSizeScript,
		/// <summary>Treat as subscript size (approximately 60% of text size).</summary>
		SizeScriptScript = tomSizeScriptScript,
		/// <summary>Do not break arguments across a line.</summary>
		NoBreak = tomNoBreak,
		/// <summary>Position as if only the argument appears.</summary>
		TransparentForPositioning = tomTransparentForPositioning,
		/// <summary>Space according to argument properties.</summary>
		TransparentForSpacing = tomTransparentForSpacing
	};

	/// <summary>
	/// Alignment values for boxed formula objects (<see cref="InlineObjectType::BoxedFormula"/>).
	/// </summary>
	public enum class BoxedFormulaAlign {
		/// <summary>Hide top border.</summary>
		BoxHideTop = tomBoxHideTop,
		/// <summary>Hide bottom border.</summary>
		BoxHideBottom = tomBoxHideBottom,
		/// <summary>Hide left border.</summary>
		BoxHideLeft = tomBoxHideLeft,
		/// <summary>Hide right border.</summary>
		BoxHideRight = tomBoxHideRight,
		/// <summary>Display horizontal strikethrough.</summary>
		BoxStrikeH = tomBoxStrikeH,
		/// <summary>Display vertical strikethrough.</summary>
		BoxStrikeV = tomBoxStrikeV,
		/// <summary>Display diagonal strikethrough from the top left to the lower right.</summary>
		BoxStrikeTLBR = tomBoxStrikeTLBR,
		/// <summary>Display diagonal strikethrough from the lower left to the top right.</summary>
		BoxStrikeBLTR = tomBoxStrikeBLTR
	};

	/// <summary>
	/// Alignment values for bracket objects (<see cref="InlineObjectType::Brackets"/>).
	/// </summary>
	public enum class BracketsAlign {
		/// <summary>Center brackets at baseline.</summary>
		AlignDefault = tomAlignDefault,
		/// <summary>Text is centered between the margins.</summary>
		AlignCenter = tomAlignCenter,
		/// <summary>Use brackets that match the argument ascent and descent.</summary>
		AlignMatchAscentDescent = tomAlignMatchAscentDescent,
		/// <summary>TeX variant 0: big</summary>
		TeXVariant0 = tomMathVariant,
		/// <summary>TeX variant 1: Big</summary>
		TeXVariant1 = (tomMathVariant + 64),
		/// <summary>TeX variant 2: bigg</summary>
		TeXVariant2 = (tomMathVariant + 128),
		/// <summary>TeX variant 3: Bigg</summary>
		TeXVariant3 = (tomMathVariant + 192)
	};

	/// <summary>
	/// Alignment values for equation array objects  (<see cref="InlineObjectType::EquationArray"/>).
	/// </summary>
	public enum class EquationArrayAlign {
		/// <summary>Expand the right size to the layout width (for equation number)</summary>
		LayoutWidth = tomEqArrayLayoutWidth,
		/// <summary>Align the center of the equation array on the baseline.</summary>
		AlignCenter = tomEqArrayAlignCenter,
		/// <summary>Align the top row of the equation on the baseline.</summary>
		AlignTopRow = tomEqArrayAlignTopRow,
		/// <summary>Align the bottom row of the equation on the baseline.</summary>
		AlignBottomRow = tomEqArrayAlignBottomRow,
	};

	/// <summary>
	/// Alignment values for matrix objects (<see cref="InlineObjectType::Matrix"/>).
	/// </summary>
	public enum class MatrixAlign {
		/// <summary>Align the matrix center on baseline.</summary>
		AlignCenter = tomMatrixAlignCenter,
		/// <summary>Align the matrix top row on the baseline.</summary>
		AlignTopRow = tomMatrixAlignTopRow,
		/// <summary>Align the matrix bottom row on the baseline.</summary>
		AlignBottomRow = tomMatrixAlignBottomRow,
		/// <summary>Align the top of equations on the baseline.</summary>
		ShowMatPlaceHldr = tomShowMatPlaceHldr
	};

	/// <summary>
	/// Alignment values for n-ary objects (<see cref="InlineObjectType::Nary"/>).
	/// </summary>
	public enum class NaryAlign {
		/// <summary>Limit locations use document default.</summary>
		LimitsDefault = tomLimitsDefault,
		/// <summary>Limits are placed under and over the operator.</summary>
		LimitsUnderOver = tomLimitsUnderOver,
		/// <summary>Limits are operator subscript and superscript.</summary>
		LimitsSubSup = tomLimitsSubSup,
		/// <summary>The upper limit is a superscript.</summary>
		UpperLimitAsSuperScript = tomUpperLimitAsSuperScript,
		/// <summary>Switch between <see cref="NaryAlign::LimitsSubSup"/> and <see cref="NaryAlign::LimitsUnderOver"/>.</summary>
		LimitsOpposite = tomLimitsOpposite,
		/// <summary>Show empty lower limit placeholder.</summary>
		ShowLLimPlaceHldr = tomShowLLimPlaceHldr,
		/// <summary>Show empty upper limit placeholder.</summary>
		ShowULimPlaceHldr = tomShowULimPlaceHldr,
		/// <summary>Don't grow the n-ary operator with the argument.</summary>
		DontGrowWithContent = tomDontGrowWithContent,
		/// <summary>Grow the n-ary operator with the argument.</summary>
		GrowWithContent = tomGrowWithContent
	};

	/// <summary>
	/// Alignment values for phantom objects (<see cref="InlineObjectType::Phantom"/>).
	/// </summary>
	public enum class PhantomAlign {
		/// <summary>Display the phantom object's argument.</summary>
		Show = tomPhantomShow,
		/// <summary>The phantom object has zero width.</summary>
		ZeroWidth = tomPhantomZeroWidth,
		/// <summary>The phantom object has zero ascent.</summary>
		ZeroAscent = tomPhantomZeroAscent,
		/// <summary>The phantom object has zero descent.</summary>
		ZeroDescent = tomPhantomZeroDescent,
		/// <summary>Space the phantom object as if only the argument is present.</summary>
		Transparent = tomPhantomTransparent
	};

	/// <summary>
	/// Alignment values for radical objects (<see cref="InlineObjectType::Radical"/>).
	/// </summary>
	public enum class RadicalAlign {
		/// <summary>Default alignment.</summary>
		Default = tomAlignDefault,
		/// <summary>Show empty radical degree placeholder.</summary>
		ShowDegPlaceHldr = tomShowDegPlaceHldr
	};

	/// <summary>
	/// Alignment values for subscript-superscript objects (<see cref="InlineObjectType::SubSup"/>).
	/// </summary>
	public enum class SubSupAlign {
		/// <summary>Default alignment.</summary>
		Default = tomAlignDefault,
		/// <summary>Align subscript under superscript.</summary>
		SubSupAlign = tomSubSupAlign
	};

	/// <summary>
	/// Alignment values for stretched stack objects (<see cref="InlineObjectType::StretchStack"/>).
	/// </summary>
	public enum class StretchStackAlign {
		/// <summary>Stretch character below base.</summary>
		CharBelow = tomStretchCharBelow,
		/// <summary>Stretch character above base.</summary>
		CharAbove = tomStretchCharAbove,
		/// <summary>Stretch base below character.</summary>
		BaseBelow = tomStretchBaseBelow,
		/// <summary>Stretch base above character.</summary>
		BaseAbove = tomStretchBaseAbove
	};

	/// <summary>
	/// Compression mode for East Asian characters.
	/// </summary>
	public enum class EastAsianCompressionMode {
		/// <summary>No compression.</summary>
		None = tomCompressNone,
		/// <summary>Compress punctuation.</summary>
		Punctuation = tomCompressPunctuation,
		/// <summary>Compress punctuation and kana.</summary>
		PunctuationAndKana = tomCompressPunctuationAndKana,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Character effects.
	/// </summary>
	[Flags]
	public enum class CharacterEffects {
		/// <summary>No effects.</summary>
		None = 0,
		/// <summary>All caps.</summary>
		AllCaps = tomAllCaps,
		/// <summary>Bold.</summary>
		Bold = tomBold,
		/// <summary>Disabled text.</summary>
		Disabled = tomDisabled,
		/// <summary>Emboss.</summary>
		Emboss = tomEmboss,
		/// <summary>Hidden text.</summary>
		Hidden = tomHidden,
		/// <summary>Imprint.</summary>
		Imprint = tomImprint,
		/// <summary>The start delimiter of an inline object.</summary>
		InlineObjectStart = tomInlineObjectStart,
		/// <summary>Italic.</summary>
		Italic = tomItalic,
		/// <summary>Hyperlink.</summary>
		Link = tomLink,
		/// <summary>The link is protected (friendly name link).</summary>
		LinkProtected = tomLinkProtected,
		/// <summary>Math zone.</summary>
		MathZone = tomMathZone,
		/// <summary>Display math zone.</summary>
		MathZoneDisplay = tomMathZoneDisplay,
		/// <summary>Don't build up operator.</summary>
		MathZoneNoBuildUp = tomMathZoneNoBuildUp,
		/// <summary>Math zone ordinary text.</summary>
		MathZoneOrdinary = tomMathZoneOrdinary,
		/// <summary>Outline.</summary>
		Outline = tomOutline,
		/// <summary>Protected text.</summary>
		Protected = tomProtected,
		/// <summary>Revised text.</summary>
		Revised = tomRevised,
		/// <summary>Shadow.</summary>
		Shadow = tomShadow,
		/// <summary>Small caps.</summary>
		SmallCaps = tomSmallCaps,
		/// <summary>Strikeout.</summary>
		Strikeout = tomStrikeout,
		/// <summary>Underline.</summary>
		Underline = tomUnderline,
		/// <summary>Combines all character effects.</summary>
		All = AllCaps | Bold | Disabled | Emboss | Hidden | Imprint | InlineObjectStart
			| Italic | Link | LinkProtected | MathZone | MathZoneDisplay | MathZoneNoBuildUp 
			| MathZoneOrdinary | Outline | Protected | Revised | Shadow | SmallCaps 
			| Strikeout | Underline,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Additional character effects.
	/// </summary>
	[Flags]
	public enum class CharacterEffects2 {
		/// <summary>No additional effects.</summary>
		None = 0,
		/// <summary>Use East Asian auto spacing between alphabetics.</summary>
		AutoSpaceAlpha = tomAutoSpaceAlpha,
		/// <summary>Use East Asian auto spacing for digits.</summary>
		AutoSpaceNumeric = tomAutoSpaceNumeric,
		/// <summary>Use East Asian automatic spacing for parentheses or brackets.</summary>
		AutoSpaceParens = tomAutoSpaceParens,
		/// <summary>Double strikeout.</summary>
		DoubleStrike = tomDoublestrike,
		/// <summary>Embedded font.</summary>
		EmbeddedFont = tomEmbeddedFont,
		/// <summary>Use East Asian character-pair-width modification.</summary>
		ModWidthPairs = tomModWidthPairs,
		/// <summary>Use East Asian space-width modification.</summary>
		ModWidthSpace = tomModWidthSpace,
		/// <summary>Run has overlapping text.</summary>
		Overlapping = tomOverlapping,
		/// <summary>Combines all additional character effects.</summary>
		All = AutoSpaceAlpha | AutoSpaceNumeric | AutoSpaceParens | DoubleStrike
			| EmbeddedFont | ModWidthPairs | ModWidthSpace | Overlapping,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Hyperlink types.
	/// </summary>
	public enum class LinkType {
		/// <summary>Not a link.</summary>
		NoLink = tomNoLink,
		/// <summary>The URL only; that is, no friendly name.</summary>
		ClientLink = tomClientLink,
		/// <summary>The name of friendly name link.</summary>
		FriendlyLinkName = tomFriendlyLinkName,
		/// <summary>The URL of a friendly name link.</summary>
		FriendlyLinkAddress = tomFriendlyLinkAddress,
		/// <summary>The URL of an automatic link.</summary>
		AutoLinkURL = tomAutoLinkURL,
		/// <summary>An automatic link to an email address.</summary>
		AutoLinkEmail = tomAutoLinkEmail,
		/// <summary>An automatic link to a phone number.</summary>
		AutoLinkPhone = tomAutoLinkPhone,
		/// <summary>An automatic link to a storage location.</summary>
		AutoLinkPath = tomAutoLinkPath
	};

	/// <summary>
	/// Underline position modes.
	/// </summary>
	public enum class UnderlinePositionMode {
		/// <summary>Automatically set the underline position.</summary>
		Auto = tomUnderlinePositionAuto,
		/// <summary>Render underline below text.</summary>
		Below = tomUnderlinePositionBelow,
		/// <summary>Render underline above text.</summary>
		Above = tomUnderlinePositionAbove,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Paragraph effects.
	/// </summary>
	[Flags]
	public enum class ParagraphEffects {
		/// <summary>No effects.</summary>
		None = 0,
		/// <summary>Right-to-left paragraph</summary>
		RTL = tomParaEffectRTL,
		/// <summary>Keep the paragraph together.</summary>
		Keep = tomParaEffectKeep,
		/// <summary>Keep with next the paragraph.</summary>
		KeepNext = tomParaEffectKeepNext,
		/// <summary>Put a page break before this paragraph.</summary>
		PageBreakBefore = tomParaEffectPageBreakBefore,
		/// <summary>No line number for this paragraph.</summary>
		NoLineNumber = tomParaEffectNoLineNumber,
		/// <summary>No widow control.</summary>
		NoWidowControl = tomParaEffectNoWidowControl,
		/// <summary>Don't hyphenate this paragraph.</summary>
		DoNotHyphen = tomParaEffectDoNotHyphen,
		/// <summary>Side by side.</summary>
		SideBySide = tomParaEffectSideBySide,
		/// <summary>Heading contents are collapsed (in outline view).</summary>
		Collapsed = tomParaEffectCollapsed,
		/// <summary>Outline view nested level.</summary>
		OutlineLevel = tomParaEffectOutlineLevel,
		/// <summary>Paragraph has boxed effect (is not displayed).</summary>
		Box = tomParaEffectBox,
		/// <summary>At or inside table delimiter.</summary>
		TableRowDelimiter = tomParaEffectTableRowDelimiter,
		/// <summary>Inside or at the start of a table.</summary>
		Table = tomParaEffectTable,
		/// <summary>Combines all paragraph effects.</summary>
		All = RTL | Keep | KeepNext | PageBreakBefore | NoLineNumber 
			| NoWidowControl | DoNotHyphen | SideBySide | Collapsed 
			| OutlineLevel | Box | TableRowDelimiter | Table,
		/// <summary>Undefined.</summary>
		Undefined = tomUndefined
	};

	/// <summary>
	/// Font alignment modes.
	/// </summary>
	public enum class FontAlignment {
		/// <summary>For horizontal layout, align CJK characters on the baseline. For vertical layout, center align CJK characters.</summary>
		Auto = tomFontAlignmentAuto,
		/// <summary>For horizontal layout, top align CJK characters. For vertical layout, right align CJK characters.</summary>
		Top = tomFontAlignmentTop,
		/// <summary>For horizontal or vertical layout, align CJK characters on the baseline.</summary>
		Baseline = tomFontAlignmentBaseline,
		/// <summary>For horizontal layout, bottom align CJK characters. For vertical layout, left align CJK characters.</summary>
		Bottom = tomFontAlignmentBottom,
		/// <summary>For horizontal layout, center CJK characters vertically. For vertical layout, center align CJK characters horizontally.</summary>
		Center = tomFontAlignmentCenter
	};

	/// <summary>
	/// Controls how row formatting attributes are applied.
	/// </summary>
	public enum class RowApplyOptions {
		/// <summary>Apply all formatting attributes.</summary>
		Default = tomRowApplyDefault,
		/// <summary>Apply only cell widths and count.</summary>
		CellStructureChangeOnly = tomCellStructureChangeOnly		
	};

	/// <summary>
	/// Indicates whether a cell part of a horizontal or vertical merged cell set.
	/// </summary>
	[Flags]
	public enum class CellMergeFlags {
		None = 0,
		/// <summary>Any cell except the start in a horizontally merged cell set.</summary>
		HorizontalCont = tomHContCell,
		/// <summary>Start a cell in a horizontally merged cell set.</summary>
		HorizontalStart = tomHStartCell,
		/// <summary>Any cell except the top cell in a vertically merged cell set.</summary>
		VerticalCont = tomVLowCell,
		/// <summary>The top cell in a vertically merged cell set.</summary>
		VerticalStart = tomVTopCell
	};

	/// <summary>
	/// Options for converting Math RTF to Office MathML (OMML).
	/// </summary>
	public enum class OfficeMathMLMode {
		/// <summary>Preserves the type(s) of math zone in the range.</summary>
		Default = 0,
		/// <summary>Convert to inline math zone(s).</summary>
		Inline,
		/// <summary>Combine individual math zones into a single display math zone.</summary>
		Display,
		/// <summary>Wraps the math zone(s) in a Word paragraph.</summary>
		WordProcessingML
	};

	/// <summary>
	/// Versions of the Text Object Model.
	/// </summary>
	public enum class TOMVersion {
		/// <summary>TOM version 1. Supports the ITextDocument interface only.</summary>
		TOM1 = 1,
		/// <summary>TOM version 2. Supports the ITextDocument and ITextDocument2 interfaces.</summary>
		TOM2 = 2
	};

#endif

} NAMESPACE_CLOSE