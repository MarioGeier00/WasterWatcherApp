using System.Threading.Tasks;
using WasteWatcherApp.OpenFoodFacts;

namespace WasteWatcherApp.Product.Persistance
{
    /// <summary>
    /// An interface implementation of the static OpenFoodFactsApi class
    /// </summary>
    class OpenFoodFactsProductSource : IProductSource<string>, IProductSource<ProductData>
    {
        /// <summary>
        /// Load the json data for a given barcode.
        /// </summary>
        async Task<string> IProductSource<string>.GetData(string barcode)
            => await OpenFoodFactsApi.GetProductDataJsonByBarcode(barcode);

        /// <summary>
        /// Load the parsed product data for a given barcode.
        /// </summary>
        async Task<ProductData> IProductSource<ProductData>.GetData(string barcode)
            => await OpenFoodFactsApi.GetProductDataByBarcode(barcode);
    }
}
