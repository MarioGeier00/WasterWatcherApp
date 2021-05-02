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

            bool hasBrand = product.Brand != null || product.Brand.Length > 0;
            BrandContainer.IsVisible = hasBrand;
            BrandUnavailableContainer.IsVisible = !hasBrand;

            switch (product.EcoScore)
            {
                case "1":
                    EcoImage.Source = "eco_score_a.png";
                    break;
                case "2":
                    EcoImage.Source = "eco_score_b.png";
                    break;
                case "3":
                    EcoImage.Source = "eco_score_c.png";
                    break;
                case "4":
                    EcoImage.Source = "eco_score_d.png";
                    break;
                case "5":
                    EcoImage.Source = "eco_score_e.png";
                    break;
                default:
                    EcoScoreContainer.IsVisible = false;
                    EcoScoreUnavailableContainer.IsVisible = !EcoScoreContainer.IsVisible;
                    break;
            }
        }

    }
}