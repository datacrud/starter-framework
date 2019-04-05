using System;
using System.Collections.Generic;
using System.Linq;
using Project.Server.Repository;
using Project.Service;
using static Project.Server.Models.SecurityModels;

namespace Project.Server.Service
{
    public interface IResourceService :ISecurityBaseService<AspNetResource>
    {
    }


    public class ResourceService: SecurityBaseService<AspNetResource>, IResourceService
    {
        private readonly IResourceRepository _repository;

        public ResourceService(IResourceRepository repository) : base(repository)
        {
            _repository = repository;
        }
     

    }
}