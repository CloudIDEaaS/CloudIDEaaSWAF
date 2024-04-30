using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class OSVersion
    {
        public static string GetOSName(this OperatingSystem operatingSystem)
        {
            switch (operatingSystem.Platform)
            {
                case PlatformID.Win32S:

                    return "Win 3.1";

                case PlatformID.Win32Windows:

                    switch (operatingSystem.Version.Minor)
                    {
                        case 0:
                            return "Win95";
                        case 10:
                            return "Win98";
                        case 90:
                            return "WinME";
                    }

                    break;

                case PlatformID.Win32NT:

                    switch (operatingSystem.Version.Major)
                    {
                        case 3:
                            return "NT 3.51";
                        case 4:
                            return "NT 4.0";
                        case 5:
                            switch (operatingSystem.Version.Minor)
                            {
                                case 0:
                                    return "Win2000";
                                case 1:
                                    return "WinXP";
                                case 2:
                                    return "Win2003";
                            }
                            break;

                        case 6:
                            switch (operatingSystem.Version.Minor)
                            {
                                case 0:
                                    return "Vista/Win2008Server";
                                case 1:
                                    return "Win7/Win2008Server R2";
                                case 2:
                                    return "Win8/Win2012Server";
                            }
                            break;
                    }

                    break;

                case PlatformID.WinCE:
                    return "Win CE";
            }

            return "Unknown";
        }
    }
}
