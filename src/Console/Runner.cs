using AutoMapper;
using ConverterLibrary;

namespace wordpress_to_hugo_and_staticman_converter
{
    public class Runner
    {
        private readonly WordpressToHugoConverter _converter;
        private readonly IMapper _mapper;

        public Runner(WordpressToHugoConverter converter, IMapper mapper)
        {
            _converter = converter;
            _mapper = mapper;
        }

        public void Run(Options o)
        {
            var converterOptions = _mapper.Map<ConverterOptions>(o);
            _converter.Convert(converterOptions);
        }
    }
}
