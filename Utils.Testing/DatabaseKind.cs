using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public enum DatabaseKind
    {
        SqlLite,
        SqlLiteSingleThread,
        SqlLiteFile,
        SqlLiteNewFile,
        SqlLiteNewSingleThreadFile,
        EFInMemory,
        ServiceFile
    }
}
