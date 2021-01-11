using Unity;

namespace RedisViewer.Core
{
    public static class UnityResolver
    {
        private static IUnityContainer _container;

        public static void SetContainer(IUnityContainer container)
        {
            _container = container;
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}