using CodingChallenge.Data;
using CodingChallenge.Data.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Service.UnitTests
{
    public class InMemoryCodingChallengeDbContext : IDisposable
    {
        private readonly ICodingChallengeDbContext _codingChallengeDbContex;

        public InMemoryCodingChallengeDbContext()
        {
            var builder = new DbContextOptionsBuilder<CodingChallengeDbContext>();
            builder.UseInMemoryDatabase("CodingChallenge");
            var options = builder.Options;

            _codingChallengeDbContex = new CodingChallengeDbContext(options);
        }

        public ICodingChallengeDbContext CodingChallengeDbContext { get { return _codingChallengeDbContex; } }

        public void Dispose()
        {
            _codingChallengeDbContex.Dispose();
        }
    }
}
