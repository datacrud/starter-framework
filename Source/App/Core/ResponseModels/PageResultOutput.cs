using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.ResponseModels
{
    public class PageResultOutput<T>
    {
        public int TotalCount { get; set; }

        public List<T> Items { get; set; }

        public PageResultOutput(int totalCount, List<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
