using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    public interface IProductSource<T>
    {
        /// <summary>
        /// Load the data of a product.
        /// </summary>
        Task<T> GetData(string barcode);
    }
}
