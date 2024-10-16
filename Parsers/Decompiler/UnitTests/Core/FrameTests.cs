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

using Decompiler.Arch.X86;
using Decompiler.Core;
using Decompiler.Core.Code;
using Decompiler.Core.Expressions;
using Decompiler.Core.Types;
using NUnit.Framework;
using System;
using System.IO;

namespace Decompiler.UnitTests.Core
{
	[TestFixture]
	public class FrameTests
	{
        private IntelArchitecture arch;

        [SetUp]
        public void Setup()
        {
            arch = new IntelArchitecture(ProcessorMode.Real);
        }
		[Test]
		public void RegisterTest()
		{
			Frame f = new Frame(PrimitiveType.Word16);
			Identifier id0 = f.EnsureRegister(Registers.ax);
			Identifier id1 = f.EnsureRegister(Registers.bx);
			Identifier id2 = f.EnsureRegister(Registers.ax);
			Assert.AreEqual(id0, id2);
		}

		[Test]
		public void SequenceTest()
		{
			IntelArchitecture arch = new IntelArchitecture(ProcessorMode.Real);
			Frame f = new Frame(PrimitiveType.Word16);
			Identifier ax = f.EnsureRegister(Registers.ax);
			Identifier dx = f.EnsureRegister(Registers.dx);
			Identifier dxax = f.EnsureSequence(dx, ax, PrimitiveType.Word32);

			using (FileUnitTester fut = new FileUnitTester("Core/SequenceTest.txt"))
			{
				f.Write(fut.TextWriter);
				fut.AssertFilesEqual();
			}

			Identifier dxax2 = f.EnsureSequence(dx,ax, PrimitiveType.Word32);
			Assert.IsTrue(dxax2 == dxax);
		}

		[Test]
		public void FrGrfTest()
		{
			IntelArchitecture arch = new IntelArchitecture(ProcessorMode.Real);
			Frame f = new Frame(PrimitiveType.Word16);
			uint iSz = (uint) (FlagM.ZF|FlagM.SF);
			Identifier grfSz = f.EnsureFlagGroup(iSz, arch.GrfToString(iSz), PrimitiveType.Byte);
			using (FileUnitTester fut = new FileUnitTester("Core/FrGrfTest.txt"))
			{
				f.Write(fut.TextWriter);
				fut.AssertFilesEqual();
			}
		}


		[Test]
		public void FrLocals()
		{
			Frame f = new Frame(PrimitiveType.Word16);
			f.EnsureStackLocal(2, PrimitiveType.Word16);
			f.EnsureStackLocal(4, PrimitiveType.Word32);
			using (FileUnitTester fut = new FileUnitTester("Core/FrLocals.txt"))
			{
				f.Write(fut.TextWriter);
				fut.AssertFilesEqual();
			}
			Assert.IsNotNull((StackLocalStorage) f.Identifiers[2].Storage);
		}

		[Test]
		public void FrSequenceAccess()
		{
			Frame f = new Frame(PrimitiveType.Word16);
			Identifier ax = f.EnsureRegister(Registers.ax);
			Identifier dx = f.EnsureRegister(Registers.dx);
			Identifier dx_ax = f.EnsureSequence(dx, ax, PrimitiveType.Word32);
			SequenceStorage vDx_ax = (SequenceStorage) dx_ax.Storage;
			using (FileUnitTester fut = new FileUnitTester("Core/FrSequenceAccess.txt"))
			{
				f.Write(fut.TextWriter);
				fut.TextWriter.WriteLine("Head({0}) = {1}", dx_ax.Name, vDx_ax.Head.Name);
				fut.TextWriter.WriteLine("Tail({0}) = {1}", dx_ax.Name, vDx_ax.Tail.Name);

				fut.AssertFilesEqual();
			}
		}

