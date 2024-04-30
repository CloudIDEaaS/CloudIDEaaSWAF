using System;
using System.Net;
using System.Windows;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Diagnostics.Runtime;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using Utils.ProcessHelpers;
#if SILVERLIGHT
using Utils.DebugListenerServiceReference;
using System.Windows.Controls;
#endif

namespace Utils
{
    public static class DebugUtils
    {
        [DllImport("Dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint ProcessId, IntPtr hFile, int DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallbackParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentProcessId();

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MINIDUMP_EXCEPTION_INFORMATION
        {

            public uint ThreadId;
            public IntPtr ExceptionPointers;
            public int ClientPointers;
        }

        private static readonly int MiniDumpWithFullMemory = 2;

#if SILVERLIGHT
        public static SetBreakOnThrownExceptionsOperation SetBreakOnThrownExceptions(bool on)
        {
            var service = new DebugListenerClient();
            var operation = new SetBreakOnThrownExceptionsOperation();

            service.SetBreakOnThrownExceptionsAsync(on);

            service.SetBreakOnThrownExceptionsCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    operation.SetException(e.Error);
                }
                else
                {
                    operation.SetComplete();
                }
            };

            return operation;
        }

        public static GetBreakOnThrownExceptionsOperation GetBreakOnThrownExceptions()
        {
            var service = new DebugListenerClient();
            var operation = new GetBreakOnThrownExceptionsOperation();

            service.GetBreakOnThrownExceptionsAsync();

            service.GetBreakOnThrownExceptionsCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    operation.SetException(e.Error);
                }
                else
                {
                    operation.SetComplete(e.Result);
                }
            };

            return operation;
        }
        public static string GetTypeAndName(this DependencyObject dependencyObject, string delimiter = null)
        {
            if (dependencyObject is FrameworkElement)
            {
                var element = (FrameworkElement)dependencyObject;

                if (string.IsNullOrEmpty(element.Name))
                {
                    return element.GetType().Name;
                }
                else if (delimiter != null)
                {
                    return element.GetType().Name + delimiter + element.Name;
                }
                else
                {
                    return element.GetType().Name + " (" + element.Name + ")";
                }
            }
            else
            {
                return dependencyObject.GetType().Name;
            }
        }

        private static IEnumerable<DependencyObject> GetChildren(this DependencyObject obj)
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);

            for (var x = 0; x < count; x++)
            {
                yield return VisualTreeHelper.GetChild(obj, x);
            }
        }

        private static IEnumerable<DependencyObject> GetDescendants(this DependencyObject obj)
        {
            var descendants = new List<DependencyObject>();

            obj.AddDescendants(descendants);

            return descendants;
        }

        private static void AddDescendants(this DependencyObject obj, List<DependencyObject> list)
        {
            foreach (var child in obj.GetChildren())
            {
                list.Add(child);

                child.AddDescendants(list);
            }
        }

        public static string GetTypeNameAndDescendantCount(this DependencyObject dependencyObject)
        {
            if (dependencyObject is FrameworkElement)
            {
                var element = (FrameworkElement)dependencyObject;

                if (string.IsNullOrEmpty(element.Name))
                {
                    return element.GetType().Name + " [" + element.GetDescendants().Count() + "]";
                }
                else
                {
                    return element.GetType().Name + " (" + element.Name + ")" + " [" + element.GetDescendants().Count() + "]";
                }
            }
            else
            {
                return dependencyObject.GetType().Name;
            }
        }

        public static string GetTypeNameAndChildCount(this DependencyObject dependencyObject)
        {
            if (dependencyObject is FrameworkElement)
            {
                var element = (FrameworkElement)dependencyObject;

                if (string.IsNullOrEmpty(element.Name))
                {
                    return element.GetType().Name + " [" + element.GetChildren().Count() + "]";
                }
                else
                {
                    return element.GetType().Name + " (" + element.Name + ")" + " [" + element.GetChildren().Count() + "]";
                }
            }
            else
            {
                return dependencyObject.GetType().Name;
            }
        }
