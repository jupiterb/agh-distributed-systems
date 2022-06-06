from cgitb import handler
import sys, Ice
from commands_handler import CommandsHandler
from devices_repository import Devicesrepository


with Ice.initialize(sys.argv) as communicator:
    print("Starting smarthome client")

    repo = Devicesrepository(communicator)
    handler_ = CommandsHandler(repo)
    handler_.print_help()

    command = input()
    while command != "exit":
        handler_.handle(command)
        command = input()
