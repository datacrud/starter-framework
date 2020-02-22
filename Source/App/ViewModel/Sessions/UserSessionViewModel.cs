using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Session
{
    public class UserSessionViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string CompanyId { get; set; }

        public string BranchId { get; set; }
        public bool IsHeadOfficeBranch { get; set; }
        public string WarehouseId { get; set; }

        public bool IsCompanyAccessLevel { get; set; }
        public bool IsBranchAccess => !IsCompanyAccessLevel;

        public List<string> Roles { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
    }
}
