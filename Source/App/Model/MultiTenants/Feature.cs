using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Feature : BusinessEntityBase
    {

        public string Name { get; set; }
        public string Value { get; set; }
        public FeatureValueType ValueType { get; set; }
        public bool IsEnabled { get; set; }

        public bool IsFeature { get; set; }
        public bool IsEditionFeature { get; set; }
        public bool IsTenantFeature { get; set; }

        public bool IsStatic { get; set; }

        public string EditionId { get; set; }
        [ForeignKey("EditionId")]
        public virtual Edition Edition { get; set; }
    }
}