using System;
using System.Threading.Tasks;
using WasteWatcherApp.helper;
using WasteWatcherApp.Waste;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfo : ContentPage
    {
        private static IWasteStore WasteStore { get; } = new Firestore();

        public Product Product { get; }

        public ProductInfo(Product product)
        {
            InitializeComponent();
            Product = product;
            this.Title = product.ProductName;


            ProductImage.Source = product.ProductImage;

            Package.Text = product.Package;
            Brand.Text = product.Brand;

            bool hasBrand = product.Brand != null || product.Brand.Length > 0;
            BrandContainer.IsVisible = hasBrand;
            BrandUnavailableContainer.IsVisible = !hasBrand;

            string imageFileName = product.EcoScore switch
            {
                "1" => "eco_score_a.png",
                "2" => "eco_score_b.png",
                "3" => "eco_score_c.png",
                "4" => "eco_score_d.png",
                "5" => "eco_score_e.png",
                _ => string.Empty
            };

            EcoImage.Source = imageFileName;
            if (string.IsNullOrEmpty(imageFileName))
            {
                EcoScoreContainer.IsVisible = false;
                EcoScoreUnavailableContainer.IsVisible = !EcoScoreContainer.IsVisible;
            }

            LoadWasteData();
        }

        private async void LoadWasteData()
        {
            WasteCollection wasteData = await WasteStore.GetData(Product.Barcode);

            this.Package.Text = wasteData?.ToString() ?? "Keine Daten gefunden";
            if (wasteData is not null)
            {
                AddWasteInfo.Text = "Daten ändern";
            }
        }

        private async void EcoScoreInfoButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://de.openfoodfacts.org", BrowserLaunchMode.SystemPreferred);
        }

        private async void AddWasteInfo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditWasteInfo(Product, WasteStore));
            LoadWasteData();
        }
    }
}