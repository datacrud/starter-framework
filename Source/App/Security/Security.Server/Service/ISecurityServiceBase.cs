using System.Collections.Generic;
using Project.Core.Repositories;
using Project.Core.RequestModels;
using Project.Core.ResponseModels;

namespace Security.Server.Service
{
    public interface ISecurityServiceBase<T> where T : class
    {
        List<T> GetAll();
        PageResultOutput<T> GetAll(PageFilterInput input);
        T GetById(object id);
        T Create(T entity);
        T Update(T entity);
        T Delete(object id);
    }

}