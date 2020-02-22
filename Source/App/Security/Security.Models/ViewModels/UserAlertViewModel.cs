using Project.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models.ViewModels
{
    public class UserAlertViewModel
    {
        public bool IsShow { get; set; }
        public int Identity { get; set; }

        public string Title { get; set; }
        public DashboardAlertType AlertType { get; set; }
        public string AlertClass { get; set; }
        public string Message { get; set; }

    }
}
