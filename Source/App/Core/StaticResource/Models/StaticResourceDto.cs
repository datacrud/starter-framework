using Project.Core.Enums;
using Project.Core.Enums.Framework;

namespace Project.Core.StaticResource.Models
{
    public class StaticResourceDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string State { get; set; }
        public string ParentState { get; set; }
        public string Path { get; set; }
        public bool IsPublic { get; set; }
        public int? Order { get; set; }
        public ResourceOwner ResourceOwner { get; set; }


        public StaticResourceDto(ResourceOwner resourceOwner, string name, string displayName, string state, string path = null, bool isPublic = false,
            int? order = null)
        {
            ResourceOwner = resourceOwner;
            Name = name;
            DisplayName = displayName;
            State = state;
            Path = path;
            IsPublic = isPublic;
            Order = order;

            if (string.IsNullOrWhiteSpace(path))
            {
                var p = string.Empty;
                var split = state.Split('.');
                foreach (var s in split)
                {
                    if (s.ToLower() == "root") continue;

                    p = p + $"/{s}";
                }

                Path = p;
            }
        }
    }
}