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
    public enum PostProcessResult
    {
        None,
        Continue,
        RedoGenerate
    }

    public interface ITemplateEngineHost
    {
        PostProcessResult PostProcess();
        string Generate<T>(Dictionary<string, object> sessionVariables, bool throwException = false);
        string Generate(Type generatorType, Dictionary<string, object> sessionVariables, bool throwException = false);
    }
}
