using System.Diagnostics.CodeAnalysis;
using DepositCalculator.Entities.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositCalculator.Entities.EntityConfigurations
{
    /// <inheritdoc cref="IEntityTypeConfiguration{DepositCalculationEntity}" />
    [ExcludeFromCodeCoverage]
    public class DepositCalculationEntityTypeConfiguration : IEntityTypeConfiguration<DepositCalculationEntity>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<DepositCalculationEntity> builder)
        {
            builder.ToTable(EntityTypeConfigurationConstants.DepositCalculationsTableName);

            builder
                .Property(depositCalculationEntity => depositCalculationEntity.Percent)
                .HasPrecision(8, 5);

            builder
                .Property(depositCalculationEntity => depositCalculationEntity.DepositAmount)
                .HasPrecision(9, 2);

            builder
                .HasMany(depositCalculationEntity => depositCalculationEntity.MonthlyCalculations)
                .WithOne()
                .HasForeignKey(monthlyDepositCalculationEntity => monthlyDepositCalculationEntity.DepositCalculationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}