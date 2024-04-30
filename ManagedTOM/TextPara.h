// TextPara.h

#pragma once

#include "Stdafx.h"
#include <TOM.h>
#include "enums.h"

using namespace System;
using namespace System::Runtime::InteropServices;

// Throws NotSupportedException if interface pointer is not ITextPara2
#define THROW_IF_NOT_TOM2() THROW_IF_NOT_INTERFACE(_para2)

// Generates property x of type Nullable<bool> whose underlying type is long
#define MAKE_PROPERTY_NBOOL(x)								\
property Nullable<bool> x {									\
	Nullable<bool> get() {									\
		long value;											\
		HRESULT hr = _para->Get##x(&value);					\
		if (hr == E_NOTIMPL) return Nullable<bool>();		\
		THROW_HRESULT(hr);									\
															\
		return LONG_TO_NBOOL(value);						\
	}														\
	void set(Nullable<bool> value) {						\
		HRESULT hr = _para->Set##x(NBOOL_TO_LONG(value));	\
		THROW_HRESULT(hr);									\
	}														\
}

// Generates a getter for property x of type y and underlying type z
#define MAKE_GETTER_TOM2(x,y,z)								\
	y get() {												\
		THROW_IF_NOT_TOM2();								\
															\
		z value;											\
		HRESULT hr = _para2->Get##x(&value);				\
		THROW_HRESULT(hr);									\
															\
		return (y)value;									\
	}														

// Generates property x of type y and underlying type z
#define MAKE_PROPERTY_TOM2(x,y,z)							\
property y x {												\
	MAKE_GETTER_TOM2(x,y,z)									\
	void set(y value) {										\
		THROW_IF_NOT_TOM2();								\
															\
		HRESULT hr = _para2->Set##x((z)value);				\
		THROW_HRESULT(hr);									\
	}														\
}

// Generates property x of type y whose underlying type is long
#define MAKE_PROPERTY_TOM2_LONG(x,y) MAKE_PROPERTY_TOM2(x,y,long)

// Generates property x of type Nullable<bool> whose underlying type is long
#define MAKE_PROPERTY_TOM2_NBOOL(x)							\
property Nullable<bool> x {									\
	Nullable<bool> get() {									\
		THROW_IF_NOT_TOM2();								\
															\
		long value;											\
		HRESULT hr = _para2->Get##x(&value);				\
		if (hr == E_NOTIMPL) return Nullable<bool>();		\
		THROW_HRESULT(hr);									\
															\
		return LONG_TO_NBOOL(value);						\
	}														\
	void set(Nullable<bool> value) {						\
		THROW_IF_NOT_TOM2();								\
															\
		HRESULT hr = _para2->Set##x(NBOOL_TO_LONG(value));	\
		THROW_HRESULT(hr);									\
	}														\
}

namespace NAMESPACE_DECL {

