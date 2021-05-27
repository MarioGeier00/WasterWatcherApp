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

        /// <summary>
        /// Shows a short toast message on the bottom of the screen.
        /// </summary>
        /// <param name="message">The message to display to the user</param>
        public static void ShowToastShort(string message)
        {
            Service.ShortAlert(message);
        }

        /// <summary>
        /// Shows a long toast message on the bottom of the screen.
        /// </summary>
        /// <param name="message">The message to display to the user</param>
        public static void ShowToastLong(string message)
        {
            Service.LongAlert(message);
        }
    }
}
