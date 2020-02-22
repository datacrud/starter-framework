using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Email;
using Project.Core.EmailPublisher;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.Helpers;
using Project.Core.StaticResource;
using Project.Model;
using Project.Repository;
using Project.Repository.MultiTenants;
using Project.RequestModel;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ISubscriptionPaymentService : IHaveTenantIdCompanyIdServiceBase<SubscriptionPayment, SubscriptionPaymentViewModel>
    {
        bool ConfirmPaymentAndUpdateStatus(PaymentConfirmationRequestModel requestModel);
        bool DeleteSubscriptionPayments(string subscriptioId);
        Company GetCompany(string subscriptionId);
        void SendPaymentInvoiceEmail(string paymentId, string companyEmail, string companyName);
        void SendPaymentConfirmationEmail(string subscriptionId, string companyEmail, string companyName);
        void SendPaymentRejectionEmail(string subscriptionId, string companyEmail, string companyName);
    }

    public class SubscriptionPaymentService : HaveTenantIdCompanyIdServiceBase<SubscriptionPayment, SubscriptionPaymentViewModel>, ISubscriptionPaymentService
    {
        private readonly ISubscriptionPaymentRepository _repository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailSender _emailSender;

        public SubscriptionPaymentService(ISubscriptionPaymentRepository repository, 
            ICompanyRepository companyRepository, 
            IEmailSender emailSender) : base(repository)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _emailSender = emailSender;
        }

        public override ResponseModel<SubscriptionPaymentViewModel> GetAll(RequestModelBase<SubscriptionPayment> requestModel)
        {

            var queryable = GetPagingQuery(Repository.GetAll().Include(x=> x.Tenant), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<SubscriptionPaymentViewModel>(entities, Repository.GetAll().Count());
            
            return response;
        }


        public  bool ConfirmPaymentAndUpdateStatus(PaymentConfirmationRequestModel requestModel)
        {
            bool isVerified = false;

            var payment = _repository.GetAll().FirstOrDefault(x =>
                x.SubscriptionId == requestModel.SubscriptionId && x.PaymentStatus == requestModel.PaymentStatus);

            if (payment == null) return false;

            //if (payment.TransectionNumber.Trim() == requestModel.VerificationCode.Trim())
            //{
                payment.PaymentStatus = SubscriptionPaymentStatus.Paid;
                //payment.VerificationCode = requestModel.VerificationCode;
                payment.VerificationCode = payment.TransectionNumber;
                isVerified = true;
            //}
            //else
            //{
            //    payment.PaymentStatus = SubscriptionPaymentStatus.Rejected;
            //    payment.VerificationCode = requestModel.VerificationCode;
            //}

            _repository.EditAsHost(payment);
            _repository.Commit();

            return isVerified;
        }

        public bool DeleteSubscriptionPayments(string subscriptioId)
        {
            var payments = _repository.GetAll().Where(x => x.SubscriptionId == subscriptioId).ToList();

            _repository.Delete(payments);

            return _repository.Commit();
        }

        public Company GetCompany(string subscriptionId)
        {
            var payment = _repository.GetAll().AsNoTracking()
                .FirstOrDefault(x => x.SubscriptionId == subscriptionId);

            if (payment != null)
            {
                var company = _companyRepository.GetAll().AsNoTracking().SingleOrDefault(x=> x.Id == payment.CompanyId);
                return company;
            }

            return null;
        }

        public void SendPaymentInvoiceEmail(string paymentId, string companyEmail, string companyName)
        {
            var payment = _repository.AsNoTracking().SingleOrDefault(x => x.Id == paymentId);

            string subject = $"DataCrud just receive a payment from {companyName}";
            string body = $"Dear {companyName}, <br/>" +
                          $"We have received a BDT{payment.PaidAmount.ToRound2Decimal()} amount of payment from {companyName} on {payment.Date: dd/MM/yyyy HH:mm} by {payment.PaymentMethod.GetDescription()} <br/><br/>" +
                          $"Your payment reference number is <strong>{payment.Id}.</strong> For any further query please use the last 6 digit of this reference number. <br/><br/>" +
                          $"Your payment is waiting for confirmation. This will take 24-48 hours maximum. Our sales team will contact you shortly or you can contact us directory at {SmsHelper.GetSupportNo()} to complete the verification process early to activate your account. We are sorry for waiting for you for payment confirmation.<br/></br/>" +
                          $"Thanks for your patience and having with us." +
                          $"{EmailHelper.Signature.Sales}";
            
            //Attach an invoice with this email

            var task = Task.Run(async () => await _emailSender.SendSecurityEmailAsync(companyEmail, companyName, subject, body));
            task.Wait();
        }

        public void SendPaymentConfirmationEmail(string subscriptionId, string companyEmail, string companyName)
        {
            var payment = _repository.GetAll().FirstOrDefault(x =>
                x.SubscriptionId == subscriptionId && x.PaymentStatus == SubscriptionPaymentStatus.Paid);

            if (payment != null)
            {
                string subject = $"Congratulation, your payment has been confirmed.";
                string body = $"Dear {companyName}, <br/>" +
                              $"We had received a BDT{payment.PaidAmount.ToRound2Decimal()} amount of payment from {companyName} on {payment.Date: dd/MM/yyyy HH:mm} by {payment.PaymentMethod.GetDescription()}. Your payment has been successfully verified bearing reference number {payment.Id}. <br/><br/>" +
                              
                              $"To continue with datacrud please sign out first and than sign in again.<br/></br/>" +
                              $"Thanks for your patience and having with us." +
                              $"{EmailHelper.Signature.Sales}";

                //Attach an invoice with this email

                var task = Task.Run(async () => await _emailSender.SendSecurityEmailAsync(companyEmail, companyName, subject, body));
                task.Wait();
            }

           
        }

        public void SendPaymentRejectionEmail(string subscriptionId, string companyEmail, string companyName)
        {
            var payment = _repository.GetAll().FirstOrDefault(x =>
                x.SubscriptionId == subscriptionId && x.PaymentStatus == SubscriptionPaymentStatus.Rejected);

            if (payment != null)
            {
                string subject = $"Sorry, your payment has been rejected.";
                string body = $"Dear {companyName}, <br/>" +
                              $"We had received a BDT{payment.PaidAmount.ToRound2Decimal()} amount of payment from {companyName} on {payment.Date: dd/MM/yyyy HH:mm} by {payment.PaymentMethod.GetDescription()}. But your payment failed to verify bearing reference number {payment.Id}. <br/><br/>";

                if (payment.PaymentMethod == SubscriptionPaymentMethod.Bkash ||
                    payment.PaymentMethod == SubscriptionPaymentMethod.Rocket)
                {
                    body += $"The trnasection numner <strong>{payment.TransectionNumber}</strong> doesnot match.";
                }


                body += $"For any further query please contact {SmsHelper.GetSupportNo()}.<br/></br/>" +
                        $"Thanks for your patience and having with us." +
                        $"{EmailHelper.Signature.Sales}";

                //Attach an invoice with this email

                var task = Task.Run(async () => await _emailSender.SendSecurityEmailAsync(companyEmail, companyName, subject, body));
                task.Wait();
            }

        }
    }


}
