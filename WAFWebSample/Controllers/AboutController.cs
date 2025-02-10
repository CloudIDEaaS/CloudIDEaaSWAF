using Asp.Versioning;
using Hydra.Application.Common.Interfaces;
using Hydra.Application.Models.User;
using Hydra.Data.Contexts;
using Hydra.Domain.Entities;
using Hydra.Identity.Services;
using Hydra.WebApi.Providers;
using HydraDevOps.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;
using System.Text;
using Utils;

namespace Hydra.WebApi.Controllers.User.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("[instancekey]/api/[controller]")]
    [InstanceKeyRouteValue()]
    public class AboutController : ApiControllerBase
    {
        private readonly ILogger<AboutController> logger;
        private readonly HydraDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;
        private readonly string? automationServiceAccount;

        public AboutController(IConfiguration configuration, IHostEnvironment environment, HydraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<AboutController> logger) : base(httpContextAccessor)
        {
            this.logger = logger;

            this.dbContext = dbContext;
            this.configuration = configuration;
            this.environment = environment;
        }

        [HttpGet]
        [Route("about")]
        public string About()
        {
            var thisAssembly = Assembly.GetEntryAssembly();
            var assemblyName = thisAssembly.GetName();
            var versionAttribute = thisAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var builder = new StringBuilder($"{ assemblyName.Name } v{ versionAttribute.InformationalVersion } { environment.EnvironmentName }");
            var host = dbContext.GetHost();
            var port = dbContext.GetPort();
            var databaseName = dbContext.GetDatabaseName();
            var database = dbContext.Database;
            var canConnect = false;
            string connectionError = null;

            if (database.CanConnect())
            {
                canConnect = true;
            }

            try
            {
                database.OpenConnection();
            }
            catch (Exception ex) 
            {
                connectionError = ex.Message;
                canConnect = false;
            }

            builder.AppendLine();
            builder.AppendLine("Database:");
            builder.Append($"{ host }:{ port }/{ databaseName}" );

            if (canConnect) 
            {
                builder.AppendLine(" (Operational)");
            }
            else
            {
                builder.AppendLineFormat(" (Connection Error '{0}')", connectionError);
            }

            return builder.ToString();
        }
    }
}
