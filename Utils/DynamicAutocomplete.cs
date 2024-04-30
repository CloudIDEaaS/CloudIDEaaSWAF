using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public static class DynamicAutocomplete
    {
        private static readonly Guid CLSID_AutoComplete = new Guid("{00BB2763-6A77-11D0-A535-00C04FD7D062}");
        private static readonly Dictionary<TextBox, KeyValuePair<IAutoComplete2, EnumString>> autoCompletedTextboxes;

        static DynamicAutocomplete()
        {
            autoCompletedTextboxes = new Dictionary<TextBox, KeyValuePair<IAutoComplete2, EnumString>>();
        }

        private static IAutoComplete2 GetAutoComplete()
        {
            var CAutoComplete = Type.GetTypeFromCLSID(CLSID_AutoComplete);

            return (IAutoComplete2)Activator.CreateInstance(CAutoComplete);
        }

        public static void SetAutocomplete(this TextBox textBox, IEnumerable<string> strings)
        {
            if (autoCompletedTextboxes.ContainsKey(textBox))
            {
                var pair = autoCompletedTextboxes[textBox];
                var autoComplete = pair.Key;
                var enumStrings = pair.Value;
                var autoCompleteDropdown = (IAutoCompleteDropDown)autoComplete;

                enumStrings.ResetStrings(strings);

                autoCompleteDropdown.ResetEnumerator();
            }
            else
            {
                var enumStrings = new EnumString(strings);
                var autoComplete = GetAutoComplete();
                int hr;

                hr = autoComplete.Init(textBox.Handle, enumStrings, 0, 0);

                if (hr != 0)
                {
                    DebugUtils.Break();
                }

                textBox.HandleDestroyed += (sender, e) => 
                {
                    var pair = autoCompletedTextboxes[textBox];
                    var autoComplete2 = pair.Key;

                    Marshal.ReleaseComObject(autoComplete2);

                    autoCompletedTextboxes.Remove((TextBox) sender);
                };

                autoComplete.SetOptions(AUTOCOMPLETEOPTIONS.ACO_AUTOSUGGEST | AUTOCOMPLETEOPTIONS.ACO_UPDOWNKEYDROPSLIST);

                autoCompletedTextboxes.Add(textBox, new KeyValuePair<IAutoComplete2, EnumString>(autoComplete, enumStrings));
            }
        }
    }

    internal class EnumString : IEnumString
    {
        private IEnumerable<string> strings;
        private int index = 0;
        private const int S_OK = 0;
        private const int S_FALSE = 1;

        public EnumString(IEnumerable<string> strings)
        {
            this.strings = strings;
        }

        public void ResetStrings(IEnumerable<string> strings)
        {
            index = 0;
            this.strings = strings;
        }

        public unsafe static void CopyString(char* buffer, string str)
        {
            var ch = buffer;

            foreach (char b in str.ToCharArray())
            {
                *ch = b;
                ch++;
            }

            *ch = '\0';
        }

        public unsafe int Next(int celt, IntPtr* rgelt, out int pceltFetched)
        {
            if (index < strings.Count() + celt)
            {
                var array = strings.Skip(index).Take(celt).ToArray();
                var size = array.Sum(s => (s.Length * 2) + (array.Count() * 2));
                IntPtr ptr = (IntPtr) Marshal.AllocHGlobal(size);

                *rgelt = ptr;
                index++;

                foreach (var str in array)
                {
                    CopyString((char*) ptr, str);

                    ptr += (str.Length + 1);
                }

                pceltFetched = array.Length;

                return S_OK;
            }
            else
            {
                pceltFetched = 0;
                return S_FALSE;
            }
        }

        public int Skip(int celt)
        {
            throw new NotImplementedException();
        }

        public int Reset()
        {
            index = 0;
            return S_OK;
        }

        public int Clone(out IEnumString ppenum)
        {
            throw new NotImplementedException();
        }
    }

    [Guid("00000101-0000-0000-C000-000000000046")]
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumString
    {
        [PreserveSig]
        unsafe int Next(int celt, IntPtr* rgelt, out int pceltFetched);
        [PreserveSig]
        int Skip(int celt);
        [PreserveSig]
        int Reset();
        [PreserveSig]
        int Clone(out IEnumString ppenum);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EAC04BC0-3791-11D2-BB95-0060977B464C")]
    public interface IAutoComplete2
    {
        [PreserveSig]
        int Init(IntPtr hwndEdit, [MarshalAs(UnmanagedType.IUnknown)] object punkACL, int pwszRegKeyPath, int pwszQuickComplete);

        // Enables or disables autocompletion.
        [PreserveSig] int Enable(bool value);

        // Sets the current autocomplete options.
        [PreserveSig] int SetOptions(AUTOCOMPLETEOPTIONS dwFlag);

        // Retrieves the current autocomplete options.
        [PreserveSig] int GetOptions(out AUTOCOMPLETEOPTIONS pdwFlag);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3CD141F4-3C6A-11d2-BCAA-00C04FD929DB")]
    public interface IAutoCompleteDropDown
    {
        int GetDropDownStatus(IntPtr pdwFlags, string ppwszString);
        int ResetEnumerator();
    }

    [Flags]
    public enum AUTOCOMPLETEOPTIONS
    {
        ACO_NONE = 0x0000,
        ACO_AUTOSUGGEST = 0x0001,
        ACO_AUTOAPPEND = 0x0002,
        ACO_SEARCH = 0x0004,
        ACO_FILTERPREFIXES = 0x0008,
        ACO_USETAB = 0x0010,
        ACO_UPDOWNKEYDROPSLIST = 0x0020,
        ACO_RTLREADING = 0x0040,
        ACO_WORD_FILTER = 0x0080,
        ACO_NOPREFIXFILTERING = 0x0100
    }
}
