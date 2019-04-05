using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Server.Models
{
    public class ViewModels
    {

        public class ProfileViewModel
        {
            public ProfileViewModel(ApplicationUser model)
            {
                Id = model.Id;
                FirstName = model.FirstName;
                LastName = model.LastName;
                Email = model.Email;
                PhoneNumber = model.PhoneNumber;
                UserName = model.UserName;
                Roles = model.Roles;
            }

            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string PhoneNumber { get; set; }

            public string UserName { get; set; }             

            public ICollection<IdentityUserRole> Roles { get; set; }

            public ICollection<string> RoleNames { get; set; }
        }


        public class RoleViewModel
        {

            public RoleViewModel(IdentityRole model)
            {
                Id = model.Id;
                Name = model.Name;
            }

            public string Id { get; set; }

            public string Name { get; set; }
        }

    }
}