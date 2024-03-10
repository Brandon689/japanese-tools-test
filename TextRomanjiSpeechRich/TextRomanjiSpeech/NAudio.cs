using NAudio.Wave;
using System;
using System.IO;

namespace TextRomanjiSpeech
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

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            playbackFinish = true;
        }

        public void AudioFile(string soundfile)
        {
            if (playbackFinish)
            {
                audioFile = new AudioFileReader(soundfile);
                waveOut.Init(audioFile);
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