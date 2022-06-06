using Ice;
using SmartHome;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeServer.Smart
{
    public class SmartRoomI : SmartRoomDisp_
    {
        private readonly Dictionary<string, Device> devices = new();
        private readonly string name;

        public SmartRoomI(string name)
        {
            this.name = name;
        }

        public void AddDevice(Device device)
        {
            devices.Add(device.GetName(), device);
        }

        public Device GetBy(string name)
        {
            try
            {
                return devices[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public override string[] GetDevicesNames(Current current = null)
        {
            return devices.Values.Select(device => device.GetName()).ToArray();
        }

        public override string GetName(Current current = null)
        {
            return name;
        }
    }
}
