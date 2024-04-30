// TextFont.h

#pragma once

#include "Stdafx.h"
#include <TOM.h>
#include "enums.h"

using namespace System;
using namespace System::Drawing;
using namespace System::Runtime::InteropServices;

// Throws NotSupportedException if interface pointer is not ITextFont2
#define THROW_IF_NOT_TOM2() THROW_IF_NOT_INTERFACE(_font2)

// Generates property x of type Nullable<bool> whose underlying type is long
#define MAKE_PROPERTY_NBOOL(x)								\
property Nullable<bool> x {									\
	Nullable<bool> get() {									\
		long value;											\
		HRESULT hr = _font->Get##x(&value);					\
		if (hr == E_NOTIMPL) return Nullable<bool>();		\
		THROW_HRESULT(hr);									\
															\
		return LONG_TO_NBOOL(value);						\
	}														\
	void set(Nullable<bool> value) {						\
		HRESULT hr = _font->Set##x(NBOOL_TO_LONG(value));	\
		THROW_HRESULT(hr);									\
	}														\
}

// Generates a getter for property x of type y and underlying type z
#define MAKE_GETTER_TOM2(x,y,z)								\
	y get() {												\
		THROW_IF_NOT_TOM2();								\
															\
		z value;											\
		HRESULT hr = _font2->Get##x(&value);				\
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
		HRESULT hr = _font2->Set##x((z)value);				\
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
		HRESULT hr = _font2->Get##x(&value);				\
		if (hr == E_NOTIMPL) return Nullable<bool>();		\
		THROW_HRESULT(hr);									\
															\
		return LONG_TO_NBOOL(value);						\
	}														\
	void set(Nullable<bool> value) {						\
		THROW_IF_NOT_TOM2();								\
															\
		HRESULT hr = _font2->Set##x(NBOOL_TO_LONG(value));	\
		THROW_HRESULT(hr);									\
	}														\
}

namespace NAMESPACE_DECL {

