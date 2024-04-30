// TextRow.h

#pragma once
#ifdef _TOM2

#include "Stdafx.h"
#include <Windows.h>
#include <Richedit.h>
#include <TOM.h>

// Generates a getter for property x of type y and underlying type z
#define MAKE_GETTER(x,y,z)									\
	y get() {												\
		z value;											\
		HRESULT hr = _row->Get##x(&value);					\
		THROW_HRESULT(hr);									\
															\
		return (y)value;									\
	}														

// Generates property x of type y and underlying type z
#define MAKE_PROPERTY(x,y,z)								\
property y x {												\
	MAKE_GETTER(x,y,z)										\
	void set(y value) {										\
		HRESULT hr = _row->Set##x((z)value);				\
		THROW_HRESULT(hr);									\
	}														\
}

// Generates property x of type y whose underlying type is long
#define MAKE_PROPERTY_LONG(x,y) MAKE_PROPERTY(x,y,long)

// Generates cell style property x of type y with default value z
#define MAKE_CELL_PROPERTY(x,y,z)							\
property y x {												\
	y get() {												\
		if (CellIndex < 0) return z;						\
															\
		long value;											\
		HRESULT hr = _row->Get##x(&value);					\
		THROW_HRESULT(hr);									\
															\
		return (y)value;									\
	}														\
	void set(y value) {										\
		if (CellIndex < 0) throw gcnew InvalidOperationException("Cell index has not been set."); \
		HRESULT hr = _row->Set##x((long)value);				\
		THROW_HRESULT(hr);									\
	}														\
}

namespace NAMESPACE_DECL {

	ref class TextRange;

