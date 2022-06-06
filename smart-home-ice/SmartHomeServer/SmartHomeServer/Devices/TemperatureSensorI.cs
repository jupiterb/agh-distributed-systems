using Ice;
using SmartHome;

namespace SmartHomeServer.Devices
{
    public class TemperatureSensorI : TemperatureSensorDisp_
    {
        private static readonly float energyConsumption = 1;
        private static readonly float warningTemperature = 35;
        private static readonly float alarmTemperature = 50;

        private readonly string name;
        public float Temperature = 20;

        public TemperatureSensorI(string name)
        {
            this.name = name;
        }

        public override float GetEnergyConsumption(Current current = null)
        {
            return energyConsumption;
        }

        public override string GetName(Current current = null)
        {
            return name;
        }

        public override SensorState GetState(Current current = null)
        {
            return Temperature < warningTemperature
                ? SensorState.Ok
                : Temperature < alarmTemperature
                ? SensorState.Warning
                : SensorState.Alarm;
        }

        public override float GetTemperarure(Current current = null)
        {
            return Temperature;
        }
    }
}
