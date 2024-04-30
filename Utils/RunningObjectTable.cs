using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Utils
{
    public enum RotFlags : int
    {
        ROTFLAGS_REGISTRATIONKEEPSALIVE = 0x01,
        ROTFLAGS_ALLOWANYCLIENT = 0x02
    }

    public static class RunningObjectTable
    {
        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        [DllImport("ole32.dll")]
        private static extern int CreateFileMoniker([MarshalAs(UnmanagedType.LPWStr)] string lpszPathName, out System.Runtime.InteropServices.ComTypes.IMoniker ppmk);

        [DllImport("oleaut32.dll")]
        private static extern int RevokeActiveObject(int register, IntPtr reserved);

        private static Dictionary<string, int> runningObjects;

        static RunningObjectTable()
        {
            runningObjects = new Dictionary<string, int>();
        }

        public static void RevokeAll()
        {
            foreach (var key in runningObjects.Keys)
            {
                RevokeObject(key);
            }
        }

        public static void RegisterObject(string name, object obj)
        {
            IRunningObjectTable runningObjectTable;
            IMoniker moniker = null;
            int id;

            var hr = GetRunningObjectTable(0, out runningObjectTable);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error getting running object table", exception);
            }

            hr = CreateFileMoniker(name, out moniker);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error creating file moniker", exception);
            }

            id = runningObjectTable.Register((int) RotFlags.ROTFLAGS_REGISTRATIONKEEPSALIVE, obj, moniker);

            if (id == 0)
            {
                throw new Exception(string.Format("Error registering object with running object table, name='{0}'", name));
            }

            runningObjects.Add(name, id);
        }

        public static void RegisterComObject(string name, object obj)
        {
            IRunningObjectTable runningObjectTable;
            IMoniker moniker = null;
            int id;

            var hr = GetRunningObjectTable(0, out runningObjectTable);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error getting running object table", exception);
            }

            hr = CreateFileMoniker(name, out moniker);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error creating file moniker", exception);
            }

            id = runningObjectTable.Register((int)RotFlags.ROTFLAGS_REGISTRATIONKEEPSALIVE, obj, moniker);

            if (id == 0)
            {
                throw new Exception(string.Format("Error registering object with running object table, name='{0}'", name));
            }

            runningObjects.Add(name, id);
        }

        public static bool ObjectRunning(string name)
        {
            IRunningObjectTable runningObjectTable;
            IEnumMoniker enumMoniker;
            var hr = GetRunningObjectTable(0, out runningObjectTable);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error getting running object table", exception);
            }

            runningObjectTable.EnumRunning(out enumMoniker);

            var fetched = IntPtr.Zero;
            var moniker = new IMoniker[1];

            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                string displayName;

                CreateBindCtx(0, out bindCtx);
                moniker[0].GetDisplayName(bindCtx, null, out displayName);

                if (displayName == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static void RevokeObject(string name)
        {
            IRunningObjectTable runningObjectTable;
            IEnumMoniker enumMoniker;
            var hr = GetRunningObjectTable(0, out runningObjectTable);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error getting running object table", exception);
            }

            runningObjectTable.EnumRunning(out enumMoniker);

            var fetched = IntPtr.Zero;
            var moniker = new IMoniker[1];

            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                string displayName;

                CreateBindCtx(0, out bindCtx);
                moniker[0].GetDisplayName(bindCtx, null, out displayName);

                if (displayName == name)
                {
                    var id = runningObjects[name];

                    runningObjectTable.Revoke(id);
                    runningObjects.Remove(name);

                    return;
                }
            }

            throw new Exception("Object not found in running object table");
        }

        public static T GetComObject<T>(string name)
        {
            IRunningObjectTable runningObjectTable;
            IEnumMoniker enumMoniker;
            var hr = GetRunningObjectTable(0, out runningObjectTable);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error getting running object table", exception);
            }

            runningObjectTable.EnumRunning(out enumMoniker);

            var fetched = IntPtr.Zero;
            var moniker = new IMoniker[1];

            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                string displayName;
                object obj;
                var type = typeof(T);
                T returnObject;

                CreateBindCtx(0, out bindCtx);
                moniker[0].GetDisplayName(bindCtx, null, out displayName);

                if (displayName == name)
                {
                    hr = runningObjectTable.GetObject(moniker[0], out obj);

                    if (hr != 0)
                    {
                        var exception = Marshal.GetExceptionForHR(hr);

                        throw new Exception(string.Format("Error getting object from running object table, name='{0}'", name), exception);
                    }

                    var ptr = Marshal.GetIUnknownForObject(obj);

                    if (type.HasCustomAttribute<GuidAttribute>())
                    {
                        var guidAttribute = type.GetCustomAttribute<GuidAttribute>();
                        var guid = Guid.Parse(guidAttribute.Value);

                        hr = Marshal.QueryInterface(ptr, ref guid, out ptr);

                        if (hr != 0)
                        {
                            var exception = Marshal.GetExceptionForHR(hr);

                            throw new InvalidCastException(string.Format("Error converting object to type ='{0}', name='{1}'", guid.ToString(), name), exception);
                        }
                    }

                    returnObject = (T) Marshal.GetObjectForIUnknown(ptr);

                    return returnObject;
                }
            }

            throw new Exception(string.Format("Object not found in running object table, name='{0}'", name));
        }

        public static T GetObject<T>(string name) where T : class
        {
            IRunningObjectTable runningObjectTable;
            IEnumMoniker enumMoniker;
            var hr = GetRunningObjectTable(0, out runningObjectTable);

            if (hr != 0)
            {
                var exception = Marshal.GetExceptionForHR(hr);

                throw new Exception("Error getting running object table", exception);
            }

            runningObjectTable.EnumRunning(out enumMoniker);

            var fetched = IntPtr.Zero;
            var moniker = new IMoniker[1];

            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                string displayName;
                object returnObject;

                CreateBindCtx(0, out bindCtx);
                moniker[0].GetDisplayName(bindCtx, null, out displayName);

                if (displayName == name)
                {
                    hr = runningObjectTable.GetObject(moniker[0], out returnObject);

                    if (hr != 0)
                    {
                        var exception = Marshal.GetExceptionForHR(hr);

                        throw new Exception(string.Format("Error getting object from running object table, name='{0}'", name), exception);
                    }

                    return (T)returnObject;
                }
            }

            throw new Exception(string.Format("Object not found in running object table, name='{0}'", name));
        }
    }
}
