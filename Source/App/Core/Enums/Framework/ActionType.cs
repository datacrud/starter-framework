using System.ComponentModel;

namespace Project.Core.Enums.Framework
{
    public enum ActionType
    {
        [Description("Create")] Post = 0,
        [Description("Edit")] Put = 1,
        [Description("Trash")] Trash = 3,
        [Description("Delete")] Delete = 4
    }
}