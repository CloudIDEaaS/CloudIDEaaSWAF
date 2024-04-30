using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class NetworkExtensions
    {
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static string GetRandomIpAddress()
        {
            var random = new Random();

            return $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
        }

        public static bool IsIPv6(this IPAddress ipAddress)
        {
            return ipAddress.GetPrivatePropertyValue<bool>("IsIPv6");
        }

        public static IPAddress GetPublicIPV4(this IPAddress ipAddress)
        {
            if (IPAddress.IsLoopback(ipAddress))
            {
                var hostAddresses = Dns.GetHostAddresses(Dns.GetHostByAddress(ipAddress.ToString()).HostName);
                var ipAddresses = hostAddresses.Where(a => a.AddressFamily == AddressFamily.InterNetwork && a.IsIPv4() && a.IsPublic() && !IPAddress.IsLoopback(a));

                ipAddress = ipAddresses.MaxBy(a => a.Address);

                if (ipAddress == null)
                {
                    ipAddress = GetPublicIPV4();
                }
            }

            if (ipAddress.IsIPv6())
            {
                ipAddress = ipAddress.MapToIPv4();
            }

            return ipAddress;
        }

        public static IPAddress GetPublicIPV4()
        {
            var urlContent = GetUrlContentAsStringAsync("http://ipv4.icanhazip.com/");

            return ParseSingleIPv4Address(urlContent);
        }

        public static IPAddress ParseSingleIPv4Address(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input string must not be null", input);
            }

            var addressBytesSplit = input.Trim().Split('.').ToList();
            if (addressBytesSplit.Count != 4)
            {
                throw new ArgumentException("Input string was not in valid IPV4 format \"a.b.c.d\"", input);
            }

            var addressBytes = new byte[4];
            foreach (var i in Enumerable.Range(0, addressBytesSplit.Count))
            {
                if (!int.TryParse(addressBytesSplit[i], out var parsedInt))
                {
                    throw new FormatException($"Unable to parse integer from {addressBytesSplit[i]}");
                }

                if (0 > parsedInt || parsedInt > 255)
                {
                    throw new ArgumentOutOfRangeException($"{parsedInt} not within required IP address range [0,255]");
                }

                addressBytes[i] = (byte)parsedInt;
            }

            return new IPAddress(addressBytes);
        }

        public static string GetUrlContentAsStringAsync(string url)
        {
            var urlContent = string.Empty;

            try
            {
                using (var httpClient = new HttpClient())

                using (var httpResonse = httpClient.GetAsync(url).Result)
                {
                    urlContent = httpResonse.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException ex)
            {
                throw;
            }

            return urlContent;
        }

        public static IPAddress GetPrivateIPV4(this IPAddress ipAddress)
        {
            if (IPAddress.IsLoopback(ipAddress))
            {
                var hostAddresses = Dns.GetHostAddresses(Dns.GetHostByAddress(ipAddress.ToString()).HostName);
                var ipAddresses = hostAddresses.Where(a => a.AddressFamily == AddressFamily.InterNetwork && a.IsIPv4() && a.IsPrivate() && !IPAddress.IsLoopback(a));

                ipAddress = ipAddresses.MaxBy(a => a.Address);

                if (ipAddress == null)
                {
                    ipAddress = hostAddresses.FirstOrDefault();
                }
            }

            if (ipAddress.IsIPv6())
            {
                ipAddress = ipAddress.MapToIPv4();
            }

            return ipAddress;
        }

        public static bool IsIPv4(this IPAddress ipAddress)
        {
            return ipAddress.GetPrivatePropertyValue<bool>("IsIPv4");
        }

        public static bool IsPublic(this IPAddress ipAddress)
        {
            return !ipAddress.IsPrivate();
        }

        public static bool IsPrivate(this IPAddress ipAddress)
        {
            List<int> ipParts;

            if (ipAddress.IsIPv6())
            {
                ipAddress = ipAddress.MapToIPv4();
            }

            ipParts = ipAddress.ToString().Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();

            // in private ip range

            if (ipParts[0] == 0 || ipParts[0] == 10 || (ipParts[0] == 192 && ipParts[1] == 168) || (ipParts[0] == 172 && (ipParts[1] >= 16 && ipParts[1] <= 31)))
            {
                return true;
            }

            return false;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static bool CheckNetworkConnectivity()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }

        public static void Shutdown(this Socket socket)
        {
            ProcessExtensions.CloseHandle((uint) socket.Handle);
        }
    }
}
