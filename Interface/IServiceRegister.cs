using Microsoft.Extensions.DependencyInjection;

namespace Root
{
    public interface IServiceRegister
    {
        void Register(IServiceCollection services);
    }
}