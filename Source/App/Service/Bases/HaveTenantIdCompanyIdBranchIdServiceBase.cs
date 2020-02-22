using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Core.Enums.Framework;
using Project.Model.EntityBase;
using Project.Model.EntityBases;
using Project.Model.Repositories;
using Project.RequestModel.Bases;
using Project.ViewModel;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public abstract class HaveTenantIdCompanyIdBranchIdServiceBase<TEntity, TVm> : ServiceBase<TEntity, TVm>, IHaveTenantIdCompanyIdBranchIdServiceBase<TEntity, TVm>
        where TEntity : HaveTenantIdCompanyIdBranchIdEntityBase
        where TVm : HaveTenantIdCompanyIdBranchIdViewModelBase<TEntity>
    {
        protected IHaveTenantIdCompanyIdBranchIdRepositoryBase<TEntity> Repository;

        protected HaveTenantIdCompanyIdBranchIdServiceBase(IHaveTenantIdCompanyIdBranchIdRepositoryBase<TEntity> repository): base(repository)
        {
            Repository = repository;
        }



        ///=================
        //PAGING BLOCK Start
        //==================

        #region Paging List Query Block


        public virtual ResponseModel<TVm> GetAllAsTenant(HaveTenantIdCompanyIdBranchIdRequestModelBase<TEntity> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAllAsTenant(), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<TVm>(entities, Repository.GetAllAsTenant().Count());
            //
            return response;
        }


        #endregion

        #region Paging Search Query Block

        public virtual ResponseModel<TVm> SearchAsTenant(PagingDataType status, HaveTenantIdCompanyIdBranchIdRequestModelBase<TEntity> requestModel)
        {
            var count = Repository.GetAllAsTenant().Where(requestModel.GetExpression()).Count();
            var response = GetAllAsTenant(requestModel);
            response.Count = count;

            return response;
        }

        #endregion

        //================
        //PAGING BLOCK END
        //================


        #region Tenant Gets

        public virtual List<TVm> GetAllAsTenant(string tenantId = null)
        {
            var list = Repository.GetAllAsTenant(tenantId).ToList();
            var entities = list.ConvertAll(x => (TVm)Activator.CreateInstance(typeof(TVm), x));

            return entities;
        }



        #endregion




        #region Tenant Add, Edit

        public virtual bool CreateAsTenant(TEntity entity)
        {
            var add = Repository.AddAsTenant(entity);
            var commit = Repository.Commit();

            return commit;
        }

        public virtual bool CreateAsTenant(List<TEntity> entities)
        {
            var queryable = Repository.AddAsTenant(entities);
            var commit = Repository.Commit();

            return commit;
        }

        public virtual bool EditAsTenant(TEntity entity)
        {
            var entityState = Repository.EditAsTenant(entity);
            return Repository.Commit();
        }

        public virtual bool EditAsTenant(List<TEntity> entities)
        {
            EntityState entityState;
            foreach (var entity in entities)
            {
                Repository.EditAsTenant(entity);
            }
            return Repository.Commit();
        }

        #endregion


        public virtual bool Restore(string id)
        {
            var entity = Repository.GetById(id);
            entity.Active = true;
            var entityState = Repository.EditAsTenant(entity);
            var commit = Repository.Commit();

            return commit;
        }


        public static IQueryable<TEntity> GetPagingQuery(IQueryable<TEntity> queryable, HaveTenantIdCompanyIdBranchIdRequestModelBase<TEntity> requestModel)
        {
            return requestModel.GetSkipAndTakeQuery(requestModel.GetOrderedDataQuery(queryable.Where(requestModel.GetExpression())));
        }


    }
}