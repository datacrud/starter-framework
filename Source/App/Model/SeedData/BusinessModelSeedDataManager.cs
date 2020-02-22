using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.DynamicFilters;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.Helpers;
using Project.Core.StaticResource;
using Security.Models.Models;

namespace Project.Model.SeedData
{
    public static class BusinessModelSeedDataManager
    {
        public static void FillTenantSubscription()
        {
            using (var db = new BusinessDbContext())
            {
                var tenants = db.Tenants.ToList();

                var subscriptions = db.Subscriptions.AsNoTracking().ToList();

                foreach (var tenant in tenants)
                {
                    var subscription = subscriptions.Where(x => x.TenantId == tenant.Id).OrderByDescending(x=> x.ExpireOn).FirstOrDefault();
                    if (subscription != null)
                    {
                        tenant.SubscriptionId = subscription.Id;

                        db.Entry(tenant).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }
        }

        public static Employee AddEmployee(BusinessDbContext db, User user)
        {

            var count = db.Employees.Count(x => x.TenantId == user.TenantId);

            var employee = new Employee()
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                Modified = null,

                CreatedBy = null,
                Active = true,

                Code = $"EMP-000{count + 1}",
                Name = user.FirstName,
                Surname = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Type = EmployeeType.FullTime,
                DateOfBirth = null,

                TenantId = user.TenantId,
                CompanyId = user.CompanyId,
                BranchId = user.BranchId
            };

            if (!db.Employees.Any(x => x.TenantId == employee.TenantId && x.Email == employee.Email))
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                return employee;
            }

            return db.Employees.FirstOrDefault(x => x.TenantId == employee.TenantId && x.Email == employee.Email);
        }
        
        public static void CreateFiscalYear(BusinessDbContext db, string tenantId = null)
        {
            var tenantIds = string.IsNullOrWhiteSpace(tenantId)
                ? db.Tenants.Select(x => x.Id).ToList()
                : new List<string>() { tenantId };

            foreach (var id in tenantIds)
            {
                var start = new DateTime(DateTime.Today.Year, 1, 1).Date;
                var end = start.AddYears(1).Date.AddDays(-1);

                var company = db.Companies.FirstOrDefault(x => x.TenantId == id);
                for (int i = 0; i < 10; i++)
                {
                    var fiscalYear = new FiscalYear()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Created = DateTime.Now,
                        CreatedBy = null,
                        Active = true,

                        StartDate = start,
                        EndDate = end,

                        TenantId = id,
                        CompanyId = company?.Id
                    };

                    if (!db.FiscalYears.Any(x => x.TenantId == id && x.StartDate == start && x.EndDate == end))
                    {
                        db.FiscalYears.Add(fiscalYear);
                        db.SaveChanges();
                    }

                    start = end.Date.AddDays(1).Date;
                    end = start.AddYears(1).Date.AddDays(-1);
                }
            }
        }



        public static void AddWarehouses(BusinessDbContext db)
        {
            if (db.Warehouses.Any()) return;

            var tenant = db.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
            var company = db.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);

