using Prism.Ioc;
using Prism.Services.Dialogs;
using Unity;

namespace RedisViewer.Core
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer AddServices(this IUnityContainer container)
        {
            container.RegisterSingleton<IRegionDialogService, RegionDialogService>();
            container.RegisterSingleton<IMessageService, MessageService>();
            container.RegisterSingleton<IConnectionService, ConnectionService>();
            return container;
        }
    }
}