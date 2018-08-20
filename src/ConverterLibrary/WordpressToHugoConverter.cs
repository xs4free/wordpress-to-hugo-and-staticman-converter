using Microsoft.Extensions.Logging;
using System.IO;
using WordpressWXR12;

namespace ConverterLibrary
{
    public class WordpressToHugoConverter
    {
        private readonly ILogger<WordpressToHugoConverter> logger;
        private ConverterOptions options;

        public WordpressToHugoConverter(ILogger<WordpressToHugoConverter> logger)
        {
            this.logger = logger;
        }

        public void Convert(ConverterOptions options)
        {
            this.options = options;

            Directory.CreateDirectory(options.OutputDirectory);
            logger.LogInformation($"Output will be written to: '{options.OutputDirectory}'.");

            if (ValidateOptions(options))
            {
                logger.LogInformation($"Start processing '{options.InputFile}'...");
                var content = ReadWordpressExportFile(options.InputFile);
                logger.LogInformation(content.Channel.WxrVersion);

                logger.LogInformation("Done.");
            }
        }

        private RSS ReadWordpressExportFile(string inputFile)
        {
            var parser = new WordpressWXRParser();
            return parser.LoadFromFile(inputFile);
        }

        private bool ValidateOptions(ConverterOptions options)
        {
            bool result = true;

            if (!File.Exists(options.InputFile))
            {
                logger.LogError($"Input file '{options.InputFile}' not found.");
                result = false;
            }

            return result;
        }
    }
}
