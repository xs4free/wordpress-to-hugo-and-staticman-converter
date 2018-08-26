using System.Collections.Generic;
using AutoMapper;
using WordpressWXR12;

namespace ConverterLibrary
{
    internal static class ResolutionContextExtensions
    {
        public static string GetSiteUrl(this ResolutionContext context)
        {
            return context.Items[ConverterLibraryAutoMapperProfile.ItemNameSiteUrl] as string;
        }

        public static IDictionary<int, Item> GetAttachments(this ResolutionContext context)
        {
            return context.Items[ConverterLibraryAutoMapperProfile.ItemNameAttachments] as IDictionary<int, Item>;
        }
    }
}
