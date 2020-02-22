using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Project.Core.Enums;
using Project.Service.Lookups;
using Project.ViewModel;

namespace Project.Server.Controllers.Lookups
{
    [Authorize]
    [RoutePrefix("api/Dropdown")]
    public class DropdownController : ApiController
    {
        private readonly IDropdownDataService _dropdownDataService;

        public DropdownController(IDropdownDataService dropdownDataService)
        {
            _dropdownDataService = dropdownDataService;
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("Edition")]
        public async Task<IHttpActionResult> Edition()
        {           
            List<DropdownViewModel> dropdownViewModels = await _dropdownDataService.GetData(DropdownType.Edition);
            return Ok(dropdownViewModels);
        }

        public async Task<IHttpActionResult> Get(DropdownType type)
        {
            
            List<DropdownViewModel> dropdownViewModels = await _dropdownDataService.GetData(type);
            return Ok(dropdownViewModels);
        }


        public async Task<IHttpActionResult> Get(string id, DropdownType type)
        {
            List<DropdownViewModel> dropdownViewModels = await _dropdownDataService.GetData(type, id);
            return Ok(dropdownViewModels);
        }

        public async Task<IHttpActionResult> Get(string cid, string pid, DropdownType type)
        {
            List<DropdownViewModel> dropdownViewModels = await _dropdownDataService.GetData(type, pid, cid);
            return Ok(dropdownViewModels);
        }
    }
    
}