using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Session;
using Project.Model;
using Project.ViewModel;

namespace Project.Service.Lookups
{
    public interface ITypeAHeadDataService
    {
        Task<List<TypeAHeadViewModel>> GetData(string request, TypeAHeadType type);
    }

    public class TypeAHeadDataService : IDisposable, ITypeAHeadDataService
    {
        private readonly BusinessDbContext _db = new BusinessDbContext();



        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<List<TypeAHeadViewModel>> GetData(string request, TypeAHeadType type)
        {
            AppSession appSession = new AppSession();
            switch (type)
            {
                case TypeAHeadType.Customer:
                    var customers = await _db.Customers.Where(x => x.TenantId == appSession.TenantId)
                        .Where(x => x.Id == request  || x.Name.StartsWith(request) || x.Phone.Contains(request) || x.Code.Contains(request) || x.Name.Contains(request))
                        .Select(x => new { x.Id, x.Code, x.Name, x.TotalDue, x.Phone })
                        .Take(10).ToListAsync();

                    return customers.ConvertAll(x =>
                        new TypeAHeadViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Code = x.Code,
                            DisplayData = x.Name + " " + x.Code + " " + x.Phone,
                            MetaData = x.TotalDue
                        });

                case TypeAHeadType.Supplier:
                    var suppliers = await _db.Suppliers.Where(x => x.TenantId == appSession.TenantId)
                        .Where(x => x.Id == request || x.Name.StartsWith(request) || x.Phone.Contains(request) ||  x.Code.StartsWith(request) || x.Name.Contains(request))
                        .Select(x => new { x.Id, x.Code, x.Name, x.TotalDue, x.Phone })
                        .Take(10).ToListAsync();

                    return suppliers.ConvertAll(x =>
                        new TypeAHeadViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Code = x.Code,
                            DisplayData = x.Name + " " + x.Code + " " + x.Phone,
                            MetaData = x.TotalDue
                        });


                case TypeAHeadType.Employee:
                    var employees = await _db.Employees.Where(x => x.TenantId == appSession.TenantId)
                        .Where(x => x.Id == request || x.Code.StartsWith(request) || x.Name.Contains(request) || x.Surname.Contains(request))
                        .Select(x => new
                        {
                            x.Id,
                            x.Code,
                            x.Name,
                            x.Surname,
                            x.MonthlyGrossSalary
                        }).Take(10).ToListAsync();


                    return employees.ConvertAll(x =>
                        new TypeAHeadViewModel()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            DisplayData = string.IsNullOrWhiteSpace(x.Code) ? $"{x.Name} {x.Surname}" : $"{x.Name} {x.Surname}({x.Code})",
                            MetaData = x.MonthlyGrossSalary
                        });

                case TypeAHeadType.Partner:
                    var partners = await _db.Partners.Where(x => x.TenantId == appSession.TenantId)
                        .Where(x => x.Id == request || x.Name.StartsWith(request) || x.Name.Contains(request)).Take(10)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.CurrentContribution
                        }).ToListAsync();

                    return partners.ConvertAll(x =>
                        new TypeAHeadViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            DisplayData = x.Name,
                            MetaData = x.CurrentContribution
                        });


            }

            return new List<TypeAHeadViewModel>();
        }
    }
}