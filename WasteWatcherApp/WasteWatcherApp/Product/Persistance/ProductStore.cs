using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    class ProductStore : IProductSource<ProductData>
    {
        public Task<ProductData> GetData(string barcode)
        {
            throw new System.NotImplementedException();
        }
    }
}
