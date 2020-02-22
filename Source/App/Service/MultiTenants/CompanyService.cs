using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Core.Email;
using Project.Core.Extensions;
using Project.Core.Helpers;
using Project.Model;
using Project.Repository;
using Project.Repository.MultiTenants;
using Project.RequestModel;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ICompanyService : IHaveTenantIdServiceBase<Company, CompanyViewModel>
    {
        Task<EmailResponseModel> SendEmailConfirmationCode(EmailConfirmationRequestModel requestModel);
        Task<EmailResponseModel> ConfirmEmail(EmailConfirmationRequestModel requestModel);
    }

    public class CompanyService : HaveTenantIdServiceBase<Company, CompanyViewModel>, ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IEmailService _emailService;

        public CompanyService(ICompanyRepository repository, IEmailService emailService) : base(repository)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public override ResponseModel<CompanyViewModel> GetAll(RequestModelBase<Company> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAll().Include(x=> x.Tenant), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<CompanyViewModel>(entities, Repository.GetAll().Count());
            //
            return response;
        }

        public async Task<EmailResponseModel> SendEmailConfirmationCode(EmailConfirmationRequestModel requestModel)
        {
            var company = requestModel.IsQueryAsTracking
                ? _repository.GetById(requestModel.Id)
                : _repository.AsNoTracking().SingleOrDefault(x => x.Id == requestModel.Id);

            if (company == null) return null;

            EmailResponseModel response = new EmailResponseModel();
            if (string.IsNullOrWhiteSpace(company.EmailConfirmationCode) || requestModel.IsResend)
            {
                company.EmailConfirmationCode = EmailHelper.GenerateVerificationCode();
                company.EmailConfirmationCodeExpireTime = DateTime.Now.AddMinutes(30);

                if (company.IsChangeEmail)
                {
                    await _emailService.SendConfirmationEmailToUser(company.AwaitingConfirmEmail, company.Name,
                        company.EmailConfirmationCode, company.EmailConfirmationCodeExpireTime);
                }
                else if (!company.IsEmailConfirmed)
                {
                    await _emailService.SendConfirmationEmailToUser(company.Email, company.Name,
                        company.EmailConfirmationCode, company.EmailConfirmationCodeExpireTime);
                }

                if (requestModel.IsQueryAsTracking)
                {
                    _repository.EditAsTenant(company);
                    response.IsSuccess = await _repository.CommitAsync();
                }
                else
                {
                    response.ConfirmationCode = company.EmailConfirmationCode;
                    response.ExpireTime = company.EmailConfirmationCodeExpireTime;
                }
            }

            return response;
        }

        public async Task<EmailResponseModel> ConfirmEmail(EmailConfirmationRequestModel requestModel)
        {
            var company = _repository.GetById(requestModel.Id);

            EmailResponseModel response = new EmailResponseModel();

            if (company.EmailConfirmationCodeExpireTime.HasValue &&
                company.EmailConfirmationCodeExpireTime.GetValueOrDefault() < DateTime.Now)
            {
                response.IsSuccess = false;
                response.Message = "Your confirmation code has been expired.";
            }

            else if (string.Equals(company.EmailConfirmationCode, requestModel.ConfirmationCode))
            {
                company.IsEmailConfirmed = true;
                company.EmailConfirmationCode = null;

                if (!string.IsNullOrWhiteSpace(company.AwaitingConfirmEmail))
                    company.Email = company.AwaitingConfirmEmail;

                company.IsChangeEmail = false;
                company.EmailConfirmationCodeExpireTime = null;
                company.AwaitingConfirmEmail = null;

                _repository.EditAsTenant(company);

                await _repository.CommitAsync();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Your confirmation code is not valid";
            }

            return response;
        }
    }
}
