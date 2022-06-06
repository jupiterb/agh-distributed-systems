from abc import abstractmethod
from SmartHome import DevicePrx


class Device:
    def __init__(self, device: DevicePrx) -> None:
        self._devide = device

    def get_name(self) -> str:
        return self._devide.GetName()

    def get_status(self) -> str:
        energy_consumption = self._devide.GetEnergyConsumption()
        result = f"energy consumption: {energy_consumption}\ndevice details: {self._get_specified_deatils()}\n"
        return result

    @abstractmethod
    def _get_specified_deatils(self) -> str:
        pass
