﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WasteWatcherApp
{
    public static class MessageService
    {
        private static IMessage _ToastService;

        public static IMessage ToastService
        {
            get
            {
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