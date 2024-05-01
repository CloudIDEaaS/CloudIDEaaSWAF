using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.StartupTests.Models
{
    [Serializable]
    public class IPAddressCity : ICloneable<IPAddressCity>
    {
        public string IpAddress { get; set; }
        public string? City { get; set; }
        public float Weight { get; set; }

        public IPAddressCity()
        {
        }

        public IPAddressCity(IPAddress ipAddress, string? city)
        {
            IpAddress = ipAddress.ToString();
            City = city;
        }

        public IPAddressCity ShallowClone()
        {
            return this.DefaultClone();
        }

        public IPAddressCity DeepClone()
        {
            return this.DefaultClone();
        }

        public IPAddressCity DefaultClone()
        {
            var ipAddress = IPAddress.Parse(this.IpAddress);
            var regex = new Regex(@"^(?<1>\d+?\.\d+?\.\d+?\.)(?<2>\d+?)$");
            
            if (regex.IsMatch(IpAddress))
            {
                var match = regex.Match(this.IpAddress);
                var firstPart = match.GetGroupValue("1");
                var lastPartInt = int.Parse(match.GetGroupValue("2"));

                lastPartInt = Math.Max(Math.Min(lastPartInt + NumberExtensions.GetRandomIntWithinRange(-50, 50), 255), 1);

                this.IpAddress = IPAddress.Parse(firstPart + lastPartInt.ToString()).ToString();
            }

            return new IPAddressCity
            {
                IpAddress = ipAddress.ToString(),
                City = this.City,
                Weight = this.Weight
            };
        }
    }
}
