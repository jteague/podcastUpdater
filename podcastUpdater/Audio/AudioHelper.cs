using System;
using System.IO;
using System.Threading.Tasks;
using log4net;
using NAudio.Lame;
using NAudio.Wave;

namespace podcastUpdater
{
    public static class AudioHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task ConvertWavToMp3(string wavFile, string mp3File)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var wav = new WaveFileReader(wavFile))
                    {
                        using (var mp3Writer = new LameMP3FileWriter(mp3File, wav.WaveFormat, LAMEPreset.ABR_128))
                        {
                            wav.CopyTo(mp3Writer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while converting to MP3 (input file: \"{wavFile}\". output file: \"{mp3File}\").", ex);
                    throw;
                }
            });
        }

        public static TimeSpan GetAudioFileDuration(string file)
        {
            AudioFileReader afReader = new AudioFileReader(file);
            return afReader.TotalTime;
        }
    }
}
