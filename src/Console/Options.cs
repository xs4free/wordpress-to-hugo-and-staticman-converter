using System.Collections.Generic;
using CommandLine;

namespace wordpress_to_hugo_and_staticman_converter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Location of XML export from WordPress to convert.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Location where hugo-files will be written.")]
        public string OutputDirectory { get; set; }

        [Option('c', "content", Required = false, HelpText = "Location(s) of wp-contents folder(s) where images from WordPress can be found.", Separator = ',')]
        public IEnumerable<string> UploadDirectories { get; set; }

        [Option('p', "pageresources", HelpText = "Use page resources as location to store images (instead of static 'uploads' folder).")]
        public bool PageResources { get; set; }

        [Option('s', "shortcodeimage", Required = false, HelpText = "Hugo Shortcode used for rendering images in front-matter. If not specified a Markdown image tag will be used.")]
        public string ImageShortCode { get; set; }
    }
}
