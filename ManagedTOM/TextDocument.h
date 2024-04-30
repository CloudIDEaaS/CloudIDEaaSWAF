// TextDocument.h

#pragma once

#include "Stdafx.h"
#include <Windows.h>
#include <Richedit.h>
#include <TOM.h>
#include "PreferredFontInfo.h"
#include "DocumentProperties.h"
#include "MathProperties.h"
#include "TextFont.h"
#include "TextPara.h"
#include "TextRange.h"
#include "TextSelection.h"
#include "TextStory.h"
#include "TextStoryRanges.h"
#include "TextStrings.h"

using namespace System;
using namespace System::Drawing;
using namespace System::Windows::Forms;
using namespace System::Runtime::InteropServices;

// Throws NotSupportedException if interface pointer is not ITextDocument2
#define THROW_IF_NOT_TOM2() THROW_IF_NOT_INTERFACE(_doc2)

namespace NAMESPACE_DECL {

	/// <summary>
	/// Represents a top-level document in the Text Object Model (TOM).
	/// </summary>
	/// <remarks>
	/// <para>
#ifdef _TOM2
	/// Managed wrapper class for the COM interface ITextDocument, including ITextDocument2 functionality. 
	/// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb774052%28v=vs.85%29.aspx">ITextDocument interface</seealso>
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768436%28v=vs.85%29.aspx">ITextDocument2 interface</seealso>
#else
	/// Managed wrapper class for the COM interface ITextDocument. 
	/// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb774052%28v=vs.85%29.aspx">ITextDocument interface</seealso>
#endif
	/// </para>
	/// <para>
	/// You can obtain an instance <see cref="TextDocument"/> in a number of ways:
	/// </para>
	/// <list type="bullet">
	/// <item><description>
	/// From an instance of the Windows Forms <see cref="RichTextBox"/> control 
	/// (using the <see cref="FromRichTextBox"/> method). This is the preferred 
	/// method.
	/// </description></item>
	/// <item><description>
	/// From a native RichEdit control (by passing its window handle to the 
	/// <see cref="FromRichEditControl"/> method). Use this method if you need 
	/// to use a specific or custom version of the RichEdit control that you 
	/// have instantiated yourself.
	/// </description></item>
	/// <item><description>
	/// From a Runtime Callable Wrapper (RCW) for a COM object that implements 
	/// the ITextDocument interface (see <see cref="FromComObject"/> method).
	/// </description></item>
	/// <item><description>
	/// From an unmanaged pointer to the IUnknown interface of a COM object 
	/// that implements ITextDocument (see <see cref="FromIUnknown"/> method).
	/// </description></item>
	/// </list>
	/// </remarks>
	public ref class TextDocument {

	public:
				
		/// <summary>
		/// Creates a <see cref="TextDocument"/> instance from the specified 
		/// <see cref="RichTextBox"/> control.
		/// </summary>
		/// <param name="rtb">A <see cref="RichTextBox"/> control.</param>
		/// <param name="enableAdvancedTypography">Whether to enable advanced typography options in the control.</param>
		/// <returns><see cref="TextDocument"/> instance representing the rich text document in the control.</returns>
		/// <remarks>
		/// <para>
		/// The version of the underlying RichEdit control determines the 
		/// breadth of TOM functionality supported. Some methods and properties 
		/// may not be available. 
#ifdef _TOM2
		/// Use the <see cref="SupportedVersion"/> property to determine which 
		/// version of TOM the control supports.
#endif
		/// </para>
		/// <para>
		/// This method binds the <see cref="TextDocument"/> to the lifetime of 
		/// the control. All resources used by the object will be released when 
		/// the control is disposed.
		/// </para>
		/// </remarks>
		/// <example>
		/// The following example demonstrates how to obtain an instance of the 
		/// <see cref="TextDocument"/> class from a RichTextBox control: 
		/// <code source="..\Examples\TextDocument.cs" region="FromRichTextBox" language="cs" />
		/// </example>
		static TextDocument^ FromRichTextBox(RichTextBox^ rtb, [Optional] bool enableAdvancedTypography) {
			if (!rtb->Created) rtb->CreateControl();

			TextDocument^ textDocument = FromRichEditControl(rtb->Handle, enableAdvancedTypography);

			// disposing the control will also release any TextDocument instances created from it
			rtb->Disposed += gcnew EventHandler(textDocument, &TextDocument::RichTextBox_Disposed);

			return textDocument;
		}

		/// <summary>
		/// Creates a <see cref="TextDocument"/> instance from a RichEdit control.
		/// </summary>
		/// <param name="handle">Handle to the native RichEdit control..</param>
		/// <param name="enableAdvancedTypography">Whether to enable advanced typography options in the control.</param>
		/// <returns><see cref="TextDocument"/> instance representing the rich text document in the control.</returns>
		/// <remarks>
		/// <para>
		/// The version of the RichEdit control determines the breadth of TOM 
		/// functionality supported. Some methods and properties may not be 
		/// available. 
#ifdef _TOM2
		/// Use the <see cref="SupportedVersion"/> property to determine which 
		/// version of TOM the control supports.
#endif
		/// </para>
		/// </remarks>
		static TextDocument^ FromRichEditControl(IntPtr handle, [Optional] bool enableAdvancedTypography) {
			HWND hwnd = (HWND)handle.ToPointer();
			
			if (enableAdvancedTypography) {
				SendMessage(hwnd, EM_SETTYPOGRAPHYOPTIONS, TO_ADVANCEDTYPOGRAPHY, TO_ADVANCEDTYPOGRAPHY);
			}

			::IUnknown* punk = NULL;
			LRESULT lr = SendMessage(hwnd, EM_GETOLEINTERFACE, 0, (LPARAM)&punk);
			if (lr == 0) throw gcnew InvalidOperationException("The operation failed");

			::ITextDocument* doc = NULL;
			HRESULT hr1 = punk->QueryInterface(__uuidof(ITextDocument), (void**)&doc);
			
			::ITextDocument2* doc2 = NULL;

#ifdef _TOM2
			HRESULT hr2 = punk->QueryInterface(__uuidof(ITextDocument2), (void**)&doc2);			
#else
			HRESULT hr2 = E_FAIL;
#endif

			punk->Release();
			
			THROW_HRESULT(hr1);
			
			// create an instance using the appropriate interface pointer
			TextDocument^ textDocument = nullptr;

			if (SUCCEEDED(hr2))
				textDocument = gcnew TextDocument(doc2);
			else
				textDocument = gcnew TextDocument(doc);

			return textDocument;
		}

		/// <summary>
		/// Creates a <see cref="TextDocument"/> instance from the specified COM object.
		/// </summary>
		/// <param name="comObject">COM object implementing the ITextDocument interface.</param>
		/// <returns><see cref="TextDocument"/> instance derived from the COM object.</returns>
		/// <remarks>
		/// The <paramref name="comObject"/> parameter must be a Runtime 
		/// Callable Wrapper (RCW) for a COM object that implements the 
		/// ITextDocument interface.
		/// </remarks>
		static TextDocument^ FromComObject(Object^ comObject) {
			IntPtr pUnk = Marshal::GetIUnknownForObject(comObject);
			
			try {
				return FromIUnknown(pUnk);
			}
			finally {
				Marshal::Release(pUnk);
			}
		}

		/// <summary>
		/// Creates a <see cref="TextDocument"/> instance from a pointer to an IUnknown object.
		/// </summary>
		/// <param name="pUnk">Pointer to an IUnknown object.</param>
		/// <returns><see cref="TextDocument"/> instance derived from the IUnknown object.</returns>
		/// <remarks>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms680509(v=vs.85).aspx">IUnknown interface</seealso>
		/// </remarks>
		static TextDocument^ FromIUnknown(IntPtr pUnk) {
			::IUnknown* punk = (::IUnknown*)pUnk.ToPointer();

			::ITextDocument* doc = NULL;
			HRESULT hr1 = punk->QueryInterface(__uuidof(ITextDocument), (void**)&doc);
			
			::ITextDocument2* doc2 = NULL;

#ifdef _TOM2
			HRESULT hr2 = punk->QueryInterface(__uuidof(ITextDocument2), (void**)&doc2);			
#else
			HRESULT hr2 = E_FAIL;
#endif

			THROW_HRESULT(hr1);

			if (SUCCEEDED(hr2))
				return gcnew TextDocument(doc2);
			else
				return gcnew TextDocument(doc);
		}

		/// <summary>
		/// Gets or sets the default tab width.
		/// </summary>
		property float DefaultTabStop { 
			float get() {
				float ts;
				HRESULT hr = _doc->GetDefaultTabStop(&ts);
				THROW_HRESULT(hr);
				return ts;
			}
			void set (float value) {
				HRESULT hr = _doc->SetDefaultTabStop(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Returns a <see cref="TextRange"/> representing the entire document.
		/// </summary>
		property TextRange^ EntireRange {
			TextRange^ get() {
				if (_doc2 != NULL) {
					ITextRange2* range = NULL;
					HRESULT hr = _doc2->Range2(0, 0, &range);
					THROW_HRESULT(hr);
					range->MoveEnd(tomStory, 1, NULL);
					return gcnew TextRange(range);
				}
				else {
					ITextRange* range = NULL;
					HRESULT hr = _doc->Range(0, 0, &range);
					THROW_HRESULT(hr);
					range->MoveEnd(tomStory, 1, NULL);
					return gcnew TextRange(range);
				}
			}
		}
		/// <summary>
		/// Gets the file name of this document.
		/// </summary>
		property String^ Name {
			String^ get() {
				BSTR bstr = NULL;
				HRESULT hr = _doc->GetName(&bstr);
				THROW_HRESULT(hr);
				if (hr == S_FALSE) return String::Empty;
				String^ result = Marshal::PtrToStringBSTR(IntPtr(bstr));
				SysFreeString(bstr);
				return result;
			}
		}
		/// <summary>
		/// Gets or sets a value that indicates whether changes have been made since the file was last saved.
		/// </summary>
		property bool Saved {
			bool get() {
				long value;
				HRESULT hr = _doc->GetSaved(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
			void set(bool value) {
				HRESULT hr = _doc->SetSaved(value ? tomTrue : tomFalse);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets the active selection.
		/// </summary>
		property TextSelection^ Selection { 
			TextSelection^ get() {
				if (_doc2 != NULL) {
					ITextSelection2* sel = NULL;
					HRESULT hr = _doc2->GetSelection2(&sel);
					THROW_HRESULT(hr);
					return gcnew TextSelection(sel);
				}
				else {
					ITextSelection* sel = NULL;
					HRESULT hr = _doc->GetSelection(&sel);
					THROW_HRESULT(hr);
					return gcnew TextSelection(sel);
				}
			}
		}
		/// <summary>
		/// Gets a <see cref="TextStoryRanges"/> which enumerates through the stories in the document.
		/// </summary>
		property TextStoryRanges^ StoryRanges {			
			TextStoryRanges^ get() {
				HRESULT hr;
				long count;
				hr = _doc->GetStoryCount(&count);
				THROW_HRESULT(hr);

				if (count <= 1) {
					return gcnew TextStoryRanges(EntireRange);
				}
				else if (_doc2 != NULL) {
					ITextStoryRanges2* ranges = NULL;
					hr = _doc2->GetStoryRanges2(&ranges);
					THROW_HRESULT(hr);
					return gcnew TextStoryRanges(ranges);
				}
				else {
					ITextStoryRanges* ranges = NULL;
					hr = _doc->GetStoryRanges(&ranges);
					THROW_HRESULT(hr);
					return gcnew TextStoryRanges(ranges);
				}
			}
		}

		/// <summary>
		/// Increments the freeze count. If the freeze count is nonzero, screen updating is disabled.
		/// </summary>
		/// <remarks>
		/// Disabling screen updating will reduce flicker when performing 
		/// several consecutive operations on a range.
		/// </remarks>
		/// <example>
		/// The following example demonstrates how to disable and re-enable screen updating: 
		/// <code source="..\Examples\TextDocument.cs" region="BeginUpdate" language="cs" />
		/// </example>
		void BeginUpdate() {
			long count;
			HRESULT hr = _doc->Freeze(&count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Decrements the freeze count. If the freeze count goes to zero, screen updating is enabled.
		/// </summary>
		/// <example>
		/// The following example demonstrates how to disable and re-enable screen updating: 
		/// <code source="..\Examples\TextDocument.cs" region="BeginUpdate" language="cs" />
		/// </example>
		void EndUpdate() {
			long count;
			HRESULT hr = _doc->Unfreeze(&count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Turns on edit collection (also called undo grouping). 
		/// </summary>
		void BeginEditCollection() {
			HRESULT hr = _doc->BeginEditCollection();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Turns off edit collection (also called undo grouping). 
		/// </summary>
		void EndEditCollection() {
			HRESULT hr = _doc->EndEditCollection();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Suspends undo processing.
		/// </summary>
		void SuspendUndo() {
			long count;
			HRESULT hr = _doc->Undo(tomSuspend, &count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Resumes undo processing.
		/// </summary>
		void ResumeUndo() {
			long count;
			HRESULT hr = _doc->Undo(tomResume, &count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Performs a specified number of undo operations.
		/// </summary>
		/// <param name="count">The specified number of undo operations.</param>
		/// <returns>The actual count of undo operations performed.</returns>
		int Undo(int count) {
			if (count < 1) throw gcnew ArgumentOutOfRangeException();
			long actual;
			HRESULT hr = _doc->Undo(count, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Performs a specified number of redo operations.
		/// </summary>
		/// <param name="count">The specified number of redo operations.</param>
		/// <returns>The actual count of redo operations performed.</returns>
		int Redo(int count) {
			if (count < 1) throw gcnew ArgumentOutOfRangeException();
			long actual;
			HRESULT hr = _doc->Redo(count, &actual);
			THROW_HRESULT(hr);
			return actual;
		}				

		/// <summary>
		/// Opens a specified document.
		/// </summary>
		/// <param name="fileName">Path and filename of the document to open.</param>
		/// <param name="mode">Mutually exclusive options for opening the document.</param>
		/// <param name="flags">Additional options for opening the document.</param>
		/// <param name="codePage">The code page to use for the file (0=auto).</param>
		void Open(String^ fileName, [Optional] TextOpenSave mode, [Optional] TextOpenFlags flags, [Optional] int codePage) {
			if (fileName == nullptr) fileName = String::Empty;
			
			VARIANT var;
			VariantInit(&var);
			
			var.vt = VT_BSTR;
			var.bstrVal = (BSTR)Marshal::StringToBSTR(fileName).ToPointer();
			
			HRESULT hr = _doc->Open(&var, (long)mode | (long)flags, codePage);
			
			Marshal::FreeBSTR(IntPtr(var.bstrVal));
			
			switch (hr & ~0x40000) {
				case STG_E_FILEALREADYEXISTS:
					throw gcnew IOException("File already exists.", hr);
				case STG_E_ACCESSDENIED:
					throw gcnew IOException("Access denied.", hr);
				case STG_E_FILENOTFOUND:
					throw gcnew IOException("File not found.", hr);
				case STG_E_INUSE:
					throw gcnew IOException("File already in use.", hr);
				case STG_E_INVALIDNAME:
					throw gcnew IOException("Invalid name.", hr);
				case STG_E_PATHNOTFOUND:
					throw gcnew IOException("Path not found.", hr);
				case STG_E_SHAREVIOLATION:
					throw gcnew IOException("Sharing violation.", hr);
			}

			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Opens a new document.
		/// </summary>
		void New() {
			HRESULT hr = _doc->New();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Retrieves a text range object for a specified range of content in the active story of the document.
		/// </summary>
		/// <param name="start">Character position for the start of the range.</param>
		/// <param name="length">Length of the range.</param>
		TextRange^ Range(int start, int length) {
			if (_doc2 != NULL) {
				ITextRange2* range = NULL;
				HRESULT hr = _doc2->Range2(start, start + length, &range);
				THROW_HRESULT(hr);
				return gcnew TextRange(range);
			}
			else {
				ITextRange* range = NULL;
				HRESULT hr = _doc->Range(start, start + length, &range);
				THROW_HRESULT(hr);
				return gcnew TextRange(range);
			}
		}

		/// <summary>
		/// Retrieves a range for the content at or nearest to the specified point on the screen.
		/// </summary>
		/// <param name="p">The horizontal and vertical coordinates of the specified point, in screen coordinates.</param>
		TextRange^ RangeFromPoint(Point p) {
			if (_doc2 != NULL) {
				ITextRange2* range = NULL;
				HRESULT hr = _doc2->RangeFromPoint2(p.X, p.Y, (tomStart + TA_BASELINE + TA_LEFT), &range);
				THROW_HRESULT(hr);
				return gcnew TextRange(range);
			}
			else {
				ITextRange* range = NULL;
				HRESULT hr = _doc->RangeFromPoint(p.X, p.Y, &range);
				THROW_HRESULT(hr);
				return gcnew TextRange(range);
			}
		}
		
		/// <summary>
		/// Saves the document.
		/// </summary>
		/// <param name="fileName">Path and filename of the save target. Specify null to overwrite an existing document.</param>
		/// <param name="mode">Mutually exclusive options for saving the document.</param>
		/// <param name="flags">Additional options for saving the document.</param>
		/// <param name="codePage">The code page to use for the file (0=auto).</param>
		void Save(String^ fileName, [Optional] TextOpenSave mode, [Optional] TextSaveFlags flags, [Optional] int codePage) {
			VARIANT var;
			VARIANT* varPtr;
			
			if (String::IsNullOrEmpty(fileName)) {
				varPtr = NULL;
			}
			else {
				VariantInit(&var);			
				var.vt = VT_BSTR;
				var.bstrVal = (BSTR)Marshal::StringToBSTR(fileName).ToPointer();
				varPtr = &var;
			}

			HRESULT hr = _doc->Save(varPtr, (long)mode | (long)flags, codePage);
			
			if (varPtr != NULL) Marshal::FreeBSTR(IntPtr(var.bstrVal));
			
			switch (hr & ~0x40000) {
				case STG_E_FILEALREADYEXISTS:
					throw gcnew IOException("File already exists.", hr);
				case STG_E_ACCESSDENIED:
					throw gcnew IOException("Access denied.", hr);
				case STG_E_FILENOTFOUND:
					throw gcnew IOException("File not found.", hr);
				case STG_E_INUSE:
					throw gcnew IOException("File already in use.", hr);
				case STG_E_INVALIDNAME:
					throw gcnew IOException("Invalid name.", hr);
				case STG_E_PATHNOTFOUND:
					throw gcnew IOException("Path not found.", hr);
				case STG_E_SHAREVIOLATION:
					throw gcnew IOException("Sharing violation.", hr);
				case STG_E_DISKISWRITEPROTECTED:
					throw gcnew IOException("Disk is write-protected.", hr);
				case STG_E_MEDIUMFULL:
					throw gcnew IOException("Insufficient storage space.", hr);
			}

			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Returns the name of the document, or &quot;Document&quot; if <see cref="Name"/> is empty.
		/// </summary>
		virtual String^ ToString() override {
			String^ s = Name;
			if (String::IsNullOrEmpty(s))
				return "Document";
			else
				return s;
		}

#ifdef _TOM2

		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the active story; that is, the story that receives keyboard and mouse input.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is null.
		/// </exception>
		property TextStory^ ActiveStory {
			TextStory^ get() {
				THROW_IF_NOT_TOM2();

				ITextStory* story = NULL;
				HRESULT hr = _doc2->GetActiveStory(&story);
				THROW_HRESULT(hr);

				return gcnew TextStory(story);
			}
			void set(TextStory^ value) {
				THROW_IF_NOT_TOM2();
				if (value == nullptr) throw gcnew ArgumentNullException("value");

				HRESULT hr = _doc2->SetActiveStory((ITextStory*)value->ComObject.ToPointer());
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets a value indicating whether advanced typography 
		/// (special line breaking and line formatting) is turned on.
		/// </summary>
		/// <remarks>
		/// If TOM 2 is not supported by the current instance, advanced 
		/// typography can be enabled when the instance is first obtained via 
		/// the <see cref="FromRichTextBox"/> method.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property bool AdvancedTypographyEnabled {
			bool get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _doc2->GetTypographyOptions(&value);
				THROW_HRESULT(hr);

				return (value == TO_ADVANCEDTYPOGRAPHY);
			}
			void set(bool value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _doc2->SetTypographyOptions(
					value ? TO_ADVANCEDTYPOGRAPHY : TO_SIMPLELINEBREAK, 
					TO_ADVANCEDTYPOGRAPHY | TO_SIMPLELINEBREAK
				);

				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the caret type.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property NAMESPACE::CaretType CaretType {
			NAMESPACE::CaretType get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _doc2->GetCaretType(&value);
				THROW_HRESULT(hr);

				return (NAMESPACE::CaretType)value;
			}
			void set(NAMESPACE::CaretType value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _doc2->SetCaretType((long)value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets a <see cref="TextFont"/> that provides the default character format information for this instance of the TOM engine.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is null.
		/// </exception>
		property TextFont^ DocumentFont {
			TextFont^ get() {
				THROW_IF_NOT_TOM2();

				ITextFont2* font = NULL;
				HRESULT hr = _doc2->GetDocumentFont(&font);
				THROW_HRESULT(hr);

				return gcnew TextFont(font);
			}
			void set(TextFont^ value) {
				THROW_IF_NOT_TOM2();
				if (value == nullptr) throw gcnew ArgumentNullException("value");

				HRESULT hr = _doc2->SetDocumentFont((ITextFont2*)value->ComObject.ToPointer());
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets a <see cref="TextPara"/> that provides the default paragraph format information for this instance of the TOM engine.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is null.
		/// </exception>
		property TextPara^ DocumentPara {
			TextPara^ get() {
				THROW_IF_NOT_TOM2();

				ITextPara2* para = NULL;
				HRESULT hr = _doc2->GetDocumentPara(&para);
				THROW_HRESULT(hr);

				return gcnew TextPara(para);
			}
			void set(TextPara^ value) {
				THROW_IF_NOT_TOM2();
				if (value == nullptr) throw gcnew ArgumentNullException("value");

				HRESULT hr = _doc2->SetDocumentPara((ITextPara2*)value->ComObject.ToPointer());
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets the East Asian flags.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property NAMESPACE::EastAsianFlags EastAsianFlags {
			NAMESPACE::EastAsianFlags get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _doc2->GetEastAsianFlags(&value);
				THROW_HRESULT(hr);

				return (NAMESPACE::EastAsianFlags)value;
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets the name of the TOM engine.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property String^ Generator {
			String^ get() {
				THROW_IF_NOT_TOM2();

				BSTR bstr;
				HRESULT hr = _doc2->GetGenerator(&bstr);
				THROW_HRESULT(hr);
				String^ s = Marshal::PtrToStringBSTR(IntPtr(bstr));
				SysFreeString(bstr);

				// truncate after first null terminator
				array<Char>^ chars = { '\0' };				
				return s->Split(chars)[0];
			}
		}
		/// <summary>
		/// (TOM 2 only) Gets the main story.
		/// </summary>
		/// <remarks>
		/// A rich edit control automatically includes the main story; a call to the <see cref="GetNewStory"/> method is not required.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property TextStory^ MainStory {
			TextStory^ get() {
				THROW_IF_NOT_TOM2();

				ITextStory* story = NULL;
				HRESULT hr = _doc2->GetMainStory(&story);
				THROW_HRESULT(hr);

				return gcnew TextStory(story);
			}
		}
		/// <summary>
		/// Provides access to the math properties for the document.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property NAMESPACE::MathProperties^ MathProperties {
			NAMESPACE::MathProperties^ get() {
				return _mathProps;
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the notification mode.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property bool NotificationMode {
			bool get() {
				THROW_IF_NOT_TOM2();

				long value;
				HRESULT hr = _doc2->GetNotificationMode(&value);
				THROW_HRESULT(hr);

				return value == tomTrue;
			}
			void set(bool value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _doc2->SetNotificationMode(value ? tomTrue : tomFalse);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Provides access to the properties of the document.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property DocumentProperties^ Properties {
			DocumentProperties^ get() {
				return _docProps;
			}
		}
		/// <summary>
		/// Gets a value indicating which version of the Text Object Model (TOM) 
		/// is supported by the current document.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property TOMVersion SupportedVersion {
			TOMVersion get() {
				if (_doc2 != NULL)
					return TOMVersion::TOM2;
				else
					return TOMVersion::TOM1;
			}
		}
		/// <summary>
		/// (TOM 2 only) 
		/// Gets the handle of the window that the TOM engine is using to display output.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		property IntPtr WindowHandle {
			IntPtr get() {
				THROW_IF_NOT_TOM2();

				__int64 value;
				HRESULT hr = _doc2->GetWindow(&value);
				THROW_HRESULT(hr);
				
				return IntPtr(value);
			}
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Attaches a new message filter to the edit instance. 
		/// All window messages that the edit instance receives are forwarded to the message filter.
		/// </summary>
		/// <param name="filter">The message filter.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void AttachMsgFilter(Object^ filter) {
			THROW_IF_NOT_TOM2();

			IUnknown* pUnk = (IUnknown*)Marshal::GetIUnknownForObject(filter).ToPointer();
			HRESULT hr = _doc2->AttachMsgFilter(pUnk);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Checks whether the number of characters to be added would exceed the maximum text limit.
		/// </summary>
		/// <param name="count">The number of characters to be added.</param>
		/// <returns>The number of characters that exceed the maximum text limit. This parameter is 0 if the number of characters does not exceed the limit.</returns>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		int CheckTextLimit(int count) {
			THROW_IF_NOT_TOM2();

			long result;
			HRESULT hr = _doc2->CheckTextLimit(count, &result);
			THROW_HRESULT(hr);

			return result;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Retrieves the client rectangle of the rich edit control.
		/// </summary>
		/// <param name="flags">The client rectangle retrieval options.</param>
		/// <remarks>
		/// The bounds of the rectangle are expressed in terms of pixels.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		System::Drawing::Rectangle GetClientRectangle([Optional] ClientRectangleFlags flags) {
			THROW_IF_NOT_TOM2();
			
			long left, top, right, bottom;
			HRESULT hr = _doc2->GetClientRect((long)flags, &left, &top, &right, &bottom);
			THROW_HRESULT(hr);

			return System::Drawing::Rectangle::FromLTRB(left, top, right, bottom); 
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Retrieves the color used for special text attributes.
		/// </summary>
		/// <param name="index">The index of the color to retrieve.</param>
		/// <returns>The color that corresponds to the specified index.</returns>
		/// <remarks>
		/// Indexes in the range [1-16] are for special underline colors, and 
		/// can be redefined with the <see cref="SetEffectColor"/> method.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		Color GetEffectColor(int index) {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _doc2->GetEffectColor(index, &value);
			
			if (FAILED(hr) || (value == tomAutoColor) || (value == tomUndefined)) 
				return Color::Empty;
			else
				return ColorTranslator::FromWin32(value);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Specifies the color to use for special text attributes.
		/// </summary>
		/// <param name="index">The index of the color to retrieve.</param>
		/// <param name="value">The new color for the specified index.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void SetEffectColor(int index, Color value) {
			THROW_IF_NOT_TOM2();
			
			HRESULT hr = _doc2->SetEffectColor(index, ColorTranslator::ToWin32(value));
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets a new story.
		/// </summary>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the document does not support this operation.
		/// </exception>
		TextStory^ GetNewStory() {
			THROW_IF_NOT_TOM2();

			ITextStory* story = NULL;
			HRESULT hr = _doc2->GetNewStory(&story);
			THROW_HRESULT(hr);

			return gcnew TextStory(story);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the displays collection for this Text Object Model (TOM) engine instance.
		/// </summary>
		/// <returns>The displays collection, which implements the COM interface ITextDisplays.</returns>
		/// <remarks>
		/// The rich edit control doesn't implement this method. 
		/// The ITextDisplays interface is currently undefined.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the document does not support this operation.
		/// </exception>
		Object^ GetDisplays() {
			THROW_IF_NOT_TOM2();

			ITextDisplays* displays = NULL;
			HRESULT hr = _doc2->GetDisplays(&displays);
			THROW_HRESULT(hr);

			return Marshal::GetObjectForIUnknown(IntPtr(displays));
		}

		/// <summary>
		/// (TOM 2 only)
		/// Retrieves the preferred font for a particular character repertoire and character position.
		/// </summary>
		/// <param name="index">The character position for the preferred font.</param>
		/// <param name="rep">The character repertoire index for the preferred font.</param>
		/// <param name="options">The preferred font options.</param>
		/// <param name="curRep">The index of the current character repertoire.</param>
		/// <param name="curFontSize">The current font size.</param>
		/// <returns>A <see cref="PreferredFontInfo"/> object containing information about the font.</returns>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		PreferredFontInfo^ GetPreferredFont(int index, CharRepertoire rep, PreferredFontFlags options, CharRepertoire curRep, int curFontSize) {
			THROW_IF_NOT_TOM2();
			
			HRESULT hr;
			BSTR bstr;
			long pitchAndFamily;
			long fontSize;

			hr = _doc2->GetPreferredFont(index, (long)rep, (long)options, (long)curRep, curFontSize, &bstr, &pitchAndFamily, &fontSize);
			THROW_HRESULT(hr);

			String^ s = Marshal::PtrToStringBSTR(IntPtr(bstr));
			SysFreeString(bstr);

			return gcnew PreferredFontInfo(s, pitchAndFamily, fontSize);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Retrieves the story that corresponds to a particular index.
		/// </summary>
		/// <param name="index">The 0-based index of the story to retrieve.</param>
		/// <remarks>
		/// The number of stories in the document can be found via the 
		/// <see cref="StoryRanges"/> property.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the document does not support this operation.
		/// </exception>
		TextStory^ GetStory(int index) {
			THROW_IF_NOT_TOM2();

			HRESULT hr;
				
			long count;
			hr = _doc->GetStoryCount(&count);
			THROW_HRESULT(hr);

			if ((index < 0) || (index >= count)) throw gcnew ArgumentOutOfRangeException("index");

			if (count <= 1) {
				return ActiveStory;
			}
			else {
				ITextStory* story = NULL;
				hr = _doc2->GetStory(index + 1, &story);
				THROW_HRESULT(hr);
				return gcnew TextStory(story);
			}
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Notifies the TOM engine client of particular IME events.
		/// </summary>
		/// <param name="imeNotificationCode">An IME notification code.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void Notify(int imeNotificationCode) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _doc2->Notify(imeNotificationCode);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Updates the selection and caret.
		/// </summary>
		/// <param name="scrollToCaret">Whether to scroll the caret into view.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void Update([Optional] bool scrollToCaret) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _doc2->Update(scrollToCaret ? tomTrue : tomFalse);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Notifies the client that the view has changed and the client should update the view if the TOM engine is in-place active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void UpdateWindow() {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _doc2->UpdateWindow();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Sets the state of the IME in-progress flag.
		/// </summary>
		/// <param name="value">Whether to turn on the IME in-progress flag.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void SetIMEInProgress(bool value) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _doc2->SetIMEInProgress(value ? tomTrue : tomFalse);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets a new collection of rich-text strings.
		/// </summary>
		/// <remarks>
		/// The collection initially contains one empty string.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		TextStrings^ GetStrings() {
			THROW_IF_NOT_TOM2();

			ITextStrings* strings = NULL;
			HRESULT hr = _doc2->GetStrings(&strings);
			THROW_HRESULT(hr);
			return gcnew TextStrings(strings);
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Generates a system beep.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextDocument2 interface.
		/// </exception>
		void SysBeep() {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _doc2->SysBeep();
			THROW_HRESULT(hr);
		}

#endif

	internal:

		TextDocument(ITextDocument* doc) {
			_doc = doc;
			_doc2 = NULL;
#ifdef _TOM2
			_docProps = gcnew NAMESPACE::DocumentProperties(this);
			_mathProps = gcnew NAMESPACE::MathProperties(this);
#endif
		}

#ifdef _TOM2

		TextDocument(ITextDocument2* doc) {
			_doc = doc;
			_doc2 = doc;
			_docProps = gcnew DocumentProperties(this);
			_mathProps = gcnew NAMESPACE::MathProperties(this);
		}

		int GetProperty(int type) {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _doc2->GetProperty(type, &value);
			THROW_HRESULT(hr);

			return value;
		}

		void SetProperty(int type, int value) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _doc2->SetProperty(type, value);
			THROW_HRESULT(hr);
		}

		int GetMathProperties() {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _doc2->GetMathProperties(&value);
			THROW_HRESULT(hr);

			return value;
		}

		void SetMathProperty(int mask, int value) {
			THROW_IF_NOT_TOM2();
				
			HRESULT hr = _doc2->SetMathProperties((long)value, (long)mask);
			THROW_HRESULT(hr);
		}

#endif

	protected:

		~TextDocument() {
#ifdef _TOM2
			delete _docProps;
			delete _mathProps;
#endif
			this->!TextDocument();
		}

		!TextDocument() {
			if (_doc2 != NULL) {
				// there is only one object with 2 pointers
				_doc2->Release();
				_doc2 = NULL;
				_doc = NULL;
			}
			else if (_doc != NULL) {
				_doc->Release();
				_doc = NULL;
			}
		}

	private:

		void RichTextBox_Disposed(Object^ sender, EventArgs^ e) {
			delete this;
		}

		ITextDocument* _doc;				
		ITextDocument2* _doc2;

#ifdef _TOM2

		DocumentProperties^ _docProps;
		NAMESPACE::MathProperties^ _mathProps;

#endif

	};
} NAMESPACE_CLOSE

#undef THROW_IF_NOT_TOM2
