using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.StartupTests.Models
{
    public enum RuleUniquenessKind
    {
        UnaryBinaryPropertyPresence,
        UnaryBinary,
        UnaryBinaryNonUnique,
        UniqueInputsForUnaryBinary,
        UniqueInputs,
        UniqueInputsAndOperators,
        UniqueOperators,
        NullOrNot,
    }
}
