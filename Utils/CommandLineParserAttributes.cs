using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utils
{
    public class CommandLineParserAttribute : Attribute
    {
        public Type ProgramAssemblyType { get; }
        public Type CommandType { get; }
        public Type SwitchType { get; } 
        public string Description { get; }
        public string[] Syntax { get; }
        public bool SupplyHelpSwitch { get; }
        public bool SupplyVersionSwitch { get; }

        public CommandLineParserAttribute(Type programAssemblyType, Type commandType, Type switchType, string description, bool supplyHelpSwitch = true, bool supplyVersionSwitch = true, params string[] syntax)
        {
            this.ProgramAssemblyType = programAssemblyType;
            this.CommandType = commandType;
            this.SwitchType = switchType;
            this.Description = description;
            this.SupplyHelpSwitch = supplyHelpSwitch;
            this.SupplyVersionSwitch = supplyVersionSwitch;
            this.Syntax = syntax;
        }
    }

    public class CommandLineSwitchAttribute : Attribute
    {
        public string Description { get; }
        public bool IsDefault { get; }
        public string SwitchShortcut { get; }
        public bool ShortcutOnly { get; }
        public int DescriptionLeftPaddingTabCount { get; }
        public string FallbackDescription { get; }

        public CommandLineSwitchAttribute(string description, bool isDefault = false, int descriptionLeftPaddingTabCount = 2, string switchShortcut = null, bool shortcutOnly = false, string fallbackDescription = null)
        {
            this.IsDefault = isDefault;
            this.Description = description;
            this.SwitchShortcut = switchShortcut;
            this.ShortcutOnly = shortcutOnly;
            this.DescriptionLeftPaddingTabCount = descriptionLeftPaddingTabCount;
            this.FallbackDescription = fallbackDescription;
        }
    }
}
