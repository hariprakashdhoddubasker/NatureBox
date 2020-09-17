namespace NatureBox.DataAccess
{
    using Microsoft.EntityFrameworkCore;
    using NatureBox.Model;
    using System.Configuration;

    //using System.Data.Entity.ModelConfiguration.Conventions;

    public class NatureBoxDbContext : DbContext
    {
        public NatureBoxDbContext()
        {
            CreateSchema();
        }

        public DbSet<Partner> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<PartnerPayment> PartnerPayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings["NatureBoxDB"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        private void CreateSchema()
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
                AddAdmin();
            }
        }

        private bool AddAdmin()
        {
            Employees.Add(new Partner
            {
                UserName = "Admin",
                Password = "admin@naturebox",
                Role = "Admin"
            });

            return SaveChanges() == 1;
        }
    }
}
