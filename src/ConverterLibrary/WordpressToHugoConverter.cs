using Microsoft.Extensions.Logging;
using System.IO;

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
                logger.LogInformation("Done.");
            }
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
