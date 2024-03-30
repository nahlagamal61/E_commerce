namespace E_commerce_DataModeling.Context
{
    using E_commerce_DataModeling.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderLineDetails> OrderLineDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();
            modelBuilder.Entity<OrderLineDetails>()
               .HasKey(ol => ol.ID);   

            modelBuilder.Entity<OrderLineDetails>()
                .HasOne(ol => ol.Order)
                .WithMany(x =>x.OrderLineDetails)
                .IsRequired();

            modelBuilder.Entity<OrderLineDetails>()
               .HasOne(ol => ol.Product);
            modelBuilder.Entity<Product>()
              .HasKey(p => p.ProductID);
        }


    }
}
