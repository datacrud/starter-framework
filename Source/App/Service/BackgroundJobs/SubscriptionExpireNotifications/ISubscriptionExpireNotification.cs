using System.Threading.Tasks;

namespace Project.Service.BackgroundJobs.SubscriptionExpireNotifications
{
    public interface ISubscriptionExpireNotification
    {
        Task ExecuteNotify();
    }
}