using Ice;
using SmartHome;
using System;

namespace SmartHomeServer.Devices
{
    public class TemperatureControllerI : TemperatureControllerDisp_
    {
        protected static float baseTemperature = 20;

        private TemperatureRequest lastRequest = new();
        private float lastTemperature = baseTemperature;

        private readonly string name;
        private readonly float minTemperature;
        private readonly float maxTemperature;

        public TemperatureControllerI(string name, float minTemperature, float maxTemperature)
        {
            this.name = name;
            this.minTemperature = minTemperature;
            this.maxTemperature = maxTemperature;
            lastRequest.temperature = baseTemperature;
        }

        public override float GetTemperature(Current current = null)
        {
            var now = DateTime.Now;
            var start = lastRequest.start;
            var duration = lastRequest.duration;

            var durationInSeconds = duration.hours * 3600 + duration.minutes * 60 + duration.seconds;

            var timeFromStartInSeconds = 
                (now.Hour - start.hours) * 3600 +
                (now.Minute - start.minutes) * 60 + 
                now.Second - start.seconds;

            lastTemperature = timeFromStartInSeconds < 0
                ? lastTemperature
                : timeFromStartInSeconds < durationInSeconds
                ? lastRequest.temperature
                : baseTemperature;

            return lastTemperature;
        }

        public override void RequestTemperature(TemperatureRequest request, Current current = null)
        {
            VerifyTemperarure(request.temperature);
            VerifyTime(request.start);
            lastRequest = request;
        }

        public override string GetName(Current current = null)
        {
            return name;
        }

        public void Reset()
        {
            lastRequest = new();
        }
        private void VerifyTime(Time time)
        {
            if (time.hours >= 24 || time.minutes > 60 || time.seconds >= 60 
                || time.hours < 0 || time.minutes < 0 || time.seconds < 0)
            {
                throw new UnsuccessfulOperationException("Wrong time format. " +
                    "Hours should be between 0 and 23, " +
                    "minutes and seconds should be between 0 and 59");
            }
        }

        private void VerifyTemperarure(float temperature)
        {
            if (temperature < minTemperature || temperature > maxTemperature)
            {
                throw new UnsuccessfulOperationException($"The given temperature ({temperature}) " +
                    $"is not within the range (from {minTemperature} to {maxTemperature}) acceptable by the device");
            }
        }

        public override float GetEnergyConsumption(Current current = null)
        {
            return 0;
        }
    }
}
