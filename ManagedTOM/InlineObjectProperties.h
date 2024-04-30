// InlineObjectProperties.h

#pragma once
#ifdef _TOM2

#include "Stdafx.h"
#include "enums.h"

using namespace System;

namespace NAMESPACE_DECL {

	ref class TextRange;

	/// <summary>
	/// (TOM 2 only) 
	/// Provides access to the inline object properties for a <see cref="TextRange"/>.
	/// </summary>
	public value class InlineObjectProperties {

	public:

		/// <summary>
		/// Gets or sets the inline object type.
		/// </summary>
		property InlineObjectType Type {
			InlineObjectType get() {
				return _type;
			}
			void set(InlineObjectType value) {
				_type = value;
			}
		}
		/// <summary>
		/// Gets or sets the inline object alignment, whose meaning depends on the inline object type.
		/// </summary>
		/// <remarks>
		/// The enumeration type is given by the <see cref="AlignType"/> property.
		/// </remarks>
		property Enum^ Align {
			Enum^ get() {
				return _align;
			}
			void set(Enum^ value) {
				_align = value;
			}
		}
		/// <summary>
		/// Gets the <see cref="System::Type"/> of the enumeration containing the valid values for the <see cref="Align"/> property.
		/// </summary>
		/// <remarks>
		/// If the <see cref="Align"/> property has no meaning for the current object type, the value of this property is null.
		/// </remarks>
		property System::Type^ AlignType {
			System::Type^ get() {
				switch (_type) {
					case InlineObjectType::Ruby:
						return RubyAlign::typeid;
					case InlineObjectType::Box:
						return BoxAlign::typeid;
					case InlineObjectType::BoxedFormula:
						return BoxedFormulaAlign::typeid;
					case InlineObjectType::Brackets:
						return BracketsAlign::typeid;
					case InlineObjectType::EquationArray:
						return EquationArrayAlign::typeid;
					case InlineObjectType::Matrix:
						return MatrixAlign::typeid;
					case InlineObjectType::Nary:
						return NaryAlign::typeid;
					case InlineObjectType::Phantom:
						return PhantomAlign::typeid;
					case InlineObjectType::Radical:
						return RadicalAlign::typeid;
					case InlineObjectType::SubSup:
						return SubSupAlign::typeid;
					case InlineObjectType::StretchStack:
						return StretchStackAlign::typeid;
				}

				return nullptr;
			}
		}
		/// <summary>
		/// Gets or sets the inline object character.
		/// </summary>
		property System::Char Char {
			System::Char get() {
				return _char;
			}
			void set(System::Char value) {
				_char = value;
			}
		}
		/// <summary>
		/// Gets or sets the closing bracket character.
		/// </summary>
		property System::Char Char1 {
			System::Char get() {
				return _char1;
			}
			void set(System::Char value) {
				_char1 = value;
			}
		}
		/// <summary>
		/// Gets or sets the separator character. 
		/// </summary>
		property System::Char Char2 {
			System::Char get() {
				return _char2;
			}
			void set(System::Char value) {
				_char2 = value;
			}
		}
		/// <summary>
		/// Gets or sets the inline object count of arguments.
		/// </summary>
		property int ArgCount {
			int get() {
				return _count;
			}
			void set(int value) {
				_count = value;
			}
		}
		/// <summary>
		/// Gets or sets the inline object TeX style.
		/// </summary>
		property MathTeXStyle TeXStyle {
			MathTeXStyle get() {
				return _texStyle;
			}
			void set(MathTeXStyle value) {
				_texStyle = value;
			}
		}
		/// <summary>
		/// Gets or sets the inline object count of columns (<see cref="InlineObjectType::Matrix"/> only).
		/// </summary>
		property int ColCount {
			int get() {
				return _col;
			}
			void set(int value) {
				_col = value;
			}
		}
		/// <summary>
		/// Gets the inline object 0-based nesting level.
		/// </summary>
		property int Level {
			int get() {
				return _level;
			}	
			internal: void set(int value) {
				_level = value;
			}
		}

	private:

		InlineObjectType _type;
		Enum^ _align;
		System::Char _char;
		System::Char _char1;
		System::Char _char2;
		int _count;
		MathTeXStyle _texStyle;
		int _col;
		int _level;

	};
} NAMESPACE_CLOSE

#endif