#endif

        [Flags]
        public enum UnDecorateFlags
        {
            UNDNAME_COMPLETE = (0x0000),  // Enable full undecoration
            UNDNAME_NO_LEADING_UNDERSCORES = (0x0001),  // Remove leading underscores from MS extended keywords
            UNDNAME_NO_MS_KEYWORDS = (0x0002),  // Disable expansion of MS extended keywords
            UNDNAME_NO_FUNCTION_RETURNS = (0x0004),  // Disable expansion of return type for primary declaration
            UNDNAME_NO_ALLOCATION_MODEL = (0x0008),  // Disable expansion of the declaration model
            UNDNAME_NO_ALLOCATION_LANGUAGE = (0x0010),  // Disable expansion of the declaration language specifier
            UNDNAME_NO_MS_THISTYPE = (0x0020),  // NYI Disable expansion of MS keywords on the 'this' type for primary declaration
            UNDNAME_NO_CV_THISTYPE = (0x0040),  // NYI Disable expansion of CV modifiers on the 'this' type for primary declaration
            UNDNAME_NO_THISTYPE = (0x0060),  // Disable all modifiers on the 'this' type
            UNDNAME_NO_ACCESS_SPECIFIERS = (0x0080),  // Disable expansion of access specifiers for members
            UNDNAME_NO_THROW_SIGNATURES = (0x0100),  // Disable expansion of 'throw-signatures' for functions and pointers to functions
            UNDNAME_NO_MEMBER_TYPE = (0x0200),  // Disable expansion of 'static' or 'virtual'ness of members
            UNDNAME_NO_RETURN_UDT_MODEL = (0x0400),  // Disable expansion of MS model for UDT returns
            UNDNAME_32_BIT_DECODE = (0x0800),  // Undecorate 32-bit decorated names
            UNDNAME_NAME_ONLY = (0x1000),  // Crack only the name for primary declaration;
                                           // return just [scope::]name.  Does expand template params
            UNDNAME_NO_ARGUMENTS = (0x2000),  // Don't undecorate arguments to function
            UNDNAME_NO_SPECIAL_SYMS = (0x4000),  // Don't undecorate special names (v-table, vcall, vector xxx, metatype, etc)
        }

        [DllImport("dbghelp.dll", SetLastError = true, PreserveSig = true)]
        public static extern int UnDecorateSymbolName(
            [In][MarshalAs(UnmanagedType.LPStr)] string DecoratedName,
            [Out] StringBuilder UnDecoratedName,
            [In][MarshalAs(UnmanagedType.U4)] int UndecoratedLength,
            [In][MarshalAs(UnmanagedType.U4)] UnDecorateFlags Flags);

        public static string UndecorateSymbolName(string decoratedName)
        {
            var builder = new StringBuilder(255);
            UnDecorateSymbolName(decoratedName, builder, builder.Capacity, UnDecorateFlags.UNDNAME_COMPLETE);

            return builder.ToString();
        }


        [DllImport("kernel32.dll")]
        public static extern FilterDelegate SetUnhandledExceptionFilter(FilterDelegate lpTopLevelExceptionFilter);
        public delegate bool FilterDelegate(Exception ex);

        [Conditional("DEBUG")]
        [Obsolete("Please remove me before checkin.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoOp()
        {
        }

        [DebuggerHidden()]
        public static T BreakReturn<T>(T value)
        {
#if DEBUG
            Debugger.Break();
            return value;
#else
            throw new InvalidOperationException();
#endif
        }

        [DebuggerHidden()]
        public static dynamic BreakReturnNull()
        {
#if DEBUG
            Debugger.Break();
            return null;
#else
            throw new InvalidOperationException();
#endif
        }

        [DebuggerHidden()]
        public static void Break()
        {
#if DEBUG
            Debugger.Break();
#else
            throw new NotImplementedException();
#endif
        }

        [DebuggerHidden()]
        public static void BreakIfAttached()
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
#endif
        }

        [DebuggerHidden()]
        public static void Break(string error)
        {
#if DEBUG
            Debugger.Break();
#else
            throw new InvalidOperationException(error);
#endif
        }

        public static void CreateDump(this Process process, string dumpFile, int threadId)
        {
            using (var file = new FileStream(dumpFile, FileMode.Create))
            {
                bool result;
                var processHandle = NativeMethods.OpenProcess(ProcessAccessRights.PROCESS_DUP_HANDLE | ProcessAccessRights.PROCESS_QUERY_INFORMATION | ProcessAccessRights.PROCESS_VM_READ, true, process.Id);

                result = MiniDumpWriteDump(process.Handle, (uint) process.Id, file.SafeFileHandle.DangerousGetHandle(), MiniDumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                processHandle.Close();
            }
        }

        public static IEnumerable<StackFrame> GetStack(this object obj, int count)
        {
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Take(count + 1);

            return frames;
        }

        public static DebugState GetDebugState(this Process process, int threadId)
        {
            var dataTarget = DataTarget.AttachToProcess(process.Id, true);
            var version = dataTarget.ClrVersions[0];
            var debugState = new DebugState();

            debugState.ProcessName = process.ProcessName;
            debugState.ExceptionMessage = "Unknown";
            debugState.ExceptionText = "Unknown";

            try
            {
                using (var runtime = version.CreateRuntime())
                {
                    var thread = runtime.Threads.Single(t => t.ManagedThreadId == threadId);
                    var currentException = thread.CurrentException;
                    var heap = runtime.Heap;
                    var stackBase = thread.StackBase;
                    var stackTrace = currentException.StackTrace;

                    debugState.ProcessName = process.ProcessName;
                    debugState.ExceptionMessage = currentException.Message;
                    debugState.ExceptionText = currentException.ToString();

                    foreach (var frame in stackTrace)
                    {
                        try
                        {
                            var index = stackTrace.IndexOf(frame);
                            var stackPointer = frame.StackPointer;
                            var clrMethod = frame.Method;
                            var clrType = clrMethod.Type;
                            var location = clrType.Module.AssemblyName;
                            Assembly assembly = null;
                            Type type = null;
                            MethodBase method = null;
                            ClrStackFrame nextFrame;
                            ulong nextPointer;

                            try
                            {
                                assembly = Assembly.ReflectionOnlyLoadFrom(location);
                                type = assembly.ManifestModule.ResolveType(clrType.MetadataToken);
                                method = assembly.ManifestModule.ResolveMethod(clrMethod.MetadataToken);
                            }
                            catch
                            {
                            }

                            if (index >= stackTrace.Length - 1)
                            {
                                nextPointer = stackBase;
                            }
                            else
                            {
                                nextFrame = stackTrace[index + 1];
                                nextPointer = nextFrame.StackPointer;
                            }

                            try
                            {
                                debugState.WriteLine("{0}!{1}.{2}", assembly.FullName, type.FullName, method.Name);
                            }
                            catch
                            {
                                debugState.WriteLine("Unknown");
                            }

                            debugState.WriteLine("    Stack objects:");

                            for (var ptr = stackPointer; ptr < nextPointer; ptr += (uint)IntPtr.Size)
                            {
                                if (!dataTarget.DataReader.ReadPointer(ptr, out ulong ptrToObject))
                                {
                                    break;
                                }

                                try
                                {
                                    clrType = heap.GetObjectType(ptrToObject);
                                }
                                catch
                                {
                                }

                                if (clrType == null)
                                {
                                    continue;
                                }

                                if (!clrType.IsFree)
                                {
                                    debugState.WriteLine("    {0,16:X} {1,16:X} {2}", ptr, ptrToObject, clrType.Name);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {
            }

            return debugState;
        }

        public static bool InPotentiallyPartialIterator(this object obj)
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(2);
            var reflectedType = frame.GetMethod().ReflectedType;

            if (reflectedType.Namespace == "System.Linq")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IEnumerable<StackFrame> GetStack(this object obj)
        {
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames();

            return frames;
        }

        public static IEnumerable<StackFrame> GetStackWhile(this object obj, Func<StackFrame, bool> whileFilter, int skip = 0)
        {
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Skip(skip).Where(f => whileFilter(f));

            return frames;
        }

        public static IEnumerable<StackFrame> GetStackUntil(this object obj, Func<StackFrame, bool> untilFilter, int skip = 0)
        {
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Skip(skip).Where(f => !untilFilter(f));

            return frames;
        }

        public static string GetDebuggerDisplay(this object obj)
        {
            //////var type = obj.GetType();

            //////if (type.HasCustomAttribute<DebuggerDisplayAttribute>())
            //////{
            //////    var attr = type.GetCustomAttribute<DebuggerDisplayAttribute>();
            //////    var attrValue = attr.Value;
            //////    var builder = new StringBuilder();
            //////    var regex = new Regex("{[^}]*}");
            //////    var project = ICsProjectFactory.create(project_namespace.pn_project_namespace);

            //////    if (regex.IsMatch(attrValue))
            //////    {
            //////        var matches = regex.Matches(attrValue);
            //////        var x = 0;
            //////        var index = 0;
            //////        var matchStrings = new List<string>();
            //////        //var evaluator = new BracedAttributeArgumentEvaluator(obj.GetType(), @"C:\Projects\RazorViewsDesigner\TestEval\DebuggerDisplayEvaluator.dll");
            //////        var evaluator = new BracedAttributeArgumentEvaluator(obj.GetType());

            //////        foreach (Match match in matches)
            //////        {
            //////            var leftString = attrValue.Substring(index, match.Index - index);
            //////            var matchString = match.Value;

            //////            builder.Append(leftString);
            //////            builder.AppendFormat("{{{0}}}", x);

            //////            matchStrings.Add(matchString);

            //////            index = match.Index + match.Length;
            //////            x++;
            //////        }

            //////        evaluator.ProcessFormat(builder.ToString());

            //////        for (var y = 0; y < matchStrings.Count; y++)
            //////        {
            //////            var matchString = matchStrings[y];
            //////            var snippet = ICsSnippetFactory.create(matchString.ToCharArray(), null);
            //////            CsNode rootNode;

            //////            try
            //////            {
            //////                project.parseSnippet(snippet, CsExpectedSnippet.cses_statement, null, true);
            //////            }
            //////            catch
            //////            {
            //////                Debugger.Break();
            //////            }

            //////            rootNode = snippet.getNodes()[0];

            //////            evaluator.ProcessArg(rootNode, matchString);
            //////        }

            //////        evaluator.PostProcess(x);

            //////        try
            //////        {
            //////            return evaluator.Evaluate(obj);
            //////        }
            //////        catch (Exception ex)
            //////        {
            //////            Debugger.Break();
            //////        }
            //////    }
            //////}

            //////return obj.ToString();
            ///

            throw new NotImplementedException();
        }

        public static string GetFriendlyName(this StackFrame stackFrame)
        {
            var method = stackFrame.GetMethod();

            return method.DeclaringType.Name + "." + method.Name;
        }

        public static string ToString(this IEnumerable<StackFrame> stack)
        {
            return stack.Reverse().Select(f => f.GetFriendlyName()).ToDelimitedList(" -> ");
        }

        public static string GetDebugTime(bool includeBrackets = true)
        {
            if (includeBrackets)
            {
                return string.Format("[{0:hh:mm:ss.FFFFF}]", DateTime.Now);
            }
            else
            {
                return string.Format("{0:hh:mm:ss.FFFFF]", DateTime.Now);
            }
        }

        public static string ToDebugTimeText(this DateTime time, bool includeBrackets = true)
        {
            if (includeBrackets)
            {
                return string.Format("[{0:hh:mm:ss.FFFFF}]", time);
            }
            else
            {
                return string.Format("{0:hh:mm:ss.FFFFF}", time);
            }
        }

        public static string ToDebugDateTimeText(this DateTime dateTime, bool includeBrackets = true)
        {
            if (includeBrackets)
            {
                return string.Format("[{0:yyyy-MM-dd hh:mm:ss.FFFFF}]", dateTime);
            }
            else
            {
                return string.Format("{0:yyyy-MM-dd hh:mm:ss.FFFFF}", dateTime);
            }
        }

        public static string ToDebugTimeTextShort(this DateTime time, bool includeBrackets = true)
        {
            if (includeBrackets)
            {
                return string.Format("[{0:yyyy-MM-dd hh:mm:ss}]", time);
            }
            else
            {
                return string.Format("{0:yyyy-MM-dd hh:mm:ss}", time);
            }
        }

        public static string GetStackFileName(this object obj, int frameIndex)
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(frameIndex);

            return frame.GetFileName();
        }

        public static int GetStackLineNumber(this object obj, int frameIndex)
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(frameIndex);

            return frame.GetFileLineNumber();
        }

        public static string GetStackText(this object obj, int count, bool includeTime)
        {
            string callStack = string.Empty;
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Take(count + 1);

            try
            {
                callStack = frames.Reverse().Take(count - 1).Select(f => string.Format("{0}:{1}", f.GetMethod().Name, f.GetFileLineNumber())).ToDelimitedList(" -> ");
            }
            catch
            {
            }

            return (includeTime ? GetDebugTime() + " " : string.Empty) + callStack;
        }

        public static string GetStackText(this object obj, int count)
        {
            string callStack = string.Empty;
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Take(count + 1);

            try
            { 
                callStack = frames.Reverse().Take(count - 1).Select(f => string.Format("{0}:{1}", f.GetMethod().Name, f.GetFileLineNumber())).ToDelimitedList(" -> ");
            }
            catch
            {
            }

            return callStack;
        }

        public static string GetStackText(this object obj, int count, int skip)
        {
            string callStack = string.Empty;
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Take(count + 1);
            
            try
            { 
                callStack = frames.Reverse().Take(count - skip - 1).Select(f => string.Format("{0}:{1}", f.GetMethod().Name, f.GetFileLineNumber())).ToDelimitedList(" -> ");
            }
            catch
            {
            }

            return callStack;
        }

        [DebuggerStepThrough()]
        public static void DoIf(bool b, Action action)
        {
            if (b)
            {
                action();
            }
        }


        public static void AssertThrow(bool b, string genericMessage)
        {
            if (!b)
            {
                throw new Exception(genericMessage);
            }
        }

        public static void AssertThrow(bool b, Func<string> getGenericMessage)
        {
            if (!b)
            {
                throw new Exception(getGenericMessage());
            }
        }


        [DebuggerStepThrough()]
        public static void DoIfNot(bool b, Action action)
        {
            if (!b)
            {
                action();
            }
        }

        public static void ThrowIf(bool b, Func<Exception> func)
        {
            if (b)
            {
                throw func();
            }
        }

        public static void ThrowIfNot(bool b, Func<Exception> func)
        {
            if (!b)
            {
                throw func();
            }
        }

        public static void WriteLineIf(bool b, object value)
        {
            if (b)
            {
                Debug.WriteLine(value);
            }
        }

        public static void WriteLineIf(bool b, string message)
        {
            if (b)
            {
                Debug.WriteLine(message);
            }
        }

        public static void WriteLineIf(bool b, string format, params object[] args)
        {
            if (b)
            {
                Debug.WriteLine(format, args);
            }
        }

        public static void Fail(string message)
        {
            Debug.Assert(false, message);
        }

        public static void Fail(string p, string p_2)
        {
            throw new NotImplementedException();
        }
    }
}
