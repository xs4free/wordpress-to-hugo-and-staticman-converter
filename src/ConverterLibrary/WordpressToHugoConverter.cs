using System;
using System.IO;

namespace ConverterLibrary
{
    public class WordpressToHugoConverter
    {
        private ConverterOptions options;
        private ConverterOutput output;

        public ConverterOutput Convert(ConverterOptions options)
        {
            this.options = options;
            output = new ConverterOutput();

            Directory.CreateDirectory(options.OutputDirectory);
            Console.WriteLine($"Output will be written to: '{options.OutputDirectory}'.");

            ValidateOptions(options);

            Console.WriteLine($"Start processing '{options.InputFile}'...");
            Console.WriteLine("Done.");

            return output;
        }

        private void ValidateOptions(ConverterOptions options)
        {
            if (!File.Exists(options.InputFile))
            {
                throw new Exception($"Input file '{options.InputFile}' not found.");
            }
        }
    }
}
