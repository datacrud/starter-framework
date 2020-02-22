using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Model.Repositories;


namespace Project.Repository
{
    public interface IEditionRepository : IRepository<Edition>
    {

        //Tenant Gets
        IQueryable<Edition> GetAllAsTenant(string tenantId = null);
    }

    public class EditionRepository : Repository<Edition>, IEditionRepository
    {
        public EditionRepository(BusinessDbContext db) : base(db)
        {

        }

        #region Tenant Gets

        public IQueryable<Edition> GetAllAsTenant(string tenantId = null)
        {

            return Context.Set<Edition>().AsQueryable();
        }



        #endregion
    }
}