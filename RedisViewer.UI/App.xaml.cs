using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Unity;
using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using RedisViewer.UI.Views;
using System.Windows;
using Unity;

namespace RedisViewer.UI
{
    sealed partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            // Register services
            container.AddServices();

            // Register view models
            container.RegisterType<INewConnectionViewModel, NewConnectionViewModel>();
            container.RegisterType<IKeyHashViewModel, KeyHashViewModel>();
            container.RegisterType<IKeyListViewModel, KeyListViewModel>();
            container.RegisterType<IKeySetViewModel, KeySetViewModel>();
            container.RegisterType<IKeyStreamViewModel, KeyStreamViewModel>();
            container.RegisterType<IKeyStringViewModel, KeyStringViewModel>();
            container.RegisterType<IKeyZsetViewModel, KeyZsetViewModel>();
            container.RegisterType<IKeyViewerViewModel, KeyViewerViewModel>();
            container.RegisterType<INewKeyViewModel, NewKeyViewModel>();
            container.RegisterType<ILeftNavViewModel, LeftNavViewModel>();
            container.RegisterType<IShellViewModel, ShellViewModel>();
            container.RegisterType<IServerInfoViewModel, ServerInfoViewModel>();

            // Register dialogs
            container.RegisterType<IDialogWindow, NewConnectionView>(nameof(NewConnectionView));
            container.RegisterType<IDialogWindow, NewKeyView>(nameof(NewKeyView));

            // Register view for dialog services
            containerRegistry.RegisterForNavigation<KeyHashView>(nameof(KeyHashView));
            containerRegistry.RegisterForNavigation<KeyListView>(nameof(KeyListView));
            containerRegistry.RegisterForNavigation<KeySetView>(nameof(KeySetView));
            containerRegistry.RegisterForNavigation<KeyStreamView>(nameof(KeyStreamView));
            containerRegistry.RegisterForNavigation<KeyStringView>(nameof(KeyStringView));
            containerRegistry.RegisterForNavigation<KeyZsetView>(nameof(KeyZsetView));
            containerRegistry.RegisterForNavigation<KeyViewerView>(nameof(KeyViewerView));
            containerRegistry.RegisterForNavigation<HomeView>(nameof(HomeView));
            containerRegistry.RegisterForNavigation<ServerInfoView>(nameof(ServerInfoView));

            UnityResolver.SetContainer(container);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }
    }
}