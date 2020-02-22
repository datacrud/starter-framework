using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Project.Core.DomainBase;
using Project.Core.Repositories;

namespace Security.Server.Repository
{
    public interface ISecurityRepository<T, TKey> : IGenericRepository<T, TKey> where T : class, IEntity<TKey>, IHaveTenant<string>, IHaveCompany<string>
    {
        IQueryable<T> GetAllIgnoreFilter();
    }

}