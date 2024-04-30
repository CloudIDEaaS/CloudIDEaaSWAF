// PreferredFontInfo.h

#pragma once
#ifdef _TOM2

#include "Stdafx.h"

using namespace System;

namespace NAMESPACE_DECL {
	
	/// <summary>
	/// (TOM 2 only) 
	/// Contains preferred font information that was requested for a particular document. 
	/// </summary>
	public ref class PreferredFontInfo {

	public:

		/// <summary>
		/// Gets the font name.
		/// </summary>
		property String^ FontName {
			String^ get() {
				return _fontName;
			}
		}

		/// <summary>
		/// Gets the pitch and family of the font.
		/// </summary>
		property int PitchAndFamily {
			int get() {
				return _pitchAndFamily;
			}
		}

		/// <summary>
		/// Gets the font size.
		/// </summary>
		property int FontSize {
			int get() {
				return _fontSize;
			}
		}

	internal:

		PreferredFontInfo() {
			_fontName = String::Empty;
			_pitchAndFamily = 0;
			_fontSize = 0;
		}

		PreferredFontInfo(String^ fontName, int pitchAndFamily, int fontSize) {
			_fontName = fontName;
			_pitchAndFamily = pitchAndFamily;
			_fontSize = fontSize;
		}

	private:

		String^ _fontName;
		int _pitchAndFamily;
		int _fontSize;
	};
} NAMESPACE_CLOSE

#endif