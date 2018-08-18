using CommandLine;

namespace wordpress_to_hugo_and_staticman_converter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Locatio of XML export from Wordpress to convert.")]
        public string Input { get; set; }
    }
}
