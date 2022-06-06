from models import Device
from SmartHome import SolarPanelClusterPrx


class SolarPanelCluster(Device):
    def __init__(self, device: SolarPanelClusterPrx) -> None:
        super().__init__(device)
        self._device = device

    def _get_specified_deatils(self) -> str:
        prediction = self._device.PredictDailyProduction()
        return f"energy production prediction: {prediction}"
