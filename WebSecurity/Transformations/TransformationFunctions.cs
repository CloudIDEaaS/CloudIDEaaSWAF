using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.Transformations;

public static class TransformationFunctions
{
    [Transformation(TransformationType.Lowercase)]
    public static object Lowercase(object text)
    {
        return ((string) text).ToLower();
    }

    [Transformation(TransformationType.Sha1)]
    public static object Sha1(object text)
    {
        return ((string)text).GetSha1();
    }

    [Transformation(TransformationType.HexEncode)]
    public static object HexEncode(object bytes)
    {
        return ((byte[])bytes).GetHexString(255, false, true, true);
    }
}
