using System.Net;

namespace Project.BindingModel.BackgroundJobs
{
    public class BackgroundJobOutput
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public BackgroundJobOutput()
        {
            IsSuccess = true;
            Message = HttpStatusCode.OK.ToString();
        }

        public BackgroundJobOutput(bool isSuccess)
        {
            IsSuccess = IsSuccess;
            Message = isSuccess ? HttpStatusCode.OK.ToString() : HttpStatusCode.NotAcceptable.ToString();
        }

        public BackgroundJobOutput(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}