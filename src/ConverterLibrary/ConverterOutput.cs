using System.Collections.Generic;

namespace ConverterLibrary
{
    public class ConverterOutput
    {
        public bool Success { get; set; }

        public ConverterOptions Options { get; set; }

        public IEnumerable<string> Warnings { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
