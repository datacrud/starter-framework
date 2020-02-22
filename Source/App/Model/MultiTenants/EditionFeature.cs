using System.ComponentModel.DataAnnotations.Schema;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class EditionFeature : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public string EditionId { get; set; }
        [ForeignKey("EditionId")]
        public virtual Edition Edition { get; set; }

        public string FeatureId { get; set; }
        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }
    }
}