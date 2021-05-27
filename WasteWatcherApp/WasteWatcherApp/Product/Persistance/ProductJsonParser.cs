using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    public record ProductJsonParser(IProductSource<string> JsonProductSource) : IProductSource<ProductData>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<ProductData> GetData(string barcode)
        {
            string jsonData = await JsonProductSource.GetData(barcode);
            return Parse(jsonData, barcode);
        }


        /// <summary>
        /// Converts a given json object from OpenFoodFacts into a <see cref="ProductData"/> object.
        /// </summary>
        /// <param name="jsonData">The string in json format</param>
        /// <param name="barcode">The barcode of the product</param>
        /// <returns>An instance of <see cref="ProductData"/> with the values from the json data</returns>
        /// <exception cref="ProductNotFoundException"></exception>
        public static ProductData Parse(string jsonData, string barcode)
        {
            JObject root = JObject.Parse(jsonData);
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

            ProductData prod = new(ProductName: productName, Brand: brand, Barcode: barcode, ProductImage: productImage, Package: package, EcoScore: ecoScore);

            return prod;
        }

    }
}
