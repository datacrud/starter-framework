using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Project.Core.Helpers;
using Project.Core.Session;
using Project.Server.Providers;
using Project.ViewModel;

namespace Project.Server.Controllers.FileUpload
{
    [Authorize]
    public class FileUploadController  : ApiController
    {
        //private readonly string _uploadFolder = HttpRuntime.AppDomainAppPath + @"\Upload";        
        //private static readonly string UploadFolder = GetUploadFolder();

        private static string GetUploadFolder()
        {
            var path = HttpRuntime.AppDomainAppPath + @"\Upload";
            CreateDirectory(path);
            return path;
        }

        private static string GetTenantUploadFolder()
        {            
            AppSession appSession = new AppSession();
            var path = Path.Combine(GetUploadFolder(), appSession.TenantName.ToTitleCase());
            CreateDirectory(path);
            return path;
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static string GetTenantReadFolder()
        {
            AppSession appSession = new AppSession();
            var path = Path.Combine(@"/Upload", appSession.TenantName.ToTitleCase());
            return path;
        }

        /// <summary>
        ///   Get all photos
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get()
        {
            var photos = new List<FileViewModel>();

            var photoFolder = new DirectoryInfo(GetTenantUploadFolder());

            await Task.Factory.StartNew(() =>
            {
                var tenantReadFolder = GetTenantReadFolder();

                photos = photoFolder.EnumerateFiles()
                    .Where(fi => new[] { ".jpeg", ".jpg", ".bmp", ".png", ".gif", ".tiff" }
                        .Contains(fi.Extension.ToLower()))
                    .Select(fi => new FileViewModel
                    {
                        Name = fi.Name,
                        LocalFilePath = tenantReadFolder + $"/{fi.Name}",
                        Created = fi.CreationTime,
                        Modified = fi.LastWriteTime,
                        Size = fi.Length / 1024
                    })
                    .ToList();
            });

            return Ok(new { Photos = photos });
        }


        /// <summary>
        ///   Delete photo
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string fileName)
        {
            if (!FileExists(fileName))
            {
                return NotFound();
            }

            try
            {
                var filePath = Directory.GetFiles(GetTenantUploadFolder(), fileName)
                    .FirstOrDefault();

                await Task.Factory.StartNew(() =>
                {
                    if (filePath != null)
                        File.Delete(filePath);
                });

                var result = new FileUploadResponseModel
                {
                    IsSuccess = true,
                    Message = fileName + "deleted successfully"
                };
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                var result = new FileUploadResponseModel
                {
                    IsSuccess = false,
                    Message = "error deleting fileName " + ex.GetBaseException().Message
                };
                return BadRequest(result.Message);
            }
        }


        /// <summary>
        ///   Add a photo
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest("Unsupported media type");
            }
            try
            {
                var provider = new CustomMultipartFormDataStreamProvider(GetTenantUploadFolder());
                //await Request.Content.ReadAsMultipartAsync(provider);
                await Task.Run(async () => await Request.Content.ReadAsMultipartAsync(provider));

                FileUploadResponseModel response = new FileUploadResponseModel();

                var file = provider.FileData.FirstOrDefault();
                if (file != null)
                {
                    var locaFilePath = file.LocalFileName;
                    var fileName = Path.GetFileName(locaFilePath);
                    var extension = Path.GetExtension(locaFilePath);

                    var copyFilePath = GetTenantUploadFolder() + @"\\" + Guid.NewGuid() + extension;
                    if(!string.IsNullOrWhiteSpace(locaFilePath))File.Copy(locaFilePath, copyFilePath);                                                  

                    var fileInfo = new FileInfo(file.LocalFileName);
                    response.FileId = fileInfo.GetFileNameWithoutExtension();
                    response.OriginalFileName = fileName;
                    response.FileName = Path.GetFileName(copyFilePath);
                    response.LocalFilePath = copyFilePath;
                    response.IsSuccess = true;
                    response.Message = "Success";

                    File.Delete(locaFilePath);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }


        /// <summary>
        ///   Check if file exists on disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool FileExists(string fileName)
        {
            var file = Directory.GetFiles(GetTenantUploadFolder(), fileName)
                .FirstOrDefault();

            return file != null;
        }
    }
}

