using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Session;
using Project.Model;
using Project.Model.Repositories;


namespace Project.Repository
{
    public interface IFeatureRepository : IRepository<Feature>
    {
        //Tenant Gets
        IQueryable<Feature> GetEditionFeatures(string editionId);
    }

    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {

        public FeatureRepository(BusinessDbContext db) : base(db)
        {

        }

        public override IQueryable<Feature> GetAll()
        {
            return base.GetAll().Where(x => x.IsFeature);
        }


        #region Tenant Gets

        public IQueryable<Feature> GetEditionFeatures(string editionId)
        {
            return base.GetAll().Where(x => x.EditionId == editionId && x.IsEditionFeature).AsQueryable();
        }

        #endregion

    }
}