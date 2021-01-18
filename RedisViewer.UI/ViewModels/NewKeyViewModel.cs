using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Validators;
using System.Threading.Tasks;

namespace RedisViewer.UI.ViewModels
{
    internal class NewKeyViewModel : ValidatableBindableBase<NewKeyViewModel>, INewKeyViewModel
    {
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private readonly IMessageService<NewKeyViewModel> _messageService;

        public NewKeyViewModel(IMessageService<NewKeyViewModel> messageService)
        {
            _messageService = messageService;

            Types = new string[] { "String", "List", "Set", "Zset", "Hash", "Stream" };
            Type = Types[0];

            SetValidator(new NewKeyViewModelValidator(), this);

            SaveCommand = new DelegateCommand(() =>
            {
                if (Validate())
                {
                    Task.Run(async () =>
                    {
                        DispatcherService.BeginInvoke(() => ShowLoading(true));

                        var success = false;

                        switch (Type)
                        {
                            case "String":
                                success = await _database.AddStringKeyAsync(Name.Trim(), Value.Trim());
                                break;

                            case "List":
                                success = await _database.AddListKeyAsync(Name.Trim(), Value.Trim());
                                break;

                            case "Set":
                                success = await _database.AddSetKeyAsync(Name.Trim(), Value.Trim());
                                break;

                            case "Zset":
                                success = await _database.AddZsetKeyAsync(Name.Trim(), Value.Trim(), Score.Value);
                                break;

                            case "Hash":
                                success = await _database.AddHashKeyAsync(Name.Trim(), Value.Trim(), Key.Trim());
                                break;

                            case "Stream":
                                break;
                        }

                        if (success)
                            DispatcherService.BeginInvoke(() => CloseDialog(ButtonResult.OK));
                        else
                            DispatcherService.BeginInvoke(() => _messageService.ShowAlert("Redis Viewer", "error"));
                    });
                }
            });

            CancelCommand = new DelegateCommand(() => CloseDialog(ButtonResult.Cancel));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                var database = parameters.GetValue<DatabaseInfo>("value");

                if (database != null)
                {
                    _database = database;
                }
            }
        }

        private DatabaseInfo _database;

        private string[] _types;
        public string[] Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private double? _score;
        public double? Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private string _id = "*";
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _key;
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }
    }
}