namespace EFCoreExample
{
    using System;
    using System.Collections.Generic;

    public class Order
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastChangeDate { get; set; }

        public OrderState State { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public List<Position> Positions { get; set; }
    }
}