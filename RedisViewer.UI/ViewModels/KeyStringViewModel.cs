using Prism.Commands;

namespace RedisViewer.UI.ViewModels
{
    /// <summary>
    /// Key view model (string)
    /// </summary>
    internal class KeyStringViewModel : KeyTypeViewModelBase, IKeyStringViewModel
    {
        /// <summary>
        /// Save key value command
        /// </summary>
        public DelegateCommand SaveCommand { get; }

        public KeyStringViewModel()
        {
            // Save key value command
            SaveCommand = new DelegateCommand(async () =>
            {
                await Key.SetByStringAsync(Value);
            });
        }

        public async override void PageLoad()
        {
            // Get string key value
            Value = await Key.GetValueByStringAsync();
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}