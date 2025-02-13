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

using Decompiler.Core;
using Decompiler.Core.Types;
using Decompiler.Typing;
using NUnit.Framework;
using System;

namespace Decompiler.UnitTests.Typing
{
	[TestFixture]
	public class UnifierTests
	{
		private Unifier un;
		private TypeFactory factory;

		[SetUp]
		public void Setup()
		{
			factory = new TypeFactory();
			un = new Unifier(factory);
		}

		[Test]
		public void UnifyInt32()
		{
			DataType d = un.Unify(PrimitiveType.Word32, PrimitiveType.Word32);
			PrimitiveType p = (PrimitiveType) d;
			Assert.AreEqual(PrimitiveType.Word32, p);
		}

		[Test]
		public void UnifyIntShort()
		{
			DataType d = un.Unify(PrimitiveType.Word16, PrimitiveType.Word32);
			UnionType u = (UnionType) d;
			Assert.AreEqual(2, u.Alternatives.Count);
		}

		[Test]
		public void UnifyIntReal()
		{
			DataType d = un.Unify(PrimitiveType.Real32, PrimitiveType.Word32);
			PrimitiveType p = (PrimitiveType) d;
			Assert.AreEqual(PrimitiveType.Real32, p);

			d = un.Unify(PrimitiveType.Real64, PrimitiveType.Word32);
			UnionType u = (UnionType) d;
			Assert.AreEqual(2, u.Alternatives.Count);
		}

		[Test]
		public void UnifyStructs()
		{
            StructureType m1 = new StructureType { Fields = { { 4, PrimitiveType.Word32 } } };
            StructureType m2 = new StructureType { Fields = { { 8, PrimitiveType.Word32 } } };

			StructureType m = (StructureType) un.Unify(m1, m2);
			Assert.AreEqual(2, m.Fields.Count);
		}

		[Test]
		public void UnifyStructsSameSize()
		{
			StructureType s1 = new StructureType(null, 20);
			StructureType s2 = new StructureType(null, 20);
			StructureType m = (StructureType) un.Unify(s1, s2);
			Assert.AreEqual(20, m.Size);
		}

		[Test]
		public void UnifyStructTypevars()
		{
            StructureType m1 = new StructureType(null, 0) { Fields = { { 4, factory.CreateTypeVariable() } } };
            StructureType m2 = new StructureType(null, 0) { Fields = { { 4, factory.CreateTypeVariable() } } };

			StructureType m = (StructureType) un.Unify(m1, m2);
			Assert.AreEqual(1, m.Fields.Count);
			UnionType u = (UnionType) m.Fields[0].DataType;
			Assert.AreEqual(2, u.Alternatives.Count);
		}

		[Test]
		public void UnifyNull()
		{
			PrimitiveType p = PrimitiveType.Word32;
			DataType d = un.Unify(null, p);
			Assert.AreSame(d, p);

			d = un.Unify(p, null);
			Assert.AreEqual(d, p);
		}

		[Test]
		public void UnifyEmptyStruct()
		{
            StructureType m1 = new StructureType(null, 0) { Fields = { { 4, PrimitiveType.Word32 } } };
			StructureType m2 = new StructureType(null, 0);

			StructureType m = (StructureType) un.Unify(m1, m2);
			Assert.AreEqual(1, m.Fields.Count);
		}

		[Test]
		public void UnifyTypeVars()
		{
			TypeVariable tv1 = new TypeVariable(1);
			TypeVariable tv2 = new TypeVariable(2);

			UnionType u = (UnionType) un.Unify(tv1, tv2);
			Assert.AreEqual(2, u.Alternatives.Count);

			TypeVariable tv3 = new TypeVariable(3);
			TypeVariable tv = (TypeVariable) un.Unify(tv3, tv3);
			Assert.AreEqual(3, tv.Number);
		}

