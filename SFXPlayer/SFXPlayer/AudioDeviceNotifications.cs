using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFXPlayer
{
    internal class AudioDeviceNotifications : NAudio.CoreAudioApi.Interfaces.IMMNotificationClient
    {
        public event EventHandler AudioDevicesChanged;
        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            //Debug.WriteLine($"OnDefaultDeviceChanged {defaultDeviceId}, role: {role}, flow: {flow}");
        }

        public void OnDeviceAdded(string pwstrDeviceId)
        {
            Debug.WriteLine($"OnDeviceAdded {pwstrDeviceId}");
            AudioDevicesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnDeviceRemoved(string deviceId)
        {
            Debug.WriteLine($"OnDeviceRemoved {deviceId}");
            AudioDevicesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            Debug.WriteLine($"OnDeviceStateChanged {deviceId} -> {newState}");
            AudioDevicesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
            //Debug.WriteLine($"OnPropertyValueChanged {pwstrDeviceId}, {key.propertyId}");
            //Debug.WriteLine($"");
        }
    }
}
