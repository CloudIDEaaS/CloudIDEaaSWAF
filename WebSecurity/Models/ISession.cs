using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models;

public interface ISession : IStorageBase
{
    string SessionVariable { get; set; }
}
