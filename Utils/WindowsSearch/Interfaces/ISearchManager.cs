using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utils.WindowsSearch.Interfaces
{
	[ComImport, Guid("AB310581-AC80-11D1-8DF3-00C04FB6EF69"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISearchManager
	{
		int GetIndexerVersionStr(string ppszVersionString);
		int GetIndexerVersion(uint pdwMajor, uint pdwMinor);
		int GetParameter(string pszName, object ppValue);
		int SetParameter(string pszName, object pValue);
		int get_ProxyName(string ppszProxyName);
		int get_BypassList(string ppszBypassList);
		int SetProxy(PROXY_ACCESS sUseProxy, int fLocalByPassProxy, uint dwPortNumber, string pszProxyName, string pszByPassList);
		int GetCatalog(string pszCatalog, ISearchCatalogManager[] ppCatalogManager);
		int get_UserAgent(string ppszUserAgent);
		int put_UserAgent(string pszUserAgent);
		int get_UseProxy(PROXY_ACCESS pUseProxy);
		int get_LocalBypass(int pfLocalBypass);
		int get_PortNumber(uint pdwPortNumber);
	}
}
