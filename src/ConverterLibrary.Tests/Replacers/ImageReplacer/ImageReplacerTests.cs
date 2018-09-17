using Xunit;
using CL = ConverterLibrary.Replacers.ImageReplacer;

namespace ConverterLibrary.Tests.Replacers.ImageReplacer
{
    public class ImageReplacerTests
    {
        private const string SiteUrlHttp = "http://www.site.com";
        private const string ImageBaseUrl = "/static/upload/2010/08";

        [Fact]
        public void Replace_should_handle_simple_markdown_image()
        {
            string yaml = "[![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)]";

            var sut = new CL.ImageReplacer();
            string resultYaml = sut.Replace(yaml, SiteUrlHttp, ImageBaseUrl, out CL.ImageReplacerResult[] results);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg)]";

            Assert.Equal(expectedYaml, resultYaml);
        }

        [Fact]
        public void Replace_should_handle_markdown_image_with_title()
        {
            string yaml = "[![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg \"test\")]";

            var sut = new CL.ImageReplacer();
            string resultYaml = sut.Replace(yaml, SiteUrlHttp, ImageBaseUrl, out CL.ImageReplacerResult[] results);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg \"test\")]";

            Assert.Equal(expectedYaml, resultYaml);
        }

        [Fact]
        public void Replace_should_handle_markdown_image_with_alt_text()
        {
            string yaml = "[![test](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)]";

            var sut = new CL.ImageReplacer();
            string resultYaml = sut.Replace(yaml, SiteUrlHttp, ImageBaseUrl, out CL.ImageReplacerResult[] results);

            string expectedYaml = $"[![test]({ImageBaseUrl}/image.jpg)]";

            Assert.Equal(expectedYaml, resultYaml);
        }

        [Fact]
        public void Replace_should_handle_http_urls_to_images()
        {
            string yaml =
                "[![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg \"test\")](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)";

            var sut = new CL.ImageReplacer();
            string resultYaml = sut.Replace(yaml, SiteUrlHttp, ImageBaseUrl, out CL.ImageReplacerResult[] results);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg \"test\")]({ImageBaseUrl}/image.jpg)";

            Assert.Equal(expectedYaml, resultYaml);
        }

        [Fact]
        public void Replace_should_handle_https_urls_to_images()
        {
            string yaml =
                "[![](https://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg \"test\")](https://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)";

            var sut = new CL.ImageReplacer();
            string resultYaml = sut.Replace(yaml, SiteUrlHttp, ImageBaseUrl, out CL.ImageReplacerResult[] results);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg \"test\")]({ImageBaseUrl}/image.jpg)";

            Assert.Equal(expectedYaml, resultYaml);
        }
    }
}
