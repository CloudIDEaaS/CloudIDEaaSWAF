// TextSelection.h

#pragma once

#include "Stdafx.h"
#include "TextRange.h"
#include <TOM.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace NAMESPACE_DECL {

	/// <summary>
	/// Represents a text range with selection highlighting.
	/// </summary>
	/// <remarks>
#ifdef _TOM2
	/// Managed wrapper class for the COM interface ITextSelection, including ITextSelection2 functionality. 
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb774060(v=vs.85).aspx">ITextSelection interface</seealso>
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768717(v=vs.85).aspx">ITextSelection2 interface</seealso>
#else
	/// Managed wrapper class for the COM interface ITextSelection.
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb774060(v=vs.85).aspx">ITextSelection interface</seealso>
#endif
	/// </remarks>	
	public ref class TextSelection : public TextRange {

	public:
				
		/// <summary>
		/// Mimics the functionality of the End key. 
		/// </summary>
		/// <param name="ctrl">Whether to simulate the CTRL key (false = end of line, true = end of story).</param>
		/// <param name="extend">Flag that indicates how to change the selection.</param>
		/// <returns>The count of characters that the insertion point or the active end is moved.</returns>
		int EndKey(bool ctrl, RangeShiftType extend) {
			long actual;
			HRESULT hr = _selection->EndKey(ctrl ? tomStory : tomLine, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Gets or sets the text selection flags.
		/// </summary>
		property TextSelectionFlags Flags {
			TextSelectionFlags get() {
				long flags;
				HRESULT hr = _selection->GetFlags(&flags);
				THROW_HRESULT(hr);
				return (TextSelectionFlags)flags;
			}
			void set(TextSelectionFlags value) {
				HRESULT hr = _selection->SetFlags((long)value);
				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// Gets the type of text selection.
		/// </summary>
		property TextSelectionType Type {
			TextSelectionType get() {
				long type;
				HRESULT hr = _selection->GetType(&type);
				THROW_HRESULT(hr);
				return (TextSelectionType)type;
			}
		}

		/// <summary>
		/// Mimics the functionality of the Home key. 
		/// </summary>
		/// <param name="ctrl">Whether to simulate the CTRL key (false = start of line, true = start of story).</param>
		/// <param name="extend">Flag that indicates how to change the selection.</param>
		/// <returns>The count of characters that the insertion point or the active end is moved.</returns>
		int HomeKey(bool ctrl, RangeShiftType extend) {
			long actual;
			HRESULT hr = _selection->HomeKey(ctrl ? tomStory : tomLine, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Mimics the functionality of the Down Arrow and Page Down keys. 
		/// </summary>
		/// <param name="unit">Unit to use in the operation.</param>
		/// <param name="count">Number of Units to move past.</param>
		/// <param name="extend">Flag that indicates how to change the selection.</param>
		/// <returns>the actual count of units the insertion point or active end is moved.</returns>
		int MoveDown(TextUnit unit, int count, RangeShiftType extend) {
			long actual;
			HRESULT hr = _selection->MoveDown((int)unit, count, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Mimics the functionality of the Up Arrow and Page Up keys. 
		/// </summary>
		/// <param name="unit">Unit to use in the operation.</param>
		/// <param name="count">Number of Units to move past.</param>
		/// <param name="extend">Flag that indicates how to change the selection.</param>
		/// <returns>the actual count of units the insertion point or active end is moved.</returns>
		int MoveUp(TextUnit unit, int count, RangeShiftType extend) {
			long actual;
			HRESULT hr = _selection->MoveUp((int)unit, count, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Generalizes the functionality of the Left Arrow key. 
		/// </summary>
		/// <param name="unit">Unit to use in the operation.</param>
		/// <param name="count">Number of Units to move past.</param>
		/// <param name="extend">Flag that indicates how to change the selection.</param>
		/// <returns>the actual count of units the insertion point or active end is moved.</returns>
		int MoveLeft(TextUnit unit, int count, RangeShiftType extend) {
			long actual;
			HRESULT hr = _selection->MoveLeft((int)unit, count, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Generalizes the functionality of the Right Arrow key. 
		/// </summary>
		/// <param name="unit">Unit to use in the operation.</param>
		/// <param name="count">Number of Units to move past.</param>
		/// <param name="extend">Flag that indicates how to change the selection.</param>
		/// <returns>the actual count of units the insertion point or active end is moved.</returns>
		int MoveRight(TextUnit unit, int count, RangeShiftType extend) {
			long actual;
			HRESULT hr = _selection->MoveRight((int)unit, count, (int)extend, &actual);
			THROW_HRESULT(hr);
			return actual;
		}

		/// <summary>
		/// Types the string given by <paramref name="text"/> at this 
		/// selection as if someone typed it. This is similar to setting 
		/// the <see cref="Text"/> property, but is sensitive 
		/// to the Insert/Overtype key state and UI settings like 
		/// AutoCorrect and smart quotes.
		/// </summary>
		/// <param name="text">String to type into this selection.</param>
		void TypeText(String^ text) {
			if (text == nullptr) text = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(text).ToPointer();
			HRESULT hr = _selection->TypeText(bstr);
			SysFreeString(bstr);					
			THROW_HRESULT(hr);
		}

	internal:

		TextSelection(ITextSelection* selection) : TextRange(selection) {
			_selection = selection;
		}

#ifdef _TOM2

		TextSelection(ITextSelection2* selection) : TextRange(selection) {
			_selection = selection;
		}

#endif

	protected:

		~TextSelection() {
			this->!TextSelection();
		}

		!TextSelection() {
			_selection = NULL;
			_selection2 = NULL;
		}

	private:

		ITextSelection* _selection;
		ITextSelection2* _selection2;
	};
} NAMESPACE_CLOSE