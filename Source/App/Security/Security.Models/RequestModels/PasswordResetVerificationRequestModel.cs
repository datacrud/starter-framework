using System.ComponentModel.DataAnnotations;

namespace Security.Models.RequestModels
{
    public class PasswordResetVerificationRequestModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}