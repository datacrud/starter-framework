using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EntityFramework.DynamicFilters;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core;
using Project.Core.DomainBase;
using Project.Core.Extensions;
using Project.Core.Repositories;
using Project.Core.Session;
using Project.Core.UoW;

namespace Security.Models.Models
{
    public  class SecurityDbContext : IdentityDbContext<User, Role, string, UserLogin, UserRole, UserClaim>, IUnitOfWork
    {

        public IAppSession AppSession { get; set; }

        public SecurityDbContext()
            : base(ConnectionStringProvider.DefaultConnection)
        {
            //Database.SetInitializer<SecurityDbContext>(null);
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = false;
        }

        protected SecurityDbContext(string connectionString)
            : base(connectionString)
        {
            //Database.SetInitializer<SecurityDbContext>(null);
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = false;
        }



        //public virtual void Initialize()
        //{
        //    AppSession = new AppSession();

        //    Database.Initialize(false);
        //    this.SetFilterScopedParameterValue(DataFilter.MustHaveTenant, DataFilter.Parameters.TenantId, AppSession.TenantId);
        //}

        protected virtual void ConfigureFilters(DbModelBuilder modelBuilder)
        {

            modelBuilder.Filter(DataFilter.SoftDelete, (IEntity<string> d) => d.IsDeleted, false);
            modelBuilder.Filter(DataFilter.IsActive, (IHaveIsActive d) => d.Active, true);


            //modelBuilder.Filter(DataFilter.MustHaveTenant, (IHaveTenant<string> entity, string tenantId) => entity.TenantId == tenantId || entity.TenantId == null, () => null);

            //modelBuilder.Filter(DataFilter.MustHaveTenant, (IHaveTenant<string> e) => e.TenantId, () => GetTenantId());
        }


        private string GetTenantId()
        {
            //var currentPrincipal = Thread.CurrentPrincipal;

            var tenantId = HttpContext.Current?.User?.Identity?.GetTenantId();
            return tenantId;
        }


        //modelBuilder.Filter(DataFilter.MustHaveTenant,
        //    (IHaveTenant<string> t, string tenantId) => t.TenantId == tenantId || t.TenantId == null || t.TenantId == "", "");
        //While "t.TenantId == null" seems wrong, it's needed. See https://github.com/jcachat/EntityFramework.DynamicFilters/issues/62#issuecomment-208198058




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SecurityDbContext, Configuration>());


            //modelBuilder.Entity<User>().ToTable("AppUsers");
            //modelBuilder.Entity<Role>().ToTable("AppRoles");
            //modelBuilder.Entity<UserRole>().ToTable("AppUserRoles");
            //modelBuilder.Entity<UserClaim>().ToTable("AppUserClaims");
            //modelBuilder.Entity<UserLogin>().ToTable("AppUserLogins");
            modelBuilder.Entity<Resource>().ToTable("AppResources");
            modelBuilder.Entity<Permission>().ToTable("AppPermissions");

            //modelBuilder.Entity<User>().Property(r => r.Id);
            //modelBuilder.Entity<Role>().Property(r => r.Id);
            //modelBuilder.Entity<UserClaim>().Property(r => r.Id);

            //modelBuilder.Entity<UserRole>()
            //    .HasKey(r => new { r.UserId, r.RoleId })
            //    .ToTable("AspNetUserRoles");

            //modelBuilder.Entity<UserLogin>()
            //    .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
            //    .ToTable("AspNetUserLogins");


            #region Mititenancy User

            var user = modelBuilder.Entity<User>();

            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 1 }));

            user.Property(u => u.TenantId)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 2 }));

            #endregion


            #region Mutitenancy Role

            var role = modelBuilder.Entity<Role>()
                .ToTable("AppRoles");

            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("RoleNameIndex")
                        {IsUnique = false}));

            #endregion

            ConfigureFilters(modelBuilder);
        }

        //protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        //{
        //    var res = base.ValidateEntity(entityEntry, items);
        //    //hack to convince EF that AspNetRole.Name does not need to be unique
        //    if (!res.IsValid
        //        && entityEntry.Entity is Role
        //        && entityEntry.State == EntityState.Added
        //        && res.ValidationErrors.Count == 1
        //        && res.ValidationErrors.First().PropertyName == "Role")
        //    {
        //        return new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
        //    }

        //    return res;

        //}


        protected override DbEntityValidationResult ValidateEntity(
            DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();

                if (entityEntry.Entity is User user)
                {
                    if (this.Users.Any(u => string.Equals(u.UserName, user.UserName)
                                            && u.TenantId == user.TenantId))
                    {
                        errors.Add(new DbValidationError("User",
                            $"Username {user.UserName} is already taken for AppId {user.TenantId}"));
                    }

                    if (this.RequireUniqueEmail
                        && this.Users.Any(u => string.Equals(u.Email, user.Email)
                                               && u.TenantId == user.TenantId))
                    {
                        errors.Add(new DbValidationError("User",
                            $"Email Address {user.UserName} is already taken for AppId {user.TenantId}"));
                    }
                }
                else
                {
                    //if (entityEntry.Entity is Role role && this.Roles.Any(r => string.Equals(r.Name, role.Name)))
                    //{
                    //    errors.Add(new DbValidationError("Role",
                    //        $"Role {role.Name} already exists"));
                    //}

                    var res = base.ValidateEntity(entityEntry, items);
                    //hack to convince EF that AspNetRole.Name does not need to be unique
                    if (!res.IsValid
                        && entityEntry.Entity is Role
                        && entityEntry.State == EntityState.Added
                        && res.ValidationErrors.Count == 1
                        && res.ValidationErrors.First().PropertyName == "Role")
                    {
                        return new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
                    }

                    return res;

                }
                if (errors.Any())
                {
                    return new DbEntityValidationResult(entityEntry, errors);
                }

            }

            return new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
        }



        public DbSet<Resource> Resources { get; set; }
        public DbSet<Permission> Permissions { get; set; }




        #region IUnitOfWork Members

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public Task SaveAsync()
        {
            return base.SaveChangesAsync();
        }

        public void Save()
        {
            base.SaveChanges();
        }

        #endregion
    }
}