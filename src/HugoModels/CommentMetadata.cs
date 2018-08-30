using System;

namespace HugoModels
{
    public class CommentMetadata
    {
        public string Id { get; set; }
        public string ReplyTo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
