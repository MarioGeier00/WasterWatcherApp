using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfo : ContentPage
    {

        public ProductInfo(Product product)
        {
            InitializeComponent();
            this.Title = product.ProductName;


            ProductImage.Source = product.ProductImage;

            Package.Text = product.Package;
            Brand.Text = product.Brand;
        }

    }
}