using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;

namespace podcastUpdater
{
    public class Podcast : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Properties

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        private string _languageCode;
        public string LanguageCode
        {
            get { return _languageCode; }
            set
            {
                _languageCode = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(LanguageCode)));
            }
        }

        private string _copyright;
        public string Copyright
        {
            get { return _copyright; }
            set
            {
                _copyright = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Copyright)));
            }
        }

        private string _webmaster;
        public string Webmaster
        {
            get { return _webmaster; }
            set
            {
                _webmaster = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Webmaster)));
            }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Author)));
            }
        }

        private string _subtitle;
        public string Subtitle
        {
            get { return _subtitle; }
            set
            {
                _subtitle = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Subtitle)));
            }
        }

        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                _ownerName = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(OwnerName)));
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        private bool _isExplicit;
        public bool IsExplicit
        {
            get { return _isExplicit; }
            set
            {
                _isExplicit = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(IsExplicit)));
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(ImageUrl)));
            }
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Category)));
            }
        }

        #endregion

        public List<Episode> Episodes { get; private set; }
        private readonly XmlData _xmlData;
        public string Filepath { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, e);
        }
        
        public Podcast(string file)
        {
            Filepath = file;
            _xmlData = new XmlData(file);
            PopulatePodcast();
            PopulateEpisodes();
        }

        private void PopulatePodcast()
        {
            Title = _xmlData.GetSingleNodeInfo(@"//channel/title");
            Description = _xmlData.GetSingleNodeInfo(@"//channel/description");
            LanguageCode = _xmlData.GetSingleNodeInfo(@"//channel/language");
            Copyright = _xmlData.GetSingleNodeInfo(@"//channel/copyright");
            Webmaster = _xmlData.GetSingleNodeInfo(@"//channel/webmaster");
            Author = _xmlData.GetSingleNodeInfo(@"//channel/itunes:author");
            Subtitle = _xmlData.GetSingleNodeInfo(@"//channel/itunes:subtitle");
            OwnerName = _xmlData.GetSingleNodeInfo(@"//channel/itunes:owner/itunes:name");
            Email = _xmlData.GetSingleNodeInfo(@"//channel/itunes:owner/itunes:email");
            ImageUrl = _xmlData.GetSingleNodeInfo(@"//channel/itunes:image", "href");
            Category = _xmlData.GetSingleNodeInfo(@"//channel/itunes:category");

            bool isExplicit;
            if (bool.TryParse(_xmlData.GetSingleNodeInfo(@"//channel/itunes:explicit"), out isExplicit))
                IsExplicit = isExplicit;
        }

        private void PopulateEpisodes()
        {
            Episodes = new List<Episode>();
            var nodes = _xmlData.GetMultiNodeInfo(@"//channel/item");
            foreach (XmlNode node in nodes)
            {
                var title = node.SelectSingleNode(@"title")?.InnerText;
                var author = node.SelectSingleNode(@"itunes:author", _xmlData.NamespaceManager)?.InnerText;
                var subtitle = node.SelectSingleNode(@"itunes:subtitle", _xmlData.NamespaceManager)?.InnerText;
                var summary = node.SelectSingleNode(@"itunes:summary", _xmlData.NamespaceManager)?.InnerText;
                var keywords = node.SelectSingleNode(@"itunes:keywords", _xmlData.NamespaceManager)?.InnerText;
                var dateStr = node.SelectSingleNode(@"pubDate", _xmlData.NamespaceManager)?.InnerText;
                var isExplicitStr = node.SelectSingleNode(@"itunes:explicit", _xmlData.NamespaceManager)?.InnerText;
                var guidStr = node.SelectSingleNode(@"guid", _xmlData.NamespaceManager)?.InnerText;
                
                var enclosure = node.SelectSingleNode(@"enclosure", _xmlData.NamespaceManager);
                var type = "audio/mpeg"; // mp3 is default
                var url = String.Empty;
                if (enclosure?.Attributes != null)
                {
                    type = enclosure.Attributes["type"].Value;
                    url = enclosure.Attributes["url"].Value;
                }

                var lengthStr = node.SelectSingleNode(@"itunes:duration", _xmlData.NamespaceManager)?.InnerText;
                TimeSpan length;
                if (!TimeSpan.TryParse(lengthStr, out length)) length = new TimeSpan(0, 10, 0); // default is 10min
                DateTime date;
                if (!DateTime.TryParse(dateStr, out date)) date = DateTime.Now; // default is now
                bool isExplicitEp;
                if (!bool.TryParse(isExplicitStr, out isExplicitEp)) isExplicitEp = false; // default is false

                Episode episode = new Episode(title, author, url, summary, type, isExplicitEp, length, keywords, date,
                    subtitle, guidStr);
                Episodes.Add(episode);
            }
        }

        public void SavePodcastInfoToFile()
        {
            // should probably do this in the property fields
            _xmlData.ChangeNodeInnerText(@"//channel/title", Title);
            _xmlData.ChangeNodeInnerText(@"//channel/description", Description);
            _xmlData.ChangeNodeInnerText(@"//channel/language", LanguageCode);
            _xmlData.ChangeNodeInnerText(@"//channel/copyright", Copyright);
            _xmlData.ChangeNodeInnerText(@"//channel/webmaster", Webmaster);
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:author", Author);
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:subtitle", Subtitle);
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:owner/itunes:name", OwnerName);
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:owner/itunes:email", Email);
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:explicit", IsExplicit.ToString());
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:image", "href", ImageUrl);
            _xmlData.ChangeNodeInnerText(@"//channel/itunes:category", Category);
            
            _xmlData.Save();
        }

        public bool SaveEpisodeToFile(Episode episodeToSave)
        {
            try
            {
                _xmlData.SaveEpisodeToXmlDocument(episodeToSave);
                _xmlData.Save();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error while saving episode (\"{episodeToSave.Title}\") to file.", ex);
                return false;
            }
        }

        public bool SaveAllEpisodesToFile()
        {
            bool allGood = true;
            foreach (var episode in Episodes)
            {
                allGood = SaveEpisodeToFile(episode) && allGood;
            }
            return allGood;
        }
    }
}
