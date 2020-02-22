namespace Project.Core.Session
{
    public interface IAppSession
    {
        string UserId { get; }
        string EmployeeId { get; }
        string UserName { get; }
        string UserEmail { get; }
        string TenantId { get; }
        string TenantName { get; }
        string CompanyId { get; }
        string BranchId { get; }
    }
}