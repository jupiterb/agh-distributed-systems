from models import Device
from SmartHome import AirQualitySensorPrx


class AirQualitySensor(Device):
    def __init__(self, device: AirQualitySensorPrx) -> None:
        super().__init__(device)
        self._device = device

    def _get_specified_deatils(self) -> str:
        pollution = self._device.GetPollution()
        state = self._device.GetState()
        return f"pollution: {pollution}, state: {state}"
