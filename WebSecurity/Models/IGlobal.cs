using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models;

public interface IGlobal : IStorageBase
{
    [OwaspName("crs_setup_version")]
    string? CrsSetupVersion { get; set; }
    [OwaspName("ENABLE_DEFAULT_COLLECTIONS")]
    bool? EnableDefaultCollections { get; set; }
    [OwaspName("MAX_FILE_SIZE")]
    long? MaxFileSize { get; set; }
    [OwaspName("MAX_NUM_ARGS")]
    int? MaxNumArgs { get; set; }
    [OwaspName("TOTAL_ARG_LENGTH")]
    int? TotalArgLength { get; set; }
    bool PartialMatchFromFile(string filename, string searchText);
    IDisposable IndexFile(string filename);
}
