using System.Collections.Generic;

namespace Weeb.net.Data
{
    public class RandomData
    {
        public string Id { get; set; }
        public string BaseType { get; set; }
        public string FileType { get; set; }
        public string MimeType { get; set; }
        public string Account { get; set; }
        public bool Hidden { get; set; }
        public bool Nsfw { get; set; }
        public List<Tags> Tags { get; set; }
        public string Url { get; set; }
    }

    public class Tags
    {
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public string User { get; set; }
    }
}