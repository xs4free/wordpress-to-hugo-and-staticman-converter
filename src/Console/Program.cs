using CommandLine;
using System;

namespace wordpress_to_hugo_and_staticman_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    Console.WriteLine($"Start processing '{o.Input}'...");
                    Console.WriteLine("Done.");
                });
        }
    }
}
