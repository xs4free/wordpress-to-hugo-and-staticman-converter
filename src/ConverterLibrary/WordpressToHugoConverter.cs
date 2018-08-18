using System;

namespace ConverterLibrary
{
    public class WordpressToHugoConverter
    {
        private readonly string inputFile;

        public WordpressToHugoConverter(string inputFile)
        {
            this.inputFile = inputFile;
        }

        public void Convert()
        {
            Console.WriteLine($"Start processing '{inputFile}'...");
            Console.WriteLine("Done.");
        }
    }
}
