namespace EFCoreExample
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("Orders").HasKey(order => order.Id);
            builder.Property(order => order.State).HasColumnName("State").IsRequired();
            builder.Property(order => order.CreateDate).HasColumnName("CreateDate").IsRequired();
            builder.Property(order => order.LastChangeDate).HasColumnName("LastChangeDate").IsRequired(false);

            builder.HasMany(order => order.Positions).WithMany(position => position.Orders)
                .UsingEntity(typeBuilder => { typeBuilder.ToTable("OrdersPositions"); });
        }
    }
}