		[Test]
		public void UnifySameOffsets()
		{
			StructureType m1 = new StructureType(null, 0);
			m1.Fields.Add(new StructureField(4, PrimitiveType.Word32));
			m1.Fields.Add(new StructureField(8, PrimitiveType.Int32));
			m1.Fields.Add(new StructureField(0x0C, PrimitiveType.Int32));

			StructureType m2 = new StructureType(null, 0);
			m2.Fields.Add(new StructureField(4, PrimitiveType.Word32));
			m2.Fields.Add(new StructureField(8, PrimitiveType.Int32));
			m2.Fields.Add(new StructureField(0x0C, PrimitiveType.Real32));

			StructureType m = (StructureType) un.Unify(m1, m2);
			Assert.AreEqual(3, m.Fields.Count);

			StructureField f = m.Fields[0];
			PrimitiveType p = (PrimitiveType) f.DataType;
			Assert.AreEqual(PrimitiveType.Word32, p);

			f = m.Fields[1];
			p = (PrimitiveType) f.DataType;
			Assert.AreEqual(PrimitiveType.Int32, p);

			f = m.Fields[2];
			UnionType u = (UnionType) f.DataType;
			Assert.AreEqual(2, u.Alternatives.Count);
		}

		[Test]
		public void UnifyUnion()
		{
			PrimitiveType p1 = PrimitiveType.Real32;
			PrimitiveType p2 = PrimitiveType.UInt32;
			UnionType u1 = (UnionType) un.Unify(p1, p2);
			PrimitiveType p3 = PrimitiveType.Bool;
			UnionType u2 = (UnionType) un.Unify(u1, p3);
			Assert.AreEqual(3, u2.Alternatives.Count);
			Assert.AreEqual("bool", u2.Alternatives.Values[0].DataType.ToString());
			Assert.AreEqual("uint32", u2.Alternatives.Values[1].DataType.ToString());
			Assert.AreEqual("real32", u2.Alternatives.Values[2].DataType.ToString());
		}

		[Test]
		public void UnifyUnionMem()
		{
			TypeVariable tv1 = new TypeVariable(1);
			TypeVariable tv2 = new TypeVariable(2);
            DataType dt = un.Unify(
                new StructureType { Fields = { { 0, tv1 } } },
                new StructureType { Fields = { { 4, tv1 } } });

			StructureType mem = (StructureType) dt;
			Assert.AreEqual(2, mem.Fields.Count);
			Assert.IsNotNull((TypeVariable) mem.Fields[0].DataType);
			Assert.IsNotNull((TypeVariable) mem.Fields[1].DataType);
            dt = un.Unify(
                dt,
                new StructureType { Fields = { { 0, PrimitiveType.Word32 } } });

			mem = (StructureType) dt;
			Assert.AreEqual(2, mem.Fields.Count);
			Assert.IsNotNull((UnionType) mem.Fields[0].DataType);
			Assert.IsNotNull((TypeVariable) mem.Fields[1].DataType);
		}

		[Test]
		public void UnifyUnknownInt()
		{
			DataType dt = un.Unify(PrimitiveType.Int32, factory.CreateUnknown());
			PrimitiveType p = (PrimitiveType) dt;
			Assert.AreEqual("int32", dt.ToString());
		}

		[Test]
		public void UnifyPtrWord()
		{
			Pointer ptr = factory.CreatePointer(PrimitiveType.Word32, 4);
			DataType dt = un.Unify(ptr, PrimitiveType.Word32);
			Assert.AreEqual("(ptr word32)", dt.ToString());
			Assert.IsFalse(Object.ReferenceEquals(ptr, dt), "Should be different");
		}

		// Ensures that if a named but sizeless structure is unified with an unnamed one, the resulting structure keeps the name.
		[Test]
		public void UnifyStructNamedStruct()
		{
			StructureType st1 = new StructureType("foo", 0);
            StructureType st2 = new StructureType { Fields = { { 0, PrimitiveType.Word32, "bar" } } };
			StructureType st = (StructureType) un.Unify(st1, st2);
			Assert.AreEqual("foo", st.Name);
		}

