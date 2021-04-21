﻿using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        // Button für Scanner
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Scan_Button.IsEnabled = false;
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status == PermissionStatus.Granted)
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
                if (result != null)
                {
                    Barcode.Text = result.Text;
                    try
                    {
                        Product prod = await GetDataFoodFacts(result.Text);
                        await Navigation.PushAsync(new ProductInfo(prod));
                    }
                    catch (ProductNotFoundException)
                    {
                        MessageService.ShowToastLong("Datenabruf für Produkt nicht möglich! Produkt nicht gefunden");
                    }
                    //TODO: Internet Fehlermeldung finden. Welche Exception wird geworfen bei fehlender Internetverbindung
                    catch (HttpRequestException)
                    {
                        MessageService.ShowToastLong("Datenabruf für Produkt nicht möglich! Internetverbindung prüfen");
                    }
                    catch (Exception)
                    {
                        MessageService.ShowToastLong("Sonstiger Fehler. Bitte Devs kontaktieren!!!!!1!");
                    }

                }
            }
            else
            {
                MessageService.ShowToastShort("Einscannen nur mit Kamerazugriff möglich.");
            }

            Scan_Button.IsEnabled = true;
        }

        private async Task<Product> GetDataFoodFacts(string barcode)
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
            Barcode.Text = $" {brand} - {productName} ";

            Product prod = new Product(prodName: productName, brand: brand, barcode: barcode, productImage: productImage, package: package);
            return prod;
        }
    }

    internal class ProductNotFoundException : Exception
    {
        
    }
}
