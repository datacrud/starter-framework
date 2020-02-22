using System.Web.Http;
using Project.Core.DomainBase;
using Project.Core.Enums.Framework;

namespace Project.Server.Controllers.Bases
{
    public interface IControllerBase<in T> : IPagingController
        where T : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase
    {
        IHttpActionResult Get(DataType type);
        IHttpActionResult Get(string id);
        IHttpActionResult Post(T model);
        IHttpActionResult Put(T entity);
        IHttpActionResult Delete(DeleteActionType type, string id);
    }
}