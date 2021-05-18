using WasteWatcherApp.Messaging;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public static class MessageService
    {
        static IMessage _ToastService;

        static IMessage ToastService
        {
            get
            {
                // Get IMessage implementation only once as soon as it is requested
                // to improve performance and decrease memory usage
                if (_ToastService == null)
                {
                    _ToastService = DependencyService.Get<IMessage>();
                }

                return _ToastService;
            }
        }

        public static void ShowToastShort(string message)
        {
            ToastService.ShortAlert(message);
        }

        public static void ShowToastLong(string message)
        {
            ToastService.LongAlert(message);
        }
    }
}
