using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace wordpress_to_hugo_and_staticman_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var serviceProvider = ServiceProviderFactory.Create();
                    var runner = serviceProvider.GetRequiredService<Runner>();

                    try
                    {
                        runner.Run(o);
                    }
                    finally
                    {
                        NLog.LogManager.Shutdown();
                    }
                });
        }
    }
}
