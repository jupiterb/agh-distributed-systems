from SmartHome import SmartBuildingPrx
from models import SmartRoom, Device


class SmartHome:
    def __init__(self, smart_home: SmartBuildingPrx) -> None:
        self._home = smart_home
        self._rooms: dict[str, SmartRoom] = {}
        self._unassigned_devices: dict[str, Device] = {}

    def add_device(self, device: Device):
        self._unassigned_devices[device.get_name()] = device

    def add_room(self, room: SmartRoom):
        self._rooms[room.get_name()] = room

    def get_room_by_name(self, room_name: str) -> SmartRoom | None:
        return self._rooms.get(room_name)

    def get_device_by_name(self, device_name: str) -> Device | None:
        if device_name in self._unassigned_devices:
            return self._unassigned_devices[device_name]
        else:
            for room in self._rooms.values():
                device = room.get_device_by_name(device_name)
                if device is not None:
                    return device
        return None

    def get_name(self):
        return self._home.GetName()

    def get_status(self) -> str:
        result = ""
        for device in self._unassigned_devices.values():
            result += f"{device.get_name()}:\n{device.get_status()}\n"
        for room in self._rooms.values():
            result += f"{room.get_name()}:\n\n{room.get_status()}\n"
        return result