		// Arrays with the same sized elements should unify just fine.
		[Test]
		public void UnifyArrays()
		{
			ArrayType a1 = new ArrayType(PrimitiveType.Word32, 0);
			ArrayType a2 = new ArrayType(PrimitiveType.Word32, 0);
			DataType dt = un.Unify(a1, a2);
			Assert.AreEqual("(arr word32)", dt.ToString());
		}

		// Arrays with struct fields should unify too.
		[Test]
		public void UnifyArrays2()
		{
            ArrayType a1 = new ArrayType(new StructureType(null, 10) { Fields = { { 0, new TypeVariable(1), "t1" } } }, 0);
            ArrayType a2 = new ArrayType(new StructureType(null, 10) { Fields = { { 4, new TypeVariable(2), "t2" } } }, 0);
			DataType dt = un.Unify(a1, a2);
			Assert.AreEqual("(arr (struct 000A (0 T_1 t1) (4 T_2 t2)))", dt.ToString());
		}

		// If an array has a size but the other one doesn't, the one with a size wins.
		[Test]
		public void UnifySizedArrays()
		{
			ArrayType a1 = new ArrayType(PrimitiveType.Word32, 10);
			ArrayType a2 = new ArrayType(PrimitiveType.Word32, 0);
			DataType dt = un.Unify(a1, a2);
			Assert.AreEqual("(arr word32 10)", dt.ToString());
		}

		[Test]
		public void UnifyStructInt()
		{
			StructureType s1 = new StructureType(null, 30);
			PrimitiveType p = PrimitiveType.Int32;
			DataType dt = un.Unify(s1, p);
			Assert.AreEqual("(struct 001E (0 int32 dw0000))", dt.ToString());
		}

		[Test]
		public void UnifyArrayInt()
		{
			ArrayType a = new ArrayType(PrimitiveType.Word32, 0);
			PrimitiveType p = PrimitiveType.Int32;
			DataType dt = un.Unify(a, p);
			Assert.AreEqual("(arr int32)", dt.ToString());
		}

		[Test]
		public void UnifyWordMembptr()
		{
			MemberPointer mp = new MemberPointer(new StructureType("foo", 4), PrimitiveType.Int32, 2);
			DataType dt = un.Unify(mp, PrimitiveType.Word16);
			Assert.AreEqual("(memptr (struct \"foo\" 0004) int32)", dt.ToString());
		}

		[Test]
		public void UnifyPointerStructSegment()
		{
			Pointer p = new Pointer(new StructureType{ Fields = { { 4, PrimitiveType.UInt32} } }, 2);
			DataType dt = un.Unify(p, PrimitiveType.SegmentSelector);
			Assert.AreEqual("(ptr (struct (4 uint32 dw0004)))", dt.ToString());
		}

		[Test]
		public void UnifyWordMemptr()
		{
			TypeVariable tv = new TypeVariable(1);
			TypeVariable tv2 = new TypeVariable(2);
			TypeVariable tv3 = new TypeVariable(3);
			MemberPointer mp2 = new MemberPointer(tv, tv2, 0);
			MemberPointer mp3 = new MemberPointer(tv, tv3, 0);
			UnionType ut = new UnionType(null, null);
			un.UnifyIntoUnion(ut, mp2);
			un.UnifyIntoUnion(ut, mp3);
			un.UnifyIntoUnion(ut, PrimitiveType.Word16);

			Assert.AreEqual("(union ((memptr T_1 T_2) u0) ((memptr T_1 T_3) u1))", ut.ToString());
		}

		[Test]
		public void UnifyPtrHybrid()
		{
			Pointer p = new Pointer(new StructureType(null, 32), 4);
			PrimitiveType hybrid = PrimitiveType.Create(Domain.SignedInt|Domain.UnsignedInt|Domain.Pointer, 4);
			DataType dt = un.Unify(p, hybrid);
			Assert.AreEqual("(ptr (struct 0020))", dt.ToString());
		}

