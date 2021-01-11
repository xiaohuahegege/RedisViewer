using Prism.Ioc;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prism.Services.Dialogs
{
    /// <summary>
    /// Dialog service
    /// </summary>
    internal class RegionDialogService : IRegionDialogService
    {
        private readonly IContainerExtension _containerExtension;

        public RegionDialogService(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public void Show(string name, IDialogParameters dialogParameters, Action loadedCallback, Action<IDialogResult> closedCallback)
        {
            var window = _containerExtension.Resolve<IDialogWindow>(name);
            var dialogAware = window.DataContext as IDialogAware;

            dialogAware.OnDialogOpened(dialogParameters);

            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (result) =>
            {
                window.Result = result;
                window.Close();
            };

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (e.Cancel)
                    return;

                if (!dialogAware.CanCloseDialog())
                    e.Cancel = true;
            };
            window.Closing += closingHandler;

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                loadedCallback?.Invoke();

                window.Loaded -= loadedHandler;
                dialogAware.RequestClose += requestCloseHandler;
            };
            window.Loaded += loadedHandler;

            EventHandler closedHandler = null;
            closedHandler = (o, e) =>
            {
                window.Closed -= closedHandler;
                window.Closing -= closingHandler;
                dialogAware.RequestClose -= requestCloseHandler;

                dialogAware.OnDialogClosed();

                var result = window.Result ?? new DialogResult();
                closedCallback.Invoke(result);

                window.DataContext = null;
                window.Content = null;
            };
            window.Closed += closedHandler;
            window.Show();
        }

        public void ShowDialog(string name, IDialogParameters dialogParameters, Action<IDialogResult> callback)
        {
            var window = _containerExtension.Resolve<IDialogWindow>(name);
            var dialogAware = window.DataContext as IDialogAware;

            dialogAware.OnDialogOpened(dialogParameters);

            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (result) =>
            {
                window.Result = result;
                window.Close();
            };

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (!dialogAware.CanCloseDialog())
                    e.Cancel = true;
            };
            window.Closing += closingHandler;

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                window.Loaded -= loadedHandler;
                dialogAware.RequestClose += requestCloseHandler;
            };
            window.Loaded += loadedHandler;

            EventHandler closedHandler = null;
            closedHandler = (o, e) =>
            {
                window.Closed -= closedHandler;
                window.Closing -= closingHandler;
                dialogAware.RequestClose -= requestCloseHandler;

                dialogAware.OnDialogClosed();

                var result = window.Result;
                if (result == null)
                    result = new DialogResult();

                callback.Invoke(result);

                window.DataContext = null;
                window.Content = null;
            };
            window.Closed += closedHandler;

            if (window.Owner == null)
                window.Owner = Application.Current.MainWindow;

            window.ShowDialog();
        }
    }
}