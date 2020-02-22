using System;
using System.Linq;
using EntityFramework.DynamicFilters;
using Project.Core.Enums.Framework;
using Project.Core.Session;
using Project.Model;

namespace Project.Service.AutoGenData
{
    public class AutoGenDataService : IAutoGenDataService, IDisposable
    {
        private readonly BusinessDbContext _db;
        private AppSession _appSession;

        public AutoGenDataService(BusinessDbContext db)
        {
            _db = db;
        }


        public string GetData(AutoGenType type)
        {
            _appSession = new AppSession();

            _db.DisableAllFilters();
            string invoiceNumber;
            switch (type)
            {
                case AutoGenType.CustomerCode:
                    invoiceNumber = AutoGenCustomerCode();
                    break;
                case AutoGenType.EmployeeCode:
                    invoiceNumber = AutoGenEmployeeCode();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            _db.EnableAllFilters();

            return invoiceNumber;
        }


        private string AutoGenEmployeeCode()
        {
            var company = _db.Companies.AsNoTracking().FirstOrDefault(x => x.TenantId == _appSession.TenantId);

            string prefix = "EMP";
            if (company != null)
            {
                var strings = company.Name.Split(' ');
                if (strings.Any())
                {
                    prefix = "";
                    foreach (var s in strings)
                    {
                        var c = s[0];
                        prefix = $"{prefix}{c}";
                    }
                }
            }

            var employee = _db.Employees.AsNoTracking().Where(x => x.TenantId == _appSession.TenantId)
                .OrderByDescending(x => x.Code).FirstOrDefault();
            if (employee?.Code == null) return GetAutoGenData(prefix, 1);

            string[] split = employee.Code.Split('-');
            return GetAutoGenData(prefix, Convert.ToInt32(split[1]) + 1);
        }

        private string AutoGenCustomerCode()
        {
            Customer customer = _db.Customers.Where(x => x.TenantId == _appSession.TenantId).OrderByDescending(
               x => x.Code).FirstOrDefault();
            if (customer == null) return GetAutoGenData("C", 1);

            string[] split = customer.Code.Split('-');
            return GetAutoGenData("C", Convert.ToInt32(split[1]) + 1);
        }



        public static string GetAutoGenData(string prefix, int identity)
        {
            var postfix = string.Empty;

            for (var i = 0; i < 5 - identity.ToString().Length; i++)
            {
                postfix = string.Concat("0", postfix);
            }
            postfix = string.Concat(postfix, identity.ToString());
            postfix = string.Concat("-", postfix);

            return string.Concat(prefix, postfix);
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
