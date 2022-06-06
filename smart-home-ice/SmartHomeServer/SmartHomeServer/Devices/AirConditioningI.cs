using Ice;
using SmartHome;

namespace SmartHomeServer.Devices
{
    public class AirConditioningI : AirConditioningDisp_
    {
        private static readonly float energyConsumptionFactor = 4;
        private static readonly float baseFilterQuality = 30;

        private static readonly float minTemperature = 10;
        private static readonly float maxTemperature = 25;

        private readonly TemperatureControllerI temperatureComponent;

        public AirConditioningI(string name)
        {
            temperatureComponent = new(name, minTemperature, maxTemperature);
        }

        public override float GetEnergyConsumption(Current current = null)
        {
            return energyConsumptionFactor * (maxTemperature - GetTemperature());
        }

        public override float GetFilterQuality(Current current = null)
        {
            return baseFilterQuality;
        }

        public override float GetTemperature(Current current = null)
        {
            return temperatureComponent.GetTemperature();
        }

        public override void RequestTemperature(TemperatureRequest request, Current current = null)
        {
            temperatureComponent.RequestTemperature(request);
        }

        public override string GetName(Current current = null)
        {
            return temperatureComponent.GetName();
        }
    }
}
