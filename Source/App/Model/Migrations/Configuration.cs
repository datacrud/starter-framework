using System.Data.Entity.Migrations;
using Project.Model.SeedData;

namespace Project.Model.Migrations
{
    public class Configuration : DbMigrationsConfiguration<BusinessDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }


        protected override void Seed(BusinessDbContext context)
        {
            //BusinessModelSeedDataManager.RunSeed(context);
        }
    }
}