	/// <summary>
	/// Provides access to the character format in a text range.
	/// </summary>
	/// <remarks>
	/// <para>
#ifdef _TOM2
	/// Managed wrapper class for the COM interface ITextFont, including ITextFont2 functionality. 
	/// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb774054(v=vs.85).aspx">ITextFont interface</seealso>
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768487(v=vs.85).aspx">ITextFont2 interface</seealso>
#else
	/// Managed wrapper class for the COM interface ITextFont. 
	/// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb774054(v=vs.85).aspx">ITextFont interface</seealso>
#endif
	/// </para>
	/// <para>
	/// Most properties of this class are implemented using nullable types. 
	/// If a property returns null, its value is undefined. Properties may be 
	/// undefined if the range spans several character formats.
	/// </para>
	/// </remarks>
	public ref class TextFont : public IEquatable<TextFont^>, public ICloneable {

	public:

		/// <summary>
		/// Determines whether the font can be changed. 
		/// </summary>
		property bool CanChange {
			bool get() {
				long value;
				HRESULT hr = _font->CanChange(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
		}
		/// <summary>
		/// Gets whether the characters are all uppercase.
		/// </summary>
		MAKE_PROPERTY_NBOOL(AllCaps)
		/// <summary>
		/// Gets or sets the text background (highlight) color.
		/// </summary>
		property Color BackColor {
			Color get() {
				long value;
				HRESULT hr = _font->GetBackColor(&value);
				THROW_HRESULT(hr);
						
				if ((value == tomAutoColor) || (value == tomUndefined)) {
					return Color::Empty;
				}
				else if ((value & 0xFF000000) == 0) {
					return ColorTranslator::FromWin32(value & 0x00FFFFFF);
				}
				else if ((value & 0xFF000000) == 0x01000000) {
					return ColorTranslator::FromWin32(PALETTEINDEX(value & 0x00FFFFFF));
				}

				throw gcnew InvalidOperationException("Unrecognised background color");
			}
			void set(Color value) {
				long lv;

				if (value.Equals(Color::Empty))
					lv = tomAutoColor;
				else
					lv = ColorTranslator::ToWin32(value);

				HRESULT hr = _font->SetBackColor(lv);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets whether the characters are bold.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Bold);
		/// <summary>
		/// Gets or sets whether the characters are embossed.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Emboss);
		/// <summary>
		/// Gets or sets the foreground (text) color.
		/// </summary>
		property Color ForeColor {
			Color get() {
				long value;
				HRESULT hr = _font->GetForeColor(&value);
				THROW_HRESULT(hr);
						
				if ((value == tomAutoColor) || (value == tomUndefined)) {
					return Color::Empty;
				}
				else if ((value & 0xFF000000) == 0) {
					return ColorTranslator::FromWin32(value & 0x00FFFFFF);
				}
				else if ((value & 0xFF000000) == 0x01000000) {
					return ColorTranslator::FromWin32(PALETTEINDEX(value & 0x00FFFFFF));
				}

				// unrecognised/multiple colors
				return Color::Empty;
			}
			void set(Color value) {
				long lv;

				if (value.Equals(Color::Empty))
					lv = tomAutoColor;
				else
					lv = ColorTranslator::ToWin32(value);

				HRESULT hr = _font->SetForeColor(lv);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>	
		/// Gets or sets whether characters are hidden.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Hidden);
		/// <summary>	
		/// Gets or sets whether characters are in italics.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Italic);
		/// <summary>	
		/// Gets or sets the minimum font size at which kerning occurs.
		/// </summary>
		property float Kerning {
			float get() {
				float value;
				HRESULT hr = _font->GetKerning(&value);
				THROW_HRESULT(hr);
				return value;
			}
			void set (float value) {
				HRESULT hr = _font->SetKerning(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>	
		/// Gets or sets the language ID or language code identifier (LCID).
		/// </summary>
		property int LanguageID {
			int get() {
				long value;
				HRESULT hr = _font->GetLanguageID(&value);
				THROW_HRESULT(hr);
				return value;
			}
			void set (int value) {
				HRESULT hr = _font->SetLanguageID(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>	
		/// Gets or sets the font name.
		/// </summary>
		property String^ Name {
			String^ get() {
				BSTR bstr;
				HRESULT hr = _font->GetName(&bstr);
				THROW_HRESULT(hr);
				String^ s = Marshal::PtrToStringBSTR(IntPtr(bstr));
				SysFreeString(bstr);
				return s;
			}
			void set (String^ value) {
				BSTR bstr = (BSTR)Marshal::StringToBSTR(value).ToPointer();
				HRESULT hr = _font->SetName(bstr);
				SysFreeString(bstr);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>	
		/// Gets or sets whether characters are displayed as outlined characters.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Outline);
		/// <summary>	
		/// Gets or sets the amount that characters are offset vertically relative to the baseline.
		/// </summary>
		property float Position {
			float get() {
				float value;
				HRESULT hr = _font->GetPosition(&value);
				THROW_HRESULT(hr);
				return value;
			}
			void set (float value) {
				HRESULT hr = _font->SetPosition(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets whether characters are protected against attempts to modify them.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Protected);
		/// <summary>
		/// Gets or sets whether characters are displayed as shadowed characters.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Shadow);
		/// <summary>	
		/// Gets or sets the font size, in points.
		/// </summary>
		property Nullable<float> Size {
			Nullable<float> get() {
				float value;
				HRESULT hr = _font->GetSize(&value);
				THROW_HRESULT(hr);
				if ((long)value == tomUndefined) return Nullable<float>();
				return value;
			}
			void set(Nullable<float> value) {
				HRESULT hr = _font->SetSize(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets whether characters are in small capital letters.
		/// </summary>
		MAKE_PROPERTY_NBOOL(SmallCaps);
		/// <summary>
		/// Gets or sets whether characters are displayed with a horizontal line through the center.
		/// </summary>
		MAKE_PROPERTY_NBOOL(StrikeThrough);
		/// <summary>
		/// Gets or sets the character style handle of the characters in a range.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The Text Object Model (TOM) version 1.0 does not specify the meanings of the style handles. 
		/// The meanings depend on other facilities of the text system that implements TOM.
		/// </para>
		/// <para>
		/// This implementation uses the low byte to store information about colored underlining. 
		/// The higher 3 bytes are preserved and can be used for your own purposes.
		/// </para>
		/// </remarks>
		property int Style {
			int get() {
				long value;
				HRESULT hr = _font->GetStyle(&value);
				THROW_HRESULT(hr);
				return value;
			}
			void set (int value) {
				HRESULT hr = _font->SetStyle(value);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets whether characters are displayed as subscript.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Subscript);
		/// <summary>
		/// Gets or sets whether characters are displayed as superscript.
		/// </summary>
		MAKE_PROPERTY_NBOOL(Superscript);
		/// <summary>
		/// Gets or sets the type of underlining for the characters in a range.
		/// </summary>
		property TextUnderlineStyle UnderlineStyle {
			TextUnderlineStyle get() {
				TextUnderlineStyle style;
				long v;
				HRESULT hr = _font->GetUnderline(&v);
				THROW_HRESULT(hr);

				if (SplitStyle(GetStyleLB(), &style, NULL)) {
					if ((v > 0) && (style == TextUnderlineStyle::None)) style = TextUnderlineStyle::Single;
				}
				else {
					style = (v > 0) ? TextUnderlineStyle::Single : TextUnderlineStyle::None;
				}

				return style;
			}
			void set(TextUnderlineStyle value) {
				HRESULT hr;			
				TextUnderlineColor color = UnderlineColor;

				// preferred method (RICHEDIT50W and above)
				_font->Reset(tomApplyTmp);
				hr = _font->SetUnderline(0xFF000000 | GetRGB(color));
				_font->SetUnderline((int)value);
				_font->Reset(tomApplyNow);

				SetStyleLB(JoinStyle(value, color));

				if (FAILED(hr)) {
					// fallback method (RICHEDIT20W)
					hr = _font->SetUnderline((int)value | (((int)color)<<4));
					THROW_HRESULT(hr);
				}
			}
		}
		/// <summary>
		/// Gets or sets the underline color for the characters in a range.
		/// </summary>
		property TextUnderlineColor UnderlineColor {
			TextUnderlineColor get() {
				TextUnderlineColor color;
				SplitStyle(GetStyleLB(), NULL, &color);
				return color;
			}
			void set(TextUnderlineColor value) {
				HRESULT hr;
				TextUnderlineStyle style = UnderlineStyle;

				// preferred method (RICHEDIT50W and above)
				_font->Reset(tomApplyTmp);
				hr = _font->SetUnderline(0xFF000000 | GetRGB(value));
				_font->SetUnderline(0x00FFFFFF & (int)style);
				_font->Reset(tomApplyNow);
				
				SetStyleLB(JoinStyle(style, value));

				if (FAILED(hr)) {
					// fallback method (RICHEDIT20W)
					hr = _font->SetUnderline((((int)value)<<4) | ((int)style & 0x0F));
					THROW_HRESULT(hr);
				}
			}
		}
		/// <summary>
		/// Gets or sets the font weight for the characters in a range.
		/// </summary>
		property FontWeight Weight {
			FontWeight get() {
				long value;
				HRESULT hr = _font->GetWeight(&value);
				THROW_HRESULT(hr);
				return (FontWeight)value;
			}
			void set(FontWeight value) {
				HRESULT hr = _font->SetWeight((int)value);
				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// Gets a duplicate of this text font object.
		/// </summary>
		/// <returns>The duplicate text font object.</returns>
		TextFont^ Clone() {
			if (_font2 != NULL) {
				ITextFont2* dup = NULL;
				HRESULT hr = _font2->GetDuplicate2(&dup);
				THROW_HRESULT(hr);
				return gcnew TextFont(dup);
			}
			else {
				ITextFont* dup = NULL;
				HRESULT hr = _font->GetDuplicate(&dup);
				THROW_HRESULT(hr);
				return gcnew TextFont(dup);
			}
		}

		/// <summary>
		/// Resets the character formatting to the default character format.
		/// </summary>
		void Reset() {
			HRESULT hr = _font->Reset(tomDefault);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Sets the character formatting by copying another text font object. 
		/// </summary>
		/// <param name="other">The text font object to apply to this font object.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="other"/> is null.
		/// </exception>
		/// <example>
		/// The following example demonstrates how to copy character formatting 
		/// from one range to another: 
		/// <code source="..\Examples\TextFont.cs" region="CopyFrom" language="cs" />
		/// </example>
		void CopyFrom(TextFont^ other) {
			if (other == nullptr) throw gcnew ArgumentNullException("other");

			if (_font2 != NULL) {
				HRESULT hr = _font2->SetDuplicate2(other->_font2);
				if (hr != E_NOTIMPL) {
					THROW_HRESULT(hr);
					return;
				}
			}
			
			HRESULT hr = _font->SetDuplicate(other->_font);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Tests if this <see cref="TextFont"/> is considered equal to another.
		/// </summary>
		/// <param name="other">The <see cref="TextFont"/> to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(TextFont^ other) {
			long result;
			if (_font2 != NULL) {
				HRESULT hr = _font2->IsEqual2(other->_font2, &result);
				THROW_HRESULT(hr);
				return (result == tomTrue);
			}
			else {
				HRESULT hr = _font->IsEqual(other->_font, &result);
				THROW_HRESULT(hr);
				return (result == tomTrue);
			}
		}

		/// <summary>	
		/// Tests if this object is considered equal to another.
		/// </summary>
		/// <param name="other">The object to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(Object^ other) override {
			TextFont^ font = dynamic_cast<TextFont^>(other);
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
			int hash = AllCaps.GetHashCode() 
				^ BackColor.GetHashCode() 
				^ Bold.GetHashCode()
				^ ForeColor.GetHashCode()
				^ Hidden.GetHashCode()
				^ Italic.GetHashCode()
				^ Kerning.GetHashCode()
				^ LanguageID.GetHashCode()
				^ Name->GetHashCode()
				^ Position.GetHashCode()
				^ Protected.GetHashCode()
				^ Size.GetHashCode()
				^ StrikeThrough.GetHashCode()
				^ Style.GetHashCode()
				^ Subscript.GetHashCode()
				^ Superscript.GetHashCode()
				^ UnderlineColor.GetHashCode()
				^ UnderlineStyle.GetHashCode()
				^ Weight.GetHashCode();

#ifdef _TOM2
			if (_font2 != NULL) {
				hash ^= AutoLigatures.GetHashCode()
					^ AutospaceAlpha.GetHashCode()
					^ AutospaceAlpha.GetHashCode()
					^ AutospaceParens.GetHashCode()
					^ CharRep.GetHashCode()
					^ CompressionMode.GetHashCode()
					^ Cookie.GetHashCode()
					^ Count.GetHashCode()
					^ DoubleStrike.GetHashCode()
					^ Effects.GetHashCode()
					^ Effects2.GetHashCode()
					^ LinkType.GetHashCode()
					^ MathZone.GetHashCode()
					^ ModWidthPairs.GetHashCode()
					^ ModWidthSpace.GetHashCode()
					^ OldNumbers.GetHashCode()
					^ Overlapping.GetHashCode()
					^ PositionSubSuper.GetHashCode()
					^ SpaceExtension.GetHashCode()
					^ UnderlinePositionMode.GetHashCode();
			}
#endif

			return hash;
		}

#ifdef _TOM2

		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether support for automatic ligatures is active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(AutoLigatures)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the East Asian "autospace alphabetics" state.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(AutospaceAlpha)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the East Asian "autospace numeric" state.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(AutospaceNumeric)
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the East Asian "autospace parentheses" state.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(AutospaceParens)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the character repertoire (writing system).
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_LONG(CharRep,CharRepertoire)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the East Asian compression mode.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_LONG(CompressionMode,EastAsianCompressionMode)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the client cookie.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_LONG(Cookie,int)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets the count of extra properties in this character formatting collection.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		property int Count {
			MAKE_GETTER_TOM2(Count,int,long)
		}
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether characters are displayed with double horizontal lines through the center.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(DoubleStrike)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the character format effects.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		property CharacterEffects Effects {
			CharacterEffects get() {
				THROW_IF_NOT_TOM2();

				long value, mask;
				HRESULT hr = _font2->GetEffects(&value, &mask);
				THROW_HRESULT(hr);
				return (CharacterEffects)value;
			}
			void set(CharacterEffects value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _font2->SetEffects((long)value, (long)CharacterEffects::All);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the additional character format effects.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		property CharacterEffects2 Effects2 {
			CharacterEffects2 get() {
				THROW_IF_NOT_TOM2();

				long value, mask;
				HRESULT hr = _font2->GetEffects2(&value, &mask);
				THROW_HRESULT(hr);
				return (CharacterEffects2)value;
			}
			void set(CharacterEffects2 value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _font2->SetEffects2((long)value, (long)CharacterEffects2::All);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets the link type.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		property NAMESPACE::LinkType LinkType {
			MAKE_GETTER_TOM2(LinkType,NAMESPACE::LinkType,long)
		}
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether a math zone is active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(MathZone)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether "decrease widths on pairs" is active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(ModWidthPairs)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether "increase width of whitespace" is active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(ModWidthSpace)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether old-style numbers are active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(OldNumbers)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets whether overlapping text is active.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_NBOOL(Overlapping)
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the subscript or superscript position relative to the baseline (as a percent of the font height).
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_LONG(PositionSubSuper,int)
		/// <summary>
		/// (TOM 2 only) 
		/// Gets or sets the East Asian space extension value.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		property Nullable<float> SpaceExtension {
			Nullable<float> get() {
				THROW_IF_NOT_TOM2();

				float value;
				HRESULT hr = _font2->GetSpaceExtension(&value);
				THROW_HRESULT(hr);
				if ((long)value == tomUndefined) return Nullable<float>();
				return value;
			}
			void set(Nullable<float> value) {
				THROW_IF_NOT_TOM2();

				HRESULT hr = _font2->SetSpaceExtension(value.GetValueOrDefault(tomUndefined));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// (TOM 2 only) 		
		/// Gets or sets the underline position mode.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		MAKE_PROPERTY_TOM2_LONG(UnderlinePositionMode,NAMESPACE::UnderlinePositionMode)

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the value of a property.
		/// </summary>
		/// <param name="type">The property ID.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		int GetProperty(int type) {
			THROW_IF_NOT_TOM2();

			long value;
			HRESULT hr = _font2->GetProperty(type, &value);
			THROW_HRESULT(hr);

			return value;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Gets the property type and value of the specified extra property.
		/// </summary>
		/// <param name="index">he collection index of the extra property.</param>
		/// <param name="type">The property ID.</param>
		/// <param name="value">The property value.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		void GetPropertyInfo(int index, [Out] int% type, [Out] int% value) {
			THROW_IF_NOT_TOM2();

			long p0, p1;
			HRESULT hr = _font2->GetPropertyInfo(index, &p0, &p1);
			THROW_HRESULT(hr);

			type = p0;
			value = p1;
		}

		/// <summary>
		/// (TOM 2 only) 
		/// Sets the value of the specified property.
		/// </summary>
		/// <param name="type">The ID of the property to set.</param>
		/// <param name="value">The new property value.</param>
		/// <exception cref="NotSupportedException">
		/// The underlying COM object does not implement the ITextFont2 interface.
		/// </exception>
		void SetProperty(int type, int value) {
			THROW_IF_NOT_TOM2();

			HRESULT hr = _font2->SetProperty(type, value);
			THROW_HRESULT(hr);
		}

#endif

	internal:

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="font">The ITextFont to wrap.</param>
		TextFont(ITextFont* font) {
			_font = font;
			_font2 = NULL;
		}								

#ifdef _TOM2

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="font">The ITextFont2 to wrap.</param>
		TextFont(ITextFont2* font) {
			_font2 = font;
			_font = font;
		}	

#endif

		/// <summary>
		/// Gets a pointer to the underlying COM object.
		/// </summary>
		property IntPtr ComObject {
			IntPtr get() {
				return IntPtr(_font);
			}
		}

	protected:

		~TextFont() {
			this->!TextFont();
		}

		!TextFont() {
			if (_font2 != NULL) {
				_font2->Release();
				_font2 = NULL;
				_font = NULL;
			}
			else if (_font != NULL) {
				_font->Release();
				_font = NULL;
			}
		}

	private:

		virtual System::Object^ ICloneableClone() sealed = System::ICloneable::Clone {
			return Clone();
		}

		long GetRGB(TextUnderlineColor code) {
			switch (code) {
				// format: 0xBBGGRR
				case TextUnderlineColor::Blue:
					return 0xFF0000;
				case TextUnderlineColor::Brown:
					return 0x000080;
				case TextUnderlineColor::Cyan:
					return 0xFFFF00;
				case TextUnderlineColor::DarkBlue:
					return 0x800000;
				case TextUnderlineColor::DarkCyan:
					return 0x808000;
				case TextUnderlineColor::DarkGray:
					return 0x404040;
				case TextUnderlineColor::DarkMagenta:
					return 0x800080;
				case TextUnderlineColor::Gray:
					return 0x808080;
				case TextUnderlineColor::Green:
					return 0x008000;
				case TextUnderlineColor::LimeGreen:
					return 0x00FF00;
				case TextUnderlineColor::Magenta:
					return 0xFF00FF;
				case TextUnderlineColor::OliveGreen:
					return 0x008080;
				case TextUnderlineColor::Red:
					return 0x0000FF;
				case TextUnderlineColor::White:
					return 0xFFFFFF;
				case TextUnderlineColor::Yellow:
					return 0x00FFFF;
			}

			return 0;
		}

		long JoinStyle(TextUnderlineStyle style, TextUnderlineColor color) {
			return (int)style | (((int)color)<<4);
		}

		bool SplitStyle(long value, TextUnderlineStyle* style, TextUnderlineColor* color) {
			if (value != tomUndefined) {
				if (style != NULL) (*style) = (TextUnderlineStyle)(value & 0x0F);
				if (color != NULL) (*color) = (TextUnderlineColor)((value & 0xF0)>>4);
				return true;
			}
			else {
				if (style != NULL) (*style) = TextUnderlineStyle::None;
				if (color != NULL) (*color) = TextUnderlineColor::Black;
			}

			return false;
		}

		unsigned int GetStyleLB() {
			HRESULT hr;
			long style;
			
			hr = _font->GetStyle(&style);
			THROW_HRESULT(hr);

			return (style & 0xFF);
		}

		void SetStyleLB(unsigned int value) {
			HRESULT hr;
			long style;
			
			hr = _font->GetStyle(&style);
			THROW_HRESULT(hr);

			if (style == tomUndefined) style = 0;
			style &= ~0xFF;
			style |= (0xFF & value);
			
			hr = _font->SetStyle(style);
			THROW_HRESULT(hr);
		}

		ITextFont* _font;
		ITextFont2* _font2;
	};
} NAMESPACE_CLOSE

#undef THROW_IF_NOT_TOM2
#undef MAKE_PROPERTY_NBOOL
#undef MAKE_GETTER_TOM2
#undef MAKE_PROPERTY_TOM2
#undef MAKE_PROPERTY_TOM2_LONG
#undef MAKE_PROPERTY_TOM2_NBOOL
