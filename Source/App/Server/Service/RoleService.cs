using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Project.Server.Models;
using Project.Server.Repository;
using Project.Service;

namespace Project.Server.Service
{
    public interface IRoleService : ISecurityBaseService<IdentityRole>
    {
        
    }



    public class RoleService : SecurityBaseService<IdentityRole>, IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository) :base (repository)
        {
            _repository = repository;
        }

        public override List<IdentityRole> GetAll()
        {
            var roles = _repository.GetAll().ToList();

            var isInRole = HttpContext.Current.User.IsInRole("SystemAdmin");
            if (isInRole) return roles;

            foreach (var role in roles.Where(role => role.Name.ToLower() == "SystemAdmin".ToLower()))
            {
                roles.Remove(role);
                break;
            }

            return roles;
        }
    }
}