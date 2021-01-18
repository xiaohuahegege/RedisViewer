namespace Prism.Services.Dialogs
{
    public interface IMessageService<T>
    {
        void ShowAlert(string title, string message);
        ButtonResult ShowConfirm(string title, string message);
    }
}