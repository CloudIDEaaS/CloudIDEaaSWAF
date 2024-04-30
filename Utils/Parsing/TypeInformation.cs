using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Parsing
{
    [ReadOnly(true)]
    public class TypeInformation
    {
        readonly List<TypeInformation> innerTypes = new List<TypeInformation>();
        public string TypeSymbol { get; set; }
        public string TypeName { get; set; }
        public bool IsArray { get; set; }
        public string NamespaceName { get; internal set; }
        public string MethodName { get; internal set; }
        public string ReturnType { get; internal set; }
        public string Modifiers { get; internal set; }
        public bool IsDestructor { get; internal set; }
        public bool IsConstructor { get; internal set; }
        public string InterfaceType { get; internal set; }
        public string MemberName { get; internal set; }
        public string Lambda { get; internal set; }
        public bool IsGuidAddress { get; internal set; }
        public string GuidName { get; internal set; }
        public bool IsVTableConst { get; internal set; }
        [Browsable(true)]
        public SpecialFunction SpecialFunction { get; set; }

        public TypeInformation()
        {
        }

        public bool IsGeneric
        {
            get
            {
                return innerTypes.Count > 0;
            }
        }

        [Browsable(true)]
        public List<TypeInformation> InnerTypes
        {
            get
            {
                return innerTypes;
            }
        }

        public string FullTypeName
        {
            get
            {
                var builder = new StringBuilder();
                Action<TypeInformation> recurse = null;

                recurse = (t) =>
                {
                    var innerTypes = t.InnerTypes;

                    builder.Append(t.TypeName);

                    if (innerTypes.Count > 0)
                    {
                        var firstType = true;

                        builder.Append("<");

                        foreach (var innerType in innerTypes)
                        {
                            if (!firstType)
                            {
                                builder.Append(", ");
                                recurse(innerType);
                            }
                            else
                            {
                                recurse(innerType);
                            }


                            firstType = false;
                        }

                        builder.Append(">");
                    }
                };

                recurse(this);

                return builder.ToString();
            }
        }

        public string GetTypeMemberName(uint id)
        {
            string name;

            if (this.FullTypeName.IsNullOrEmpty())
            {
                name = string.Format("{0} [{1}]", this.MemberName, id);
            }
            else if (this.MemberName.IsNullOrEmpty())
            {
                name = string.Format("{0} [{1}]", this.FullTypeName, id);
            }
            else
            {
                name = string.Format("{0}::{1} [{2}]", this.FullTypeName, this.MemberName.AsDisplayText(), id);
            }

            return name;
        }

        public override string ToString()
        {
            // Create a string builder with the type name prefilled.
            var sb = new StringBuilder(this.TypeName);

            // If this type is generic (has inner types), append each recursively.
            if (this.IsGeneric)
            {
                sb.Append("<");

                // Get the number of inner types.
                int innerTypeCount = this.InnerTypes.Count();

                // Append each inner type's friendly string recursively.
                for (int i = 0; i < innerTypeCount; i++)
                {
                    sb.Append(innerTypes[i].ToString());

                    // Check if we need to add a comma to separate from the next inner type name.
                    if (i + 1 < innerTypeCount)
                        sb.Append(", ");
                }

                sb.Append(">");
            }

            // If this type is an array, we append the array '[]' marker.
            if (this.IsArray)
                sb.Append("[]");

            return sb.ToString();
        }
    }
}
