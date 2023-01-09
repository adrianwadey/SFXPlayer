using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace AudioPlayerSample
{
    public class MusicPlayer : Component
    {
        private WaveOut _soundOut;
        private WaveStream _waveSource;

        public event EventHandler<StoppedEventArgs> PlaybackStopped;

        public PlaybackState PlaybackState
        {
            get
            {
                if (_soundOut != null)
                    return _soundOut.PlaybackState;
                return PlaybackState.Stopped;
            }
        }

        public TimeSpan Position
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.CurrentTime;
                return TimeSpan.Zero;
            }
            set
            {
                if (_waveSource != null)
                    _waveSource.CurrentTime = value;
                if (_soundOut != null)
                    _soundOut.Init(_waveSource);
            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.TotalTime;
                return TimeSpan.Zero;
            }
        }

        public int Volume
        {
            get
            {
                if (_soundOut != null)
                    return Math.Min(100, Math.Max((int)(_soundOut.Volume * 100), 0));
                return 100;
            }
            set
            {
                if (_soundOut != null)
                {
                    _soundOut.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
                }
            }
        }

        public void Open(string filename, int deviceNumber)
        {
            CleanupPlayback();

            _waveSource = new AudioFileReader(filename);
            _soundOut = new WaveOut();
            _soundOut.DeviceNumber = deviceNumber; // new WaveOutEvent() { DeviceNumber = 1 }; // new WasapiOut(device, AudioClientShareMode.Shared, false, 100);
            _soundOut.Init(_waveSource);
            if (PlaybackStopped != null) _soundOut.PlaybackStopped += PlaybackStopped;
        }

        public void Play()
        {
            if (_soundOut != null)
                _soundOut.Init(_waveSource);
            _soundOut.Play();
        }

        public void Pause()
        {
            if (_soundOut != null)
                _soundOut.Pause();
        }

        public void Resume()
        {
            if (_soundOut != null)
                _soundOut.Play();
        }

        public void Stop()
        {
            if (_soundOut != null)
            {
                //for (int i = 0; i < 12; i++)
                //{          //workaround for buffer not being flushed and what's left in the buffer
                //           //plays when you restart. This assumes first 10ms of track are silent and
                //           //plays it 12 times (guess based on latency=100)
                //    _soundOut.WaveSource.Position = 0;
                //    _soundOut.WaitForStopped(10);
                //}
                _soundOut.Stop();
            }
        }

        private void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CleanupPlayback();
        }
    }
}