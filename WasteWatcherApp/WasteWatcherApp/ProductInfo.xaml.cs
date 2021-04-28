
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfo : ContentPage
    {
        Product product;

        public ProductInfo(Product prod)
        {
            InitializeComponent();
            this.product = prod;
            productPic.Source = prod.ProductImage;
            
            if (prod.Package == "")
            {
                package.Text = "Keine Informationen.";
            }
            else
            {
                package.Text = prod.Package;
            }
           
            Brand_and_Product.Text = $"{prod.Brand} - {prod.ProductName}";
        }
    }
}