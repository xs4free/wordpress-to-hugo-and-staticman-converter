using AutoMapper;
using CommandLine;
using ConverterLibrary;
using System.IO;

namespace wordpress_to_hugo_and_staticman_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    IMapper mapper = CreateMapper();
                    var converterOptions = mapper.Map<ConverterOptions>(o);
                    var converter = new WordpressToHugoConverter(converterOptions);
                    converter.Convert();
                });
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => 
                cfg.CreateMap<Options, ConverterOptions>()
                    .ForMember(dest => dest.InputFile, opt => opt.MapFrom(src => Path.GetFullPath(src.InputFile)))
                    .ForMember(dest => dest.OutputDirectory, opt => opt.MapFrom(src => Path.GetFullPath(src.OutputDirectory)))
                );
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
