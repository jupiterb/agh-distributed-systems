from Ice import CommunicatorI
from SmartHome import (
    SmartBuildingPrx,
    SmartRoomPrx,
    AirConditioningPrx,
    AirQualitySensorPrx,
    OvenPrx,
    RefrigeratorPrx,
    SolarPanelClusterPrx,
    TemperatureSensorPrx,
)
from models import SmartHome, SmartRoom, Device, devices


class Devicesrepository:
    def __init__(self, communicator: CommunicatorI):
        self._communicator = communicator
        self._smart_homes: dict[str, SmartHome] = {}

    def add_smart_home(self, server_name: str, address: str, port: int):
        base = self._communicator.stringToProxy(f"{server_name}:{address} -p {port}")
        home_prx: SmartBuildingPrx = SmartBuildingPrx.checkedCast(base)

        if not home_prx:
            raise RuntimeError("Invalid proxy")

        smart_home = SmartHome(home_prx)
        devices_names = set(home_prx.GetDevicesNames())

        for room_name in home_prx.GetRoomsNames():
            base = self._communicator.stringToProxy(f"{room_name}:{address} -p {port}")
            room_prx: SmartRoomPrx = SmartRoomPrx.checkedCast(base)
            smart_room = SmartRoom(room_prx)
            for device_name in room_prx.GetDevicesNames():
                type = home_prx.GetTypeOfDevice(device_name)
                smart_room.add_device(
                    self._create_device(device_name, type, address, port)
                )
                devices_names.remove(device_name)
            smart_home.add_room(smart_room)

        for device_name in devices_names:
            type = home_prx.GetTypeOfDevice(device_name)
            smart_home.add_device(self._create_device(device_name, type, address, port))
        self._smart_homes[home_prx.GetName()] = smart_home

    def print_status(self):
        print("# Status of all devices:")
        for name, smart_home in self._smart_homes.items():
            print(f"{name}:\n\n{smart_home.get_status()}\n")

    def print_status_of(self, what: str):
        print(f"# Status of {what}:")
        # status of smarthome
        if what in self._smart_homes:
            print(f"{what}:\n{self._smart_homes[what].get_status()}\n")
        # status of smartroom
        for smart_home in self._smart_homes.values():
            smart_room = smart_home.get_room_by_name(what)
            if smart_room:
                print(f"{smart_home.get_name()}/{what}:\n\n{smart_room.get_status()}\n")
            # status of device
            device = smart_home.get_device_by_name(what)
            if device:
                print(f"{smart_home.get_name()}/{what}:\n\n{device.get_status()}\n")

    def get_device(self, smart_home_name: str, device_name: str) -> Device | None:
        if smart_home_name in self._smart_homes:
            return self._smart_homes[smart_home_name].get_device_by_name(device_name)
        return None

    def _create_device(self, device_name, type, address: str, port: int) -> Device:
        base = self._communicator.stringToProxy(f"{device_name}:{address} -p {port}")
        if "AirConditioning" == type:
            air_conditioning_prx = AirConditioningPrx.checkedCast(base)
            return devices.AirConditioning(air_conditioning_prx)
        if "AirQualitySensor" == type:
            air_conditioning_prx = AirQualitySensorPrx.checkedCast(base)
            return devices.AirQualitySensor(air_conditioning_prx)
        if "Oven" == type:
            air_conditioning_prx = OvenPrx.checkedCast(base)
            return devices.Oven(air_conditioning_prx)
        if "Refrigerator" == type:
            air_conditioning_prx = RefrigeratorPrx.checkedCast(base)
            return devices.Refrigerator(air_conditioning_prx)
        if "SolarPanelCluster" == type:
            air_conditioning_prx = SolarPanelClusterPrx.checkedCast(base)
            return devices.SolarPanelCluster(air_conditioning_prx)
        if "TemperatureSensor" == type:
            air_conditioning_prx = TemperatureSensorPrx.checkedCast(base)
            return devices.TemperatureSensor(air_conditioning_prx)
        raise RuntimeError(f"Device type ({type}) not found")
