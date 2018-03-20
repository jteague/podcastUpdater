using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Microsoft.SqlServer.Server;

namespace podcastUpdater
{
    public class FtpHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string FtpHost { get; private set; }
        public string FtpDirectory { get; private set; }
        public int FtpPort { get; private set; }
        public string FtpUsername { get; private set; }
        public string FtpPassword { get; private set; }

        public FtpHelper(string host, string directory, int port, string username, string password)
        {
            FtpHost = host.StartsWith("ftp://") ? host.Trim('/') : $"ftp://{host}".Trim('/');
            FtpDirectory = directory.Trim('/');
            FtpPort = port;
            FtpUsername = username;
            FtpPassword = password;
        }

        public async Task<bool> ConnSettingsValid()
        {
            try
            {
                var uri = new Uri($"{FtpHost}/{FtpDirectory}");
                FtpWebRequest requestDir = (FtpWebRequest)WebRequest.Create(uri);
                requestDir.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
                requestDir.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                var stream = response.GetResponseStream();
                var bytes = new byte[128];
                if (stream != null && stream.CanRead)
                {
                    await stream?.ReadAsync(bytes, 0, bytes.Length);
                    return true;
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Error while checking FTP connection settings.", ex);
                return false;
            }
            return false;
        }

        public async Task<bool> RemoteFileExists(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            if (url.StartsWith("http://")) return await RemoteFileExistsHttp(url);
            if (url.StartsWith("ftp://")) return await RemoteFileExistsFtp(url);
            return false;
        }

        private async Task<bool> RemoteFileExistsHttp(string url)
        {
            try
            {
                var uri = new Uri(url);
                HttpWebRequest requestDir = (HttpWebRequest)WebRequest.Create(uri);
                requestDir.Method = WebRequestMethods.Http.Get;

                HttpWebResponse response = (HttpWebResponse)requestDir.GetResponse();
                var stream = response.GetResponseStream();
                var bytes = new byte[128];
                if (stream != null && stream.CanRead)
                {
                    await stream?.ReadAsync(bytes, 0, bytes.Length);
                    return true;
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Error while checking if HTTP file exists", ex);
                return false;
            }
            return false;
        }

        private async Task<bool> RemoteFileExistsFtp(string url)
        {
            try
            {
                var uri = new Uri(url);
                FtpWebRequest requestDir = (FtpWebRequest)WebRequest.Create(uri);
                requestDir.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
                requestDir.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                var stream = response.GetResponseStream();
                var bytes = new byte[128];
                if (stream != null && stream.CanRead)
                {
                    await stream?.ReadAsync(bytes, 0, bytes.Length);
                    return true;
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Error while checking if FTP file exists", ex);
                return false;
            }
            return false;
        }

        private ProgressBar _progBar;
        public bool UploadFile(string localFilePath, ProgressBar progBar)
        {
            WebClient client = new WebClient();
            
            try
            {
                if (string.IsNullOrEmpty(localFilePath) || !File.Exists(localFilePath))
                {
                    var message = $"Cannot upload: The file \"{localFilePath}\" does not exist.";
                    MessageBox.Show(message);
                    Log.Error(message);
                    return false;
                }

                var result = MessageBox.Show("Upload file?", "Upload file to FTP server?",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _progBar = progBar;
                    client.UploadFileCompleted += ClientOnUploadFileCompleted;
                    client.UploadProgressChanged += ClientOnUploadProgressChanged;
                    client.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
                    string ftpLocation = new Uri($"{FtpHost}/{FtpDirectory}/{Path.GetFileName(localFilePath)}").ToString();
                    client.UploadFileAsync(new Uri(ftpLocation), "STOR", localFilePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error while uploading audio file \"{localFilePath}\".", ex);
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }

        private void ClientOnUploadProgressChanged(object sender, UploadProgressChangedEventArgs args)
        {
            _progBar.Value = args.ProgressPercentage;
        }

        private void ClientOnUploadFileCompleted(object sender, UploadFileCompletedEventArgs args)
        {
            MessageBox.Show($"File uploaded!");
        }

        public async Task<bool> DownloadFile(string remoteUri, string localFile)
        {
            if(string.IsNullOrEmpty(remoteUri) || string.IsNullOrEmpty(localFile))
                throw new ArgumentNullException();

            WebClient client = new WebClient();

            try
            {
                client.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
                await client.DownloadFileTaskAsync(new Uri(remoteUri), localFile);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error while downloading file \"{remoteUri}\" to \"{localFile}\".", ex);
                throw;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
