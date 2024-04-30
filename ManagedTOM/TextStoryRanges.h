// TextStoryRanges.h

#pragma once

#include "Stdafx.h"
#include <Windows.h>
#include <Richedit.h>
#include <TOM.h>

using namespace System::Collections::Generic;

namespace NAMESPACE_DECL {

	ref class TextDocument;
	ref class TextRange;

	/// <summary>
	/// Enumerates through the stories in a <see cref="TextDocument"/>.
	/// </summary>
	/// <remarks>
#ifdef _TOM2
	/// Managed wrapper class for the COM interface ITextStoryRanges, including ITextStoryRanges2 functionality. 
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb774062(v=vs.85).aspx">ITextStoryRanges interface</seealso>
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768722(v=vs.85).aspx">ITextStoryRanges2 interface</seealso>
#else
	/// Managed wrapper class for the COM interface ITextStoryRanges. 
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb774062(v=vs.85).aspx">ITextStoryRanges interface</seealso>
#endif
	/// </remarks>
	public ref class TextStoryRanges : public IDisposable, 
		public IEnumerable<TextRange^>, 
		public IReadOnlyCollection<TextRange^>, 
		public IReadOnlyList<TextRange^> 
	{

	public:

		/// <summary>
		/// Gets an object that enumerates through the collection.
		/// </summary>
		virtual IEnumerator<TextRange^>^ GetEnumerator() {
			return gcnew StoryEnumerator(this);
		}

		/// <summary>
		/// Gets the number of stories in the document.
		/// </summary>
		virtual property int Count {
			int get() {
				if (_singleStory != nullptr) {
					return 1;
				}
				else if (_ranges != NULL) {
					long value;

					HRESULT hr = _ranges->GetCount(&value);
					THROW_HRESULT(hr);

					return value;
				}

				return 0;
			}
		}

		/// <summary>
		/// Gets a <see cref="TextRange"/> for the story at the specified index.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="index"/> was outside the bounds of the collection.
		/// </exception>
		[System::Runtime::CompilerServices::IndexerName("Item")]		
		virtual property TextRange^ default[int] {
			TextRange^ get(int index) {		
				if ((index < 0) || (index >= Count)) {
					throw gcnew ArgumentOutOfRangeException("index", "Index was outside the bounds of the collection.");
				}
				
				if (_singleStory != nullptr) {
					return _singleStory;
				}
				else if (_ranges != NULL) {
					ITextRange* range = NULL;
					HRESULT hr = _ranges->Item(index + 1, &range);
					THROW_HRESULT(hr);

					ITextRange2* range2 = NULL;
					hr = range->QueryInterface(__uuidof(ITextRange2), (void**)&range2);

					if (SUCCEEDED(hr))
						return gcnew TextRange(range2);
					else
						return gcnew TextRange(range);
				}

				return nullptr;
			}
		}

	internal:

		TextStoryRanges(TextRange^ singleStory) {
			_singleStory = singleStory;
			_ranges = NULL;
			_ranges2 = NULL;
		}

		TextStoryRanges(ITextStoryRanges* ranges) {
			_ranges = ranges;
			_ranges2 = NULL;
			_singleStory = nullptr;
		}

#ifdef _TOM2

		TextStoryRanges(ITextStoryRanges2* ranges) {
			_ranges2 = ranges;
			_ranges = ranges;
			_singleStory = nullptr;
		}

#endif

	protected:

		~TextStoryRanges() {
			delete _singleStory;
			this->!TextStoryRanges();
		}

		!TextStoryRanges() {
			if (_ranges2 != NULL) {
				_ranges2->Release();
				_ranges2 = NULL;
				_ranges = NULL;
			}
			else if (_ranges != NULL) {
				_ranges->Release();
				_ranges = NULL;
			}
		}

	private:

		ITextStoryRanges* _ranges;
		ITextStoryRanges2* _ranges2;
		TextRange^ _singleStory;

		virtual System::Collections::IEnumerator^ EnumerableGetEnumerator() sealed = System::Collections::IEnumerable::GetEnumerator {
			return GetEnumerator();
		}

		ref class StoryEnumerator : public IEnumerator<TextRange^> {

		public:

			StoryEnumerator(TextStoryRanges^ parent) {
				_parent = parent;
				_index = -1;
				_current = nullptr;
			}

			virtual property TextRange^ Current {
				TextRange^ get() {
					return _current;
				}
			}

			virtual bool MoveNext() {
				if ((_index + 1) < _parent->Count) {
					_index++;
					_current = _parent->default[_index];
					return true;
				}

				return false;
			}

			virtual void Reset() {
				_index = -1;
				_current = nullptr;
			}

		protected:

			~StoryEnumerator() {
				this->!StoryEnumerator();
			}

			!StoryEnumerator() {

			}

		private:

			virtual property Object^ DummyCurrent {
				Object^ get() sealed = System::Collections::IEnumerator::Current::get {
					return _current;
				}
			}

			TextStoryRanges^ _parent;
			TextRange^ _current;
			int _index;
		};
	};
} NAMESPACE_CLOSE
