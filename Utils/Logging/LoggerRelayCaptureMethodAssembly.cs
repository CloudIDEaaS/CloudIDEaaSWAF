using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Utils
{
    [DataContract]
    public class LoggerRelayCaptureMethodAssembly
    {
        public string Assembly { get; set; }
        public bool Enabled { get; set; }
        public List<LoggerRelayCaptureMethodType> Types { get; set; }
        private Dictionary<string, LoggerRelayCaptureMethodType> typesLookup;

        public Dictionary<string, LoggerRelayCaptureMethodType> TypesLookup
        {
            get
            {
                if (typesLookup == null)
                {
                    typesLookup = this.Types.Where(t => t.Enabled).ToDictionary(t => t.Type, t => t);
                }

                return typesLookup;
            }
        }
    }
}
