using System.Collections.Generic;
using Project.Core;

namespace Project.Server.Providers.CorsPolicies
{
    public static class AllowedClient
    {
        public static Dictionary<string, string> GetClients = new Dictionary<string, string>()
        {
            {AppConst.Localhost, $"*,*,*,*" },
            {"host", $"*,*,*,*" },
            {"test", $"*,*,*,*" },
            {"demo", $"*,*,*,*" },
        };
    }
}