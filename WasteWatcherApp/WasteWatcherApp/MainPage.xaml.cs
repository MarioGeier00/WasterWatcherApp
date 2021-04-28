﻿using Acr.UserDialogs;
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


        async void Button_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                MessageService.ShowToastShort("Verbinde dich mit dem Internet");
                return;
            }


            Scan_Button.IsEnabled = false;
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

            Scan_Button.IsEnabled = true;
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
            //TODO: Internet Fehlermeldung finden. Welche Exception wird geworfen bei fehlender Internetverbindung
            catch (HttpRequestException)
            {
                MessageService.ShowToastLong("Datenabruf für Produkt nicht möglich! Internetverbindung prüfen");
            }
            catch (Exception)
            {
                MessageService.ShowToastLong("Fehler beim Datenabruf");
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

    internal class ProductNotFoundException : Exception
    {

    }
}
