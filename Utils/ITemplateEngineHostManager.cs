using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Microsoft.VisualStudio.TextTemplating;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace Utils
{
    public interface ITemplateEngineHostManager
    {
        bool WriteFileMode { get; }
        string DebugAssistantAddress { get; }
        void ReportGenerate(Type generatorType, string templateFile);
    }
}
