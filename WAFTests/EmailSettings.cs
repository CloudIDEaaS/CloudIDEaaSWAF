using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.StartupTests
{
    public class EmailSettings
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool Authentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Template { get; set; }
    }
}
