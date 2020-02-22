using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.Session;
using Project.Core.StaticResource;
using Project.Model;
using Project.ViewModel;

namespace Project.Service
{
    public interface IDashboardDataService
    {
        DashboardViewModel GetData();
        List<DashboardAlertViewModel> CheckUserAlert();
    }

    public class DashboardDataService : IDashboardDataService, IDisposable
    {
        private readonly BusinessDbContext _db;
        private IAppSession _appSession;

        public DashboardDataService()
        {
            _db = new BusinessDbContext();
        }

        public DashboardViewModel GetData()
        {
            _appSession = new AppSession();

            DashboardViewModel viewModel = new DashboardViewModel
            {
                SaleToday = 0,
                Cash = 0,
                Bank = 0,
                PurchaseToday = 0,
                DepositToday = 0,
                ExpenseToday = 0,


                DashboardAlerts =  new List<DashboardAlertViewModel>()
            };

            viewModel.DashboardAlerts.AddRange(CheckWarehouseAlert());
            viewModel.DashboardAlerts.AddRange(CheckCompanySetupAlert());
            viewModel.DashboardAlerts.AddRange(CheckHostingExpiration());
            viewModel.DashboardAlerts.AddRange(CheckSubscriptionExpiration());
            viewModel.DashboardAlerts.AddRange(CheckEmployeeAlert());
            viewModel.DashboardAlerts.AddRange(CheckSupplierAlert());

            viewModel.DashboardAlerts = viewModel.DashboardAlerts.OrderBy(x => x.AlertType).ToList();

            return viewModel;
        }

        private List<DashboardAlertViewModel> CheckWarehouseAlert()
        {
            List<DashboardAlertViewModel> viewModels = new List<DashboardAlertViewModel>();

            var warehouses = _db.Warehouses.Where(x => x.Active && x.TenantId == _appSession.TenantId).ToList();
            if (warehouses.Count == 0)
            {
                var message = $"You do not add you warehouses/stocks. You can add you warehouses from here <a class='btn btn-xs blue' href='{StaticResource.Private.Stock.Warehouse.Path.PrefixAngularHashUri()}'>Add Warehouse</a>";

                viewModels.Add(new DashboardAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "Add Warehouse: ",
                    AlertType = DashboardAlertType.Danger,
                    AlertClass = AlertClass.Danger,
                    Message = message
                });
            }

            if (warehouses.Count == 1 && warehouses.FirstOrDefault()?.Type == WarehouseType.Showroom)
            {
                var message = $"You have added your showroom type warehouse only. You can add you others type warehouse from here. <a class='btn btn-xs blue' href='{StaticResource.Private.Stock.Warehouse.Path.PrefixAngularHashUri()}'>Add Warehouse</a>";

                viewModels.Add(new DashboardAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "Add Warehouse: ",
                    AlertType = DashboardAlertType.Info,
                    AlertClass = AlertClass.Info,
                    Message = message
                });
            }

