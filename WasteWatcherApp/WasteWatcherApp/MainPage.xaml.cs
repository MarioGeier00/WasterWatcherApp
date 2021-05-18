using Acr.UserDialogs;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WasteWatcherApp.OpenFoodFacts;
using WasteWatcherApp.Product;
using WasteWatcherApp.Product.Persistance;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace WasteWatcherApp
{
    public partial class MainPage : ContentPage
    {
        public IProductSource<ProductData> ProductSource { get; }

        Task<ProductData> productLoadingTask;


        public MainPage(IProductSource<ProductData> productSource)
        {
            InitializeComponent();
            CachingSwitch.IsToggled = ProductCache.IsCachingEnabled;

            this.Appearing += MainPage_Appearing;
            ProductSource = productSource ?? throw new ArgumentNullException(nameof(productSource));
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


            string[] scannedBarcodes = ProductRequestStore.GetBarcodRequestsUntil(DateTime.Today);
            RequestCounterLabel.Text = scannedBarcodes.Length switch
            {
                0 => "Heute wurde noch kein Produkt eingescannt.",
                1 => $"Du hast heute einen Barcode gescannt.",
                _ => $"Heute wurden {scannedBarcodes.Length} Barcodes gescannt."
            };
                
        }

        async void ShowTestProduct_Clicked(object sender, EventArgs e)
        {
            var testProduct = new ProductData("WasteWatcher App", "1902398237497", "Technische Hochschule Nürnberg", "recycling.png", "Festplatte", "1");
            await Navigation.PushAsync(new ProductInfo(testProduct));
        }

        async void ScanButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                ToastService.ShowToastShort("Verbinde dich mit dem Internet");
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
                var scanner = new ZXing.Mobile.MobileBarcodeScanner()
                {
                    TopText = "Barcode Einscannen",
                };

                var scanResult = await scanner.Scan(new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    /*PossibleFormats = new List<BarcodeFormat>()
                    {
                        BarcodeFormat.All_1D
                    },*/
                });
                if (scanResult != null && scanResult.BarcodeFormat != BarcodeFormat.QR_CODE)
                {
                    // If Caching is enabled reduce minLoadingTime to zero
                    if (ProductCache.IsCachingEnabled)
                    {
                        productLoadingTask = LoadProduct(scanResult.Text, 0);
                    }
                    else
                    {
                        productLoadingTask = LoadProduct(scanResult.Text);
                    }

                    var product = await productLoadingTask;
                    productLoadingTask = null;

                    UserDialogs.Instance.HideLoading();
                    if (product != null)
                    {
                        await Navigation.PushAsync(new ProductInfo(product));
                    }
                }
                else
                {
                    ToastService.ShowToastLong("Einscannen war nicht möglich. Code nicht lesbar.");
                }
            }
            else
            {
                ToastService.ShowToastShort("Einscannen nur mit Kamerazugriff möglich.");
            }

            ScanButton.IsEnabled = true;
        }
        /// <summary>
        /// Load Product data and show loading screen 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="minLoadingTime"></param>
        /// <returns></returns>
        async Task<ProductData> LoadProduct(string productId, uint minLoadingTime = 500)
        {
            var minLoadingTimeTask = Task.Delay((int)minLoadingTime);
            ProductData result = null;

            try
            {
                result = await ProductSource.GetData(productId);
                if (!minLoadingTimeTask.IsCompleted)
                {
                    await minLoadingTimeTask;
                }
            }
            catch (ProductNotFoundException)
            {
                ToastService.ShowToastLong("Datenabruf für Produkt nicht möglich, das Produkt wurde nicht gefunden");
            }
            catch (HttpRequestException)
            {
                ToastService.ShowToastLong("Datenabruf für Produkt nicht möglich, bitte Internetverbindung prüfen");
            }
            catch (Exception ex)
            {
                bool isNetworkException = ex.GetType().FullName.ToLower().Contains("java.net");
                if (isNetworkException)
                {
                    ToastService.ShowToastLong("Datenabruf für Produkt nicht möglich, bitte Internetverbindung prüfen");
                }
                else
                {
                    ToastService.ShowToastLong($"Fehler beim Datenabruf: {ex.GetType().FullName}");
                }
            }

            return result;
        }


        private void CachingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            ProductCache.IsCachingEnabled = CachingSwitch.IsToggled;
        }
    }
}
