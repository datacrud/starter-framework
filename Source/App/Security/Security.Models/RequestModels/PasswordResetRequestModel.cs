using System.ComponentModel.DataAnnotations;

namespace Security.Models.RequestModels
{
    public class PasswordResetRequestModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}