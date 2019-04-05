using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Server.Models;
using Project.Server.Repository;
using Project.Service;
using static Project.Server.Models.RequestModels;
using static Project.Server.Models.ViewModels;

namespace Project.Server.Service
{
    public interface IProfileService : ISecurityBaseService<ApplicationUser>
    {

        ProfileViewModel GetUserProfile();
        Task<IdentityResult> UpdateProfile(UserProfileUpdateRequestModel model);
        bool UpdatePassword(ChangePasswordRequestModel model);
    }

    public class ProfileService : SecurityBaseService<ApplicationUser>, IProfileService
    {
        private readonly IProfileRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly ApplicationUserManager _manager;

        public ProfileService(IProfileRepository repository, IRoleRepository roleRepository): base(repository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }



        //Profile Section
        public ProfileViewModel GetUserProfile()
        {
            var user = _manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            var viewModel = new ProfileViewModel(user);

            foreach (var userRole in user.Roles.Select(role => _roleRepository.GetById(role.RoleId)))
            {
                viewModel.RoleNames = new[] { userRole.Name };
            }

            return viewModel;
        }


        public Task<IdentityResult> UpdateProfile(UserProfileUpdateRequestModel model)
        {
            var user = _manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            return _repository.UpdateProfile(user);
        }


        public bool UpdatePassword(ChangePasswordRequestModel model)
        {
            var success = false;

            if (model.NewPassword != model.RetypePassword) return false;

            var user = _manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            var verifyHashedPassword = new PasswordHasher().VerifyHashedPassword(user.PasswordHash, model.CurrentPassword);

            if (verifyHashedPassword == PasswordVerificationResult.Success)
            {
                success = _repository.UpdatePassword(model);
            }

            return success;
        }

    }
}