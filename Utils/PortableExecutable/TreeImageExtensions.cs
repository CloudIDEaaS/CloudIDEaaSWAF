using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Utils.PortableExecutable
{
    public static class TreeImageExtensions
    {
        public static string GetTreeImage(this object obj)
        {
            //////var type = obj.GetType();

            //////if (type.HasCustomAttribute<TreeImageAttribute>())
            //////{
            //////    var attr = type.GetCustomAttribute<TreeImageAttribute>();
            //////    var attrImage = attr.Image;
            //////    var builder = new StringBuilder();
            //////    var regex = new Regex("{[^}]*}");
            //////    var project = ICsProjectFactory.create(project_namespace.pn_project_namespace);
            //////    var debug = false;

            //////    if (regex.IsMatch(attrImage))
            //////    {
            //////        var matches = regex.Matches(attrImage);
            //////        var x = 0;
            //////        var index = 0;
            //////        string rightString = null;
            //////        var matchStrings = new List<string>();
            //////        BracedAttributeArgumentEvaluator evaluator;

            //////        if (debug)
            //////        {
            //////            var path = Environment.ExpandEnvironmentVariables(@"%HYDRASOLUTIONPATH%\TestEval\BracedAttributeArgumentEvaluator.dll");

            //////            evaluator = new BracedAttributeArgumentEvaluator(obj.GetType(), path);
            //////        }
            //////        else
            //////        {
            //////            evaluator = new BracedAttributeArgumentEvaluator(obj.GetType());
            //////        }

            //////        foreach (Match match in matches)
            //////        {
            //////            var leftString = attrImage.Substring(index, match.Index - index);
            //////            var matchString = match.Value;
            //////            var endOfString = match.Index + matchString.Length;

            //////            rightString = attrImage.Substring(endOfString, attrImage.Length - endOfString);

            //////            builder.Append(leftString);
            //////            builder.AppendFormat("{{{0}}}", x);

            //////            matchStrings.Add(matchString);

            //////            index = match.Index + match.Length;
            //////            x++;
            //////        }

            //////        if (rightString != null)
            //////        {
            //////            builder.Append(rightString);
            //////        }

            //////        evaluator.ProcessFormat(builder.ToString());

            //////        for (var y = 0; y < matchStrings.Count; y++)
            //////        {
            //////            var matchString = matchStrings[y];
            //////            var snippet = ICsSnippetFactory.create(matchString.ToCharArray(), null);
            //////            CsNode rootNode;

            //////            try
            //////            {
            //////                project.parseSnippet(snippet, CsExpectedSnippet.cses_statement, null, true);
            //////            }
            //////            catch
            //////            {
            //////                Debugger.Break();
            //////            }

            //////            rootNode = snippet.getNodes()[0];

            //////            evaluator.ProcessArg(rootNode, matchString);
            //////        }

            //////        evaluator.PostProcess(x);

            //////        try
            //////        {
            //////            if (debug)
            //////            {
            //////                evaluator.Save();
            //////            }

            //////            return evaluator.Evaluate(obj);
            //////        }
            //////        catch (Exception ex)
            //////        {
            //////            Debugger.Break();
            //////        }
            //////    }
            //////}

            //////return obj.ToString();
            ///

            throw new NotImplementedException();
        }
    }
}
