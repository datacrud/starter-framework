using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.RequestModel
{
    public class EmailConfirmationRequestModel
    {
        public EmailConfirmationRequestModel()
        {
            IsResend = false;
            IsQueryAsTracking = true;
        }

        public string Id { get; set; }
        public string ConfirmationCode { get; set; }
        public bool IsResend { get; set; }

        public bool IsQueryAsTracking { get; set; }
    }
}
