using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Project.Core.Helpers;
using Security.Models.Models;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Project.ViewModel.Session;
using Security.Server.Managers;
using Security.Server.Providers.Sessions;

namespace Security.Server.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly ITenantProvider _tenantProvider;
        private readonly ISessionProvider _sessionProvider;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            _publicClientId = publicClientId;
            _tenantProvider = new TenantProvider();
            _sessionProvider = new SessionProvider();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var header = context.OwinContext.Response.Headers.SingleOrDefault(h => h.Key == "Access-Control-Allow-Origin");
            if (header.Equals(default(KeyValuePair<string, string[]>)))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
            }

            var form = await context.Request.ReadFormAsync();
            var tenancyName = form["tenancyName"];
            if (string.IsNullOrWhiteSpace(tenancyName))
            {
                context.SetError("invalid_company_id", "The company id is incorrect");
                //await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, null, null, null);
                return;
            }

            var tenant = await _tenantProvider.GetTenantByNameAsync(tenancyName);
            if (tenant == null)
            {
                context.SetError("invalid_company_id", "The company id is incorrect");
                //await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, null, null, null);
                return;
            }
            var tenantId = tenant.Id;

            var userManager = context.OwinContext.GetUserManager<UserManager>();
            userManager.SetTenantId(tenantId);

            User user = await userManager.FindAsync(tenantId, context.UserName, context.Password);            
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, null, null, null);
                return;
            }


            //var referer = context.Request.Headers["Referer"];
            //if (referer != null)
            //{
            //    tenancyName = TenantHelper.GetTenantNameFromReferer(referer);
            //}
            if (!string.IsNullOrWhiteSpace(tenancyName) && user.TenantName.ToLower() != tenancyName)
            {
                context.SetError("others_tenant_user", "Login failed! You are not the user of current company.");
                await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, user.TenantId, user.CompanyId, user.Id);
                return;
            }


            if (!user.IsActive)
            {
                context.SetError("user_inactive", "Login failed! You are not an active user. Please contact with administration.");
                await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, user.TenantId, user.CompanyId, user.Id);
                return;
            }

            bool enableEmailConfirmation;
            try
            {
                enableEmailConfirmation = Convert.ToBoolean(ConfigurationManager.AppSettings["Security:IsRequireEmailConfirmation"]);
                if (Convert.ToBoolean(enableEmailConfirmation))
                {
                    var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user.Id);
                    if (!isEmailConfirmed)
                    {
                        context.SetError("email_not_confirmed", "Your email is not confirmed.");
                        await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, user.TenantId, user.CompanyId, user.Id);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                context.SetError("app_setting__read_error", "Failed to read email confirmation checking setting value.");
                await _tenantProvider.SaveLoginAttempt(false, context.UserName, context.ErrorDescription, user.TenantId, user.CompanyId, user.Id);
                return;
            }

            
            //var tenant = await _tenantProvider.GetTenantAsync(user.TenantId);

            if (!tenant.IsActive)
            {
                context.SetError("company_inactive", "Your company is not active yet.");
                await _tenantProvider.SaveLoginAttempt(true, context.UserName, context.ErrorDescription, user.TenantId, user.CompanyId, user.Id);
                return;
            }

            var edition = await _tenantProvider.GetEditionAsync(tenant.EditionId);

            //var anyActiveSubscription = await _tenantProvider.AnyActiveSubscription(user.TenantId);
            //var anyUnpaidSubscription = await _tenantProvider.AnyUnpaidSubscription(user.TenantId);
            //var allUnpaidSubscription = await _tenantProvider.AllUnpaidSubscription(user.TenantId);

            //string lastPaymentStatus = await _tenantProvider.SubscriptionLastPaymentStatus(user.TenantId);

            var session = _sessionProvider.GetCurrentUserSession(user.Id);
            session.Application.IsEnableEmailActivation = enableEmailConfirmation;

            user.LastLoginTime = DateTime.Now;
            await userManager.UpdateAsync(user);

            await _tenantProvider.SaveLoginAttempt(true, context.UserName, context.ErrorDescription, user.TenantId, user.CompanyId, user.Id);

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            //AuthenticationProperties properties = CreateProperties(user.UserName);
            var roles = await userManager.GetRolesAsync(user.Id);
            AuthenticationProperties properties = CreateProperties(session); //user, roles, tenant, edition, anyActiveSubscription, anyUnpaidSubscription, allUnpaidSubscription, lastPaymentStatus, enableEmailConfirmation

        AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);                       
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },

            };
            return new AuthenticationProperties(data);
        }

        public static AuthenticationProperties CreateProperties(SessionViewModel session)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"data", JsonConvert.SerializeObject(session)}
                //{"Id", user.Id},
                //{"FirstName", user.FirstName},
                //{"LastName", user.LastName},
                //{"FullName", user.FirstName + " " + user.LastName},
                //{"Username", user.UserName},
                //{"Email", user.Email},
                //{"Phone", user.PhoneNumber},
                //{"TenantId", user.TenantId},
                //{"TenantName", user.TenantName},
                //{"CompanyId", user.CompanyId},
                //{"BranchId", user.BranchId},
                //{"RoleNames", string.Join(",", roles.Select(x => x))},
                //{"Roles", string.Join(",", roles.Select(x => x))},
                //{"DefaultRole", roles.FirstOrDefault()},
                //{"IsInTrialPeriod", tenant?.IsInTrialPeriod.ToString()},
                //{"TrialDayCount", edition?.TrialDayCount.ToString()},
                //{"SubscriptionEndTime", tenant?.SubscriptionEndTime.GetValueOrDefault().ToShortDateString()},
                //{"WaitingDayAfterExpire", edition?.WaitingDayAfterExpire.ToString()},
                //{"AnyActiveSubscription", anyActiveSubscription.ToString() },
                //{"AnyUnpaidSubscription", anyUnpaidSubscription.ToString() },
                //{"AllUnpaidSubscription", allUnpaidSubscription.ToString() },
                //{"LastPaymentStatus", lastPaymentStatus },
                //{"EnableEmailConfirmation", enableEmailConfirmation },
                //{"Today", DateTime.Today.ToShortDateString() },
            };


            return new AuthenticationProperties(data);
        }
    }
}