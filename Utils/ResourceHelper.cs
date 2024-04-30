using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.Drawing;

namespace Utils
{
    public static class ResourceLoader
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPTStr)]string FileName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int LoadString(IntPtr hInstance, uint uID, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int BufferMax);
        [DllImport("User32.dll", EntryPoint = "LoadIconW", SetLastError = true)]
        internal static extern IntPtr LoadIcon(IntPtr hInst, string sID);
        [DllImport("User32.dll", EntryPoint = "LoadIconW", SetLastError = true)]
        internal static extern IntPtr LoadIcon(IntPtr hInst, int iID);

        internal const int MAX_PATH = 256;

        public static Icon LoadIconFrom(string dllPath, string resourceName)
        {
            var uri = new Uri(dllPath);

            if (uri.Scheme == "file")
            {
                dllPath = uri.LocalPath;
            }

            if (IOExtensions.IsClrImage(dllPath))
            {
                var assm = Assembly.LoadFile(dllPath);
                var stream = assm.GetManifestResourceStream(resourceName);

                if (stream == null)
                {
                    var names = assm.GetManifestResourceNames();

                    foreach (var name in names)
                    {
                        try
                        {
                            stream = assm.GetManifestResourceStream(name);

                            var resourceReader = new ResourceReader(stream);
                            var nameFind = resourceReader.Cast<DictionaryEntry>().SingleOrDefault(k => ((string)k.Key) == resourceName);

                            if (nameFind.Key != null)
                            {
                                return (Icon)nameFind.Value;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            else
            {
                var buffer = new StringBuilder(MAX_PATH);
                var hInst = LoadLibrary(dllPath);

                if (hInst != IntPtr.Zero)
                {
                    var hIcon = LoadIcon(hInst, resourceName);

                    if (hIcon != IntPtr.Zero)
                    {
                        return Icon.FromHandle(hIcon);
                    }
                    else
                    {
                        var id = 0;

                        if (resourceName.StartsWith("#"))
                        {
                            id = int.Parse(resourceName.RemoveStart(1));
                        }
                        else
                        {
                            id = int.Parse(resourceName);
                        }

                        hIcon = LoadIcon(hInst, id);

                        if (hIcon != IntPtr.Zero)
                        {
                            return Icon.FromHandle(hIcon);
                        }
                    }
                }
            }

            return null;
        }

        public static string LoadStringFrom(string dllPath, string resourceName)
        {
            if (IOExtensions.IsClrImage(dllPath))
            {
                var assm = Assembly.LoadFile(dllPath);
                var stream = assm.GetManifestResourceStream(resourceName);

                if (stream == null)
                {
                    var names = assm.GetManifestResourceNames();

                    foreach (var name in names)
                    {
                        try
                        {
                            stream = assm.GetManifestResourceStream(name);

                            var resourceReader = new ResourceReader(stream);
                            var nameFind = resourceReader.Cast<DictionaryEntry>().SingleOrDefault(k => ((string) k.Key) == resourceName);

                            if (nameFind.Key != null)
                            {
                                return (string)nameFind.Value;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            else
            {
                var buffer = new StringBuilder(MAX_PATH);
                var hInst = LoadLibrary(dllPath);
                var id = 0;

                if (resourceName.StartsWith("#"))
                {
                    id = int.Parse(resourceName.RemoveStart(1));
                }
                else
                {
                    id = int.Parse(resourceName);
                }

                if (hInst != IntPtr.Zero)
                {
                    var length = LoadString(hInst, (uint)id, buffer, MAX_PATH);

                    if (length > 0)
                    {
                        return buffer.ToString();
                    }
                }
            }

            return null;
        }
    }
}
