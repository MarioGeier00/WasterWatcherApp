using System;
using System.Threading.Tasks;
using WasteWatcherApp.Waste;

namespace WasteWatcherApp
{
    public interface IWasteStore
    {
        [Obsolete]
        Task SaveData(string productId, string plasticWaste, string paperWaste, string glasWaste);
        Task SaveData(string productId, WasteCollection waste);
        Task<WasteCollection> GetData(string productId);
    }
}