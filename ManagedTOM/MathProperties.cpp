// DocumentProperties.cpp

#pragma once
#ifdef _TOM2

#include "MathProperties.h"
#include "TextDocument.h"

using namespace System;

namespace NAMESPACE_DECL {

	MathDispAlign MathProperties::DispAlign::get() {
		return (MathDispAlign)(_parent->GetMathProperties() & tomMathDispAlignMask);
	}
	void MathProperties::DispAlign::set(MathDispAlign value) {
		_parent->SetMathProperty(tomMathDispAlignMask, (int)value);
	}

	bool MathProperties::DispIntUnderOver::get() {
		return (_parent->GetMathProperties() & tomMathDispIntUnderOver) == tomMathDispIntUnderOver;
	}
	void MathProperties::DispIntUnderOver::set(bool value) {
		_parent->SetMathProperty(tomMathDispIntUnderOver, value ? tomMathDispIntUnderOver : 0);
	}

	bool MathProperties::DispFracTeX::get() {
		return (_parent->GetMathProperties() & tomMathDispFracTeX) == tomMathDispFracTeX;
	}
	void MathProperties::DispFracTeX::set(bool value) {
		_parent->SetMathProperty(tomMathDispFracTeX, value ? tomMathDispFracTeX : 0);
	}

	bool MathProperties::DispNaryGrow::get() {
		return (_parent->GetMathProperties() & tomMathDispNaryGrow) == tomMathDispNaryGrow;
	}
	void MathProperties::DispNaryGrow::set(bool value) {
		_parent->SetMathProperty(tomMathDispNaryGrow, value ? tomMathDispNaryGrow : 0);
	}

	MathDocEmptyArg MathProperties::DocEmptyArg::get() {
		return (MathDocEmptyArg)(_parent->GetMathProperties() & tomMathDocEmptyArgMask);
	}
	void MathProperties::DocEmptyArg::set(MathDocEmptyArg value) {
		_parent->SetMathProperty(tomMathDocEmptyArgMask, (int)value);
	}

	bool MathProperties::DocSbSpOpUnchanged::get() {
		return (_parent->GetMathProperties() & tomMathDocSbSpOpUnchanged) == tomMathDocSbSpOpUnchanged;
	}
	void MathProperties::DocSbSpOpUnchanged::set(bool value) {
		_parent->SetMathProperty(tomMathDocSbSpOpUnchanged, value ? tomMathDocSbSpOpUnchanged : 0);
	}

	MathDocDiff MathProperties::DocDiff::get() {
		return (MathDocDiff)(_parent->GetMathProperties() & tomMathDocDiffMask);
	}
	void MathProperties::DocDiff::set(MathDocDiff value) {
		_parent->SetMathProperty(tomMathDocDiffMask, (int)value);
	}

	bool MathProperties::DispNarySubSup::get() {
		return (_parent->GetMathProperties() & tomMathDispNarySubSup) == tomMathDispNarySubSup;
	}
	void MathProperties::DispNarySubSup::set(bool value) {
		_parent->SetMathProperty(tomMathDispNarySubSup, value ? tomMathDispNarySubSup : 0);
	}

	bool MathProperties::DispDef::get() {
		return (_parent->GetMathProperties() & tomMathDispDef) == tomMathDispDef;
	}
	void MathProperties::DispDef::set(bool value) {
		_parent->SetMathProperty(tomMathDispDef, value ? tomMathDispDef : 0);
	}

	bool MathProperties::EnableRtl::get() {
		return (_parent->GetMathProperties() & tomMathEnableRtl) == tomMathEnableRtl;
	}
	void MathProperties::EnableRtl::set(bool value) {
		_parent->SetMathProperty(tomMathEnableRtl, value ? tomMathEnableRtl : 0);
	}

	MathBrkBin MathProperties::BrkBin::get() {
		return (MathBrkBin)(_parent->GetMathProperties() & tomMathBrkBinMask);
	}
	void MathProperties::BrkBin::set(MathBrkBin value) {
		_parent->SetMathProperty(tomMathBrkBinMask, (int)value);
	}

	MathBrkBinSub MathProperties::BrkBinSub::get() {
		return (MathBrkBinSub)(_parent->GetMathProperties() & tomMathBrkBinSubMask);
	}
	void MathProperties::BrkBinSub::set(MathBrkBinSub value) {
		_parent->SetMathProperty(tomMathBrkBinSubMask, (int)value);
	}

	MathProperties::MathProperties(TextDocument^ parent) {
		_parent = parent;
	}

	MathProperties::MathProperties(const MathProperties^ that) {
		_parent = that->_parent;
	}
} NAMESPACE_CLOSE

#endif