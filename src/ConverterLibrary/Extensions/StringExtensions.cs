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

        public static string PathToUrl(this string path)
        {
            string encodedPath = HttpUtility.UrlEncode(path);
            string url = encodedPath?.Replace('\\', '/');

            return url;
        }

        public static string CleanUrl(this string url)
        {
            return url.Replace(" ", "-").Replace("%20", "-");
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
            string uri = baseUri;

            if (uriParts != null && uriParts.Any())
            {
                char[] trims = { '\\', '/' };
                uri = (baseUri ?? string.Empty).TrimEnd(trims);
                foreach (var part in uriParts)
                {
                    if (uri.Length == 0)
                    {
                        uri = (part ?? string.Empty).TrimStart(trims);
                    }
                    else
                    {
                        uri = string.Format("{0}/{1}", uri.TrimEnd(trims), (part ?? string.Empty).TrimStart(trims));
                    }
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
