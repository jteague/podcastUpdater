using System;
using System.Text.RegularExpressions;
using System.Xml;
using log4net;

namespace podcastUpdater
{
    public class XmlData
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly XmlDocument _xmlDoc;
        private readonly string _file;
        private const string ItunesNamespaceUri = "http://www.itunes.com/dtds/podcast-1.0.dtd";
        private const string ItunesPrefix = "itunes";

        private XmlNamespaceManager _nsMan;
        public XmlNamespaceManager NamespaceManager
        {
            get { return _nsMan; }
            private set { _nsMan = value; }
        }
        
        public XmlData(string filename)
        {
            _file = filename;
            _xmlDoc = new XmlDocument();
            using (var reader = XmlReader.Create(_file))
            {
                _xmlDoc.Load(reader);
            }
            
            NamespaceManager = new XmlNamespaceManager(_xmlDoc.NameTable);
            NamespaceManager.AddNamespace("itunes", ItunesNamespaceUri);
        }

        public void Save()
        {
            _xmlDoc.Save(_file);
        }

        #region Node Access Methods

        public XmlNode GetSingleXmlNode(string nodeSearchString)
        {
            XmlNode node = _xmlDoc.SelectSingleNode(nodeSearchString, NamespaceManager);
            return node;
        }

        public string GetSingleNodeInfo(string nodeSearchString, string attribute = null)
        {
            XmlNode node = _xmlDoc.SelectSingleNode(nodeSearchString, NamespaceManager);

            if (node != null)
            {
                if (attribute == null)
                    return node.InnerText.Trim();

                if (node.Attributes?[attribute] != null)
                    return node.Attributes[attribute].Value.Trim();
            }

            return string.Empty;
        }

        public XmlNodeList GetMultiNodeInfo(string nodeSearchString, string attribute = null)
        {
            XmlNodeList nodes = _xmlDoc.SelectNodes(nodeSearchString, NamespaceManager);
            return nodes;
        }

