using System.Web.Http;
using Project.Core.Enums.Framework;
using Project.Service.AutoGenData;

namespace Project.Server.Controllers.Lookups
{
    [Authorize]
    public class AutoGenController : ApiController
    {
        private readonly IAutoGenDataService _service;

        public AutoGenController(IAutoGenDataService service)
        {
            _service = service;
        }

        public IHttpActionResult Get(AutoGenType type)
        {
            return Ok(_service.GetData(type));
        }
    }
}
