using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CodingChallenge.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class UserDataModelConfiguration : IEntityTypeConfiguration<UserDataModel>
    {
        public void Configure(EntityTypeBuilder<UserDataModel> builder)
        {
            _ = builder.ToTable("Users");

            builder.Property(u => u.UserId)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(u => u.UserName)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(u => u.IsActive)
                   .HasDefaultValue(true);

            builder.HasIndex(u => u.UserId)
                   .IsUnique();

            builder.HasIndex(u => u.Email)
                   .IsUnique();
        }
    }
}
