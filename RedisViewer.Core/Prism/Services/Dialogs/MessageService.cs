using System.Linq;
using System.Windows;

namespace Prism.Services.Dialogs
{
    internal class MessageService<T> : IMessageService<T>
    {
        public void ShowAlert(string title, string message)
        {
            var view = new AlertView
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = GetWindow(typeof(T)),
                Title = title,
                Message = message
            };

            view.ShowDialog();
        }

        public ButtonResult ShowConfirm(string title, string message)
        {
            var view = new ConfirmView
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = GetWindow(typeof(T)),
                Title = title,
                Message = message
            };

            var result = view.ShowDialog();

            return result.HasValue ?
                (result.Value == true ? ButtonResult.OK : ButtonResult.Cancel) : ButtonResult.Cancel;
        }

        internal Window GetWindow(object type)
        {
            // Get window by datacontext, T is view model
            return Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(c => c.DataContext != null && c.DataContext.GetType().Equals(type)) ?? Application.Current.MainWindow;
        }
    }
}