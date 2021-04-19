using System;
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
                }
            }
            else
            {
                MessageService.ShowToast("Einscannen nur mit Kamerazugriff möglich.");
            }
        }
    }
}
