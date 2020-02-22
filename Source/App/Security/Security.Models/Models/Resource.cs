using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.DomainBase;
using Project.Core.Enums.Framework;

namespace Security.Models.Models
{
    public class Resource : SecurityTenantEntityBase<string>
    {

        [Required]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public string Route { get; set; }

        public string ParentRoute { get; set; }


        public bool IsPublic { get; set; }

        public int? Order { get; set; }

        public ResourceOwner ResourceOwner { get; set; } = ResourceOwner.Host;

    }
}