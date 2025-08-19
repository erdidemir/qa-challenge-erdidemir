using CodingChallenge.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CodingChallenge.WebApi.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                ConfigureDatabaseContext(services);
            });
        }

        private static void ConfigureDatabaseContext(IServiceCollection services)
        {
            services.Remove(services.Single(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<CodingChallengeDbContext>)));

            services.AddDbContext<CodingChallengeDbContext>(options =>
            {
                options.UseInMemoryDatabase("CodingChallenge");
            });
        }
    }
}
