using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    public class DropdownController : ApiController
    {

        public IHttpActionResult Get(DropdownType type)
        {
            DropdownDataService dropdownDataService = new DropdownDataService();
            List<DropdownViewModel> dropdownViewModels = dropdownDataService.GetData(type);
            return Ok(dropdownViewModels);
        }


        public IHttpActionResult Get(DropdownType type, string id)
        {
            DropdownDataService dropdownDataService = new DropdownDataService();
            List<DropdownViewModel> dropdownViewModels = dropdownDataService.GetData(type, id);
            return Ok(dropdownViewModels);
        }
    }
    
}