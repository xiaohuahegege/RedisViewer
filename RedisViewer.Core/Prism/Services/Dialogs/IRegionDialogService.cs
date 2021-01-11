using System;

namespace Prism.Services.Dialogs
{
    public interface IRegionDialogService
    {
        void Show(string name, IDialogParameters dialogParameters, Action loadedCallback, Action<IDialogResult> closedCallback);
        void ShowDialog(string name, IDialogParameters dialogParameters, Action<IDialogResult> callback);
    }
}