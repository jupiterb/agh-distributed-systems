module SmartHome {

    sequence<string> List;

    struct Time {
        int hours;
        int minutes;
        int seconds;
    }

    struct TemperatureRequest {
        float temperature;
        Time start;
        Time duration;
    }

    enum OvenProgram {
        TurnedOff,
        HotAir,
        Grill,
        ConvectionBaking
    }

    enum SensorState {
        Ok,
        Warning,
        Alarm
    }

    exception UnsuccessfulOperationException {
        string reason;
    }

    interface SmartRoom {
        idempotent string GetName();
        idempotent List GetDevicesNames();
    }

    interface SmartBuilding extends SmartRoom {
        idempotent List GetRoomsNames();
        idempotent string GetTypeOfDevice(string deviceName);
    }

    interface Device {
        idempotent string GetName();
        float GetEnergyConsumption();
    }

    interface TemperatureController extends Device {
        void RequestTemperature(TemperatureRequest request) throws UnsuccessfulOperationException;
        float GetTemperature();
    }

    interface Oven extends TemperatureController {
        void SetProgram(OvenProgram program);
        OvenProgram GetProgram();
    }

    interface AirConditioning extends TemperatureController {
        float GetFilterQuality();
    }

    interface SolarPanelCluster extends Device {
        float PredictDailyProduction();
    }

    interface Refrigerator extends Device {
        List GetProductList();
        void AddProduct(string product);
        void RemoveProduct(string product);
    }

    interface Sensor extends Device {
        SensorState GetState();
    }

    interface TemperatureSensor extends Sensor {
        float GetTemperarure();
    }

    interface AirQualitySensor extends Sensor {
        float GetPollution();
    }
}