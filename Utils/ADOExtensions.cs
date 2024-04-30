using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if USES_EF_CORE
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#elif USE_SYSTEM_DATA
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data.Objects.DataClasses;
#endif
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BTreeIndex.Collections.Generic.BTree;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Runtime.Intrinsics.Arm;
using DocumentFormat.OpenXml.Bibliography;
using Utils.SqlParser;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Configuration.Provider;
using System.Data.Common;

namespace Utils
{
    public static class ADOExtensions
    {
        private static BTreeDictionary<string, string> sqlIndex;
#if USES_EF_CORE
        public static void AddFromTSV<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, string tabSeparatedValuesPath, Func<string> getFirstColumnValue = null, Action<TEntity> validate = null) where TEntity : class
        {
            var entityParserHost = new TsvEntityParserHost<TEntity>();
            var entities = entityParserHost.ParseFile(tabSeparatedValuesPath, null, getFirstColumnValue);

            if (validate != null)
            {
                entities.ForEach(e => validate(e));
            }

            entityTypeBuilder.HasData(entities);
        }

        public static void AddFromTSV<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, string tabSeparatedValuesPath, EntityTypeConfigRelationshipManager relationshipManager, Action<TEntity> validate = null) where TEntity : class
        {
            var entityParserHost = new TsvEntityParserHost<TEntity>();
            var entities = entityParserHost.ParseFile(tabSeparatedValuesPath, null, relationshipManager);

            if (validate != null)
            {
                entities.ForEach(e => validate(e));
            }

            entityTypeBuilder.HasData(entities);
        }

        public static string GetAttachDbFilename(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).AttachDBFilename;
        }

        public static string Specialize(this DatabaseFacade database, FormattableString sqlString, Func<string, string, string> getDataTypeOfEntityProperty, bool cachResults = false)
        {
            string sql;
            string returnSql;

            foreach (var argument in sqlString.GetArguments()) 
            {
                var argString = argument.ToString();
                IList<ParseError> errors;

                using (var reader = new StringReader(argString))
                {
                    var parser = new TSql150Parser(true, SqlEngineType.All);
                    var tree = parser.Parse(reader, out errors);

                    if (errors.Count == 0)
                    {
                        throw new ArgumentException("Sql injection violation");
                    }
                }
            }

            sql = sqlString.ToString();

            if (cachResults)
            {
                if (sqlIndex == null)
                {
                    sqlIndex = new BTreeDictionary<string, string>();
                }
                else
                {
                    if (sqlIndex.ContainsKey(sql))
                    {
                        returnSql = sqlIndex[sql];

                        return returnSql;
                    }
                }
            }

            using (var reader = new StringReader(sql))
            {
                IList<ParseError> errors;
                var parser = new TSql150Parser(true, SqlEngineType.All);
                var tree = parser.Parse(reader, out errors);

                using (var visitor = new SqlParserVisitor(database, getDataTypeOfEntityProperty))
                {
                    var test = false;

                    if (test)
                    {
                        var tokenTypes = tree.ScriptTokenStream.Select(s => new KeyValuePair<TSqlTokenType, string>(s.TokenType, s.Text.SurroundWithQuotes())).ToList();
                    }

                    visitor.Visit(tree);

                    returnSql = visitor.Result;

                    if (cachResults)
                    {
                        sqlIndex.Add(sql, returnSql);
                    }
                }
            }

            return returnSql;
        }

        public static string GetDatabaseName(this DbContext dbContext)
        {
            var database = dbContext.Database;
            var connectionString = database.GetConnectionString();
            var providerName = database.ProviderName;

            if (providerName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);

                return builder.Database;
            }

            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        public static string GetHost(this DbContext dbContext)
        {
            var database = dbContext.Database;
            var connectionString = database.GetConnectionString();
            var providerName = database.ProviderName;

            if (providerName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);

                return builder.Host;
            }

            return new SqlConnectionStringBuilder(connectionString).DataSource;
        }

        public static int GetPort(this DbContext dbContext)
        {
            var database = dbContext.Database;
            var connectionString = database.GetConnectionString();
            var providerName = database.ProviderName;

            if (providerName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);

                return builder.Port;
            }

            return DebugUtils.BreakReturn<int>(-1);
        }

        public static string GetDatabaseName(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        //public static void TruncateAll(this DbContext dbContext)
        //{
        //    SqlCommand cmd;
        //    var builder = new StringBuilder();
        //    var entityType = dbContext.GetType();
        //    var properties = entityType.GetProperties().Where(p => p.PropertyType.Name.StartsWith("ObjectSet"));
        //    var database = dbContext.GetDatabaseName();
        //    var entityConnection = (EntityConnection)entities.Connection;
        //    var connection = new SqlConnection(entityConnection.StoreConnection.ConnectionString);

        //    connection.Open();

        //    cmd = connection.CreateCommand();

        //    foreach (var property in properties)
        //    {
        //        builder.AppendLineFormat("delete {0}.dbo.{1}", database, property.Name.Singularize());
        //    }

        //    cmd.CommandText = builder.ToString();

        //    cmd.ExecuteNonQuery();

        //    connection.Close();
        //}

        //public static void Truncate(this ObjectContext entities, params string[] tables)
        //{
        //    SqlCommand cmd;
        //    var builder = new StringBuilder();
        //    var entityType = entities.GetType();
        //    var database = entities.GetDatabaseName();
        //    var entityConnection = (EntityConnection)entities.Connection;
        //    var connection = new SqlConnection(entityConnection.StoreConnection.ConnectionString);

        //    connection.Open();

        //    cmd = connection.CreateCommand();

        //    foreach (var table in tables)
        //    {
        //        builder.AppendLineFormat("delete {0}.dbo.{1}", database, table);
        //    }

        //    cmd.CommandText = builder.ToString();

        //    cmd.ExecuteNonQuery();

        //    connection.Close();
        //}

        //public static T SaveIfNotExists<T>(this ObjectContext entities, Func<T, bool> query, Func<T> createFunc) where T : EntityObject
        //{
        //    var entityType = entities.GetType();
        //    var objectSet = (IEnumerable<T>)entityType.GetProperties().Single(p => p.PropertyType.Name.StartsWith("ObjectSet") && p.PropertyType.GetGenericArguments().First() == typeof(T)).GetValue(entities, null);
        //    var entity = objectSet.SingleOrDefault(e => query(e));

        //    if (entity == null)
        //    {
        //        entity = createFunc();
        //        ((dynamic)objectSet).AddObject(entity);

        //        entities.SaveChanges();
        //    }

        //    return entity;
        //}

#elif USE_SYSTEM_DATA
        public static string GetDatabaseName(this ObjectContext entities)
        {
            var connection = (EntityConnection) entities.Connection;

            return new SqlConnectionStringBuilder(connection.StoreConnection.ConnectionString).InitialCatalog;
        }

        public static string GetDatabaseName(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        public static void TruncateAll(this ObjectContext entities)
        {
            SqlCommand cmd;
            var builder = new StringBuilder();
            var entityType = entities.GetType();
            var properties = entityType.GetProperties().Where(p => p.PropertyType.Name.StartsWith("ObjectSet"));
            var database = entities.GetDatabaseName();
            var entityConnection = (EntityConnection)entities.Connection;
            var connection = new SqlConnection(entityConnection.StoreConnection.ConnectionString);

            connection.Open();

            cmd = connection.CreateCommand();

            foreach (var property in properties)
            {
                builder.AppendLineFormat("delete {0}.dbo.{1}", database, property.Name.Singularize());
            }

            cmd.CommandText = builder.ToString();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static void Truncate(this ObjectContext entities, params string[] tables)
        {
            SqlCommand cmd;
            var builder = new StringBuilder();
            var entityType = entities.GetType();
            var database = entities.GetDatabaseName();
            var entityConnection = (EntityConnection)entities.Connection;
            var connection = new SqlConnection(entityConnection.StoreConnection.ConnectionString);

            connection.Open();

            cmd = connection.CreateCommand();

            foreach (var table in tables)
            {
                builder.AppendLineFormat("delete {0}.dbo.{1}", database, table);
            }

            cmd.CommandText = builder.ToString();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static T SaveIfNotExists<T>(this ObjectContext entities, Func<T, bool> query, Func<T> createFunc) where T : EntityObject
        {
            var entityType = entities.GetType();
            var objectSet = (IEnumerable<T>)entityType.GetProperties().Single(p => p.PropertyType.Name.StartsWith("ObjectSet") && p.PropertyType.GetGenericArguments().First() == typeof(T)).GetValue(entities, null);
            var entity = objectSet.SingleOrDefault(e => query(e));

            if (entity == null)
            {
                entity = createFunc();
                ((dynamic)objectSet).AddObject(entity);

                entities.SaveChanges();
            }

            return entity;
        }

#endif
    }
}
