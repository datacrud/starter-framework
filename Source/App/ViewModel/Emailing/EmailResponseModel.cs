using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class EmailResponseModel
    {
        public EmailResponseModel(bool isSuccess = true, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string ConfirmationCode { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}
