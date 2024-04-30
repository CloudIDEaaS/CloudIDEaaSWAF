using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum CertificateType : ushort
    {
        X509 = 0x0001,                /* X.509 Certificate */
        PKCS_Signed_data = 0x0002,    /* PKCS SignedData */
        Reserved = 0x0003,          /* Reserved */
        TS_Stack_Signed = 0x0004
    }
}
