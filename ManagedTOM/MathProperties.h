// DocumentProperties.h

#pragma once
#ifdef _TOM2

#include "Stdafx.h"
#include <TOM.h>
#include "enums.h"

#undef DocumentProperties

using namespace System;

namespace NAMESPACE_DECL {

	ref class TextDocument;

	/// <summary>
	/// (TOM 2 only) 
	/// Provides access to the math properties for a <see cref="TextDocument"/>.
	/// </summary>
	public ref class MathProperties {

	public:

		/// <summary>
		/// Display-mode alignment.
		/// </summary>
		property MathDispAlign DispAlign {
			MathDispAlign get();
			void set(MathDispAlign value);
		}
		/// <summary>
		/// Display-mode integral limits location.
		/// </summary>
		property bool DispIntUnderOver {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Display-mode nested fraction script size.
		/// </summary>
		property bool DispFracTeX {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Math-paragraph n-ary grow.
		/// </summary>
		property bool DispNaryGrow {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Empty arguments display.
		/// </summary>
		property MathDocEmptyArg DocEmptyArg {
			MathDocEmptyArg get();
			void set(MathDocEmptyArg value);
		}
		/// <summary>
		/// Display the underscore (_) and caret (^) as themselves.
		/// </summary>
		property bool DocSbSpOpUnchanged {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Style for math differentials.
		/// </summary>
		property MathDocDiff DocDiff {
			MathDocDiff get();
			void set(MathDocDiff value);
		}
		/// <summary>
		/// Math-paragraph non-integral n-ary limits location.
		/// </summary>
		property bool DispNarySubSup {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Math-paragraph spacing defaults.
		/// </summary>
		property bool DispDef {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Enable right-to-left (RTL) math zones in RTL paragraphs.
		/// </summary>
		property bool EnableRtl {
			bool get();
			void set(bool value);
		}
		/// <summary>
		/// Equation line break position.
		/// </summary>
		property MathBrkBin BrkBin {
			MathBrkBin get();
			void set(MathBrkBin value);
		}
		/// <summary>
		/// Duplicate minus operator.
		/// </summary>
		property MathBrkBinSub BrkBinSub {
			MathBrkBinSub get();
			void set(MathBrkBinSub value);
		}

	internal:

		MathProperties(TextDocument^ parent);
		MathProperties(const MathProperties^ that);

	private:

		TextDocument^ _parent;

	};
} NAMESPACE_CLOSE

#endif