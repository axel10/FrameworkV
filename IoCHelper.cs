using Microsoft.Extensions.DependencyInjection;

namespace Root
{
    public class IoCHelper
    {
        private static readonly IServiceCollection serviceCollection = new ServiceCollection();

        public static void AddSingleton<T>(T o) where T : class
        {
            serviceCollection.AddSingleton(o);
        }

        public static T Resolve<T>() where T : class
        {
            var provider = serviceCollection.BuildServiceProvider();
            return provider.GetService<T>();
        }
    }
}