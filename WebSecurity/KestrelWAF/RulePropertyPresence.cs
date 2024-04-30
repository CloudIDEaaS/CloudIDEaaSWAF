using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.KestrelWAF
{
    [Flags]
    public enum RulePropertyPresence
    {
        NotSet = 0,
        MemberName = 1 << 1,
        InputMethod = 1 << 2,
        InputMethod2 = 1 << 3,
        TargetValue = 1 << 4,
        InputArgument = 1 << 5,
        InputArgument2 = 1 << 6,
        MemberName_InputMethod_InputArgument = MemberName | InputMethod | InputArgument,
        MemberName_InputMethod_InputArgument_InputArgument2 = MemberName | InputMethod | InputArgument | InputArgument2,
        InputMethod_TargetValue = InputMethod | TargetValue,
        InputMethod_TargetValue_InputArgument = InputMethod | TargetValue | InputArgument,
        MemberName_InputMethod_TargetValue = MemberName | InputMethod | TargetValue,
        MemberName_InputMethod_TargetValue_InputArgument = MemberName | InputMethod | TargetValue | InputArgument,
        MemberName_InputMethod_InputMethod2_TargetValue_InputArgument = MemberName | InputMethod | InputMethod2 | TargetValue | InputArgument,
        MemberName_InputMethod_InputMethod2_InputArgument_InputArgument2 = MemberName | InputMethod | InputMethod2 | InputArgument | InputArgument2,
        MemberName_InputMethod_TargetValue_InputArgument_InputArgument2 = MemberName | InputMethod | TargetValue | InputArgument | InputArgument2,
        InputMethod_InputMethod2_TargetValue_InputArgument = InputMethod | InputMethod2 | TargetValue | InputArgument,
        InputMethod_InputMethod2_InputArgument_InputArgument2 = InputMethod | InputMethod2 | InputArgument | InputArgument2,
        MemberName_InputMethod = MemberName | InputMethod,
        MemberName_InputMethod_InputMethod2_TargetValue_InputArgument_InputArgument2 = MemberName | InputMethod | InputMethod2 | TargetValue | InputArgument | InputArgument2,
        InputMethod_InputMethod2_TargetValue_InputArgument_InputArgument2 = InputMethod | InputMethod2 | TargetValue | InputArgument | InputArgument2,
        InputMethod_InputMethod2_TargetValue = InputMethod | InputMethod2 | TargetValue,
        MemberName_TargetValue = MemberName | TargetValue,
        InputMethod_InputMethod2_InputArgument = InputMethod | InputMethod2 | InputArgument
    }
}
