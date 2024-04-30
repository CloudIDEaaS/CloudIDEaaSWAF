using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using static alglib;

namespace Utils.SqlParser
{
    public class SqlParserVisitor : TSqlFragmentVisitor, IDisposable
    {
        private DatabaseFacade database;
        private Func<string, string, string> getDataTypeOfEntityProperty;
        private string providerName;
        private Stack<StringBuilder> resultStack;
        private Stack<string> tableNameStack;
        private Stack<string> functionResultStack;
        private StringBuilder builder;

        public SqlParserVisitor(DatabaseFacade database, Func<string, string, string> getDataTypeOfEntityProperty)
        {
            this.database = database;
            this.getDataTypeOfEntityProperty = getDataTypeOfEntityProperty;

            try
            {
                this.providerName = database.ProviderName;

                Debug.Assert(this.providerName == "Npgsql.EntityFrameworkCore.PostgreSQL", "Only supports PostgresSQL");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private string InternalResult
        {
            get
            {
                return this.Builder.ToString();
            }
        }

        public string Result
        {
            get
            {
                return this.InternalResult;
            }
        }

        private StringBuilder Builder
        {
            get
            {
                if (builder == null)
                {
                    builder = new StringBuilder();
                }
                else
                {
                    if (resultStack == null)
                    {
                        resultStack = new Stack<StringBuilder>();
                    }
                    else if (resultStack.Count > 0)
                    {
                        var resultBuilder = resultStack.Peek();

                        return resultBuilder;
                    }
                }

                return builder;
            }
        }

        private IDisposable Push()
        {
            resultStack.Push(new StringBuilder());

            return this.CreateDisposable(() =>
            {
                resultStack.Pop();
            });
        }

        public override void Visit(TSqlFragment node)
        {
            switch (node)
            {
                case TSqlScript script:

                    foreach (var batch in script.Batches)
                    {
                        foreach (var statement in batch.Statements)
                        {
                            statement.Accept(this);
                        }
                    }

                    break;
                default:
                    node.AcceptChildren(this);
                    break;
            }
        }

        public override void ExplicitVisit(NamedTableReference node)
        {
            this.Builder.AppendWithLeadingIfLength(" ", node.SchemaObject.Identifiers.Select(i => i.Value.SurroundWithQuotes()).ToCommaDelimitedList());

            node.AcceptChildren(this);
        }

        public override void ExplicitVisit(FunctionCall node)
        {
            node.AcceptChildren(this);

            switch (node.FunctionName.Value.ToUpper())
            {
                case "DATEDIFF":
                    {
                        var parameters = node.Parameters;
                        var interval = parameters[0];
                        var startDate = parameters[1];
                        var endDate = parameters[2];
                        var intervalText = string.Empty;
                        var startDateResult = string.Empty;
                        var endDateResult = string.Empty;

                        if (functionResultStack == null)
                        {
                            functionResultStack = new Stack<string>();
                        }

                        functionResultStack.Push("int");

                        switch (interval)
                        {
                            case ColumnReferenceExpression columnReferenceExpression:
                                {
                                    var identifiers = columnReferenceExpression.MultiPartIdentifier.Identifiers;

                                    foreach (var identifier in identifiers)
                                    {
                                        switch (identifier.Value)
                                        {
                                            case "minute":
                                            case "mi":
                                                intervalText = "minute";
                                                DebugUtils.AssertThrow(identifiers.Count == 1, "Unexpected Sql identifier");

                                                break;
                                            default:
                                                DebugUtils.Break();
                                                break;
                                        }
                                    }
                                }
                                break;
                            default:
                                DebugUtils.Break();
                                break;
                        }

                        startDateResult = GetDateFromScalar(startDate);
                        endDateResult = GetDateFromScalar(endDate);

                        switch (intervalText)
                        {
                            case "minute":
                                {
                                    var daysDiff = GetDaysDiff(startDateResult, endDateResult);
                                    var hoursDiff = GetHoursDiff(daysDiff, startDateResult, endDateResult);
                                    var minutesDiff = GetMinutesDiff(hoursDiff, startDateResult, endDateResult);

                                    builder.AppendLineWithLeadingIfLength(" ", minutesDiff);
                                }
                                break;
                            default:
                                DebugUtils.Break();
                                break;
                        }
                    }
                    break;
                default:
                    DebugUtils.Break();
                    break;
            }
        }

        private string GetDateFromScalar(ScalarExpression date)
        {
            string dateResult = null;

            switch (date)
            {
                case ColumnReferenceExpression columnReferenceExpression:
                    {
                        var identifiers = columnReferenceExpression.MultiPartIdentifier.Identifiers;
                        Identifier identifier;

                        DebugUtils.AssertThrow(identifiers.Count == 1, "Unexpected Sql identifier");

                        identifier = identifiers[0];

                        using (this.Push())
                        {
                            this.ExplicitVisit(identifiers[0]);

                            switch (this.InternalResult)
                            {
                                case "":
                                    Debug.Assert(identifier.QuoteType == QuoteType.NotQuoted);
                                    dateResult = identifier.Value.SurroundWithQuotes();
                                    break;
                                default:
                                    DebugUtils.Break();
                                    break;
                            }
                        }
                    }
                    break;
                case StringLiteral stringLiteral:
                    {
                        dateResult = DateTime.Parse(stringLiteral.Value).ToString("yyyy-MM-dd HH:mm:ss").SurroundWithSingleQuotes();
                    }
                    break;
                default:
                    DebugUtils.Break();
                    break;
            }

            return dateResult;
        }

        private string GetMinutesDiff(string hoursDiff, string startDateResult, string endDateResult)
        {
            return $"({ hoursDiff }) * 60 + DATE_PART('minute', { endDateResult } - { startDateResult })";
        }

        private string GetHoursDiff(string daysDiff, string startDateResult, string endDateResult)
        {
            return $"({ daysDiff }) * 24 + DATE_PART('hour', { endDateResult } - { startDateResult } )";
        }

        private string GetDaysDiff(string startDateResult, string endDateResult)
        {
            return $"DATE_PART('day', { endDateResult } - { startDateResult })";
        }

        public override void ExplicitVisit(UpdateStatement node)
        {
            this.Builder.Append("UPDATE");

            node.AcceptChildren(this);
        }

        public override void ExplicitVisit(UpdateSpecification node)
        {
            var target = (NamedTableReference) node.Target;
            var tableName = target.SchemaObject.Identifiers.Single().Value;
            string setClauseText;

            if (tableNameStack == null)
            {
                tableNameStack = new Stack<string>();
            }

            tableNameStack.Push(tableName);

            this.Builder.Append($" { tableName.SurroundWithQuotes() } ");

            using (this.Push())
            {
                this.Builder.Append("SET ");

                foreach (var setClause in node.SetClauses)
                {
                    setClause.Accept(this);

                    this.Builder.AppendIfLength(", ");
                }

                setClauseText = this.Builder.ToString().RemoveEndIfMatches(", ");
            }

            builder.Append($" { setClauseText } ");

            if (node.FromClause != null)
            {
                node.FromClause.Accept(this);
            }

            if (node.WhereClause != null)
            {
                node.WhereClause.Accept(this);
            }
        }

        public override void ExplicitVisit(AssignmentSetClause node)
        {
            var column = node.Column.MultiPartIdentifier.Identifiers.Single().Value;
            var value = node.NewValue;
            string type = null;

            this.Builder.AppendWithLeadingIfLength(" ", column.SurroundWithQuotes());

            switch (node.AssignmentKind)
            {
                case AssignmentKind.Equals:
                    this.Builder.Append(" = ");
                    break;
                default:
                    DebugUtils.Break();
                    break;
            }

            switch (value)
            {
                case IntegerLiteral integerLiteral:

                    type = getDataTypeOfEntityProperty(tableNameStack.Peek(), column);

                    switch (type)
                    {
                        case "bool":
                            this.Builder.Append($" { (integerLiteral.Value == "1") } ");
                            break;
                        default:
                            this.Builder.Append($" {integerLiteral.Value} ");
                            break;
                    }

                    break;
                case StringLiteral stringLiteral:

                    var stringValue = stringLiteral.Value;
                   
                    type = getDataTypeOfEntityProperty(tableNameStack.Peek(), column);

                    switch (type)
                    {
                        case "DateTime":
                            var dateValue = DateTime.Parse(stringValue).ToString("yyyy-MM-dd HH:mm:ss").SurroundWithSingleQuotes();
                            this.Builder.Append($" {dateValue} ");
                            break;
                        default:
                            this.Builder.Append($" {stringValue.SurroundWithQuotes()} ");
                            break;
                    }

                    break;
                default:
                    break;
            }

            node.AcceptChildren(this);
        }

        public override void ExplicitVisit(WhereClause node)
        {
            this.Builder.AppendWithLeadingIfLength(" ", "WHERE");

            node.AcceptChildren(this);
        }

        public override void ExplicitVisit(BooleanBinaryExpression node)
        {
            var firstExpression = node.FirstExpression;

            switch (firstExpression)
            {
                case BooleanIsNullExpression booleanIsNullExpression:

                    var columnReferenceExpression = (ColumnReferenceExpression)booleanIsNullExpression.Expression;
                    var identifiers = columnReferenceExpression.MultiPartIdentifier.Identifiers;
                    Identifier identifier;
                    string column;

                    DebugUtils.AssertThrow(identifiers.Count == 1, "Unexpected Sql identifier");

                    identifier = identifiers[0];

                    Debug.Assert(identifier.QuoteType == QuoteType.NotQuoted);

                    column = identifier.Value;

                    this.Builder.AppendWithLeadingIfLength(" ", column.SurroundWithQuotes());

                    if (booleanIsNullExpression.IsNot)
                    {
                        this.Builder.Append(" IS NOT NULL ");
                    }
                    else
                    {
                        this.Builder.Append(" IS NULL ");
                    }

                    break;
                default:
                    DebugUtils.Break();
                    break;
            }

            switch (node.BinaryExpressionType)
            {
                case BooleanBinaryExpressionType.And:
                    this.Builder.Append(" AND ");
                    break;

                default:
                    DebugUtils.Break();
                    break;
            }

            node.AcceptChildren(this);
        }

        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            var firstExpression = node.FirstExpression;
            var secondExpression = node.SecondExpression;

            switch (firstExpression)
            {
                case FunctionCall functionCall:
                    this.ExplicitVisit(functionCall);
                    break;
                default:
                    DebugUtils.Break();
                    break;
            }

            switch (node.ComparisonType)
            {
                case BooleanComparisonType.GreaterThan:
                    this.Builder.Append(" > ");
                    break;
                default:
                    DebugUtils.Break();
                    break;
            }

            switch (secondExpression)
            {
                case IntegerLiteral integerLiteral:

                    string dataType = null;

                    if (functionResultStack.Count > 0)
                    {
                        dataType = functionResultStack.Pop();
                    }

                    switch (dataType)
                    {
                        case "int":
                            this.Builder.Append($" {integerLiteral.Value}");
                            break;
                        default:
                            DebugUtils.Break();
                            break;
                    }

                    break;
                default:
                    DebugUtils.Break();
                    break;
            }
        }

        public void Dispose()
        {
            tableNameStack.Clear();
        }
    }
}
