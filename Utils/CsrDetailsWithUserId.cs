using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class CsrDetailsWithPrivateKey : CsrDetails
    {
        public SecureString PrivateKey { get; set; }
    }
}
