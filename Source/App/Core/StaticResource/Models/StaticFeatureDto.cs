using Project.Core.Enums;

namespace Project.Core.StaticResource.Models
{
    public class StaticFeatureDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public FeatureValueType ValueType { get; set; }
        public bool IsStatic { get; set; } = true;

        public int Order { get; set; }
    }
}