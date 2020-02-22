using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Model;
using Project.Model.Repositories;
using Project.RequestModel;

namespace Project.Repository
{
    public interface ISubscriptionPaymentRepository : IHaveTenantIdCompanyIdRepositoryBase<SubscriptionPayment>
    {

    }

    public class SubscriptionPaymentRepository : HaveTenantIdCompanyIdRepositoryBase<SubscriptionPayment>, ISubscriptionPaymentRepository
    {
        private readonly BusinessDbContext _db;

        public SubscriptionPaymentRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
