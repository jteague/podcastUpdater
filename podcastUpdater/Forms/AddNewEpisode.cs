using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using podcastUpdater.Properties;

namespace podcastUpdater
{
    public partial class AddNewEpisode : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Episode _episode;
        private Podcast _podcast;
        private FtpHelper _ftpHelper;

        public AddNewEpisode(Podcast podcast, FtpHelper ftpHelper)
        {
            InitializeComponent();
            _podcast = podcast;
            _episode = new Episode();
            _ftpHelper = ftpHelper;
        }

        private void btn_AddEpisode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEpisodeTitle.Text)) _episode.Title = txtEpisodeTitle.Text;
            if (!string.IsNullOrEmpty(txtEpisodeAuthor.Text)) _episode.Author = txtEpisodeAuthor.Text;
            if (!string.IsNullOrEmpty(txtEpisodeKeywords.Text)) _episode.Keywords = txtEpisodeKeywords.Text;
            if (!string.IsNullOrEmpty(datePickerEpisode.Text)) _episode.PublishDate = datePickerEpisode.Value;
            if (!string.IsNullOrEmpty(txtEpisodeSubtitle.Text)) _episode.Subtitle = txtEpisodeSubtitle.Text;
            if (!string.IsNullOrEmpty(txtEpisodeSummary.Text)) _episode.Summary = txtEpisodeSummary.Text;
            if (!string.IsNullOrEmpty(txtEpisodeUrl.Text))
            {
                _episode.Url = txtEpisodeUrl.Text;
                _episode.EpisodeGuid = txtEpisodeUrl.Text;
            }

            _episode.IsExplicit = cb_isExplicit.Checked;
            _episode.AudioFormat = "audio/mpeg";

            TimeSpan timeSpan;
            if (TimeSpan.TryParse(txtEpisodeDuration.Text, out timeSpan))
                _episode.Duration = timeSpan;

            _podcast?.Episodes?.Add(_episode);
            // close the window
            Log.Debug($"Added episode (\"{_episode.DisplayTitle}\") to podcast (\"{_podcast?.Title}\").");
            Close();
        }

        private async void btnSelectEpisodeAudioFile_Click(object sender, EventArgs e)
        {
            try
            {
                await SelectEpisodeAudioFileCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                Log.Error($"Error while selecting audio file for podcast episode.", ex);
            }
        }

        private async Task SelectEpisodeAudioFileCommand()
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*|Audio Files (.wav)|*.wav|mp3 (.mp3)|*.mp3"
            };
            if (!string.IsNullOrEmpty(Settings.Default.previousAudioLocation) &&
                    Directory.Exists(Settings.Default.previousAudioLocation))
                openFileDlg.InitialDirectory = Settings.Default.previousAudioLocation;

            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDlg.FileName);
                Settings.Default.previousAudioLocation = Directory.GetParent(openFileDlg.FileName).ToString();
                Settings.Default.Save();
                if (!fileInfo.Extension.Equals(".mp3", StringComparison.InvariantCultureIgnoreCase))
                {
                    var result = MessageBox.Show("Convert to MP3?", "Convert Dialog", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        // convert the file to an MP3
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            CreatePrompt = false,
                            OverwritePrompt = false,
                            CheckFileExists = false,
                            DefaultExt =  "mp3",
                            FileName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}.mp3"
                        };

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // TODO - add a progress bar for the conversion
                            await AudioHelper.ConvertWavToMp3(fileInfo.FullName, saveFileDialog.FileName);
                            fileInfo = new FileInfo(saveFileDialog.FileName);
                        }
                    }
                    else if (result != DialogResult.No)
                    {
                        // user cancelled
                        return;
                    }
                }

                string movieTitle = GetMovieTitleFromFileName(Path.GetFileNameWithoutExtension(fileInfo.Name));
                string imdbUrl = GetImdbUrlForMovieTitle(movieTitle);
                // update the text boxes
                txt_EpisodeFilePath.Text = fileInfo.FullName;
                txtEpisodeTitle.Text = Path.GetFileNameWithoutExtension(movieTitle);
                txtEpisodeUrl.Text = $"{Settings.Default.httpBaseAddress}/{fileInfo.Name}";
                txtEpisodeDuration.Text = AudioHelper.GetAudioFileDuration(fileInfo.FullName).ToString("g");
                txtEpisodeGuid.Text = _episode.EpisodeGuid;
                txtEpisodeAuthor.Text = _podcast.Author;
                txtEpisodeKeywords.Text = Settings.Default.defaults_episodeKeywords;
                txtEpisodeSubtitle.Text = Settings.Default.defaults_episodeSubtitle;
                txtEpisodeSummary.Text = $"This time Katie & Jeremiah review <a href=\"{imdbUrl}\">\"{movieTitle}\"</a>.";
                Log.Debug($"Audio file has been selected for episode");
            }
        }

        private string GetImdbUrlForMovieTitle(string movie)
        {
            if (string.IsNullOrEmpty(movie)) return string.Empty;
            
            string urlMovieTitle = string.Empty;
            foreach (var word in movie.Split(' '))
            {
                urlMovieTitle += $"{word}%20";
            }
            urlMovieTitle = urlMovieTitle.Substring(0, urlMovieTitle.Length - 3);

            return $"http://www.imdb.com/search/title?title={urlMovieTitle}&title_type=feature";
        }

        private string GetMovieTitleFromFileName(string filename)
        {
            // remove the filename prefix
            string movieTitle = filename.Replace("otdh_", "").Trim();

            // work through the camelCase
            StringBuilder movieTitleBuilder = new StringBuilder();
            for (int index = 0; index < movieTitle.Count(); index++)
            {
                char c = movieTitle[index];

                if (index == 0)
                {
                    movieTitleBuilder.Append(char.ToUpper(c).ToString()); // make the first letter uppercase
                    continue;
                }
                
                if (char.IsUpper(c))
                {
                    // new word
                    movieTitleBuilder.Append(' '.ToString());
                }

                movieTitleBuilder.Append(c.ToString());
            }

            return movieTitleBuilder.ToString();
        }

        private async void btnEpisodeUrlTest_Click(object sender, EventArgs e)
        {
            string url = txtEpisodeUrl.Text;
            bool fileExists = await _ftpHelper.RemoteFileExists(url);
            if (fileExists)
            {
                MessageBox.Show($"All good! File exists (\'{url}\')");
                Log.Debug($"Remote file (\"{url}\") exists");
            }
            else
            {
                MessageBox.Show($"File does not exist (\'{url}\')");
                Log.Debug($"Remote file (\"{url}\") does not exist");
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            string filePath = txt_EpisodeFilePath.Text;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                var message = $"Cannot upload: The file \"{filePath}\" does not exist.";
                MessageBox.Show(message);
                Log.Warn(message);
                return;
            }

            progBar_uploadFile.Value = 0;
            _ftpHelper.UploadFile(filePath, progBar_uploadFile);
        }
    }
}
