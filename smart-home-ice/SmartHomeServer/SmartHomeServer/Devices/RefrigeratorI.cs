using Ice;
using SmartHome;
using System.Collections.Generic;

namespace SmartHomeServer.Devices
{
    public class RefrigeratorI : RefrigeratorDisp_
    {
        private static float defaultEnergyConsumption = 10;

        private readonly List<string> products = new();
        private readonly string name;

        public RefrigeratorI(string name)
        {
            this.name = name;
        }

        public override void AddProduct(string product, Current current = null)
        {
            products.Add(product);
        }

        public override float GetEnergyConsumption(Current current = null)
        {
            return defaultEnergyConsumption;
        }

        public override string GetName(Current current = null)
        {
            return name;
        }

        public override string[] GetProductList(Current current = null)
        {
            return products.ToArray();
        }

        public override void RemoveProduct(string product, Current current = null)
        {
            products.Remove(product);
        }
    }
}
