using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.WindowsSearch.Interfaces;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace Utils.WindowsSearch
{
    public class SearchManager : IDisposable
    {
        private List<SearchCatalog> catalogs;

        public SearchManager()
        {
            catalogs = new List<SearchCatalog>();
        }

        public void AddCatalog(string catalogName)
        {
            catalogs.Add(new SearchCatalog(catalogName));
        }

        public IEnumerable<string> Search(string searchText)
        {
            var fileList = new List<string>();

            foreach (var catalog in catalogs)
            {
                var results = catalog.Search(searchText);

                fileList.AddRange(results);
            }

            return fileList;
        }

        public void Dispose()
        {
        }
    }
}
