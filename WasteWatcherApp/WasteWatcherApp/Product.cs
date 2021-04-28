namespace WasteWatcherApp
{
    public class Product
    {

        public string ProductName { get; }
        public string Barcode { get; }
        public string Brand { get; }
        public string ProductImage { get; }
        public string Package { get; }

        public Product(string prodName, string barcode, string brand, string productImage, string package)
        {
            this.ProductName = prodName;
            this.Barcode = barcode;
            this.Brand = brand;
            this.ProductImage = productImage;
            this.Package = package;
        }

    }
}
