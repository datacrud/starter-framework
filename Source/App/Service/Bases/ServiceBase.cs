using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.DomainBase;
using Project.Core.Enums.Framework;
using Project.Core.Session;
using Project.Model.Repositories;
using Project.RequestModel.Bases;
using Project.ViewModel;
using Project.ViewModel.Bases;
using Security.Server.Managers;

namespace Project.Service.Bases
{
    public abstract class ServiceBase<TEntity, TVm> : IServiceBase<TEntity, TVm>
        where TEntity : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase
        where TVm : ViewModelBase<TEntity>
    {
        protected IRepository<TEntity> Repository;

        protected readonly UserManager UserManager;

        protected readonly IAppSession AppSession;


        protected ServiceBase(IRepository<TEntity> repository)
        {
            Repository = repository;

            UserManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            AppSession = new AppSession();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            UserManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }



        ///=================
        //PAGING BLOCK Start
        //==================

        #region Paging List Query Block

        public virtual ResponseModel<TVm> GetAll(RequestModelBase<TEntity> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAll(), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<TVm>(entities, Repository.GetAll().Count());
            //
            return response;
        }


        #endregion

        #region Paging Search Query Block

        public virtual ResponseModel<TVm> Search(PagingDataType status, RequestModelBase<TEntity> requestModel)
        {
            var count = Repository.GetAll().Where(requestModel.GetExpression()).Count();
            var response = GetAll(requestModel);
            response.Count = count;

            return response;
        }

        #endregion

        //================
        //PAGING BLOCK END
        //================


        #region Host Gets

        public virtual List<TVm> GetAll()
        {
            var list = Repository.GetAll().ToList();
            var entities = list.ConvertAll(x => (TVm)Activator.CreateInstance(typeof(TVm), x));

            return entities;
        }

        #endregion




        public virtual TVm GetById(string id)
        {
            var entity = Repository.GetById(id);
            var entityViewModel = (TVm)Activator.CreateInstance(typeof(TVm), entity);

            return entityViewModel;
        }

        public Task<TEntity> GetByIdAsync(string id)
        {
            return Repository.GetByIdAsync(id);
        }

        public TVm GetByIdAsNoTracking(string id)
        {
            var entity = Repository.AsNoTracking().SingleOrDefault(x => x.Id == id);
            var entityViewModel = (TVm)Activator.CreateInstance(typeof(TVm), entity);

            return entityViewModel;
        }

        public Task<TEntity> GetByIdAsNoTrackingAsync(string id)
        {
            return Repository.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }


        public virtual TEntity GetEntityById(string id)
        {
            var entity = Repository.GetById(id);
            return entity;
        }

        public TEntity GetEntityByIdAsNoTracking(string id, params Expression<Func<TEntity, object>>[] expression)
        {
            var queryable = Repository.AsNoTracking();
            foreach (var exp in expression)
            {
                queryable = queryable.Include(exp);
            }
            var entity = queryable.SingleOrDefault(x => x.Id == id);

            return entity;
        }


        #region Host Add, Edit

        public virtual bool CreateAsHost(TEntity entity)
        {
            var add = Repository.CreateAsHost(entity);
            var commit = Repository.Commit();

            return commit;
        }

        public virtual bool CreateAsHost(List<TEntity> entities)
        {
            var enumerable = Repository.CreateAsHost(entities);
            var commit = Repository.Commit();

            return commit;
        }

        public virtual bool EditAsHost(TEntity entity)
        {
            var entityState = Repository.EditAsHost(entity);
            return Repository.Commit();
        }

        public virtual bool EditAsHost(List<TEntity> entities)
        {
            EntityState entityState;
            foreach (var entity in entities)
            {
                entityState = Repository.EditAsHost(entity);
            }
            return Repository.Commit();
        }

        #endregion



        public virtual bool Delete(string id)
        {
            var delete = Repository.Delete(Repository.GetById(id));
            var commit = Repository.Commit();

            return commit;
        }

        public virtual bool Delete(List<TEntity> entries)
        {
            IEnumerable<TEntity> removeAll = Repository.Delete(entries);
            bool commit = Repository.Commit();

            return commit;
        }

        public virtual bool Trash(string id)
        {
            var entity = Repository.GetById(id);
            entity.Active = false;
            var entityState = Repository.Trash(entity);
            var commit = Repository.Commit();

            return commit;
        }



        public void Dispose()
        {
            Repository.Dispose();
        }


        public static IQueryable<TEntity> GetPagingQuery(IQueryable<TEntity> queryable, RequestModelBase<TEntity> requestModel)
        {
            return requestModel.GetSkipAndTakeQuery(requestModel.GetOrderedDataQuery(queryable.Where(requestModel.GetExpression())));
        }

        public static List<TVm> GetEntries(IQueryable<TEntity> queryable)
        {
            return queryable.ToList().ConvertAll(x => (TVm)Activator.CreateInstance(typeof(TVm), x));
        }

    }
}