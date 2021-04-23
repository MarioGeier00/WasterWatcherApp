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
        Product prod;

        public ProductInfo()
        {
            InitializeComponent();
           
        }
        public ProductInfo(Product prod)
        {
            InitializeComponent();
            this.prod = prod;
            productPic.Source = prod.ProductImage;
            
            if (prod.Package == "")
            {
                package.Text = "Keine Informationen.";
            }
            else
            {
                package.Text = prod.Package;
            }
           
            Brand_and_Product.Text = $"{prod.Brand} - {prod.ProductName}";

        }
    }
}