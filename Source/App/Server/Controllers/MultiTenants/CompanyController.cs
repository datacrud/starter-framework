using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Core.Helpers;
using Project.Core.StaticResource;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Server.Providers;
using Project.Service;
using Project.Service.MultiTenants;
using Project.ViewModel;

namespace Project.Server.Controllers.MultiTenants
{
    [Authorize]
    [RoutePrefix("api/Company")]
    public class CompanyController : HaveTenantIdControllerBase<Company, CompanyViewModel, CompanyRequestModel>
    {
        private readonly ICompanyService _service;
        private readonly ICompanySettingsService _companySettingsService;


        public CompanyController(ICompanyService service, ICompanySettingsService companySettingsService) : base(service)
        {
            _service = service;
            _companySettingsService = companySettingsService;
        }


        [HttpPost]
        [Route("UploadLogo")]
        public async Task<IHttpActionResult> UploadLogo()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest("Unsupported media type");
            }
            try
            {
                var provider = new CustomMultipartFormDataStreamProvider(FileHelper.TenantUploadDirectory);
                //await Request.Content.ReadAsMultipartAsync(provider);
                await Task.Run(async () => await Request.Content.ReadAsMultipartAsync(provider));

                FileUploadResponseModel response = new FileUploadResponseModel();

                var file = provider.FileData.FirstOrDefault();                
                if (file != null)
                {                    

                    var locaFilePath = file.LocalFileName;
                    var fileName = Path.GetFileName(locaFilePath);
                    var extension = Path.GetExtension(locaFilePath);


                    var fileId = Guid.NewGuid();                                        
                    var copyFilePath = FileHelper.TenantUploadDirectory + @"\\" + fileId + extension;
                    if (!string.IsNullOrWhiteSpace(locaFilePath)) File.Copy(locaFilePath, copyFilePath);

                    //var fileInfo = new FileInfo(file.LocalFileName);
                    //using (Image img = Image.FromFile(locaFilePath))
                    //using (Bitmap bmp = new Bitmap(img))
                    //{
                    //    for (int x = 0; x < img.Width; x++)
                    //    {
                    //        for (int y = 0; y < img.Height; y++)
                    //        {
                    //            Color c = bmp.GetPixel(x, y);
                    //            if (c.R == 255 && c.G == 255 && c.B == 255)
                    //                bmp.SetPixel(x, y, Color.FromArgb(0));
                    //        }
                    //    }

                    //    copyFilePath = copyFilePath + ".png";
                    //    bmp.Save(copyFilePath, ImageFormat.Png);
                    //}


                    response.FileId = Path.GetFileNameWithoutExtension(copyFilePath);
                    response.OriginalFileName = fileName;
                    response.FileName = Path.GetFileName(copyFilePath);
                    response.FileType = file.Headers.ContentType;
                    response.LocalFilePath = FileHelper.TenantReadPath + response.FileName;
                    response.IsSuccess = true;
                    response.Message = "Success";

                    if (locaFilePath != null) File.Delete(locaFilePath);

                    var company = _service.GetEntityById(User.Identity.GetCompanyId());
                    company.LogoName = response.OriginalFileName;
                    company.LogoFileType = response.FileType.ToString();
                    company.LogoPath = response.LocalFilePath;

                    _service.EditAsTenant(company);

                    response.CompanyId = company.Id;
                    response.TenantId = company.TenantId;
                }
                

                return Ok(response);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }



        public override IHttpActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id == "undefined") id = User.Identity.GetCompanyId();

            var entity = Service.GetById(id);

            entity.Settings = _companySettingsService.GetAll().FirstOrDefault(x => x.CompanyId == id);
            entity.PoweredBy = entity.Settings?.PoweredBy;

            return Ok(entity);
        }

        public override IHttpActionResult Get(PagingDataType status, string request)
        {
            var requestModel = JsonConvert.DeserializeObject<CompanyRequestModel>(request);

            var isSystemAdminUser = User.IsInRole(StaticRoles.SystemAdmin.Name);

            var responseModel = isSystemAdminUser
                ? _service.GetAll(requestModel)
                : _service.GetAllAsTenant(requestModel);

            return Ok(responseModel);
        }

        public override IHttpActionResult Get(SearchType type, PagingDataType status, string request)
        {
            var isSystemAdminUser = User.IsInRole(StaticRoles.SystemAdmin.Name);

            var entities = isSystemAdminUser
                ? Service.Search(status, JsonConvert.DeserializeObject<CompanyRequestModel>(request))
                : Service.SearchAsTenant(status, JsonConvert.DeserializeObject<CompanyRequestModel>(request));

            return Ok(entities);
        }




        public override IHttpActionResult Put(Company model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //if (model.IsChangeEmail)
            //{
            //    var requestModel = new EmailConfirmationRequestModel
            //    {
            //        Id = model.Id,
            //        IsResend = false,
            //        IsQueryAsTracking = false
            //    };

            //    var task = _service.SendEmailConfirmationCode(requestModel);
            //    task.ConfigureAwait(false);
            //    var responseModel = task.GetAwaiter().GetResult();

            //    if (!string.IsNullOrWhiteSpace(responseModel.ConfirmationCode))
            //        model.EmailConfirmationCode = responseModel.ConfirmationCode;
            //    if (responseModel.ExpireTime != null)
            //        model.EmailConfirmationCodeExpireTime = responseModel.ExpireTime;
            //}

            if (IsSystemAdminUser() && model.IsHostAction)
            {
                Service.EditAsHost(model);
            }
            else
            {
                Service.EditAsTenant(model);
            }

            return Ok(model.Id);
        }


        [HttpPost]
        [Route("SendEmailConfirmationCode")]
        public async Task<IHttpActionResult> SendEmailConfirmationCode(EmailConfirmationRequestModel requestModel)
        {
            var responseModel = await _service.SendEmailConfirmationCode(requestModel);
            return Ok(responseModel);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(EmailConfirmationRequestModel requestModel)
        {
            var responseModel = await _service.ConfirmEmail(requestModel);
            return Ok(responseModel);
        }
    }
}
