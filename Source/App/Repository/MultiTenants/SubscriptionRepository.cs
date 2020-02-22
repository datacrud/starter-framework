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
    public interface ISubscriptionRepository : IHaveTenantIdCompanyIdRepositoryBase<Subscription>
    {

    }

    public class SubscriptionRepository : HaveTenantIdCompanyIdRepositoryBase<Subscription>, ISubscriptionRepository
    {

        public SubscriptionRepository(BusinessDbContext db) : base(db)
        {
        }

    }
}