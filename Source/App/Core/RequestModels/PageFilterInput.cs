using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.RequestModels
{
    public class PageFilterInput
    {
        public string Sorting { get; set; }
        public bool IsAscending { get; set; } = true;
        public int PageNumber { get; set; }
        public int SkipCount { get; set; }
        public int ResultCount { get; set; }
    }
}
