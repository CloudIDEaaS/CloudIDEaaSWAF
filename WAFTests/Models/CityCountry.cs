using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.StartupTests.Models
{
    public class CityCountry
    {
        public string City { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public string Definition { get; set; }
        public int Area { get; set; }
        public int Density { get; set; }

        public CityCountry(string record)
        {
            var fields = record.Split('\t').Select(f => f.RemoveQuotes().RemoveSpecialCharacters()).ToArray();

            City = fields[0];
            Country = fields[1];
            Population = int.Parse(fields[2].RemoveText(","));
            Definition = fields[3];
            Area = int.Parse(fields[4].RemoveText(","));
            Density = int.Parse(fields[5].RemoveText(","));
        }
    }
}
