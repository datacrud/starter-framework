using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Project.Repository;
using Project.Server.Models;
using static Project.Server.Models.RequestModels;

namespace Project.Server.Repository
{
    public interface IUserRepository : ISecurityBaseRepository<ApplicationUser>
    {        
        IQueryable<ApplicationUser> GetUsers();
        IQueryable<ApplicationUser> GetUsersInRole(string roleId);
        ApplicationUser GetUser(string id);

        Task<IdentityResult> CreateUser(ApplicationUser applicationUser);        
        Task<IdentityResult> UpdateUser(ApplicationUser applicationUser);
        Task<IdentityResult> DeleteUser(ApplicationUser applicationUser);

        IdentityResult UpdateUserRole(string userId, string oldRoleId, string newRoleId);        
    }


    public class UserRepository : SecurityBaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ApplicationUserManager _manager;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;            
            _manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }


//User Section
        public IQueryable<ApplicationUser> GetUsers()
        {
            return _manager.Users.AsQueryable();
        }

        public IQueryable<ApplicationUser> GetUsersInRole(string roleId)
        {
            return _manager.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId)).AsQueryable();
        }

        public ApplicationUser GetUser(string id)
        {
            return _db.Users.Find(id);
        }




        public Task<IdentityResult> CreateUser(ApplicationUser applicationUser)
        {
            return _manager.CreateAsync(applicationUser);
        }
        

        public Task<IdentityResult> UpdateUser(ApplicationUser applicationUser)
        {
            return _manager.UpdateAsync(applicationUser);
        }

        public Task<IdentityResult> DeleteUser(ApplicationUser applicationUser)
        {
            return _manager.DeleteAsync(applicationUser);
        }




        public IdentityResult UpdateUserRole(string userId, string oldRoleId, string newRoleId)
        {
            var oldRole = _db.Roles.Find(oldRoleId);
            var newRole = _db.Roles.Find(newRoleId);

            
            if(oldRole != null) _manager.RemoveFromRole(userId, oldRole.Name);
            return _manager.AddToRole(userId, newRole.Name);
        }
        
    }
}