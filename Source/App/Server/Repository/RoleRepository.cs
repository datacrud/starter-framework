using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Repository;
using Project.Server.Models;

namespace Project.Server.Repository
{
    public interface IRoleRepository : ISecurityBaseRepository<IdentityRole>
    {        
    }

    public class RoleRepository : SecurityBaseRepository<IdentityRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _db;

        public RoleRepository(DbContext db) : base(db)
        {
            _db = db as ApplicationDbContext;
        }
        
    }
}