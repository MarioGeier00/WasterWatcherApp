using System.Threading.Tasks;
using WasteWatcherApp.OpenFoodFacts;

namespace WasteWatcherApp.Product.Persistance
{
    class OpenFoodFactsProductSource : IProductSource<string>, IProductSource<ProductData>
    {
        async Task<string> IProductSource<string>.GetData(string barcode)
            => await OpenFoodFactsApi.GetProductDataJsonByBarcode(barcode);

        async Task<ProductData> IProductSource<ProductData>.GetData(string barcode)
            => await OpenFoodFactsApi.GetProductDataByBarcode(barcode);
    }
}