		[Test]
        [Ignore()]
		public void FrBindStackParameters()
		{
			Frame f = new Frame(PrimitiveType.Word16);
			f.ReturnAddressSize = 4;						// far call.
			int stack = 2;
			Identifier loc02 = f.EnsureStackLocal(-stack, PrimitiveType.Word16, "wLoc02");
			stack += loc02.DataType.Size;
			Identifier loc04 = f.EnsureStackLocal(-stack, PrimitiveType.Word16, "wLoc04");

			ProcedureSignature sig = new ProcedureSignature(
				null, new Identifier[] {
					new Identifier("arg0", PrimitiveType.Word16, new StackArgumentStorage(4, PrimitiveType.Word16)),
					new Identifier("arg1", PrimitiveType.Word16, new StackArgumentStorage(6, PrimitiveType.Word16)) });

			var cs = new CallSite(f.ReturnAddressSize + 2 * 4, 0);
			var fn = new ProcedureConstant(PrimitiveType.Pointer32, new PseudoProcedure("foo", sig));
			ApplicationBuilder ab = new ApplicationBuilder(arch, f, cs, fn, sig, true);
            Instruction instr = ab.CreateInstruction(); 
			using (FileUnitTester fut = new FileUnitTester("Core/FrBindStackParameters.txt"))
			{
				f.Write(fut.TextWriter);
				fut.TextWriter.WriteLine(instr.ToString());
				fut.AssertFilesEqual();
			}
		}

		[Test]
		public void FrBindMixedParameters()
		{
			Frame f = new Frame(PrimitiveType.Word16);
			Identifier ax = f.EnsureRegister(Registers.ax);
			Identifier cx = f.EnsureRegister(Registers.cx);
			int stack = PrimitiveType.Word16.Size;
			Identifier arg1 = f.EnsureStackLocal(-stack, PrimitiveType.Word16);

			ProcedureSignature sig = new ProcedureSignature(
				ax,
			    cx,
			    new Identifier("arg0", PrimitiveType.Word16, new StackArgumentStorage(0, PrimitiveType.Word16)));
			
			var cs = new CallSite(stack, 0);
			ProcedureConstant fn = new ProcedureConstant(PrimitiveType.Pointer32, new PseudoProcedure("bar", sig));
			ApplicationBuilder ab = new ApplicationBuilder(arch, f, cs, fn, sig, true);
            Instruction instr = ab.CreateInstruction();
			using (FileUnitTester fut = new FileUnitTester("Core/FrBindMixedParameters.txt"))
			{
				f.Write(fut.TextWriter);
				fut.TextWriter.WriteLine(instr.ToString());
				fut.AssertFilesEqual();
			}
		}

		[Test]
		public void FrFpuStack()
		{
			Frame f = new Frame(PrimitiveType.Word16);
			f.EnsureFpuStackVariable(-1, PrimitiveType.Real64);
			f.EnsureFpuStackVariable(-2, PrimitiveType.Real64);
			f.EnsureFpuStackVariable(0, PrimitiveType.Real64);
			
			using (FileUnitTester fut = new FileUnitTester("Core/FrFpuStack.txt"))
			{
				f.Write(fut.TextWriter);
				fut.AssertFilesEqual();
			}
		}

		[Test]
		public void FrEnsureRegister()
		{
			Frame f = new Frame(PrimitiveType.Word32);
			f.EnsureRegister(new Mocks.MockMachineRegister("eax", 0, PrimitiveType.Word32));
			Assert.AreEqual("eax", f.Identifiers[2].Name);
			Assert.AreSame(PrimitiveType.Word32, f.Identifiers[2].DataType);
		}

		[Test]
		public void EnsureOutRegister()
		{
			Frame f = new Frame(PrimitiveType.Word32);
			Identifier r = f.EnsureRegister(new Mocks.MockMachineRegister("r1", 1, PrimitiveType.Word32));
			Identifier arg = f.EnsureOutArgument(r, PrimitiveType.Pointer32);
			Assert.AreEqual("r1Out", arg.Name);
			Assert.AreSame(PrimitiveType.Pointer32, arg.DataType);
		}
	}
}
