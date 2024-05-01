using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.StartupTests
{
    public static class InProcessTestRunner
    {
        public static void RunTests(params KeyValuePair<Enum, object>[] args)
        {
            var argsList = new List<string>();

            foreach (var pair in args)
            {
                var enumSwitch = pair.Key.ToString();
                var switchText = "/" + typeof(Switches).GetStaticFieldValue<string>(enumSwitch);

                if (switchText != "/")
                {
                    argsList.Add(switchText);
                }
                
                if (pair.Value != null)
                {
                    var value = pair.Value.ToString();

                    if (value.RegexIsMatch(@"\s+"))
                    {
                        value = value.SurroundWithQuotesIfNeeded();
                    }

                    if (switchText != "/")
                    {
                        argsList.Add(":" + value);
                    }
                    else
                    {
                        argsList.Add("/" + value);
                    }
                }
            }

            argsList.Add("/" + Switches.IN_PROCESS);

            Program.Main(argsList.ToArray());
        }
    }
}
