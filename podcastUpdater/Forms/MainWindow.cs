using System;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using log4net;
using podcastUpdater.Properties;

namespace podcastUpdater
{
    public partial class MainWindow : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _programDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PodcastUpdater");

        private Podcast _podcast;
        
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(_programDataFolder)) Directory.CreateDirectory(_programDataFolder);
            HandleSettings();
        }

        private void HandleSettings()
        {
            txtFtpDirectory.Text = Settings.Default.ftpDirectory;
            txtHttpBaseAddress.Text = Settings.Default.httpBaseAddress;
            txtRemoteRssFileLocation.Text = Settings.Default.remoteRssFileLocation;
            txtFtpHost.Text = Settings.Default.ftpHost ?? "ftp://";
            txtFtpPort.Text = Settings.Default.ftpPort.ToString();
            txtFtpUsername.Text = Settings.Default.ftpUsername;
            cb_ftpSavePassword.Checked = Settings.Default.ftpSavePassword;

            if (cb_ftpSavePassword.Checked)
            {
                txtFtpPassword.Text = Settings.Default.ftpPassword;
                txtFtpPassword.DataBindings.Add(nameof(txtFtpPassword.Text), Settings.Default,
                    nameof(Settings.Default.ftpPassword));
            }

            cb_ftpSavePassword.DataBindings.Add(nameof(cb_ftpSavePassword.Checked), Settings.Default,
                nameof(Settings.Default.ftpSavePassword));
            txtFtpHost.DataBindings.Add(nameof(txtFtpHost.Text), Settings.Default, nameof(Settings.Default.ftpHost));
            txtRemoteRssFileLocation.DataBindings.Add(nameof(txtRemoteRssFileLocation.Text), Settings.Default,
                nameof(Settings.Default.remoteRssFileLocation));
            txtFtpDirectory.DataBindings.Add(nameof(txtFtpDirectory.Text), Settings.Default,
                nameof(Settings.Default.ftpDirectory));
            txtHttpBaseAddress.DataBindings.Add(nameof(txtHttpBaseAddress.Text), Settings.Default,
                nameof(Settings.Default.httpBaseAddress));
            txtFtpPort.DataBindings.Add(nameof(txtFtpPort.Text), Settings.Default, nameof(Settings.Default.ftpPort));
            txtFtpUsername.DataBindings.Add(nameof(txtFtpUsername.Text), Settings.Default, nameof(Settings.Default.ftpUsername));
        }

        private void btnOpenExistingPodcast_onClick(object sender, EventArgs e)
        {
            try
            {
                UnloadPodcast();
                OpenFileDialog openFileDlg = new OpenFileDialog
                {
                    Filter = "XML Files (.xml)|*.xml|RSS Files (.rss)|*.rss|All Files (*.*)|*.*"
                };
                if (!string.IsNullOrEmpty(Settings.Default.previousRssLocation) &&
                    Directory.Exists(Settings.Default.previousRssLocation))
                    openFileDlg.InitialDirectory = Settings.Default.previousRssLocation;

                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    string localFile = openFileDlg.FileName;
                    // load the podcast info
                    LoadPodcastFromFile(localFile);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error opening podcast file: {ex.Message}");
                Log.Error("Error opening podcast file", ex);
            }
        }

        private void LoadPodcastFromFile(string localFile)
        {
            _podcast = new Podcast(localFile);
            Settings.Default.previousRssLocation = Directory.GetParent(localFile).ToString();
            Settings.Default.Save();
            PopulatePodcastTab(_podcast);
            SetCurrentPodcastBindings();

            // load the episode info
            LoadEpisodeComboBox();
            SetCurrentEpisodeBindings();

            // switch to podcast tab
            tabControl.SelectedTab = tabPagePodcast;
        }

        private void LoadEpisodeComboBox(bool loadLastEntry = false)
        {
            cbEpisodeSelect.DataBindings.Clear();

            BindingList<Episode> episodeBindingList = new BindingList<Episode>();
            if (_podcast != null)
            {
                foreach (var episide in _podcast.Episodes)
                {
                    episodeBindingList.Add(episide);
                }

                cbEpisodeSelect.DataSource = episodeBindingList;
                cbEpisodeSelect.DisplayMember = nameof(Episode.DisplayTitle);

                if(loadLastEntry)
                    cbEpisodeSelect.SelectedIndex = cbEpisodeSelect.Items.Count - 1;
            }
        }

        private void SetCurrentEpisodeBindings()
        {
            Episode currentEp = cbEpisodeSelect.SelectedItem as Episode;
            if (currentEp == null) return;

            ClearEpisodeBindings();
            
            txtEpisodeAuthor.DataBindings.Add("Text", currentEp, nameof(currentEp.Author));
            txtEpisodeDuration.DataBindings.Add("Text", currentEp, nameof(currentEp.Duration));
            txtEpisodeKeywords.DataBindings.Add("Text", currentEp, nameof(currentEp.Keywords));
            txtEpisodeSubtitle.DataBindings.Add("Text", currentEp, nameof(currentEp.Subtitle));
            txtEpisodeSummary.DataBindings.Add("Text", currentEp, nameof(currentEp.Summary));
            txtEpisodeTitle.DataBindings.Add("Text", currentEp, nameof(currentEp.Title));
            txtEpisodeUrl.DataBindings.Add("Text", currentEp, nameof(currentEp.Url));
            txtEpisodeGuid.DataBindings.Add("Text", currentEp, nameof(currentEp.EpisodeGuid));
        }

        private void SetCurrentPodcastBindings()
        {
            ClearPodcastBindings();
            txtPodcastName.DataBindings.Add("Text", _podcast, nameof(_podcast.Title));
            txtCopyright.DataBindings.Add("Text", _podcast, nameof(_podcast.Copyright));
            txtImageUrl.DataBindings.Add("Text", _podcast, nameof(_podcast.ImageUrl));
            txtPodcastAuthor.DataBindings.Add("Text", _podcast, nameof(_podcast.Author));
            txtPodcastCategory.DataBindings.Add("Text", _podcast, nameof(_podcast.Category));
            txtPodcastDescription.DataBindings.Add("Text", _podcast, nameof(_podcast.Description));
            txtPodcastEmail.DataBindings.Add("Text", _podcast, nameof(_podcast.Email));
            txtPodcastSubtitle.DataBindings.Add("Text", _podcast, nameof(_podcast.Subtitle));
            txtPodcastWebmaster.DataBindings.Add("Text", _podcast, nameof(_podcast.Webmaster));
            rbPodcastExplicitYes.DataBindings.Add("Checked", _podcast, nameof(_podcast.IsExplicit));
        }

        private void PopulatePodcastTab(Podcast podcast)
        {
            txtPodcastName.Text = podcast.Title;
            txtCopyright.Text = podcast.Copyright;
            txtImageUrl.Text = podcast.ImageUrl;
            txtPodcastAuthor.Text = podcast.Author;
            txtPodcastCategory.Text = podcast.Category;
            txtPodcastDescription.Text = podcast.Description;
            txtPodcastEmail.Text = podcast.Email;
            txtPodcastSubtitle.Text = podcast.Subtitle;
            txtPodcastWebmaster.Text = podcast.Webmaster;
            rbPodcastExplicitYes.Checked = podcast.IsExplicit;
            rbPodcastExplicitNo.Checked = !podcast.IsExplicit;
        }

        public void UnloadPodcast()
        {
            cbEpisodeSelect.DataBindings.Clear();
            ClearEpisodeBindings();
            ClearPodcastBindings();
            _podcast = null;
        }

        private void ClearPodcastBindings()
        {
            txtPodcastName.DataBindings.Clear();
            txtCopyright.DataBindings.Clear();
            txtImageUrl.DataBindings.Clear();
            txtPodcastAuthor.DataBindings.Clear();
            txtPodcastCategory.DataBindings.Clear();
            txtPodcastDescription.DataBindings.Clear();
            txtPodcastEmail.DataBindings.Clear();
            txtPodcastSubtitle.DataBindings.Clear();
            txtPodcastWebmaster.DataBindings.Clear();
            rbPodcastExplicitYes.DataBindings.Clear();
            rbPodcastExplicitNo.DataBindings.Clear();
        }

        private void ClearEpisodeBindings()
        {
            txtEpisodeAuthor.DataBindings.Clear();
            txtEpisodeDuration.DataBindings.Clear();
            txtEpisodeKeywords.DataBindings.Clear();
            txtEpisodeSubtitle.DataBindings.Clear();
            txtEpisodeSummary.DataBindings.Clear();
            txtEpisodeTitle.DataBindings.Clear();
            txtEpisodeUrl.DataBindings.Clear();
            txtEpisodeGuid.DataBindings.Clear();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ask if they want to save, then close
            this.Close();
        }

        private void cbEpisode_OnSelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Episode episode = cbEpisodeSelect.SelectedItem as Episode;
                PopulateEpisodeInfo(episode);
                SetCurrentEpisodeBindings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing episode selection: {ex.Message}");
                Log.Error("Error changing episode selection", ex);
            }
        }

        private void PopulateEpisodeInfo(Episode episode)
        {
            if (episode == null) throw new NullReferenceException("Podcast episode is null");
            
            txtEpisodeTitle.Text = episode.Title;
            txtEpisodeAuthor.Text = episode.Author;
            txtEpisodeDuration.Text = episode.Duration.ToString();
            txtEpisodeKeywords.Text = episode.Keywords;
            txtEpisodeSubtitle.Text = episode.Subtitle;
            txtEpisodeSummary.Text = episode.Summary;
            txtEpisodeUrl.Text = episode.Url;
            datePickerEpisode.Value = episode.PublishDate;
            txtEpisodeGuid.Text = episode.EpisodeGuid;
        }

        private void btnPodcastSave_Click(object sender, EventArgs e)
        {
            if (_podcast == null)
            {
                try
                {
                    // save new podcast
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.AddExtension = true;
                    saveDialog.Filter = "RSS Files (*.rss)|*.rss|All Files (*.*)|*.*";
                    var result = saveDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        File.Create(saveDialog.FileName);
                        Thread.Sleep(200);
                        // save the info to a new file
                        _podcast = new Podcast(saveDialog.FileName)
                        {
                            Title = txtPodcastName.Text,
                            Copyright = txtCopyright.Text,
                            ImageUrl = txtImageUrl.Text,
                            Author = txtPodcastAuthor.Text,
                            Category = txtPodcastCategory.Text,
                            Description = txtPodcastDescription.Text,
                            Email = txtPodcastEmail.Text,
                            Subtitle = txtPodcastSubtitle.Text,
                            Webmaster = txtPodcastWebmaster.Text,
                            IsExplicit = rbPodcastExplicitYes.Checked
                        };
                        _podcast.SavePodcastInfoToFile();
                        LoadPodcastFromFile(saveDialog.FileName); // creates new bindings
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error saving Podcast to file: " + ex.Message);
                    Log.Error("Error saving Podcast to file.", ex);
                }
            }
            else
            {
                try
                {
                    // Update the Podcast object with the current text fields (done with data binding)
                    _podcast.SavePodcastInfoToFile();
                    MessageBox.Show($"\"{_podcast.Title}\" podcast saved!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving Podcast to file: " + ex.Message);
                    Log.Error("Error saving Podcast to file", ex);
                }
            }
        }

        private void btnEpisodeSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                bool saved = _podcast.SaveAllEpisodesToFile();
                LoadEpisodeComboBox();

                MessageBox.Show(saved ? "All episodes saved!" : "Failed to save some/all episodes");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving the episodes: {ex.Message}");
                Log.Error("Error saving the episodes.", ex);
            }
        }

        private void btnAddNewEpisode_Click(object sender, EventArgs e)
        {
            AddNewEpisode addNewEpisode = new AddNewEpisode(_podcast, CreateFtpHelper());
            addNewEpisode.ShowDialog();
            
            LoadEpisodeComboBox(true);
        }

        private async void btnEpisodeUrlTest_Click(object sender, EventArgs e)
        {
            string url = txtEpisodeUrl.Text;
            var ftpHelper = CreateFtpHelper();
            bool exists = await ftpHelper.RemoteFileExists(url);
            MessageBox.Show(exists ? $"All good! File exists (\'{url}\')" : $"File does not exist (\'{url}\')");
        }

        private async void btnTestImageUrl_Click(object sender, EventArgs e)
        {
            string url = txtImageUrl.Text;
            var ftpHelper = CreateFtpHelper();
            bool exists = await ftpHelper.RemoteFileExists(url);
            MessageBox.Show(exists ? $"All good! File exists (\'{url}\')" : $"File does not exist (\'{url}\')");
        }

        private void btnSaveThisEpisode_Click(object sender, EventArgs e)
        {
            try
            {
                if (_podcast == null)
                {
                    Episode savedEpisode = cbEpisodeSelect.SelectedItem as Episode;
                    // Save the podcast to file first
                    btnPodcastSave_Click(sender, e);
                }

                Episode currentEpisode = cbEpisodeSelect.SelectedItem as Episode;
                if (currentEpisode == null) throw new NullReferenceException("No current episode selected.");

                bool saved = _podcast.SaveEpisodeToFile(currentEpisode);
                LoadEpisodeComboBox();

                MessageBox.Show(saved ? "Episode saved!" : "Failed to save podcast");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving the episode: {ex.Message}");
                Log.Error("Error saving episode.", ex);
            }
        }

        private void btnCreateNewPodcast_Click(object sender, EventArgs e)
        {
            // switch to podcast tab
            tabControl.SelectedTab = tabPagePodcast;
        }

        private async void btnFtpTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                int port = 21;
                if (!string.IsNullOrEmpty(txtFtpPort.Text))
                    int.TryParse(txtFtpPort.Text, out port);

                FtpHelper ftpHelper = new FtpHelper(txtFtpHost.Text, txtFtpDirectory.Text, port, txtFtpUsername.Text, txtFtpPassword.Text);
                bool valid = await ftpHelper.ConnSettingsValid();
                if (valid) MessageBox.Show("FTP info is correct.");
                else MessageBox.Show("FTP info is invalid");
            }
            catch (Exception ex)
            {
                Log.Error($"Error while testing FTP connection", ex);
            }
        }

        private FtpHelper CreateFtpHelper()
        {
            int port = 21;
            if (!string.IsNullOrEmpty(txtFtpPort.Text))
                int.TryParse(txtFtpPort.Text, out port);

            FtpHelper ftpHelper = new FtpHelper(txtFtpHost.Text, txtFtpDirectory.Text, port, txtFtpUsername.Text, txtFtpPassword.Text);
            return ftpHelper;
        }

        private void btnFtpSettingsSave_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.Default.Save();
                MessageBox.Show("Settings saved");
            }
            catch (Exception ex)
            {
                string message = $"Error while saving settings: {ex.Message}";
                MessageBox.Show(message);
                Log.Error(message, ex);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        private async void btnOpenRemotePodcastFile_Click(object sender, EventArgs e)
        {
            try
            {
                FtpHelper ftpHelper = CreateFtpHelper();
                string remoteFilename = Path.GetFileName(txtRemoteRssFileLocation.Text);
                if (string.IsNullOrEmpty(remoteFilename))
                    throw new Exception($"Coulnd't get the filename from the full URI (\"{txtRemoteRssFileLocation.Text}\")");

                string localFile = Path.Combine(_programDataFolder, remoteFilename);
                if (string.IsNullOrEmpty(txtRemoteRssFileLocation.Text))
                {
                    MessageBox.Show("Remote RSS file location is not set in the settings.");
                    return;
                }

                bool exists = await ftpHelper.RemoteFileExists(txtRemoteRssFileLocation.Text);
                if (exists)
                {
                    await ftpHelper.DownloadFile(txtRemoteRssFileLocation.Text, localFile);
                    LoadPodcastFromFile(localFile);
                }
                else
                {
                    MessageBox.Show($"\"{txtRemoteRssFileLocation.Text}\" doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                string message = $"Unable to load RSS file from \"{txtRemoteRssFileLocation.Text}\".";
                Log.Warn(message, ex);
                MessageBox.Show(message);
            }
        }

        private async void btnTestRemoteRssLink_Click(object sender, EventArgs e)
        {
            FtpHelper ftpHelper = CreateFtpHelper();
            bool exists = await ftpHelper.RemoteFileExists(txtRemoteRssFileLocation.Text);
            MessageBox.Show(exists ? "File exists" : "File does not exist");
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _podcast.SaveAllEpisodesToFile();
                _podcast.SavePodcastInfoToFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving podcast");
                Log.Error("Error saving podcast", ex);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void uploadSavedChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _podcast.SaveAllEpisodesToFile();
                _podcast.SavePodcastInfoToFile();
            }
            catch (Exception ex)
            {
                Log.Error("Error saving podcast", ex);
                MessageBox.Show("Error saving podcast");
            }

            try
            {
                FtpHelper ftpHelper = CreateFtpHelper();
                ProgressBar progressBar = new ProgressBar();
                ftpHelper.UploadFile(_podcast.Filepath, progressBar);
            }
            catch (Exception ex)
            {
                string message = $"Error uploading \"{_podcast.Filepath}\"";
                Log.Error(message, ex);
                MessageBox.Show(message);
            }
        }
    }
}
