using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.Models;

namespace WAFWebSample.Data
{
    public class WAFGlobalSettings
    {
        [Key]
        public Guid WAFGlobalSettingsId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? AllowedRequestContentTypes { get; set; }
        public string? AllowedRequestContentTypeCharsets { get; set; }
        public string? CrsSetupVersion { get; set; }
        public long? MaxFileSize { get; set; }
        public int? MaxNumArgs { get; set; }
        public int? TotalArgLength { get; set; }
        public bool? EnableDefaultCollections { get; set; }
    }
}
