using System.Collections.Generic;
using System.Linq;
using Project.Model;

namespace Security.Server.Providers
{
    public class FeatureProvider : IFeatureProvider
    {
        private readonly BusinessDbContext _context;

        public FeatureProvider(BusinessDbContext context)
        {
            _context = context;
        }

        public List<Feature> GetEditionFeatures(string editionName)
        {
            var edition = _context.Editions.FirstOrDefault(x => x.Name == editionName);
            if (edition == null) return null;

            var features = _context.Features.Where(x =>
                x.EditionId == edition.Id && x.IsEditionFeature && !x.IsTenantFeature && !x.IsFeature).ToList();
            return features;
        }

        public List<Feature> GetTenantEditionFeatures(string tenantId)
        {
            var tenant = _context.Tenants.Find(tenantId);
            if (tenant == null) return null;

            var features = _context.Features.Where(x =>
                x.EditionId == tenant.EditionId && x.IsEditionFeature && !x.IsTenantFeature && !x.IsFeature).ToList();
            return features;
        }

        public string GetEditionFeatureValue(string tenantId, string featureName)
        {
            var features = GetTenantEditionFeatures(tenantId);
            return features.FirstOrDefault(x => x.Name == featureName)?.Value;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}