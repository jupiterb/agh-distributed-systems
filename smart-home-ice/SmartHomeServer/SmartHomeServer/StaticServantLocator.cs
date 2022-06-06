using Ice;
using SmartHome;
using SmartHomeServer.Smart;

namespace SmartHomeServer
{
    public class StaticServantLocator : ServantLocator
    {
        private readonly SmartBuildingI home;

        public StaticServantLocator(SmartBuildingI smartHome)
        {
            home = smartHome;
        }

        public void deactivate(string category)
        {
            
        }

        public void finished(Current curr, Object servant, object cookie)
        {
            
        }

        public Object locate(Current curr, out object cookie)
        {
            cookie = null;
            var name = curr.id.name;

            ServerLogger.CallingTheRequest(name, home.GetName());

            if (name.Equals(home.GetName()))
            {
                ServerLogger.ServantCreated(home.GetName());
                curr.adapter.add(home, Util.stringToIdentity(name));
                return home;
            }

            var device = home.GetBy(name);
            if (device == null)
            {
                throw new ObjectNotFoundException();
            }

            if (ShouldHaveServant(device))
            {
                ServerLogger.ServantCreated(device.GetName());
                curr.adapter.add(device, Util.stringToIdentity(name));
            }
            return device;
        }

        private static bool ShouldHaveServant(Device device)
        {
            if (device is Sensor sensor)
            {
                return sensor.GetState() != SensorState.Ok;
            }

            return device is TemperatureController;
        }
    }
}
