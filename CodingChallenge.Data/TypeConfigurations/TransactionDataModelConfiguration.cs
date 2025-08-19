using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CodingChallenge.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class TransactionDataModelConfiguration : IEntityTypeConfiguration<TransactionDataModel>
    {
        public void Configure(EntityTypeBuilder<TransactionDataModel> builder)
        {
            _ = builder.ToTable("Transactions");

            builder.Property(u => u.TransactionType)
                   .HasMaxLength(10)
                   .HasConversion<string>();
        }
    }
}
