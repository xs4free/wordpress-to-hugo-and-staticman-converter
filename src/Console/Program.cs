using CommandLine;
using ConverterLibrary;

namespace wordpress_to_hugo_and_staticman_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var converter = new WordpressToHugoConverter(o.Input);
                    converter.Convert();
                });
        }
    }
}
