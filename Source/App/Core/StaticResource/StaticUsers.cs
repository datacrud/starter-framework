namespace Project.Core.StaticResource
{
    public static class StaticUsers
    {
        public const string Systemadmin = "systemadmin";
        public const string Superadmin = "superadmin";
        public const string Admin = "admin";


        public static bool IsReservedUsername(this string username)
        {
            username = username.ToLower();
            return username == Systemadmin || username == Superadmin || username == Admin;
        }
    }

}