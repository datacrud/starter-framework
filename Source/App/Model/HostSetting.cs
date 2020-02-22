using Project.Core.Enums;

namespace Project.Model
{
    public class HostSetting
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public HostSettingValueType ValueType { get; set; }
        public bool IsStatic { get; set; }
    }
}