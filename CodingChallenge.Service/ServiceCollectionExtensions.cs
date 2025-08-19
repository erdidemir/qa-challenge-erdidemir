using CodingChallenge.Service.Abstraction;
using CodingChallenge.Service.Factories;
using CodingChallenge.Service.Factories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CodingChallenge.Service
{
    /// <summary>
    /// The service collection extension to add services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Services
        /// </summary>
        /// <param name="services">The instance of <seealso cref="IServiceCollection"/>.</param>
        /// <returns>The instance of <seealso cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(typeof(ServiceCollectionExtensions));

            // Add Services
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionSummaryService, TransactionSummaryService>();
            services.AddScoped<IUserService, UserService>();

            // Add Factories
            services.AddScoped<ITransactionDataModelFactory, TransactionDataModelFactory>();

            return services;
        }
    }
}
