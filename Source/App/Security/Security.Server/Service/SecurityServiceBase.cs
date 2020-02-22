using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.DomainBase;
using Project.Core.Extensions.Framework;
using Project.Core.Repositories;
using Project.Core.RequestModels;
using Project.Core.ResponseModels;
using Project.Core.Session;
using Security.Models.Models;
using Security.Server.Managers;
using Security.Server.Repository;

namespace Security.Server.Service
{
    public abstract class SecurityServiceBase<TEntity> : ISecurityServiceBase<TEntity> where TEntity : class, IEntity<string>, IHaveTenant<string>, IHaveCompany<string>
    {
        protected ISecurityRepository<TEntity, string> Repository;
        protected readonly UserManager UserManager;
        protected readonly IAppSession AppSession;

        protected SecurityServiceBase(ISecurityRepository<TEntity, string> repository)
        {
            Repository = repository;
            UserManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            AppSession = new AppSession();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            UserManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }

        public virtual List<TEntity> GetAll()
        {
            return Repository.GetAll().ToList();
        }

        public PageResultOutput<TEntity> GetAll(PageFilterInput input)
        {
            var entities = Repository.GetAll().ApplyOrder(input.Sorting).Skip(input.SkipCount * input.PageNumber).Take(input.ResultCount).ToList();
            return new PageResultOutput<TEntity>(Repository.GetAll().Count(), entities);
        }

        public virtual TEntity GetById(object id)
        {
            return Repository.Get(id);
        }


        public virtual TEntity Create(TEntity entity)
        {
            Repository.Create(entity);
            Repository.Save();

            return entity;
        }



        public virtual TEntity Update(TEntity entity)
        {
            Repository.Modify(entity);
            Repository.Save();

            return entity;
        }


        public virtual TEntity Delete(object id)
        {
            var entity = Repository.Get(id);
            Repository.Delete(entity);
            Repository.Save();

            return entity;
        }

    }





}
