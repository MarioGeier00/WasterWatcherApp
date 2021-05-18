using WasteWatcherApp.Product.Persistance;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            OpenFoodFactsProductSource apiSource = new();
            ProductCache productCacheWithApiSource = new(apiSource);

            ProductJsonParser parsedProductSource = new(productCacheWithApiSource);
            MainPage = new NavigationPage(new MainPage(parsedProductSource));
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
