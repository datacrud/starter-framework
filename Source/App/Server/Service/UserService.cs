using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Project.Server.Models;
using Project.Server.Repository;
using Project.Service;
using static Project.Server.Models.RequestModels;

namespace Project.Server.Service
{
    public interface IUserService : ISecurityBaseService<ApplicationUser>
    {
        List<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string id);

        Task<IdentityResult> CreateUser(UserCreateRequestModel model);        
        Task<IdentityResult> UpdateUser(UserCreateRequestModel model);
        Task<IdentityResult> DeleteUser(string id);
    }


    public class UserService: SecurityBaseService<ApplicationUser>,IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly ApplicationUserManager _manager;

        public UserService(IUserRepository repository, IRoleRepository roleRepository) : base(repository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }


//User Section
        public List<ApplicationUser> GetUsers()
        {            
            var users = _repository.GetUsers().ToList();

            var isInSystemAdminRole = HttpContext.Current.User.IsInRole("SystemAdmin");
            if (isInSystemAdminRole) return users;

            var roles = _roleRepository.GetAll();
            var systemAdminRole = roles.First(x => x.Name.ToLower() == "SystemAdmin".ToLower());

            var systemAdminRoleUsers = _repository.GetUsersInRole(systemAdminRole.Id).ToList();

            foreach (var user in systemAdminRoleUsers)
            {
                users.Remove(user);
            }

            return users;
        }


        public ApplicationUser GetUser(string id)
        {
            return _repository.GetUser(id);
        }




        public Task<IdentityResult> CreateUser(UserCreateRequestModel model)
        {
            if (model.PasswordHash != model.RetypePassword) return _repository.CreateUser(null);

            var id = Guid.NewGuid().ToString();
            var applicationUser = new ApplicationUser()
            {
                Id = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                PasswordHash = new PasswordHasher().HashPassword(model.PasswordHash),
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = {new IdentityUserRole() {RoleId = model?.RoleId, UserId = id}}
            };

            return _repository.CreateUser(applicationUser);
        }        


        public Task<IdentityResult> UpdateUser(UserCreateRequestModel model)
        {            
            var user = _manager.FindById(model.Id);

            var oldRoleId = user.Roles.SingleOrDefault()?.RoleId;
            if (oldRoleId != model.RoleId) _repository.UpdateUserRole(user.Id, oldRoleId, model.RoleId);

            if (model.PasswordHash != model.RetypePassword) return _repository.UpdateUser(user);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            if(!string.IsNullOrEmpty(model.PasswordHash))
                user.PasswordHash = new PasswordHasher().HashPassword(model.PasswordHash);

            return _repository.UpdateUser(user);
        }


        public Task<IdentityResult> DeleteUser(string id)
        {            
            var user = _manager.FindById(id);
            return _repository.DeleteUser(user);
        }
    }
}