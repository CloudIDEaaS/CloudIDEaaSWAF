// TextStrings.h

#pragma once
#ifdef _TOM2

#include "Stdafx.h"
#include <Windows.h>
#include <Richedit.h>
#include <TOM.h>

// Throws NotSupportedException if interface pointer is not ITextRange2
#define THROW_IF_NOT_TOM2() THROW_IF_NOT_INTERFACE(_range2)

namespace NAMESPACE_DECL {

	/// <summary>
	/// (TOM 2 only) 
	/// Represents a collection of rich-text strings that are useful for manipulating rich text.
	/// </summary>
	/// <remarks>
	/// Managed wrapper class for the COM interface ITextStrings.
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768736(v=vs.85).aspx">ITextStrings interface</seealso>
	/// </remarks>
	public ref class TextStrings : public IDisposable {

	public:

		/// <summary>
		/// Gets a <see cref="TextRange"/> for the selected index in the string collection.
		/// </summary>
		[System::Runtime::CompilerServices::IndexerName("Item")]		
		property TextRange^ default[int] {
			TextRange^ get(int index) {
				ITextRange2* range = NULL;
				HRESULT hr = _strings->Item(index + 1, &range);
				THROW_HRESULT(hr);
				return gcnew TextRange(range);
			}
		}
		/// <summary>
		/// Gets the number of strings in the string collection.
		/// </summary>
		property int Count {
			int get() {
				long value;
				HRESULT hr = _strings->GetCount(&value);
				THROW_HRESULT(hr);
				return value;
			}
		}

		/// <summary>
		/// Adds a string to the end of the collection.
		/// </summary>
		/// <param name="str">The string.</param>
		void Add(String^ str) {
			if (str == nullptr) str = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(str).ToPointer();
			HRESULT hr = _strings->Add(bstr);
			SysFreeString(bstr);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Appends a string to the string at the specified index in the collection.
		/// </summary>
		/// <param name="range">Range containing the string to append.</param>
		/// <param name="index">The string index.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="range"/> is null.
		/// </exception>
		void Append(TextRange^ range, int index) {
			if (range == nullptr) throw gcnew ArgumentNullException("range");
			HRESULT hr = _strings->Append((ITextRange2*)range->ComObject.ToPointer(), index);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Concatenates two strings.
		/// </summary>
		/// <param name="index">The string index.</param>
		void Cat2(int index) {
			HRESULT hr = _strings->Cat2(index);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Inserts text between the top two strings in a collection.
		/// </summary>
		/// <param name="str">The string.</param>
		void CatTop2(String^ str) {
			if (str == nullptr) str = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(str).ToPointer();
			HRESULT hr = _strings->CatTop2(bstr);
			SysFreeString(bstr);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Deletes the contents of a given range.
		/// </summary>
		/// <param name="range">The range to delete.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="range"/> is null.
		/// </exception>
		void DeleteRange(TextRange^ range) {
			if (range == nullptr) throw gcnew ArgumentNullException("range");
			HRESULT hr = _strings->DeleteRange((ITextRange2*)range->ComObject.ToPointer());
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Encodes an object, given a set of properties.
		/// </summary>
		/// <param name="properties">The object properties.</param>
		/// <param name="range">The specifying range that points at the desired formatting.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="range"/> is null.
		/// </exception>
		void EncodeFunction(InlineObjectProperties properties, TextRange^ range) {
			if (range == nullptr) throw gcnew ArgumentNullException("range");
			HRESULT hr = _strings->EncodeFunction(
				(long)properties.Type,
				Convert::ToInt32(properties.Align),
				(long)properties.Char,
				(long)properties.Char1,
				(long)properties.Char2,
				properties.ArgCount,
				(long)properties.TeXStyle,
				properties.ColCount,
				(ITextRange2*)range->ComObject.ToPointer()
			);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Gets the count of characters for a selected string index.
		/// </summary>
		/// <param name="index">The string index.</param>
		int GetCharacterCount(int index) {
			long count;
			HRESULT hr = _strings->GetCch(index, &count);
			THROW_HRESULT(hr);
			return count;
		}

		/// <summary>
		/// Inserts a null string in the collection at the specified string index.
		/// </summary>
		/// <param name="index">The string index.</param>
		void InsertNullStr(int index) {
			HRESULT hr = _strings->InsertNullStr(index);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Moves the start boundary of a string, by index, by the specified number of characters.
		/// </summary>
		/// <param name="index">The string index.</param>
		/// <param name="count">The selected number of characters to move the boundary.</param>
		void MoveBoundary(int index, int count) {
			HRESULT hr = _strings->MoveBoundary(index, count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Prefixes a string to the top string in the collection.
		/// </summary>
		/// <param name="str">The string to prefix to the collection.</param>
		void PrefixTop(String^ str) {
			if (str == nullptr) str = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(str).ToPointer();
			HRESULT hr = _strings->PrefixTop(bstr);
			SysFreeString(bstr);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Removes one or more strings from the string collection, starting at the specified index.
		/// </summary>
		/// <param name="index">The string index.</param>
		/// <param name="count">The count of strings to remove.</param>
		void Remove(int index, int count) {
			HRESULT hr = _strings->Remove(index, count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Removes a string from the string collection at the specified index.
		/// </summary>
		/// <param name="index">The string index.</param>
		void Remove(int index) {
			HRESULT hr = _strings->Remove(index, 1);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Replaces text with formatted text.
		/// </summary>
		/// <param name="oldRange">The text to be replaced.</param>
		/// <param name="newRange">The formatted text.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="oldRange"/> is null, or <paramref name="newRange"/> is null.
		/// </exception>
		void SetFormattedText(TextRange^ oldRange, TextRange^ newRange) {
			if (oldRange == nullptr) throw gcnew ArgumentNullException("oldRange");
			if (newRange == nullptr) throw gcnew ArgumentNullException("newRange");
			
			HRESULT hr = _strings->SetFormattedText(
				(ITextRange2*)oldRange->ComObject.ToPointer(),
				(ITextRange2*)newRange->ComObject.ToPointer()
			);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Sets the character position in the source range's story that has the desired character formatting attributes.
		/// </summary>
		/// <param name="index">The index of the string to associate with a character position.</param>
		/// <param name="charPos">The character position in the source range's story that has the desired character formatting.</param>
		/// <remarks>
		/// The <see cref="EncodeFunction"/> method applies these character formatting attributes to the operators.
		/// </remarks>
		void SetOpCp(int index, int charPos) {
			HRESULT hr = _strings->SetOpCp(index, charPos);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Suffixes a string to the top string in the collection.
		/// </summary>
		/// <param name="str">The string to suffix to the collection.</param>
		/// <param name="range">The range with the desired character formatting.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="range"/> is null.
		/// </exception>
		void SuffixTop(String^ str, TextRange^ range) {
			if (range == nullptr) throw gcnew ArgumentNullException("range");
			if (str == nullptr) str = String::Empty;
			BSTR bstr = (BSTR)Marshal::StringToBSTR(str).ToPointer();
			HRESULT hr = _strings->SuffixTop(bstr, (ITextRange2*)range->ComObject.ToPointer());
			SysFreeString(bstr);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Swaps the top two strings in the collection.
		/// </summary>
		void Swap() {
			HRESULT hr = _strings->Swap();
			THROW_HRESULT(hr);
		}

	internal:

		TextStrings(ITextStrings* strings) {
			_strings = strings;
		}

	protected:

		~TextStrings() {
			this->!TextStrings();
		}

		!TextStrings() {
			if (_strings != NULL) {
				_strings->Release();
				_strings = NULL;
			}
		}

	private:

		ITextStrings* _strings;

	};
} NAMESPACE_CLOSE

#undef THROW_IF_NOT_TOM2
#endif
