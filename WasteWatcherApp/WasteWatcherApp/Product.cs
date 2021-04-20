using System;
using System.Collections.Generic;
using System.Text;

namespace WasteWatcherApp
{
   public class Product
    {
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string Brand { get; set; }
        
        public Product(string prodName, string barcode, string brand)
        {
            this.ProductName = prodName;
            this.Barcode = barcode;
            this.Brand = brand;
        }

    }
}
