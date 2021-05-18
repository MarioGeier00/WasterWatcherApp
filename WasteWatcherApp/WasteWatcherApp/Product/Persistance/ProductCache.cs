using System.Threading.Tasks;

namespace WasteWatcherApp.Product.Persistance
{
    class ProductCache : IProductSource<string>
    {
        private const string SETTINGS_KEY = "ProductCaching";
        private static bool isCachingEnabled;

        public static bool IsCachingEnabled
        {
            get => isCachingEnabled;
            set
            {
                App.Current.Properties["ProductCaching"] = value;
                _ = App.Current.SavePropertiesAsync();
                isCachingEnabled = value;
            }
        }


        static ProductCache()
        {
            if (App.Current.Properties.TryGetValue(SETTINGS_KEY, out object settings) &&
                settings.GetType() == typeof(bool))
            {
                isCachingEnabled = (bool)settings;
            }
            else
            {
                isCachingEnabled = true;
            }
        }



        public IProductSource<string> Fallback { get; }

        public ProductCache(IProductSource<string> fallback = null)
        {
            Fallback = fallback;
        }

        public async Task<string> GetData(string barcode)
        {
            if (!IsCachingEnabled)
            {
                if (Fallback != null)
                {
                    return await Fallback.GetData(barcode);
                }
                throw new ProductNotFoundException();
            }

            if (App.Current.Properties.TryGetValue(barcode, out object savedData) &&
                savedData.GetType() == typeof(string))
            {
                return (string)savedData;
            }
            else
            {
                string result = await Fallback.GetData(barcode);
                App.Current.Properties[barcode] = result;
                // Do not await save call because it doesn't matter
                // whether the call succeeds or not
                _ = App.Current.SavePropertiesAsync();
                return result;
            }
        }
    }
}
