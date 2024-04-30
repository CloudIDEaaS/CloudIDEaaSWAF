using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace Utils.WindowsSearch
{
    public class SearchCatalog
    {
        private OleDbConnection connection;

        public SearchCatalog(string catalogName)
        {
            var connectionString = string.Format("Provider= \"MSIDXS\";Data Source=\"{0}\";", catalogName);

            connection = new OleDbConnection(connectionString);
            connection.Open();
        }

        public IEnumerable<string> Search(string searchText)
        {
            var query = string.Format(@"SELECT Path FROM scope() " + @"WHERE FREETEXT(Contents, '{0}')", searchText);
            var command = new OleDbCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return reader.GetString(0);
            }

            connection.Close();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
