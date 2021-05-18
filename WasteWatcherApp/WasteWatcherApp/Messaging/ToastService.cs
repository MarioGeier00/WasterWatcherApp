using WasteWatcherApp.Messaging;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public static class ToastService
    {
        static IMessage _service;

        static IMessage Service
        {
            get
            {
                // Get IMessage implementation only once as soon as it is requested
                // to improve performance and decrease memory usage
                return _service ??= DependencyService.Get<IMessage>();
            }
        }

        public static void ShowToastShort(string message)
        {
            Service.ShortAlert(message);
        }

        public static void ShowToastLong(string message)
        {
            Service.LongAlert(message);
        }
    }
}
