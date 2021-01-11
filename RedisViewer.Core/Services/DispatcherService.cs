using System;
using System.Windows;
using System.Windows.Threading;

namespace RedisViewer.Core
{
    public static class DispatcherService
    {
        public static void BeginInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Background)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(priority, action);
            }
        }

        public static void Invoke(Action action, DispatcherPriority priority = DispatcherPriority.Background)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher == null || dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(priority, action);
        }
    }
}