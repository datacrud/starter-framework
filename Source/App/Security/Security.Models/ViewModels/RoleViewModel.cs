using Microsoft.AspNet.Identity.EntityFramework;
using Security.Models.Models;

namespace Security.Models.ViewModels
{
    public class RoleViewModel
    {

        public RoleViewModel(Role model)
        {
            Id = model.Id;
            Name = model.DisplayName;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}