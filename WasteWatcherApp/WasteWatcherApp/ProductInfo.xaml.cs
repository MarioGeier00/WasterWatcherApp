using System;
using WasteWatcherApp.Firebase;
using WasteWatcherApp.Product;
using WasteWatcherApp.Waste;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfo : ContentPage
    {
        /// <summary>
        /// Set Reference to Firestore implementation to get WasteData
        /// </summary>
        static IWasteStore WasteStore { get; } = new Firestore();

        EditWasteInfo editWasteInfoPage;

        public ProductData Product { get; }


        public ProductInfo(ProductData product)
        {
            InitializeComponent();
            Product = product;
            this.Title = product.ProductName;


            ProductImage.Source = product.ProductImage;

            Package.Text = product.Package;
            Brand.Text = product.Brand;

            bool noBrand = string.IsNullOrEmpty(product.Brand);
            BrandContainer.IsVisible = !noBrand;
            BrandUnavailableContainer.IsVisible = noBrand;

            // Use of new C# 9.0 switch assignment
            // Checks the values of the EcoScore Property
            // and assigns an according image file name to the variable
            string imageFileName = product.EcoScore switch
            {
                "1" => "eco_score_a.png",
                "2" => "eco_score_b.png",
                "3" => "eco_score_c.png",
                "4" => "eco_score_d.png",
                "5" => "eco_score_e.png",
                "a" => "eco_score_a.png",
                "b" => "eco_score_b.png",
                "c" => "eco_score_c.png",
                "d" => "eco_score_d.png",
                "e" => "eco_score_e.png",
                _ => string.Empty
            };

            EcoImage.Source = imageFileName;
            if (string.IsNullOrEmpty(imageFileName))
            {
                EcoScoreContainer.IsVisible = false;
                EcoScoreUnavailableContainer.IsVisible = !EcoScoreContainer.IsVisible;
            }

            this.Appearing += MainPage_Appearing;
        }

        /// <summary>
        /// Is called everytime the ProductInfoPage is visible again on the screen
        /// </summary>
        void MainPage_Appearing(object sender, EventArgs e)
        {
            LoadWasteData(editWasteInfoPage?.WasteData);
            editWasteInfoPage = null;
        }


        /// <summary>
        /// Uses the waste store the get WasteData for the product's barcode and displays it to the user.
        /// </summary>
        /// <param name="wasteData">Optional value to immediately load the WasteData from.
        /// The WasteData store get method is not called when a value is supplied here</param>
        async void LoadWasteData(WasteCollection wasteData = null)
        {
            wasteData ??= await WasteStore.GetData(Product.Barcode);

            this.Package.Text = wasteData?.ToString() ?? "Keine Daten gefunden";
            if (wasteData is not null)
            {
                AddWasteInfo.Text = "Daten ändern";
            }
        }

        /// <summary>
        /// Opens the browser to show the OpenFoodFacts website so that the user
        /// can read about the EcoScore.
        /// </summary>
        async void EcoScoreInfoButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://de.openfoodfacts.org", BrowserLaunchMode.SystemPreferred);
        }

        /// <summary>
        /// Opens the EditWasteInfo page to edit the waste information for the current product.
        /// </summary>
        async void AddWasteInfo_Clicked(object sender, EventArgs e)
        {
            editWasteInfoPage = new EditWasteInfo(Product, WasteStore);
            await Navigation.PushAsync(editWasteInfoPage);
        }
    }
}