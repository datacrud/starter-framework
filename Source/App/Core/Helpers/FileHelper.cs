using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Project.Core.Extensions;
using Project.Core.Session;

namespace Project.Core.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// Return file upload read root path like http://{ServerRootAddress}/Upload/
        /// </summary>
        public static string UploadDirectory = GetUploadDirectory();
        /// <summary>
        /// Return file upload root path like http://{ServerRootAddress}/Upload/{TenancyName}
        /// </summary>
        public static string TenantUploadDirectory = GetTenantUploadDirectory();
        /// <summary>
        /// Return tenant uploaded files read root path like http://{ServerRootAddress}/Upload/{TenancyName}
        /// </summary>
        public static string TenantReadPath = GetTenantReadPath();
        /// <summary>
        /// Return file upload read root path like http://{ServerRootAddress}/Upload/
        /// </summary>
        public static string ReadRootPath = GetReadRootPath();

        //public static string LogoUploadDirectory = Path.Combine(UploadDirectory, "Logo/");
        //public static string ImageUploadDirectory = Path.Combine(UploadDirectory, "Image/");



        public static string ExportDirectory = "/Export/";        
        public static string PdfExportDirectory = Path.Combine(ExportDirectory, "Pdf/");
        public static string ExcelExportDirectory = Path.Combine(ExportDirectory, "Excel/");
        public static string CsvExportDirectory = Path.Combine(ExportDirectory, "Csv");
        


        private static string GetUploadDirectory()
        {
            var path = HttpRuntime.AppDomainAppPath + @"\Upload";
            CreateDirectoryIfNotExist(path);
            return path;
        }

        private static string GetTenantUploadDirectory()
        {
            string path;
            if (HttpContext.Current?.User != null)
            {
                AppSession appSession = new AppSession();
                var tenancyName = appSession.TenantName.ToCapitalize();
                path = Path.Combine(GetUploadDirectory(), tenancyName);
            }
            else
            {
                path = GetUploadDirectory();
            }
            CreateDirectoryIfNotExist(path);

            return path;
        }


        private static string GetTenantReadPath()
        {
            string tenantReadPath = null;
            if (HttpContext.Current?.User != null)
            {
                AppSession appSession = new AppSession();
                tenantReadPath = appSession.TenantName.ToCapitalize() + "/";
            }

            return tenantReadPath;
        }

        private static string GetReadRootPath()
        {
            var uploadReadRootPath = HttpContext.Current != null && HttpContext.Current.Request.Url.Host.Contains(AppConst.Localhost)
                ? ConfigurationManager.AppSettings["Filing:UploadReadRootPath:Dev"]
                : ConfigurationManager.AppSettings["Filing:UploadReadRootPath:Prod"];
            return uploadReadRootPath;
        }

        

        public static bool IsExist(string file)
        {
            if (File.Exists(file)) return true;
            return false;
        }

        public static string CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static string GetPdfFileName(string fileName)
        {
            return fileName + "_" + DateTime.Now.Day + DateTime.Now.ToString("MMM") + DateTime.Now.Year + ".pdf";
        }

        public static string GetCsvFileName(string fileName)
        {
            return fileName + "_" + DateTime.Now.Day + DateTime.Now.ToString("MMM") + DateTime.Now.Year + ".csv";
        }

        public static string GetExcelFileName(string fileName)
        {
            return fileName + "_" + DateTime.Now.Day + DateTime.Now.ToString("MMM") + DateTime.Now.Year + ".xlsx";
        }

        public static string GetPdfFileName(string fileName, DateTime from, DateTime to)
        {
            return fileName + "_" + from.Day + from.ToString("MMM") +
                   from.Year + "-" + to.Day + to.ToString("MMM") +
                   to.Year + ".pdf";
        }

        public static string GetCsvFileName(string fileName, DateTime from, DateTime to)
        {
            return fileName + "_" + from.Day + from.ToString("MMM") +
                   from.Year + "-" + to.Day + to.ToString("MMM") +
                   to.Year + ".csv";
        }

        public static string GetExcelFileName(string fileName, DateTime from, DateTime to)
        {
            return fileName + "_" + from.Day + from.ToString("MMM") +
                   from.Year + "-" + to.Day + to.ToString("MMM") +
                   to.Year + ".xlsx";
        }
    }
}
