using System.Collections.Generic;
using System.Linq;
using Project.Core.Enums.Framework;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IFeatureService : IServiceBase<Feature, FeatureViewModel>
    {
        bool IsFeatureExist(string name, string id, ActionType actionType);
        bool Edit(List<Feature> features, string id);
        List<Feature> GetTenantEditionFeatures(string tenantId);
        string GetEditionFeatureValue(string tenantId, string featureName);
    }

    public class FeatureService : ServiceBase<Feature, FeatureViewModel>, IFeatureService
    {
        private readonly IFeatureRepository _repository;
        private readonly ITenantRepository _tenantRepository;

        public FeatureService(IFeatureRepository repository, ITenantRepository tenantRepository) : base(repository)
        {
            _repository = repository;
            _tenantRepository = tenantRepository;
        }

        public bool IsFeatureExist(string name, string id, ActionType actionType)
        {
            var feature = _repository.AsNoTracking()
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.IsFeature);
            if (feature != null && feature.Id != id) return true;
            
            return false;
        }

        public bool Edit(List<Feature> features, string id)
        {
            var existingFeatures = _repository.GetAll().Where(x => x.EditionId == id && x.IsEditionFeature).ToList();

            foreach (var existingFeature in existingFeatures)
            {
                var saleProduct = features.FirstOrDefault(x => x.Id == existingFeature.Id);
                if (saleProduct == null)
                {
                    _repository.Delete(existingFeature);
                }
            }

            foreach (var feature in features)
            {
                if (existingFeatures.FirstOrDefault(x => x.Id == feature.Id) == null)
                {
                    feature.Edition = null;
                    _repository.CreateAsHost(feature);
                }
                else
                {
                    _repository.UpdateAsHost(feature);
                }
            }


            return _repository.Commit();
        }


        public List<Feature> GetTenantEditionFeatures(string tenantId)
        {
            var tenant = _tenantRepository.AsNoTracking().SingleOrDefault(x=> x.Id == tenantId);
            if (tenant == null) return null;

            var features = _repository.AsNoTracking().Where(x =>
                x.EditionId == tenant.EditionId && x.IsEditionFeature && !x.IsTenantFeature && !x.IsFeature).ToList();
            return features;
        }

        public string GetEditionFeatureValue(string tenantId, string featureName)
        {
            var features = GetTenantEditionFeatures(tenantId);
            return features.FirstOrDefault(x => x.Name == featureName)?.Value;
        }
    }
}
