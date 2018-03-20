using System;
using log4net;

namespace podcastUpdater
{
    public class Episode
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string DisplayTitle => $"{Title} ({PublishDate.ToString("d")})";

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Summary { get; set; }
        public string FileType { get; set; }
        public bool IsExplicit { get; set; }
        public TimeSpan Duration { get; set; }
        public string Keywords { get; set; }
        public DateTime PublishDate { get; set; }
        public string EpisodeGuid { get; set; }
        public string AudioFormat { get; set; }

        public const string Category = "Podcasts"; // I don't see why this would change

        public Episode() { }

        public Episode(string title, string author, string url, string summary, string filetype,
            bool isExplicit, TimeSpan duration, string keywords, DateTime pubDate, string subtitle,
            string guid, string audioFormat = "audio/mpeg")
        {
            Title = title.Trim();
            Author = author.Trim();
            Url = url.Trim();
            EpisodeGuid = guid.Trim();
            Summary = summary.Trim();
            FileType = filetype.Trim();
            IsExplicit = isExplicit;
            Duration = duration;
            Keywords = keywords.Trim();
            PublishDate = pubDate;
            Subtitle = subtitle.Trim();
            AudioFormat = audioFormat.Trim();
        }
    }
}
