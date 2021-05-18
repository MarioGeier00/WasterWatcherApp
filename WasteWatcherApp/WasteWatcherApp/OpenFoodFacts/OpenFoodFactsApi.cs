using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WasteWatcherApp.OpenFoodFacts
{
    public static class OpenFoodFactsApi
    {
        /// <summary>
        /// Get Data from OpenFoodfacts
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>Product object</returns>
        public static async Task<Product> GetProductDataByBarcode(string barcode)
        {
            string data = await ProductCache.GetDataWithCache(barcode, GetProductDataJsonByBarcode);

            JObject root = JObject.Parse(data);
            var fields = root.Value<JObject>("product");
            if (fields == null)
            {
                throw new ProductNotFoundException();
            }
            string productName = fields["product_name"]?.ToString();
            string brand = fields["brands"]?.ToString();
            string productImage = fields["image_front_url"]?.ToString();
            string package = fields["packaging"]?.ToString();
            string ecoScore = fields["ecoscore_grade"]?.ToString();

            Product prod = new Product(ProductName: productName, Brand: brand, Barcode: barcode, ProductImage: productImage, Package: package, EcoScore: ecoScore);
            return prod;
        }
        
        /// <summary>
        /// Function to retrieve the data from OpenFoodFacts api
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private static async Task<string> GetProductDataJsonByBarcode(string barcode)
        {
            string url = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
            HttpClient client = new();
            return await client.GetStringAsync(url);
        }
    }
}
