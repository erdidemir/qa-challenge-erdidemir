using CodingChallenge.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CodingChallenge.WebApi.IntegrationTests
{
    public abstract class BaseIntegrationTest
    {
        protected readonly HttpClient HttpClient;
        
        protected readonly CodingChallengeDbContext CodingChallengeDbContext;

        protected BaseIntegrationTest(CustomWebApplicationFactory factory)
        {
            HttpClient = factory.CreateClient();
            IServiceScope serviceScope = factory.Services.CreateAsyncScope();
            CodingChallengeDbContext = serviceScope.ServiceProvider.GetRequiredService<CodingChallengeDbContext>();
        }
    }
}
