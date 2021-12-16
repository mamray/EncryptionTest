using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Services;

namespace EncryptionService.Core.Extensions
{
    public static class DataEncryptionCoreExtension
    {
        public static IServiceCollection AddDataEncryptionCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IKeyStorage, KeyStorage>();
            services.AddTransient<IDataEncryptionService, DataEncryptioService>();            
            return services;
        }
    }
}
