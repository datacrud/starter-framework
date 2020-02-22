using System.Threading.Tasks;
using System.Web.Http;
using Project.Core.Enums;
using Project.Service.Lookups;

namespace Project.Server.Controllers.Lookups
{
    [Authorize]
    public class TypeAHeadController : ApiController
    {
        private readonly ITypeAHeadDataService _typeAHeadDataService;

        public TypeAHeadController(ITypeAHeadDataService typeAHeadDataService)
        {
            _typeAHeadDataService = typeAHeadDataService;
        }

        public async Task<IHttpActionResult> Get(string request, TypeAHeadType type)
        {
            var models = await _typeAHeadDataService.GetData(request, type);

            return Ok(models);
        }
    }
}
