using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace AudioPlayerSample
{
    public class MusicPlayer : Component
    {
        private WaveOutEvent _soundOut;
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
            Debug.WriteLine($"Open {filename}, {deviceNumber}");
            _waveSource = new AudioFileReader(filename);
            _soundOut = new WaveOutEvent();
            _soundOut.DeviceNumber = deviceNumber; // new WaveOutEvent() { DeviceNumber = 1 }; // new WasapiOut(device, AudioClientShareMode.Shared, false, 100);
            _soundOut.Init(_waveSource);
            if (PlaybackStopped != null) _soundOut.PlaybackStopped += PlaybackStopped;
        }

        public void Play()
        {
            if (_soundOut != null)
            {
                Debug.WriteLine("_soundOut.Init(_waveSource)");
                _soundOut.Init(_waveSource);
            }
            Debug.WriteLine("Play");
            _soundOut.Play();
        }

        public void Pause()
        {
            if (_soundOut != null)
            {
                Debug.WriteLine("Pause");
                _soundOut.Pause();
            }
        }

        public void Resume()
        {
            if (_soundOut != null)
            {
                Debug.WriteLine("Resume");
                _soundOut.Play();
            }
        }

        public void Stop()
        {
            if (_soundOut != null)
            {
                Debug.WriteLine("Stop");
                _soundOut.Stop();
            }
        }

        private void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                Debug.WriteLine("_soundOut.Dispose");
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                Debug.WriteLine("_waveSource.Dispose");
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