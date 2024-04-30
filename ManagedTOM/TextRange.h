// TextRange.h

#pragma once

#include "Stdafx.h"
#include <TOM.h>
#include <Richedit.h>
#include "DataObject.h"
#include "enums.h"
#include "InlineObjectProperties.h"
#include "TextFont.h"
#include "TextPara.h"
#include "TextRow.h"

// Throws NotSupportedException if interface pointer is not ITextRange2
#define THROW_IF_NOT_TOM2() THROW_IF_NOT_INTERFACE(_range2)

using namespace System;
using namespace System::IO;
using namespace System::Drawing;
using namespace System::Windows::Forms;
using namespace System::Runtime::InteropServices;
using namespace System::Xml::Linq;
using namespace System::Text;
using namespace System::Text::RegularExpressions;
using namespace System::Collections::Generic;

namespace NAMESPACE_DECL {

	/// <summary>
	/// Represents a range of text in a document in the Text Object Model (TOM).
	/// </summary>
	/// <remarks>
	/// <para>
#ifdef _TOM2
	/// Managed wrapper class for the COM interface ITextRange, including ITextRange2 functionality. 
	/// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb774058%28v=vs.85%29.aspx">ITextRange interface</seealso>
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768622(v=vs.85).aspx">ITextRange2 interface</seealso>
#else
	/// Managed wrapper class for the COM interface ITextRange. 
	/// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb774058%28v=vs.85%29.aspx">ITextRange interface</seealso>
#endif
	/// </para>
	/// <para>
	/// Ranges are the primary mechanism for reading and manipulating text in 
	/// TOM. A range provides access to the content and formatting options for 
	/// the rich text bounded by a particular start and end position.
	/// </para>
	/// <para>
	/// A range can be obtained by:
	/// </para>
	/// <list type="bullet">
	/// <item><description>
	/// Using the TextDocument.EntireRange property, to get a range that spans the entire document
	/// </description></item>
	/// <item><description>
	/// Requesting a range with a particular start and end position via the TextDocument.Range(Int32,Int32) method
	/// </description></item>
	/// <item><description>
	/// Creating a duplicate of an existing range (using the <see cref="TextRange::Clone"/> method), which can then be moved/resized independently
	/// </description></item>
	/// </list>
	/// </remarks>
	public ref class TextRange : public IDisposable, public IEquatable<TextRange^>, public ICloneable {

	public:
				
		/// <summary>
		/// Gets a value determining whether the specified range can be edited.
		/// </summary>
		property bool CanEdit {
			bool get() {
				HRESULT hr = _range->CanEdit(NULL);
				return (hr == S_OK);
			}
		}
		/// <summary>
		/// Gets a value determining if data on the clipboard can be pasted into the current range. 
		/// </summary>
		property bool CanPaste {
			bool get() {
				HRESULT hr = _range->CanPaste(NULL, 0, NULL);
				return (hr == S_OK);
			}
		}
		/// <summary>
		/// Gets or sets the character at the start position of the range.
		/// </summary>
		property System::Char Char {
			System::Char get() {
				long chr;
				HRESULT hr = _range->GetChar(&chr);
				THROW_HRESULT(hr);
				return (System::Char)chr;	
			}
			void set(System::Char chr) {
				HRESULT hr = _range->SetChar(chr);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the end character position of the range.
		/// </summary>
		property int End {
			int get() {
				long end;
				HRESULT hr = _range->GetEnd(&end);
				THROW_HRESULT(hr);
				return end;
			}
			void set(int value) {
				HRESULT hr = _range->SetEnd(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the length of the range.
		/// </summary>
		property int Length {
			int get() {
				return End - Start;
			}
			void set(int value) {
				End = Start + value;
			}
		}
		/// <summary>
		/// Gets or sets a <see cref="TextFont"/> object with the character attributes of the specified range.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is null.
		/// </exception>
		property TextFont^ Font {
			TextFont^ get() {
				if (_range2 != NULL) {
					ITextFont2* font = NULL;
					HRESULT hr = _range2->GetFont2(&font);
					THROW_HRESULT(hr);
					return gcnew TextFont(font);
				}
				else {
					ITextFont* font = NULL;
					HRESULT hr = _range->GetFont(&font);
					THROW_HRESULT(hr);
					return gcnew TextFont(font);
				}
			}
			void set(TextFont^ value) {
				if (value == nullptr) throw gcnew ArgumentNullException("value");

				if (_range2 != NULL) {
					HRESULT hr = _range2->SetFont2((ITextFont2*)value->ComObject.ToPointer());
					THROW_HRESULT(hr);
				}
				else {
					HRESULT hr = _range->SetFont((ITextFont*)value->ComObject.ToPointer());
					THROW_HRESULT(hr);
				}
			}
		}
		/// <summary>
		/// Gets or sets a <see cref="TextPara"/> object with the paragraph attributes of the specified range.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is null.
		/// </exception>
		property TextPara^ Para {
			TextPara^ get() {
				if (_range2 != NULL) {
					ITextPara2* para = NULL;
					HRESULT hr = _range2->GetPara2(&para);
					THROW_HRESULT(hr);
					return gcnew TextPara(para);
				}
				else {
					ITextPara* para = NULL;
					HRESULT hr = _range->GetPara(&para);
					THROW_HRESULT(hr);
					return gcnew TextPara(para);
				}
			}
			void set(TextPara^ value) {
				if (value == nullptr) throw gcnew ArgumentNullException("value");

				if (_range2 != NULL) {
					HRESULT hr = _range2->SetPara2((ITextPara2*)value->ComObject.ToPointer());
					THROW_HRESULT(hr);
				}
				else {
					HRESULT hr = _range->SetPara((ITextPara*)value->ComObject.ToPointer());
					THROW_HRESULT(hr);
				}
			}
		}
		/// <summary>
		/// Gets or sets the content of the range as a Rich Text Format (RTF) string.
		/// </summary>
		/// <remarks>
		/// If the range is empty, value of this property is <see cref="String::Empty"/>. 
		/// Setting this property to <see cref="String::Empty"/> will clear the range. 
		/// </remarks>
		property String^ Rtf {
			String^ get() {
				HRESULT hr;
				String^ str = String::Empty;

				if (End == Start) return str;

				// copy RTF data into local data object
				::IDataObject *pData = NULL;

				VARIANT var;
				var.vt = VT_UNKNOWN | VT_BYREF;
				var.ppunkVal = (LPUNKNOWN *)(&pData);

				hr = _range->Copy(&var);

				THROW_HRESULT(hr);
				if (!pData) throw gcnew InvalidOperationException("IDataObject was not returned by ITextRange::Copy() method");

				// decode RTF data
				::FORMATETC fe;
				fe.cfFormat = RegisterClipboardFormat(CF_RTF);
				fe.dwAspect = DVASPECT_CONTENT;
				fe.lindex = -1;
				fe.ptd = NULL;
				fe.tymed = TYMED_HGLOBAL;

				hr = pData->QueryGetData(&fe); //test if data is available
				THROW_HRESULT(hr);

				::STGMEDIUM stm;
				hr = pData->GetData(&fe, &stm);   //get data. Be sure to check the return value.

				if (hr == S_OK) {
					// lock the memory location of the string and get a pointer to it
					LPSTR pszMsg = (LPSTR)GlobalLock(stm.hGlobal);
					str = Marshal::PtrToStringAnsi(IntPtr(pszMsg));

					if (stm.pUnkForRelease) 
						ReleaseStgMedium(&stm);
					else
						GlobalFree(pszMsg); 
				}
				
				if (pData) pData->Release();

				THROW_HRESULT(hr);

				return str;
			}
			void set(String^ value) {				
				HRESULT hr;
				if (value == nullptr) value = String::Empty;

				// create a local data object which contains the string
				::IDataObject* pDataObject = NULL;
					
				::FORMATETC fmt = { RegisterClipboardFormat(CF_RTF), 0, DVASPECT_CONTENT, -1, TYMED_HGLOBAL };
					
				// convert managed string to global string
				IntPtr ptr = Marshal::StringToHGlobalAnsi(value);

				::STGMEDIUM stgm = { TYMED_HGLOBAL, { 0 }, 0 };				
				stgm.hGlobal = ptr.ToPointer();

				hr = ::CreateDataObject(&fmt, &stgm, 1, &pDataObject);
				
				if (hr == S_OK) {
					// if we can paste the RTF data, do so
					VARIANT var;
					var.vt = VT_UNKNOWN;
					var.punkVal = (IUnknown*)pDataObject;

					if (_range->CanPaste(&var, 0, NULL) == S_OK) {
						hr = _range->Paste(&var, 0);
					}
				}

				// clean up
				if (pDataObject) pDataObject->Release();
				ReleaseStgMedium(&stgm);

				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the start character position of the range.
		/// </summary>
		property int Start {
			int get() {
				long start;
				HRESULT hr = _range->GetStart(&start);
				THROW_HRESULT(hr);
				return start;
			}
			void set(int value) {
				HRESULT hr = _range->SetStart(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the plain text in this range.
		/// </summary>
		property String^ Text {
			String^ get() {
				BSTR bstr = NULL;
				HRESULT hr = _range->GetText(&bstr);
				THROW_HRESULT(hr);
						
				IntPtr ip = IntPtr(bstr);
						
				if (ip != IntPtr::Zero) {
					String^ value = Marshal::PtrToStringBSTR(ip);
					SysFreeString(bstr);
					return value;
				}
				else {
					return String::Empty;
				}
			}
			void set(String^ value) {
				if (value == nullptr) value = String::Empty;
				BSTR bstr = (BSTR)Marshal::StringToBSTR(value).ToPointer();
				HRESULT hr = _range->SetText(bstr);
				THROW_HRESULT(hr);
				SysFreeString(bstr);
			}
		}

		/// <summary>
		/// Changes the case of letters in this range according to the <paramref name="type"/> parameter.
		/// </summary>
		/// <param name="type">Type of case change.</param>
		/// <example>
		/// The following example demonstrates how to change the case of text 
		/// in a range: 
		/// <code source="..\Examples\TextRange.cs" region="ChangeCase" language="cs" />
		/// </example>
		void ChangeCase(TextCasing type) {
			HRESULT hr = _range->ChangeCase((int)type);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Collapses the specified text range into a degenerate point at either the beginning or end of the range. 
		/// </summary>
		/// <param name="collapseTo">Flag specifying the end to collapse at.</param>
		/// <example>
		/// The following example demonstrates the effect of collapsing and 
		/// expanding a range: 
		/// <code source="..\Examples\TextRange.cs" region="Collapse" language="cs" />
		/// </example>
		void Collapse(RangePosition collapseTo) {
			HRESULT hr = _range->Collapse((int)collapseTo);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Copies the text to the clipboard.
		/// </summary>
		void Copy() {
			HRESULT hr = _range->Copy(NULL);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Returns an <see cref="System::Windows::Forms::IDataObject"/> containing the text in the range.
		/// </summary>
		/// <returns>A data object containing RTF and Unicode Text formats.</returns>
		/// <example>
		/// The following example demonstrates how to extract RTF text from the   
		/// <see cref="System::Windows::Forms::IDataObject"/> obtained from a 
		/// range: 
		/// <code source="..\Examples\TextRange.cs" region="GetDataObject" language="cs" />
		/// </example>
		System::Windows::Forms::IDataObject^ GetDataObject() {
			::IDataObject *pData = NULL;
			DataObject^ o = gcnew DataObject();

			VARIANT var;
			var.vt = VT_UNKNOWN | VT_BYREF;
			var.ppunkVal = (LPUNKNOWN *)(&pData);
					
			HRESULT hr = _range->Copy(&var);
			THROW_HRESULT(hr);

			try {
				// RTF
				o->SetData(DataFormats::Rtf, ExtractText(pData, RegisterClipboardFormat(CF_RTF), false));						

				// plain text
				o->SetData(DataFormats::UnicodeText, ExtractText(pData, CF_UNICODETEXT, true));
			}
			finally {
				if (pData) pData->Release();
			}

			return o;
		}

		/// <summary>
		/// Cuts the plain or rich text to the clipboard. 
		/// </summary>
		void Cut() {
			HRESULT hr = _range->Cut(NULL);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Mimics the DELETE and BACKSPACE keys, with and without the CTRL key depressed. 
		/// </summary>
		/// <param name="unit">
		/// Unit to use. Unit can be <see cref="TextUnit::Character"/> 
		/// (the default value) or <see cref="TextUnit::Word"/>.
		/// </param>
		/// <param name="count"></param>
		/// <returns>The count of units deleted.</returns>
		int Delete(TextUnit unit, int count) {
			switch (unit) {
				case TextUnit::Character:
				case TextUnit::Word:
					break;
				default:
					throw gcnew ArgumentException("Only character and word units are allowed");
			}

			long actual;
			HRESULT hr = _range->Delete((int)unit, count, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Moves this range's ends to the end of the last overlapping <paramref name="unit"/> in the range. 
		/// </summary>
		/// <param name="unit">Unit to use.</param>
		/// <param name="extend">Indicator of how the shifting of the range ends is to proceed.</param>
		/// <returns>The count of characters that End is moved past.</returns>
		/// <example>
		/// The following example demonstrates how to use the 
		/// <see cref="StartOf"/> and <see cref="EndOf"/> methods to move the 
		/// ends of a range: 
		/// <code source="..\Examples\TextRange.cs" region="EndOf" language="cs" />
		/// </example>
		int EndOf(TextUnit unit, RangeShiftType extend) {
			long actual;
			HRESULT hr = _range->EndOf((int)unit, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Expands this range so that any partial units it contains are completely contained. 
		/// </summary>
		/// <param name="unit">Unit to include.</param>
		/// <returns>The count of characters added to the range.</returns>
		/// <example>
		/// The following example demonstrates the effect of collapsing and 
		/// expanding a range: 
		/// <code source="..\Examples\TextRange.cs" region="Collapse" language="cs" />
		/// </example>
		int Expand(TextUnit unit) {
			long actual;
			HRESULT hr = _range->Expand((int)unit, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Searches to the end of the story for the text given by <paramref name="text"/>. 
		/// The matching criteria are given by <paramref name="flags"/>. 
		/// </summary>
		/// <param name="text">String to find.</param>
		/// <param name="flags">Flags governing comparisons.</param>
		/// <returns>The length of string matched.</returns>
		/// <example>
		/// The following example demonstrates how to find text in a range: 
		/// <code source="..\Examples\TextRange.cs" region="FindText" language="cs" />
		/// </example>
		int FindText(String^ text, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType flags
		) {
			long length;
			
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _range->FindText(bstr, tomForward, (int)flags, &length);
			if (hr == S_FALSE) return -1;
			Marshal::FreeBSTR(IntPtr(bstr));

			THROW_HRESULT(hr);
			return length;
		}

		/// <summary>
		/// Searches up to <paramref name="count"/> characters for the text given by <paramref name="text"/>. 
		/// The starting position and direction are also specified by <paramref name="count"/>, 
		/// and the matching criteria are given by <paramref name="flags"/>. 
		/// </summary>
		/// <param name="text">String to find.</param>
		/// <param name="count">Maximum number of characters to search.</param>
		/// <param name="flags">Flags governing comparisons.</param>
		/// <returns>The length of string matched.</returns>
		/// <example>
		/// The following example demonstrates how to find text in a range: 
		/// <code source="..\Examples\TextRange.cs" region="FindText" language="cs" />
		/// </example>
		int FindText(String^ text, int count, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType flags
		) {
			long length;
					
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _range->FindText(bstr, count, (int)flags, &length);
			if (hr == S_FALSE) return -1;
			Marshal::FreeBSTR(IntPtr(bstr));

			THROW_HRESULT(hr);
			return length;
		}

		/// <summary>
		/// Searches to the end of the story for <paramref name="text"/>, starting from the range's end. 
		/// </summary>
		/// <param name="text">String to find.</param>
		/// <param name="flags">Flags governing comparisons.</param>
		/// <returns>The length of string matched.</returns>
		/// <remarks>
		/// The search is subject to the comparison parameter, <paramref name="flags"/>. 
		/// If the string is found, the end is changed to be the end of the matched string, and the length of the string is returned. 
		/// If the string is not found, the range is unchanged and zero is returned.
		/// </remarks>
		int FindTextEnd(String^ text, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType flags
		) {
			long length;
					
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _range->FindTextEnd(bstr, tomForward, (int)flags, &length);
			if (hr == S_FALSE) return -1;
			Marshal::FreeBSTR(IntPtr(bstr));

			THROW_HRESULT(hr);
			return length;
		}

		/// <summary>
		/// Searches up to <paramref name="count"/> characters for <paramref name="text"/>, starting from the range's end. 
		/// </summary>
		/// <param name="text">String to find.</param>
		/// <param name="count">Maximum number of characters to search.</param>
		/// <param name="flags">Flags governing comparisons.</param>
		/// <returns>The length of string matched.</returns>
		/// <remarks>
		/// The search is subject to the comparison parameter, <paramref name="flags"/>. 
		/// If the string is found, the end is changed to be the end of the matched string, and the length of the string is returned. 
		/// If the string is not found, the range is unchanged and zero is returned.
		/// </remarks>
		int FindTextEnd(String^ text, int count, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType flags
		) {
			long length;
					
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _range->FindTextEnd(bstr, count, (int)flags, &length);
			if (hr == S_FALSE) return -1;
			Marshal::FreeBSTR(IntPtr(bstr));

			THROW_HRESULT(hr);
			return length;
		}

		/// <summary>
		/// Searches to the end of the story for <paramref name="text"/>, starting at the range's start. 
		/// </summary>
		/// <param name="text">String to find.</param>
		/// <param name="flags">Flags governing comparisons.</param>
		/// <returns>The length of string matched.</returns>
		/// <remarks>
		/// The search is subject to the comparison parameter, <paramref name="flags"/>. 
		/// If the string is found, the start is changed to be the matched string, and the length of the string is returned. 
		/// If the string is not found, the range is unchanged and zero is returned.
		/// </remarks>
		int FindTextStart(String^ text, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType flags
		) {
			long length;
					
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _range->FindTextStart(bstr, tomForward, (int)flags, &length);
			if (hr == S_FALSE) return -1;
			Marshal::FreeBSTR(IntPtr(bstr));

			THROW_HRESULT(hr);
			return length;
		}

		/// <summary>
		/// Searches up to <paramref name="count"/> characters for <paramref name="text"/>, starting at the range's start. 
		/// </summary>
		/// <param name="text">String to find.</param>
		/// <param name="count">Maximum number of characters to search.</param>
		/// <param name="type">Flags governing comparisons.</param>
		/// <returns>The length of string matched.</returns>
		/// <remarks>
		/// The search is subject to the comparison parameter, <paramref name="type"/>. 
		/// If the string is found, the start is changed to be the matched string, and the length of the string is returned. 
		/// If the string is not found, the range is unchanged and zero is returned.
		/// </remarks>
		int FindTextStart(String^ text, int count, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType type
		) {
			long length;
					
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _range->FindTextStart(bstr, count, (int)type, &length);
			if (hr == S_FALSE) return -1;
			Marshal::FreeBSTR(IntPtr(bstr));

			THROW_HRESULT(hr);
			return length;
		}

		/// <summary>
		/// Gets a duplicate of this range object. 
		/// </summary>
		/// <returns>The duplicate of the range.</returns>
		/// <example>
		/// The following example demonstrates how to duplicate a 
		/// <see cref="TextRange"/> object: 
		/// <code source="..\Examples\TextRange.cs" region="Clone" language="cs" />
		/// </example>
		TextRange^ Clone() {
			if (_range2 != NULL) {
				ITextRange2* dup = NULL;
				HRESULT hr = _range2->GetDuplicate2(&dup);
				THROW_HRESULT(hr);
				return gcnew TextRange(dup);
			}
			else {
				ITextRange* dup = NULL;
				HRESULT hr = _range->GetDuplicate(&dup);
				THROW_HRESULT(hr);
				return gcnew TextRange(dup);
			}
		}

		/// <summary>
		/// Retrieves the embedded object at the start of the specified range. 
		/// The range must either be an insertion point or it must select only the embedded object. 
		/// </summary>
		/// <returns>The object.</returns>
		Object^ GetEmbeddedObject() {
			IUnknown* punk = NULL;
			HRESULT hr = _range->GetEmbeddedObject(&punk);
			THROW_HRESULT(hr);
			return (hr == S_OK) ? Marshal::GetObjectForIUnknown(IntPtr(punk)) : nullptr;
		}

		/// <summary>
		/// Retrieves the story index of the <paramref name="unit"/> parameter at the specified range start character position. 
		/// The first unit in a story has an index value of 1. 
		/// The index of a Unit is the same for all character positions from that immediately preceding the unit up to the last character in the unit.
		/// </summary>
		/// <param name="unit">Unit that is indexed.</param>
		/// <returns>The index value.</returns>
		int GetIndex(TextUnit unit) {
			long index;
			HRESULT hr = _range->GetIndex((int)unit, &index);
			THROW_HRESULT(hr);
			return index;
		}

		/// <summary>
		/// Retrieves screen coordinates for the start or end character position in the text range, along with the intra-line position.
		/// </summary>
		/// <param name="character">Indicates the start or end of the range. </param>
		/// <param name="position">Indicate the horizontal and vertical position.</param>
		/// <param name="flags">Flag that indicates the position to retrieve.</param>
		/// <returns>The coordinates of the point.</returns>
		Point GetPoint(RangePosition character, ContentAlignment position, 
			[Optional, DefaultParameterValue(RangePointFlags::None)] RangePointFlags flags
		) {
			long type = (int)character + ToPositionFlags(position) + (int)flags;
			long x;
			long y;
			HRESULT hr = _range->GetPoint(type, &x, &y);
			THROW_HRESULT(hr);
			return Point(x,y);
		}

		/// <summary>
		/// Determines whether this range is within or at the same text as a specified range. 
		/// </summary>
		/// <param name="other">Text that is compared to the current range. </param>
		/// <returns>The comparison result.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other"/> is null.
		/// </exception>
		bool InRange(TextRange^ other) {
			if (other == nullptr) throw gcnew ArgumentNullException("other");

			long result;
			HRESULT hr = _range->InRange(other->_range, &result);
			THROW_HRESULT(hr);
			return (result == tomTrue);
		}

		/// <summary>
		/// Moves the insertion point forward or backward a specified number of units. 
		/// If the range is nondegenerate, the range is collapsed to an insertion point at either end, 
		/// depending on <paramref name="count"/>, and then is moved. 
		/// </summary>
		/// <param name="unit">Unit to use.</param>
		/// <param name="count">Number of units to move past.</param>
		/// <returns>The actual number of units the insertion point moves past.</returns>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int Move(TextUnit unit, int count) {
			long result;
			HRESULT hr = _range->Move((int)unit, count, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Moves the end position of the range. 
		/// </summary>
		/// <param name="unit">The units by which to move the end of the range.</param>
		/// <param name="count">Number of units to move past.</param>
		/// <returns>The actual number of units that the end position of the range is moved past.</returns>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveEnd(TextUnit unit, int count) {
			long result;
			HRESULT hr = _range->MoveEnd((int)unit, count, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Moves the range's end to the character position of the first 
		/// character found that is in the set of characters specified by 
		/// <paramref name="chars"/>.
		/// </summary>
		/// <param name="chars">The character set to use in the match.</param>
		/// <returns>The actual number of characters that the range end is moved.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="chars"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveEndUntil(... array<System::Char>^ chars) {
			if (chars == nullptr) throw gcnew ArgumentNullException("chars");

			BSTR bstr = (BSTR)Marshal::StringToBSTR(gcnew String(chars)).ToPointer();

			VARIANT v;
			v.vt = VT_BSTR;
			v.bstrVal = bstr;
					
			long result;
			HRESULT hr = _range->MoveEndUntil(&v, tomForward, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Moves the end of the range just past all contiguous characters 
		/// that are found in the set of characters specified by 
		/// <paramref name="chars"/>, whichever is less. 
		/// </summary>
		/// <param name="chars">The character set to use in the match.</param>
		/// <returns>The actual number of characters that the range end is moved.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="chars"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveEndWhile(... array<System::Char>^ chars) {
			if (chars == nullptr) throw gcnew ArgumentNullException("chars");

			BSTR bstr = (BSTR)Marshal::StringToBSTR(gcnew String(chars)).ToPointer();

			VARIANT v;
			v.vt = VT_BSTR;
			v.bstrVal = bstr;
					
			long result;
			HRESULT hr = _range->MoveEndWhile(&v, tomForward, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Moves the start position of the range. 
		/// </summary>
		/// <param name="unit">Unit used in the move.</param>
		/// <param name="count">Number of units to move.</param>
		/// <returns>The actual number of units that the end is moved.</returns>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveStart(TextUnit unit, int count) {
			long result;
			HRESULT hr = _range->MoveStart((int)unit, count, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Moves the range's start to the character position of the first 
		/// character found that is in the set of characters specified by 
		/// <paramref name="chars"/>.
		/// </summary>
		/// <param name="chars">The character set to use in the match.</param>
		/// <returns>The actual number of characters the start of the range is moved.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="chars"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveStartUntil(... array<System::Char>^ chars) {
			if (chars == nullptr) throw gcnew ArgumentNullException("chars");

			BSTR bstr = (BSTR)Marshal::StringToBSTR(gcnew String(chars)).ToPointer();

			VARIANT v;
			v.vt = VT_BSTR;
			v.bstrVal = bstr;
					
			long result;
			HRESULT hr = _range->MoveStartUntil(&v, tomForward, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Moves the start position of the range just past all contiguous 
		/// characters that are found in the set of characters specified by 
		/// <paramref name="chars"/>. 
		/// </summary>
		/// <param name="chars">The character set to use in the match.</param>
		/// <returns>The actual number of characters the start of the range is moved.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="chars"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveStartWhile(... array<System::Char>^ chars) {
			if (chars == nullptr) throw gcnew ArgumentNullException("chars");

			BSTR bstr = (BSTR)Marshal::StringToBSTR(gcnew String(chars)).ToPointer();

			VARIANT v;
			v.vt = VT_BSTR;
			v.bstrVal = bstr;
					
			long result;
			HRESULT hr = _range->MoveStartWhile(&v, tomForward, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Searches for the first character in the set of characters 
		/// specified by <paramref name="chars"/>. If a character is found, 
		/// the range is collapsed to that point.
		/// </summary>
		/// <param name="chars">The character set to use in the match.</param>
		/// <returns>The number of characters the insertion point is moved.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="chars"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveUntil(... array<System::Char>^ chars) {
			if (chars == nullptr) throw gcnew ArgumentNullException("chars");

			BSTR bstr = (BSTR)Marshal::StringToBSTR(gcnew String(chars)).ToPointer();

			VARIANT v;
			v.vt = VT_BSTR;
			v.bstrVal = bstr;
					
			long result;
			HRESULT hr = _range->MoveUntil(&v, tomForward, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Starts at the end of the range and searches while the characters 
		/// belong to the set specified by <paramref name="chars"/>.
		/// The range is collapsed to an insertion point when a non-matching character is found.
		/// </summary>
		/// <param name="chars">The character set to use in the match.</param>
		/// <returns>The number of characters the insertion point is moved.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="chars"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates the various move methods: 
		/// <code source="..\Examples\TextRange.cs" region="Move" language="cs" />
		/// </example>
		int MoveWhile(... array<System::Char>^ chars) {
			if (chars == nullptr) throw gcnew ArgumentNullException("chars");

			BSTR bstr = (BSTR)Marshal::StringToBSTR(gcnew String(chars)).ToPointer();

			VARIANT v;
			v.vt = VT_BSTR;
			v.bstrVal = bstr;
					
			long result;
			HRESULT hr = _range->MoveWhile(&v, tomForward, &result);
			THROW_HRESULT(hr);
			return result;
		}

		/// <summary>
		/// Pastes text from the clipboard.
		/// </summary>
		void Paste() {
			HRESULT hr = _range->Paste(NULL, 0);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Replaces the text in the range with the specified data object.
		/// </summary>
		/// <param name="o">The data object containing the new text.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="o"/> is null.
		/// </exception>
		/// <remarks>
		/// <para>
		/// The data object may contain:
		/// </para>
		/// <list type="bullet">
		/// <item><description><see cref="DataFormats::Text"/></description></item>
		/// <item><description><see cref="DataFormats::UnicodeText"/></description></item>
		/// <item><description><see cref="DataFormats::Rtf"/></description></item>
		/// <item><description><see cref="DataFormats::Bitmap"/> (image)</description></item>
		/// <item><description><see cref="DataFormats::Dib"/> (image)</description></item>
		/// <item><description><see cref="DataFormats::MetafilePict"/> (image)</description></item>
		/// <item><description><see cref="DataFormats::FileDrop"/> (OLE object)</description></item>
		/// <item><description>...as well as several other formats</description></item>
		/// </list>
		/// <para>
		/// Different implementations of the Text Object Model (TOM) may 
		/// support different formats.
		/// </para>
		/// </remarks>
		/// <example>
		/// The following example demonstrates how to set the text in a range 
		/// using an <see cref="System::Windows::Forms::IDataObject"/>:
		/// <code source="..\Examples\TextRange.cs" region="SetDataObject" language="cs" />
		/// </example>
		void SetDataObject(System::Windows::Forms::IDataObject^ o) {
			if (o == nullptr) throw gcnew ArgumentNullException("o");
			
			HRESULT hr;
			IUnknown* punk = (IUnknown*)Marshal::GetIUnknownForObject(o).ToPointer();
					
			try {
				::IDataObject *pData = NULL;
					
				hr = punk->QueryInterface(__uuidof(::IDataObject), (void**)&pData);
				THROW_HRESULT(hr);

				VARIANT var;
				var.vt = VT_UNKNOWN | VT_BYREF;
				var.ppunkVal = (IUnknown**)&pData;
					
				hr = _range->Paste(&var, 0);
				THROW_HRESULT(hr);
			}
			finally {
				Marshal::Release(IntPtr(punk));
			}
		}

		/// <summary>
		/// Scrolls the range into view. 
		/// </summary>
		/// <param name="position">Flag specifying the end to scroll into view.</param>
		void ScrollIntoView(RangePosition position) {
			HRESULT hr = _range->ScrollIntoView((int)position);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Sets the start and end positions, and story values of the active selection, to those of this range. 
		/// </summary>
		void Select() {
			HRESULT hr = _range->Select();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Sets the formatted text of this range text to the formatted text of the specified range.
		/// </summary>
		/// <param name="other">The formatted text to replace this range's text. </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other"/> is null.
		/// </exception>		
		void SetFormattedText(TextRange^ other) {
			if (other == nullptr) throw gcnew ArgumentNullException("other");

			if (_range2 != NULL) {
				HRESULT hr = _range2->SetFormattedText2(other->_range2);
				THROW_HRESULT(hr);
			}
			else {
				HRESULT hr = _range->SetFormattedText(other->_range);
				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// Changes this range to the specified unit of the story. 
		/// </summary>
		/// <param name="unit">Unit used to index the range.</param>
		/// <param name="index">Index for the unit.</param>
		/// <param name="extend">Flag that indicates the extent of the range.</param>
		void SetIndex(TextUnit unit, int index, RangeShiftType extend) {
			HRESULT hr = _range->SetIndex((int)unit, index, (int)extend);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Changes the range based on a specified point at or up through 
		/// (depending on <paramref name="extend"/>) the point (x, y) aligned 
		/// according to <paramref name="position"/>. 
		/// </summary>
		/// <param name="p">Coordinates of the specified point, in absolute screen coordinates. </param>
		/// <param name="position">The end to move to the specified point.</param>
		/// <param name="extend">How to set the endpoints of the range.</param>
		void SetPoint(Point p, RangePosition position, RangeShiftType extend) {
			HRESULT hr = _range->SetPoint(p.X, p.Y, (int)position, (int)extend);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Adjusts the range endpoints to the specified values. 
		/// </summary>
		/// <param name="start">Character position for the start of the range.</param>
		/// <param name="length">New length for the range.</param>
		void SetRange(int start, int length) {
			HRESULT hr = _range->SetRange(start, start + length);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Moves the range ends to the start of the first overlapping Unit in the range. 
		/// </summary>
		/// <param name="unit">Unit to use in the move operation.</param>
		/// <param name="extend">How to move the ends of the range.</param>
		/// <returns>The number of characters that the start position is moved.</returns>
		/// <example>
		/// The following example demonstrates how to use the 
		/// <see cref="StartOf"/> and <see cref="EndOf"/> methods to move the 
		/// ends of a range: 
		/// <code source="..\Examples\TextRange.cs" region="EndOf" language="cs" />
		/// </example>
		int StartOf(TextUnit unit, RangeShiftType extend) {
			long actual;
			HRESULT hr = _range->StartOf((int)unit, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Returns the plain text in the range.
		/// </summary>
		virtual String^ ToString() override {
			return Text;
		}

		/// <summary>
		/// Determines whether this range has the same character positions and story as those of a specified range.
		/// </summary>
		/// <param name="other">Text range that is compared to the current range.</param>
		/// <returns>The comparison result.</returns>
		virtual bool Equals(TextRange^ other) {
			long result;
			HRESULT hr = _range->IsEqual(other->_range, &result);
			THROW_HRESULT(hr);
			return (result == tomTrue);
		}

		/// <summary>	
		/// Tests if this object is considered equal to another.
		/// </summary>
		/// <param name="other">The object to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(Object^ other) override {
			TextRange^ range = dynamic_cast<TextRange^>(other);
			if (range != nullptr)
				return Equals(range);
			else
				return Object::Equals(other);
		}

		/// <summary>
		/// Returns the hash code for the object.
		/// </summary>
		virtual int GetHashCode() override {
			return Start.GetHashCode() ^ End.GetHashCode() ^ Text->GetHashCode();
		}

#ifdef _TOM2

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the count of subranges, including the active subrange in the current range.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		property int SubrangeCount {
			int get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _range2->GetCount(&value);
				THROW_HRESULT(hr);

				return value;
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the gravity of this range.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		property RangeGravity Gravity {
			RangeGravity get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _range2->GetGravity(&value);
				THROW_HRESULT(hr);

				return (RangeGravity)value;
			}
			void set(RangeGravity value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _range2->SetGravity((long)value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the properties of the inline object at the range active end.
		/// </summary>
		/// <remarks>
		/// <see cref="InlineObjectProperties"/> is a value type. 
		/// You must re-assign the value of this property if you make any changes.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <example>
		/// The following example demonstrates how to retrieve the properties 
		/// of an inline math object:
		/// <code source="..\Examples\TextRange.cs" region="InlineObject" language="cs" />
		/// </example>
		property InlineObjectProperties InlineObject {
			InlineObjectProperties get() {
				THROW_IF_NOT_TOM2();
				if (Length != 0) throw gcnew InvalidOperationException("This property is only valid on degenerate (collapsed) ranges.");

				long p0, p1, p2, p3, p4, p5, p6, p7, p8;
				HRESULT hr = _range2->GetInlineObject(&p0, &p1, &p2, &p3, &p4, &p5, &p6, &p7, &p8);
				THROW_HRESULT(hr);
				
				InlineObjectProperties value = InlineObjectProperties();

				if (hr == S_FALSE) {
					// not an inline object
					value.Type = InlineObjectType::None;
					return value;
				}

				value.Type = (InlineObjectType)p0;
				if (value.AlignType != nullptr) value.Align = (Enum^)Enum::ToObject(value.AlignType, p1);
				value.Char = (System::Char)p2;
				value.Char1 = (System::Char)p3;
				value.Char2 = (System::Char)p4;
				value.ArgCount = p5;
				value.TeXStyle = (MathTeXStyle)p6;
				value.ColCount = p7;
				value.Level = p8;

				return value;
			}
			void set(InlineObjectProperties value) {
				THROW_IF_NOT_TOM2();
				if (Length != 0) throw gcnew InvalidOperationException("This property is only valid on degenerate (collapsed) ranges.");

				HRESULT hr = _range2->SetInlineObject(
					(long)value.Type, 
					Convert::ToInt32(value.Align), 
					(long)value.Char,
					(long)value.Char1,
					(long)value.Char2,
					value.ArgCount,
					(long)value.TeXStyle,
					value.ColCount
				);

				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets the character position of the start of the paragraph that contains the range's start character position.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		property int StartPara {
			int get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _range2->GetStartPara(&value);
				THROW_HRESULT(hr);

				return value;
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the URL text associated with a range.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		property String^ URL {
			String^ get() {
				THROW_IF_NOT_TOM2();

				BSTR bstr;
				HRESULT hr = _range2->GetURL(&bstr);
				THROW_HRESULT(hr);
				if (hr == S_FALSE) return String::Empty;
				String^ s = Marshal::PtrToStringBSTR(IntPtr(bstr));
				SysFreeString(bstr);

				return s;
			}
			void set(String^ value) {
				THROW_IF_NOT_TOM2();
				if (value == nullptr) value = String::Empty;

				BSTR bstr = (BSTR)Marshal::StringToBSTR(value).ToPointer();
				HRESULT hr = _range2->SetURL(bstr);
				SysFreeString(bstr);

				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Adds a subrange to this range.
		/// </summary>
		/// <param name="index">The active-end character position of the subrange.</param>
		/// <param name="length">The length of the subrange.</param>
		/// <param name="activate">If true, the new subrange is the active subrange.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void AddSubrange(int index, int length, bool activate) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->AddSubrange(index, index + length, activate ? tomTrue : tomFalse);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Converts the linear-format math in a range to a built-up form, or modifies the current built-up form.
		/// </summary>
		/// <param name="options">Combination of <see cref="MathBuildUpFlags"/> flags.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support math.
		/// </exception>
		/// <example>
		/// The following example demonstrates how to build-up math text from 
		/// a linear format:
		/// <code source="..\Examples\TextRange.cs" region="BuildUpMath" language="cs" />
		/// </example>
		void BuildUpMath([Optional, DefaultParameterValue(MathBuildUpFlags::None)] MathBuildUpFlags options) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->BuildUpMath((long)options);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Deletes a subrange from the range.
		/// </summary>
		/// <param name="index">The start character position of the subrange.</param>
		/// <param name="length">The length of the subrange.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void DeleteSubrange(int index, int length) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->DeleteSubrange(index, index + length);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Searchs for math inline functions in text as specified by a source range.
		/// </summary>
		/// <param name="range">The formatted text to find in the range's text.</param>
		/// <param name="count">The number of characters to search through.</param>
		/// <param name="flags">Flags that control the search.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void Find(TextRange^ range, int count, 
			[Optional, DefaultParameterValue(RangeMatchType::None)] RangeMatchType flags
		) {
			THROW_IF_NOT_TOM2();
			if (range == nullptr) throw gcnew ArgumentNullException("range");

			long delta;
			HRESULT hr = _range2->Find((ITextRange2*)range->ComObject.ToPointer(), count, (long)flags, &delta);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the cells in the range.
		/// </summary>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support tables.
		/// </exception>
		Object^ GetCells() {
			THROW_IF_NOT_TOM2();

			IUnknown* cells;
			HRESULT hr = _range2->GetCells(&cells);
			THROW_HRESULT(hr);

			return Marshal::GetObjectForIUnknown(IntPtr(cells));
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the character at the specified offset from the end of this range.
		/// </summary>
		/// <param name="offset">The offset from the end of the range. An offset of 0 gets the character at the end of the range.</param>
		/// <returns>The character value, as a string converted from its UTF-32 representation.</returns>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		System::String^ GetChar([Optional] int offset) {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _range2->GetChar2(&value, offset);
			THROW_HRESULT(hr);

			return System::Char::ConvertFromUtf32(value);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the column in the range.
		/// </summary>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support tables.
		/// </exception>
		Object^ GetColumn() {
			THROW_IF_NOT_TOM2();

			IUnknown* column;
			HRESULT hr = _range2->GetColumn(&column);
			THROW_HRESULT(hr);

			return Marshal::GetObjectForIUnknown(IntPtr(column));
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the line and position of the drop cap.
		/// </summary>
		/// <param name="line">Line.</param>
		/// <param name="position">Position.</param>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void GetDropCap([Out] int% line, [Out] int% position) {
			THROW_IF_NOT_TOM2();

			long p0, p1;
			HRESULT hr = _range2->GetDropCap(&p0, &p1);
			THROW_HRESULT(hr);

			line = p0;
			position = p1;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Retrieves the math function type associated with the specified math function name.
		/// </summary>
		/// <param name="functionName">The math function name that is checked to determine the math function type.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support math.
		/// </exception>
		MathFunctionType GetMathFunctionType(String^ functionName) {
			THROW_IF_NOT_TOM2();
			if (functionName == nullptr) functionName = String::Empty;

			BSTR bstr = (BSTR)Marshal::StringToBSTR(functionName).ToPointer();

			long value;
			HRESULT hr = _range2->GetMathFunctionType(bstr, &value);
			SysFreeString(bstr);

			THROW_HRESULT(hr);

			return (MathFunctionType)value;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the value of a property.
		/// </summary>
		/// <param name="type">The property ID.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		int GetProperty(int type) {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _range2->GetProperty(type, &value);
			THROW_HRESULT(hr);

			return value;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Retrieves a rectangle of the specified type for the current range.
		/// </summary>
		/// <param name="type">The type of rectangle to return.</param>
		/// <param name="vPos">The vertical position.</param>
		/// <param name="hPos">The horizontal position.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		System::Drawing::Rectangle GetRectangle(RangePointFlags type, 
			[Optional, DefaultParameterValue(VerticalPosition::Top)] VerticalPosition vPos, 
			[Optional, DefaultParameterValue(HorizontalPosition::Left)] HorizontalPosition hPos
		) {
			THROW_IF_NOT_TOM2();
			
			long left, top, right, bottom, hit;
			HRESULT hr = _range2->GetRect((long)type | (long)vPos | (long)hPos, &left, &top, &right, &bottom, &hit);
			THROW_HRESULT(hr);

			return System::Drawing::Rectangle::FromLTRB(left, top, right, bottom); 
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the row properties in the currently selected row.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support tables.
		/// </exception>
		TextRow^ GetRow() {
			THROW_IF_NOT_TOM2();

			ITextRow* row = NULL;
			HRESULT hr = _range2->GetRow(&row);
			if (hr == E_FAIL) return nullptr;
			THROW_HRESULT(hr);

			return gcnew TextRow(row);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Retrieves a subrange in a range.
		/// </summary>
		/// <param name="subrange">The subrange index.</param>
		/// <param name="index">The character position for the start of the subrange.</param>
		/// <param name="length">The length of the subrange.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void GetSubrange(int subrange, [Out] int% index, [Out] int% length) {
			THROW_IF_NOT_TOM2();

			long p0, p1;
			HRESULT hr = _range2->GetSubrange(subrange, &p0, &p1);
			THROW_HRESULT(hr);

			index = p0;
			length = p1 - p0;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the table in the range.
		/// </summary>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support tables.
		/// </exception>
		Object^ GetTable() {
			THROW_IF_NOT_TOM2();

			IUnknown* table;
			HRESULT hr = _range2->GetTable(&table);
			THROW_HRESULT(hr);

			return Marshal::GetObjectForIUnknown(IntPtr(table));
		}

		/// <summary>
		/// (TOM 2 only)
		/// Gets the text in this range according to the specified conversion flags.
		/// </summary>
		/// <param name="options">The flags controlling how the text is retrieved.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		String^ GetText([Optional] PlainTextFlags options) {
			THROW_IF_NOT_TOM2();
			
			BSTR bstr;
			HRESULT hr = _range2->GetText2((long)options, &bstr);
			THROW_HRESULT(hr);
			if (bstr == NULL) return String::Empty;
			String^ s = Marshal::PtrToStringBSTR(IntPtr(bstr));
			SysFreeString(bstr);

			return s;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Converts and replaces the hexadecimal number at the end of this range to a Unicode character.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void HexToUnicode() {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->HexToUnicode();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Inserts a table in a range.
		/// </summary>
		/// <param name="width">The width, in HIMETRIC units (0.01 mm), of the image.</param>
		/// <param name="height">The height, in HIMETRIC units, of the image.</param>
		/// <param name="ascent">
		/// If <paramref name="vPos"/> is <see cref="VerticalPosition::Baseline"/>, this parameter is the distance, in HIMETRIC units, that the top of the image extends above the text baseline. 
		/// If <paramref name="vPos"/> is <see cref="VerticalPosition::Baseline"/> and ascent is zero, the bottom of the image is placed at the text baseline.
		/// </param>
		/// <param name="vPos">The vertical alignment of the image.</param>
		/// <param name="altText">The alternate text for the image.</param>
		/// <param name="stream">The stream that contains the image data.</param>
		/// <remarks>
		/// This method makes a copy of the data in the stream. 
		/// It is the caller's responsibility to close the original stream.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="stream"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates how to insert an image into a range: 
		/// <code source="..\Examples\TextRange.cs" region="InsertImage" language="cs" />
		/// </example>
		void InsertImage(int width, int height, int ascent, VerticalPosition vPos, String^ altText, System::IO::Stream^ stream) {
			THROW_IF_NOT_TOM2();
			if (stream == nullptr) throw gcnew ArgumentNullException("stream");

			HRESULT hr;

			Stream^ ms;
			int length;
			bool closeStream = false;

			try {
				// we are only interested in the data after the current position in the stream
				length = (int)(stream->Length - stream->Position);
				ms = stream;
			}
			catch (NotSupportedException^) {
				// make an in-memory copy of the stream (in order to determine its length)
				ms = gcnew MemoryStream();
				closeStream = true;
				stream->CopyTo(ms);
				length = (int)ms->Length;
				ms->Position = 0;
			}
			
			// allocate HGLOBAL			
			HGLOBAL hGlobal = GlobalAlloc(GHND, length);
			unsigned char* block = (unsigned char*)GlobalLock(hGlobal);

			// copy data to HGLOBAL
			for (int i=0; i<length; i++) {
				*(block + i) = (unsigned char)ms->ReadByte();
			}
			if (closeStream) delete ms;
			
			GlobalUnlock(hGlobal);

			// create stream on HGLOBAL (if successful, will auto free when stream is closed)
			IStream* pStream = NULL;
			hr = CreateStreamOnHGlobal(hGlobal, TRUE, (LPSTREAM*)&pStream);
			if (hr < S_OK) {
				GlobalFree(hGlobal);
				Marshal::ThrowExceptionForHR(hr);
			}

			// marshal the string as a BSTR
			if (altText == nullptr) altText = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(altText).ToPointer();
			
			hr = _range2->InsertImage(width, height, ascent, (long)vPos, bstr, pStream);			

			// clean up
			SysFreeString(bstr);
			pStream->Release();

			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Inserts a table in a range.
		/// </summary>
		/// <param name="columns">The number of columns in the table.</param>
		/// <param name="rows">The number of rows in the table.</param>
		/// <param name="autoFit">Specifies how the cells fit the target space.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support tables.
		/// </exception>
		void InsertTable(int columns, int rows, bool autoFit) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->InsertTable(columns, rows, autoFit ? tomTrue : tomFalse);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Translates the built-up math, ruby, and other inline objects in this range to linearized form.
		/// </summary>
		/// <param name="options">Combination of <see cref="MathLinearizeFlags"/> flags.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the range does not support math.
		/// </exception>
		/// <example>
		/// The following example demonstrates how to build-down (linearize) math text: 
		/// <code source="..\Examples\TextRange.cs" region="Linearize" language="cs" />
		/// </example>
		void Linearize([Optional, DefaultParameterValue(MathLinearizeFlags::None)] MathLinearizeFlags options) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->Linearize((long)options);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Makes the specified subrange the active subrange of this range.
		/// </summary>
		/// <param name="index">The anchor end character position of the subrange to make active.</param>
		/// <param name="length">The length of the subrange.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void SetActiveSubrange(int index, int length) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->SetActiveSubrange(index, index + length);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Sets the line and position of the drop cap.
		/// </summary>
		/// <param name="line">Line.</param>
		/// <param name="position">Position.</param>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void SetDropCap(int line, int position) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->SetDropCap(line, position);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Sets the value of the specified property.
		/// </summary>
		/// <param name="type">The ID of the property to set.</param>
		/// <param name="value">The new property value.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void SetProperty(int type, int value) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->SetProperty(type, value);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only)
		/// Sets the text of this range.
		/// </summary>
		/// <param name="value">The new text.</param>
		/// <param name="options">Flags controlling how the text is inserted in the range.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void SetText(String^ value, [Optional] RichTextFlags options) {
			THROW_IF_NOT_TOM2();

			if (value == nullptr) value = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(value).ToPointer();
			HRESULT hr = _range2->SetText2((long)options, bstr);
			THROW_HRESULT(hr);
			SysFreeString(bstr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Converts the Unicode character(s) preceding the start position of this text range to a hexadecimal number, and selects it.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextRange2 interface.
		/// </exception>
		void UnicodeToHex() {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _range2->UnicodeToHex();
			THROW_HRESULT(hr);
		}

#endif

	internal:

		TextRange(ITextRange* range) {
			_range = range;
			_range2 = NULL;
		}

#ifdef _TOM2

		TextRange(ITextRange2* range) {
			_range2 = range;
			_range = range;
		}

		property TOMVersion SupportedVersion {
			TOMVersion get() {
				if (_range2 != NULL)
					return TOMVersion::TOM2;
				else
					return TOMVersion::TOM1;
			}
		}

#endif

		/// <summary>
		/// Gets a pointer to the underlying COM object.
		/// </summary>
		property IntPtr ComObject {
			IntPtr get() {
				return IntPtr(_range);
			}
		}

	protected:

		~TextRange() {
			this->!TextRange();
		}

		!TextRange() {
			if (_range2 != NULL) {
				_range2->Release();
				_range2 = NULL;
				_range = NULL;
			}
			else if (_range != NULL) {
				_range->Release();
				_range = NULL;
			}
		}

	private:

		ITextRange* _range;
		ITextRange2* _range2;

		virtual System::Object^ ICloneableClone() sealed = System::ICloneable::Clone {
			return Clone();
		}

		String^ ExtractText(::IDataObject* pData, CLIPFORMAT fmt, bool isUnicode) {
			HRESULT hr;
			String^ str = String::Empty;

			::FORMATETC fe;
			fe.cfFormat = fmt;
			fe.dwAspect = DVASPECT_CONTENT;
			fe.lindex = -1;
			fe.ptd = NULL;
			fe.tymed = TYMED_HGLOBAL;

			hr = pData->QueryGetData(&fe);
			THROW_HRESULT(hr);

			::STGMEDIUM stm;
			hr = pData->GetData(&fe, &stm);
					
			if (hr == S_OK) {
				LPSTR pszMsg = (LPSTR)GlobalLock(stm.hGlobal);
				IntPtr strPtr = IntPtr(pszMsg);
				str = (isUnicode) ? Marshal::PtrToStringUni(strPtr) : Marshal::PtrToStringAnsi(strPtr);						
				ReleaseStgMedium(&stm);
			}
				
			THROW_HRESULT(hr);

			return str;
		}

		long ToPositionFlags(ContentAlignment position) {
			switch (position) {
				case ContentAlignment::BottomCenter:
					return TA_BOTTOM + TA_CENTER;
				case ContentAlignment::BottomLeft:
					return TA_BOTTOM + TA_LEFT;
				case ContentAlignment::BottomRight:
					return TA_BOTTOM + TA_RIGHT;
				case ContentAlignment::MiddleCenter:
					return TA_BASELINE + TA_CENTER;
				case ContentAlignment::MiddleLeft:
					return TA_BASELINE + TA_LEFT;
				case ContentAlignment::MiddleRight:
					return TA_BASELINE + TA_RIGHT;
				case ContentAlignment::TopCenter:
					return TA_TOP + TA_CENTER;
				case ContentAlignment::TopLeft:
					return TA_TOP + TA_LEFT;
				case ContentAlignment::TopRight:
					return TA_TOP + TA_RIGHT;
			}

			throw gcnew ArgumentException("Invalid content alignment", "position");
		}

	};
} NAMESPACE_CLOSE

#undef THROW_IF_NOT_TOM2
