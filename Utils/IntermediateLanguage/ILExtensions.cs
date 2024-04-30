using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.IntermediateLanguage;

public static class ILExtensions
{
    public static IEnumerable<ILInstruction> GetGetInstructions(this PropertyInfo property, Func<ILInstruction, bool>? filter = null)
    {
        return property.GetGetMethod().GetInstructions(filter);
    }

    public static IEnumerable<ILInstruction> GetSetInstructions(this PropertyInfo property, Func<ILInstruction, bool>? filter = null)
    {
        return property.GetSetMethod().GetInstructions(filter);
    }

    public static IEnumerable<ILInstruction> GetInstructions(this MethodInfo method, Func<ILInstruction, bool>? filter = null)
    {
        var methodBodyReader = new MethodBodyReader(method);

        foreach (var instruction in methodBodyReader.Instructions)
        {
            if (filter != null)
            {
                if (filter(instruction))
                {
                    yield return instruction;
                }
            }
            else
            {
                yield return instruction;
            }
        }
    }

    public static string GetGetBodyCode(this PropertyInfo property, Func<ILInstruction, bool>? filter = null)
    {
        return property.GetGetMethod().GetBodyCode(filter);
    }

    public static string GetSetBodyCode(this PropertyInfo property, Func<ILInstruction, bool>? filter = null)
    {
        return property.GetSetMethod().GetBodyCode(filter);
    }

    public static string GetBodyCode(this MethodInfo method, Func<ILInstruction, bool>? filter = null)
    {
        var methodBodyReader = new MethodBodyReader(method);

        return methodBodyReader.GetBodyCode();
    }
}
