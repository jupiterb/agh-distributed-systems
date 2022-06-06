from SmartHome import SmartRoomPrx
from models import Device


class SmartRoom:

    def __init__(self, smart_room: SmartRoomPrx) -> None:
        self._room = smart_room
        self._devices: dict[str, Device] = {}

    def add_device(self, device: Device):
        self._devices[device.get_name()] = device

    def get_device_by_name(self, device_name: str) -> Device | None:
        if device_name in self._devices:
            return self._devices[device_name]
        return None

    def get_name(self) -> str:
        return self._room.GetName()

    def get_status(self) -> str:
        result = ""
        for device in self._devices.values():
            result += f'{device.get_name()}:\n{device.get_status()}\n'
        return result
    