            return viewModels;
        }

        private List<DashboardAlertViewModel> CheckCompanySetupAlert()
        {
            List<DashboardAlertViewModel> viewModels = new List<DashboardAlertViewModel>();

            var branches = _db.Branches.Where(x => x.Active && x.TenantId == _appSession.TenantId).ToList();
            if (branches.Count == 0)
            {
                var message = $"You do not add you showrooms yet. Add you showrooms from company setup. <a class='btn btn-xs blue' href='{StaticResource.Private.Administration.Company.Path.PrefixAngularHashUri()}/{_appSession.CompanyId}'>Go to company setup</a>";

                viewModels.Add(new DashboardAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "Company Setup: ",
                    AlertType = DashboardAlertType.Danger,
                    AlertClass = AlertClass.Danger,
                    Message = message
                });
            }

            if (branches.Count == 1 && branches.FirstOrDefault()?.Type == BranchType.HeadOffice )
            {
                var tenant = _db.Tenants.Find(_appSession.TenantId);
                var feature = _db.Features.FirstOrDefault(x =>
                    x.EditionId == tenant.EditionId && x.Name == StaticFeature.Showroom.Name);
                if (feature != null && Convert.ToInt32(feature.Value) > 1)
                {
                    var message = $"You have added your main showroom only. You can add you others showroom from company setup. <a class='btn btn-xs blue' href='{StaticResource.Private.Administration.Company.Path.PrefixAngularHashUri()}/{_appSession.CompanyId}'>Go to company setup</a>";

                    viewModels.Add(new DashboardAlertViewModel()
                    {
                        IsShow = true,
                        Identity = 1,
                        Title = "Company Setup: ",
                        AlertType = DashboardAlertType.Info,
                        AlertClass = AlertClass.Info,
                        Message = message
                    });
                }

               
            }

            return viewModels;
        }

      
        private List<DashboardAlertViewModel> CheckSupplierAlert()
        {
            List<DashboardAlertViewModel> viewModels = new List<DashboardAlertViewModel>();

            var suppliers = _db.Suppliers.Where(x => x.Active && x.TenantId == _appSession.TenantId).ToList();
            if (suppliers.Count == 0)
            {
                var message = $"You do not add your suppliers yet. You can add your suppliers from here. <a class='btn btn-xs blue' href='{StaticResource.Private.Purchase.Supplier.Path.PrefixAngularHashUri()}'>Add Supplier</a>";

                viewModels.Add(new DashboardAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "Add Supplier: ",
                    AlertType = DashboardAlertType.Danger,
                    AlertClass = AlertClass.Danger,
                    Message = message
                });
            }

            return viewModels;
        }

       
        public List<DashboardAlertViewModel> CheckUserAlert()
        {
            throw new NotImplementedException();
        }

        private List<DashboardAlertViewModel> CheckEmployeeAlert()
        {
            List<DashboardAlertViewModel> viewModels = new List<DashboardAlertViewModel>();

            var employee = _db.Employees.Where(x=> x.Active && x.TenantId == _appSession.TenantId).ToList();
            if (employee.Count == 0)
            {
                var message = $"You do not add your employees yet. You can add your employees from here. <a class='btn btn-xs blue' href='{StaticResource.Private.Hr.Employee.Path.PrefixAngularHashUri()}'>Add Employee</a>";

                viewModels.Add(new DashboardAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "Add Your Employees: ",
                    AlertType = DashboardAlertType.Danger,
                    AlertClass = AlertClass.Danger,
                    Message = message
                });
            }
            else if (employee.Count == 1)
            {
                var message = $"You have added your one employee. You can add your others employees  from here. <a class='btn btn-xs blue' href='{StaticResource.Private.Hr.Employee.Path.PrefixAngularHashUri()}'>Create Employee</a>";

                viewModels.Add(new DashboardAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "Add More Employees: ",
                    AlertType = DashboardAlertType.Info,
                    AlertClass = AlertClass.Info,
                    Message = message
                });
            }

            return viewModels;
        }


        private List<DashboardAlertViewModel> CheckSubscriptionExpiration()
        {
            List<DashboardAlertViewModel> viewModels = new List<DashboardAlertViewModel>();

            var tenant = _db.Tenants.Find(_appSession.TenantId);
            if (tenant?.SubscriptionEndTime != null)
            {
                var trialText = tenant.IsInTrialPeriod ? "trial" : "";
                var compare = DateTime.Compare(DateTime.Now.AddDays(7), tenant.SubscriptionEndTime.GetValueOrDefault());
                
                if (compare >= 0)
                {
                    var message = tenant.SubscriptionEndTime.GetValueOrDefault().Date == DateTime.Today
                        ? $"Today is the last day of your {trialText} subscription. Tomorrow you need to upgrade your subscription. Tomorrow system will redirect to upgrade subscription page."
                        : $"Your {trialText} subscription will expire on {tenant.SubscriptionEndTime.GetValueOrDefault().Date:dd/MM/yyyy}.";
                    message +=
                        $" <a class='btn btn-xs red' href='{StaticResource.Private.MultiTenant.ManageSubscription.Path.PrefixAngularHashUri()}'>View Subscription</a>";

                    viewModels.Add(new DashboardAlertViewModel()
                    {
                        IsShow = true,
                        Identity = 1,
                        Title = "Subscription Expire Notification: ",
                        AlertType = DashboardAlertType.Danger,
                        AlertClass = AlertClass.Danger,
                        Message =
                       message
                    });
                }
            }

            return viewModels;
        }

        private List<DashboardAlertViewModel> CheckHostingExpiration()
        {
            List<DashboardAlertViewModel> viewModels = new List<DashboardAlertViewModel>();

            var companySettings = _db.CompanySettings.FirstOrDefault(x=> x.CompanyId == _appSession.CompanyId);

            if (companySettings != null && companySettings.HasYearlyHostingCharge && companySettings.HostingValidTill != null)
            {
                var message = companySettings.HostingValidTill.GetValueOrDefault() < DateTime.Today
                    ? $"Your hosting has been expired on {companySettings.HostingValidTill.Value.Date: dd/MM/yyyy}. Please renew you hosting to continue access."
                    : $"Your hosting will expire on {companySettings.HostingValidTill.Value.Date: dd/MM/yyyy}. Please renew you hosting before {companySettings.HostingValidTill.Value.Date: dd/MM/yyyy} to get non stop access.";

                var compare = DateTime.Compare(DateTime.Now.AddDays(30), companySettings.HostingValidTill.GetValueOrDefault());
                if (compare >= 0)
                {
                    viewModels.Add(new DashboardAlertViewModel()
                    {
                        IsShow = true,
                        Identity = 1,
                        Title = "Hosting Expire Notification: ",
                        AlertType = DashboardAlertType.Danger,
                        AlertClass = AlertClass.Danger,
                        Message =
                            message
                    });
                }
            }

            return viewModels;
        }

  


        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
