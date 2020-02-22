using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.DynamicFilters;
using Microsoft.AspNet.Identity;
using Project.Core.Enums.Framework;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.StaticResource;
using Security.Models.Models;
using Security.Server.Stores;

namespace Project.Model.SeedData
{
    public static class SecurityModelSeedDataManager
    {
        public static void RunSeed()
        {
            BusinessModelSeedDataManager.CheckMultiTenantData();

            using (var context = BusinessDbContext.Create())
            {
                context.DisableAllFilters();

                ResourceBuilder.Build(context);
                RoleBuilder.Build(context, null);
                PermissionBuilder.Build(context, null);
                UserBuilder.Build(context, null);
                UserRoleBuilder.Build(context, null);

                TenantBuilder.BuildTenantRoles(context);
                TenantBuilder.BuildTenantUserRoles(context);
                TenantBuilder.BuildTenantPermissions(context);

                context.EnableAllFilters();
            }

        }
    }

    public class ResourceBuilder
    {
        public static void Build(BusinessDbContext db)
        {
            var resourceBinders = StaticResource.GetResources();

            if (!db.Resources.Any())
            {
                foreach (var resourceBinder in resourceBinders)
                {
                    var resource = new Resource
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = resourceBinder.Name,
                        Route = resourceBinder.State,
                        IsPublic = resourceBinder.IsPublic,
                        Order = resourceBinder.Order,
                        DisplayName = resourceBinder.DisplayName,
                        ResourceOwner = resourceBinder.ResourceOwner,
                        ParentRoute = resourceBinder.ParentState
                    };
                    db.Resources.Add(resource);
                }
                db.SaveChanges();
            }
            else
            {
                foreach (var resourceBinder in resourceBinders.Where(resource => db.Resources.FirstOrDefault(x => x.Route == resource.State) == null))
                {
                    var resource = new Resource
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = resourceBinder.Name,
                        Route = resourceBinder.State,
                        IsPublic = resourceBinder.IsPublic,
                        Order = resourceBinder.Order,
                        DisplayName = resourceBinder.DisplayName,
                        ResourceOwner = resourceBinder.ResourceOwner,
                        ParentRoute = resourceBinder.ParentState
                    };
                    db.Resources.Add(resource);
                }
                db.SaveChanges();
            }
        }

    }

    public class RoleBuilder
    {
        public static void Build(BusinessDbContext db, string tenantId)
        {
            var roles = StaticRoles.GetRoles();
            Company company;
            Tenant tenant;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                company = db.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);
                tenant = db.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
                tenantId = tenant?.Id;
            }
            else
            {
                company = db.Companies.FirstOrDefault(x => x.TenantId == tenantId);
                tenant = db.Tenants.FirstOrDefault(x => x.Id == tenantId);
            }

            if (tenant?.Name != StaticTenant.Host)
                roles = roles.Where(x => x.Name != StaticRoles.SystemAdmin.Name).ToList();

            if (!db.Roles.Any(x=> x.TenantId == tenantId))
            {
                foreach (var role in roles)
                {
                    db.Roles.Add(new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = role.Name,
                        DisplayName = role.DisplayName,
                        AccessLevel = role.AccessLevel,
                        TenantId = tenant?.Id,
                        CompanyId = company?.Id
                    });
                }

                db.SaveChanges();
            }

            else
            {
                foreach (var role in roles.Where(role => db.Roles.FirstOrDefault(x => x.Name == role.Name && x.TenantId == tenantId) == null))
                {
                    db.Roles.Add(new Role { Id = Guid.NewGuid().ToString(), Name = role.Name });
                }

                db.SaveChanges();
            }
        }
    }

    public class UserRoleBuilder
    {
        public static void Build(BusinessDbContext db, string tenantId)
        {
            var manager = new Security.Server.Managers.UserManager(new AppUserStore(new SecurityDbContext()));

            Company company;
            Tenant tenant;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                company = db.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);
                tenant = db.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
                tenantId = tenant?.Id;
            }
            else
            {
                company = db.Companies.FirstOrDefault(x => x.TenantId == tenantId);
                tenant = db.Tenants.FirstOrDefault(x => x.Id == tenantId);
            }
            manager.SetTenantId(tenantId);

            var adminRole  = db.Roles.AsNoTracking().FirstOrDefault(x=> x.TenantId == tenantId && x.Name == StaticRoles.Admin.Name);
            var users = db.Users.Where(x => x.TenantId == tenantId && !x.Roles.Any()).ToList();

            foreach (var user in users)
            {
                if (user.UserName.ToLower() == "systemadmin")
                {
                    var systemAdminRole = db.Roles.AsNoTracking().FirstOrDefault(x => x.TenantId == tenantId && x.Name == StaticRoles.SystemAdmin.Name);
                    manager.AddRoleToUser(user.Id, systemAdminRole.Id, tenantId, company.Id);
                }
                else
                {
                    manager.AddRoleToUser(user.Id, adminRole.Id, tenantId, company.Id);
                }
            }
        }
    }

    public class PermissionBuilder
    {

        public static void Build(BusinessDbContext db, string tenantId)
        {
            var tenant = TenantBuilder.GetTenant(db, tenantId);

            var permissions = new List<Permission>();

            var roles = db.Roles.Where(x => x.TenantId == tenant.Id).ToList();
            var privateResources = db.Resources.Where(x => x.IsPublic == false).ToList();

            #region System Admin Role Permissions

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                var systemAdminRole = roles.First(x => x.Name == StaticRoles.SystemAdmin.Name);
                permissions.AddRange(from resource in privateResources
                                     let id = Guid.NewGuid().ToString()
                                     select new Permission()
                                     {
                                         Id = id,
                                         RoleId = systemAdminRole.Id,
                                         ResourceId = resource.Id,
                                         TenantId = tenant?.Id
                                     });
            }

            #endregion


            #region Admin Role Permission

            var adminRole = roles.FirstOrDefault(x => x.Name == StaticRoles.Admin.Name);
            var adminResources = privateResources.Where(x => x.ResourceOwner == ResourceOwner.Tenant || x.ResourceOwner == ResourceOwner.Both).ToList();

            permissions.AddRange(from privateResource in adminResources
                                 let id = Guid.NewGuid().ToString()
                                 select new Permission()
                                 {
                                     Id = id,
                                     RoleId = adminRole?.Id,
                                     ResourceId = privateResource.Id,
                                     TenantId = tenant?.Id
                                 });

            #endregion


            foreach (var permission in permissions)
            {
                if (!db.Permissions.Any(x => x.RoleId == permission.RoleId && x.ResourceId == permission.ResourceId && x.TenantId == permission.TenantId))
                    db.Permissions.Add(permission);
            }

            db.SaveChanges();
        }

    }

    public class UserBuilder
    {
        public static void Build(BusinessDbContext db, string tenantId)
        {
            BusinessDbContext businessDbContext = new BusinessDbContext();
            Branch branch;
            Company company;
            Tenant tenant;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                branch = businessDbContext.Branches.FirstOrDefault(x => x.Type == BranchType.HeadOffice);
                company = businessDbContext.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);
                tenant = businessDbContext.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
            }
            else
            {
                branch = businessDbContext.Branches.FirstOrDefault(x => x.Type == BranchType.HeadOffice && x.TenantId == tenantId);
                company = businessDbContext.Companies.FirstOrDefault(x => x.TenantId == tenantId);
                tenant = businessDbContext.Tenants.FirstOrDefault(x => x.Id == tenantId);
            }

            var roles = db.Roles.ToList();

            var users = new List<User>();

            #region Create System Admin User

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                var systemRole = roles.FirstOrDefault(x => x.Name == StaticRoles.SystemAdmin.Name);

                var systemAdminUserId = Guid.NewGuid().ToString();

                var systemAdmin = new User()
                {
                    Id = systemAdminUserId,
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@datacrud.com",
                    UserName = "systemadmin",
                    PhoneNumber = "+8801911831907",
                    PasswordHash = new PasswordHasher().HashPassword("123qwe"),
                    Roles = { new UserRole() { UserId = systemAdminUserId, RoleId = systemRole?.Id, TenantId = tenant?.Id, CompanyId = company?.Id } },
                    BranchId = branch?.Id,
                    TenantId = tenant?.Id,
                    TenantName = tenant?.TenancyName,
                    CompanyId = company?.Id,
                    IsActive = true,
                    Created = DateTime.Now,
                    CreatedBy = null,
                };

                systemAdmin.EmployeeId = BusinessModelSeedDataManager.AddEmployee(db, systemAdmin).Id;

                users.Add(systemAdmin);
            }

            #endregion

            #region Create Admin User

            var adminRole = roles.FirstOrDefault(x => x.Name == StaticRoles.Admin.Name);

            var adminUserId = Guid.NewGuid().ToString();
            var admin = new User()
            {
                Id = adminUserId,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@default.com",
                UserName = "admin",
                PhoneNumber = "",
                PasswordHash = new PasswordHasher().HashPassword("123qwe"),
                Roles = { new UserRole() { UserId = adminUserId, RoleId = adminRole?.Id, TenantId = tenant?.Id, CompanyId = company?.Id } },
                BranchId = branch?.Id,
                TenantId = tenant?.Id,
                TenantName = tenant?.TenancyName,
                CompanyId = company?.Id,
                IsActive = true,
            };

            admin.EmployeeId = BusinessModelSeedDataManager.AddEmployee(db, admin).Id;

            users.Add(admin);

            #endregion

            if (!db.Users.Any())
            {
                foreach (var user in users)
                {
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    db.Users.Add(user);
                }
            }
            else
            {
                foreach (var user in users.Where(user => db.Users.FirstOrDefault(x => x.UserName == user.UserName) == null))
                {
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    db.Users.Add(user);
                }
            }

            db.SaveChanges();
        }
    }


    public class TenantBuilder
    {
        public static Tenant GetTenant(BusinessDbContext db, string tenantId)
        {
            var tenancyName = StaticTenant.Host.ToTenancyName();

            var tenant = string.IsNullOrWhiteSpace(tenantId)
                ? db.Tenants.FirstOrDefault(x => x.TenancyName == tenancyName)
                : db.Tenants.Find(tenantId);

            return tenant;
        }

        public static List<Tenant> GetHostTenants(BusinessDbContext db)
        {
            var tenancyName = StaticTenant.Host.ToTenancyName();
            var tenants = db.Tenants.Where(x => x.TenancyName != tenancyName).AsNoTracking().ToList();

            return tenants;
        }

        public static void BuildTenantRoles(BusinessDbContext context)
        {
            var tenants = GetHostTenants(context);
            foreach (var tenant in tenants)
            {
                RoleBuilder.Build(context, tenant.Id);
            }
        }

        public static void BuildTenantUserRoles(BusinessDbContext context)
        {
            var tenants = GetHostTenants(context);
            foreach (var tenant in tenants)
            {
                UserRoleBuilder.Build(context, tenant.Id);
            }
        }

        public static void BuildTenantPermissions(BusinessDbContext context)
        {
            var tenants = GetHostTenants(context);
            foreach (var tenant in tenants)
            {
                PermissionBuilder.Build(context, tenant.Id);
            }
        }
    }

}