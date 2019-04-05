using System;
using System.Data.Entity;
using System.Linq;
using Project.Repository;
using Project.Server.Models;
using static Project.Server.Models.SecurityModels;

namespace Project.Server.Repository
{
    public interface IResourceRepository : ISecurityBaseRepository<AspNetResource>
    {
        AspNetResource CheckResource(string route);
    }




    public class ResourceRepository: SecurityBaseRepository<AspNetResource>, IResourceRepository
    {
        private readonly ApplicationDbContext _db;

        public ResourceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public AspNetResource CheckResource(string route)
        {
            return _db.Resources.FirstOrDefault(x => x.Route.ToLower() == route.ToLower());
        }

    }
}