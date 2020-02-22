using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Session;

namespace Project.Core.Extensions.Framework
{
    public static class DynamicQueryExtension
    {
        public static IQueryable<T> FilterSoftDelete<T>(this IQueryable<T> source)
        {
            return source.Where($"IsDeleted == false");
        }

        public static IQueryable<T> FilterActive<T>(this IQueryable<T> source)
        {
            return source.Where("Active == false");
        }
    }
}
