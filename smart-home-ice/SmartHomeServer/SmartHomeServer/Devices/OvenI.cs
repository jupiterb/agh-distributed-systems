using Ice;
using SmartHome;

namespace SmartHomeServer.Devices
{
    public class OvenI : OvenDisp_
    {
        private static readonly float energyConsumptionFactor = 1;
        private static readonly float turnOffEnergyConsumption = 1;

        private static readonly float minTemperature = 100;
        private static readonly float maxTemperature = 300;

        private OvenProgram program = OvenProgram.ConvectionBaking;
        private readonly TemperatureControllerI temperatureComponent;

        public OvenI(string name)
        {
            temperatureComponent = new(name, minTemperature, maxTemperature);
        }

        public override float GetEnergyConsumption(Current current = null)
        {
            return program == OvenProgram.TurnedOff
                ? turnOffEnergyConsumption
                : energyConsumptionFactor * temperatureComponent.GetTemperature();
        }

        public override OvenProgram GetProgram(Current current = null)
        {      
            return GetTemperature() < minTemperature ? OvenProgram.TurnedOff : program;
        }

        public override void SetProgram(OvenProgram program, Current current = null)
        {
            this.program = program;
            if (program == OvenProgram.TurnedOff)
            {
                temperatureComponent.Reset();
            }
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
