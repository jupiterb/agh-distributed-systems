using System;

namespace SmartHomeServer
{
    public class ServerLogger
    {
        public static void ServantCreated(string deviceName)
        {
            Console.WriteLine($"Device {deviceName} now has dedicated servant!");
        }

        public static void CallingTheRequest(string deviceName, string servant)
        {
            Console.WriteLine($"Device {deviceName} was called on servant {servant}");
        }
    }
}
