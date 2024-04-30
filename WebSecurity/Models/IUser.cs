using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models;

public interface IUser : IStorageBase
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? OrganizationName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserName { get; set; }
    public string? EmailAddress { get; set; }
    public string? Location { get; set; }
    public bool Enabled { get; set; }
}
