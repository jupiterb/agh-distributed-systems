from requests import request
from models import Device
from SmartHome import AirConditioningPrx, Time, TemperatureRequest


class AirConditioning(Device):
    def __init__(self, device: AirConditioningPrx) -> None:
        super().__init__(device)
        self._device: AirConditioningPrx = device

    def request_temperature(self, temperature: int, start: Time, duration: Time):
        request = TemperatureRequest(
            temperature=temperature, start=start, duration=duration
        )
        self._device.RequestTemperature(request)

    def _get_specified_deatils(self) -> str:
        filter_quality = self._device.GetFilterQuality()
        temperature = self._device.GetTemperature()
        return f"filter quality: {filter_quality}, temperature: {temperature}"
