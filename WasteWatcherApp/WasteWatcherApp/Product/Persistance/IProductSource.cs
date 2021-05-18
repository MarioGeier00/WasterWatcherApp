using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    interface IProductSource<T>
    {
        Task<T> GetData(string barcode);
    }
}
