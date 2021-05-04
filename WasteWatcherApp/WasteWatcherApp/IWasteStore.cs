using System.Threading.Tasks;

namespace WasteWatcherApp
{
    public interface IWasteStore
    {
        Task SaveData(string plasticWaste, string paperWaste, string glasWaste);
    }
}