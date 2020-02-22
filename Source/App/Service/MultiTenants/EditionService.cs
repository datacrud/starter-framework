using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Project.Model;
using Project.Repository;
using Project.RequestModel;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IEditionService : IServiceBase<Edition, EditionViewModel>
    {
        bool IsEditionExist(string name, string id);
        Task<List<EditionViewModel>> GetEditionsForSubscription();
    }

    public class EditionService : ServiceBase<Edition, EditionViewModel>, IEditionService
    {
        private readonly IEditionRepository _repository;
        private readonly IFeatureRepository _featureRepository;

        public EditionService(IEditionRepository repository, IFeatureRepository featureRepository) : base(repository)
        {
            _repository = repository;
            _featureRepository = featureRepository;
        }


        public bool IsEditionExist(string name, string id)
        {
            var edition = _repository.AsNoTracking().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (edition != null && edition.Id != id) return true;

            return false;
        }

        public override EditionViewModel GetById(string id)
        {
            var entity = Repository.GetById(id);
            var entityViewModel = new EditionViewModel(entity);

            IQueryable<Feature> queryable = _featureRepository.GetEditionFeatures(entityViewModel.Id);
            entityViewModel.Features = queryable.ToList().DistinctBy(x => x.Name).ToList()
                .ConvertAll(x => new FeatureViewModel(x));

            return entityViewModel;
        }

        public async Task<List<EditionViewModel>> GetEditionsForSubscription()
        {
            var editions = await _repository.GetAll()
                .Where(x => x.IsActive)
                .Include(x => x.Features)
                .OrderBy(x => x.Order)
                .ToListAsync();

            var viewModels = editions.ConvertAll(x => new EditionViewModel(x));

            return viewModels;
        }


    }

    public class FeatureEqualityComparer : IEqualityComparer<Feature>
    {
        public bool Equals(Feature x, Feature y)
        {
            return x != null && (y != null && x.Name == y.Name);
        }

        public int GetHashCode(Feature obj)
        {
            return obj.GetHashCode();
        }
    }
}
