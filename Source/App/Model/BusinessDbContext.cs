using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Security.Models.Models;

namespace Project.Model
{
    public class BusinessDbContext : SecurityDbContext
    {
        public BusinessDbContext() : base("name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public static BusinessDbContext Create()
        {
            return new BusinessDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Edition>().Ignore(t => t.EnableFeatureEdit);
            modelBuilder.Entity<Company>().Ignore(e => e.IsHostAction);
            modelBuilder.Entity<CompanySetting>().Ignore(e => e.IsHostAction);
            modelBuilder.Entity<Branch>().Ignore(e => e.IsHostAction);
            modelBuilder.Entity<Partner>().Ignore(e => e.IsHostAction);
            modelBuilder.Entity<FiscalYear>().Ignore(e => e.IsHostAction);
            modelBuilder.Entity<Employee>().Ignore(e => e.IsHostAction);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BusinessDbContext, Configuration>());

        }


        public DbSet<AuditLog> TransactionLogs { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<EmailLog> EmailNotificationLogs { get; set; }
        public DbSet<MonthlyEmailBalance> MonthlyEmailBalances { get; set; }

        public DbSet<Feature> Features { get; set; }
        public DbSet<Edition> Editions { get; set; }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPayment> Payments { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public DbSet<HostSetting> HostSetting { get; set; }
        public DbSet<Rfq> Rfqs { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
