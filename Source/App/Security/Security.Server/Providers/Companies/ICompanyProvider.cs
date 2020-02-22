using System;
using System.Linq;
using Project.Model;

namespace Security.Server.Providers
{
    public interface ICompanyProvider : IDisposable
    {
        /// <summary>
        /// Get all active entries
        /// </summary>
        /// <returns></returns>
        IQueryable<Company> GetAll();

    }
}