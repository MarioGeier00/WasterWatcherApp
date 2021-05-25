using WasteWatcherApp.Product.Persistance;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public partial class App : Application
    {
        /// <summary>
        /// Initially called to create the application and the first page to display.
        /// Creates a source chain <see cref="OpenFoodFactsProductSource"/> -> <see cref="ProductCache"/> -> <see cref="ProductJsonParser"/> -> <see cref="ProductRequestStore"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Creates a new instance of the OpenFoodFactsApi and adds a ProductCache on top of that
            OpenFoodFactsProductSource apiSource = new();
            ProductCache productCacheWithApiSource = new(apiSource);

            // Maps the json string from the OpenFoodFactsApi and the store to an
            // instance of the class ProductData
            ProductJsonParser parsedProductSource = new(productCacheWithApiSource);

            // The request store is used to save the requested barcode and time to the local storage
            // to provide waste statistics like total waste consumption
            ProductRequestStore requestStore = new(parsedProductSource);
            MainPage = new NavigationPage(new MainPage(requestStore));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
