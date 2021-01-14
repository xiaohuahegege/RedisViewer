using Prism.Commands;
using Prism.Services.Dialogs;

namespace RedisViewer.UI.ViewModels
{
    /// <summary>
    /// Key view model (string)
    /// </summary>
    internal class KeyStringViewModel : KeyTypeViewModelBase, IKeyStringViewModel
    {
        private readonly IMessageService<KeyStringViewModel> _messageService;

        /// <summary>
        /// Save key value command
        /// </summary>
        public DelegateCommand SaveCommand { get; }

        public KeyStringViewModel(IMessageService<KeyStringViewModel> messageService)
        {
            _messageService = messageService;

            // Save key value command
            SaveCommand = new DelegateCommand(async () =>
            {
                var success = await Key.SetByStringAsync(Value);
                _messageService.ShowAlert("Redis Viewer", success ? "Value updated successfully" : "Value update failed");
            });
        }

        public async override void PageLoad()
        {
            ShowLoading(true);
            Value = await Key.GetValueByStringAsync();
            ShowLoading(false);
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}