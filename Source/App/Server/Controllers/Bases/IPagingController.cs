using System.Web.Http;
using Project.Core.Enums.Framework;

namespace Project.Server.Controllers.Bases
{
    public interface IPagingController
    {
        IHttpActionResult Get(PagingDataType status, string request);
        IHttpActionResult Get(SearchType type, PagingDataType status, string request);
    }
}