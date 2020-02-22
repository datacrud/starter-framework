using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models.ViewModels
{
    public class PermissionTreeViewModel
    {
        public string Label { get; set; }
        public string Value { get; set; }       
       
        public bool Selected { get; set; }
        public bool IsExpanded { get; set; }

        public List<PermissionTreeViewModel> Children { get; set; }
    }
}
