using System.Diagnostics.CodeAnalysis;
using DepositCalculator.Entities.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositCalculator.Entities.EntityConfigurations
{
    /// <inheritdoc cref="IEntityTypeConfiguration{MonthlyDepositCalculationEntity}" />
    [ExcludeFromCodeCoverage]
    public class MonthlyDepositCalculationEntityTypeConfiguration : IEntityTypeConfiguration<MonthlyDepositCalculationEntity>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<MonthlyDepositCalculationEntity> builder)
        {
            builder.ToTable(EntityTypeConfigurationConstants.MonthlyDepositCalculationsTableName);

            builder
                .Property(monthlyDepositCalculationEntity => monthlyDepositCalculationEntity.TotalDepositAmount)
                .HasPrecision(17, 9);

            builder
                .Property(monthlyDepositCalculationEntity => monthlyDepositCalculationEntity.DepositByMonth)
                .HasPrecision(17, 9);
        }
    }
}