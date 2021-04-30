using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public partial class MainPage : ContentPage
    {

        Task<Product> productLoadingTask;


        public MainPage()
        {
            InitializeComponent();

            this.Appearing += MainPage_Appearing;
        }


        void MainPage_Appearing(object sender, EventArgs e)
        {
            // Show loading indicator as soon as this page appears
            // and the data fetching task is still in running
            if (productLoadingTask != null &&
                !productLoadingTask.IsCompleted)
            {
                UserDialogs.Instance.ShowLoading();
            }
        }

        async void ShowTestProduct_Clicked(object sender, EventArgs e)
        {
            var testProduct = new Product("WasteWatcher App", "1902398237497", "Technische Hochschule Nürnberg", "Recycle.png", "Festplatte");
            await Navigation.PushAsync(new ProductInfo(testProduct));
        }

        async void ScanButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                MessageService.ShowToastShort("Verbinde dich mit dem Internet");
                return;
            }


            ScanButton.IsEnabled = false;
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status == PermissionStatus.Granted)
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                var scanResult = await scanner.Scan();
                if (scanResult != null)
                {
                    productLoadingTask = LoadProduct(scanResult.Text);

                    var product = await productLoadingTask;
                    productLoadingTask = null;

                    UserDialogs.Instance.HideLoading();
                    if (product != null)
                    {
                        await Navigation.PushAsync(new ProductInfo(product));
                    }
                }
            }
            else
            {
                MessageService.ShowToastShort("Einscannen nur mit Kamerazugriff möglich.");
            }

            ScanButton.IsEnabled = true;
        }


        async Task<Product> LoadProduct(string productId)
        {
            var minLoadingTimeTask = Task.Delay(500);
            Product result = null;

            try
            {
                result = await GetDataFoodFacts(productId);
                if (!minLoadingTimeTask.IsCompleted)
                {
                    await minLoadingTimeTask;
                }
            }
            catch (ProductNotFoundException)
            {
                MessageService.ShowToastLong("Datenabruf für Produkt nicht möglich, das Produkt wurde nicht gefunden");
            }
            catch (HttpRequestException)
            {
                MessageService.ShowToastLong("Datenabruf für Produkt nicht möglich, bitte Internetverbindung prüfen");
            }
            catch (Exception ex)
            {
                bool isNetworkException = ex.GetType().FullName.ToLower().Contains("java.net");
                if (isNetworkException)
                {
                    MessageService.ShowToastLong("Datenabruf für Produkt nicht möglich, bitte Internetverbindung prüfen");
                }
                else
                {
                    MessageService.ShowToastLong($"Fehler beim Datenabruf: {ex.GetType().FullName}");
                }
            }

            return result;
        }

        async Task<Product> GetDataFoodFacts(string barcode)
        {
            string url = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
            HttpClient client = new HttpClient();
            string res = await client.GetStringAsync(url);

            JObject root = JObject.Parse(res);
            var fields = root.Value<JObject>("product");
            if (fields == null)
            {
                throw new ProductNotFoundException();
            }
            string productName = fields["product_name"]?.ToString();
            string brand = fields["brands"]?.ToString();
            string productImage = fields["image_front_url"]?.ToString();
            string package = fields["packaging"]?.ToString();

            Product prod = new Product(prodName: productName, brand: brand, barcode: barcode, productImage: productImage, package: package);
            return prod;
        }

    }
}
