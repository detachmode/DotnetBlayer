using DotnetBlayer;
using Microsoft.Extensions.DependencyInjection;

namespace Blayer.Shared
{
    public static class Extension
    {
          public static void AddBlayerShared(this IServiceCollection services)
        {
            services.AddSingleton<Api>();
            services.AddSingleton<Interactive<Api>>();
            services.AddHttpClient();
        }
    }
}