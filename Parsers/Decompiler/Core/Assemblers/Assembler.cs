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

using Decompiler;
using System;
using System.IO;
using System.Collections.Generic;

namespace Decompiler.Core.Assemblers
{
	public interface Assembler
	{
        Program Assemble(Address baseAddress, TextReader reader);
        Program AssembleFragment(Address baseAddress, string fragment);
		Address StartAddress { get; }
        ICollection<EntryPoint> EntryPoints { get; }
        Dictionary<Address, ImportReference> ImportReferences { get; }
    }
}
