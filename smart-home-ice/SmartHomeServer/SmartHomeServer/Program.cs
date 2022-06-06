using Ice;
using System;
using System.Linq;

namespace SmartHomeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonPath = args[0];
            args = args.Skip(1).ToArray();

            ServerConfig serverConfig;
            try
            {
                serverConfig = new ServerConfig(jsonPath);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            var smartHome = serverConfig.SmartHome;
            
            Console.WriteLine($"Starting {smartHome.GetName()} server!");

            using var communicator = Util.initialize(ref args);
            var adapter = communicator.createObjectAdapterWithEndpoints("SmartHomeAdapter", $"default -p {serverConfig.Port}");

            var homeServantLocator = new StaticServantLocator(smartHome);
            adapter.addServantLocator(homeServantLocator, "");

            foreach (var room in serverConfig.Rooms.Values)
            {
                ServerLogger.ServantCreated(room.GetName());
                adapter.add(room, Util.stringToIdentity(room.GetName()));
            }

            adapter.activate();

            communicator.waitForShutdown();
        }
    }
}
