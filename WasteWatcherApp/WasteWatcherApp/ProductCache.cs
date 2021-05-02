using System;
using System.Threading.Tasks;

namespace WasteWatcherApp
{
    static class ProductCache
    {
        private const string SETTINGS_KEY = "ProductCaching";
        private static bool isCachingEnabled;

        public static bool IsCachingEnabled
        {
            get => isCachingEnabled;
            set
            {
                App.Current.Properties["ProductCaching"] = value;
                isCachingEnabled = value;
            }
        }


        static ProductCache()
        {
            object settings = App.Current.Properties[SETTINGS_KEY];
            if (settings != null && settings.GetType() == typeof(bool))
            {
                isCachingEnabled = (bool)settings;
            }
            else
            {
                isCachingEnabled = true;
            }
        }

        public static async Task<string> GetDataWithCache(string barcode, Func<string, Task<string>> fetchCall)
        {
            object savedData = App.Current.Properties[barcode];
            if (savedData != null && savedData.GetType() == typeof(string))
            {
                return (string)savedData;
            }
            else
            {
                string result = await fetchCall(barcode);
                App.Current.Properties[barcode] = result;
                // Do not await save call because it doesn't matter
                // whether the call succeeds or not
                _ = App.Current.SavePropertiesAsync();
                return result;
            }
        }

    }
}
