using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utils.ConditionPoints
{
    public enum ConditionPointDependencyKind
    {
        CountSource,
        TypeSource,
        InsertBetweenSource,
        ActionPoint
    }

    public class ConditionPointDependency
    {
        public IEnumerable<FieldInfo> EnumFields { get; }
        public Attribute Attribute { get; }
        public Type DependencyType { get; }
        public ConditionPointBase ConditionPointInstance { get; }
        public MemberInfo RelatedMember { get; }
        public ConditionPointDependencyKind Kind { get; }

        public ConditionPointDependency(Attribute attribute, Type dependencyType, MemberInfo relatedMember, ConditionPointDependencyKind kind) : this(attribute, dependencyType, kind)
        {
            RelatedMember = relatedMember;
        }

        public ConditionPointDependency(Attribute attribute, Type dependencyType, ConditionPointDependencyKind kind)
        {
            this.DependencyType = dependencyType;
            this.Kind = kind;

            if (dependencyType.IsClass && dependencyType.InheritsFrom<ConditionPointBase>() && dependencyType.GetConstructors().Any(c => c.GetParameters().Length == 0))
            {
                this.ConditionPointInstance = (ConditionPointBase) Activator.CreateInstance(dependencyType);
            }
        }

        public ConditionPointDependency(Type dependencyType, ConditionPointDependencyKind kind, IEnumerable<FieldInfo> enumFields) : this(null, dependencyType, kind)
        {
            this.EnumFields = enumFields;
        }

        public Array EnumValues
        {
            get
            {
                if (this.EnumFields != null)
                {
                    return Enum.GetValues(this.DependencyType);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
