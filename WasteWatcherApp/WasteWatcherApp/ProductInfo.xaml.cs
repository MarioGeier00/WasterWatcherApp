using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfo : ContentPage
    {
        public ProductInfo()
        {
            InitializeComponent();
            testOut.Text = $"HALL0 anderer Konstruktor!!!";

        }
        public ProductInfo(Product prod )
        {
            InitializeComponent();
            testOut.Text = $"{prod.Brand} - {prod.ProductName}";

        }
    }
}