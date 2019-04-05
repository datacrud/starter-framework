using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Project.Repository;
using Project.Server.Models;

namespace Project.Server.Repository
{
    public interface IPermissionRepository : ISecurityBaseRepository<SecurityModels.AspNetPermission>
    {               
        SecurityModels.AspNetPermission CheckPermission(RequestModels.PermissionRequestModel model);
        IQueryable<SecurityModels.AspNetPermission> GetListById(string request);
        bool AddList(List<SecurityModels.AspNetPermission> models);
    }



    public class PermissionRepository : SecurityBaseRepository<SecurityModels.AspNetPermission>, IPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public PermissionRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }



        public IQueryable<SecurityModels.AspNetPermission> GetListById(string request)
        {
            var permissions = _db.Permissions.Where(x => x.RoleId == request).AsQueryable();
            return permissions;
        }

        public SecurityModels.AspNetPermission CheckPermission(RequestModels.PermissionRequestModel model)
        {

            return _db.Permissions.FirstOrDefault(x => x.RoleId == model.RoleId && x.ResourceId == model.ResourceId);
        }        
        

        public bool AddList(List<SecurityModels.AspNetPermission> models)
        {
            try
            {
                var permissions = models.Select(model => _db.Permissions.Where(x => x.RoleId == model.RoleId).ToList()).FirstOrDefault();

                if (permissions != null)
                {
                    _db.Permissions.RemoveRange(permissions);
                }

                if (models.Any(model => model.ResourceId != "00000000-0000-0000-0000-000000000000"))
                {
                    _db.Permissions.AddRange(models);
                }                

                _db.SaveChanges();
            }
            catch (Exception exception)
            {                
                throw new Exception(exception.Message);
            }

            return true;
        }
    }
}