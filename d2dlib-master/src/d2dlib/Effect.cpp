/*
* MIT License
*
* Copyright (c) 2009-2021 Jingwood, unvell.com. All right reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

#include "stdafx.h"
#include "Effect.h"
#include "d2d1effects.h"
#include "d2d1_1.h";

HANDLE CreateEffect(HANDLE ctx, REFCLSID effect)
{
	RetrieveContext(ctx);

	ID2D1HwndRenderTarget* renderTarget = (ID2D1HwndRenderTarget*) context->renderTarget;
	ID2D1Effect* compositeEffect;
	ID2D1DeviceContext* deviceContext;
	HRESULT hr;

	hr = renderTarget->QueryInterface(&deviceContext);

	hr = deviceContext->CreateEffect(effect, &compositeEffect);

	return (HANDLE)compositeEffect;
}

void SetInput(HANDLE d2dEffect, INT index, HANDLE d2dbitmap, BOOL invalidate)
{
	ID2D1Effect* effect = reinterpret_cast<ID2D1Effect*>(d2dEffect);
	ID2D1Bitmap* bitmap = reinterpret_cast<ID2D1Bitmap*>(d2dbitmap);

	effect->SetInput(index, bitmap, invalidate);
}