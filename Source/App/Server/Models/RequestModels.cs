using System.ComponentModel.DataAnnotations;

namespace Project.Server.Models
{
    public class RequestModels
    {
        public class ChangePasswordRequestModel
        {
            public string CurrentPassword { get; set; }

            public string NewPassword { get; set; }

            public string RetypePassword { get; set; }
        }


        public class UserProfileUpdateRequestModel
        {
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string PhoneNumber { get; set; }
            
             
        }


        public class UserCreateRequestModel
        {
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string PhoneNumber { get; set; }

            [Required]
            public string UserName { get; set; }

            [Required]
            public string PasswordHash { get; set; }

            [Required]
            public string RetypePassword { get; set; }

            public string SecurityStamp { get; set; }

            [Required]
            public string RoleId { get; set; } 
        }


        public class PermissionRequestModel
        {

            public string RoleId { get; set; }

            public string Route { get; set; }

            public string ResourceId { get; set; }

        }
    }
}