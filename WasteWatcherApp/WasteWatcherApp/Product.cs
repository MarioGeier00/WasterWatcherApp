namespace WasteWatcherApp
{

    public record Product(string ProductName, string Barcode, string Brand, string ProductImage, string Package, string EcoScore);


    // Before C# 9.0 the class Product with its properties
    // had to be defined like this:

    // public class Product
    // {

    //     public string ProductName { get; }
    //     public string Barcode { get; }
    //     public string Brand { get; }
    //     public string ProductImage { get; }
    //     public string Package { get; }
    //     public string EcoScore { get; }

    //     public Product(string prodName, string barcode, string brand, string productImage, string package, string ecoScore)
    //     {
    //         this.ProductName = prodName;
    //         this.Barcode = barcode;
    //         this.Brand = brand;
    //         this.ProductImage = productImage;
    //         this.Package = package;
    //         this.EcoScore = ecoScore;
    //     }

    // }

}
