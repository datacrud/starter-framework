namespace Project.Core.Session
{
    public class AppSessionModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string EmployeeId { get; set; }
    }
}