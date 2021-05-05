using System.Threading.Tasks;

namespace WasteWatcherApp
{
    public interface IWasteStore
    {
        Task SaveData(string productId, string plasticWaste, string paperWaste, string glasWaste);
        Task<WasteData> GetData(string productId);
    }
}