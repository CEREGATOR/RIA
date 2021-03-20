namespace EFCoreExample
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public List<Order> Orders { get; set; }
    }
}