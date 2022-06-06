from models import Device
from SmartHome import RefrigeratorPrx


class Refrigerator(Device):
    def __init__(self, device: RefrigeratorPrx) -> None:
        super().__init__(device)
        self._device = device

    def add_product(self, product: str):
        self._device.AddProduct(product)

    def remove_product(self, product: str):
        self._device.RemoveProduct(product)

    def _get_specified_deatils(self) -> str:
        products = self._device.GetProductList()
        result = f"list of products:"
        for product in products:
            result += f" {product}"
        return result
