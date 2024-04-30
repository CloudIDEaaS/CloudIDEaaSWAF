using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models;

public interface IStorageBase
{
    object? this[string indexOrRegex] { get; set; }
    void Save();
}
