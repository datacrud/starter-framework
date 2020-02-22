using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core
{
    public static class AppConst
    {
        public const string Localhost = "localhost";

        public const string LoggerPath = @"\App_Data" + @"\Logs\\Log.log";

        public const string PoweredBy = "Powered by www.datacrud.com";

        public const string TenancyName = "{TENANCY_NAME}";
        public const string AppBaseUrl = "http://{TENANCY_NAME}.datacrud.com";

        public static string GetAppBaseUrl()
        {
            return AppConst.AppBaseUrl.Replace(AppConst.TenancyName, null);
        }
    }
}
