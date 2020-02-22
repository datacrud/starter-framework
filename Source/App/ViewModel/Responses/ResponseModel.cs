using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class ResponseModel<TVm> where TVm: class 
    {
        public List<TVm> Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public int Count { get; set; }


        public ResponseModel(List<TVm> data = null, int count = 0, bool isSuccess = true, string message = "Success")
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
            Count = count;
        }
    }
}
