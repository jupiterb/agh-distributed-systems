using SmartHome;
using SmartHomeServer.Devices;
using SmartHomeServer.Smart;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SmartHomeServer
{
    public class DeviceInfo
    {
        public string Name { get; set; }
        public string Room { get; set; }
        public string DeviceType { get; set; }
    }

    public class SmartHomeSet
    {
        public int Port { get; set; }
        public string SmartHomeName { get; set; }
        public string[] RoomsNames { get; set; }
        public DeviceInfo[] DeviceInfos { get; set; } 
    }

    public class ServerConfig
    {
        public int Port { get; private set; }
        public SmartBuildingI SmartHome { get; private set; }
        public Dictionary<string, SmartRoomI> Rooms { get; private set; } = new();
        public ServerConfig(string pathToJson)
        {
            string jsonString = File.ReadAllText(pathToJson);
            SmartHomeSet? smartHomeSet = JsonSerializer.Deserialize<SmartHomeSet>(jsonString);

            if (smartHomeSet != null)
            {
                InterpreteSmartHomeSet(smartHomeSet);
            }
        }

        private void InterpreteSmartHomeSet(SmartHomeSet smartHomeSet)
        {
            SmartHome = new SmartBuildingI(smartHomeSet.SmartHomeName);
            Port = smartHomeSet.Port;
            foreach (string name in smartHomeSet.RoomsNames)
            {
                var room = new SmartRoomI(name);
                Rooms.Add(name, room);
                SmartHome.AddRoom(room);
            }
            foreach (var deviceInfo in smartHomeSet.DeviceInfos)
            {
                var device = CreateDevice(deviceInfo.Name, deviceInfo.DeviceType);
                if (device != null)
                {
                    if (deviceInfo.Room != null && Rooms.ContainsKey(deviceInfo.Room))
                    {
                        Rooms[deviceInfo.Room].AddDevice(device);
                    }
                    else
                    {
                        SmartHome.AddDevice(device);
                    }
                }
            }
            VerifyDevicesNamesAreUniqal();
        }

        private void VerifyDevicesNamesAreUniqal()
        {
            var names = new List<string>(SmartHome.GetDevicesNames());
            if (names.Distinct().Count() != names.Count)
            {
                throw new System.Exception("Names of divaces should be uniqal");
            } 
        }

        private static Device CreateDevice(string name, string deviceType)
        {
            switch (deviceType)
            {
                case "AirConditioning":
                    return new AirConditioningI(name);
                case "AirQualitySensor":
                    return new AirQualitySensorI(name);
                case "Oven":
                    return new OvenI(name);
                case "Refrigerator":
                    return new RefrigeratorI(name);
                case "SolarPanelCluster":
                    return new SolarPanelClusterI(name);
                case "TemperatureSensor":
                    return new TemperatureSensorI(name);
                default:
                    return null;
            }
        }
    }
}
