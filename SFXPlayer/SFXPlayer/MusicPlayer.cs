using System;
using System.ComponentModel;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace AudioPlayerSample {
    public class MusicPlayer : Component {
        private ISoundOut _soundOut;
        private IWaveSource _waveSource;

        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;

        public PlaybackState PlaybackState {
            get {
                if (_soundOut != null)
                    return _soundOut.PlaybackState;
                return PlaybackState.Stopped;
            }
        }

        public TimeSpan Position {
            get {
                if (_waveSource != null)
                    return _waveSource.GetPosition();
                return TimeSpan.Zero;
            }
            set {
                if (_waveSource != null)
                    _waveSource.SetPosition(value);
                if (_soundOut != null)
                    _soundOut.Initialize(_waveSource);
            }
        }

        public TimeSpan Length {
            get {
                if (_waveSource != null)
                    return _waveSource.GetLength();
                return TimeSpan.Zero;
            }
        }

        public int Volume {
            get {
                if (_soundOut != null)
                    return Math.Min(100, Math.Max((int)(_soundOut.Volume * 100), 0));
                return 100;
            }
            set {
                if (_soundOut != null) {
                    _soundOut.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
                }
            }
        }

        public void Open(string filename, MMDevice device) {
            CleanupPlayback();

            _waveSource =
                CodecFactory.Instance.GetCodec(filename)
                    .ToSampleSource()
                    //.ToMono()
                    .ToWaveSource();
            _soundOut = new WasapiOut() { Latency = 100, Device = device };
            _soundOut.Initialize(_waveSource);
            if (PlaybackStopped != null) _soundOut.Stopped += PlaybackStopped;
        }

        public void Play() {
            if (_soundOut != null)
                _soundOut.Initialize(_waveSource);
                _soundOut.Play();
        }

        public void Pause() {
            if (_soundOut != null)
                _soundOut.Pause();
        }

        public void Resume() {
            if (_soundOut != null)
                _soundOut.Resume();
        }

        public void Stop() {
            if (_soundOut != null) {
                for (int i = 0; i < 12; i++) {          //workaround for buffer not being flushed and what's left in the buffer
                                                        //plays when you restart. This assumes first 10ms of track are silent and
                                                        //plays it 12 times (guess based on latency=100)
                    _soundOut.WaveSource.Position = 0;
                    _soundOut.WaitForStopped(10);
                }
                _soundOut.Stop();
            }
        }

        private void CleanupPlayback() {
            if (_soundOut != null) {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null) {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            CleanupPlayback();
        }
    }
}