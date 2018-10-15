using System.Collections.Generic;

namespace HugoModels
{
    public class PostMetadata
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Url { get; set; }
        public string CommentFolder { get; set; }
        public string Excerpt { get; set; }
        public bool Draft { get; set; }
        public bool Private { get; set; }
        public string[] Categories { get; set; }
        public string[] Tags { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}