using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Security.Models.Models;
using Security.Server.Repository;

namespace Security.Server.Service
{
    public class ResourceService: SecurityServiceBase<Resource>, IResourceService
    {
        private readonly ISecurityRepository<Resource, string> _repository;

        public ResourceService(ISecurityRepository<Resource, string> repository) : base(repository)
        {
            _repository = repository;
        }


        public override List<Resource> GetAll()
        {
            return _repository.GetAllIgnoreFilter().ToList();
        }

    }
}