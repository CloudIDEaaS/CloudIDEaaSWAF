using WebSecurity.StartupTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.StartupTests.Extensions
{
    public static class RuleExtensions
    {
        public static string BuildCommaSeparated(this List<RuleUniqueHash> rulesWithUniqueHash, RuleUniquenessKind kind, List<string>? propertyFilters = null, Func<Rule?, List<KeyValuePair<string, object>>>? addColumnValues = null)
        {
            var builder = new StringBuilder();
            var nameBuilder = new StringBuilder();

            if (propertyFilters == null)
            {
                propertyFilters = typeof(Rule).GetPublicPropertyNames().ToList();

                if (addColumnValues != null)
                {
                    var columns = addColumnValues(null!).Select(c => c.Key).ToList();

                    propertyFilters = propertyFilters.Concat(columns).ToList();
                }
            }
            else
            {
                propertyFilters = propertyFilters.Prepend("Id").ToList();

                if (addColumnValues != null)
                {
                    var columns = addColumnValues(null!).Select(c => c.Key).ToList();

                    propertyFilters = propertyFilters.Concat(columns).ToList();
                }
            }

            builder.AppendLine(kind.ToString() + ":");

            foreach (var propertyName in propertyFilters)
            {
                nameBuilder.AppendWithLeadingIfLength("\t", "{0}",  propertyName);
            }

            builder.AppendLine(nameBuilder.ToString());

            foreach (var rule in rulesWithUniqueHash.Select(r => r.Rule))
            {
                var values = rule.GetPublicPropertyValues().Where(p => p.Key.IsOneOf(propertyFilters)).OrderBy(p => propertyFilters.IndexOf(p.Key)).Select(p => p.Value);
                var valueBuilder = new StringBuilder();

                foreach (var value in values)
                {
                    valueBuilder.AppendWithLeadingIfLength("\t", "{0}", value.AsDisplayText().Crop(50, true).ToSingleLine().SurroundWithQuotesIfNeeded());
                }

                if (addColumnValues != null)
                {
                    var columnValues = addColumnValues(rule).Select(c => c.Value).ToList();

                    foreach (var value in columnValues)
                    {
                        valueBuilder.AppendWithLeadingIfLength("\t", "{0}", value.AsDisplayText().ToSingleLine().SurroundWithQuotesIfNeeded());
                    }
                }

                builder.AppendLine(valueBuilder.ToString());
            }

            return builder.ToString();
        }
    }
}
