from models import Device
from SmartHome import OvenPrx, TemperatureRequest, Time, OvenProgram


class Oven(Device):
    def __init__(self, device: OvenPrx) -> None:
        super().__init__(device)
        self._device = device

    def request_temperature(
        self, temperature: int, start: Time, duration: Time, program: OvenProgram
    ):
        request = TemperatureRequest(
            temperature=temperature, start=start, duration=duration
        )
        self._device.RequestTemperature(request)
        self._device.SetProgram(program)

    def _get_specified_deatils(self) -> str:
        program = self._device.GetProgram()
        temperature = self._device.GetTemperature()
        return f"program: {program}, temperature: {temperature}"
