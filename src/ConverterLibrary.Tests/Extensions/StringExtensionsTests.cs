using System.Linq;
using ConverterLibrary.Extensions;
using Xunit;

namespace ConverterLibrary.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void CombineUri_should_not_add_slash_at_front_with_no_baseUri()
        {
            string baseUri = null;
            string[] uriParts = {"image.jpg"};

            string result = baseUri.CombineUri(uriParts);

            Assert.Equal(uriParts.First(), result);
        }

        [Fact]
        public void CombineUri_should_not_add_slash_at_front_with_baseUri()
        {
            string baseUri = "http://example.com";
            string[] uriParts = { "image.jpg" };
            string expected = $"{baseUri}/{uriParts.First()}";

            string result = baseUri.CombineUri(uriParts);

            Assert.Equal(expected, result);
        }
    }
}
