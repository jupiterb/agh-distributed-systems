using Ice;
using SmartHome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeServer.Smart
{
    public class SmartBuildingI : SmartBuildingDisp_
    {
        private readonly List<SmartRoomI> smartRooms = new();
        private readonly Dictionary<string, Device> devices = new();

        private readonly string name;

        public SmartBuildingI(string name)
        {
            this.name = name;
        }
        public void AddRoom(SmartRoomI room)
        {
            smartRooms.Add(room);
        }

        public void AddDevice(Device device)
        {
            devices[device.GetName()] = device;
        }

        public Device GetBy(string name)
        {
            try
            {
                return devices[name];
            }
            catch (KeyNotFoundException)
            {
                return smartRooms
                    .Where(room => room.GetBy(name) != null)
                    .Select(room => room.GetBy(name))
                    .FirstOrDefault();
            }
        }

        public override string[] GetDevicesNames(Current current = null)
        {
            var devicesnames = devices.Values.Select(device => device.GetName());

            return smartRooms
                .Aggregate(new List<string>(devicesnames), (names, room) =>
                { 
                    names.AddRange(room.GetDevicesNames());
                    return names; 
                })
                .ToArray();
        }

        public override string[] GetRoomsNames(Current current = null)
        {
            return smartRooms.Select(room => room.GetName()).ToArray();
        }

        public override string GetName(Current current = null)
        {
            return name;
        }

        public override string GetTypeOfDevice(string deviceName, Current current = null)
        {
            var device = GetBy(deviceName);
            var className = device.GetType().Name;
            var length = className.Length;
            return className.Substring(0, length - 1);
        }
    }
}
