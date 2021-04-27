using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.Black
            };
        
        }

        protected override void OnStart()
        {
            var current = Connectivity.NetworkAccess;
            
            if (current != NetworkAccess.Internet)
            {
                MessageService.ShowToastLong("Verbinde dich mit dem Internet");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
