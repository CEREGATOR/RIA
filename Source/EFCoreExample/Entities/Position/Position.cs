namespace EFCoreExample
{
    using System.Collections.Generic;

    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double PriceInEuro { get; set; }

        public List<Order> Orders { get; set; }
    }
}