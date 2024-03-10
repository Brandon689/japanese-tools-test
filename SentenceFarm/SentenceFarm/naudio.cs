using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentenceFarm
{
    public class NAudioWrapper
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;
        private bool playbackFinish = true;
        public NAudioWrapper()
        {
            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        private VorbisWaveReader? vorbis = null;
        private WaveOut? oggPlayer = null;

        private void mememe(string oggfile)
        {
            vorbis = new VorbisWaveReader(oggfile);
            oggPlayer = WaveOutInit(vorbis);
            oggPlayer.Play();
        }
        private WaveOut WaveOutInit(IWaveProvider reader)
        {
            var waveOut = new WaveOut();
            waveOut.Volume = 0.15f;
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            waveOut.Init(reader);
            return waveOut;
        }

        private void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            playbackFinish = true;

        }

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            playbackFinish = true;
        }

        public void AudioFile(string soundfile)
        {
            if (playbackFinish)
            {

                mememe(soundfile);

                //audioFile = new AudioFileReader(soundfile);
                //waveOut.Init(audioFile);
                playbackFinish = false;
            }
        }

        public void Pause()
        {
            waveOut.Pause();
            Console.WriteLine("pause");
        }

        public void Stop()
        {
            if (waveOut != null)
            {
                playbackFinish = true;
                waveOut.Stop();
            }
        }

        public void Play()
        {
            Console.WriteLine(waveOut.PlaybackState);
            if (playbackFinish)
            {
                audioFile.Seek(0, SeekOrigin.Begin);
            }
            if (waveOut.PlaybackState == PlaybackState.Paused || waveOut.PlaybackState == PlaybackState.Stopped)
            {
                waveOut.Play();
            }
            else if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                Pause();
            }
        }
    }
}