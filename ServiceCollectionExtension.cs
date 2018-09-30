using Microsoft.Extensions.DependencyInjection;

namespace Root
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigBaseController(this IServiceCollection service, IBootOptions bootOptions)
        {
            IoCHelper.AddSingleton(bootOptions);
        }
    }
}