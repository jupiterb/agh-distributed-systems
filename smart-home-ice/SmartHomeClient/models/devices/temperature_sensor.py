from models import Device
from SmartHome import TemperatureSensorPrx


class TemperatureSensor(Device):
    def __init__(self, device: TemperatureSensorPrx) -> None:
        super().__init__(device)
        self._device = device

    def _get_specified_deatils(self) -> str:
        state = self._device.GetState()
        temperature = self._device.GetTemperarure()
        return f"state: {state}, temperature: {temperature}"
