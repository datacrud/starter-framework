using System.Linq;
using Project.Core.Session;
using Project.Model;

namespace Security.Server.Providers
{
    public class CompanyProvider : ICompanyProvider
    {
        private readonly BusinessDbContext _context;

        public CompanyProvider(IAppSession appSession)
        {
            _context = new BusinessDbContext();
        }



        public void Dispose()
        {
            _context?.Dispose();
        }

        public IQueryable<Company> GetAll()
        {
            return _context.Companies.Where(x=> x.Active).AsQueryable();
        }
    }
}