using System;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConverterLibrary
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConverterLibrary(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<Serializer>(GetYamlSerializer());

            return services;
        }

        private static Serializer GetYamlSerializer()
        {
            return new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        }
    }
}
