using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace CodingChallenge.Data
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public class CodingChallengeDbContextFactory : IDesignTimeDbContextFactory<CodingChallengeDbContext>
    {
        /// <inheritdoc />
        public CodingChallengeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CodingChallengeDbContext>();

            _ = optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("CodingChallengeDatabaseConnectionString")!);

            return new CodingChallengeDbContext(optionsBuilder.Options);
        }
    }
}
