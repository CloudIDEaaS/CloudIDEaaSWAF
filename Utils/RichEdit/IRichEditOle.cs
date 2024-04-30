using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils.RichEdit
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00020D00-0000-0000-c000-000000000046")]
    public interface IRichEditOle
    {
        int GetClientSite(IntPtr lplpolesite);
        int GetObjectCount();
        int GetLinkCount();
        int GetObject(int iob, REOBJECT lpreobject, [MarshalAs(UnmanagedType.U4)] GetObjectOptions flags);
        int InsertObject(REOBJECT lpreobject);
        int ConvertObject(int iob, Guid rclsidNew, string lpstrUserTypeNew);
        int ActivateAs(Guid rclsid, Guid rclsidAs);
        int SetHostNames(string lpstrContainerApp, string lpstrContainerObj);
        int SetLinkAvailable(int iob, int fAvailable);
        int SetDvaspect(int iob, uint dvaspect);
        int HandsOffStorage(int iob);
        int SaveCompleted(int iob, IntPtr lpstg);
        int InPlaceDeactivate();
        int ContextSensitiveHelp(int fEnterMode);
        //int GetClipboardData(CHARRANGE FAR * lpchrg, uint reco, 
        //                                                     IntPtr lplpdataobj);
        //int ImportDataObject(IntPtr lpdataobj, CLIPFORMAT cf, HGLOBAL hMetaPict);
    }

    public enum GetObjectOptions
    {
        REO_GETOBJ_NO_INTERFACES = 0, // Get no interfaces
        REO_GETOBJ_POLEOBJ = 1, // Get object interface
        REO_GETOBJ_PSTG = 2, // Get storage interface
        REO_GETOBJ_POLESITE = 4, // Get site interface
        REO_GETOBJ_ALL_INTERFACES = 7  // Get all interfaces
    }

    [StructLayout(LayoutKind.Sequential)]
    public class REOBJECT
    {
        public REOBJECT()
        {
        }
        // Size of structure
        public int cbStruct = Marshal.SizeOf(typeof(REOBJECT));
        public int cp = 0;                    // Character position of object
        public Guid Guid = new Guid();      // Class ID of object
        public IntPtr poleobj = IntPtr.Zero;   // OLE object interface
        public IntPtr pstg = IntPtr.Zero;      // Associated storage interface
        public IntPtr polesite = IntPtr.Zero;  // Associated client site interface
        public Size sizel = new Size();      // Size of object (may be 0,0)
        public uint dvaspect = 0;             // Display aspect to use
        public uint dwFlags = 0;              // Object status flags
        public uint dwUser = 0;               // Dword for user's use
    }
}
