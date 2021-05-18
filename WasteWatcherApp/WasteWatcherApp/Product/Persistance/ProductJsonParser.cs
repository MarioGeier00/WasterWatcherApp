using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    public record ProductJsonParser(IProductSource<string> JsonProductSource) : IProductSource<ProductData>
    {
        public async Task<ProductData> GetData(string barcode)
        {
            string jsonData = await JsonProductSource.GetData(barcode);
            return Parse(jsonData, barcode);
        }

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
