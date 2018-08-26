using System.IO;
using AutoMapper;
using ConverterLibrary;

namespace wordpress_to_hugo_and_staticman_converter
{
    public class ConverterAutoMapperProfile : Profile
    {
        public ConverterAutoMapperProfile()
        {
            CreateMap<Options, ConverterOptions>()
                .ForMember(dest => dest.InputFile, opt => opt.MapFrom(src => Path.GetFullPath(src.InputFile)))
                .ForMember(dest => dest.OutputDirectory,
                    opt => opt.MapFrom(src => Path.GetFullPath(src.OutputDirectory)));
        }
    }
}
