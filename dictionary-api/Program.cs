using dictionary_api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console();
Log.Logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedDatabase(Log.Logger);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/api/translate", async (string text, Context context) =>
{
    var phrase = await context.Phrases.SingleOrDefaultAsync(p => p.English == text);

    if (phrase is null) return Results.NotFound();

    return Results.Ok(phrase.Hungarian);
})
    .Produces<string>()
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi(op => new OpenApiOperation(op)
    {
        Summary = "Translate a phrase from English to Hungarian"
    });

app.Run();