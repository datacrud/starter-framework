using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Server.Models;
using static Project.Server.Models.SecurityModels;

namespace Project.Server.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            AddRoles(context);
            AddResources(context);
            AddPermissions(context);
            AddUsers(context);
        }

        private static void AddUsers(ApplicationDbContext db)
        {
            var roles = db.Roles.ToList();
            var adminRole = roles.FirstOrDefault(x => x.Name == "Admin");
            var systemRole = roles.FirstOrDefault(x => x.Name == "SystemAdmin");

            var id = "";
            var users = new List<ApplicationUser>();

            id = Guid.NewGuid().ToString();
            users.Add(new ApplicationUser()
            {
                Id = id, FirstName = "System", LastName = "Admin", Email = "systemdmin@default.com",
                UserName = "systemadmin", PasswordHash = new PasswordHasher().HashPassword("123qwe"),
                Roles = {new IdentityUserRole() {UserId = id, RoleId = systemRole.Id}}
            });

            id = Guid.NewGuid().ToString();
            users.Add(new ApplicationUser()
            {
                Id = id, FirstName = "Super", LastName = "Admin", Email = "superadmin@default.com",
                UserName = "superadmin", PasswordHash = new PasswordHasher().HashPassword("123qwe"),
                Roles = {new IdentityUserRole() {UserId = id, RoleId = adminRole.Id}}
            });


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


        private static void AddPermissions(ApplicationDbContext db)
        {
            var permissions = new List<AspNetPermission>();

            var roles = db.Roles.ToList();
            var privateSesources =  db.Resources.Where(x=> x.IsPublic == false).ToList();

            if (!db.Permissions.Any())
            {
                var role = roles.First(x => x.Name == "SystemAdmin");
                permissions.AddRange(from resource in privateSesources
                                     let id = Guid.NewGuid().ToString()
                                     select new AspNetPermission()
                                     {
                                         Id = id,
                                         RoleId = role.Id,
                                         RoleName = role.Name,
                                         ResourceId = resource.Id
                                     });

            }

            db.Permissions.AddRange(permissions);
            db.SaveChanges();
        }        


        private static void AddResources(ApplicationDbContext db)
        {
            var resources = new List<AspNetResource>
            {
                new AspNetResource {Id = "", Name = "Root", Route = "root.root", IsPublic = false},

                new AspNetResource {Id = "", Name = "Login", Route = "root.login", IsPublic = true},
                new AspNetResource {Id = "", Name = "Access Denied", Route = "root.access-denied", IsPublic = true},
                new AspNetResource {Id = "", Name = "About", Route = "root.about", IsPublic = true},
                new AspNetResource {Id = "", Name = "Contact", Route = "root.contact", IsPublic = true},

                new AspNetResource {Id = "", Name = "Home", Route = "root.home", IsPublic = false},
                new AspNetResource {Id = "", Name = "Dashboard", Route = "root.dashboard", IsPublic = false},
                new AspNetResource {Id = "", Name = "Profile", Route = "root.profile", IsPublic = false},
                new AspNetResource {Id = "", Name = "User", Route = "root.user", IsPublic = false},

                new AspNetResource {Id = "", Name = "Role", Route = "root.role", IsPublic = false},
                new AspNetResource {Id = "", Name = "AspNetResource", Route = "root.resource", IsPublic = false},
                new AspNetResource {Id = "", Name = "AspNetPermission", Route = "root.permission", IsPublic = false}

            };

            if (!db.Resources.Any())
            {
                foreach (var resource in resources)
                {
                    resource.Id = Guid.NewGuid().ToString();
                    db.Resources.Add(resource);
                }
                db.SaveChanges();
            }
            else
            {
                foreach (var resource in resources.Where(resource => db.Resources.FirstOrDefault(x => x.Route == resource.Route) == null))
                {
                    resource.Id = Guid.NewGuid().ToString();
                    db.Resources.Add(resource);
                }
                db.SaveChanges();
            }
        }


        private static void AddRoles(ApplicationDbContext db)
        {
            var roles = new List<string>
            {
                "SystemAdmin", "Admin", "Manager", "User"
            };

            if (!db.Roles.Any())
            {
                foreach (var role in roles)
                {
                    db.Roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = role });
                }
                db.SaveChanges();
            }

            else
            {
                foreach (var role in roles.Where(role => db.Roles.FirstOrDefault(x => x.Name == role) == null))
                {
                    db.Roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = role });
                }
                db.SaveChanges();
            }
        }
    }
}
