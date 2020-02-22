namespace Security.Models.RequestModels
{
    public class PermissionRequestModel
    {

        public string RoleId { get; set; }

        public string Route { get; set; }

        public string ResourceId { get; set; }

    }
}