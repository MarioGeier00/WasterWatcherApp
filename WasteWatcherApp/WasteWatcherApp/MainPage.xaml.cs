using Acr.UserDialogs;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WasteWatcherApp.Firebase;
using WasteWatcherApp.Product;
using WasteWatcherApp.Product.Persistance;
using WasteWatcherApp.Waste;
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

        /// <summary>
        /// Is called everytime the MainPage is visible again on the screen
        /// </summary>
        void MainPage_Appearing(object sender, EventArgs e)
        {
            // Show loading indicator as soon as this page appears
            // and the data fetching task is still running
            if (productLoadingTask != null &&
                !productLoadingTask.IsCompleted)
            {
                UserDialogs.Instance.ShowLoading();
            }

            _ = LoadWasteStatisticsAsync();
        }

        /// <summary>
        /// Shows the ProductData Page with test data to check the page layout.
        /// </summary>
        async void ShowTestProduct_Clicked(object sender, EventArgs e)
        {
            var testProduct = new ProductData("WasteWatcher App", "1902398237497", "Technische Hochschule Nürnberg", "recycling.png", "Festplatte", "1");
            await Navigation.PushAsync(new ProductInfo(testProduct));
        }

        /// <summary>
        /// Changes the caching settting
        /// </summary>
        void CachingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            ProductCache.IsCachingEnabled = CachingSwitch.IsToggled;
        }

        /// <summary>
        /// Starts the scanning process by verifying internet access and camera access,
        /// opens the ZXing barcode scanner and checks that the scanned barcode is valid.
        /// Finally calls the <see cref="LoadProduct"/> method.
        /// </summary>
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

                var scanResult = await scanner.Scan();
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
        /// Load product data and show loading screen.
        /// </summary>
        /// <param name="productId">The id of the product, which is a barcode string</param>
        /// <param name="minLoadingTime">The minimum time span how long the loading spinner should be displayed</param>
        /// <returns>The product data</returns>
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

        /// <summary>
        /// Loads waste statistics for the total waste usage for the current day.
        /// </summary>
        /// <returns>The background task of fetching the waste data</returns>
        async Task LoadWasteStatisticsAsync()
        {
            string[] scannedBarcodes = ProductRequestStore.GetBarcodeRequestsSince(DateTime.Today);
            RequestCounterLabel.Text = scannedBarcodes.Length switch
            {
                0 => "Heute wurde noch kein Produkt eingescannt.",
                1 => $"Du hast heute einen Barcode gescannt.",
                _ => $"Heute wurden {scannedBarcodes.Length} Barcodes gescannt."
            };

            Firestore wasteSource = new();
            WasteCollection waste = new();
            WasteCollection currentWaste;


            foreach (var barcode in scannedBarcodes)
            {
                currentWaste = null;
                // Tries to get the wasteData for the barcode
                try
                {
                    currentWaste = await wasteSource.GetData(barcode);
                }
                catch
                {
                    // The wasteData couldn't be loaded, but we don't care
                    // because the waste statistics isn't crucial for using the app
                }
                if (currentWaste != null)
                {
                    waste += currentWaste;
                }
            }

            WasteStatisticsLabel.Text = waste.ToString();
        }
    }
}
