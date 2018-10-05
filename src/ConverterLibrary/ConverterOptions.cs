namespace ConverterLibrary
{
    public class ConverterOptions
    {
        public string InputFile { get; set; }
        public string OutputDirectory { get; set; }
        public string[] UploadDirectories { get; set; }
        public bool PageResources { get; set; }
    }
}
