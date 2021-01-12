using Prism.Ioc;
using Prism.Services.Dialogs;
using Unity;
using Unity.Lifetime;

namespace RedisViewer.Core
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer AddServices(this IUnityContainer container)
        {
            container.RegisterSingleton<IRegionDialogService, RegionDialogService>();
            container.RegisterType(typeof(IMessageService<>), typeof(MessageService<>), new TransientLifetimeManager());
            container.RegisterSingleton<IConnectionService, ConnectionService>();
            return container;
        }
    }
}