using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.WindowsSearch.Interfaces
{
    public enum PROXY_ACCESS
    {	
        PROXY_ACCESS_PRECONFIG	= 0,
	    PROXY_ACCESS_DIRECT	= ( PROXY_ACCESS_PRECONFIG + 1 ) ,
	    PROXY_ACCESS_PROXY	= ( PROXY_ACCESS_DIRECT + 1 ) 
    }
}
