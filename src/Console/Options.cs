using CommandLine;

namespace wordpress_to_hugo_and_staticman_converter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Location of XML export from Wordpress to convert.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Location where hugo-files will be written.")]
        public string OutputDirectory { get; set; }
    }
}
