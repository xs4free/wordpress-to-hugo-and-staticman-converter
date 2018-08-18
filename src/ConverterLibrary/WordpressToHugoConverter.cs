using System;

namespace ConverterLibrary
{
    public class WordpressToHugoConverter
    {
        private readonly ConverterOptions options;

        public WordpressToHugoConverter(ConverterOptions options)
        {
            this.options = options;
        }

        public void Convert()
        {
            Console.WriteLine($"Output will be written to: '{options.OutputDirectory}'...");
            Console.WriteLine($"Start processing '{options.InputFile}'...");
            Console.WriteLine("Done.");
        }
    }
}