            db.Warehouses.Add(new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,

                CreatedBy = null,

                Active = true,
                Code = "HO",
                Name = "Head Office",
                Type = WarehouseType.HeadOffice,

                TenantId = tenant?.Id,
                CompanyId = company?.Id
            });

            db.SaveChanges();
        }


        public static void AddSupplier(BusinessDbContext db)
        {
            if (db.Suppliers.Any()) return;

            var tenant = db.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
            var company = db.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);

            var suppliers = StaticSupplier.GetSuppliers();

            var i = 0;
            foreach (var supplier in suppliers)
            {
                i = i + 1;
                db.Suppliers.Add(new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreatedBy = null,
                    ModifiedBy = null,
                    Active = true,

                    Code = supplier,
                    Name = supplier,
                    Type = SupplierType.Company,
                    Phone = "",
                    OpeningDue = 0,
                    TotalDue = 0,

                    TenantId = tenant?.Id,
                    CompanyId = company?.Id
                });
            }
            
            db.SaveChanges();
        }


        public static void AddSubscription(BusinessDbContext db)
        {
            if (db.Subscriptions.Any()) return;

            var tenant = db.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
            var company = db.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);
            var edition = db.Editions.FirstOrDefault(x => x.Name == StaticEdition.Deluxe);

            db.Subscriptions.Add(new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = null,
                Modified = DateTime.Now,
                ModifiedBy = null,
                Active = true,

                TenantId = tenant?.Id,
                CompanyId = company?.Id,

                EditionId = edition?.Id,
                Package = SubscriptionPackage.LifeTime,
                Price = 0,
                Status = SubscriptionStatus.Active,
                ExpireOn = DateTime.Today.AddYears(100),
                IsPaymentCompleted = true,
                PaymentStatus = SubscriptionPaymentStatus.Paid
            });

            db.SaveChanges();
        }

        public static void AddBranches()
        {

            using (var context = new BusinessDbContext())
            {
                if (context.Branches.Any()) return;

                AddWarehouses(context);

                var tenant = context.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
                var company = context.Companies.FirstOrDefault(x => x.Name == StaticCompany.Host);
                var warehouse = context.Warehouses.FirstOrDefault(x => x.Type == WarehouseType.HeadOffice);

                var entity = new Branch()
                {
                    Id = Guid.NewGuid().ToString(),
                    Active = true,
                    Created = DateTime.Now,
                    CreatedBy = null,



                    Code = "HO",
                    Name = "Head Office",
                    Address = "",
                    Type = BranchType.HeadOffice,
                    LinkedWarehouseId = warehouse?.Id,

                    CompanyId = company?.Id,
                    TenantId = tenant?.Id,
                };

                context.Branches.Add(entity);

                context.SaveChanges();
            }
        }



        public static void AddCompanySettings(BusinessDbContext context)
        {
            if (context.CompanySettings.Any()) return;

            var tenant = context.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);
            var company = context.Companies.FirstOrDefault(x => x.Name == StaticTenant.Host);

            var id = Guid.NewGuid().ToString();
            var companySettings = new CompanySetting
            {
                Id = id,
                Active = true,
                Created = DateTime.Now,
                CreatedBy = null,
                Modified = DateTime.Now,
                ModifiedBy = null,

                HostingValidTill = null,
                PoweredBy = "Powered by www.datacrud.com",

                TenantId = tenant?.Id,                
                CompanyId = company?.Id
            };

            context.CompanySettings.Add(companySettings);
            context.SaveChanges();
        }

        public static void AddCompany(BusinessDbContext context)
        {
            if (context.Companies.Any()) return;

            var tenant = context.Tenants.FirstOrDefault(x => x.Name == StaticTenant.Host);

            var id = Guid.NewGuid().ToString();
            var company = new Company()
            {
                Id = id,
                Active = true,
                Created = DateTime.Now,
                CreatedBy = null,
                Modified = DateTime.Now,
                ModifiedBy = null,

                Name = StaticCompany.Host,
                Address = "Dhaka",
                Email = "sabbir@datacrud.com",
                Phone = "01715189043",

                TenantId = tenant?.Id,
                Web = "http://datacrud.com",
            };

            context.Companies.Add(company);
            context.SaveChanges();
        }

        private static void AddTenant(BusinessDbContext context)
        {
            if(context.Tenants.Any()) return;

            var edition = context.Editions.FirstOrDefault(x => x.Name == StaticEdition.Deluxe);

            context.Tenants.Add(new Tenant()
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                Active = true,

                ConnectionString = null,
                EditionId = edition?.Id,
                IsInTrialPeriod = false,
                Name = StaticTenant.Host,
                TenancyName = TenantHelper.BuildTenancyName(StaticTenant.Host),
                SubscriptionEndTime = DateTime.Today.AddYears(100),
                Url = $"http://{StaticTenant.Host.ToLower()}" +
                      $".datacrud.com"
            });

            context.SaveChanges();
        }

        private static void AddEditionFeature(BusinessDbContext context)
        {
            var staticEditionFeatures = StaticEditionFeature.GetAll();
            List<Feature> features = new List<Feature>();

            var editions = context.Editions.ToList();

            foreach (var staticEditionFeature in staticEditionFeatures)
            {
                var edition = editions.FirstOrDefault(x => x.Name == staticEditionFeature.EditionName);
                if (edition != null)
                {
                    Feature feature = new Feature
                    {
                        Id = Guid.NewGuid().ToString(),
                        Active = true,
                        Created = DateTime.Now,
                        CreatedBy = null,
                        Modified = DateTime.Now,
                        ModifiedBy = null,

                        EditionId = edition.Id,
                        Name = staticEditionFeature.Name,
                        Value = staticEditionFeature.Value,
                        ValueType = staticEditionFeature.ValueType,

                        Order = staticEditionFeature.Order,

                        IsEnabled = true,
                        IsFeature = false,
                        IsEditionFeature = true,
                        IsTenantFeature = false
                    };

                    features.Add(feature);
                }
                
            }

            if (!context.Features.Any())
            {
                context.Features.AddRange(features);
            }
            else
            {
                foreach (var feature in features)
                {
                    if (!context.Features.Any(x =>
                        x.Name == feature.Name && !x.IsFeature && x.IsEditionFeature && !x.IsTenantFeature))
                    {
                        context.Features.Add(feature);
                    }
                }
            }
            context.SaveChanges();
        }

        private static void AddEdition(BusinessDbContext context)
        {
            if (context.Editions.Any())
                return;           

            context.Editions.Add(new Edition()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = null,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Active = true,
                IsActive = true,

                Name = StaticEdition.Deluxe,
                DisplayName = StaticEdition.Deluxe,
                Descriminator = StaticEdition.Deluxe,

                MonthlyPrice = 4000,
                QuarterPrice = 4000 * 3,
                HalfYearlyPrice = 4000 * 6,
                AnnualPrice = 4000 * 12,
                TrialDayCount = 0,
                WaitingDayAfterExpire = 10,

                MaximumNoOfShowroom = 5,
                MinimumNoOfShowroom = 4
            });

            context.Editions.Add(new Edition()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = null,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                Active = true,

                Name = StaticEdition.Advanced,
                DisplayName = StaticEdition.Advanced,
                Descriminator = StaticEdition.Advanced,

                MonthlyPrice = 3000,
                QuarterPrice = 3000 * 3,
                HalfYearlyPrice = 3000 * 6,
                AnnualPrice = 3000 * 12,
                TrialDayCount = 0,
                WaitingDayAfterExpire = 10,

                Order = 4,
                MaximumNoOfShowroom = 3,
                MinimumNoOfShowroom = 3,
            });

            context.Editions.Add(new Edition()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = null,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                Active = true,

                Name = StaticEdition.Stanard,
                DisplayName = StaticEdition.Stanard,
                Descriminator = StaticEdition.Stanard,

                MonthlyPrice = 2250,
                QuarterPrice = 2250 * 3,
                HalfYearlyPrice = 2250 * 6,
                AnnualPrice = 2250 * 12,
                TrialDayCount = 0,
                WaitingDayAfterExpire = 10,

                Order = 3,
                MaximumNoOfShowroom = 2,
                MinimumNoOfShowroom = 2
            });

            context.Editions.Add(new Edition()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = null,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                Active = true,

                Name = StaticEdition.Starter,
                DisplayName = StaticEdition.Starter,
                Descriminator = StaticEdition.Starter,

                MonthlyPrice = 1750,
                QuarterPrice = 1750 * 3,
                HalfYearlyPrice = 1750 * 6,
                AnnualPrice = 1750 * 12,
                TrialDayCount = 0,
                WaitingDayAfterExpire = 10,

                Order = 2,
                MaximumNoOfShowroom = 1,
                MinimumNoOfShowroom = 1
            });

            context.Editions.Add(new Edition()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = null,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                Active = true,

                Name = StaticEdition.Trial,
                DisplayName = StaticEdition.Trial,
                Descriminator = StaticEdition.Trial,

                MonthlyPrice = 0,
                AnnualPrice = 0,
                TrialDayCount = 7,
                WaitingDayAfterExpire = 0,

                Order = 1,
                MaximumNoOfShowroom = 1,
                MinimumNoOfShowroom = 1
            });

            context.SaveChanges();

        }

        private static void AddFeatures(BusinessDbContext context)
        {
            var staticFeatures = StaticFeature.GetAll();
            List<Feature> features = new List<Feature>();

            foreach (var staticFeature in staticFeatures)
            {
                Feature feature = new Feature
                {
                    Id = Guid.NewGuid().ToString(),
                    Active = true,
                    Created = DateTime.Now,
                    CreatedBy = null,
                    Modified = DateTime.Now,
                    ModifiedBy = null,

                    EditionId = null,
                    Name = staticFeature.Name,
                    Value = staticFeature.Value,
                    ValueType = staticFeature.ValueType,

                    Order = staticFeature.Order,

                    IsEnabled = true,
                    IsFeature = true,
                    IsEditionFeature = false,
                    IsTenantFeature = false
                };

                features.Add(feature);
            }

            if (!context.Features.Any())
            {
                context.Features.AddRange(features);
            }
            else
            {
                foreach (var feature in features)
                {
                    if (!context.Features.Any(x =>
                        x.Name == feature.Name && x.IsFeature && !x.IsEditionFeature && !x.IsTenantFeature))
                    {
                        context.Features.Add(feature);
                    }
                }
            }
            context.SaveChanges();
        }



        public static void CheckMultiTenantData()
        {

            using (var context = new BusinessDbContext())
            {
                context.DisableAllFilters();

                AddFeatures(context);
                AddEdition(context);
                AddEditionFeature(context);
                AddTenant(context);
                AddCompany(context);
                AddCompanySettings(context);
                AddBranches();
                AddSubscription(context);

                context.EnableAllFilters();
            }
        }



        public static void RunSeed()
        {
            CheckMultiTenantData();

            using (var context = new BusinessDbContext())
            {
                context.DisableAllFilters();

                //AddWarehouses(context);
                //AddSupplier(context);

                context.EnableAllFilters();
            }

        }

    }
}