namespace EFCoreExample
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserMap : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<User> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("Users").HasKey(user => user.Id);
            builder.Property(user => user.FullName).HasColumnName("FullName").HasMaxLength(100).IsRequired();

            builder.HasMany(user => user.Orders).WithOne(order => order.User).HasForeignKey(order => order.Id);
        }
    }
}