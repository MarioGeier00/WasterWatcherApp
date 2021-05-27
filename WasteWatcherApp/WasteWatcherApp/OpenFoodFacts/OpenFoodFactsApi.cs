using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WasteWatcherApp.Product;
using WasteWatcherApp.Product.Persistance;

namespace WasteWatcherApp.OpenFoodFacts
{
    public static class OpenFoodFactsApi
    {
        /// <summary>
        /// Get Data from OpenFoodfacts
        /// </summary>
        /// <returns>Product object</returns>
        public static async Task<ProductData> GetProductDataByBarcode(string barcode)
        {
            string data = await GetProductDataJsonByBarcode(barcode);
            return ProductJsonParser.Parse(data, barcode);
        }
        
        /// <summary>
        /// Function to retrieve the data from OpenFoodFacts api
        /// </summary>
        public static async Task<string> GetProductDataJsonByBarcode(string barcode)
        {
            string url = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
            HttpClient client = new();
            return await client.GetStringAsync(url);
        }
    }
}
