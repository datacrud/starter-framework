using System.Linq;
using Project.Core.Enums.Framework;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IEmployeeService : IHaveTenantIdCompanyIdBranchIdServiceBase<Employee, EmployeeViewModel>
    {
        bool IsEmployeeCodeExist(string code, string id, ActionType actionType);
        bool IsEmployeeNameExist(string name, string id, ActionType actionType);
    }

    public class EmployeeService : HaveTenantIdCompanyIdBranchIdServiceBase<Employee, EmployeeViewModel>, IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        public EmployeeService(IEmployeeRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public bool IsEmployeeCodeExist(string code, string id, ActionType actionType)
        {
            var employee = _repository.AsNoTracking().FirstOrDefault(x => x.Code == code);
            if (employee != null && employee.Id != id) return true;

            return false;
        }

        public bool IsEmployeeNameExist(string name, string id, ActionType actionType)
        {
            var employee = _repository.AsNoTracking().FirstOrDefault(x => x.Name == name);
            if (employee != null && employee.Id != id) return true;

            return false;
        }
    }
}
