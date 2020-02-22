using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Models.Models
{
    public class Permission : SecurityTenantEntityBase<string>
    {

        [Required]
        public string RoleId { get; set; }
        [ForeignKey("RoleId")] public virtual Role Role { get; set; }


        [Required]
        public string ResourceId { get; set; }
        [ForeignKey("ResourceId")] public virtual Resource Resource { get; set; }

    }
}