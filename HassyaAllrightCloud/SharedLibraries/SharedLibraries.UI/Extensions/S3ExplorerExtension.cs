using Microsoft.Extensions.DependencyInjection;
using SharedLibraries.UI.Services;

namespace SharedLibraries.UI.Extensions
{
    public static class SharedLibrariesExtension
    {
        public static void AddLibServices(this IServiceCollection services)
        {
            services.AddTransient<IKoboSimpleGridService, KoboSimpleGridService>();
            services.AddTransient<ISharedLibrariesApi, SharedLibrariesApi>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
        }
    }
}