		[Test]
		public void CompatibleWord32()
		{
			Assert.IsTrue(un.AreCompatible(PrimitiveType.Word32, PrimitiveType.Word32));
		}

		[Test]
		public void CompatibleReal32Word32()
		{
			Assert.IsTrue(un.AreCompatible(PrimitiveType.Real32, PrimitiveType.Word32));
		}

		[Test]
		public void CompatibleReal32Int32()
		{
			Assert.IsFalse(un.AreCompatible(PrimitiveType.Real32, PrimitiveType.Int32));
		}

		[Test]
		public void CompatibleInt32Uint32()
		{
			Assert.IsFalse(un.AreCompatible(PrimitiveType.UInt16, PrimitiveType.Int16));
		}

		[Test]
		public void CompatibleTv1Tv1()
		{
			Assert.IsTrue(un.AreCompatible(new TypeVariable(1), new TypeVariable(1)));
		}

		[Test]
		public void CompatibleTv1Tv2()
		{
			Assert.IsFalse(un.AreCompatible(new TypeVariable(1), new TypeVariable(2)));
		}

		[Test]
		public void CompatiblePointers()
		{
			TypeVariable tv1 = new TypeVariable(1);
			Pointer p1 = new Pointer(tv1, 4);
			Pointer p2 = new Pointer(tv1, 4);
			Assert.IsTrue(un.AreCompatible(p1, p2));
		}

		[Test]
		public void IncompatiblePointers()
		{
			TypeVariable tv1 = new TypeVariable(1);
			TypeVariable tv2 = new TypeVariable(2);
			Assert.IsFalse(un.AreCompatible(new Pointer(tv1, 4), new Pointer(tv2, 4)));
		}

		[Test]
		public void CompatibleArrays()
		{
			ArrayType a1 = new ArrayType(new StructureType(null, 4), 0);
			ArrayType a2 = new ArrayType(new StructureType(null, 4), 0);
			Assert.IsTrue(un.AreCompatible(a1, a2));
		}

		[Test]
		public void CompatibleFunctions()
		{
			FunctionType f1 = new FunctionType(null, null, new DataType[] { PrimitiveType.Int16 }, null );
			FunctionType f2 = new FunctionType(null, null, new DataType[] { PrimitiveType.Int32 }, null );
			Assert.IsTrue(un.AreCompatible(f1, f2));
		}

		[Test]
		public void CompatibleMemberPointers()
		{
			MemberPointer mp1 = new MemberPointer(new TypeVariable(1), PrimitiveType.Word16, 0);
			MemberPointer mp2 = new MemberPointer(new TypeVariable(1), PrimitiveType.Word16, 0);
			Assert.IsTrue(un.AreCompatible(mp1, mp2));
		}

        [Test]
        public void CompatiblePtrToArray()
        {
            var eq = new EquivalenceClass(new TypeVariable(3));
            var a = new ArrayType(eq, 0);
            var p1 = new Pointer(eq, 4);
            var p2 = new Pointer(a, 4);
            Assert.IsTrue(un.AreCompatible(p1, p2));
        }

        [Test]
        public void CompatiblePtrToCode()
        {
            var code = new CodeType();
            var p1 = new Pointer(code, 4);
            var p2 = new Pointer(code, 4);
            Assert.IsTrue(un.AreCompatible(p1, p2));
        }

        [Test]
        public void CompatibleTypeReference()
        {
            var t1 = new TypeReference("CHAR", PrimitiveType.Char);
            var t2 = PrimitiveType.Char;
            Assert.IsTrue(un.AreCompatible(t1, t2));
        }


        [Test]
        public void UnifyTypeReferences()
        {
            var t1 = new TypeReference("CHAR", PrimitiveType.Char);
            var t2 = PrimitiveType.Char;
            Assert.AreEqual("CHAR", un.Unify(t1, t2).ToString());
        }
	}
}
