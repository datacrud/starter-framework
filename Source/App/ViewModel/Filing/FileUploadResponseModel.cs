using System.Net.Http.Headers;

namespace Project.ViewModel
{
    public class FileUploadResponseModel
    {
        public string FileId { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public MediaTypeHeaderValue FileType { get; set; }
        public string LocalFilePath { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string TenantId { get; set; }
        public string CompanyId { get; set; }
    }
}