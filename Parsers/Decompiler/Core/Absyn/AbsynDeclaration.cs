#region License
/* 
 * Copyright (C) 1999-2015 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Decompiler.Core.Code;
using Decompiler.Core.Expressions;
using System;

namespace Decompiler.Core.Absyn
{
	public class AbsynDeclaration : AbsynStatement
	{
		public AbsynDeclaration(Identifier id, Expression expr)
		{
			this.Identifier = id;
			this.Expression = expr;
		}

        public Identifier Identifier { get; private set; }
        public Expression Expression { get; private set; }
        
        public override void Accept(IAbsynVisitor visitor)
		{
			visitor.VisitDeclaration(this);
		}
	}
}