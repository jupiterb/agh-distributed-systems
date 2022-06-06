from numpy import arange
from devices_repository import Devicesrepository
from models.devices import Refrigerator, AirConditioning, Oven
from SmartHome import UnsuccessfulOperationException, Time, OvenProgram


class CommandsHandler:
    def __init__(self, devices_repository: Devicesrepository) -> None:
        self._repo = devices_repository

    def print_help(self):
        print("# Smart Home Client help:")
        print("help - print this informations")
        print("connect <home> <address> <port> - connect to smart home server")
        print("status - print status of all devices in all smart homes")
        print(
            "status <home | room | device> - print status of all smart homes, rooms and devices with given name"
        )
        print(
            "refrigerator <home> <refrigerator> <add | remove> <product> - add/remove product to/from refrigator in specified smart home"
        )
        print(
            "aircondition <home> <airconditioning> <temp> <start> <duration> - request temepreature for airconditioning in specified smart home"
        )
        print(
            "oven <home> <oven> <temp> <start> <duration> <program> - request temepreature and program for oven in specified smart home"
        )
        print("exit - close client")
        print()

    def is_exit(self, command: str) -> bool:
        return command == "exit"

    def handle(self, command: str):
        if command == "help":
            self.print_help()
        elif command.startswith("connect"):
            self._add_smart_home(command)
        elif command == "status":
            self._repo.print_status()
        elif command.startswith("status"):
            self._repo.print_status_of(command.split(" ")[1])
        elif command.startswith("refrigerator"):
            self._handle_refrigator(command)
        elif command.startswith("aircondition"):
            self._handle_aircondition(command)
        elif command.startswith("oven"):
            self._handle_oven(command)
        else:
            print("Invalid command. Type help to show available commands")

    def _add_smart_home(self, command: str):
        args = command.split(" ")
        if len(args) == 4:
            name = args[1]
            address = args[2]
            port = args[3]
            try:
                self._repo.add_smart_home(name, address, int(port))
            except:
                print(f"Unable to connect to server {name} ({address}:{port})")
        else:
            print("Invalid number od arguments")

    def _handle_refrigator(self, command: str):
        args = command.split(" ")
        if len(args) == 5:
            refrigator = self._repo.get_device(args[1], args[2])
            operation = args[3]
            product = args[4]
            if refrigator is None:
                print(f"Refrigator {args[1]}/{args[2]} dont exist.")
                return
            if isinstance(refrigator, Refrigerator):
                if operation == "add":
                    refrigator.add_product(product)
                elif operation == "remove":
                    refrigator.remove_product(product)
                else:
                    print("Invaid operation. Valid operations are; add, remove")
                    return
            else:
                print(f"Device {args[1]}/{args[2]} is not refrigator")
        else:
            print("Invalid number od arguments")

    def _handle_aircondition(self, command: str):
        args = command.split(" ")
        if len(args) == 6:
            airconditioning = self._repo.get_device(args[1], args[2])
            try:
                temperature = int(args[3])
                start_list = args[4].split(":")
                duration_list = args[5].split(":")
                start = Time(
                    hours=int(start_list[0]),
                    minutes=int(start_list[1]),
                    seconds=int(start_list[2]),
                )
                duration = Time(
                    hours=int(duration_list[0]),
                    minutes=int(duration_list[1]),
                    seconds=int(duration_list[2]),
                )
            except:
                print("Invalid arguments types.")
                return
            if airconditioning is None:
                print(f"AirConditioning {args[1]}/{args[2]} dont exist.")
                return
            if isinstance(airconditioning, AirConditioning):
                try:
                    airconditioning.request_temperature(temperature, start, duration)
                except UnsuccessfulOperationException as e:
                    print(e.reason)
            else:
                print(f"Device {args[1]}/{args[2]} is not AirConditioning")
        else:
            print("Invalid number od arguments")

    def _handle_oven(self, command: str):
        oven_programs = ["TurnedOff", "HotAir", "Grill", "ConvectionBaking"]
        args = command.split(" ")
        if len(args) == 7:
            oven = self._repo.get_device(args[1], args[2])
            try:
                temperature = int(args[3])
                start_list = args[4].split(":")
                duration_list = args[5].split(":")
                start = Time(
                    hours=int(start_list[0]),
                    minutes=int(start_list[1]),
                    seconds=int(start_list[2]),
                )
                duration = Time(
                    hours=int(duration_list[0]),
                    minutes=int(duration_list[1]),
                    seconds=int(duration_list[2]),
                )
                program = OvenProgram(args[6], oven_programs.index(args[6]))
            except:
                print("Invalid arguments types.")
                return
            if oven is None:
                print(f"Oven {args[1]}/{args[2]} dont exist.")
                return
            if isinstance(oven, Oven):
                try:
                    oven.request_temperature(temperature, start, duration, program)
                except UnsuccessfulOperationException as e:
                    print(e.reason)
            else:
                print(f"Device {args[1]}/{args[2]} is not oven")
        else:
            print("Invalid number od arguments")
