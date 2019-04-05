using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Server.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<SecurityModels.AspNetResource> Resources { get; set; }
        public DbSet<SecurityModels.AspNetPermission> Permissions { get; set; }
    }
}