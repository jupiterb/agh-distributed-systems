using Ice;
using SmartHome;
using System;
using System.Linq;

namespace SmartHomeServer.Devices
{
    public class SolarPanelClusterI : SolarPanelClusterDisp_
    {
        private static readonly float maxEnergyPrduction = 300;
        private static readonly int sunriseHour = 7;
        private static readonly int sunsetHour = 21;

        private readonly string name;

        public SolarPanelClusterI(string name)
        {
            this.name = name;
        }

        public override float GetEnergyConsumption(Current current = null)
        {
            var hour = DateTime.Now.Hour;
            return -GetEnergyPrduction(hour);
        }

        public override string GetName(Current current = null)
        {
            return name;
        }

        public override float PredictDailyProduction(Current current = null)
        {
            return Enumerable.Range(sunriseHour, sunsetHour).Select(hour => GetEnergyPrduction(hour)).Sum();
        }

        private float GetEnergyPrduction(int hour)
        {
            if (hour >= sunriseHour && hour < sunsetHour)
            {
                var maxEnergyHour = (sunriseHour + sunsetHour) / 2;
                return Math.Abs(maxEnergyHour - hour) * maxEnergyPrduction / maxEnergyHour;
            }
            return 0;
        }
    }
}
