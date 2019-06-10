using Blayer.ClientSide.Interactive;
using Microsoft.Extensions.DependencyInjection;

namespace Blayer.ClientSide
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