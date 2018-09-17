using System;
using ConverterLibrary.Replacers.ImageReplacer;
using Microsoft.Extensions.DependencyInjection;
using WordpressWXR12;
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
            services.AddSingleton(new ImageReplacer());
            services.AddSingleton<IWordpressWXRParser>(new WordpressWXRParser());

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
