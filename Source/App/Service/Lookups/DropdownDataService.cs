using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.Session;
using Project.Model;
using Project.ViewModel;

namespace Project.Service.Lookups
{
    public interface IDropdownDataService
    {
        Task<List<DropdownViewModel>> GetData(DropdownType type);
        Task<List<DropdownViewModel>> GetData(DropdownType type, string id);
        Task<List<DropdownViewModel>> GetData(DropdownType type, string pid, string cid);
    }

    public class DropdownDataService : IDropdownDataService, IDisposable
    {
        private readonly BusinessDbContext _db;

        public DropdownDataService()
        {
            _db = new BusinessDbContext();
        }

        private string GetPackageName(string package)
        {
            var subscriptionStatus = (SubscriptionPackage)Convert.ToInt32(package);
            return "(" + subscriptionStatus.GetDescription() + ")";
        }

        public async Task<List<DropdownViewModel>> GetData(DropdownType type)
        {
            var session = new AppSession();

            switch (type)
            {
                case DropdownType.BranchEmployee:
                    return await _db.Employees.Where(x => x.Active && x.BranchId == session.BranchId)
                        .Select(x => new DropdownViewModel { Id = x.Id, Name = x.Name })
                        .OrderBy(x => x.Name).ToListAsync();

                
                case DropdownType.Feature:
                    return await _db.Features.Where(x => x.Active)
                        .Select(x => new DropdownViewModel { Id = x.Id, Name = x.Name })
                        .OrderBy(x => x.Name).ToListAsync();

                case DropdownType.Edition:
                    return await _db.Editions.Where(x => x.Active && x.IsActive)
                        .Select(x => new DropdownViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            MetaData = "MP-" + x.MonthlyPrice + "::AP-" + x.AnnualPrice + "::APD-" + x.AnnualPriceDiscountPercentage,
                        })
                        .OrderBy(x => x.Name).ToListAsync();


                case DropdownType.Tenant:
                    return await _db.Tenants.Where(x => x.Active)
                        .Select(x => new DropdownViewModel { Id = x.Id, Name = x.Name })
                        .OrderBy(x => x.Name).ToListAsync();

                case DropdownType.Subscription:
                    return await _db.Subscriptions.Where(x => x.Active && x.TenantId == session.TenantId)
                        .Select(x => new DropdownViewModel { Id = x.Id, Name = x.Package.GetDescription() })
                        .OrderBy(x => x.Name).ToListAsync();

                case DropdownType.UnpaidSubscription:
                    var unpaidSubscriptions = await _db.Subscriptions.Where(x => x.Active && !x.IsPaymentCompleted && x.TenantId == session.TenantId)
                        .Select(x => new DropdownViewModel { Id = x.Id, Name = x.Id, Code  = x.Package.GetDescription() })
                        .OrderBy(x => x.Name).ToListAsync();
                    foreach (var unpaidSubscription in unpaidSubscriptions)
                    {
                        unpaidSubscription.Name = unpaidSubscription.Name + GetPackageName(unpaidSubscription.Code);
                    }
                    return unpaidSubscriptions;
                    
                case DropdownType.Branch:
                    return await _db.Branches.Where(x => x.Active && x.TenantId == session.TenantId)
                        .Select(x => new DropdownViewModel { Id = x.Id, Name = x.Name })
                        .OrderBy(x => x.Name).ToListAsync();

                case DropdownType.Supplier:
                    return await _db.Suppliers.Where(x => x.Active && x.TenantId == session.TenantId)
                        .Select(x => new DropdownViewModel {Id = x.Id, Name = x.Name, Code = x.TotalDue.ToString()})
                        .OrderBy(x => x.Name).ToListAsync();

                case DropdownType.Employee:
                    return await _db.Employees.Where(x => x.Active && x.TenantId == session.TenantId)
                        .Select(x => new DropdownViewModel {Id = x.Id, Name = x.Name, ExtraData = x.MonthlyGrossSalary.ToString()})
                        .OrderBy(x => x.Name).ToListAsync();

                case DropdownType.Customer:
                    return await _db.Customers.Where(x => x.Active && x.TenantId == session.TenantId)
                        .Select(
                            x =>
                                new DropdownViewModel
                                {
                                    Id = x.Id,
                                    Name = x.Name + "(" + x.Code + ")",
                                    Code = x.Code,
                                    ExtraData = x.TotalDue.ToString()
                                })
                        .OrderBy(x => x.Name).ToListAsync();


                case DropdownType.Warehouse:
                    return await _db.Warehouses.Where(x => x.Active && x.TenantId == session.TenantId)
                        .Select(x => new DropdownViewModel {Id = x.Id, Name = x.Name, Code = x.Code})
                        .OrderBy(x => x.Name).ToListAsync();


                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public async Task<List<DropdownViewModel>> GetData(DropdownType type, string id)
        {
            var session = new AppSession();
            switch (type)
            {

                case DropdownType.TenantBranch:
                    return await _db.Branches.Where(x => x.Active && x.TenantId == id)
                        .Select(x => new DropdownViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Code = x.Code
                        })
                        .OrderBy(x => x.Name).ToListAsync();

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }



        public async Task<List<DropdownViewModel>> GetData(DropdownType type, string pid, string cid)
        {            
            List<DropdownViewModel> viewModels = new List<DropdownViewModel>();
            var session = new AppSession();

            switch (type)
            {

                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }



        public void Dispose()
        {
            _db.Dispose();
        }
    }

    
}