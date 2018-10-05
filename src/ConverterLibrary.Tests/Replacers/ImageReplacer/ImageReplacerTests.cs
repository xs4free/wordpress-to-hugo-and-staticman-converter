using System.Linq;
using HugoModels;
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
            var post = new Post { Content = "[![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)]" };
            var sut = new CL.ImageReplacer();

            sut.Replace(post, SiteUrlHttp, ImageBaseUrl);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg)]";
            Assert.Equal(expectedYaml, post.Content);
        }

        [Fact]
        public void Replace_should_handle_markdown_image_with_title()
        {
            var post = new Post { Content = "[![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg \"test\")]" };
            var sut = new CL.ImageReplacer();

            sut.Replace(post, SiteUrlHttp, ImageBaseUrl);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg \"test\")]";
            Assert.Equal(expectedYaml, post.Content);
        }

        [Fact]
        public void Replace_should_handle_markdown_image_with_alt_text()
        {
            var post = new Post { Content = "[![test](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)]" };
            var sut = new CL.ImageReplacer();

            sut.Replace(post, SiteUrlHttp, ImageBaseUrl);

            string expectedYaml = $"[![test]({ImageBaseUrl}/image.jpg)]";
            Assert.Equal(expectedYaml, post.Content);
        }

        [Fact]
        public void Replace_should_handle_http_urls_to_images()
        {
            var post = new Post
            {
                Content =
                    "[![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg \"test\")](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)"
            };
            var sut = new CL.ImageReplacer();

            sut.Replace(post, SiteUrlHttp, ImageBaseUrl);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg \"test\")]({ImageBaseUrl}/image.jpg)";
            Assert.Equal(expectedYaml, post.Content);
        }

        [Fact]
        public void Replace_should_handle_https_urls_to_images()
        {
            var post = new Post
            {
                Content =
                    "[![](https://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg \"test\")](https://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)"
            };
            var sut = new CL.ImageReplacer();

            sut.Replace(post, SiteUrlHttp, ImageBaseUrl);

            string expectedYaml = $"[![]({ImageBaseUrl}/image.jpg \"test\")]({ImageBaseUrl}/image.jpg)";
            Assert.Equal(expectedYaml, post.Content);
        }

        [Fact]
        public void Replace_should_change_resources()
        {
            var post = new Post
            {
                Metadata = new PostMetadata
                {
                    Resources = new []
                    {
                        new Resource
                        {
                            Src = "/img/test1/image.jpg"
                        }
                    }
                        
                }
            };
            var sut = new CL.ImageReplacer();

            sut.Replace(post, SiteUrlHttp, ImageBaseUrl);

            string expectedResourceSrc = $"{ImageBaseUrl}/image.jpg";
            Assert.Equal(expectedResourceSrc, post.Metadata.Resources.First().Src);
        }
    }
}
