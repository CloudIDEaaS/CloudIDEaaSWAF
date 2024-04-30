// DocumentProperties.cpp

#pragma once
#ifdef _TOM2

#include "DocumentProperties.h"
#include "TextDocument.h"

using namespace System;

namespace NAMESPACE_DECL {

	bool DocumentProperties::CanCopy::get() {
		return _parent->GetProperty(tomCanCopy) == tomTrue;
	}
	void DocumentProperties::CanCopy::set(bool value) {
		_parent->SetProperty(tomCanCopy, value ? tomTrue : tomFalse);
	}

	bool DocumentProperties::CanRedo::get() {
		return _parent->GetProperty(tomCanRedo) == tomTrue;
	}
	void DocumentProperties::CanRedo::set(bool value) {
		_parent->SetProperty(tomCanRedo, value ? tomTrue : tomFalse);
	}

	bool DocumentProperties::CanUndo::get() {
		return _parent->GetProperty(tomCanUndo) == tomTrue;
	}
	void DocumentProperties::CanUndo::set(bool value) {
		_parent->SetProperty(tomCanUndo, value ? tomTrue : tomFalse);
	}

	MathBuildFlags DocumentProperties::DocMathBuild::get() {
		return (MathBuildFlags)_parent->GetProperty(tomDocMathBuild);
	}
	void DocumentProperties::DocMathBuild::set(MathBuildFlags value) {
		_parent->SetProperty(tomDocMathBuild, (int)value);
	}

	int DocumentProperties::MathInterSpace::get() {
		return _parent->GetProperty(tomMathInterSpace);
	}
	void DocumentProperties::MathInterSpace::set(int value) {
		_parent->SetProperty(tomMathInterSpace, value);
	}

	int DocumentProperties::MathIntraSpace::get() {
		return _parent->GetProperty(tomMathIntraSpace);
	}
	void DocumentProperties::MathIntraSpace::set(int value) {
		_parent->SetProperty(tomMathIntraSpace, value);
	}

	int DocumentProperties::MathLMargin::get() {
		return _parent->GetProperty(tomMathLMargin);
	}
	void DocumentProperties::MathLMargin::set(int value) {
		_parent->SetProperty(tomMathLMargin, value);
	}


	int DocumentProperties::MathPostSpace::get() {
		return _parent->GetProperty(tomMathPostSpace);
	}
	void DocumentProperties::MathPostSpace::set(int value) {
		_parent->SetProperty(tomMathPostSpace, value);
	}

	int DocumentProperties::MathPreSpace::get() {
		return _parent->GetProperty(tomMathPreSpace);
	}
	void DocumentProperties::MathPreSpace::set(int value) {
		_parent->SetProperty(tomMathPreSpace, value);
	}

	int DocumentProperties::MathRMargin::get() {
		return _parent->GetProperty(tomMathRMargin);
	}
	void DocumentProperties::MathRMargin::set(int value) {
		_parent->SetProperty(tomMathRMargin, value);
	}

	int DocumentProperties::MathWrapIndent::get() {
		return _parent->GetProperty(tomMathWrapIndent);
	}
	void DocumentProperties::MathWrapIndent::set(int value) {
		_parent->SetProperty(tomMathWrapIndent, value);
	}

	int DocumentProperties::MathWrapRight::get() {
		return _parent->GetProperty(tomMathWrapRight);
	}
	void DocumentProperties::MathWrapRight::set(int value) {
		_parent->SetProperty(tomMathWrapRight, value);
	}

	int DocumentProperties::UndoLimit::get() {
		return _parent->GetProperty(tomUndoLimit);
	}
	void DocumentProperties::UndoLimit::set(int value) {
		_parent->SetProperty(tomUndoLimit, value);
	}

	TextEllipsisMode DocumentProperties::EllipsisMode::get() {
		return (TextEllipsisMode)_parent->GetProperty(tomEllipsisMode);
	}
	void DocumentProperties::EllipsisMode::set(TextEllipsisMode value) {
		_parent->SetProperty(tomEllipsisMode, (int)value);
	}

	TextEllipsisState DocumentProperties::EllipsisState::get() {
		return (TextEllipsisState)_parent->GetProperty(tomEllipsisState);
	}
	void DocumentProperties::EllipsisState::set(TextEllipsisState value) {
		_parent->SetProperty(tomEllipsisState, (int)value);
	}

	DocumentProperties::DocumentProperties(TextDocument^ parent) {
		_parent = parent;
	}

	DocumentProperties::DocumentProperties(const DocumentProperties^ that) {
		_parent = that->_parent;
	}
} NAMESPACE_CLOSE

#endif