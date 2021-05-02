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
            
            switch (product.EcoScore)
            {
                case "1":
                    EcoImage.Source = "ecoscore_a.png";
                    Eco_Score.Text = product.EcoScore;
                    break;
                case "2":
                    EcoImage.Source = "Nutri_Score_B.png";
                    Eco_Score.Text = product.EcoScore;
                    break;
                case "3":
                    EcoImage.Source = "Nutri_Score_C.png";
                    Eco_Score.Text = product.EcoScore;
                    break;
                case "4":
                    EcoImage.Source = "Nutri_Score_D.png";
                    Eco_Score.Text = product.EcoScore;
                    break;
                case "5":
                    EcoImage.Source = "Nutri_Score_E.png";
                    Eco_Score.Text = product.EcoScore;
                    break;
                default:
                    Eco_Score.Text = "Kein Eco-Score angegeben!";
                    break;
            }
        }

    }
}