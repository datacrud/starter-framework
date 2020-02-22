using System;
using System.Collections.Generic;
using Project.Model;

namespace Security.Server.Providers
{
    public interface IFeatureProvider : IDisposable
    {
        List<Feature> GetEditionFeatures(string editionName);
        List<Feature> GetTenantEditionFeatures(string tenantId);
        string GetEditionFeatureValue(string tenantId, string featureName);
    }
}