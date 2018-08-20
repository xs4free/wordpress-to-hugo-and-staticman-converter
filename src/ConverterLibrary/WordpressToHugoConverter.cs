using HugoModels;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text;
using WordpressWXR12;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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

                WriteConfig(content, options);

                var posts = content.Channel.Items.Where(item => item.PostType == "post" || item.PostType == "page").ToList();
                logger.LogInformation($"Found {posts.Count} posts/pages.");

                logger.LogInformation("Done.");
            }
        }

        private void WriteConfig(RSS content, ConverterOptions options)
        {
            var config = new Config();
            config.Name = content.Channel.Title;
            config.Description = content.Channel.Description;
            config.Url = content.Channel.Link;

            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            var yaml = serializer.Serialize(config);

            string configFileName = Path.Combine(options.OutputDirectory, "config.yaml");
            File.WriteAllText(configFileName, yaml, Encoding.UTF8);

            logger.LogInformation($"Written '{configFileName}'.");
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