        public XmlNode GetEpisodeXmlNodeByGuid(string guid, XmlNodeList nodes)
        {
            if (nodes == null) return null;

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.LocalName.Equals("guid", StringComparison.InvariantCultureIgnoreCase) &&
                        childNode.InnerText.Equals(guid, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Node Modify Methods

        public void ChangeNodeInnerText(string searchNodeString, string innerText)
        {
            XmlNode node = _xmlDoc.SelectSingleNode(searchNodeString, _nsMan);
            if(node != null) node.InnerText = innerText;
        }

        public void ChangeNodeInnerText(string searchNodeString, string attribute, string newText)
        {
            XmlNode node = _xmlDoc.SelectSingleNode(searchNodeString, _nsMan);
            if (node?.Attributes != null) node.Attributes[attribute].Value = newText;
        }

        public void SaveEpisodeToXmlDocument(Episode episodeToSave)
        {
            XmlNode episodeXmlNode = GetEpisodeXmlNodeByGuid(episodeToSave.EpisodeGuid, GetMultiNodeInfo(@"//channel/item"));
            if (episodeXmlNode == null)
            {
                AddEpisodeToXmlDocument(episodeToSave);
            }
            else
            {
                OverwriteSaveEpisodeToXmlDocument(episodeToSave, episodeXmlNode);
            }
        }

        private void AddEpisodeToXmlDocument(Episode episode)
        {
            XmlNode root = GetSingleXmlNode(@"//channel");
            XmlElement item = _xmlDoc.CreateElement("item");
            
            XmlElement title = _xmlDoc.CreateElement("title");
            if(episode.Title != null) title.InnerText = episode.Title.Trim();
            XmlElement author = _xmlDoc.CreateElement(ItunesPrefix, "author", ItunesNamespaceUri);
            if (episode.Author!= null) author.InnerText = episode.Author.Trim();
            XmlElement subtitle = _xmlDoc.CreateElement(ItunesPrefix, "subtitle", ItunesNamespaceUri);
            if (episode.Subtitle!= null) subtitle.InnerText = episode.Subtitle.Trim();
            XmlElement enclosure = _xmlDoc.CreateElement("enclosure");
            XmlAttribute url = _xmlDoc.CreateAttribute("url");
            if (episode.Url != null) url.Value = episode.Url.Trim();
            XmlAttribute format = _xmlDoc.CreateAttribute("type");
            if (episode.AudioFormat != null) format.Value = episode.AudioFormat.Trim();
            XmlElement summary = _xmlDoc.CreateElement(ItunesPrefix, "summary", ItunesNamespaceUri);
            if(episode.Summary != null) summary.InnerText = episode.Summary.Trim();
            XmlElement description = _xmlDoc.CreateElement("description");
            if (episode.Summary != null) description.InnerText = episode.Summary.Trim();
            XmlElement isExplicit = _xmlDoc.CreateElement(ItunesPrefix, "explicit", ItunesNamespaceUri);
            isExplicit.InnerText = episode.IsExplicit ? "Yes" : "No";
            XmlElement duration = _xmlDoc.CreateElement(ItunesPrefix, "duration", ItunesNamespaceUri);
            duration.InnerText = episode.Duration.ToString("g");
            XmlElement keywords = _xmlDoc.CreateElement(ItunesPrefix, "keywords", ItunesNamespaceUri);
            if (episode.Keywords != null) keywords.InnerText = episode.Keywords.Trim();
            XmlElement pubDate = _xmlDoc.CreateElement("pubDate");
            pubDate.InnerText = episode.PublishDate.ToString("d");
            XmlElement guid = _xmlDoc.CreateElement("guid");
            guid.InnerText = episode.EpisodeGuid.Trim();
            
            enclosure.Attributes.Append(url);
            enclosure.Attributes.Append(format);

            item.AppendChild(title);
            item.AppendChild(author);
            item.AppendChild(subtitle);
            item.AppendChild(enclosure);
            item.AppendChild(summary);
            item.AppendChild(description);
            item.AppendChild(isExplicit);
            item.AppendChild(duration);
            item.AppendChild(keywords);
            item.AppendChild(pubDate);
            item.AppendChild(guid);

            //root.AppendChild(item);
            var firstItem = GetSingleXmlNode(@"//channel/item");
            root.InsertBefore(item, firstItem);
        }

        private void AddPodcastToXmlDocument(Podcast podcast)
        {
            //<rss xmlns:itunes="http://www.itunes.com/dtds/podcast-1.0.dtd" version="2.0">

            //_xmlDoc = new XmlDocument();
            //XmlNode root = 
            
            XmlElement channel = _xmlDoc.CreateElement("channel");
            
            XmlElement title = _xmlDoc.CreateElement("title");
            title.InnerText = podcast.Title;
            XmlElement author = _xmlDoc.CreateElement(ItunesPrefix, "author", ItunesNamespaceUri);
            author.InnerText = podcast.Author;
            XmlElement subtitle = _xmlDoc.CreateElement(ItunesPrefix, "subtitle", ItunesNamespaceUri);
            subtitle.InnerText = podcast.Subtitle;

            XmlElement image = _xmlDoc.CreateElement(ItunesPrefix, "image", ItunesNamespaceUri);
            XmlAttribute imageUrl = _xmlDoc.CreateAttribute("href");
            imageUrl.Value = podcast.ImageUrl;
            image.Attributes.Append(imageUrl);

            XmlElement category = _xmlDoc.CreateElement(ItunesPrefix, "category", ItunesNamespaceUri);
            XmlAttribute catText = _xmlDoc.CreateAttribute("text");
            catText.Value = podcast.Category;
            category.Attributes.Append(catText);

            XmlElement description = _xmlDoc.CreateElement("description");
            description.InnerText = podcast.Description;
            XmlElement summary = _xmlDoc.CreateElement(ItunesPrefix, "summary", ItunesNamespaceUri);
            summary.InnerText = podcast.Description;
            XmlElement language = _xmlDoc.CreateElement("language");
            language.InnerText = podcast.LanguageCode;
            XmlElement copyright = _xmlDoc.CreateElement("copyright");
            copyright.InnerText = podcast.Copyright;
            XmlElement webmaster = _xmlDoc.CreateElement("webmaster");
            webmaster.InnerText = podcast.Webmaster;

            XmlElement owner = _xmlDoc.CreateElement(ItunesPrefix, "owner", ItunesNamespaceUri);
            XmlElement ownerName = _xmlDoc.CreateElement(ItunesPrefix, "name", ItunesNamespaceUri);
            ownerName.InnerText = podcast.OwnerName;
            XmlElement ownerEmail = _xmlDoc.CreateElement(ItunesPrefix, "email", ItunesNamespaceUri);
            ownerEmail.InnerText = podcast.Webmaster;
            owner.AppendChild(ownerName);
            owner.AppendChild(ownerEmail);

            XmlElement isExplicit = _xmlDoc.CreateElement(ItunesPrefix, "explicit", ItunesNamespaceUri);
            isExplicit.InnerText = podcast.IsExplicit ? "Yes" : "No";
            
            channel.AppendChild(title);
            channel.AppendChild(author);
            channel.AppendChild(subtitle);
            channel.AppendChild(image);
            channel.AppendChild(category);
            channel.AppendChild(description);
            channel.AppendChild(summary);
            channel.AppendChild(language);
            channel.AppendChild(copyright);
            channel.AppendChild(webmaster);
            channel.AppendChild(owner);
            channel.AppendChild(isExplicit);

            //root.AppendChild(channel);
        }

        private void OverwriteSaveEpisodeToXmlDocument(Episode episodeToSave, XmlNode episodeXmlNode)
        {
            var titleNode = episodeXmlNode.SelectSingleNode("title", NamespaceManager);
            if (titleNode != null) titleNode.InnerText = episodeToSave.Title.Trim();

            var authorNode = episodeXmlNode.SelectSingleNode("itunes:author", NamespaceManager);
            if (authorNode != null) authorNode.InnerText = episodeToSave.Author.Trim();

            var explicitNode = episodeXmlNode.SelectSingleNode("itunes:explicit", NamespaceManager);
            if (explicitNode != null) explicitNode.InnerText = episodeToSave.IsExplicit ? "Yes" : "No";

            var keywordsNode = episodeXmlNode.SelectSingleNode("itunes:keywords", NamespaceManager);
            if (keywordsNode != null) keywordsNode.InnerText = episodeToSave.Keywords.Trim();

            var lengthNode = episodeXmlNode.SelectSingleNode("itunes:duration", NamespaceManager);
            if (lengthNode != null) lengthNode.InnerText = episodeToSave.Duration.ToString();

            var publishNode = episodeXmlNode.SelectSingleNode("pubDate", NamespaceManager);
            if (publishNode != null) publishNode.InnerText = episodeToSave.PublishDate.ToString("MM/dd/yyyy");

            var subtitleNode = episodeXmlNode.SelectSingleNode("itunes:subtitle", NamespaceManager);
            if (subtitleNode != null) subtitleNode.InnerText = episodeToSave.Subtitle.Trim();

            var summaryNode = episodeXmlNode.SelectSingleNode("itunes:summary", NamespaceManager);
            if (summaryNode != null) summaryNode.InnerText = episodeToSave.Summary.Trim();

            var descriptionNode = episodeXmlNode.SelectSingleNode("description", NamespaceManager);
            if (descriptionNode != null) descriptionNode.InnerText = episodeToSave.Summary.Trim();

            var enclosureNode = episodeXmlNode.SelectSingleNode("enclosure", NamespaceManager);
            if (enclosureNode?.Attributes != null) enclosureNode.Attributes["url"].InnerText = episodeToSave.Url.Trim();

            var guidNode = episodeXmlNode.SelectSingleNode("guid", NamespaceManager);
            if (guidNode != null)
            {
                guidNode.InnerText = string.IsNullOrEmpty(episodeToSave.EpisodeGuid)
                    ? episodeToSave.Url.Trim()
                    : episodeToSave.EpisodeGuid.Trim();
            }
        }
        
        #endregion
    }
}
