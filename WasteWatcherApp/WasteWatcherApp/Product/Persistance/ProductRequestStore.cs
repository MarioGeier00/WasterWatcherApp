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


        /// <summary>
        /// Load the barcodes that were requested since the minimumDate.
        /// </summary>
        /// <param name="minimumDate">The minimum date when barcodes are still relevant</param>
        /// <returns>An array of barcode strings</returns>
        public static string[] GetBarcodeRequestsSince(DateTime minimumDate)
        {
            List<string> barcodes = new();

            foreach (var item in App.Current.Properties)
            {
                if (item.Key.EndsWith(SAVE_DATE_KEY))
                {
                    DateTime requestDate = DateTime.Parse((string)item.Value);
                    if (requestDate > minimumDate)
                    {
                        barcodes.Add(item.Key.Replace(SAVE_DATE_KEY, string.Empty));
                    }
                }
            }

            return barcodes.ToArray();
        }


        /// <summary>
        /// Saves the requested barcode and the given date.
        /// </summary>
        /// <param name="barcode">The barcode to use as part of the key</param>
        /// <param name="dateTime">The date which is saved as a string value</param>
        void SaveRequestDate(string barcode, DateTime dateTime)
        {
            App.Current.Properties[$"{barcode}{SAVE_DATE_KEY}"] = dateTime.ToString();
            _ = App.Current.SavePropertiesAsync();
        }
    }
}
