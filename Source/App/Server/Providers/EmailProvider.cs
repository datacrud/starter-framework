using System.Threading.Tasks;
using Project.BindingModel;
using Project.BindingModel.Emailing;
using Project.Service;
using Security.Server.Service;

namespace Project.Server.Providers
{
    public interface IEmailProvider
    {
        Task<SenderEmailModel> ProvideSenderEmailModel(string userId);
    }


    public class EmailProvider : IEmailProvider
    {
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;
        private readonly ICompanyService _companyService;

        public EmailProvider(IUserService userService,
            IBranchService branchService,
            ICompanyService companyService)
        {
            _userService = userService;
            _branchService = branchService;
            _companyService = companyService;
        }

        public async Task<SenderEmailModel> ProvideSenderEmailModel(string userId)
        {
            var applicationUser = await _userService.GetUserAsNoTrackingAsync(userId);

            var emailModel = new SenderEmailModel
            {
                UserId = applicationUser.Id,
                Username = applicationUser.UserName,
                FullName = applicationUser.FirstName + " " + applicationUser.LastName,
                EmailAddress = applicationUser.Email,
                Sender = userId
            };

            var branch = _branchService.GetById(applicationUser.BranchId);
            emailModel.BranchCode = branch.Code;
            emailModel.BranchName = branch.Name;

            var company = _companyService.GetEntityById(applicationUser.CompanyId);
            emailModel.TenantId = company.TenantId;
            emailModel.CompanyId = company.Id;
            emailModel.CompanyName = company?.Name;
            emailModel.CompanyEmailAddress = company?.Email;
            emailModel.Receiver = "Company";
            emailModel.IsReceiverEmailVerified = company.IsEmailConfirmed;

            return emailModel;
        }
    }
}