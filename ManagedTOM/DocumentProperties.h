// DocumentProperties.h

#pragma once
#ifdef _TOM2

#include "stdafx.h"
#include <TOM.h>
#include "enums.h"

#undef DocumentProperties

using namespace System;

namespace NAMESPACE_DECL {

	ref class TextDocument;

	/// <summary>
	/// (TOM 2 only) 
	/// Provides access to the document properties for a <see cref="TextDocument"/>.
	/// </summary>
	public ref class DocumentProperties {

	public:

		/// <summary>
		/// Indicates whether data can be copied to the clipboard.
		/// </summary>
		property bool CanCopy {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Indicates whether one or more redo operations exist.
		/// </summary>
		property bool CanRedo {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Indicates whether one or more undo operations exist.
		/// </summary>
		property bool CanUndo {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// One or more of the <see cref="MathBuildFlags"/> flags.
		/// </summary>
		property MathBuildFlags DocMathBuild {
			MathBuildFlags get();
			void set(MathBuildFlags value);
		}
		/// <summary>
		/// Space between equations in math paragraphs.
		/// </summary>
		property int MathInterSpace {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Space between lines in a display math equation.
		/// </summary>
		property int MathIntraSpace {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Left margin for display math.
		/// </summary>
		property int MathLMargin {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Space after a display math equation.
		/// </summary>
		property int MathPostSpace {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Space before a display math equation.
		/// </summary>
		property int MathPreSpace {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Right margin for display math.
		/// </summary>
		property int MathRMargin {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Equation wrap indent for display math.
		/// </summary>
		property int MathWrapIndent {
			int get();
			void set(int value);
		}
		/// <summary>
		/// Equation right wrap indent for display math (in a left-to-right (LTR) math zone).
		/// </summary>
		property int MathWrapRight {
			int get();
			void set(int value);
		}
		/// <summary>
		/// The undo stack count limit.
		/// </summary>
		property int UndoLimit {
			int get();
			void set(int value);
		}
		/// <summary>
		/// The ellipsis mode.
		/// </summary>
		property TextEllipsisMode EllipsisMode {
			TextEllipsisMode get();
			void set(TextEllipsisMode value);
		}
		/// <summary>
		/// The ellipsis state.
		/// </summary>
		property TextEllipsisState EllipsisState {
			TextEllipsisState get();
			void set(TextEllipsisState value);
		}

	internal:

		DocumentProperties(TextDocument^ parent);
		DocumentProperties(const DocumentProperties^ that);

	private:

		TextDocument^ _parent;

	};
} NAMESPACE_CLOSE

#endif