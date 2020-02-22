using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.DomainBase
{
    public interface IMayHaveUserReference<TUser> 
        where TUser : class
    {

        [ForeignKey("CreatedBy")] TUser Creator { get; set; }


        [ForeignKey("ModifiedBy")] TUser Modifier { get; set; }

        [ForeignKey("DeletedBy")] TUser Deleter { get; set; }
    }
}