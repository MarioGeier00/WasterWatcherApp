﻿using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
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
                    GetDataFoodFacts(result.Text);
                }
            }
            else
            {
                MessageService.ShowToast("Einscannen nur mit Kamerazugriff möglich.");
            }
        }

        private async void  GetDataFoodFacts(string barcode)
        {
            string url = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
            HttpClient client = new HttpClient();
            string res = await client.GetStringAsync(url);

            JObject root = JObject.Parse(res);
            var fields = root.Value<JObject>("product");
            string productName = fields["product_name"].ToString();
            string brand = fields["brands"].ToString();
            Barcode.Text = $" {brand} - {productName} ";
        }
    }
}
