using System.ComponentModel.DataAnnotations;

namespace Security.Models.RequestModels
{
    public class ForgetPasswordRequestModel
    {
        [Required]
        public string Email { get; set; }

        //[Required]
        public string Domain { get; set; }
    }
}