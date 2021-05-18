using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    public interface IProductSource<T>
    {
        Task<T> GetData(string barcode);
    }
}
