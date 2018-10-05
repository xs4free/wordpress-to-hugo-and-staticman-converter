using System.Linq;
using System.Web;

namespace ConverterLibrary.Extensions
{
    public static class StringExtensions
    {
        public static string UrlToPath(this string url)
        {
            string decodedUrl = HttpUtility.UrlDecode(url);
            string path = decodedUrl?.Replace('/', '\\');

            return path;
        }

        public static string RemoveFirstBackslash(this string path)
        {
            string result = path;

            if (path.StartsWith("\\"))
            {
                result = path.Substring(1);
            }

            return result;
        }

        // https://stackoverflow.com/questions/372865/path-combine-for-urls
        public static string CombineUri(this string baseUri, params string[] uriParts)
        {
            string uri = string.Empty;
            if (uriParts != null && uriParts.Any())
            {
                char[] trims = { '\\', '/' };
                uri = (baseUri ?? string.Empty).TrimEnd(trims);
                foreach (var part in uriParts)
                {
                    uri = string.Format("{0}/{1}", uri.TrimEnd(trims), (part ?? string.Empty).TrimStart(trims));
                }
            }
            return uri;
        }

        public static string RemoveBaseUrl(this string url, string baseUrl)
        {
            return url?.Replace(baseUrl, string.Empty);
        }
    }
}
