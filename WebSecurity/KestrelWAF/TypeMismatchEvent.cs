using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF
{
    public delegate void TypeMismatchEventHandler(object sender, TypeMismatchEventArgs e);

    [Flags]
    public enum MismatchResolution
    {
        NotSet,
        ParseToInt = 1 << 1,
        ParseToLong = 1 << 2,
        GetLenth = 1 << 3,
        CastToLong = 1 << 4
    }

    public class TypeMismatchEventArgs
    {
        public Rule Rule { get; }
        public Expression Right { get; }
        public Expression Left { get; }
        public MismatchResolution LeftResolution { get; set; }
        public MismatchResolution RightResolution { get; set; }

        public TypeMismatchEventArgs(Rule rule, Expression right, Expression left)
        {
            this.Right = right;
            this.Left = left;
            this.Rule = rule;
        }
    }
}
