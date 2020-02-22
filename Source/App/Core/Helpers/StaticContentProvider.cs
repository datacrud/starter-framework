using System.Configuration;
using System.Web;

namespace Project.Core.Helpers
{
    public static class StaticContentProvider
    {
        public static string ContentDirectory = GetStaticContentRootPath();
        public static string DefaultImage = ContentDirectory + GetDefaultImage();

        private static string GetStaticContentRootPath()
        {
            var readRootPath = HttpContext.Current != null && HttpContext.Current.Request.Url.Host.Contains(AppConst.Localhost)
                ? ConfigurationManager.AppSettings["App:StaticContentRootPath:Dev"]
                : ConfigurationManager.AppSettings["App:StaticContentRootPath:Prod"];
            return readRootPath;
        }

        private static string GetDefaultImage()
        {
            return "Image/default.jpg";
        }

    }
}