	/// <summary>
	/// (TOM 2 only) 
	/// Provides methods to insert one or more identical table rows, and to retrieve and change table row properties.
	/// </summary>
	/// <remarks>
	/// Managed wrapper class for the COM interface ITextRow.
	/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/hh768670(v=vs.85).aspx">ITextRow interface</seealso>
	/// </remarks>
	public ref class TextRow : public IDisposable, public IEquatable<TextRow^> {

	public:

		/// <summary>
		/// Applies the formatting attributes of this text row object to the specified rows in the associated <see cref="TextRange"/>.
		/// </summary>
		/// <param name="count">The number of rows to apply this text row object to.</param>
		/// <param name="options">How the formatting attributes are applied.</param>
		void Apply(int count, [Optional] RowApplyOptions options) {
			HRESULT hr = _row->Apply(count, (long)options);
			THROW_HRESULT(hr);
		}
		/// <summary>
		/// Determines whether changes can be made to this row.
		/// </summary>
		property bool CanChange {
			bool get() {
				long value;
				HRESULT hr = _row->CanChange(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
		}
		/// <summary>
		/// Gets or sets the horizontal alignment of the row.
		/// </summary>
		MAKE_PROPERTY_LONG(Alignment,TextAlignment);
		/// <summary>
		/// Gets or sets the vertical alignment of the active cell.
		/// </summary>
		MAKE_CELL_PROPERTY(CellAlignment,int,0);
		/// <summary>
		/// Gets or sets the background color of the active cell.
		/// </summary>
		property Color CellBackColor {
			Color get() {
				if (CellIndex < 0) return Color::Empty;

				long value;
				HRESULT hr = _row->GetCellColorBack(&value);
				THROW_HRESULT(hr);
				
				return ConvertColor(value);
			}
			void set (Color value) {
				HRESULT hr = _row->SetCellColorBack(ColorTranslator::ToWin32(value));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the foreground color of the active cell.
		/// </summary>
		property Color CellForeColor {
			Color get() {
				if (CellIndex < 0) return Color::Empty;

				long value;
				HRESULT hr = _row->GetCellColorFore(&value);
				THROW_HRESULT(hr);
				
				return ConvertColor(value);
			}
			void set(Color value) {
				HRESULT hr = _row->SetCellColorFore(ColorTranslator::ToWin32(value));
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the count of cells in this row.
		/// </summary>
		MAKE_PROPERTY_LONG(CellCount,int);
		/// <summary>
		/// Gets or sets the count of cells cached for a row.
		/// </summary>
		MAKE_PROPERTY_LONG(CellCountCache,int);
		/// <summary>
		/// Gets or sets the index of the active cell to get or set parameters for.
		/// </summary>
		property int CellIndex {
			int get() {
				long value;
				HRESULT hr = _row->GetCellIndex(&value);
				THROW_HRESULT(hr);
				return value - 1;
			}
			void set(int value) {
				HRESULT hr = _row->SetCellIndex(value + 1);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets the cell margin for this row.
		/// </summary>
		MAKE_PROPERTY_LONG(CellMargin,int);
		/// <summary>
		/// Gets or sets the merge flags of the active cell.
		/// </summary>
		MAKE_CELL_PROPERTY(CellMergeFlags,NAMESPACE::CellMergeFlags,NAMESPACE::CellMergeFlags::None);
		/// <summary>
		/// Gets or sets the shading of the active cell (in hundredths of a percent).
		/// </summary>
		/// <remarks>
		/// Full shading is given by the value 10000.
		/// </remarks>
		MAKE_CELL_PROPERTY(CellShading,int,0);
		/// <summary>
		/// Gets or sets the vertical-text setting of the active cell.
		/// </summary>
		MAKE_CELL_PROPERTY(CellVerticalText,int,0);
		/// <summary>
		/// Gets or sets the width of the active cell (in twips).
		/// </summary>
		MAKE_CELL_PROPERTY(CellWidth,int,0);
		/// <summary>
		/// Gets or sets the height of the row.
		/// </summary>
		/// <remarks>
		/// A value of 0 indicates autoheight.
		/// </remarks>
		MAKE_PROPERTY_LONG(Height,int);
		/// <summary>
		/// Gets or sets the indent of this row.
		/// </summary>
		MAKE_PROPERTY_LONG(Indent,int);
		/// <summary>
		/// Gets or sets whether this row is allowed to be broken across pages.
		/// </summary>
		property bool KeepTogether {
			bool get() {
				long value;
				HRESULT hr = _row->GetKeepTogether(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
			void set(bool value) {
				HRESULT hr = _row->SetKeepTogether(value ? tomTrue : tomFalse);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets or sets whether this row should appear on the same page as the row that follows it.
		/// </summary>
		property bool KeepWithNext {
			bool get() {
				long value;
				HRESULT hr = _row->GetKeepWithNext(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
			void set(bool value) {
				HRESULT hr = _row->SetKeepWithNext(value ? tomTrue : tomFalse);
				THROW_HRESULT(hr);
			}
		}
		/// <summary>
		/// Gets the nest level of the table.
		/// </summary>
		property int NestLevel {
			MAKE_GETTER(NestLevel,int,long)
		}
		/// <summary>
		/// Gets or sets whether this row has right-to-left orientation.
		/// </summary>
		property bool RTL {
			bool get() {
				long value;
				HRESULT hr = _row->GetRTL(&value);
				THROW_HRESULT(hr);
				return (value == tomTrue);
			}
			void set(bool value) {
				HRESULT hr = _row->SetRTL(value ? tomTrue : tomFalse);
				THROW_HRESULT(hr);
			}
		}

		/// <summary>
		/// Gets the border colors of the active cell.
		/// </summary>
		/// <param name="left">Left border color.</param>
		/// <param name="top">Top border color.</param>
		/// <param name="right">Right border color.</param>
		/// <param name="bottom">Bottom border color.</param>
		void GetCellBorderColors([Out] Color% left, [Out] Color% top, [Out] Color% right, [Out] Color% bottom) {
			if (CellIndex < 0) {
				left = Color(Color::Empty);
				top = Color(Color::Empty);
				right = Color(Color::Empty);
				bottom = Color(Color::Empty);
			}
			else {
				long p0, p1, p2, p3;
				HRESULT hr = _row->GetCellBorderColors(&p0, &p1, &p2, &p3);
				THROW_HRESULT(hr);

				left = ConvertColor(p0);
				top = ConvertColor(p1);
				right = ConvertColor(p2);
				bottom = ConvertColor(p3);
			}
		}

		/// <summary>
		/// Sets the border colors of the active cell.
		/// </summary>
		/// <param name="left">Left border color.</param>
		/// <param name="top">Top border color.</param>
		/// <param name="right">Right border color.</param>
		/// <param name="bottom">Bottom border color.</param>
		void SetCellBorderColors(Color left, Color top, Color right, Color bottom) {
			if (CellIndex < 0) throw gcnew InvalidOperationException("Cell index has not been set.");
			
			long p0 = ColorTranslator::ToWin32(left);
			long p1 = ColorTranslator::ToWin32(top);
			long p2 = ColorTranslator::ToWin32(right);
			long p3 = ColorTranslator::ToWin32(bottom);

			HRESULT hr = _row->SetCellBorderColors(p0, p1, p2, p3);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Gets the border widths of the active cell.
		/// </summary>
		/// <param name="left">Left border width.</param>
		/// <param name="top">Top border width.</param>
		/// <param name="right">Right border width.</param>
		/// <param name="bottom">Bottom border width.</param>
		void GetCellBorderWidths([Out] int% left, [Out] int% top, [Out] int% right, [Out] int% bottom) {
			long p0, p1, p2, p3;
			
			if (CellIndex < 0) {
				p0 = p1 = p2 = p3 = 0;
			}
			else {			
				HRESULT hr = _row->GetCellBorderWidths(&p0, &p1, &p2, &p3);
				THROW_HRESULT(hr);
			}

			left = p0;
			top = p1;
			right = p2;
			bottom = p3;
		}

		/// <summary>
		/// Sets the border widths of the active cell.
		/// </summary>
		/// <param name="left">Left border width.</param>
		/// <param name="top">Top border width.</param>
		/// <param name="right">Right border width.</param>
		/// <param name="bottom">Bottom border width.</param>
		void SetCellBorderWidths(int left, int top, int right, int bottom) {
			if (CellIndex < 0) throw gcnew InvalidOperationException("Cell index has not been set.");
			
			HRESULT hr = _row->SetCellBorderWidths(left, top, right, bottom);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Gets the value of a property.
		/// </summary>
		/// <param name="type">The property ID.</param>
		int GetProperty(int type) {
			long value;
			HRESULT hr = _row->GetProperty(type, &value);
			THROW_HRESULT(hr);

			return value;
		}

		/// <summary>
		/// Sets the value of the specified property.
		/// </summary>
		/// <param name="type">The ID of the property to set.</param>
		/// <param name="value">The new property value.</param>
		void SetProperty(int type, int value) {
			HRESULT hr = _row->SetProperty(type, value);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Inserts one or more rows at the location identified by the associated <see cref="TextRange"/>.
		/// </summary>
		/// <param name="count">The number of rows to insert.</param>
		void Insert(int count) {
			HRESULT hr = _row->Insert(count);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Resets a row.
		/// </summary>
		void Reset() {
			HRESULT hr = _row->Reset(tomRowUpdate);
			THROW_HRESULT(hr);
		}

		/// <summary>
		/// Tests if this <see cref="TextRow"/> is considered equal to another.
		/// </summary>
		/// <param name="other">The <see cref="TextRow"/> to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(TextRow^ other) {
			long result;
			HRESULT hr = _row->IsEqual(other->_row, &result);
			THROW_HRESULT(hr);
			return (result == tomTrue);
		}

		/// <summary>
		/// Tests if this <see cref="TextRow"/> is considered equal to another object.
		/// </summary>
		/// <param name="other">The object to compare to this object.</param>
		/// <returns>true if the objects are considered equal, false if they are not.</returns>
		virtual bool Equals(Object^ other) override {
			TextRow^ row = dynamic_cast<TextRow^>(other);
			if (row != nullptr)
				return Equals(row);
			else
				return Object::Equals(other);
		}

		/// <summary>
		/// Calculates a hash code for this object.
		/// </summary>
		/// <returns>A hash code for this object.</returns>
		virtual int GetHashCode() override {
			Color cl, ct, cr, cb;
			GetCellBorderColors(cl, ct, cr, cb);
			
			int wl, wt, wr, wb;
			GetCellBorderWidths(wl, wt, wr, wb);

			int hash = Alignment.GetHashCode()
				^ CellIndex.GetHashCode()
				^ CellWidth.GetHashCode()
				^ CellCount.GetHashCode()
				^ CellCountCache.GetHashCode()
				^ CellMargin.GetHashCode()
				^ Height.GetHashCode()
				^ Indent.GetHashCode()
				^ KeepTogether.GetHashCode()
				^ KeepWithNext.GetHashCode()
				^ NestLevel.GetHashCode()
				^ RTL.GetHashCode()
				^ cl.GetHashCode()
				^ ct.GetHashCode()
				^ cr.GetHashCode()
				^ cb.GetHashCode()
				^ wl.GetHashCode()
				^ wt.GetHashCode()
				^ wr.GetHashCode()
				^ wb.GetHashCode();

			if (CellIndex >= 0) {
				hash ^= CellAlignment.GetHashCode()
				^ CellBackColor.GetHashCode()
				^ CellForeColor.GetHashCode()
				^ CellMergeFlags.GetHashCode()
				^ CellShading.GetHashCode()
				^ CellVerticalText.GetHashCode();
			}

			return hash;
		}

	internal:

		TextRow(ITextRow* strings) {
			_row = strings;
		}

	protected:

		~TextRow() {
			this->!TextRow();
		}

		!TextRow() {
			if (_row != NULL) {
				_row->Release();
				_row = NULL;
			}
		}

	private:

		static Color ConvertColor(long value) {
			if ((value == tomAutoColor) || (value == tomUndefined))
				return Color::Empty;
			else
				return ColorTranslator::FromWin32(value);
		}

		ITextRow* _row;

	};
} NAMESPACE_CLOSE

#undef MAKE_GETTER
#undef MAKE_PROPERTY
#undef MAKE_PROPERTY_LONG
#undef MAKE_CELL_PROPERTY

#endif