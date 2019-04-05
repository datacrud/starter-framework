using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.ViewModel;

namespace Project.Service
{
    public enum DropdownType
    {
        Table1,
        Table2
    }

    public class DropdownDataService
    {
        private BusinessDbContext _db = new BusinessDbContext();

        

        public List<DropdownViewModel> GetData(DropdownType type)
        {

            switch (type)
            {
                case DropdownType.Table1:
                    return null;

                case DropdownType.Table2:
                    return null;

                default:
                    return null;

            }
        }



        public List<DropdownViewModel> GetData(DropdownType type, string id)
        {

            switch (type)
            {
                case DropdownType.Table1:
                    return null;

                case DropdownType.Table2:
                    return null;

                default:
                    return null;

            }
        }
    }

    
}
