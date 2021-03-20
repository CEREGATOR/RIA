namespace EFCoreExample
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PositionMap : IEntityTypeConfiguration<Position>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("Positions").HasKey(position => position.Id);
            builder.Property(position => position.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            builder.Property(position => position.PriceInEuro).HasColumnName("PriceInEuro").IsRequired();
        }
    }
}