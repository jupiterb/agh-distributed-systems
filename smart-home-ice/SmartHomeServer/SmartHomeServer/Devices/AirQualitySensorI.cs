using Ice;
using SmartHome;
using System;

namespace SmartHomeServer.Devices
{
    public class AirQualitySensorI : AirQualitySensorDisp_
    {
        private static readonly float energyConsumption = 1;
        private static readonly float warningPollution= 35;
        private static readonly float alarmPollution = 70;
        private static readonly int maxPollution = 100;

        private readonly string name;
        private readonly Random random = new();

        private float currantPollution;

        public AirQualitySensorI(string name)
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

        public override float GetPollution(Current current = null)
        {
            currantPollution = random.Next(maxPollution);
            return currantPollution;
        }

        public override SensorState GetState(Current current = null)
        {
            return currantPollution < warningPollution
                ? SensorState.Ok
                : currantPollution < alarmPollution
                ? SensorState.Warning
                : SensorState.Alarm;
        }
    }
}
