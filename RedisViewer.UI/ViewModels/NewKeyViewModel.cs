using Prism.Mvvm;
using Prism.Services.Dialogs;
using RedisViewer.Core;

namespace RedisViewer.UI.ViewModels
{
    internal class NewKeyViewModel : ValidatableBindableBase<NewKeyViewModel>, INewKeyViewModel
    {
        public NewKeyViewModel()
        {

        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("database_info"))
            {
                var database = parameters.GetValue<DatabaseInfo>("database_info");

                if (database != null)
                {

                }
            }
        }

        private DatabaseInfo _database;
    }
}