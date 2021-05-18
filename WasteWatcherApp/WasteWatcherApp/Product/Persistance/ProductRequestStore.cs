using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    record ProductRequestStore(IProductSource<ProductData> ProductSource) : IProductSource<ProductData>
    {
        const string SAVE_DATE_KEY = "-sd";

        public Task<ProductData> GetData(string barcode)
        {
            SaveRequestDate(barcode, DateTime.Now);
            return ProductSource.GetData(barcode);
        }

        public static string[] GetBarcodRequestsUntil(DateTime expirationDate)
        {
            List<string> barcodes = new();

            foreach (var item in App.Current.Properties)
            {
                if (item.Key.EndsWith(SAVE_DATE_KEY))
                {
                    DateTime requestDate = DateTime.Parse((string)item.Value);
                    if (requestDate > expirationDate)
                    {
                        barcodes.Add(item.Key.Replace(SAVE_DATE_KEY, string.Empty));
                    }
                }
            }

            return barcodes.ToArray();
        }


        void SaveRequestDate(string barcode, DateTime dateTime)
        {
            App.Current.Properties[$"{barcode}{SAVE_DATE_KEY}"] = dateTime.ToString();
            _ = App.Current.SavePropertiesAsync();
        }
    }
}
