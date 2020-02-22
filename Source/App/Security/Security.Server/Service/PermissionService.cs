using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Core.Session;
using Project.Core.StaticResource;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Models.ViewModels;
using Security.Server.Managers;
using Security.Server.Repository;

namespace Security.Server.Service
{
    public class PermissionService : SecurityServiceBase<Permission>, IPermissionService
    {
        private readonly ISecurityRepository<Permission, string> _repository;
        private readonly ISecurityRepository<Resource, string> _resourceRepository;
        private readonly IAppSession _appSession;

        public PermissionService(ISecurityRepository<Permission, string> repository,
            ISecurityRepository<Resource, string> resourceRepository,
            IAppSession appSession) : base(repository)
        {
            _repository = repository;
            _resourceRepository = resourceRepository;
            _appSession = appSession;
        }

        public override List<Permission> GetAll()
        {
            return base.GetAll().Where(x => x.TenantId == _appSession.TenantId).ToList();
        }


        public override Permission Create(Permission entity)
        {

            entity.TenantId = _appSession.TenantId;
            return base.Create(entity);
        }

        public override Permission Update(Permission entity)
        {
            entity.TenantId = _appSession.TenantId;
            return base.Update(entity);
        }


        public bool CheckPermission(PermissionRequestModel model)
        {
            var resource = _resourceRepository.GetAllIgnoreFilter().AsNoTracking()
                .FirstOrDefault(x => x.Route.ToLower() == model.Route.ToLower());

            if (resource == null) return false;

            if (resource.IsPublic) return true;


            User user = UserManager.FindById(AppSession.UserId);

            bool isPermitted = false;

            foreach (var role in user.Roles)
            {
                model.RoleId = role.RoleId;
                model.ResourceId = resource.Id;

                var permission = _repository.FirstOrDefault(x => x.RoleId == model.RoleId && x.ResourceId == model.ResourceId);

                if (permission != null)
                    isPermitted = true;
            }

            return isPermitted;
        }


        public List<Permission> GetAllByRoleId(string roleId)
        {
            return _repository.GetAll().Where(x => x.RoleId == roleId).ToList();
        }

        public bool AddList(List<Permission> models)
        {
            List<Permission> missingModels = new List<Permission>();

            var resources = _resourceRepository.GetAllIgnoreFilter().Where(x => x.IsPublic == false).ToList();

            foreach (var model in models)
            {
                var resource = resources.First(x => x.Id == model.ResourceId);
                var parentResource = resources.FirstOrDefault(x => x.Route == resource.ParentRoute);
                if (parentResource != null && models.All(x => x.ResourceId != parentResource.Id) && missingModels.All(x => x.ResourceId != parentResource.Id))
                {
                    missingModels.Add(new Permission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ResourceId = parentResource.Id,
                        RoleId = model.RoleId,
                        TenantId = model.TenantId
                    });
                }
            }


            if (missingModels.Any()) models.AddRange(missingModels);


            var permissions = models.Select(model => _repository.GetAll().Where(x => x.RoleId == model.RoleId).ToList()).FirstOrDefault();

            if (permissions != null)
            {
                _repository.Delete(permissions);
            }

            if (models.Any(model => model.ResourceId != "00000000-0000-0000-0000-000000000000"))
            {
                _repository.Create(models);
            }

            _repository.Save();

            return true;
        }

        public List<PermissionTreeViewModel> GetPermissionTree(string roleId)
        {

            List<Resource> resources;
            if (HttpContext.Current.User.IsInRole(StaticRoles.SystemAdmin.Name))
            {
                resources = _resourceRepository.GetAllIgnoreFilter().Where(x => !x.IsPublic).OrderBy(x => x.Order).ToList();
            }
            else
            {

                resources = _resourceRepository.GetAllIgnoreFilter()
                    .Where(x => !x.IsPublic && x.ResourceOwner == ResourceOwner.Both || x.ResourceOwner == ResourceOwner.Tenant)
                    .OrderBy(x => x.Order).ToList();
            }

            var permissions = _repository.GetAll().Where(x =>
                x.TenantId == _appSession.TenantId && x.RoleId == roleId).ToList();

            var permissionTree = new List<PermissionTreeViewModel>();
            permissionTree = GenerateTree(resources, permissions, permissionTree);

            return permissionTree;

        }

        private List<PermissionTreeViewModel> GenerateTree(List<Resource> resources,
            List<Permission> permissions, List<PermissionTreeViewModel> permissionTree)
        {
            List<PermissionTreeViewModel> GetFourthChildren(string route)
            {
                var children = new List<PermissionTreeViewModel>();

                var childrenResources = resources.Where(x => x.ParentRoute == route).ToList();
                if (!childrenResources.Any()) return children;

                children = childrenResources.ConvertAll(x => new PermissionTreeViewModel()
                {
                    Label = x.DisplayName,
                    Value = x.Id,
                    Selected = permissions.Any(y => y.ResourceId == x.Id),
                    //Children = GetChildren(x.Route)
                });

                return children;
            }
            List<PermissionTreeViewModel> GetThiredChildren(string route)
            {
                var children = new List<PermissionTreeViewModel>();

                var childrenResources = resources.Where(x => x.ParentRoute == route).ToList();
                if (!childrenResources.Any()) return children;

                children = childrenResources.ConvertAll(x => new PermissionTreeViewModel()
                {
                    Label = x.DisplayName,
                    Value = x.Id,
                    Selected = permissions.Any(y => y.ResourceId == x.Id),
                    Children = GetFourthChildren(x.Route)
                });

                return children;
            }

            List<PermissionTreeViewModel> GetSecondChildren(string route)
            {
                var children = new List<PermissionTreeViewModel>();

                var childrenResources = resources.Where(x => x.ParentRoute == route).ToList();
                if (!childrenResources.Any()) return children;

                children = childrenResources.ConvertAll(x => new PermissionTreeViewModel()
                {
                    Label = x.DisplayName,
                    Value = x.Id,
                    Selected = permissions.Any(y => y.ResourceId == x.Id),
                    Children = GetThiredChildren(x.Route)
                });

                return children;
            }

            List<PermissionTreeViewModel> GetFirstChildren(string route)
            {
                var children = new List<PermissionTreeViewModel>();

                var childrenResources = resources.Where(x => x.ParentRoute == route).ToList();
                if (!childrenResources.Any()) return children;

                children = childrenResources.ConvertAll(x => new PermissionTreeViewModel()
                {
                    Label = x.DisplayName,
                    Value = x.Id,
                    Selected = permissions.Any(y => y.ResourceId == x.Id),
                    Children = GetSecondChildren(x.Route)
                });

                return children;
            }


            permissionTree.AddRange(resources.Where(x => x.ParentRoute == null).ToList().ConvertAll(y =>
                new PermissionTreeViewModel()
                {
                    Label = y.DisplayName,
                    Value = y.Id,
                    Selected = permissions.Any(z => z.ResourceId == y.Id),
                    Children = GetFirstChildren(y.Route)
                }));



            return permissionTree;
        }
    }
}