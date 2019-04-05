using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class BaseResponseModel
    {
        public object Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }


        public BaseResponseModel(object data = null, bool isSuccess = true, string message = "Success")
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
