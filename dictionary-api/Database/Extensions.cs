using System.Globalization;
using CsvHelper;
using dictionary_api.Entities;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace dictionary_api.Database;

public static class Extensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        var connString = config.GetConnectionString("MySQL");
        var serverVersion = ServerVersion.AutoDetect(connString);

        services.AddDbContext<Context>(options => { options.UseMySql(connString, serverVersion); });
    }

    public static void SeedDatabase(this IApplicationBuilder app, ILogger logger)
    {
        var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<Context>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var pendingMigrations = context.Database.GetPendingMigrations().ToArray();
        if (pendingMigrations.Any())
        {
            logger.Warning("There are pending migrations!");
            foreach (var m in pendingMigrations)
            {
                logger.Warning(m);
            }

            return;
        }

        if (context.Phrases.Any()) return;

        logger.Information("Seeding...");

        var file = env.ContentRootFileProvider
            .GetDirectoryContents("assets")
            .FirstOrDefault(f => f.Name == "data.csv");

        if (file is null)
        {
            logger.Error("Database seed data is missing!");
            return;
        }

        using var reader = new StreamReader(file.CreateReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var autoIncrementId = 1;

        var records = csv.GetRecords<Phrase>().Select(p =>
        {
            p.Id = autoIncrementId;
            autoIncrementId += 1;
            return p;
        });

        context.Phrases.AddRange(records);
        context.SaveChanges();

        logger.Information("Seeding done");
    }
}