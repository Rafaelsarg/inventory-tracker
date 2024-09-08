using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Models;

namespace InventoryManagementSolution.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<AppUser> appuser { get; set; }
        public DbSet<InternalAuthentication> internalauthentication { get; set; }
        public DbSet<Inventory> inventory { get; set; }
        public DbSet<ProductType> producttype { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Supplier> supplier { get; set; }
        public DbSet<RestockingTransaction> restockingtransaction { get; set; }
        public DbSet<AvailableProductInstance> availableproductinstance { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<SalesTransaction> salestransaction { get; set; }
        public DbSet<SoldProductInstance> soldproductinstance { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // AppUser -> InternalAuthentication
            modelBuilder.Entity<AppUser>()
                .HasOne(au => au.InternalAuthentication)
                .WithOne(ia => ia.AppUser)
                .HasForeignKey<InternalAuthentication>(ia => ia.userid)
                .IsRequired();

            // AppUser -> Inventory
            modelBuilder.Entity<AppUser>()
                .HasMany(au => au.Inventories)
                .WithOne(i => i.AppUser)
                .HasForeignKey(i => i.userid)
                .IsRequired();
            
            // Inventory -> AvailableProductInstances
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.ProductTypes)
                .WithOne(pt => pt.Inventory)
                .HasForeignKey(pt => pt.inventoryid)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            // Inventory -> AvailableProductInstances
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.AvailableProductInstances)
                .WithOne(avpi => avpi.Inventory)
                .HasForeignKey(avpi => avpi.inventoryid)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            // Inventory -> Supplier
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Suppliers)
                .WithOne(s => s.Inventory)
                .HasForeignKey(s => s.inventoryid)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            // Inventory -> AvailableProductInstances
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Customers)
                .WithOne(c => c.Inventory)
                .HasForeignKey(c => c.inventoryid)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            // ProductType -> Product
            modelBuilder.Entity<ProductType>()
                .HasMany(pt => pt.Products)
                .WithOne(p => p.ProductType)
                .HasForeignKey(p => p.producttypeid)
                .IsRequired();

            // Product -> AvailableProductInstances
            modelBuilder.Entity<Product>()
                .HasMany(p => p.AvailableProductInstances)
                .WithOne(avpi => avpi.Product)
                .HasForeignKey(avpi => avpi.productid)
                .IsRequired();
            
            // RestockingTransaction -> AvailableProductInstances
            modelBuilder.Entity<RestockingTransaction>()
                .HasMany(rt => rt.AvailableProductInstances)
                .WithOne(avpi => avpi.RestockingTransaction)
                .HasForeignKey(avpi => avpi.restockingtransactionid)
                .IsRequired();
            
            // Supplier -> RestockingTransaction
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.RestockingTransactions)
                .WithOne(rt => rt.Supplier)
                .HasForeignKey(rt => rt.supplierid)
                .IsRequired();
            
            // Customer -> SalesTransaction
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.SalesTransactions)
                .WithOne(st => st.Customer)
                .HasForeignKey(st => st.customerid)
                .IsRequired();
        
            // SalesTransaction -> SoldProductInstances
            modelBuilder.Entity<SalesTransaction>()
                .HasMany(st => st.SoldProductInstances)
                .WithOne(spi => spi.SalesTransaction)
                .HasForeignKey(spi => spi.salestransactionid)
                .IsRequired();
        }
    }
}