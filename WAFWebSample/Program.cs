using System.Reflection;
using WebSecurity;

namespace WAFWebSample;

public class Program
{
    public static void Main(string[] args)
    {
        bool runStartupTests;
        var currentDirectory = Directory.GetCurrentDirectory();
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var assembly = Assembly.GetEntryAssembly();
        var location = Path.GetDirectoryName(assembly.Location);
        var rulesetFile = Path.Combine(location, "wafruleset.json");
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(currentDirectory)
            .AddJsonFile($"appsettings.{environment}.json", false, true)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile(rulesetFile, false, true)
            .Build();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddKestrelWAF(configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseKestrelWAF();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        runStartupTests = bool.Parse(configuration["RunStartupTests"]);

        if (runStartupTests)
        {
            WAFWebSample.StartupTests.StartupTests.RunStartupTests();
        }

        app.Run();
    }
}
