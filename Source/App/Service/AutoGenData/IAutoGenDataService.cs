using Project.Core.Enums.Framework;

namespace Project.Service.AutoGenData
{
    public interface IAutoGenDataService
    {
        string GetData(AutoGenType type);
    }
}