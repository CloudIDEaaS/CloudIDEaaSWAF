using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models;

public interface IIPAddress : IStorageBase
{
    string? AttemptUserName { get; set; }
    bool LoginFailed { get; set; }
    DateTime Attempt { get; set; }
    string? Authorization { get; set; }
    string? IpAddress { get; set; }
    int Port { get; set; }
    string? Host { get; set; }
    string? Referer { get; set; }
    string? UserAgent { get; set; }
    string? Origin { get; set; }
    string? FetchSite { get; set; }
    string? FetchMode { get; set; }
    string? FetchDestination { get; set; }
    string? Token { get; set; }
    string? Location { get; set; }
    DateTime? LastActivity { get; set; }
}
