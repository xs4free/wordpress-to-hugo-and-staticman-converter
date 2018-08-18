using AutoMapper;
using ConverterLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace wordpress_to_hugo_and_staticman_converter
{
    static class ServiceProviderFactory
    {
        public static IServiceProvider Create()
        {
            var services = new ServiceCollection();

            services.AddTransient<Runner>();
            services.AddTransient<WordpressToHugoConverter>();
            services.AddScoped<IMapper>(CreateMapper);

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //configure NLog
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");

            return serviceProvider;
        }

        private static IMapper CreateMapper(IServiceProvider arg)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Options, ConverterOptions>()
                    .ForMember(dest => dest.InputFile, opt => opt.MapFrom(src => Path.GetFullPath(src.InputFile)))
                    .ForMember(dest => dest.OutputDirectory, opt => opt.MapFrom(src => Path.GetFullPath(src.OutputDirectory)))
                );
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
