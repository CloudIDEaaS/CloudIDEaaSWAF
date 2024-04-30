using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Utils
{
	#region "OLE definitions"
	// STGM
	[Flags(), ComVisible(false)]
	public enum STGM : int
	{
		STGM_DIRECT = 0x0,
		STGM_TRANSACTED = 0x10000,
		STGM_SIMPLE = 0x8000000,
		STGM_READ = 0x0,
		STGM_WRITE = 0x1,
		STGM_READWRITE = 0x2,
		STGM_SHARE_DENY_NONE = 0x40,
		STGM_SHARE_DENY_READ = 0x30,
		STGM_SHARE_DENY_WRITE = 0x20,
		STGM_SHARE_EXCLUSIVE = 0x10,
		STGM_PRIORITY = 0x40000,
		STGM_DELETEONRELEASE = 0x4000000,
		STGM_NOSCRATCH = 0x100000,
		STGM_CREATE = 0x1000,
		STGM_CONVERT = 0x20000,
		STGM_FAILIFTHERE = 0x0,
		STGM_NOSNAPSHOT = 0x200000,
	}

	// DVASPECT
	[Flags(), ComVisible(false)]
	public enum DVASPECT : int
	{
		DVASPECT_CONTENT = 1,
		DVASPECT_THUMBNAIL = 2,
		DVASPECT_ICON = 4,
		DVASPECT_DOCPRINT = 8,
		DVASPECT_OPAQUE = 16,
		DVASPECT_TRANSPARENT = 32,
	}

	// CLIPFORMAT
	[ComVisible(false)]
	public enum CLIPFORMAT : int
	{
		CF_TEXT = 1,
		CF_BITMAP = 2,
		CF_METAFILEPICT = 3,
		CF_SYLK = 4,
		CF_DIF = 5,
		CF_TIFF = 6,
		CF_OEMTEXT = 7,
		CF_DIB = 8,
		CF_PALETTE = 9,
		CF_PENDATA = 10,
		CF_RIFF = 11,
		CF_WAVE = 12,
		CF_UNICODETEXT = 13,
		CF_ENHMETAFILE = 14,
		CF_HDROP = 15,
		CF_LOCALE = 16,
		CF_MAX = 17,
		CF_OWNERDISPLAY = 0x80,
		CF_DSPTEXT = 0x81,
		CF_DSPBITMAP = 0x82,
		CF_DSPMETAFILEPICT = 0x83,
		CF_DSPENHMETAFILE = 0x8E,
	}

	// Object flags
	[Flags(), ComVisible(false)]
	public enum REOOBJECTFLAGS : uint
	{
		REO_NULL = 0x00000000,  // No flags
		REO_READWRITEMASK = 0x0000003F, // Mask out RO bits
		REO_DONTNEEDPALETTE = 0x00000020,   // Object doesn't need palette
		REO_BLANK = 0x00000010, // Object is blank
		REO_DYNAMICSIZE = 0x00000008,   // Object defines size always
		REO_INVERTEDSELECT = 0x00000004,    // Object drawn all inverted if sel
		REO_BELOWBASELINE = 0x00000002, // Object sits below the baseline
		REO_RESIZABLE = 0x00000001, // Object may be resized
		REO_LINK = 0x80000000,  // Object is a link (RO)
		REO_STATIC = 0x40000000,    // Object is static (RO)
		REO_SELECTED = 0x08000000,  // Object selected (RO)
		REO_OPEN = 0x04000000,  // Object open in its server (RO)
		REO_INPLACEACTIVE = 0x02000000, // Object in place active (RO)
		REO_HILITED = 0x01000000,   // Object is to be hilited (RO)
		REO_LINKAVAILABLE = 0x00800000, // Link believed available (RO)
		REO_GETMETAFILE = 0x00400000    // Object requires metafile (RO)
	}

	// OLERENDER
	[ComVisible(false)]
	public enum OLERENDER : int
	{
		OLERENDER_NONE = 0,
		OLERENDER_DRAW = 1,
		OLERENDER_FORMAT = 2,
		OLERENDER_ASIS = 3,
	}

	// TYMED
	[Flags(), ComVisible(false)]
	public enum TYMED : int
	{
		TYMED_NULL = 0,
		TYMED_HGLOBAL = 1,
		TYMED_FILE = 2,
		TYMED_ISTREAM = 4,
		TYMED_ISTORAGE = 8,
		TYMED_GDI = 16,
		TYMED_MFPICT = 32,
		TYMED_ENHMF = 64,
	}

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct FORMATETC
	{
		public CLIPFORMAT cfFormat;
		public IntPtr ptd;
		public DVASPECT dwAspect;
		public int lindex;
		public TYMED tymed;
	}

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct STGMEDIUM
	{
		//[MarshalAs(UnmanagedType.I4)]
		public int tymed;
		public IntPtr unionmember;
		public IntPtr pUnkForRelease;
	}

	[ComVisible(true),
	ComImport(),
	Guid("00000103-0000-0000-C000-000000000046"),
	InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumFORMATETC
	{
		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Next(
			[In, MarshalAs(UnmanagedType.U4)]
			int celt,
			[Out]
			FORMATETC rgelt,
			[In, Out, MarshalAs(UnmanagedType.LPArray)]
			int[] pceltFetched);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Skip(
			[In, MarshalAs(UnmanagedType.U4)]
			int celt);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Reset();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Clone(
			[Out, MarshalAs(UnmanagedType.LPArray)]
			IEnumFORMATETC[] ppenum);
	}

	[ComVisible(true), StructLayout(LayoutKind.Sequential)]
	public class COMRECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;

		public COMRECT()
		{
		}

		public COMRECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public static COMRECT FromXYWH(int x, int y, int width, int height)
		{
			return new COMRECT(x, y, x + width, y + height);
		}
	}

	public enum GETOBJECTOPTIONS
	{
		REO_GETOBJ_NO_INTERFACES = 0x00000000,
		REO_GETOBJ_POLEOBJ = 0x00000001,
		REO_GETOBJ_PSTG = 0x00000002,
		REO_GETOBJ_POLESITE = 0x00000004,
		REO_GETOBJ_ALL_INTERFACES = 0x00000007,
	}

	public enum GETCLIPBOARDDATAFLAGS
	{
		RECO_PASTE = 0,
		RECO_DROP = 1,
		RECO_COPY = 2,
		RECO_CUT = 3,
		RECO_DRAG = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CHARRANGE
	{
		public int cpMin;
		public int cpMax;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class REOBJECT
	{
		public int cbStruct = Marshal.SizeOf(typeof(REOBJECT)); // Size of structure
		public int cp;                                          // Character position of object
		public Guid clsid;                                      // Class ID of object
		public IntPtr poleobj;                              // OLE object interface
		public IStorage pstg;                                   // Associated storage interface
		public IOleClientSite polesite;                         // Associated client site interface
		public Size sizel;                                      // Size of object (may be 0,0)
		public uint dvAspect;                                   // Display aspect to use
		public uint dwFlags;                                    // Object status flags
		public uint dwUser;                                     // Dword for user's use
	}

	[ComVisible(true), Guid("0000010F-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAdviseSink
	{

		//C#r: UNDONE (Field in interface) public static readonly    Guid iid;
		void OnDataChange(
			[In]
			FORMATETC pFormatetc,
			[In]
			STGMEDIUM pStgmed);

		void OnViewChange(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwAspect,
			[In, MarshalAs(UnmanagedType.I4)]
			int lindex);

		void OnRename(
			[In, MarshalAs(UnmanagedType.Interface)]
			object pmk);

		void OnSave();


		void OnClose();
	}

	[ComVisible(false), StructLayout(LayoutKind.Sequential)]
	public sealed class STATDATA
	{

		[MarshalAs(UnmanagedType.U4)]
		public int advf;
		[MarshalAs(UnmanagedType.U4)]
		public int dwConnection;

	}

	[ComVisible(false), StructLayout(LayoutKind.Sequential)]
	public sealed class tagOLEVERB
	{
		[MarshalAs(UnmanagedType.I4)]
		public int lVerb;

		[MarshalAs(UnmanagedType.LPWStr)]
		public String lpszVerbName;

		[MarshalAs(UnmanagedType.U4)]
		public int fuFlags;

		[MarshalAs(UnmanagedType.U4)]
		public int grfAttribs;

	}

	[ComVisible(true), ComImport(), Guid("00000104-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumOLEVERB
	{

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Next(
			[MarshalAs(UnmanagedType.U4)]
			int celt,
			[Out]
			tagOLEVERB rgelt,
			[Out, MarshalAs(UnmanagedType.LPArray)]
			int[] pceltFetched);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Skip(
			[In, MarshalAs(UnmanagedType.U4)]
			int celt);

		void Reset();


		void Clone(
			out IEnumOLEVERB ppenum);


	}

	[ComVisible(true), Guid("00000105-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATDATA
	{

		//C#r: UNDONE (Field in interface) public static readonly    Guid iid;

		void Next(
			[In, MarshalAs(UnmanagedType.U4)]
			int celt,
			[Out]
			STATDATA rgelt,
			[Out, MarshalAs(UnmanagedType.LPArray)]
			int[] pceltFetched);


		void Skip(
			[In, MarshalAs(UnmanagedType.U4)]
			int celt);


		void Reset();


		void Clone(
			[Out, MarshalAs(UnmanagedType.LPArray)]
			IEnumSTATDATA[] ppenum);


	}

	[ComVisible(true), Guid("0000011B-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleContainer
	{


		void ParseDisplayName(
			[In, MarshalAs(UnmanagedType.Interface)] object pbc,
			[In, MarshalAs(UnmanagedType.BStr)] string pszDisplayName,
			[Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
			[Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);


		void EnumObjects(
			[In, MarshalAs(UnmanagedType.U4)] int grfFlags,
			[Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenum);


		void LockContainer(
			[In, MarshalAs(UnmanagedType.I4)] int fLock);
	}

	[ComVisible(true),
	ComImport(),
	Guid("0000010E-0000-0000-C000-000000000046"),
	InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDataObject
	{
		[PreserveSig()]
		uint GetData(
			ref FORMATETC a,
			ref STGMEDIUM b);

		[PreserveSig()]
		uint GetDataHere(
			ref FORMATETC pFormatetc,
			out STGMEDIUM pMedium);

		[PreserveSig()]
		uint QueryGetData(
			ref FORMATETC pFormatetc);

		[PreserveSig()]
		uint GetCanonicalFormatEtc(
			ref FORMATETC pformatectIn,
			out FORMATETC pformatetcOut);

		[PreserveSig()]
		uint SetData(
			ref FORMATETC pFormatectIn,
			ref STGMEDIUM pmedium,
			[In, MarshalAs(UnmanagedType.Bool)]
			bool fRelease);

		[PreserveSig()]
		uint EnumFormatEtc(
			uint dwDirection, IEnumFORMATETC penum);

		[PreserveSig()]
		uint DAdvise(
			ref FORMATETC pFormatetc,
			int advf,
			[In, MarshalAs(UnmanagedType.Interface)]
			IAdviseSink pAdvSink,
			out uint pdwConnection);

		[PreserveSig()]
		uint DUnadvise(
			uint dwConnection);

		[PreserveSig()]
		uint EnumDAdvise(
			[Out, MarshalAs(UnmanagedType.Interface)]
			out IEnumSTATDATA ppenumAdvise);
	}

	[ComVisible(true), Guid("00000118-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleClientSite
	{

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SaveObject();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetMoniker(
			[In, MarshalAs(UnmanagedType.U4)] int dwAssign,
			[In, MarshalAs(UnmanagedType.U4)] int dwWhichMoniker,
			[Out, MarshalAs(UnmanagedType.Interface)] out object ppmk);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetContainer([MarshalAs(UnmanagedType.Interface)] out IOleContainer container);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int ShowObject();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int OnShowWindow(
			[In, MarshalAs(UnmanagedType.I4)] int fShow);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int RequestNewObjectLayout();
	}

	[ComVisible(false), StructLayout(LayoutKind.Sequential)]
	public sealed class tagLOGPALETTE
	{
		[MarshalAs(UnmanagedType.U2)/*leftover(offset=0, palVersion)*/]
		public short palVersion;

		[MarshalAs(UnmanagedType.U2)/*leftover(offset=2, palNumEntries)*/]
		public short palNumEntries;

		// UNMAPPABLE: palPalEntry: Cannot be used as a structure field.
		//   /** @com.structmap(UNMAPPABLE palPalEntry) */
		//  public UNMAPPABLE palPalEntry;
	}

	[ComVisible(true), ComImport(), Guid("00000112-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleObject
	{

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetClientSite(
			[In, MarshalAs(UnmanagedType.Interface)]
			IOleClientSite pClientSite);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetClientSite(out IOleClientSite site);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetHostNames(
			[In, MarshalAs(UnmanagedType.LPWStr)]
			string szContainerApp,
			[In, MarshalAs(UnmanagedType.LPWStr)]
			string szContainerObj);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Close(
			[In, MarshalAs(UnmanagedType.I4)]
			int dwSaveOption);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetMoniker(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwWhichMoniker,
			[In, MarshalAs(UnmanagedType.Interface)]
			object pmk);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetMoniker(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwAssign,
			[In, MarshalAs(UnmanagedType.U4)]
			int dwWhichMoniker,
			out object moniker);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int InitFromData(
			[In, MarshalAs(UnmanagedType.Interface)]
			IDataObject pDataObject,
			[In, MarshalAs(UnmanagedType.I4)]
			int fCreation,
			[In, MarshalAs(UnmanagedType.U4)]
			int dwReserved);

		int GetClipboardData(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwReserved,
			out IDataObject data);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int DoVerb(
			[In, MarshalAs(UnmanagedType.I4)]
			int iVerb,
			[In]
			IntPtr lpmsg,
			[In, MarshalAs(UnmanagedType.Interface)]
			IOleClientSite pActiveSite,
			[In, MarshalAs(UnmanagedType.I4)]
			int lindex,
			[In]
			IntPtr hwndParent,
			[In]
			COMRECT lprcPosRect);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int EnumVerbs(out IEnumOLEVERB e);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Update();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int IsUpToDate();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetUserClassID(
			[In, Out]
			ref Guid pClsid);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetUserType(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwFormOfType,
			[Out, MarshalAs(UnmanagedType.LPWStr)]
			out string userType);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetExtent(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwDrawAspect,
			[In]
			Size pSizel);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetExtent(
			[In, MarshalAs(UnmanagedType.U4)]
			int dwDrawAspect,
			[Out]
			Size pSizel);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Advise([In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink, out int cookie);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int Unadvise([In, MarshalAs(UnmanagedType.U4)] int dwConnection);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int EnumAdvise(out IEnumSTATDATA e);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetMiscStatus([In, MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetColorScheme([In] tagLOGPALETTE pLogpal);
	}

	[ComImport]
	[Guid("0000000d-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATSTG
	{
		// The user needs to allocate an STATSTG array whose size is celt.
		[PreserveSig]
		uint
			Next(
			uint celt,
			[MarshalAs(UnmanagedType.LPArray), Out]
			STATSTG[] rgelt,
			out uint pceltFetched
			);

		void Skip(uint celt);

		void Reset();

		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumSTATSTG Clone();
	}

	[ComImport]
	[Guid("0000000b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStorage
	{
		int CreateStream(
			/* [string][in] */ string pwcsName,
			/* [in] */ uint grfMode,
			/* [in] */ uint reserved1,
			/* [in] */ uint reserved2,
			/* [out] */ out IStream ppstm);

		int OpenStream(
			/* [string][in] */ string pwcsName,
			/* [unique][in] */ IntPtr reserved1,
			/* [in] */ uint grfMode,
			/* [in] */ uint reserved2,
			/* [out] */ out IStream ppstm);

		int CreateStorage(
			/* [string][in] */ string pwcsName,
			/* [in] */ uint grfMode,
			/* [in] */ uint reserved1,
			/* [in] */ uint reserved2,
			/* [out] */ out IStorage ppstg);

		int OpenStorage(
			/* [string][unique][in] */ string pwcsName,
			/* [unique][in] */ IStorage pstgPriority,
			/* [in] */ uint grfMode,
			/* [unique][in] */ IntPtr snbExclude,
			/* [in] */ uint reserved,
			/* [out] */ out IStorage ppstg);

		int CopyTo(
			/* [in] */ uint ciidExclude,
			/* [size_is][unique][in] */ Guid rgiidExclude,
			/* [unique][in] */ IntPtr snbExclude,
			/* [unique][in] */ IStorage pstgDest);

		int MoveElementTo(
			/* [string][in] */ string pwcsName,
			/* [unique][in] */ IStorage pstgDest,
			/* [string][in] */ string pwcsNewName,
			/* [in] */ uint grfFlags);

		int Commit(
			/* [in] */ uint grfCommitFlags);

		int Revert();

		int EnumElements(
			/* [in] */ uint reserved1,
			/* [size_is][unique][in] */ IntPtr reserved2,
			/* [in] */ uint reserved3,
			/* [out] */ out IEnumSTATSTG ppenum);

		int DestroyElement(
			/* [string][in] */ string pwcsName);

		int RenameElement(
			/* [string][in] */ string pwcsOldName,
			/* [string][in] */ string pwcsNewName);

		int SetElementTimes(
			/* [string][unique][in] */ string pwcsName,
			/* [unique][in] */ FILETIME pctime,
			/* [unique][in] */ FILETIME patime,
			/* [unique][in] */ FILETIME pmtime);

		int SetClass(
			/* [in] */ Guid clsid);

		int SetStateBits(
			/* [in] */ uint grfStateBits,
			/* [in] */ uint grfMask);

		int Stat(
			/* [out] */ out STATSTG pstatstg,
			/* [in] */ uint grfStatFlag);

	}

	[ComImport]
	[Guid("0000000a-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ILockBytes
	{
		int ReadAt(
		/* [in] */ ulong ulOffset,
		/* [unique][out] */ IntPtr pv,
		/* [in] */ uint cb,
		/* [out] */ out IntPtr pcbRead);

		int WriteAt(
		/* [in] */ ulong ulOffset,
		/* [size_is][in] */ IntPtr pv,
		/* [in] */ uint cb,
		/* [out] */ out IntPtr pcbWritten);

		int Flush();

		int SetSize(
		/* [in] */ ulong cb);

		int LockRegion(
		/* [in] */ ulong libOffset,
		/* [in] */ ulong cb,
		/* [in] */ uint dwLockType);

		int UnlockRegion(
		/* [in] */ ulong libOffset,
		/* [in] */ ulong cb,
		/* [in] */ uint dwLockType);

		int Stat(
		/* [out] */ out STATSTG pstatstg,
		/* [in] */ uint grfStatFlag);

	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0c733a30-2a1c-11ce-ade5-00aa0044773d")]
	public interface ISequentialStream
	{
		int Read(
		/* [length_is][size_is][out] */ IntPtr pv,
		/* [in] */ uint cb,
		/* [out] */ out uint pcbRead);

		int Write(
		/* [size_is][in] */ IntPtr pv,
		/* [in] */ uint cb,
		/* [out] */ out uint pcbWritten);

	};

	[ComImport]
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStream : ISequentialStream
	{
		int Seek(
		/* [in] */ ulong dlibMove,
		/* [in] */ uint dwOrigin,
		/* [out] */ out ulong plibNewPosition);

		int SetSize(
		/* [in] */ ulong libNewSize);

		int CopyTo(
		/* [unique][in] */ [In] IStream pstm,
		/* [in] */ ulong cb,
		/* [out] */ out ulong pcbRead,
		/* [out] */ out ulong pcbWritten);

		int Commit(
		/* [in] */ uint grfCommitFlags);

		int Revert();

		int LockRegion(
		/* [in] */ ulong libOffset,
		/* [in] */ ulong cb,
		/* [in] */ uint dwLockType);

		int UnlockRegion(
		/* [in] */ ulong libOffset,
		/* [in] */ ulong cb,
		/* [in] */ uint dwLockType);

		int Stat(
		/* [out] */ out STATSTG pstatstg,
		/* [in] */ uint grfStatFlag);

		int Clone(
		/* [out] */ out IStream ppstm);

	};

	/// <summary>
	/// Definition for interface IPersist.
	/// </summary>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010c-0000-0000-C000-000000000046")]
	public interface IPersist
	{
		/// <summary>
		/// getClassID
		/// </summary>
		/// <param name="pClassID"></param>
		void GetClassID( /* [out] */ out Guid pClassID);
	}

	/// <summary>
	/// Definition for interface IPersistStream.
	/// </summary>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000109-0000-0000-C000-000000000046")]
	public interface IPersistStream : IPersist
	{
		/// <summary>
		/// GetClassID
		/// </summary>
		/// <param name="pClassID"></param>
		new void GetClassID(out Guid pClassID);
		/// <summary>
		/// isDirty
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int IsDirty();
		/// <summary>
		/// Load
		/// </summary>
		/// <param name="pStm"></param>
		void Load([In] IStream pStm);
		/// <summary>
		/// Save
		/// </summary>
		/// <param name="pStm"></param>
		/// <param name="fClearDirty"></param>
		void Save([In] IStream pStm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
		/// <summary>
		/// GetSizeMax
		/// </summary>
		/// <param name="pcbSize"></param>
		void GetSizeMax(out long pcbSize);
	}

	[ComImport(), Guid("00020D00-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IRichEditOle
	{
		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetClientSite(out IOleClientSite site);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetObjectCount();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetLinkCount();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetObject(int iob, [In, Out] REOBJECT lpreobject, [MarshalAs(UnmanagedType.U4)] GETOBJECTOPTIONS flags);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int InsertObject(REOBJECT lpreobject);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int ConvertObject(int iob, Guid rclsidNew, string lpstrUserTypeNew);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int ActivateAs(Guid rclsid, Guid rclsidAs);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetHostNames(string lpstrContainerApp, string lpstrContainerObj);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetLinkAvailable(int iob, bool fAvailable);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetDvaspect(int iob, uint dvaspect);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int HandsOffStorage(int iob);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SaveCompleted(int iob, IStorage lpstg);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int InPlaceDeactivate();

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int ContextSensitiveHelp(bool fEnterMode);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetClipboardData([In, Out] ref CHARRANGE lpchrg, [MarshalAs(UnmanagedType.U4)] GETCLIPBOARDDATAFLAGS reco, out IDataObject lplpdataobj);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int ImportDataObject(IDataObject lpdataobj, int cf, IntPtr hMetaPict);
	}
	#endregion

	public class DataObject : IDataObject
	{
		private Bitmap mBitmap;
		public FORMATETC mpFormatetc;

		#region IDataObject Members

		private const uint S_OK = 0;
		private const uint E_POINTER = 0x80004003;
		private const uint E_NOTIMPL = 0x80004001;
		private const uint E_FAIL = 0x80004005;

		public uint GetData(ref FORMATETC pFormatetc, ref STGMEDIUM pMedium)
		{
			IntPtr hDst = mBitmap.GetHbitmap();

			pMedium.tymed = (int)TYMED.TYMED_GDI;
			pMedium.unionmember = hDst;
			pMedium.pUnkForRelease = IntPtr.Zero;

			return (uint)S_OK;
		}

		public uint GetDataHere(ref FORMATETC pFormatetc, out STGMEDIUM pMedium)
		{
			Trace.WriteLine("GetDataHere");

			pMedium = new STGMEDIUM();

			return (uint)E_NOTIMPL;
		}

		public uint QueryGetData(ref FORMATETC pFormatetc)
		{
			Trace.WriteLine("QueryGetData");

			return (uint)E_NOTIMPL;
		}

		public uint GetCanonicalFormatEtc(ref FORMATETC pFormatetcIn, out FORMATETC pFormatetcOut)
		{
			Trace.WriteLine("GetCanonicalFormatEtc");

			pFormatetcOut = new FORMATETC();

			return (uint)E_NOTIMPL;
		}

		public uint SetData(ref FORMATETC a, ref STGMEDIUM b, bool fRelease)
		{
			//mpFormatetc = pFormatectIn;
			//mpmedium = pmedium;

			Trace.WriteLine("SetData");

			return (int)S_OK;
		}

		public uint EnumFormatEtc(uint dwDirection, IEnumFORMATETC penum)
		{
			Trace.WriteLine("EnumFormatEtc");

			return (int)S_OK;
		}

		public uint DAdvise(ref FORMATETC a, int advf, IAdviseSink pAdvSink, out uint pdwConnection)
		{
			Trace.WriteLine("DAdvise");

			pdwConnection = 0;

			return (uint)E_NOTIMPL;
		}

		public uint DUnadvise(uint dwConnection)
		{
			Trace.WriteLine("DUnadvise");

			return (uint)E_NOTIMPL;
		}

		public uint EnumDAdvise(out IEnumSTATDATA ppenumAdvise)
		{
			Trace.WriteLine("EnumDAdvise");

			ppenumAdvise = null;

			return (uint)E_NOTIMPL;
		}

		#endregion

		public DataObject()
		{
			mBitmap = new Bitmap(16, 16);
			mpFormatetc = new FORMATETC();
		}

		public void SetImage(string strFilename)
		{
			try
			{
				mBitmap = (Bitmap)Bitmap.FromFile(strFilename, true);

				mpFormatetc.cfFormat = CLIPFORMAT.CF_BITMAP;                // Clipboard format = CF_BITMAP
				mpFormatetc.ptd = IntPtr.Zero;                          // Target Device = Screen
				mpFormatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;           // Level of detail = Full content
				mpFormatetc.lindex = -1;                            // Index = Not applicaple
				mpFormatetc.tymed = TYMED.TYMED_GDI;                    // Storage medium = HBITMAP handle
			}
			catch
			{
			}
		}

		public void SetImage(Image image)
		{
			try
			{
				mBitmap = new Bitmap(image);

				mpFormatetc.cfFormat = CLIPFORMAT.CF_BITMAP;                // Clipboard format = CF_BITMAP
				mpFormatetc.ptd = IntPtr.Zero;                          // Target Device = Screen
				mpFormatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;           // Level of detail = Full content
				mpFormatetc.lindex = -1;                            // Index = Not applicaple
				mpFormatetc.tymed = TYMED.TYMED_GDI;                    // Storage medium = HBITMAP handle
			}
			catch
			{
			}
		}
	}
}
