using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Runtime.InteropServices;

namespace Utils.IntermediateLanguage
{
    public class MethodBodyReader
    {
        public List<Utils.IntermediateLanguage.ILInstruction> Instructions { get; set; }
        protected byte[] il = null;
        private MethodInfo mi = null;

        #region il read methods
        private int ReadInt16(byte[] _il, ref int position)
        {
            return ((il[position++] | (il[position++] << 8)));
        }
        private ushort ReadUInt16(byte[] _il, ref int position)
        {
            return (ushort)((il[position++] | (il[position++] << 8)));
        }
        private int ReadInt32(byte[] _il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        }
        private ulong ReadInt64(byte[] _il, ref int position)
        {
            return (ulong)(((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
        }
        private double ReadDouble(byte[] _il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
        }
        private sbyte ReadSByte(byte[] _il, ref int position)
        {
            return (sbyte)il[position++];
        }
        private byte ReadByte(byte[] _il, ref int position)
        {
            return (byte)il[position++];
        }
        private Single ReadSingle(byte[] _il, ref int position)
        {
            return (Single)(((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        }

        private int ReadInt16(ref IntPtr intPtr, ref int position)
        {
            var result = Marshal.ReadInt16(intPtr);

            intPtr += sizeof(short);
            position += sizeof(short);

            return result;
        }
        private ushort ReadUInt16(ref IntPtr intPtr, ref int position)
        {
            var result = (ushort) Marshal.ReadInt16(intPtr);

            intPtr += sizeof(short);
            position += sizeof(short);

            return result;
        }
        private int ReadInt32(ref IntPtr intPtr, ref int position)
        {
            var result = Marshal.ReadInt32(intPtr);

            intPtr += sizeof(int);
            position += sizeof(int);

            return result;
        }
        private ulong ReadInt64(ref IntPtr intPtr, ref int position)
        {
            var result = (ulong) Marshal.ReadInt64(intPtr);

            intPtr += sizeof(ulong);
            position += sizeof(ulong);

            return result;
        }
        private double ReadDouble(ref IntPtr intPtr, ref int position)
        {
            var result = (double)Marshal.ReadInt64(intPtr);

            intPtr += sizeof(double);
            position += sizeof(double);

            return result;
        }
        private sbyte ReadSByte(ref IntPtr intPtr, ref int position)
        {
            var result = (sbyte)Marshal.ReadByte(intPtr);

            intPtr += sizeof(sbyte);
            position += sizeof(sbyte);

            return result;
        }
        private byte ReadByte(ref IntPtr intPtr, ref int position)
        {
            var result = (byte)Marshal.ReadByte(intPtr);

            intPtr += sizeof(byte);
            position += sizeof(byte);

            return result;
        }
        private Single ReadSingle(ref IntPtr intPtr, ref int position)
        {
            var result = (float)Marshal.ReadInt32(intPtr);

            intPtr += sizeof(int);
            position += sizeof(float);

            return result;
        }

        #endregion

        /// <summary>
        /// Constructs the array of ILInstructions according to the IL byte code.
        /// </summary>
        /// <param name="module"></param>
        private void ConstructInstructions(Module module, Func<int, MemberInfo>? resolve = null)
        {
            byte[] il = this.il;
            int position = 0;

            Instructions = new List<ILInstruction>();

            while (position < il.Length)
            {
                var instruction = new ILInstruction();

                // get the operation code of the current instruction
                OpCode code = OpCodes.Nop;
                ushort value = il[position++];

                if (value != 0xfe)
                {
                    code = Globals.singleByteOpCodes[(int)value];
                }
                else
                {
                    value = il[position++];
                    code = Globals.multiByteOpCodes[(int)value];
                    value = (ushort)(value | 0xfe00);
                }

                instruction.Code = code;
                instruction.Offset = position - 1;
                int metadataToken = 0;

                // get the operand of the current operation
                switch (code.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        metadataToken = ReadInt32(il, ref position);
                        metadataToken += position;
                        instruction.Operand = metadataToken;
                        break;
                    case OperandType.InlineField:
                        metadataToken = ReadInt32(il, ref position);

                        if (resolve != null)
                        {
                            instruction.Operand = resolve(metadataToken);
                        }
                        else
                        {
                            instruction.Operand = module.ResolveType(metadataToken, this.mi.DeclaringType.GetGenericArguments(), this.mi.GetGenericArguments());
                        }

                        break;
                    case OperandType.InlineMethod:

                        metadataToken = ReadInt32(il, ref position);

                        if (resolve != null)
                        {
                            instruction.Operand = resolve(metadataToken);
                        }
                        else
                        {
                            try
                            {
                                instruction.Operand = module.ResolveMethod(metadataToken);
                            }
                            catch
                            {
                                try
                                {
                                    instruction.Operand = module.ResolveMember(metadataToken);
                                }
                                catch (Exception ex)
                                {
                                    instruction.Operand = "<invalid>";
                                }
                            }
                        }
                        break;
                    case OperandType.InlineSig:
                        metadataToken = ReadInt32(il, ref position);

                        if (resolve != null)
                        {
                            instruction.Operand = module.ResolveField(metadataToken);
                        }
                        else
                        {
                            instruction.Operand = module.ResolveSignature(metadataToken);
                        }

                        break;
                    case OperandType.InlineTok:
                        
                        metadataToken = ReadInt32(il, ref position);

                        if (resolve != null)
                        {
                            instruction.Operand = resolve(metadataToken);
                        }
                        else
                        {
                            instruction.Operand = module.ResolveType(metadataToken, this.mi.DeclaringType.GetGenericArguments(), this.mi.GetGenericArguments());
                        }
                        // SSS : see what to do here
                        break;
                    case OperandType.InlineType:
                        metadataToken = ReadInt32(il, ref position);

                        if (resolve != null)
                        {
                            instruction.Operand = resolve(metadataToken);
                        }
                        else
                        {
                            instruction.Operand = module.ResolveType(metadataToken, this.mi.DeclaringType.GetGenericArguments(), this.mi.GetGenericArguments());
                        }

                        break;
                    case OperandType.InlineI:
                        {
                            instruction.Operand = ReadInt32(il, ref position);
                            break;
                        }
                    case OperandType.InlineI8:
                        {
                            instruction.Operand = ReadInt64(il, ref position);
                            break;
                        }
                    case OperandType.InlineNone:
                        {
                            instruction.Operand = null;
                            break;
                        }
                    case OperandType.InlineR:
                        {
                            instruction.Operand = ReadDouble(il, ref position);
                            break;
                        }
                    case OperandType.InlineString:
                        {
                            metadataToken = ReadInt32(il, ref position);

                            if (resolve != null)
                            {
                                instruction.Operand = resolve(metadataToken);
                            }
                            else
                            {
                                instruction.Operand = module.ResolveString(metadataToken);
                            }

                            break;
                        }
                    case OperandType.InlineSwitch:
                        {
                            int count = ReadInt32(il, ref position);
                            int[] casesAddresses = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                casesAddresses[i] = ReadInt32(il, ref position);
                            }
                            int[] cases = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                cases[i] = position + casesAddresses[i];
                            }
                            break;
                        }
                    case OperandType.InlineVar:
                        {
                            instruction.Operand = ReadUInt16(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineBrTarget:
                        {
                            instruction.Operand = ReadSByte(il, ref position) + position;
                            break;
                        }
                    case OperandType.ShortInlineI:
                        {
                            instruction.Operand = ReadSByte(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineR:
                        {
                            instruction.Operand = ReadSingle(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineVar:
                        {
                            instruction.Operand = ReadByte(il, ref position);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown operand type.");
                        }
                }

                Instructions.Add(instruction);
            }
        }

        private void ConstructInstructions(Module module, IntPtr functionPointer, Func<int, MemberInfo>? resolve = null)
        {
            byte[] il = this.il;
            int position = 0;

            Instructions = new List<ILInstruction>();

            while (true)
            {
                var instruction = new ILInstruction();

                // get the operation code of the current instruction
                OpCode code = OpCodes.Nop;
                ushort value = ReadUInt16(ref functionPointer, ref position);

                if (value != 0xfe)
                {
                    code = Globals.singleByteOpCodes[(int)value];
                }
                else
                {
                    value = il[position++];
                    code = Globals.multiByteOpCodes[(int)value];
                    value = (ushort)(value | 0xfe00);
                }

                instruction.Code = code;
                instruction.Offset = position - 1;
                int metadataToken = 0;
                // get the operand of the current operation
                switch (code.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        metadataToken = ReadInt32(ref functionPointer, ref position);
                        metadataToken += position;
                        instruction.Operand = metadataToken;
                        break;
                    case OperandType.InlineField:
                        metadataToken = ReadInt32(ref functionPointer, ref position);
                        instruction.Operand = module.ResolveField(metadataToken);
                        break;
                    case OperandType.InlineMethod:

                        metadataToken = ReadInt32(ref functionPointer, ref position);

                        if (resolve != null)
                        {
                            instruction.Operand = resolve(metadataToken);
                        }
                        else
                        {
                            try
                            {
                                instruction.Operand = module.ResolveMethod(metadataToken);
                            }
                            catch
                            {
                                try
                                {
                                    instruction.Operand = module.ResolveMember(metadataToken);
                                }
                                catch (Exception ex)
                                {
                                    var nextDword = ReadInt32(ref functionPointer, ref position);
                                    long result = (nextDword << 32) | metadataToken;

                                    instruction.Operand = "<invalid>";
                                }
                            }
                        }
                        break;
                    case OperandType.InlineSig:
                        metadataToken = ReadInt32(ref functionPointer, ref position);
                        instruction.Operand = module.ResolveSignature(metadataToken);
                        break;
                    case OperandType.InlineTok:
                        metadataToken = ReadInt32(ref functionPointer, ref position);
                        try
                        {
                            instruction.Operand = module.ResolveType(metadataToken);
                        }
                        catch
                        {

                        }
                        // SSS : see what to do here
                        break;
                    case OperandType.InlineType:
                        metadataToken = ReadInt32(ref functionPointer, ref position);
                        // now we call the ResolveType always using the generic attributes type in order
                        // to support decompilation of generic methods and classes

                        // thanks to the guys from code project who commented on this missing feature

                        instruction.Operand = module.ResolveType(metadataToken, this.mi.DeclaringType.GetGenericArguments(), this.mi.GetGenericArguments());
                        break;
                    case OperandType.InlineI:
                        {
                            instruction.Operand = ReadInt32(ref functionPointer, ref position);
                            break;
                        }
                    case OperandType.InlineI8:
                        {
                            instruction.Operand = ReadInt64(ref functionPointer, ref position);
                            break;
                        }
                    case OperandType.InlineNone:
                        {
                            instruction.Operand = null;
                            break;
                        }
                    case OperandType.InlineR:
                        {
                            instruction.Operand = ReadDouble(ref functionPointer, ref position);
                            break;
                        }
                    case OperandType.InlineString:
                        {
                            metadataToken = ReadInt32(ref functionPointer, ref position);
                            instruction.Operand = module.ResolveString(metadataToken);
                            break;
                        }
                    case OperandType.InlineSwitch:
                        {
                            int count = ReadInt32(ref functionPointer, ref position);
                            int[] casesAddresses = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                casesAddresses[i] = ReadInt32(ref functionPointer, ref position);
                            }
                            int[] cases = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                cases[i] = position + casesAddresses[i];
                            }
                            break;
                        }
                    case OperandType.InlineVar:
                        {
                            instruction.Operand = ReadUInt16(ref functionPointer, ref position);
                            break;
                        }
                    case OperandType.ShortInlineBrTarget:
                        {
                            instruction.Operand = ReadSByte(ref functionPointer, ref position) + position;
                            break;
                        }
                    case OperandType.ShortInlineI:
                        {
                            instruction.Operand = ReadSByte(ref functionPointer, ref position);
                            break;
                        }
                    case OperandType.ShortInlineR:
                        {
                            instruction.Operand = ReadSingle(ref functionPointer, ref position);
                            break;
                        }
                    case OperandType.ShortInlineVar:
                        {
                            instruction.Operand = ReadByte(ref functionPointer, ref position);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown operand type.");
                        }
                }
                Instructions.Add(instruction);
            }
        }

        public object GetReferencedOperand(Module module, int metadataToken)
        {
            AssemblyName[] assemblyNames = module.Assembly.GetReferencedAssemblies();
            for (int i=0; i<assemblyNames.Length; i++)
            {
                Module[] modules = Assembly.Load(assemblyNames[i]).GetModules();
                for (int j=0; j<modules.Length; j++)
                {
                    try
                    {
                        Type t = modules[j].ResolveType(metadataToken);
                        return t;
                    }
                    catch
                    {

                    }

                }
            }
            return null;
        //System.Reflection.Assembly.Load(module.Assembly.GetReferencedAssemblies()[3]).GetModules()[0].ResolveType(metadataToken)

        }
        /// <summary>
        /// Gets the IL code of the method
        /// </summary>
        /// <returns></returns>
        public string GetBodyCode()
        {
            string result = "";
            if (Instructions != null)
            {
                for (int i = 0; i < Instructions.Count; i++)
                {
                    result += Instructions[i].GetCode() + "\n";
                }
            }
            return result;

        }

        /// <summary>
        /// MethodBodyReader constructor
        /// </summary>
        /// <param name="methodInfo">
        /// The System.Reflection defined MethodInfo
        /// </param>
        public MethodBodyReader(MethodInfo methodInfo)
        {
            this.mi = methodInfo;

            if (methodInfo.Module.Name == "<In Memory Module>")
            {
                var dynMethod = methodInfo as DynamicMethod;
                var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField;
                var ilGen = (ILGenerator)typeof(DynamicMethod).GetField("_ilGenerator", bindingFlags).GetValue(dynMethod)!;
                var ilGenType = ilGen.GetType();
                var dynamicResolverType = typeof(OpCodes).Assembly.GetType("System.Reflection.Emit.DynamicResolver");
                var resolveTokenMethod = dynamicResolverType.GetMethod("ResolveToken", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                var dynamicResolverConstructor = dynamicResolverType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance).First(c => c.GetParameters().Any(p => p.ParameterType.Name == "DynamicILGenerator"));
                var dynamicResolver = dynamicResolverConstructor.Invoke([ ilGen ]);
                Func<int, MemberInfo>? resolve = null;

                resolve = new Func<int, MemberInfo>((t) =>
                {
                    object?[] args = [t, null, null, null];
                    IntPtr typeHandle;
                    IntPtr methodHandle;
                    IntPtr fieldHandle;

                    resolveTokenMethod.Invoke(dynamicResolver, args);

                    if (((IntPtr)args[1]!) != IntPtr.Zero)
                    {
                        RuntimeTypeHandle runtimeTypeHandle;
                        Type type;

                        typeHandle = (IntPtr)args[1]!;
                        runtimeTypeHandle = RuntimeTypeHandle.FromIntPtr(typeHandle);
                        type = Type.GetTypeFromHandle(runtimeTypeHandle)!;

                        return type;
                    }
                    else if (((IntPtr)args[2]!) != IntPtr.Zero)
                    {
                        RuntimeMethodHandle runtimeMethodHandle;
                        MethodBase methodBase;

                        methodHandle = (IntPtr)args[2]!;
                        runtimeMethodHandle = RuntimeMethodHandle.FromIntPtr(methodHandle);
                        methodBase = MethodBase.GetMethodFromHandle(runtimeMethodHandle)!;

                        return methodBase;
                    }
                    else if (((IntPtr)args[3]!) != IntPtr.Zero)
                    {
                        RuntimeFieldHandle runtimeFieldHandle;
                        FieldInfo field;

                        fieldHandle = (IntPtr)args[3]!;
                        runtimeFieldHandle = RuntimeFieldHandle.FromIntPtr(fieldHandle);
                        field = FieldInfo.GetFieldFromHandle(runtimeFieldHandle)!;

                        return field;
                    }
                    else
                    {
                        return null;
                    }
                });

                if (ilGen != null)
                {
                    FieldInfo fieldIlStream;

                    // Conditional for .NET 4.x because DynamicILGenerator class derived from ILGenerator.
                    // Source: https://stackoverflow.com/a/4147132/1248295
                    if (Environment.Version.Major >= 4)
                    {
                        fieldIlStream = ilGenType.BaseType.GetField("m_ILStream", bindingFlags)!;
                    }
                    else // This worked on .NET 3.5
                    {
                        fieldIlStream = ilGenType.GetField("m_ILStream", bindingFlags)!;
                    }

                    il = (byte[]) fieldIlStream.GetValue(ilGen)!;

                    ConstructInstructions(methodInfo.Module, resolve);
                }
                else
                {
                    DebugUtils.Break();
                }
            }
            else
            {
                if (methodInfo.GetMethodBody() != null)
                {
                    il = methodInfo.GetMethodBody().GetILAsByteArray()!;
                    ConstructInstructions(methodInfo.Module);
                }
            }
        }
    }
}
