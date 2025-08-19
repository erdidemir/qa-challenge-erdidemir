using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodingChallenge.Data
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public class CodingChallengeDbContext(
        DbContextOptions<CodingChallengeDbContext> options) : DbContext(options), ICodingChallengeDbContext
    {
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <inheritdoc />
        public DbSet<TransactionDataModel> Transactions { get; set; }

        /// <inheritdoc />
        public DbSet<UserDataModel> Users { get; set; }
    }
}