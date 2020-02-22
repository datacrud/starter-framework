using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IRfqService : IServiceBase<Rfq, RfqViewModel>
    {
        
    }

    public class RfqService : ServiceBase<Rfq, RfqViewModel>, IRfqService
    {
        private readonly IRfqRepository _repository;

        public RfqService(IRfqRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
