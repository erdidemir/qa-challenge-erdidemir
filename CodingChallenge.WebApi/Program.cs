using CodingChallenge.Data;
using CodingChallenge.Service;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CodingChallenge.WebApi
{
    [ExcludeFromCodeCoverage(Justification = "Not testable using unit-test")]
    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<CodingChallengeDbContext>(options =>
            {
                _ = bool.TryParse(builder.Configuration["DatabaseContextSettings:IsEnableSensitiveDataLogging"], out bool isEnableSensitiveDataLogging);

                if (isEnableSensitiveDataLogging)
                {
                    _ = options.EnableSensitiveDataLogging();
                }
                _ = options.UseSqlServer(builder.Configuration.GetConnectionString("CodingChallengeDatabaseConnectionString"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);

                    if (int.TryParse(builder.Configuration["DatabaseContextSettings:MinBatchSize"], out int minBatchSize))
                    {
                        _ = sqlOptions.MinBatchSize(minBatchSize);
                    }

                    if (int.TryParse(builder.Configuration["DatabaseContextSettings:MaxBatchSize"], out int maxBatchSize))
                    {
                        _ = sqlOptions.MaxBatchSize(maxBatchSize);
                    }
                });

                _ = bool.TryParse(builder.Configuration["DatabaseContextSettings:IsConfigureConsoleLogger"], out bool isConfigureConsoleLogger);
                if (isConfigureConsoleLogger)
                {
                    LogLevel logLevel = LogLevel.Warning;
                    _ = Enum.TryParse(builder.Configuration["DatabaseContextSettings:ConsoleLoggerMinimumLoggingLevel"], out logLevel);
                    _ = options.UseLoggerFactory(LoggerFactory.Create(builder => _ = builder.AddConsole().SetMinimumLevel(logLevel)));
                }
            });
            builder.Services.AddScoped<ICodingChallengeDbContext, CodingChallengeDbContext>();
            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            _ = bool.TryParse(builder.Configuration["DatabaseContextSettings:IsApplyDatabaseMigrateAutomatically"], out bool isApplyDatabaseMigrateAutomatically);

            if (isApplyDatabaseMigrateAutomatically)
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<CodingChallengeDbContext>();

                if (context.Database.IsRelational())
                    context.Database.Migrate();
            }

            app.Run();
        }
    }
}