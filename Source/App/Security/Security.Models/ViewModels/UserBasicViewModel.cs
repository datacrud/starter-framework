using Security.Models.Models;

namespace Security.Models.ViewModels
{
    public class UserBasicViewModel
    {
        public UserBasicViewModel(User model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}