	/// <summary>
	/// Provides access to the paragraph format for a text range.
	/// </summary>
	/// <remarks>
	/// <para>
#ifdef _TOM2
	/// Managed wrapper class for the COM interface ITextPara, including ITextPara2 functionality. 
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb774056%28v=vs.85%29.aspx">ITextPara interface</seealso>
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768585(v=vs.85).aspx">ITextPara2 interface</seealso>
#else
	/// Managed wrapper class for the COM interface ITextPara. 
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb774056%28v=vs.85%29.aspx">ITextPara interface</seealso>
#endif
	/// </para>
	/// <para>
	/// Most properties of this class are implemented using nullable types. 
	/// If a property returns null, its value is undefined. Properties may be 
	/// undefined if the range spans several paragraph formats.
	/// </para>
	/// </remarks>
	public ref class TextPara : public IEquatable<TextPara^>, public ICloneable {

	public:

		/// <summary>
		/// Gets or sets the current paragraph alignment value.
		/// </summary>
		property TextAlignment Alignment {
			TextAlignment get() {
				long value;
				HRESULT hr = _para->GetAlignment(&value);
				THROW_HRESULT(hr);
				return (TextAlignment)value;
			}
			void set(TextAlignment value) {
				HRESULT hr = _para->SetAlignment((int)value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Determines whether the paragraph formatting can be changed.
		/// </summary>
		property bool CanChange {
			bool get() {
				long value;
				HRESULT hr = _para->CanChange(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
		}
		/// <summary>
		/// Gets the amount used to indent the first line of a paragraph 
		/// relative to the left indent. The left indent is the indent 
		/// for all lines of the paragraph except the first line.
		/// </summary>
		property Nullable<float> FirstLineIndent {
			Nullable<float> get() {
				float value;
				HRESULT hr = _para->GetFirstLineIndent(&value);
				THROW_HRESULT(hr);
				if ((int)value == tomUndefined) return Nullable<float>();
				return value;
			}
		}
		/// <summary>
		/// Gets or sets the current paragraph alignment value.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Hyphenation)
		/// <summary>
		/// Gets the distance used to indent all lines except the 
		/// first line of a paragraph. The distance is relative to the 
		/// left margin.
		/// </summary>
		property Nullable<float> LeftIndent {
			Nullable<float> get() {
				float value;
				HRESULT hr = _para->GetLeftIndent(&value);
				THROW_HRESULT(hr);
				if ((int)value == tomUndefined) return Nullable<float>();
				return value;
			}
		}
		/// <summary>
		/// Gets or sets the kind of alignment to use for bulleted and numbered lists.
		/// </summary>
		property TextAlignment ListAlignment {
			TextAlignment get() {
				long value;
				HRESULT hr = _para->GetListAlignment(&value);
				THROW_HRESULT(hr);
				return (TextAlignment)value;
			}
			void set(TextAlignment value) {
				if (value == TextAlignment::Justify) throw gcnew ArgumentException("Justified alignment is not supported for lists", "value");

				HRESULT hr = _para->SetListAlignment((int)value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the list level index used with paragraphs.
		/// </summary>
		property Nullable<int> ListLevelIndex {
			Nullable<int> get() {
				long value;
				HRESULT hr = _para->GetListLevelIndex(&value);
				THROW_HRESULT(hr);
				if (value == tomUndefined) return Nullable<int>();
				return value;
			}
			void set(Nullable<int> value) {
				HRESULT hr = _para->SetListLevelIndex(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the starting value or code of a list numbering sequence.
		/// </summary>
		property Nullable<int> ListStart {
			Nullable<int> get() {
				long value;
				HRESULT hr = _para->GetListStart(&value);
				THROW_HRESULT(hr);
				if (value == tomUndefined) return Nullable<int>();
				return value;
			}
			void set(Nullable<int> value) {
				HRESULT hr = _para->SetListStart(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Retrieves the list tab setting, which is the distance 
		/// between the first-line indent and the text on the first 
		/// line. The numbered or bulleted text is left-justified, 
		/// centered, or right-justified at the first-line indent value.
		/// </summary>
		property Nullable<float> ListTab {
			Nullable<float> get() {
				float value;
				HRESULT hr = _para->GetListTab(&value);
				THROW_HRESULT(hr);
				if ((int)value == tomUndefined) return Nullable<float>();
				return value;
			}
			void set(Nullable<float> value) {
				HRESULT hr = _para->SetListTab(value.GetValueOrDefault((float)tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the kind of numbering to use with paragraphs.
		/// </summary>
		property NAMESPACE::ListType ListType {
			NAMESPACE::ListType get() {
				int value = ListTypeInternal;
				if (value == tomUndefined) return NAMESPACE::ListType::Undefined;
				value &= 0x0FFFF;
				return (NAMESPACE::ListType)value;
			}
			void set(NAMESPACE::ListType value) {
				int existing = ListTypeInternal;
				if (existing == tomUndefined) existing = 0;
				existing &= 0xF0000;
				ListTypeInternal = (int)value | existing;
			}
		}
		/// <summary>
		/// Gets or sets the format to use with a numbered list.
		/// </summary>
		property NAMESPACE::ListNumberingFormat ListNumberingFormat {
			NAMESPACE::ListNumberingFormat get() {
				int value = ListTypeInternal;
				if (value == tomUndefined) return NAMESPACE::ListNumberingFormat::Undefined;
				value &= 0xF0000;
				return (NAMESPACE::ListNumberingFormat)value;
			}
			void set(NAMESPACE::ListNumberingFormat value) {
				int existing = ListTypeInternal;
				if (existing == tomUndefined) existing = 0;
				existing &= 0x0FFFF;
				ListTypeInternal = (int)value | existing;
			}
		}
		/// <summary>
		/// Gets or sets the Unicode character (>32) to use for bullets.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> has a character code of 32 or lower.
		/// </exception>
		property Char ListBulletChar {
			Char get() {
				int value = ListTypeInternal;
				if (value > 32)
					return (Char)value;
				else if (value == tomListBullet)
					return 0x2022;
				else
					return 0;						
			}
			void set(Char value) {
				if (value > 32)
					ListTypeInternal = value;
				else
					throw gcnew ArgumentException("Bullet character must have a Unicode value greater than 32");
			}
		}
		/// <summary>
		/// Gets or sets the size of the right margin indent of a paragraph. 
		/// </summary>
		property Nullable<float> RightIndent {
			Nullable<float> get() {
				float value;
				HRESULT hr = _para->GetRightIndent(&value);
				THROW_HRESULT(hr);
				if ((int)value == tomUndefined) return Nullable<float>();
				return value;
			}
			void set(Nullable<float> value) {
				HRESULT hr = _para->SetRightIndent(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the amount of vertical space below a paragraph. 
		/// </summary>
		property Nullable<float> SpaceAfter {
			Nullable<float> get() {
				float value;
				HRESULT hr = _para->GetSpaceAfter(&value);
				THROW_HRESULT(hr);
				if ((int)value == tomUndefined) return Nullable<float>();
				return value;
			}
			void set(Nullable<float> value) {
				HRESULT hr = _para->SetSpaceAfter(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the amount of vertical space above a paragraph. 
		/// </summary>
		property Nullable<float> SpaceBefore {
			Nullable<float> get() {
				float value;
				HRESULT hr = _para->GetSpaceBefore(&value);
				THROW_HRESULT(hr);
				if ((int)value == tomUndefined) return Nullable<float>();
				return value;
			}
			void set(Nullable<float> value) {
				HRESULT hr = _para->SetSpaceBefore(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}				
		/// <summary>
		/// Gets or sets the style handle to the paragraphs in the specified range.
		/// </summary>
		property int Style {
			int get() {
				long value;
				HRESULT hr = _para->GetStyle(&value);
				THROW_HRESULT(hr);
				return value;
			}
			void set(int value) {
				HRESULT hr = _para->SetStyle(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Retrieves the tab count.
		/// </summary>
		property Nullable<int> TabCount {
			Nullable<int> get() {
				long value;
				HRESULT hr = _para->GetTabCount(&value);
				THROW_HRESULT(hr);
				if (value == tomUndefined) return Nullable<int>();
				return value;
			}
		}

		/// <summary>
		/// Creates a duplicate of the specified paragraph format object.
		/// </summary>
		/// <returns>The duplicate text paragraph object.</returns>
		TextPara^ Clone() {
			if (_para2 != NULL) {
				ITextPara2* dup = NULL;
				HRESULT hr = _para2->GetDuplicate2(&dup);
				THROW_HRESULT(hr);
				return gcnew TextPara(dup);
			}
			else {
				ITextPara* dup = NULL;
				HRESULT hr = _para->GetDuplicate(&dup);
				THROW_HRESULT(hr);
				return gcnew TextPara(dup);
			}
		}

		/// <summary>
		/// Adds a tab at the displacement <paramref name="position"/>.
		/// </summary>
		/// <param name="position">New tab displacement, in floating-point points.</param>
		void AddTab(float position) {
			HRESULT hr = _para->AddTab(position, tomAlignLeft, tomSpaces);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Clears all tabs, reverting to equally spaced tabs with the default tab spacing.
		/// </summary>
		void ClearAllTabs() {
			HRESULT hr = _para->ClearAllTabs();
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Deletes a tab at a specified displacement.
		/// </summary>
		/// <param name="position">Displacement, in floating-point points, at which a tab should be deleted.</param>
		void DeleteTab(float position) {
			HRESULT hr = _para->DeleteTab(position);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Retrieves tab displacement for the specified tab.
		/// </summary>
		/// <param name="position">Indicates which tab to retrieve relative to <paramref name="relativeTo"/>.</param>
		/// <param name="relativeTo">The tab displacement, in floating-point points.</param>
		/// <returns>The tab displacement, in floating-point points.</returns>
		float GetTab(TabRelativePosition position, float relativeTo) {
			long index = (int)position;
			float pos = relativeTo;
			long align;
			long leader;
			HRESULT hr = _para->GetTab(index, &pos, &align, &leader);
			THROW_HRESULT(hr);
			return pos;
		}

		/// <summary>
		/// Retrieves tab displacement for the specified tab.
		/// </summary>
		/// <param name="index">Index of tab for which to retrieve info.</param>
		/// <returns>The tab displacement, in floating-point points.</returns>
		float GetTab(int index) {
			if (index < 0) throw gcnew ArgumentException("Index must be greater than or equal to zero");
			float pos;
			long align;
			long leader;
			HRESULT hr = _para->GetTab(index, &pos, &align, &leader);
			THROW_HRESULT(hr);
			return pos;
		}
				
		/// <summary>
		/// Sets the first-line indent, the left indent, and the right indent for a paragraph. 
		/// </summary>
		/// <param name="first">Indent of the first line in a paragraph, relative to the left indent. The value is in floating-point points and can be positive or negative.</param>
		/// <param name="left">Left indent of all lines except the first line in a paragraph, relative to left margin. The value is in floating-point points and can be positive or negative.</param>
		/// <param name="right">Right indent of all lines in paragraph, relative to the right margin. The value is in floating-point points and can be positive or negative. This value is optional.</param>
		void SetIndents(float first, float left, float right) {
			HRESULT hr = _para->SetIndents(first, left, right);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Resets the paragraph formatting to the default values. 
		/// </summary>
		void Reset() {
			HRESULT hr = _para->Reset(tomDefault);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Sets the paragraph formatting by copying another paragraph object. 
		/// </summary>
		/// <param name="other">The paragraph object to apply to this paragraph object.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other"/> is null.
		/// </exception>
		void CopyFrom(TextPara^ other) {
			if (other == nullptr) throw gcnew ArgumentNullException("other");

			if (_para2 != NULL) {
				HRESULT hr = _para2->SetDuplicate2(other->_para2);
				THROW_HRESULT(hr);
			}
			else {
				HRESULT hr = _para->SetDuplicate(other->_para);
				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// Tests if this <see cref="TextPara"/> is considered equal to another.
		/// </summary>
		/// <param name="other">The <see cref="TextPara"/> to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(TextPara^ other) {
			long result;
			HRESULT hr;
			
			if (_para2 != NULL)
				hr = _para2->IsEqual2(other->_para2, &result);
			else
				hr = _para->IsEqual(other->_para, &result);
			
			THROW_HRESULT(hr);
			return (result == tomTrue);
		}

		/// <summary>
		/// Tests if this <see cref="TextPara"/> is considered equal to another object.
		/// </summary>
		/// <param name="other">The object to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(Object^ other) override {
			TextPara^ font = dynamic_cast<TextPara^>(other);
			if (font != nullptr)
				return Equals(font);
			else
				return Object::Equals(other);
		}

		/// <summary>
		/// Calculates a hash code for this object.
		/// </summary>
		/// <returns>A hash code for this object.</returns>
		virtual int GetHashCode() override {
			int hash = Alignment.GetHashCode()
				^ FirstLineIndent.GetHashCode()
				^ Hyphenation.GetHashCode()
				^ LeftIndent.GetHashCode()
				^ ListAlignment.GetHashCode()
				^ ListLevelIndex.GetHashCode()
				^ ListStart.GetHashCode()
				^ ListTab.GetHashCode()
				^ ListTypeInternal.GetHashCode()
				^ RightIndent.GetHashCode()
				^ SpaceAfter.GetHashCode()
				^ SpaceBefore.GetHashCode()
				^ Style.GetHashCode()
				^ TabCount.GetHashCode();

#ifdef _TOM2
			if (_para2 != NULL) {
				hash ^= Effects.GetHashCode()
					^ FontAlignment.GetHashCode()
					^ HangingPunctuation.GetHashCode()
					^ SnapToGrid.GetHashCode()
					^ TrimPunctuationAtStart.GetHashCode();
			}
#endif

			return hash;
		}

#ifdef _TOM2

		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the character format effects.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		property ParagraphEffects Effects {
			ParagraphEffects get() {
				THROW_IF_NOT_TOM2();

				long value, mask;
				HRESULT hr = _para2->GetEffects(&value, &mask);
				THROW_HRESULT(hr);
				return (ParagraphEffects)value;
			}
			void set(ParagraphEffects value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _para2->SetEffects((long)value, (long)ParagraphEffects::All);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the paragraph font alignment state.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_LONG(FontAlignment,NAMESPACE::FontAlignment)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether to hang punctuation symbols on the right margin when the paragraph is justified.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(HangingPunctuation)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether paragraph lines snap to a vertical grid that could be defined for the whole document.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(SnapToGrid)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether to trim the leading space of a punctuation symbol at the start of a line.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(TrimPunctuationAtStart)

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the borders collection.
		/// </summary>
		/// <remarks>
		/// This method is not implemented.
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		/// <exception cref="NotImplementedException">
		/// The TOM implementation used by the paragraph does not support borders.
		/// </exception>
		Object^ GetBorders() {
			THROW_IF_NOT_TOM2();

			IUnknown* borders;
			HRESULT hr = _para2->GetBorders(&borders);
			THROW_HRESULT(hr);

			return Marshal::GetObjectForIUnknown(IntPtr(borders));
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the value of a property.
		/// </summary>
		/// <param name="type">The property ID.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		int GetProperty(int type) {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _para2->GetProperty(type, &value);
			THROW_HRESULT(hr);

			return value;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Sets the value of the specified property.
		/// </summary>
		/// <param name="type">The ID of the property to set.</param>
		/// <param name="value">The new property value.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextPara2 interface.
		/// </exception>
		void SetProperty(int type, int value) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _para2->SetProperty(type, value);
			THROW_HRESULT(hr);
		}

#endif

	internal:

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="para">The ITextPara to wrap.</param>
		TextPara(ITextPara* para) {
			_para = para;
			_para2 = NULL;
		}

#ifdef _TOM2

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="para">The ITextPara2 to wrap.</param>
		TextPara(ITextPara2* para) {
			_para2 = para;
			_para = para;
		}

#endif

		/// <summary>	
		/// Gets a pointer to the underlying COM object.
		/// </summary>
		property IntPtr ComObject {
			IntPtr get() {
				return IntPtr(_para);
			}
		}

	protected:						

		/// <summary>
		/// Gets or sets the kind of numbering to use with paragraphs.
		/// </summary>
		property int ListTypeInternal {
			int get() {
				long value;
				HRESULT hr = _para->GetListType(&value);
				THROW_HRESULT(hr);
				return value;
			}
			void set(int value) {
				HRESULT hr = _para->SetListType(value);
				THROW_HRESULT(hr);
			}
		}

		~TextPara() {
			this->!TextPara();
		}

		!TextPara() {
			if (_para2 != NULL) {
				_para2->Release();
				_para2 = NULL;
				_para = NULL;
			}
			else if (_para != NULL) {
				_para->Release();
				_para = NULL;
			}
		}

	private:

		virtual System::Object^ ICloneableClone() sealed = System::ICloneable::Clone {
			return Clone();
		}

		ITextPara* _para;
		ITextPara2* _para2;
	};
} NAMESPACE_CLOSE

#undef THROW_IF_NOT_TOM2
#undef MAKE_PROPERTY_NBOOL
#undef MAKE_GETTER_TOM2
#undef MAKE_PROPERTY_TOM2
#undef MAKE_PROPERTY_TOM2_LONG
#undef MAKE_PROPERTY_TOM2_NBOOL
