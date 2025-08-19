using CodingChallenge.Data.Abstraction.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CodingChallenge.Data.Abstraction.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class TransactionDataModelConfiguration : IEntityTypeConfiguration<TransactionDataModel>
    {
        public void Configure(EntityTypeBuilder<TransactionDataModel> builder) => _ = builder.ToTable("Transactions");
    }
}
