// TextStory.h

#pragma once
#ifdef _TOM2

#include "Stdafx.h"
#include <Windows.h>
#include <Richedit.h>
#include <TOM.h>
#include "TextRange.h"

namespace NAMESPACE_DECL {

	/// <summary>
	/// (TOM 2 only)
	/// Represents a story in a document.
	/// </summary>
	/// <remarks>
	/// Managed wrapper class for the COM interface ITextStory. 
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768721(v=vs.85).aspx">ITextStory interface</seealso>
	/// </remarks>
	public ref class TextStory {

	public:

		/// <summary>
		/// Gets or sets the active state of the story.
		/// </summary>
		property StoryActiveState Active {
			StoryActiveState get() {
				long value;
				HRESULT hr = _story->GetActive(&value);
				THROW_HRESULT(hr);

				return (StoryActiveState)value;
			}
			void set(StoryActiveState value) {
				HRESULT hr = _story->SetActive((long)value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets the index of the story.
		/// </summary>
		property int Index {
			int get() {
				long value;
				HRESULT hr = _story->GetIndex(&value);
				THROW_HRESULT(hr);

				return value;
			}
		}
		/// <summary>
		/// Gets or sets the plain text in the story, using the default conversion options.
		/// </summary>
		property String^ Text {
			String^ get() {
				return GetText(PlainTextFlags::Default);
			}
			void set(String^ value) {
				SetText(value, RichTextFlags::Default);
			}
		}
		/// <summary>
		/// Gets or sets this story's type.
		/// </summary>
		property StoryType Type {
			StoryType get() {
				long value;
				HRESULT hr = _story->GetType(&value);
				THROW_HRESULT(hr);

				return (StoryType)value;
			}
			void set(StoryType value) {
				HRESULT hr = _story->SetType((long)value);
				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// Gets a new display for a story.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the story does not support this operation.
		/// </exception>
		Object^ GetDisplay() {
			IUnknown* pUnk = NULL;
			HRESULT hr = _story->GetDisplay(&pUnk);
			THROW_HRESULT(hr);
			return Marshal::GetObjectForIUnknown(IntPtr(pUnk));
		}

		/// <summary>
		/// Gets the value of the specified property.
		/// </summary>
		/// <param name="type">The property ID.</param>
		int GetProperty(int type) {
			long value;
			HRESULT hr = _story->GetProperty(type, &value);
			THROW_HRESULT(hr);

			return value;
		}

		/// <summary>
		/// Sets the value of the specified property.
		/// </summary>
		/// <param name="type">The property ID.</param>
		/// <param name="value">The new property value.</param>
		void SetProperty(int type, int value) {
			HRESULT hr = _story->SetProperty(type, value);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Gets a <see cref="TextRange"/> for the story.
		/// </summary>
		TextRange^ GetRange(int index, int length) {
			ITextRange2* range = NULL;
			HRESULT hr = _story->GetRange(index, index + length, &range);
			THROW_HRESULT(hr);
			return gcnew TextRange(range);
		}

		/// <summary>
		/// Gets the plain text in the story according to the specified conversion options.
		/// </summary>
		String^ GetText([Optional] PlainTextFlags options) {
			BSTR bstr;
			HRESULT hr = _story->GetText((long)options, &bstr);
			THROW_HRESULT(hr);
			String^ s = Marshal::PtrToStringBSTR(IntPtr(bstr));
			SysFreeString(bstr);

			return s;
		}

		/// <summary>
		/// Sets the plain text in the story according to the specified conversion options.
		/// </summary>
		void SetText(String^ value, [Optional] RichTextFlags options) {
			if (value == nullptr) value = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(value).ToPointer();
			HRESULT hr = _story->SetText((long)options, bstr);
			THROW_HRESULT(hr);
			SysFreeString(bstr);
		}

		/// <summary>
		/// Replaces the story's text with the specified formatted text.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="range"/> is null.
		/// </exception>
		void SetFormattedText(TextRange^ range) {
			if (range == nullptr) throw gcnew ArgumentNullException("range");

			HRESULT hr = _story->SetFormattedText((IUnknown*)range->ComObject.ToPointer());
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Returns the plain text in the story, using the default conversion options.
		/// </summary>
		virtual String^ ToString() override {
			return GetText(PlainTextFlags::Default);
		}

	internal:

		TextStory(ITextStory* story) {
			_story = story;
		}

		/// <summary>
		/// Gets a pointer to the underlying COM object.
		/// </summary>
		property IntPtr ComObject {
			IntPtr get() {
				return IntPtr(_story);
			}
		}

	protected:

		~TextStory() {
			this->!TextStory();
		}

		!TextStory() {
			if (_story != NULL) {
				_story->Release();
				_story = NULL;
			}
		}

	private:

		ITextStory* _story;

	};
} NAMESPACE_CLOSE

#endif