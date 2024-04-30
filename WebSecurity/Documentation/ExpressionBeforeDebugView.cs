using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Documentation
{
    public class ExpressionDebugView
    {
        public void Failed()
        {
            .Try {
                .Block() {
                    .Call(.Try {
                        .Block() {
                            $var1.Path
                        }
                    } .Catch(System.NullReferenceException) {
                        .Default(System.String)
                                }).EndsWith(".php")
                }
            } .Catch(System.NullReferenceException) {
                False
                        } || .Call System.Text.RegularExpressions.Regex.IsMatch(
                .Try {
                    .Block() {
                        $var1.UserAgent
                    }
            } .Catch(System.NullReferenceException) {
                    .Default(System.String)
                        },
                "^(curl|java|python)",
                .Constant<System.Text.RegularExpressions.RegexOptions>(IgnoreCase)) || .Call((System.Object)False).Equals((System.Object).Try {
                    .Block() {
                        $var1.HasCrsSetupVersion
                    }
            } .Catch(System.NullReferenceException) {
                    .Default(System.Boolean)
                        }) || .Call.Constant<System.Collections.Generic.List`1[System.Object]>(System.Collections.Generic.List`1[System.Object]).Contains(.Try {
                    .Block() {
                        $var1.IpCountry
                    }
            } .Catch(System.NullReferenceException) {
                    .Default(System.String)
                        })
                    }
        }
    }
}
