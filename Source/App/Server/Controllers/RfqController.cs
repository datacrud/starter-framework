using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Project.Core.Email;
using Project.Core.EmailPublisher;
using Project.Core.Helpers;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Rfq")]
    public class RfqController : ControllerBase<Rfq, RfqViewModel, RfqRequestModel>
    {
        private readonly IRfqService _service;
        private readonly IEmailSender _emailSender;

        public RfqController(IRfqService service, IEmailSender emailSender) : base(service)
        {
            _service = service;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        public override IHttpActionResult Post(Rfq model)
        {
            string subject = $"A RFQ is awaiting for response";
            string body = $"A RFQ has been submitted by <br/><br/> " +
                          $"Name: {model.YourName}<br/>" +
                          $"Company Name: {model.CompanyName}<br/>" +
                          $"Email Address: {model.EmailAddress}<br/>" +
                          $"Phone Number: {model.PhoneNumber}<br/><br/><br/>";

            body += $"<strong>Company Data</strong><br/><br/>" +
                    $"Total Users: {model.TotalUsers}<br/>" +
                    $"Total Branch: {model.TotalBranch}<br/>" +
                    $"Total Monthly Email Notification: {model.TotalMonthlyEmailNotification}<br/>" +
                    $"Write more about your requirement: {model.WhatINeed}<br/><br/><br/>" +
                    $"How you know about us: {model.HowYouKnowAboutUs + " || " + model.HowYouKnowAboutUsMessage}<br/>" +
                    $"Comment: {model.Comment}";

            var task = Task.Run(async () => await _emailSender.SendEmailAsync(model.EmailAddress, model.YourName,
                "sabbir@datacrud.com",
                "Md. Sabbir Ahamed", subject, body));
            task.Wait();

            return base.Post(model);
        }

        public override IHttpActionResult Put(Rfq model)
        {
            if (!model.ResponseMessage.IsNullOrWhiteSpace())
            {
                string subject = $"DataCrud RFQ Response";
                string body =
                    $"Hi, {model.YourName} <br/><br/> " +
                    $"We have received a RFQ request from you at {model.Created:dd/MM/yyyy}.<br/><br/><br/>";

                body += $"<strong>Here is your company data we have received.</strong><br/><br/>" +
                        $"Total Users: {model.TotalUsers}<br/>" +
                        $"Total Branch: {model.TotalBranch}<br/>" +
                        $"Total Monthly Email Notification: {model.TotalMonthlyEmailNotification}<br/>" +
                        $"Others Requirements: {model.WhatINeed}<br/><br/><br/>";

                body +=
                    $"<strong>Thanks you for contact with us. We are pleased to write to you. Please take look at your offers from us.</strong><br>" +
                    $"{model.ResponseMessage}";

                var task = Task.Run(async () => await _emailSender.SendEmailAsync("sabbir@datacrud.com", EmailHelper.EmailAddress.Sales.Name,
                    model.EmailAddress, model.YourName, subject, body));
                task.Wait();
            }

            return base.Put(model);
        }
    }
}
