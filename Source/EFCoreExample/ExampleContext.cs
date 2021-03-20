namespace EFCoreExample
{
    using Microsoft.EntityFrameworkCore;

    public class ExampleContext : DbContext
    {
        private readonly string _connectionString;

        public ExampleContext()
        {
            _connectionString = "DataSource=F:\\example.db";
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(_connectionString);

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new PositionMap());
        }
    }
}